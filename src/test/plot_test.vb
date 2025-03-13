Imports System.IO
Imports ggplot
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Distributions
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.VBMath
Imports SMRUCC.Rsharp

Module plot_test

    Sub New()
#If NET8_0_OR_GREATER Then
        Call SkiaDriver.Register()
#Else
        Call ImageDriver.Register()
#End If
    End Sub

    Sub Main1()

        Call New Form1().ShowDialog()

        Dim x As Double() = seq(0, 50, by:=0.05).ToArray
        Dim y1 = Gaussian.Gaussian(x, 12000, 15, 1)
        Dim y2 = Gaussian.Gaussian(x, 300, 27, 2)
        Dim y3 = Gaussian.Gaussian(x, 3600, 35, 4)
        Dim noise = Vector.rand(30, 50, x.Length).ToArray

        Dim test As DataFrame = New DataFrame() _
            .add("x", x) _
            .add("y", SIMD.Add.f64_op_add_f64(SIMD.Add.f64_op_add_f64(SIMD.Add.f64_op_add_f64(y1, y2), y3), noise))
        Dim plot As ggplot.ggplot = ggplotFunction.ggplot(test, mapping:=aes("x", "y"), args:=list(slot("w") = 1920, slot("h") = 1600))

        plot += geom_point(size:=32)

        'Dim width As Integer = 1000
        'Dim height As Integer = 900
        'Dim scaleFactor As Double = 1.25
        'Dim plotPadding As New Padding(100, 100, 200, 200)
        'Dim size As New Size(width * scaleFactor, height * scaleFactor)
        'Dim g As IGraphics = DriverLoad.CreateGraphicsDevice(size, plot.ggplotTheme.background.TranslateColor, driver:=Drivers.GDI)
        'Dim region As New GraphicsRegion With {
        '    .Padding = plotPadding,
        '    .Size = size
        '}

        'Call plot.Plot(g, region)
        'Call g.Flush()

        Using s As Stream = "./scatter.png".Open(FileMode.OpenOrCreate, doClear:=True)
            Call plot.Save(s, ImageFormats.Png)
        End Using

        Using s As Stream = "./scatter.svg".Open(FileMode.OpenOrCreate, doClear:=True)
            Call plot.Save(s, ImageFormats.Svg)
        End Using

        Using s As Stream = "./scatter.pdf".Open(FileMode.OpenOrCreate, doClear:=True)
            Call plot.Save(s, ImageFormats.Pdf)
        End Using
    End Sub
End Module
