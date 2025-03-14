Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver

#If NET48 Then
Imports System.Drawing.Imaging
#End If

Module Interop

    <Extension>
    Public Function GetGdiPlusRasterImageResource(canvas As GdiRasterGraphics) As System.Drawing.Image
        Using ms As New MemoryStream
#If NET48 Then
            Call canvas.ImageResource.Save(ms, ImageFormat.Png)
#Else
            Call canvas.ImageResource.Save(ms, ImageFormats.Png)
#End If
            Call ms.Flush()
            Call ms.Seek(Scan0, SeekOrigin.Begin)

            Return System.Drawing.Image.FromStream(ms)
        End Using
    End Function

    <Extension>
    Public Function GetGdiPlusRasterImageResource(canvas As ImageData) As System.Drawing.Image
#If NET48 Then
        Return canvas.Image
#Else
        Using ms As New MemoryStream
            Call canvas.Image.Save(ms, ImageFormats.Png)
            Call ms.Flush()
            Call ms.Seek(Scan0, SeekOrigin.Begin)

            Return System.Drawing.Image.FromStream(ms)
        End Using
#End If
    End Function
End Module
