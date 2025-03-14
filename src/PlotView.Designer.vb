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
        ContextMenuStrip1 = New ContextMenuStrip(components)
        CopyToolStripMenuItem = New ToolStripMenuItem()
        SaveImageToolStripMenuItem = New ToolStripMenuItem()
        ToolTip1 = New ToolTip(components)
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        resources.ApplyResources(PictureBox1, "PictureBox1")
        PictureBox1.ContextMenuStrip = ContextMenuStrip1
        PictureBox1.Name = "PictureBox1"
        PictureBox1.TabStop = False
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {CopyToolStripMenuItem, SaveImageToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        resources.ApplyResources(ContextMenuStrip1, "ContextMenuStrip1")
        ' 
        ' CopyToolStripMenuItem
        ' 
        resources.ApplyResources(CopyToolStripMenuItem, "CopyToolStripMenuItem")
        CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        ' 
        ' SaveImageToolStripMenuItem
        ' 
        resources.ApplyResources(SaveImageToolStripMenuItem, "SaveImageToolStripMenuItem")
        SaveImageToolStripMenuItem.Name = "SaveImageToolStripMenuItem"
        ' 
        ' PlotView
        ' 
        resources.ApplyResources(Me, "$this")
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.SkyBlue
        Controls.Add(PictureBox1)
        Name = "PlotView"
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
