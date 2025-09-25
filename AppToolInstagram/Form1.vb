Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Window
Imports Guna.UI2.WinForms

Partial Public Class Form1
    Inherits Form

    ' --------------------------- CSS CHUNG ----------------------
    Public BackColorForm As Color = Color.FromArgb(249, 250, 251)          ' Màu nền toàn Form
    Public ColorTitleBar As Color = Color.FromArgb(229, 231, 235)        'Màu thanh TitleBar
    Public Shared FillColorMenu As Color = Color.FromArgb(235, 251, 239)                     'Màu nền + hover Menu
    Public HoverColorMenu As Color = Color.FromArgb(18, 0, 0, 0)         'Màu nền khi di chuột qua menu

    ' ** Biến Toàn cục thông thường **

    ' Đóng/ Ẩn/ Mở App
    Private tray As NotifyIcon
    Private trayMenu As ContextMenuStrip
    Private prevBounds As Rectangle             ' Lưu kích thước/vị trí trước khi full screen
    Private isFullScreen As Boolean = False


    'Menu
    Private currentItem As ToolStripMenuItem     ' Dịch vụ
    Private WithEvents hoverCloseTimer As New Timer With {.Interval = 300} ' auto-close nhẹ nhàng





    Public Sub New()
        InitializeComponent()



    End Sub

    ' ------------- Menu ---------------------
    Private Sub LoadMenu()
        ' CSS
        Menu.Renderer = New ToolStripProfessionalRenderer(New CssMenu())
        Menu.Items.Clear()

        ' Nhóm chính
        Menu.Items.Add(New ToolStripMenuItem("Reg Instagram"))
        Menu.Items.Add(New ToolStripMenuItem("Nuôi Instagram"))
        Menu.Items.Add(New ToolStripSeparator())

        ' Submenu: Mô hình cũ
        Dim oldModels As New ToolStripMenuItem("Dịch Vụ...")
        oldModels.DropDownItems.Add("Mua Proxy Xoay")
        oldModels.DropDownItems.Add("Mua Proxy Tĩnh")
        oldModels.DropDownItems.Add("Khác")
        AddHandler oldModels.MouseEnter, Sub()
                                             If oldModels.HasDropDownItems Then oldModels.ShowDropDown()
                                         End Sub
        Menu.Items.Add(oldModels)

        ' Chức đăng trong menu mặc định hiển
        Dim firstItem = TryCast(Menu.Items(0), ToolStripMenuItem)
        If firstItem IsNot Nothing Then
            btnMenu.Text = firstItem.Text
        End If

        ' Đổi text khi chọn
        AddHandler Menu.ItemClicked, AddressOf Menu_ItemClicked
    End Sub

    ' Hover vào nút -> mở menu
    Private Sub btnMenu_MouseEnter(sender As Object, e As EventArgs) Handles btnMenu.MouseEnter
        If Not Menu.Visible Then
            Menu.Show(btnMenu, New Point(0, btnMenu.Height))
        End If
    End Sub

    ' Đổi text nút theo item được chọn
    Private Sub Menu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)
        If TypeOf e.ClickedItem Is ToolStripSeparator Then Return
        btnMenu.Text = e.ClickedItem.Text
    End Sub

    ' Auto-close: nếu chuột rời cả nút và menu -> đóng
    Private Sub hoverCloseTimer_Tick(sender As Object, e As EventArgs) Handles hoverCloseTimer.Tick
        If Not Menu.Visible Then Return

        Dim p = Cursor.Position
        Dim onButton = btnMenu.RectangleToScreen(btnMenu.ClientRectangle).Contains(p)
        Dim onMenu = Menu.Bounds.Contains(p) OrElse IsOnAnyDropDown(Menu, p)

        If Not onButton AndAlso Not onMenu Then
            Menu.Close(ToolStripDropDownCloseReason.AppClicked)
        End If
    End Sub

    ' Kiểm tra chuột có đang ở bất kỳ dropdown con nào không
    Private Function IsOnAnyDropDown(menu As ContextMenuStrip, screenPoint As Point) As Boolean
        If menu.Bounds.Contains(screenPoint) Then Return True
        For Each obj As ToolStripItem In menu.Items
            If TypeOf obj Is ToolStripMenuItem Then
                Dim mi = CType(obj, ToolStripMenuItem)
                If mi.DropDown IsNot Nothing AndAlso mi.DropDown.Visible AndAlso mi.DropDown.Bounds.Contains(screenPoint) Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    ' Css Menu
    Public Class CssMenu
        Inherits ProfessionalColorTable
        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return Color.White
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return FillColorMenu
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
            Get
                Return FillColorMenu
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
            Get
                Return FillColorMenu
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return Color.LightGray
            End Get
        End Property
    End Class





    ' ------------------ Đóng mở app ----------------

    ' Mở lại ứng dụng từ khay
    Private Sub Tray_Open(sender As Object, e As EventArgs)
        Me.Show()
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        Me.Activate()
    End Sub

    ' Thoát hẳn ứng dụng
    Private Sub Tray_Exit(sender As Object, e As EventArgs)
        tray.Visible = False
        Application.Exit()
    End Sub

    'Ẩn form xuống khay hệ thống
    Private Sub HideToTray()
        Me.WindowState = FormWindowState.Minimized
        Me.ShowInTaskbar = False
    End Sub

    ' Tạo khay hệ thống khi cần
    Private Sub EnsureTray()
        trayMenu = New ContextMenuStrip()
        trayMenu.Items.Add("Mở cửa sổ", Nothing, AddressOf Tray_Open)
        trayMenu.Items.Add("Thoát", Nothing, AddressOf Tray_Exit)

        tray = New NotifyIcon() With {
            .Icon = Me.Icon,
            .Text = "Reg Instagram",
            .ContextMenuStrip = trayMenu,
            .Visible = True
        }
        AddHandler tray.DoubleClick, AddressOf Tray_Open
    End Sub

    ' Hộp thoại Nút Đóng app
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Using dlg As New CloseDialog()
            Dim ans As DialogResult = dlg.ShowDialog(Me)

            If ans = DialogResult.Yes Then
                Application.Exit()
            ElseIf ans = DialogResult.No Then
                HideToTray()
            End If
            ' Nếu Cancel thì không làm gì
        End Using
    End Sub

    ' Nút thu nhỏ App xuống taskBar
    Private Sub btnMinimum_Click(sender As Object, e As EventArgs) Handles btnMinimum.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub


    ' Phóng to thu nhỏ App
    Private Sub btnLargeSmall_Click(sender As Object, e As EventArgs) Handles btnLargeSmall.Click
        If Not isFullScreen Then
            prevBounds = Me.Bounds
            Me.WindowState = FormWindowState.Normal   ' đảm bảo ở Normal trước
            Me.Bounds = Screen.PrimaryScreen.Bounds   ' full màn hình
            isFullScreen = True
        Else
            Me.Bounds = prevBounds
            isFullScreen = False
        End If
    End Sub




    ' --------------- Form Load ------------------------ 
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Màu nền Form
        Me.BackColor = BackColorForm

        ' ------------- Khung Form ------------------
        khungForm.DragBarDownColor = ColorTitleBar    ' Màu Highlight thanh TitleBar
        khungForm.EnableDragBy(Me, panelTitleBar, khungForm.DragBarMode.AlwaysHighlighted)   'Hoặc luôn Highlight
        'khungForm.EnableDragBy(Me, panelTitleBar, khungForm.DragBarMode.HighlightOnDrag)     ' Highlight khi di chuyển TitleBar

        ' ------------ Load Menu --------------------
        LoadMenu()
        AddHandler btnMenu.MouseEnter, AddressOf btnMenu_MouseEnter
        AddHandler Menu.Opened, Sub() hoverCloseTimer.Start()
        AddHandler Menu.Closed, Sub() hoverCloseTimer.Stop()

        ' ------------ Bóng viền Form --------------
        Dim shadowForm As New Guna.UI2.WinForms.Guna2ShadowForm()
        shadowForm.SetShadowForm(Me)

        ' ------------ Hộp thoại đóng App ------------
        EnsureTray()

    End Sub

    Private Sub RegInstagram_Load(sender As Object, e As EventArgs) Handles RegInstagram.Load

    End Sub
End Class
