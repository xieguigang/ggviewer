Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.PostScript
Imports PlotPadding = Microsoft.VisualBasic.MIME.Html.CSS.Padding

Public Class PlotView

    Dim m_ggplot As ggplot.ggplot
    Dim m_counter As New PerformanceCounter
    Dim m_ps As PostScriptBuilder

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

        x = ggplot.base.data!x
        y = ggplot.base.data!y

        m_ps = g.GetContextInfo
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
            dataY = d3js.scale.linear.domain(values:=New Double() {0, Height}).range(y)

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

    Public Sub Save(filename As String, format As ImageFormats)

    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        Dim offset = PointToClient(Cursor.Position)
        Dim dataXy As New PointF(dataX(offset.X), dataY(offset.Y))
        Dim objectXy As New PointF(scaleX(offset.X), scaleY(offset.Y))

        Call ToolTip1.SetToolTip(PictureBox1, $"[{dataXy.X.ToString("F1")},{dataXy.Y.ToString("F1")}]")
    End Sub
End Class
