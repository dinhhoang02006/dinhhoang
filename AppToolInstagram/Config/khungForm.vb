' khungForm.vb
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Module khungForm
    '============== Cấu hình ==============
    ' Màu highlight khi hover/kéo, hoặc màu luôn hiển thị nếu chọn AlwaysHighlighted
    Public DragBarDownColor As Color = Color.FromArgb(236, 239, 243) ' #ECEFF3

    Public Enum DragBarMode
        HighlightOnDrag      ' ✅ gồm cả hover: mouse enter = highlight; mouse leave = trả màu
        AlwaysHighlighted    ' luôn highlight
    End Enum

    ' Trạng thái theo control
    Private ReadOnly _origColors As New Dictionary(Of Control, Color)
    Private ReadOnly _modes As New Dictionary(Of Control, DragBarMode)
    Private ReadOnly _wired As New HashSet(Of Control)

    '============== WinAPI drag ==============
    <DllImport("user32.dll", EntryPoint:="ReleaseCapture")>
    Private Sub ReleaseCapture()
    End Sub

    <DllImport("user32.dll", EntryPoint:="SendMessage")>
    Private Sub SendMessage(hWnd As IntPtr, wMsg As Integer, wParam As Integer, lParam As Integer)
    End Sub

    Private Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_DRAGMOVE As Integer = &HF012

    ' Cho borderless minimize từ taskbar
    Private Const GWL_STYLE As Integer = -16
    Private Const WS_SYSMENU As Integer = &H80000
    Private Const WS_MINIMIZEBOX As Integer = &H20000

    <DllImport("user32.dll", EntryPoint:="GetWindowLong")>
    Private Function GetWindowLong32(hWnd As IntPtr, nIndex As Integer) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowLong")>
    Private Function SetWindowLong32(hWnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowLongPtr")>
    Private Function GetWindowLongPtr64(hWnd As IntPtr, nIndex As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowLongPtr")>
    Private Function SetWindowLongPtr64(hWnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr,
                                  X As Integer, Y As Integer, cx As Integer, cy As Integer,
                                  uFlags As UInteger) As Boolean
    End Function

    Private Const SWP_NOSIZE As UInteger = &H1UI
    Private Const SWP_NOMOVE As UInteger = &H2UI
    Private Const SWP_NOZORDER As UInteger = &H4UI
    Private Const SWP_FRAMECHANGED As UInteger = &H20UI

    Private Function GetWndStyle(hWnd As IntPtr) As IntPtr
        If IntPtr.Size = 8 Then
            Return GetWindowLongPtr64(hWnd, GWL_STYLE)
        Else
            Return New IntPtr(GetWindowLong32(hWnd, GWL_STYLE))
        End If
    End Function

    Private Sub SetWndStyle(hWnd As IntPtr, style As IntPtr)
        If IntPtr.Size = 8 Then
            SetWindowLongPtr64(hWnd, GWL_STYLE, style)
        Else
            SetWindowLong32(hWnd, GWL_STYLE, CInt(style.ToInt64()))
        End If
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
                     SWP_NOMOVE Or SWP_NOSIZE Or SWP_NOZORDER Or SWP_FRAMECHANGED)
    End Sub

    '============== API dùng trong Form ==============

    Public Sub MakeBorderless(frm As Form, Optional paddingSize As Integer = 0)
        frm.FormBorderStyle = FormBorderStyle.None
        If paddingSize > 0 Then frm.Padding = New Padding(paddingSize)
        frm.ShowInTaskbar = True
    End Sub

    Public Sub EnableMinimizeFromTaskbar(frm As Form)
        Dim h = frm.Handle
        frm.MinimizeBox = True
        Dim style = GetWndStyle(h).ToInt64()
        style = style Or WS_SYSMENU Or WS_MINIMIZEBOX
        SetWndStyle(h, New IntPtr(style))
    End Sub

    ' Bật kéo bằng control (panel titlebar) + chọn chế độ
    Public Sub EnableDragBy(frm As Form, dragArea As Control,
                            Optional mode As DragBarMode = DragBarMode.HighlightOnDrag)
        If dragArea Is Nothing Then Return

        If Not _origColors.ContainsKey(dragArea) Then
            _origColors(dragArea) = GetAreaColor(dragArea)
        End If
        _modes(dragArea) = mode

        ' Set màu ban đầu theo chế độ
        If mode = DragBarMode.AlwaysHighlighted Then
            SetAreaColor(dragArea, DragBarDownColor)
        Else
            ' HighlightOnDrag: để màu gốc; hover sẽ tô
            SetAreaColor(dragArea, _origColors(dragArea))
        End If

        If _wired.Contains(dragArea) Then Return

        ' Hover vào: nếu HighlightOnDrag -> highlight
        AddHandler dragArea.MouseEnter,
            Sub(sender As Object, e As EventArgs)
                If GetMode(dragArea) = DragBarMode.HighlightOnDrag Then
                    SetAreaColor(dragArea, DragBarDownColor)
                End If
            End Sub

        ' Rời chuột: nếu HighlightOnDrag -> trả màu gốc
        AddHandler dragArea.MouseLeave,
            Sub(sender As Object, e As EventArgs)
                If GetMode(dragArea) = DragBarMode.HighlightOnDrag AndAlso
                   Control.MouseButtons = MouseButtons.None Then
                    RestoreOriginalColor(dragArea)
                End If
            End Sub

        ' Kéo (nhấn trái): vẫn drag; đảm bảo đang highlight
        AddHandler dragArea.MouseDown,
            Sub(sender As Object, e As MouseEventArgs)
                If e.Button = MouseButtons.Left Then
                    If GetMode(dragArea) = DragBarMode.HighlightOnDrag Then
                        SetAreaColor(dragArea, DragBarDownColor)
                    End If
                    ReleaseCapture()
                    SendMessage(frm.Handle, WM_SYSCOMMAND, SC_DRAGMOVE, 0)
                End If
            End Sub

        ' Nhả chuột: KHÔNG trả màu ở đây để còn giữ highlight nếu vẫn đang hover
        ' (MouseLeave sẽ lo trả màu khi rời khỏi titlebar)

        ' Cleanup khi control bị hủy
        AddHandler dragArea.Disposed,
            Sub()
                _wired.Remove(dragArea)
                _modes.Remove(dragArea)
                _origColors.Remove(dragArea)
            End Sub

        _wired.Add(dragArea)
    End Sub

    ' Đổi chế độ runtime
    Public Sub SetDragBarHighlightMode(dragArea As Control, mode As DragBarMode)
        If dragArea Is Nothing Then Return
        If Not _origColors.ContainsKey(dragArea) Then
            _origColors(dragArea) = GetAreaColor(dragArea)
        End If
        _modes(dragArea) = mode

        If mode = DragBarMode.AlwaysHighlighted Then
            SetAreaColor(dragArea, DragBarDownColor)
        Else
            ' HighlightOnDrag: nếu hiện tại chuột đang nằm trên, giữ highlight; nếu không, trả về gốc
            If IsMouseOver(dragArea) Then
                SetAreaColor(dragArea, DragBarDownColor)
            Else
                RestoreOriginalColor(dragArea)
            End If
        End If
    End Sub

    ' Đổi màu highlight runtime
    Public Sub SetDragBarHighlightColor(col As Color)
        DragBarDownColor = col
        For Each kv In _modes
            If kv.Value = DragBarMode.AlwaysHighlighted Then
                SetAreaColor(kv.Key, DragBarDownColor)
            ElseIf kv.Value = DragBarMode.HighlightOnDrag AndAlso IsMouseOver(kv.Key) Then
                ' đang hover thì cập nhật ngay
                SetAreaColor(kv.Key, DragBarDownColor)
            End If
        Next
    End Sub

    '============== Helpers ==============
    Private Sub RestoreOriginalColor(area As Control)
        If _origColors.ContainsKey(area) Then
            SetAreaColor(area, _origColors(area))
        End If
    End Sub

    Private Function GetMode(area As Control) As DragBarMode
        If _modes.ContainsKey(area) Then Return _modes(area)
        Return DragBarMode.HighlightOnDrag
    End Function

    Private Function IsMouseOver(c As Control) As Boolean
        If c Is Nothing OrElse c.IsDisposed Then Return False
        Dim screenRect = c.RectangleToScreen(c.ClientRectangle)
        Return screenRect.Contains(Cursor.Position)
    End Function

    Private Function GetAreaColor(area As Control) As Color
        If TypeOf area Is Guna.UI2.WinForms.Guna2Panel Then
            Return DirectCast(area, Guna.UI2.WinForms.Guna2Panel).FillColor
        Else
            Return area.BackColor
        End If
    End Function

    Private Sub SetAreaColor(area As Control, col As Color)
        If TypeOf area Is Guna.UI2.WinForms.Guna2Panel Then
            DirectCast(area, Guna.UI2.WinForms.Guna2Panel).FillColor = col
        Else
            area.BackColor = col
        End If
    End Sub
End Module
