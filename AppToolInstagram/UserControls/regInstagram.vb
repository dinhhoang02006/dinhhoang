Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Window
Imports Guna.UI2.WinForms

Public Class regInstagram

    ' ********* Biến toàn cục ************
    Private proxyManager As ProxySave   'Notepad 

    ' ---------------- Hàm chính Load Form----------------
    Private Sub regInstagram_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load hộp thoại Loại Proxy
        LoadTypeProxy()

        ' Load DatagridView
        LoaddvgLog()

        ' Load ScrollBar
        handleScrollBar.Attach(dgvLog, hideDelayMs:=3000)
        handleScrollBar.Attach(txtProxy, hideDelayMs:=3000)

        ' Load textBox and Notepad nhập Proxy
        proxyManager = New ProxySave(txtProxy)
    End Sub





    ' ----------- Nhập Proxy --------------
    Private Sub btnOpenNotepad_Click(sender As Object, e As EventArgs) Handles btnOpenNotepad.Click
        proxyManager.OpenInNotepad()
    End Sub



    ' ----------- Loại Proxy --------------
    Private Sub LoadTypeProxy()
        cboProtocol.Items.Clear()
        cboProtocol.Items.Add("HTTP")
        cboProtocol.Items.Add("SOCKS5")
        cboProtocol.Items.Add("HTTPS")

        ' Mặc định hiển thị HTTP trên Combox chọn loại Proxy
        If cboProtocol.Items.Count > 0 Then
            cboProtocol.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboProtocol_Enter(sender As Object, e As EventArgs) Handles cboProtocol.Enter
        cboProtocol.BorderThickness = 2
    End Sub

    Private Sub cboProtocol_Leave(sender As Object, e As EventArgs) Handles cboProtocol.Leave
        cboProtocol.BorderThickness = 1
    End Sub




    ' ----------------  Bắt đầu/ Dừng chạy ---------------------
    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        pctBxStatus.FillColor = Color.FromArgb(34, 197, 94)
        lblStatus.Text = "Đang chạy..."
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        pctBxStatus.FillColor = Color.FromArgb(209, 213, 219)
        lblStatus.Text = "Đang chờ..."
    End Sub



    ' ---------------- DataGrideView (Bảng thông tin kết quả chạy) --------------
    Private Sub LoaddvgLog()
        dgvLog.Left = 20
        dgvLog.Width = Me.ClientSize.Width - 60   ' 20 trái + 20 phải
        dgvLog.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom

        dgvLog.Rows.Add("2025-08-29 10:01", "user1@mail.com", "http://127.0.0.1:8080", "OK", "—")
        dgvLog.Rows.Add("2025-08-29 10:03", "user2@mail.com", "socks5://127.0.0.1:1080", "ERROR", "—")
        dgvLog.Rows.Add("2025-08-29 10:05", "user3@mail.com", "http://proxy.example.com:3128", "RETRY", "—")
        dgvLog.Rows.Add("2025-08-29 10:07", "user4@mail.com", "http://192.168.1.100:8888", "OK", "—")
        dgvLog.Rows.Add("2025-08-29 10:09", "user5@mail.com", "socks5://10.0.0.2:1080", "ERROR", "—")
        dgvLog.Rows.Add("2025-08-29 10:01", "user1@mail.com", "http://127.0.0.1:8080", "OK", "—")
        dgvLog.Rows.Add("2025-08-29 10:03", "user2@mail.com", "socks5://127.0.0.1:1080", "ERROR", "—")
        dgvLog.Rows.Add("2025-08-29 10:05", "user3@mail.com", "http://proxy.example.com:3128", "RETRY", "—")
        dgvLog.Rows.Add("2025-08-29 10:07", "user4@mail.com", "http://192.168.1.100:8888", "OK", "—")
        dgvLog.Rows.Add("2025-08-29 10:09", "user5@mail.com", "socks5://10.0.0.2:1080", "ERROR", "—")
    End Sub


    Private Sub dgvLog_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If dgvLog.Columns(e.ColumnIndex).HeaderText = "Trạng thái" AndAlso e.Value IsNot Nothing Then
            Select Case e.Value.ToString.ToUpperInvariant
                Case "OK"
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74)   ' #16A34A (green-600)
                Case "ERROR"
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38)   ' #DC2626 (red-600)
                Case "RETRY"
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6)   ' #D97706 (amber-600)
            End Select
        End If
    End Sub

    Private Sub dgvLog_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLog.CellContentClick

    End Sub

    Private Sub rdoStatic_CheckedChanged(sender As Object, e As EventArgs) Handles rdoStatic.CheckedChanged

    End Sub
End Class
