<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        panelTitleBar = New Panel()
        btnMinimum = New Guna.UI2.WinForms.Guna2CircleButton()
        btnLargeSmall = New Guna.UI2.WinForms.Guna2CircleButton()
        Panel3 = New Panel()
        btnMenu = New Guna.UI2.WinForms.Guna2Button()
        btnClose = New Guna.UI2.WinForms.Guna2CircleButton()
        boTronGocForm = New Guna.UI2.WinForms.Guna2Elipse(components)
        doBongForm = New Guna.UI2.WinForms.Guna2ShadowForm(components)
        infoAppAtTaskBar = New StatusStrip()
        stLeft = New ToolStripStatusLabel()
        stRight = New ToolStripStatusLabel()
        Menu = New Guna.UI2.WinForms.Guna2ContextMenuStrip()
        RegInstagram = New regInstagram()
        panelTitleBar.SuspendLayout()
        Panel3.SuspendLayout()
        infoAppAtTaskBar.SuspendLayout()
        SuspendLayout()
        ' 
        ' panelTitleBar
        ' 
        panelTitleBar.BackColor = Color.Transparent
        panelTitleBar.Controls.Add(btnMinimum)
        panelTitleBar.Controls.Add(btnLargeSmall)
        panelTitleBar.Controls.Add(Panel3)
        panelTitleBar.Controls.Add(btnClose)
        panelTitleBar.Dock = DockStyle.Top
        panelTitleBar.Location = New Point(0, 0)
        panelTitleBar.Margin = New Padding(0)
        panelTitleBar.Name = "panelTitleBar"
        panelTitleBar.Padding = New Padding(0, 4, 0, 0)
        panelTitleBar.Size = New Size(1486, 84)
        panelTitleBar.TabIndex = 7
        ' 
        ' btnMinimum
        ' 
        btnMinimum.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnMinimum.BackColor = Color.Transparent
        btnMinimum.DisabledState.BorderColor = Color.DarkGray
        btnMinimum.DisabledState.CustomBorderColor = Color.DarkGray
        btnMinimum.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnMinimum.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnMinimum.FillColor = Color.White
        btnMinimum.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnMinimum.ForeColor = Color.Black
        btnMinimum.Location = New Point(1299, 15)
        btnMinimum.Margin = New Padding(3, 4, 3, 4)
        btnMinimum.Name = "btnMinimum"
        btnMinimum.Padding = New Padding(2, 0, 0, 4)
        btnMinimum.PressedColor = Color.WhiteSmoke
        btnMinimum.ShadowDecoration.BorderRadius = 12
        btnMinimum.ShadowDecoration.CustomizableEdges = CustomizableEdges1
        btnMinimum.ShadowDecoration.Depth = 5
        btnMinimum.ShadowDecoration.Enabled = True
        btnMinimum.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        btnMinimum.Size = New Size(42, 48)
        btnMinimum.TabIndex = 7
        btnMinimum.Text = "–"
        ' 
        ' btnLargeSmall
        ' 
        btnLargeSmall.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnLargeSmall.BackColor = Color.Transparent
        btnLargeSmall.DisabledState.BorderColor = Color.DarkGray
        btnLargeSmall.DisabledState.CustomBorderColor = Color.DarkGray
        btnLargeSmall.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnLargeSmall.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnLargeSmall.FillColor = Color.White
        btnLargeSmall.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnLargeSmall.ForeColor = Color.Black
        btnLargeSmall.Location = New Point(1367, 15)
        btnLargeSmall.Margin = New Padding(3, 4, 3, 4)
        btnLargeSmall.Name = "btnLargeSmall"
        btnLargeSmall.Padding = New Padding(2, 0, 0, 4)
        btnLargeSmall.PressedColor = Color.WhiteSmoke
        btnLargeSmall.ShadowDecoration.BorderRadius = 12
        btnLargeSmall.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnLargeSmall.ShadowDecoration.Depth = 5
        btnLargeSmall.ShadowDecoration.Enabled = True
        btnLargeSmall.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        btnLargeSmall.Size = New Size(42, 48)
        btnLargeSmall.TabIndex = 12
        btnLargeSmall.Text = "🗗"
        ' 
        ' Panel3
        ' 
        Panel3.BackColor = Color.Transparent
        Panel3.Controls.Add(btnMenu)
        Panel3.Dock = DockStyle.Left
        Panel3.Location = New Point(0, 4)
        Panel3.Margin = New Padding(3, 4, 3, 4)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(265, 80)
        Panel3.TabIndex = 11
        ' 
        ' btnMenu
        ' 
        btnMenu.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnMenu.BackColor = Color.Transparent
        btnMenu.BorderColor = Color.Silver
        btnMenu.BorderRadius = 6
        btnMenu.BorderThickness = 2
        btnMenu.Cursor = Cursors.Hand
        btnMenu.CustomizableEdges = CustomizableEdges3
        btnMenu.DisabledState.BorderColor = Color.DarkGray
        btnMenu.DisabledState.CustomBorderColor = Color.DarkGray
        btnMenu.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnMenu.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnMenu.FillColor = Color.Transparent
        btnMenu.Font = New Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnMenu.ForeColor = Color.Black
        btnMenu.HoverState.FillColor = Color.FromArgb(CByte(235), CByte(251), CByte(239))
        btnMenu.Location = New Point(14, 11)
        btnMenu.Margin = New Padding(3, 4, 3, 4)
        btnMenu.Name = "btnMenu"
        btnMenu.PressedColor = Color.FromArgb(CByte(235), CByte(251), CByte(239))
        btnMenu.ShadowDecoration.BorderRadius = 4
        btnMenu.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnMenu.ShadowDecoration.Depth = 7
        btnMenu.Size = New Size(234, 61)
        btnMenu.TabIndex = 18
        btnMenu.Text = "Profile Auto"
        btnMenu.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.BackColor = Color.Transparent
        btnClose.DisabledState.BorderColor = Color.DarkGray
        btnClose.DisabledState.CustomBorderColor = Color.DarkGray
        btnClose.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnClose.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnClose.FillColor = Color.Red
        btnClose.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClose.ForeColor = Color.White
        btnClose.Location = New Point(1430, 15)
        btnClose.Margin = New Padding(3, 4, 3, 4)
        btnClose.Name = "btnClose"
        btnClose.Padding = New Padding(1, 0, 0, 1)
        btnClose.PressedColor = Color.LightCoral
        btnClose.ShadowDecoration.CustomizableEdges = CustomizableEdges5
        btnClose.ShadowDecoration.Depth = 5
        btnClose.ShadowDecoration.Enabled = True
        btnClose.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        btnClose.Size = New Size(42, 48)
        btnClose.TabIndex = 6
        btnClose.Text = "x"
        ' 
        ' boTronGocForm
        ' 
        boTronGocForm.BorderRadius = 12
        boTronGocForm.TargetControl = Me
        ' 
        ' doBongForm
        ' 
        doBongForm.TargetForm = Me
        ' 
        ' infoAppAtTaskBar
        ' 
        infoAppAtTaskBar.ImageScalingSize = New Size(20, 20)
        infoAppAtTaskBar.Items.AddRange(New ToolStripItem() {stLeft, stRight})
        infoAppAtTaskBar.Location = New Point(0, 1054)
        infoAppAtTaskBar.Name = "infoAppAtTaskBar"
        infoAppAtTaskBar.Padding = New Padding(1, 0, 16, 0)
        infoAppAtTaskBar.Size = New Size(1486, 26)
        infoAppAtTaskBar.TabIndex = 9
        infoAppAtTaskBar.Text = "infoAppAtTaskBar"
        ' 
        ' stLeft
        ' 
        stLeft.DisplayStyle = ToolStripItemDisplayStyle.Text
        stLeft.ForeColor = Color.FromArgb(CByte(107), CByte(114), CByte(128))
        stLeft.Margin = New Padding(10, 3, 0, 2)
        stLeft.Name = "stLeft"
        stLeft.Size = New Size(50, 21)
        stLeft.Text = "Ready"
        ' 
        ' stRight
        ' 
        stRight.DisplayStyle = ToolStripItemDisplayStyle.Text
        stRight.ForeColor = Color.FromArgb(CByte(107), CByte(114), CByte(128))
        stRight.Name = "stRight"
        stRight.Size = New Size(1409, 20)
        stRight.Spring = True
        stRight.Text = "v1.0.0 | UTF-8 | VN"
        stRight.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Menu
        ' 
        Menu.BackColor = Color.White
        Menu.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Menu.ImageScalingSize = New Size(20, 20)
        Menu.Name = "Guna2ContextMenuStrip1"
        Menu.RenderStyle.ArrowColor = Color.FromArgb(CByte(235), CByte(251), CByte(239))
        Menu.RenderStyle.BorderColor = Color.Gray
        Menu.RenderStyle.ColorTable = Nothing
        Menu.RenderStyle.RoundedEdges = True
        Menu.RenderStyle.SelectionArrowColor = Color.White
        Menu.RenderStyle.SelectionBackColor = Color.FromArgb(CByte(235), CByte(251), CByte(239))
        Menu.RenderStyle.SelectionForeColor = Color.Black
        Menu.RenderStyle.SeparatorColor = Color.DimGray
        Menu.RenderStyle.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
        Menu.Size = New Size(61, 4)
        ' 
        ' RegInstagram
        ' 
        RegInstagram.Dock = DockStyle.Fill
        RegInstagram.Location = New Point(0, 84)
        RegInstagram.Margin = New Padding(3, 5, 3, 5)
        RegInstagram.Name = "RegInstagram"
        RegInstagram.Size = New Size(1486, 970)
        RegInstagram.TabIndex = 8
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1486, 1080)
        Controls.Add(RegInstagram)
        Controls.Add(panelTitleBar)
        Controls.Add(infoAppAtTaskBar)
        FormBorderStyle = FormBorderStyle.None
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(3, 4, 3, 4)
        Name = "Form1"
        Text = "Form1"
        panelTitleBar.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        infoAppAtTaskBar.ResumeLayout(False)
        infoAppAtTaskBar.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents rdoRotate1 As RadioButton
    Friend WithEvents rdoStatic1 As RadioButton
    Friend WithEvents panelTitleBar As Panel
    Friend WithEvents cmsTitle As ContextMenuStrip
    Friend WithEvents btnClose As Guna.UI2.WinForms.Guna2CircleButton
    Friend WithEvents boTronGocForm As Guna.UI2.WinForms.Guna2Elipse
    Friend WithEvents doBongForm As Guna.UI2.WinForms.Guna2ShadowForm
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnHide As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnLargeSmall As Guna.UI2.WinForms.Guna2CircleButton
    Friend WithEvents btnMinimum As Guna.UI2.WinForms.Guna2CircleButton
    Friend WithEvents infoAppAtTaskBar As StatusStrip
    Friend WithEvents stLeft As ToolStripStatusLabel
    Friend WithEvents stRight As ToolStripStatusLabel
    Friend WithEvents Menu As Guna.UI2.WinForms.Guna2ContextMenuStrip
    Friend WithEvents btnMenu As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents RegInstagram As regInstagram

End Class
