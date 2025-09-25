'====================== SystemOverlayScrollbars.vb ======================
Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Text

Public Module handleScrollBar

#Region "Win32"
    <DllImport("user32.dll")>
    Private Function ShowScrollBar(hWnd As IntPtr, wBar As Integer, bShow As Boolean) As Boolean
    End Function
    Private Const SB_BOTH As Integer = 3

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function
    Private Const EM_GETLINECOUNT As Integer = &HBA
    Private Const EM_GETFIRSTVISIBLELINE As Integer = &HCE
    Private Const EM_LINESCROLL As Integer = &HB6

    <DllImport("user32.dll")>
    Private Function WindowFromPoint(pt As Point) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Function GetClassName(hWnd As IntPtr, lpClassName As StringBuilder, nMaxCount As Integer) As Integer
    End Function

    Private Delegate Function EnumWindowsProc(hWnd As IntPtr, lParam As IntPtr) As Boolean
    <DllImport("user32.dll")>
    Private Function EnumChildWindows(hWndParent As IntPtr, lpEnumFunc As EnumWindowsProc, lParam As IntPtr) As Boolean
    End Function

    Private Const WM_MOUSEWHEEL As Integer = &H20A
    Private Const WM_VSCROLL As Integer = &H115
    Private Const WM_MOUSEHWHEEL As Integer = &H20E
    Private Const WM_HSCROLL As Integer = &H114
#End Region

#Region "State"
    Private ReadOnly _items As New Dictionary(Of Control, OverlayItem)
    Private _filter As ScrollMessageFilter
#End Region

#Region "Public API"
    Public Sub Attach(target As Control,
                      Optional hideDelayMs As Integer = 1500,
                      Optional overlayWidth As Integer = 17,
                      Optional hideSystemScrollbar As Boolean = True)
        If target Is Nothing OrElse target.IsDisposed Then Return

        If _items.ContainsKey(target) Then
            _items(target).HideDelayMs = hideDelayMs
            _items(target).OverlayWidth = overlayWidth
            _items(target).Reposition()
            Return
        End If

        If hideSystemScrollbar Then
            Try : ShowScrollBar(target.Handle, SB_BOTH, False) : Catch : End Try
        End If

        Dim it As New OverlayItem(target, hideDelayMs, overlayWidth)
        _items(target) = it
        it.Attach()

        If _filter Is Nothing Then
            _filter = New ScrollMessageFilter(AddressOf FindItemByHandle)
            Application.AddMessageFilter(_filter)
        End If
    End Sub

    Public Sub Detach(target As Control)
        If target Is Nothing Then Return
        Dim it As OverlayItem = Nothing
        If _items.TryGetValue(target, it) Then
            it.Detach()
            it.Dispose()
            _items.Remove(target)
        End If
        If _items.Count = 0 AndAlso _filter IsNot Nothing Then
            Application.RemoveMessageFilter(_filter)
            _filter = Nothing
        End If
    End Sub
#End Region

#Region "Map HWND -> OverlayItem"
    Private Function FindItemByCursor() As OverlayItem
        Dim h As IntPtr = WindowFromPoint(Control.MousePosition)
        Return FindItemByHandle(h)
    End Function

    Private Function FindItemByHandle(hWnd As IntPtr) As OverlayItem
        If _items.Count = 0 OrElse hWnd = IntPtr.Zero Then Return Nothing
        Dim c As Control = Nothing
        Try : c = Control.FromChildHandle(hWnd) : Catch : c = Nothing : End Try
        If c Is Nothing Then Return Nothing

        Dim f As Form = TryCast(c, Form)
        If f IsNot Nothing Then
            Dim itFromTag As OverlayItem = TryCast(f.Tag, OverlayItem)
            If itFromTag IsNot Nothing Then Return itFromTag
        End If

        Dim p As Control = c
        While p IsNot Nothing
            Dim it As OverlayItem = Nothing
            If _items.TryGetValue(p, it) Then Return it
            p = p.Parent
        End While
        Return Nothing
    End Function
#End Region

