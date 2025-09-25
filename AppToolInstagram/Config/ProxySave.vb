Imports System.IO
Imports System.Text
Imports System.Diagnostics
Imports System.Windows.Forms

Public Class ProxySave
    ' ====== ĐƯỜNG DẪN LƯU FILE ======
    Private ReadOnly ProxyDir As String =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegInstagram")
    Private ReadOnly ProxyFile As String

    ' ====== HEADER BẮT BUỘC ======
    Private Shared ReadOnly HeaderLines As String() = {
        "*** Nhập một Proxy một dòng ***",
        "*** Ấn Ctrl+ S hoặc SAVE để xác nhận Proxy ***"
    }
    Private ReadOnly HeaderBlock As String =
        String.Join(Environment.NewLine, HeaderLines) & Environment.NewLine & Environment.NewLine

    ' ====== Fields ======
    Private _hostForm As Form
    Private _cachedBody As String = ""
    Private ReadOnly _txtProxy As Guna.UI2.WinForms.Guna2TextBox

    Public Sub New(txt As Guna.UI2.WinForms.Guna2TextBox)
        _txtProxy = txt
        ProxyFile = Path.Combine(ProxyDir, "Proxy.txt")

        ' Đăng ký sự kiện thay đổi để cache
        AddHandler _txtProxy.TextChanged, AddressOf OnTextChanged

        EnsureStorage()
        EnsureHeaderAtTop()
        LoadProxiesIntoTextBox()

        HookHostForm()
    End Sub

    ' ---------------- Hook host form để lưu khi đóng ----------------
    Private Sub HookHostForm()
        Dim f = _txtProxy.FindForm()
        If f Is Nothing OrElse f Is _hostForm Then Return
        If _hostForm IsNot Nothing Then
            RemoveHandler _hostForm.FormClosing, AddressOf OnHostFormClosing
        End If
        _hostForm = f
        AddHandler _hostForm.FormClosing, AddressOf OnHostFormClosing
    End Sub

    Private Sub OnHostFormClosing(sender As Object, e As FormClosingEventArgs)
        SaveAlwaysOnClose()
    End Sub

    ' ---------------- Lưu/Đọc file ----------------
    Private Sub EnsureStorage()
        If Not Directory.Exists(ProxyDir) Then Directory.CreateDirectory(ProxyDir)
        If Not File.Exists(ProxyFile) Then
            File.WriteAllText(ProxyFile, HeaderBlock, New UTF8Encoding(False))
        End If
    End Sub

    Private Sub EnsureHeaderAtTop()
        Try
            Dim lines = File.ReadAllLines(ProxyFile, Encoding.UTF8).ToList()
            Dim hasHeader As Boolean =
                lines.Count >= 2 AndAlso
                lines(0).Trim().Equals(HeaderLines(0)) AndAlso
                lines(1).Trim().Equals(HeaderLines(1))

            If Not hasHeader Then
                Dim content = New StringBuilder()
                content.Append(HeaderBlock)
                content.Append(String.Join(Environment.NewLine, lines))
                File.WriteAllText(ProxyFile, content.ToString(), New UTF8Encoding(False))
            End If
        Catch
        End Try
    End Sub

    Public Sub LoadProxiesIntoTextBox()
        Try
            Dim lines = File.ReadAllLines(ProxyFile, Encoding.UTF8).ToList()

            Dim startIdx As Integer = 0
            If lines.Count >= 2 AndAlso
               lines(0).Trim().Equals(HeaderLines(0)) AndAlso
               lines(1).Trim().Equals(HeaderLines(1)) Then
                startIdx = 2
                If lines.Count > startIdx AndAlso String.IsNullOrWhiteSpace(lines(startIdx)) Then startIdx += 1
            End If

            Dim body = lines.Skip(startIdx).
                Select(Function(x) x.Trim()).
                Where(Function(x) x <> "" AndAlso Not x.StartsWith("#"))

            _txtProxy.Text = String.Join(Environment.NewLine, body)
            _cachedBody = GetBodyFromTextBox()
        Catch ex As Exception
            MessageBox.Show("Không thể đọc Proxy.txt: " & ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Async Sub OpenInNotepad()
        Try
            SyncFileFromTextbox()
            EnsureHeaderAtTop()

            Dim psi As New ProcessStartInfo("notepad.exe", $"""{ProxyFile}""") With {.UseShellExecute = True}
            Using p = Process.Start(psi)
                If p Is Nothing Then
                    Process.Start("explorer.exe", $"""{ProxyDir}""")
                    Return
                End If
                Await p.WaitForExitAsync()
                LoadProxiesIntoTextBox()
            End Using
        Catch ex As Exception
            MessageBox.Show("Không mở được Notepad: " & ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveAlwaysOnClose()
        Try
            EnsureStorage()
            Dim body As String
            Try
                body = GetBodyFromTextBox()
            Catch
                body = _cachedBody
            End Try
            WriteFileWithHeader(body)
        Catch
        End Try
    End Sub

    Private Function GetBodyFromTextBox() As String
        Dim lines = _txtProxy.Text.Split({vbCrLf, vbLf}, StringSplitOptions.None).
            Select(Function(x) x.Trim()).
            Where(Function(x) x <> "" AndAlso
                            Not x.Equals(HeaderLines(0), StringComparison.OrdinalIgnoreCase) AndAlso
                            Not x.Equals(HeaderLines(1), StringComparison.OrdinalIgnoreCase) AndAlso
                            Not x.StartsWith("#"))
        Return String.Join(Environment.NewLine, lines)
    End Function

    Private Sub WriteFileWithHeader(body As String)
        Dim sb As New StringBuilder()
        sb.AppendLine(HeaderLines(0))
        sb.AppendLine(HeaderLines(1))
        sb.AppendLine()
        If Not String.IsNullOrWhiteSpace(body) Then sb.Append(body)
        File.WriteAllText(ProxyFile, sb.ToString(), New UTF8Encoding(False))
    End Sub

    Private Sub SyncFileFromTextbox()
        EnsureStorage()
        Dim body As String = GetBodyFromTextBox()
        WriteFileWithHeader(body)
    End Sub

    Private Sub OnTextChanged(sender As Object, e As EventArgs)
        _cachedBody = GetBodyFromTextBox()
    End Sub

End Class
