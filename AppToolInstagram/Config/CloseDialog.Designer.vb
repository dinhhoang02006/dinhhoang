<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CloseDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        doBongVienHopThoai = New Guna.UI2.WinForms.Guna2BorderlessForm(components)
        lblInfo = New Label()
        btnHide = New Guna.UI2.WinForms.Guna2Button()
        btnClose = New Guna.UI2.WinForms.Guna2Button()
        btnCancel = New Guna.UI2.WinForms.Guna2Button()
        boTronFormHopThoai = New Guna.UI2.WinForms.Guna2Elipse(components)
        SuspendLayout()
        ' 
        ' doBongVienHopThoai
        ' 
        doBongVienHopThoai.BorderRadius = 12
        doBongVienHopThoai.ContainerControl = Me
        doBongVienHopThoai.DockIndicatorColor = Color.Black
        doBongVienHopThoai.DockIndicatorTransparencyValue = 0.6R
        doBongVienHopThoai.DragForm = False
        doBongVienHopThoai.TransparentWhileDrag = True
        ' 
        ' lblInfo
        ' 
        lblInfo.AutoSize = True
        lblInfo.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblInfo.Location = New Point(67, 44)
        lblInfo.Name = "lblInfo"
        lblInfo.Size = New Size(243, 21)
        lblInfo.TabIndex = 0
        lblInfo.Text = "Bạn chắc chắn muốn thoát Tool?"
        ' 
        ' btnHide
        ' 
        btnHide.BackColor = Color.Transparent
        btnHide.BorderRadius = 5
        btnHide.Cursor = Cursors.Hand
        btnHide.CustomizableEdges = CustomizableEdges5
        btnHide.DisabledState.BorderColor = Color.DarkGray
        btnHide.DisabledState.CustomBorderColor = Color.DarkGray
        btnHide.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnHide.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnHide.FillColor = SystemColors.HotTrack
        btnHide.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnHide.ForeColor = Color.White
        btnHide.HoverState.FillColor = Color.RoyalBlue
        btnHide.Location = New Point(91, 98)
        btnHide.Name = "btnHide"
        btnHide.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        btnHide.ShadowDecoration.Depth = 3
        btnHide.ShadowDecoration.Enabled = True
        btnHide.Size = New Size(104, 33)
        btnHide.TabIndex = 1
        btnHide.Text = "Ẩn cửa sổ"
        ' 
        ' btnClose
        ' 
        btnClose.BackColor = Color.Transparent
        btnClose.BorderRadius = 5
        btnClose.Cursor = Cursors.Hand
        btnClose.CustomizableEdges = CustomizableEdges3
        btnClose.DisabledState.BorderColor = Color.DarkGray
        btnClose.DisabledState.CustomBorderColor = Color.DarkGray
        btnClose.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnClose.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnClose.FillColor = Color.Red
        btnClose.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClose.ForeColor = Color.White
        btnClose.HoverState.FillColor = Color.Firebrick
        btnClose.Location = New Point(210, 98)
        btnClose.Name = "btnClose"
        btnClose.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnClose.ShadowDecoration.Depth = 3
        btnClose.ShadowDecoration.Enabled = True
        btnClose.Size = New Size(74, 33)
        btnClose.TabIndex = 2
        btnClose.Text = "Thoát"
        ' 
        ' btnCancel
        ' 
        btnCancel.BorderRadius = 5
        btnCancel.Cursor = Cursors.Hand
        btnCancel.CustomizableEdges = CustomizableEdges1
        btnCancel.DisabledState.BorderColor = Color.DarkGray
        btnCancel.DisabledState.CustomBorderColor = Color.DarkGray
        btnCancel.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnCancel.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnCancel.FillColor = Color.Transparent
        btnCancel.Font = New Font("Segoe UI Semilight", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnCancel.ForeColor = Color.Black
        btnCancel.HoverState.FillColor = Color.WhiteSmoke
        btnCancel.Location = New Point(339, 23)
        btnCancel.Name = "btnCancel"
        btnCancel.PressedColor = Color.WhiteSmoke
        btnCancel.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnCancel.Size = New Size(33, 31)
        btnCancel.TabIndex = 3
        btnCancel.Text = "✖"
        ' 
        ' boTronFormHopThoai
        ' 
        boTronFormHopThoai.BorderRadius = 12
        boTronFormHopThoai.TargetControl = Me
        ' 
        ' CloseDialog
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.White
        ClientSize = New Size(394, 160)
        Controls.Add(btnCancel)
        Controls.Add(btnClose)
        Controls.Add(btnHide)
        Controls.Add(lblInfo)
        FormBorderStyle = FormBorderStyle.None
        Name = "CloseDialog"
        StartPosition = FormStartPosition.CenterParent
        Text = "CloseDialog"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblInfo As Label
    Friend WithEvents btnHide As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnClose As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnCancel As Guna.UI2.WinForms.Guna2Button
    Private WithEvents boTronFormHopThoai As Guna.UI2.WinForms.Guna2Elipse
    Friend WithEvents doBongVienHopThoai As Guna.UI2.WinForms.Guna2BorderlessForm
End Class