#Region "Message filter"
    Private Class ScrollMessageFilter
        Implements IMessageFilter
        Private ReadOnly _find As Func(Of IntPtr, OverlayItem)
        Public Sub New(find As Func(Of IntPtr, OverlayItem))
            _find = find
        End Sub

        Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
            Dim id As Integer = m.Msg
            If id = WM_MOUSEWHEEL OrElse id = WM_VSCROLL OrElse id = WM_MOUSEHWHEEL OrElse id = WM_HSCROLL Then
                Dim it As OverlayItem = FindItemByCursor()
                If it Is Nothing Then it = _find(m.HWnd)
                If it Is Nothing Then Return False

                If id = WM_MOUSEWHEEL Then
                    Dim delta As Integer = GetWheelDelta(m.WParam)
                    it.HandleMouseWheel(delta)
                    it.ShowOverlay()
                    it.ScheduleHide()
                    m.Result = IntPtr.Zero
                    Return True
                Else
                    it.SyncFromControl()
                    it.ShowOverlay()
                    it.ScheduleHide()
                    Return False
                End If
            End If
            Return False
        End Function

        Private Function GetWheelDelta(wParam As IntPtr) As Integer
            Dim hi As Integer = CInt((CLng(wParam) >> 16) And &HFFFF)
            If hi >= &H8000 Then hi -= &H10000
            Return hi
        End Function
    End Class
#End Region

