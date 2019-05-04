Imports BadTaskMgr.MainWnd
Public Class ImageRender

    Public Sub New(ByVal bp As Bitmap)
        Bmp = bp
        Style = Styles(SelectedStyle)
        Width = WndCanvas.Size.Width
        Height = WndCanvas.Size.Height
        wGrid = Width / LineX
        hGrid = Height / LineY
        GridPen = New Pen(Style.GridColor, 1.0!)
        ZoomRateMin = Math.Min(Width / Bmp.Width, Height / Bmp.Height)
        ZoomRateMax = Math.Max(Width / Bmp.Width, Height / Bmp.Height)
        MyFont = New Font("微软雅黑", 20.0F * ZoomRateMin)
    End Sub

    Private GrayMap() As UInteger
    Private Bmp As Bitmap
    Private Style As RenderStyle
    Private GridPen As Pen
    Private bpData As BitmapData
    Private Buffer() As Byte
    Private Stride As Integer
    Private Bytes As UInteger
    Private Width, Height As Integer
    Private Const LineX As Byte = 19
    Private Const LineY As Byte = 9
    Private wGrid, hGrid As Integer
    Private MyFont As Font
    Private ZoomRateMin, ZoomRateMax As Single
    Private FontSizeF As SizeF

    Public Function Render() As Bitmap
        Bmp = ImageResize()
        bpData = Bmp.LockBits(New Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
        Stride = Math.Abs(bpData.Stride)
        Bytes = Stride * bpData.Height
        ReDim Buffer(Bytes - 1)
        Marshal.Copy(bpData.Scan0, Buffer, 0, Bytes)
        ImageGray()
        ImageBinaryzation(Color.White, Style.FillColor)
        ImageEdgeHighLight(Style.EdgeColor)
        Marshal.Copy(Buffer, 0, bpData.Scan0, Bytes)
        Bmp.UnlockBits(bpData)
        Return ImageFitRectangle()
    End Function

    Private Function ImageResize() As Bitmap
        Dim bp As Bitmap
        If Width >= Bmp.Width And Height >= Bmp.Height Then
            Return Bmp
        End If
        Dim w, h As Integer
        If Width <= 200 And Height <= 200 Then
            w = Bmp.Width * ZoomRateMax : h = Bmp.Height * ZoomRateMax
        Else
            w = Bmp.Width * ZoomRateMin : h = Bmp.Height * ZoomRateMin
        End If
        bp = New Bitmap(w, h)
        Dim g As Graphics = Graphics.FromImage(bp)
        g.DrawImage(Bmp, New Rectangle(0, 0, w, h), New Rectangle(0, 0, Bmp.Width, Bmp.Height), GraphicsUnit.Pixel)
        g.Dispose()
        Return bp
    End Function

    Private Sub ImageGray()
        ReDim GrayMap(255)
        Dim r, g, b As Single
        Dim gray As Byte
        For i = 0 To Bytes - 1 Step 4
            b = Buffer(i)
            g = Buffer(i + 1)
            r = Buffer(i + 2)
            gray = 0.299 * r + 0.578 * g + 0.114 * b
            GrayMap(gray) += 1
            Buffer(i) = gray : Buffer(i + 1) = gray : Buffer(i + 2) = gray
        Next
    End Sub

    Private Sub ImageBinaryzation(ByVal Color1 As Color, ByVal Color2 As Color)
        Dim value As Byte
        Dim sum As UInt64
        Dim n As UInt32
        For i = 0 To 255
            n += GrayMap(i)
            sum += i * GrayMap(i)
        Next
        value = sum / n
        For i = 0 To Bytes - 1 Step 4
            If Buffer(i) >= value Then
                Buffer(i) = Color1.B
                Buffer(i + 1) = Color1.G
                Buffer(i + 2) = Color1.R
            Else
                Buffer(i) = Color2.B
                Buffer(i + 1) = Color2.G
                Buffer(i + 2) = Color2.R
            End If
        Next
    End Sub

    Private Sub ImageEdgeHighLight(ByVal HighLightColor As Color)
        Dim BufferCopy() As Byte
        Dim y As Integer
        BufferCopy = Buffer.Clone
        For i = 0 To Bytes - 1 Step 4
            y = i \ Stride + 1
            If i + 4 < (y * Stride) AndAlso BufferCopy(i) <> BufferCopy(i + 4) Then
                Buffer(i + 4) = HighLightColor.B
                Buffer(i + 5) = HighLightColor.G
                Buffer(i + 6) = HighLightColor.R
            End If

            If y < bpData.Height AndAlso BufferCopy(i) <> BufferCopy(i + Stride) Then
                Buffer(i + Stride) = HighLightColor.B
                Buffer(i + Stride + 1) = HighLightColor.G
                Buffer(i + Stride + 2) = HighLightColor.R
            End If
        Next
    End Sub

    Public Function ImageFitRectangle() As Bitmap
        Dim bp As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(bp)
        g.Clear(Color.White)
        If Width > 200 Then
            g.DrawImage(Bmp, New Rectangle((bp.Width - Bmp.Width) / 2, (bp.Height - Bmp.Height) / 2, Bmp.Width, Bmp.Height))
            For i = 0 To LineX
                g.DrawLine(GridPen, wGrid * (i + 1) - GridOffset, 0, wGrid * (i + 1) - GridOffset, Height)
            Next
            For i = 0 To LineY
                g.DrawLine(GridPen, 1, hGrid * (i + 1), Width - 1, hGrid * (i + 1))
            Next
            If ShowFrameInfo Then
                FontSizeF = g.MeasureString("F", MyFont)
                g.DrawString("FPS:" + FPS.ToString("0.00"), MyFont, New SolidBrush(GridPen.Color), 1, 1)
                g.DrawString("多线程:" + IIf(EnableMT, "ON", "OFF"), MyFont, New SolidBrush(GridPen.Color), 1, FontSizeF.Height - 2)
                g.DrawString("自同步:" + IIf(AutoSync, "ON", "OFF"), MyFont, New SolidBrush(GridPen.Color), 1, FontSizeF.Height * 2 - 2)
                g.DrawString(FrameIndex.ToString + "/" + FrameLength.ToString, MyFont, New SolidBrush(GridPen.Color), 1, bp.Height - FontSizeF.Height)
            End If
        Else
            g.DrawImage(Bmp, New Rectangle(0, 0, bp.Width, bp.Height), New Rectangle(Math.Abs(Bmp.Width - bp.Width) / 2, Math.Abs(Bmp.Height - bp.Height) / 2, bp.Width, bp.Height), GraphicsUnit.Pixel)
        End If
        g.Dispose()
        Return bp
    End Function
End Class
