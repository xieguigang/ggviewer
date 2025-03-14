Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.PostScript
Imports PlotPadding = Microsoft.VisualBasic.MIME.Html.CSS.Padding

Public Class PlotView

    Dim m_ggplot As ggplot.ggplot
    Dim m_counter As New PerformanceCounter
    Dim m_ps As PostScriptBuilder

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

    Private Sub RenderPsElements()
        Dim size As New Size(Width * ScaleFactor, Height * ScaleFactor)
        Dim bg = ggplot.ggplotTheme.background.TranslateColor
        Dim g As GraphicsPS = DriverLoad.CreateGraphicsDevice(size, bg, driver:=Drivers.PostScript)
        Dim region As New GraphicsRegion With {
            .Padding = Me.PlotPadding,
            .Size = size
        }

        Call ggplot.Plot(g, region)
        Call g.Flush()

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
End Class