#Region "Core"
    Private Class OverlayItem
        Implements IDisposable

        Public ReadOnly Target As Control
        Public HideDelayMs As Integer
        Public OverlayWidth As Integer

        Private ReadOnly _overlay As OverlayForm
        Private ReadOnly _hideTimer As Timer
        Private ReadOnly _fadeTimer As Timer
        Private _suppress As Boolean = False

        Public Sub New(target As Control, hideDelay As Integer, width As Integer)
            Me.Target = target
            Me.HideDelayMs = hideDelay
            Me.OverlayWidth = Math.Max(12, width)

            _overlay = New OverlayForm(Me)
            _overlay.Opacity = 0.0R
            _overlay.Visible = False

            _hideTimer = New Timer() With {.Interval = Math.Max(300, hideDelay)}
            AddHandler _hideTimer.Tick, AddressOf OnHideTick

            _fadeTimer = New Timer() With {.Interval = 30}
            AddHandler _fadeTimer.Tick, AddressOf OnFadeTick
        End Sub

        Public Sub Attach()
            AddHandler Target.LocationChanged, AddressOf OnTargetLayoutChanged
            AddHandler Target.SizeChanged, AddressOf OnTargetLayoutChanged
            AddHandler Target.VisibleChanged, AddressOf OnTargetVisibleChanged
            AddHandler Target.ParentChanged, AddressOf OnTargetLayoutChanged
            AddHandler Target.Disposed, AddressOf OnTargetDisposed

            If TypeOf Target Is DataGridView Then
                AddHandler DirectCast(Target, DataGridView).Scroll, AddressOf OnDgvScroll
            End If

            Dim frm = Target.FindForm()
            If frm IsNot Nothing Then
                AddHandler frm.Move, AddressOf OnTargetLayoutChanged
                AddHandler frm.SizeChanged, AddressOf OnTargetLayoutChanged
            End If

            Reposition()
            SyncFromControl()
        End Sub

        Public Sub Detach()
            Try
                RemoveHandler Target.LocationChanged, AddressOf OnTargetLayoutChanged
                RemoveHandler Target.SizeChanged, AddressOf OnTargetLayoutChanged
                RemoveHandler Target.VisibleChanged, AddressOf OnTargetVisibleChanged
                RemoveHandler Target.ParentChanged, AddressOf OnTargetLayoutChanged
                RemoveHandler Target.Disposed, AddressOf OnTargetDisposed

                If TypeOf Target Is DataGridView Then
                    RemoveHandler DirectCast(Target, DataGridView).Scroll, AddressOf OnDgvScroll
                End If

                Dim frm As Form = Target.FindForm()
                If frm IsNot Nothing Then
                    RemoveHandler frm.Move, AddressOf OnTargetLayoutChanged
                    RemoveHandler frm.SizeChanged, AddressOf OnTargetLayoutChanged
                End If
            Catch
            End Try
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Try
                _hideTimer.Stop()
                _fadeTimer.Stop()
                If _overlay IsNot Nothing AndAlso Not _overlay.IsDisposed Then
                    _overlay.Close()
                    _overlay.Dispose()
                End If
            Catch
            End Try
        End Sub

        ' ---------- Native EDIT handle finder (for Guna2TextBox etc.) ----------
        Private Function GetNativeEditHwnd(ctrl As Control) As IntPtr
            If ctrl Is Nothing OrElse ctrl.IsDisposed Then Return IntPtr.Zero

            ' 1) Chính nó là EDIT/RICHEDIT?
            Dim sb As New StringBuilder(128)
            Try
                Dim n = GetClassName(ctrl.Handle, sb, sb.Capacity)
                If n > 0 Then
                    Dim cls = sb.ToString(0, n)
                    If cls.IndexOf("EDIT", StringComparison.OrdinalIgnoreCase) >= 0 _
                        OrElse cls.IndexOf("RICHEDIT", StringComparison.OrdinalIgnoreCase) >= 0 Then
                        Return ctrl.Handle
                    End If
                End If
            Catch
            End Try

            ' 2) Tìm trong child windows
            Dim found As IntPtr = IntPtr.Zero
            Dim stopFlag As Boolean = False
            Dim cb As EnumWindowsProc =
                Function(h, l)
                    If stopFlag Then Return False
                    Dim s As New StringBuilder(128)
                    Try
                        Dim nn = GetClassName(h, s, s.Capacity)
                        If nn > 0 Then
                            Dim c = s.ToString(0, nn)
                            If c.IndexOf("EDIT", StringComparison.OrdinalIgnoreCase) >= 0 _
                               OrElse c.IndexOf("RICHEDIT", StringComparison.OrdinalIgnoreCase) >= 0 Then
                                found = h
                                stopFlag = True
                                Return False
                            End If
                        End If
                    Catch
                    End Try
                    Return True
                End Function
            Try : EnumChildWindows(ctrl.Handle, cb, IntPtr.Zero) : Catch : End Try
            Return found
        End Function

        ' ------------------ Overlay control flow ------------------
        Public Sub ShowOverlay()
            _hideTimer.Stop()
            _fadeTimer.Stop()
            _overlay.SetOwner(Target.FindForm())
            If Not _overlay.Visible Then _overlay.Show()
            _overlay.Opacity = 1.0R
        End Sub

        Public Sub ScheduleHide()
            _hideTimer.Stop()
            _hideTimer.Interval = Math.Max(300, HideDelayMs)
            _hideTimer.Start()
        End Sub

        Private Sub OnHideTick(sender As Object, e As EventArgs)
            _hideTimer.Stop()
            _fadeTimer.Start()
        End Sub

        Private Sub OnFadeTick(sender As Object, e As EventArgs)
            If _overlay.IsDisposed OrElse Not _overlay.Visible Then
                _fadeTimer.Stop()
                Return
            End If
            Dim op As Double = _overlay.Opacity - 0.14R
            If op <= 0.0R Then
                _overlay.Opacity = 0.0R
                _fadeTimer.Stop()
                _overlay.Hide()
            Else
                _overlay.Opacity = op
            End If
        End Sub

        ' -------------------- Sync & Mapping ---------------------
        Public Sub SyncFromControl()
            _suppress = True
            Try
                Dim vsb = _overlay.VSB
                vsb.Minimum = 0
                vsb.SmallChange = 1

                If TypeOf Target Is DataGridView Then
                    Dim dgv = DirectCast(Target, DataGridView)
                    Dim total As Integer = dgv.Rows.Count
                    Dim visible As Integer = Math.Max(1, dgv.DisplayedRowCount(False))
                    Dim first As Integer = 0
                    Try : first = Math.Max(0, dgv.FirstDisplayedScrollingRowIndex) : Catch : first = 0 : End Try
                    Dim maxFirst As Integer = Math.Max(0, total - visible)
                    vsb.LargeChange = visible
                    vsb.Maximum = If(maxFirst <= 0, 0, maxFirst + vsb.LargeChange - 1)
                    vsb.Value = Math.Min(Math.Max(0, first), Math.Max(0, vsb.Maximum - vsb.LargeChange + 1))

                ElseIf TypeOf Target Is RichTextBox Then
                    Dim rtb = DirectCast(Target, RichTextBox)
                    Dim totalLines As Integer = CInt(SendMessage(rtb.Handle, EM_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero))
                    Dim visibleLines As Integer = Math.Max(1, rtb.ClientSize.Height \ Math.Max(1, rtb.Font.Height))
                    Dim first As Integer = CInt(SendMessage(rtb.Handle, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero))
                    Dim maxFirst As Integer = Math.Max(0, totalLines - visibleLines)
                    vsb.LargeChange = visibleLines
                    vsb.Maximum = If(maxFirst <= 0, 0, maxFirst + vsb.LargeChange - 1)
                    vsb.Value = Math.Min(Math.Max(0, first), Math.Max(0, vsb.Maximum - vsb.LargeChange + 1))

                Else
                    ' EDIT control (TextBox chuẩn, Guna2TextBox…)
                    Dim editHwnd = GetNativeEditHwnd(Target)
                    If editHwnd <> IntPtr.Zero Then
                        Dim totalLines As Integer = CInt(SendMessage(editHwnd, EM_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero))
                        If totalLines <= 0 Then totalLines = 1
                        Dim lineH As Integer = Math.Max(1, Target.Font.Height)
                        Dim visibleLines As Integer = Math.Max(1, Target.ClientSize.Height \ lineH)
                        Dim first As Integer = CInt(SendMessage(editHwnd, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero))
                        If first < 0 Then first = 0
                        Dim maxFirst As Integer = Math.Max(0, totalLines - visibleLines)

                        vsb.LargeChange = visibleLines
                        vsb.Maximum = If(maxFirst <= 0, 0, maxFirst + vsb.LargeChange - 1)
                        vsb.Value = Math.Min(Math.Max(0, first), Math.Max(0, vsb.Maximum - vsb.LargeChange + 1))
                    End If
                End If
            Finally
                _suppress = False
            End Try

            Reposition()
        End Sub

        Private Sub ApplyFromScrollbar()
            If _suppress Then Return
            Dim vsb = _overlay.VSB
            Dim targetIndex As Integer = vsb.Value

            If TypeOf Target Is DataGridView Then
                Dim dgv = DirectCast(Target, DataGridView)
                Dim total As Integer = dgv.Rows.Count
                If total > 0 Then
                    targetIndex = Math.Min(targetIndex, Math.Max(0, total - 1))
                    Try : dgv.FirstDisplayedScrollingRowIndex = Math.Max(0, targetIndex) : Catch : End Try
                End If

            ElseIf TypeOf Target Is RichTextBox Then
                Dim rtb = DirectCast(Target, RichTextBox)
                Dim curFirst As Integer = CInt(SendMessage(rtb.Handle, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero))
                Dim delta As Integer = targetIndex - curFirst
                If delta <> 0 Then SendMessage(rtb.Handle, EM_LINESCROLL, IntPtr.Zero, CType(delta, IntPtr))

            Else
                Dim editHwnd = GetNativeEditHwnd(Target)
                If editHwnd <> IntPtr.Zero Then
                    Dim curFirst As Integer = CInt(SendMessage(editHwnd, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero))
                    Dim delta As Integer = targetIndex - curFirst
                    If delta <> 0 Then SendMessage(editHwnd, EM_LINESCROLL, IntPtr.Zero, CType(delta, IntPtr))
                End If
            End If
        End Sub

        Public Sub HandleMouseWheel(delta As Integer)
            Dim ticks As Integer
            If delta > 0 Then
                ticks = (delta + 60) \ 120
            ElseIf delta < 0 Then
                ticks = (delta - 60) \ 120
            Else
                ticks = 0
            End If
            If ticks = 0 Then Return

            Const linesPerNotch As Integer = 3

            If TypeOf Target Is DataGridView Then
                Dim dgv = DirectCast(Target, DataGridView)
                Dim first As Integer
                Try : first = Math.Max(0, dgv.FirstDisplayedScrollingRowIndex) : Catch : first = 0 : End Try
                Dim newFirst As Integer = first - ticks * linesPerNotch
                newFirst = Math.Max(0, Math.Min(newFirst, Math.Max(0, dgv.Rows.Count - 1)))
                Try : dgv.FirstDisplayedScrollingRowIndex = newFirst : Catch : End Try

            ElseIf TypeOf Target Is RichTextBox Then
                Dim rtb = DirectCast(Target, RichTextBox)
                Dim moveLines As Integer = -ticks * linesPerNotch
                If moveLines <> 0 Then SendMessage(rtb.Handle, EM_LINESCROLL, IntPtr.Zero, CType(moveLines, IntPtr))

            Else
                Dim editHwnd = GetNativeEditHwnd(Target)
                If editHwnd <> IntPtr.Zero Then
                    Dim moveLines As Integer = -ticks * linesPerNotch
                    If moveLines <> 0 Then SendMessage(editHwnd, EM_LINESCROLL, IntPtr.Zero, CType(moveLines, IntPtr))
                End If
            End If

            SyncFromControl()
            ShowOverlay()
            ScheduleHide()
        End Sub

        ' --------------------- Layout & Events --------------------
        Public Sub Reposition()
            If _overlay.IsDisposed Then Return
            Dim frm As Form = Target.FindForm()
            If frm Is Nothing Then Return

            Dim r = Target.RectangleToScreen(New Rectangle(Point.Empty, Target.ClientSize))
            Dim w = OverlayWidth
            _overlay.Bounds = New Rectangle(r.Right - w, r.Top, w, r.Height)
            _overlay.SetOwner(frm)
            _overlay.BringToFront()
        End Sub

        Private Sub OnTargetLayoutChanged(sender As Object, e As EventArgs)
            Reposition()
        End Sub

        Private Sub OnTargetVisibleChanged(sender As Object, e As EventArgs)
            If Not Target.Visible Then _overlay.Hide() Else Reposition()
        End Sub

        Private Sub OnTargetDisposed(sender As Object, e As EventArgs)
            Detach()
            Dispose()
        End Sub

        Private Sub OnDgvScroll(sender As Object, e As ScrollEventArgs)
            SyncFromControl()
            ShowOverlay()
            ScheduleHide()
        End Sub

        ' --------------------- Inner overlay form -----------------
        Private Class OverlayForm
            Inherits Form
            Private ReadOnly _ownerItem As OverlayItem
            Public ReadOnly VSB As VScrollBar

            Public Sub New(owner As OverlayItem)
                _ownerItem = owner
                Me.FormBorderStyle = FormBorderStyle.None
                Me.ShowInTaskbar = False
                Me.StartPosition = FormStartPosition.Manual
                Me.TopMost = False
                Me.DoubleBuffered = True
                Me.Opacity = 0.0R
                Me.BackColor = SystemColors.Control
                Me.Tag = owner

                VSB = New VScrollBar() With {
                    .Dock = DockStyle.Fill,
                    .Minimum = 0,
                    .Maximum = 0,
                    .LargeChange = 1,
                    .SmallChange = 1
                }
                AddHandler VSB.Scroll, AddressOf OnVsbScroll
                AddHandler VSB.ValueChanged, AddressOf OnVsbValueChanged
                Me.Controls.Add(VSB)
            End Sub

            Private Sub OnVsbScroll(sender As Object, e As ScrollEventArgs)
                _ownerItem.ApplyFromScrollbar()
            End Sub

            Private Sub OnVsbValueChanged(sender As Object, e As EventArgs)
                _ownerItem.ApplyFromScrollbar()
            End Sub

            Protected Overrides ReadOnly Property CreateParams As CreateParams
                Get
                    Const WS_EX_TOOLWINDOW As Integer = &H80
                    Const WS_EX_NOACTIVATE As Integer = &H8000000
                    Dim cp = MyBase.CreateParams
                    cp.ExStyle = cp.ExStyle Or WS_EX_TOOLWINDOW Or WS_EX_NOACTIVATE
                    Return cp
                End Get
            End Property

            Protected Overrides Sub WndProc(ByRef m As Message)
                If m.Msg = WM_MOUSEWHEEL Then
                    Dim delta As Integer = CInt((CLng(m.WParam) >> 16) And &HFFFF)
                    If delta >= &H8000 Then delta -= &H10000
                    _ownerItem.HandleMouseWheel(delta)
                    _ownerItem.ShowOverlay()
                    _ownerItem.ScheduleHide()
                    m.Result = IntPtr.Zero
                    Return
                End If
                MyBase.WndProc(m)
            End Sub

            Public Sub SetOwner(ownerForm As Form)
                If ownerForm Is Nothing Then Return
                If Me.Owner Is ownerForm Then Return
                Try : Me.Owner = ownerForm : Catch : End Try
            End Sub
        End Class
    End Class
#End Region

End Module
'================== /SystemOverlayScrollbars.vb ======================
