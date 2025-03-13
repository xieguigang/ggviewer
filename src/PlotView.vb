Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports PlotPadding = Microsoft.VisualBasic.MIME.Html.CSS.Padding

Public Class PlotView

    Dim m_ggplot As ggplot.ggplot
    Dim m_counter As New PerformanceCounter

    Public Property ggplot As ggplot.ggplot
        Get
            Return m_ggplot
        End Get
        Set
            m_ggplot = Value
            Rendering()
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

    Private Sub Rendering()
        If Width <= 0 OrElse Height <= 0 Then
            Return
        End If

        If Debug Then
            Call m_counter.Mark()
        End If

        If Not ggplot Is Nothing Then
            Dim size As New Size(Width * ScaleFactor, Height * ScaleFactor)
            Dim g As IGraphics = DriverLoad.CreateGraphicsDevice(size, ggplot.ggplotTheme.background.TranslateColor, driver:=Drivers.GDI)
            Dim region As New GraphicsRegion With {
                .Padding = Me.PlotPadding,
                .Size = size
            }

            Call ggplot.Plot(g, region)
            Call g.Flush()

            PictureBox1.BackgroundImage = DirectCast(g, GdiRasterGraphics).GetGdiPlusRasterImageResource
            g.Dispose()
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
