Imports ggplot
Imports ggplot.layers
Imports ggviewer
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Language.Vectorization
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Distributions
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis.ANOVA
Imports Microsoft.VisualBasic.Math.VBMath
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

Public Class Form1

    Dim WithEvents view As New PlotView

    Shared Sub New()
#If NET8_0_OR_GREATER Then
        Call SkiaDriver.Register()
#Else
        Call ImageDriver.Register()
#End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Controls.Add(view)
        view.Dock = DockStyle.Fill

        Call violinTest()
        ' Call largeScatter()
        ' Call histTest()
        ' Call gaussTest()
        ' Call pcaTest()
    End Sub

    Private Sub view_SizeChanged(sender As Object, e As EventArgs) Handles view.SizeChanged
        If view.Debug Then
            Me.Text = view.LastRenderCounter.ToString
        End If
    End Sub

    Private Sub violinTest()
        Dim a = Vector.norm(100, 5, 2)
        Dim b = Vector.norm(100, 6, 2)
        Dim c = Vector.norm(100, 7, 1)
        Dim data = New DataFrame().add("group", {"A", "B", "C"}.Repeats([each]:=100)).add("value", Linq.Concatenate(a, b, c))
        Dim plot As ggplot.ggplot = ggplotFunction.ggplot(data, aes(x:="group", y:="value")) + geom_violin()

        view.ScaleFactor = 1.25
        view.PlotPadding = plot.ggplotTheme.padding
        view.ggplot = plot
    End Sub

    Private Sub histTest()
        Dim right As Double() = randf.ExponentialRandomNumbers(1, 1000)
        Dim left As Double() = randf.ExponentialRandomNumbers(1, 1000).Select(Function(a) -a).ToArray
        Dim right2 As Double() = randf.ExponentialRandomNumbers(2, 1000)
        Dim raw = New DataFrame().add("value", Linq.Concatenate(left, right, right2)).add("series", {"left", "right", "right2"}.Repeats([each]:=1000))
        Dim plot As ggplot.ggplot = ggplotFunction.ggplot(data:=raw, mapping:=aes(x:="value", fill:="series")) +
            geom_histogram(position:=LayoutPosition.identity, alpha:=0.5, binwidth:=0.1) +
            labs(title:="Multiple Series Distribution", x:="Value", y:="Frequency")

        view.ScaleFactor = 1.25
        view.PlotPadding = plot.ggplotTheme.padding
        view.ggplot = plot
    End Sub

    Private Sub largeScatter()
        Dim scatters = DataFrame.read_csv("E:\ggviewer\data\feature_regions_dbscan.csv")
        Dim plot As ggplot.ggplot = ggplotFunction.ggplot(
            data:=scatters,
            mapping:=aes("x", "y", color:="Cluster"),
            colorSet:="jet",
            padding:="padding: 5% 10% 10% 10%;")

        plot += geom_point(size:=6)

        view.ScaleFactor = 1.25
        view.PlotPadding = plot.ggplotTheme.padding
        view.ggplot = plot
    End Sub

    Private Sub pcaTest()
        Dim iris = DataFrame.read_csv("E:\ggviewer\data\bezdekIris.csv")
        Dim classes As StringVector = iris.delete("class")
        Dim pca = iris.CommonDataSet().PrincipalComponentAnalysis(maxPC:=2).GetPCAScore
        Dim plot As ggplot.ggplot = ggplotFunction.ggplot(
            data:=pca.add("class", classes),
            mapping:=aes("PC1", "PC2", color:="class"),
            colorSet:="jet",
            padding:="padding: 5% 10% 10% 10%;")

        plot += geom_point(size:=12)

        view.ScaleFactor = 1.25
        view.PlotPadding = plot.ggplotTheme.padding
        view.ggplot = plot
    End Sub

    Private Sub gaussTest()
        Dim x As Double() = seq(0, 50, by:=0.05).ToArray
        Dim y1 = Gaussian.Gaussian(x, 12000, 15, 1)
        Dim y2 = Gaussian.Gaussian(x, 300, 27, 2)
        Dim y3 = Gaussian.Gaussian(x, 3600, 35, 4)
        Dim noise = Vector.rand(30, 50, x.Length).ToArray

        Dim test As DataFrame = New DataFrame() _
            .add("x", x) _
            .add("y", SIMD.Add.f64_op_add_f64(SIMD.Add.f64_op_add_f64(SIMD.Add.f64_op_add_f64(y1, y2), y3), noise))
        Dim plot As ggplot.ggplot = ggplotFunction.ggplot(test, mapping:=aes("x", "y"), colorSet:="jet", padding:="padding: 5% 10% 10% 10%;")

        plot += geom_point(size:=12, color:="y")

        view.PlotPadding = plot.ggplotTheme.padding
        view.ggplot = plot

        ' test.rownames = x.Select(Function(xi, i) CStr(i + 1)).ToArray
        '  Call test.WriteCsv("./test_signal.csv")
    End Sub
End Class
