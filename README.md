# ``ggviewer``

A interactive data visualization canvas for .NET CLR ggplot/R# based on winform framework.

![](./docs/Capture.PNG)

```vbnet
Dim iris = DataFrame.read_csv("./bezdekIris.csv")
Dim classes As StringVector = iris.delete("class")
Dim pca = iris.CommonDataSet().PrincipalComponentAnalysis(maxPC:=2).GetPCAScore
Dim plot As ggplot.ggplot = ggplotFunction.ggplot(
    data:=pca.add("class", classes),
    mapping:=aes("PC1", "PC2", color:="class"),
    colorSet:="jet",
    padding:="padding: 5% 10% 10% 10%;"
)

plot += geom_point(size:=12)

view.ScaleFactor = 1.25
view.PlotPadding = plot.ggplotTheme.padding
view.ggplot = plot
```
