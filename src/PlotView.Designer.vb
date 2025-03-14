<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlotView
    Inherits System.Windows.Forms.UserControl

    'UserControl 重写释放以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PlotView))
        PictureBox1 = New PictureBox()
        ToolTip1 = New ToolTip(components)
        ContextMenuStrip1 = New ContextMenuStrip(components)
        CopyToolStripMenuItem = New ToolStripMenuItem()
        SaveImageToolStripMenuItem = New ToolStripMenuItem()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackgroundImageLayout = ImageLayout.Zoom
        PictureBox1.ContextMenuStrip = ContextMenuStrip1
        PictureBox1.Dock = DockStyle.Fill
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(404, 336)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {CopyToolStripMenuItem, SaveImageToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(135, 48)
        ' 
        ' CopyToolStripMenuItem
        ' 
        CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), Image)
        CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        CopyToolStripMenuItem.Size = New Size(134, 22)
        CopyToolStripMenuItem.Text = "Copy"
        ' 
        ' SaveImageToolStripMenuItem
        ' 
        SaveImageToolStripMenuItem.Image = CType(resources.GetObject("SaveImageToolStripMenuItem.Image"), Image)
        SaveImageToolStripMenuItem.Name = "SaveImageToolStripMenuItem"
        SaveImageToolStripMenuItem.Size = New Size(134, 22)
        SaveImageToolStripMenuItem.Text = "Save Image"
        ' 
        ' PlotView
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.SkyBlue
        Controls.Add(PictureBox1)
        Name = "PlotView"
        Size = New Size(404, 336)
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveImageToolStripMenuItem As ToolStripMenuItem

End Class
