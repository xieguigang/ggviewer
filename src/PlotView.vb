Imports ggplot.elements
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.PostScript
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports PlotPadding = Microsoft.VisualBasic.MIME.Html.CSS.Padding

Public Class PlotView

    Dim m_ggplot As ggplot.ggplot
    Dim m_counter As New PerformanceCounter
    Dim m_ps As PostScriptBuilder
    Dim m_xy As SpatialLookup

    Dim x As DoubleRange
    Dim y As DoubleRange

    Dim dataX As d3js.scale.LinearScale
    Dim dataY As d3js.scale.LinearScale
    Dim scaleX As d3js.scale.LinearScale
    Dim scaleY As d3js.scale.LinearScale

    Public Property ggplot As ggplot.ggplot
        Get
            Return m_ggplot
        End Get
        Set
            m_ggplot = Value
            m_ggplot.driver = Drivers.PostScript

            If Not m_ggplot Is Nothing Then
                Call RenderPsElements()
                Call Rendering()
            End If
        End Set
    End Property

    Public ReadOnly Property LastRenderCounter As TimeCounter
        Get
            Return m_counter.LastCheckPoint
        End Get
    End Property

    Public Property PlotPadding As PlotPadding = "padding: 5% 10% 10% 10%;"
    Public Property ScaleFactor As Single = 1.25

#If DEBUG Then
    Public Property Debug As Boolean = True
#Else
    Public Property Debug As Boolean = False
#End If

    Shared Sub New()
        Call ImageDriver.RegisterPostScript()
    End Sub

    Private Sub RenderPsElements()
        Dim size As New Size(Width * ScaleFactor, Height * ScaleFactor)
        Dim bg = ggplot.ggplotTheme.background.TranslateColor
        Dim g As GraphicsPostScript = DriverLoad.CreateGraphicsDevice(size, bg, driver:=Drivers.PostScript)
        Dim region As New GraphicsRegion With {
            .Padding = Me.PlotPadding,
            .Size = size
        }

        Call ggplot.Plot(g, region)
        Call g.Flush()

        Dim x As axisMap = ggplot.base.data!x
        Dim y As axisMap = ggplot.base.data!y

        If x.mapper = d3js.scale.MapperTypes.Continuous Then
            Me.x = x.ToNumeric
        Else
            Throw New NotImplementedException
        End If
        If y.mapper = d3js.scale.MapperTypes.Continuous Then
            Me.y = y.ToNumeric
        Else
            Throw New NotImplementedException
        End If

        m_ps = g.GetContextInfo
        m_xy = New SpatialLookup(ps:=g.GetContextInfo)
    End Sub

    Private Sub Rendering()
        If Width <= 0 OrElse Height <= 0 Then
            Return
        End If

        If Debug Then
            Call m_counter.Mark()
        End If

        If Not m_ps Is Nothing Then
            Dim size As New Size(Width * ScaleFactor, Height * ScaleFactor)
            Dim render As PostScriptBuilder = m_ps.Resize(size)

            dataX = d3js.scale.linear.domain(values:=New Double() {0, Width}).range(x)
            dataY = d3js.scale.linear(reverse:=True).domain(values:=New Double() {0, Height}).range(y)

            scaleX = d3js.scale.linear.domain(values:=New Double() {0, Width}).range(values:=New Double() {0, size.Width})
            scaleY = d3js.scale.linear.domain(values:=New Double() {0, Height}).range(values:=New Double() {0, size.Height})

            PictureBox1.BackgroundImage = DirectCast(render.MakePaint(Drivers.GDI), ImageData).GetGdiPlusRasterImageResource
        End If

        If Debug Then
            Call m_counter.Mark("render")
        End If
    End Sub

    Private Sub PlotView_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Call Rendering()
    End Sub

    Public Sub Save(filename As String)
        Dim driver As Drivers = g.ParseDriverEnumValue(filename.ExtensionSuffix)
        Dim plotPadding As PlotPadding = plotPadding.TryParse(ggplot.ggplotTheme.padding)
        Dim size As New Size(m_ps.width, m_ps.height)
        Dim bg = ggplot.ggplotTheme.background
        Dim padding As Integer() = PaddingLayout.EvaluateFromCSS(New CSSEnvirnment(size), plotPadding)

        Using gfx As IGraphics = DriverLoad.CreateGraphicsDevice(size, bg, driver:=driver)
            Call m_ps.MakePaint(gfx)
            Call DriverLoad.GetData(gfx, padding).Save(filename)
        End Using
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        Dim offset = PointToClient(Cursor.Position)
        Dim dataXy As New PointF(dataX(offset.X), dataY(offset.Y))
        Dim objectXy As New PointF(scaleX(offset.X), scaleY(offset.Y))
        Dim obj As PSElement = m_xy.FindNearby(objectXy.X, objectXy.Y)

        If obj Is Nothing Then
            Call ToolTip1.SetToolTip(PictureBox1, $"[{dataXy.X.ToString("F1")},{dataXy.Y.ToString("F1")}]")
        Else
            Call ToolTip1.SetToolTip(PictureBox1, $"[{dataXy.X.ToString("F1")},{dataXy.Y.ToString("F1")}] {obj.ToString}")
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Call Clipboard.SetImage(PictureBox1.BackgroundImage)
    End Sub

    Private Sub SaveImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveImageToolStripMenuItem.Click
        Dim filter_str As String

#If NET48 Then
        filter_str = "Raster image(*.jpg;*.png;*.bmp;*.webp)|*.jpg;*.png;*.bmp;*.webp|SVG(*.svg)|*.svg"
#Else
        filter_str = "Raster image(*.jpg;*.png;*.bmp;*.webp)|*.jpg;*.png;*.bmp;*.webp|SVG(*.svg)|*.svg|PDF(*.pdf)|*.pdf"
#End If
#Disable Warning
        Using file As New SaveFileDialog With {.Filter = filter_str}
            If file.ShowDialog = DialogResult.OK Then
                Call Save(file.FileName)
            End If
        End Using
#Enable Warning
    End Sub
End Class
