Imports System.Runtime.InteropServices

Public Class CloseDialog

    Public Sub New()
        InitializeComponent()

        Me.FormBorderStyle = FormBorderStyle.None
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.White

        doBongVienHopThoai.ContainerControl = Me
        doBongVienHopThoai.BorderRadius = 12
    End Sub

    ' --- Sự kiện nút ---
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Yes   ' Trả về Yes cho form cha
        Me.Close()
    End Sub

    Private Sub btnHide_Click(sender As Object, e As EventArgs) Handles btnHide.Click
        Me.DialogResult = DialogResult.No    ' Trả về No cho form cha
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel ' Trả về Cancel
        Me.Close()
    End Sub

    ' -------------- Cho phép di chuyển hộp thoại ------------------
    <DllImport("user32.dll", EntryPoint:="ReleaseCapture")>
    Private Shared Sub ReleaseCapture()
    End Sub

    <DllImport("user32.dll", EntryPoint:="SendMessage")>
    Private Shared Sub SendMessage(hWnd As IntPtr, wMsg As Integer, wParam As Integer, lParam As Integer)
    End Sub

    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HTCAPTION As Integer = 2

    ' --- Cho phép kéo toàn form ---
    Private Sub CloseDialog_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
    End Sub

End Class
