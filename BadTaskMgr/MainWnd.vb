Public Class MainWnd

    <DllImport("Shcore.dll")>
    Private Shared Function SetProcessDpiAwareness(ByVal flag As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetDC(ByVal hwnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ReleaseDC(ByVal hwnd As IntPtr, ByVal hdc As IntPtr) As Integer
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function GetDeviceCaps(ByVal hdc As IntPtr, ByVal index As Integer) As Integer
    End Function

    Private g As Graphics
    Friend Shared FrameLength As UInteger
    Private ImgResFolder() As String
    Private SndResFile() As String
    Private bp As Bitmap

    Private WndTaskMgr As WndFinder
    Private MyIniParser As IniParser
    Private ConfigFilePath As String
    Private SelectedWnd As Byte
    Private FrameStart, FrameEnd As UInteger

    Private MinOnStart, MinOnClose As Boolean
    Private EnableBalloon As Boolean
    Private PlaySound As Boolean
    Private Showing As Boolean
    Friend Shared ShowFrameInfo As Boolean

    Private ResNum, SelectedRes As Byte
    Private ResNames() As String
    Private nScale As Single
    
    Private tsmiStyles, tsmiFPS As List(Of ToolStripMenuItem)
    Private MyImageRender As ImageRender
    Private GridSize As Integer

    Friend Shared FPS As Single
    Friend Shared GridOffset As Integer
    Friend Shared FrameIndex As UInteger
    Friend Shared SelectedStyle As Byte
    Friend Shared Styles() As RenderStyle
    Friend Shared WndCanvas As WndFinder.Window
    Friend Shared EnableMT As Boolean
    Friend Shared AutoSync As Boolean

    Private StopState As PlayStop
    Private MyWaveDevice As WaveOut
    Private MyAudioReader As AudioFileReader
    Private SyncNum As Single

    Private WorkThread As Thread
    Private Mtx As Mutex
    Private Const bpCache As Byte = 30
    Private BpQueue As Queue(Of Bitmap)
    Private k As UInteger
    Private InitSuccess As Boolean

    Private WithEvents MyTimer As AccuracyTimer

    Friend Structure RenderStyle
        Public FillColor As Color
        Public EdgeColor As Color
        Public GridColor As Color
    End Structure

    Private Enum PlayStop
        Auto = 0
        Pause = 1
        Excep = 2
        Force = 3
    End Enum
    '======================================================================================


    Private Sub MainWnd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetProcessDpiAwareness(1)
        nScale = GetSystemScale()

        '--------------连接控件事件--------------------
        tsmiStyles = New List(Of ToolStripMenuItem)(4)
        tsmiStyles.Add(tsmiCPU)
        tsmiStyles.Add(tsmiMEM)
        tsmiStyles.Add(tsmiHDD)
        tsmiStyles.Add(tsmiNTW)
        For i = 0 To tsmiStyles.Count - 1
            AddHandler tsmiStyles(i).Click, AddressOf Style_Click
        Next
        tsmiFPS = New List(Of ToolStripMenuItem)(8)
        tsmiFPS.Add(FPS15)
        tsmiFPS.Add(FPS20)
        tsmiFPS.Add(FPS25)
        tsmiFPS.Add(FPS30)
        tsmiFPS.Add(FPS35)
        tsmiFPS.Add(FPS40)
        tsmiFPS.Add(FPS50)
        tsmiFPS.Add(FPS60)
        For i = 0 To tsmiFPS.Count - 1
            AddHandler tsmiFPS(i).Click, AddressOf FPS_Click
        Next

        MyWaveDevice = New WaveOut(Me.Handle)
        MyWaveDevice.Volume = 0.4!
        AddHandler MyWaveDevice.PlaybackStopped, AddressOf PlayStoped

        If MinOnStart Then
            Me.WindowState = FormWindowState.Minimized
        End If

        MyTimer = New AccuracyTimer
        Mtx = New Mutex
        BpQueue = New Queue(Of Bitmap)
        Directory.SetCurrentDirectory(Application.StartupPath)
        ConfigFilePath = ".\config.ini"

        CheckFile()
        LoadConfig()
        UpdateUI()
        InitAudio()
        Initialize()
        InitWorkThread()
        ScaleForHighDpi()
    End Sub

    ''' <summary>
    ''' 检查配置文件
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckFile()
        If Not File.Exists(ConfigFilePath) Then
            File.Create(ConfigFilePath).Close()
            Dim MyIniFile As New IniParser(ConfigFilePath)
            MyIniFile.AddSection("Application")
            MyIniFile.Section("Application").AddKey("FPS", "30")
            MyIniFile.Section("Application").AddKey("ResNum", "1")
            MyIniFile.Section("Application").AddKey("SelectedRes", "0")
            MyIniFile.Section("Application").AddKey("SelectedWnd", "0")
            MyIniFile.Section("Application").AddKey("SelectedStyle", "0")
            MyIniFile.Section("Application").AddKey("EnableMT", "True")
            MyIniFile.Section("Application").AddKey("PlaySound", "True")
            MyIniFile.Section("Application").AddKey("MinOnStart", "False")
            MyIniFile.Section("Application").AddKey("MinOnClose", "True")
            MyIniFile.Section("Application").AddKey("EnableBalloon", "True")
            MyIniFile.Section("Application").AddKey("ShowFrameInfo", "False")
            MyIniFile.Section("Application").AddKey("AutoSync", "True")

            MyIniFile.AddSection("Render")
            MyIniFile.Section("Render").AddKey("FrameStart", "1")
            MyIniFile.Section("Render").AddKey("FrameEnd", "0")

            MyIniFile.AddSection("Style")
            MyIniFile.Section("Style").AddKey("CPU", "241 246 250|17 125 187|217 234 244")
            MyIniFile.Section("Style").AddKey("MEM", "244 242 244|149 40 187|236 222 240")
            MyIniFile.Section("Style").AddKey("HDD", "239 247 233|77 166 12|219 237 206")
            MyIniFile.Section("Style").AddKey("NTW", "252 243 235|167 79 1|238 222 207")

            MyIniFile.AddSection("Resource")
            MyIniFile.Section("Resource").AddKey("Res1", "BadApple")
        End If
    End Sub

    ''' <summary>
    ''' 加载配置
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadConfig()
        MyIniParser = New IniParser(ConfigFilePath)
        Single.TryParse(MyIniParser.Section("Application").Entry("FPS").Value, FPS)
        Byte.TryParse(MyIniParser.Section("Application").Entry("ResNum").Value, ResNum)
        Byte.TryParse(MyIniParser.Section("Application").Entry("SelectedRes").Value, SelectedRes)
        SelectedRes = IIf(SelectedRes > ResNum - 1, 0, SelectedRes)
        Byte.TryParse(MyIniParser.Section("Application").Entry("SelectedWnd").Value, SelectedWnd)
        Byte.TryParse(MyIniParser.Section("Application").Entry("SelectedStyle").Value, SelectedStyle)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("EnableMT").Value, EnableMT)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("PlaySound").Value, PlaySound)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("MinOnClose").Value, MinOnStart)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("MinOnClose").Value, MinOnClose)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("EnableBalloon").Value, EnableBalloon)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("ShowFrameInfo").Value, ShowFrameInfo)
        Boolean.TryParse(MyIniParser.Section("Application").Entry("AutoSync").Value, AutoSync)

        UInteger.TryParse(MyIniParser.Section("Render").Entry("FrameStart").Value, FrameStart)
        UInteger.TryParse(MyIniParser.Section("Render").Entry("FrameEnd").Value, FrameEnd)

        Dim s(), ss() As String
        Dim b(2, 2) As Byte
        ReDim Styles(3)
        s = MyIniParser.Section("Style").Entry("CPU").Value.Split("|")
        For i = 0 To 2
            ss = s(i).Split(" ")
            For j = 0 To 2
                b(i, j) = Byte.Parse(ss(j))
            Next
        Next
        With Styles(0)
            .FillColor = Color.FromArgb(b(0, 0), b(0, 1), b(0, 2))
            .EdgeColor = Color.FromArgb(b(1, 0), b(1, 1), b(1, 2))
            .GridColor = Color.FromArgb(b(2, 0), b(2, 1), b(2, 2))
        End With

        s = MyIniParser.Section("Style").Entry("MEM").Value.Split("|")
        For i = 0 To 2
            ss = s(i).Split(" ")
            For j = 0 To 2
                b(i, j) = Byte.Parse(ss(j))
            Next
        Next
        With Styles(1)
            .FillColor = Color.FromArgb(b(0, 0), b(0, 1), b(0, 2))
            .EdgeColor = Color.FromArgb(b(1, 0), b(1, 1), b(1, 2))
            .GridColor = Color.FromArgb(b(2, 0), b(2, 1), b(2, 2))
        End With

        s = MyIniParser.Section("Style").Entry("HDD").Value.Split("|")
        For i = 0 To 2
            ss = s(i).Split(" ")
            For j = 0 To 2
                b(i, j) = Byte.Parse(ss(j))
            Next
        Next
        With Styles(2)
            .FillColor = Color.FromArgb(b(0, 0), b(0, 1), b(0, 2))
            .EdgeColor = Color.FromArgb(b(1, 0), b(1, 1), b(1, 2))
            .GridColor = Color.FromArgb(b(2, 0), b(2, 1), b(2, 2))
        End With

        s = MyIniParser.Section("Style").Entry("NTW").Value.Split("|")
        For i = 0 To 2
            ss = s(i).Split(" ")
            For j = 0 To 2
                b(i, j) = Byte.Parse(ss(j))
            Next
        Next
        With Styles(3)
            .FillColor = Color.FromArgb(b(0, 0), b(0, 1), b(0, 2))
            .EdgeColor = Color.FromArgb(b(1, 0), b(1, 1), b(1, 2))
            .GridColor = Color.FromArgb(b(2, 0), b(2, 1), b(2, 2))
        End With

        ReDim ResNames(ResNum - 1)
        ReDim ImgResFolder(ResNum - 1)
        ReDim SndResFile(ResNum - 1)
        For i = 0 To ResNum - 1
            ResNames(i) = MyIniParser.Section("Resource").Entry(i).Value
            ImgResFolder(i) = ".\Resource\Image\" + ResNames(i)
            SndResFile(i) = ".\Resource\Audio\" + ResNames(i) + "\" + ResNames(i) + ".m4a"
        Next

        FrameLength = Directory.GetFiles(ImgResFolder(SelectedRes), "*.jpg").Length
        FrameEnd = IIf(FrameEnd = 0 Or FrameEnd > FrameLength, FrameLength, FrameEnd)
        FrameStart = IIf(FrameStart > FrameEnd, FrameEnd, FrameStart)
        MyTimer.Interval = 1000 / FPS
    End Sub

    ''' <summary>
    ''' 更新控件
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateUI()
        For i = 0 To ResNum - 1
            Dim tsmi As New ToolStripMenuItem(ResNames(i))
            tsmi.CheckOnClick = True
            tsmi.Tag = i
            AddHandler tsmi.Click, AddressOf Res_Click
            If i = SelectedRes Then
                tsmi.CheckState = CheckState.Checked
            End If
            miRes.DropDownItems.Add(tsmi)
        Next
        miMusic.Checked = PlaySound
        tsmiStyles(SelectedStyle).CheckState = CheckState.Checked
        For i = 0 To tsmiFPS.Count - 1
            If FPS = tsmiFPS(i).Tag Then
                tsmiFPS(i).CheckState = CheckState.Checked
            End If
        Next
        miFrameInfo.Checked = ShowFrameInfo
        miMT.Checked = EnableMT
        miAutoSync.Checked = AutoSync
    End Sub

    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Initialize()
        If EnableMT Then
            Mtx.WaitOne()
            k = FrameStart
            BpQueue.Clear()
            Mtx.ReleaseMutex()
        Else
            k = FrameStart
            BpQueue.Clear()
        End If
        miStateSwitch.Text = "开始(&B)"
        Showing = False
        FrameIndex = FrameStart
        StopState = PlayStop.Auto
        miWnd.DropDownItems.Clear()
        WndTaskMgr = New WndFinder("TaskManagerWindow", "任务管理器")
        If WndTaskMgr.Handle <> 0 And WndTaskMgr.ChildWindowCount > 0 Then
            SelectedWnd = IIf(SelectedWnd > WndTaskMgr.ChildWindowCount - 1, WndTaskMgr.ChildWindowCount - 1, SelectedWnd)
            For i = 0 To WndTaskMgr.ChildWindowCount - 1
                Dim tsmi As New ToolStripMenuItem("窗口" + (i + 1).ToString)
                tsmi.CheckOnClick = True
                tsmi.Tag = i
                If i = SelectedWnd Then
                    tsmi.CheckState = CheckState.Checked
                End If
                miWnd.DropDownItems.Add(tsmi)
                AddHandler tsmi.Click, AddressOf Wnd_Click
            Next
            CreateGdiContext()
            InitSuccess = True
        Else
            InitSuccess = False
            ShowBalloon("错误", "初始化失败，请先运行任务" + vbNewLine + "管理器并尝试点击刷新")
        End If
    End Sub

    ''' <summary>
    ''' 创建画图环境
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateGdiContext()
        MyTimer.Stop()
        If g IsNot Nothing Then
            g.Dispose()
        End If
        WndCanvas = WndTaskMgr.Child(SelectedWnd)
        GridSize = WndCanvas.Size.Width / 19
        g = Graphics.FromHwnd(WndCanvas.Handle)
    End Sub

    ''' <summary>
    ''' 初始化音频设备
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitAudio()
        If MyWaveDevice.PlaybackState = PlaybackState.Playing Then
            StopState = PlayStop.Force
            MyWaveDevice.Stop()
        End If
        If File.Exists(SndResFile(SelectedRes)) Then
            MyAudioReader = New AudioFileReader(SndResFile(SelectedRes))
            MyWaveDevice.Init(MyAudioReader)
            SyncNum = FrameLength / MyAudioReader.Length
            If AutoSync Then
                FPS = FrameLength * 1000 / MyAudioReader.TotalTime.TotalMilliseconds
                MyTimer.Interval = 1000 / FPS
            End If
        Else
            MyAudioReader = Nothing
        End If
    End Sub

    ''' <summary>
    ''' 播放声音
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Play()
        If MyAudioReader IsNot Nothing Then
            If PlaySound And Showing Then
                MyAudioReader.Position = FrameIndex / FrameLength * MyAudioReader.Length
                MyWaveDevice.Play()
            Else
                If MyWaveDevice.PlaybackState = PlaybackState.Playing Then
                    StopState = PlayStop.Pause
                    MyWaveDevice.Stop()
                End If
            End If
        End If
    End Sub

    '=============================================高DPI缩放=====================================
    Private Function GetSystemScale() As Single
        Dim hdc = GetDC(IntPtr.Zero)
        Dim DpiX As Integer
        DpiX = GetDeviceCaps(hdc, 88)
        ReleaseDC(IntPtr.Zero, hdc)
        Return DpiX / 96
    End Function

    Private Sub ScaleForHighDpi()
        Me.Width *= nScale
        Me.Height *= nScale
        Me.Location = New Point(Me.Location.X * nScale, Me.Location.Y * nScale)
        For Each child As Control In Me.Controls
            child.Font = New Font(child.Font.FontFamily, child.Font.Size * nScale * 0.85)
        Next
        MyContextMenu.Font = New Font(MyContextMenu.Font.FontFamily, MyContextMenu.Font.Size * nScale * 0.85)
    End Sub
    '===============================================DPI========================================

    ''' <summary>
    ''' 绘图
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MyTimer_Tick(sender As Object, e As EventArgs) Handles MyTimer.Tick
        If Process.GetProcessesByName("Taskmgr").Length Then
            Try
                If EnableMT Then
                    If BpQueue.Count Then
                        bp = BpQueue.Dequeue
                        FrameIndex = IIf(FrameIndex < FrameEnd, FrameIndex + 1, FrameStart)
                    End If
                Else
                    bp = Image.FromFile(ImgResFolder(SelectedRes) + "\" + FrameIndex.ToString + ".jpg")
                    MyImageRender = New ImageRender(bp)
                    bp = MyImageRender.Render()
                    If AutoSync And PlaySound Then
                        If FrameIndex < (MyAudioReader.Position * SyncNum + 15) Then
                            FrameIndex += 1
                        End If
                        If FrameIndex > (MyAudioReader.Position * SyncNum - 15) Then
                            FrameIndex -= 1
                        End If
                        FrameIndex = IIf(FrameIndex < FrameEnd, FrameIndex + 1, FrameEnd)
                    Else
                        FrameIndex = IIf(FrameIndex < FrameEnd, FrameIndex + 1, FrameStart)
                    End If
                End If
                g.DrawImage(bp, New Rectangle(1, 1, WndCanvas.Size.Width - 2, WndCanvas.Size.Height - 2), New Rectangle(0, 0, bp.Width, bp.Height), GraphicsUnit.Pixel)
                Notify.Text = "FPS:" + FPS.ToString("0.00") + vbNewLine + "Frame:" + FrameIndex.ToString
            Catch
            End Try
        Else
            g.Dispose()
            Notify.Text = "BadTaskMgr"
            OffsetTimer.Stop()
            MyTimer.Stop()
            InitSuccess = False
            Showing = False
            StopState = PlayStop.Excep
            MyWaveDevice.Stop()
            ShowBalloon("错误", "任务管理器被意外关闭,请重新" + vbNewLine + "运行任务管理器并点击刷新")
        End If
    End Sub


    Private Sub ShowBalloon(ByVal title As String, ByVal content As String)
        If EnableBalloon Then
            Me.Notify.ShowBalloonTip(2000, title, content, ToolTipIcon.Info)
        End If
    End Sub

    Private Sub SaveConfig()
        MyIniParser.Section("Application").Entry("FPS").Value = FPS.ToString("0.00")
        MyIniParser.Section("Application").Entry("PlaySound").Value = PlaySound.ToString
        MyIniParser.Section("Application").Entry("SelectedRes").Value = SelectedRes.ToString
        MyIniParser.Section("Application").Entry("SelectedWnd").Value = SelectedWnd.ToString
        MyIniParser.Section("Application").Entry("SelectedStyle").Value = SelectedStyle.ToString
        MyIniParser.Section("Application").Entry("EnableMT").Value = EnableMT.ToString
        MyIniParser.Section("Application").Entry("ShowFrameInfo").Value = ShowFrameInfo.ToString
        MyIniParser.Section("Application").Entry("AutoSync").Value = AutoSync.ToString
    End Sub

    '===========================控件事件处理====================================
    Private Sub Wnd_Click(sender As Object, e As EventArgs)
        Dim index = DirectCast(sender, ToolStripMenuItem).Tag
        For Each tsmi As ToolStripMenuItem In miWnd.DropDownItems
            If tsmi.Tag <> index Then
                tsmi.CheckState = CheckState.Unchecked
            Else
                tsmi.CheckState = CheckState.Checked
            End If
        Next
        SelectedWnd = index
        CreateGdiContext()
        If Showing Then
            MyTimer.Start()
        End If
    End Sub

    Private Sub Style_Click(sender As Object, e As EventArgs)
        Dim index = DirectCast(sender, ToolStripMenuItem).Tag
        For Each tsmi As ToolStripMenuItem In tsmiStyles
            If tsmi.Tag <> index Then
                tsmi.CheckState = CheckState.Unchecked
            Else
                tsmi.CheckState = CheckState.Checked
            End If
        Next
        SelectedStyle = index
    End Sub

    Private Sub Res_Click(sender As Object, e As EventArgs)
        Dim index = DirectCast(sender, ToolStripMenuItem).Tag
        For Each tsmi As ToolStripMenuItem In miRes.DropDownItems
            If tsmi.Tag <> index Then
                tsmi.CheckState = CheckState.Unchecked
            Else
                tsmi.CheckState = CheckState.Checked
            End If
        Next
        SelectedRes = index
        FrameLength = Directory.GetFiles(ImgResFolder(SelectedRes), "*.jpg").Length
        FrameEnd = IIf(FrameEnd = 0 Or FrameEnd > FrameLength, FrameLength, FrameEnd)
        FrameStart = IIf(FrameStart > FrameEnd, FrameEnd, FrameStart)
        FrameIndex = FrameStart
        InitAudio()
        Play()
    End Sub

    Private Sub FPS_Click(sender As Object, e As EventArgs)
        FPS = DirectCast(sender, ToolStripMenuItem).Tag
        For Each tsmi As ToolStripMenuItem In tsmiFPS
            If tsmi.Tag <> FPS Then
                tsmi.CheckState = CheckState.Unchecked
            Else
                tsmi.CheckState = CheckState.Checked
            End If
        Next
        MyTimer.Interval = 1000 / FPS
    End Sub
    '====================================控件事件=====================================

    '================================事件处理======================================
    Private Sub MainWnd_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If MinOnClose Then
            e.Cancel = True
            Me.WindowState = FormWindowState.Minimized
        Else
            ExitApplication()
        End If
    End Sub

    Private Sub cmiShowMain_Click(sender As Object, e As EventArgs) Handles cmiShowMain.Click
        Me.WindowState = FormWindowState.Normal
        Me.BringToFront()
    End Sub

    Private Sub cmiExit_Click(sender As Object, e As EventArgs) Handles cmiExit.Click
        ExitApplication()
    End Sub

    Private Sub cmiReLoad_Click(sender As Object, e As EventArgs) Handles cmiReLoad.Click
        Initialize()
    End Sub

    Private Sub miExit_Click(sender As Object, e As EventArgs) Handles miExit.Click
        ExitApplication()
    End Sub

    Private Sub miReLoad_Click(sender As Object, e As EventArgs) Handles miReLoad.Click
        Initialize()
    End Sub

    Private Sub miHelp_Click(sender As Object, e As EventArgs) Handles miHelp.Click
        MessageBox.Show("将资源分别放在Resource目录下的Image和" + vbNewLine + "Audio文件夹，并在配置文件的Resource节" + vbNewLine + "下写入文件夹名称。" + vbNewLine + "资源格式仅支持JPG图像和AAC音频", "帮助")
    End Sub

    Private Sub miAbout_Click(sender As Object, e As EventArgs) Handles miAbout.Click
        MessageBox.Show("BadTaskMgr" + vbNewLine + "By: IntPtr.Zero" + vbNewLine + "Release: 2019/05/03" + vbNewLine + "AudioLib: NAudio 1.8.4", "关于")
    End Sub

    Private Sub miMusic_Click(sender As Object, e As EventArgs) Handles miMusic.Click
        PlaySound = Not PlaySound
        Play()
    End Sub

    Private Sub PlayStoped(sender As Object, e As StoppedEventArgs)
        Select Case StopState
            Case PlayStop.Auto
                If AutoSync And Not EnableMT Then
                    FrameIndex = FrameStart
                End If
                MyAudioReader.Position = 0
                MyWaveDevice.Play()
            Case PlayStop.Pause
                Exit Select
            Case PlayStop.Excep
                MyAudioReader.Position = 0
            Case PlayStop.Force
                MyAudioReader.Dispose()
                MyAudioReader = Nothing
        End Select
        StopState = PlayStop.Auto
    End Sub

    Private Sub ExitApplication()
        SaveConfig()
        If MyAudioReader IsNot Nothing Then
            StopState = PlayStop.Force
            MyWaveDevice.Stop()
            MyWaveDevice.Dispose()
        End If
        Me.Notify.Dispose()
        MyTimer.Dispose()
        End
    End Sub

    Private Sub tsmiFPSDown_Click(sender As Object, e As EventArgs) Handles tsmiFPSDown.Click
        For i = 0 To tsmiFPS.Count - 1
            tsmiFPS(i).CheckState = CheckState.Unchecked
        Next
        FPS = IIf(FPS > 15, FPS - 0.5F, 15)
        MyTimer.Interval = 1000 / FPS
    End Sub

    Private Sub tsmiFPSUp_Click(sender As Object, e As EventArgs) Handles tsmiFPSUp.Click
        For i = 0 To tsmiFPS.Count - 1
            tsmiFPS(i).CheckState = CheckState.Unchecked
        Next
        FPS = IIf(FPS < 60, FPS + 0.5F, 60)
        MyTimer.Interval = 1000 / FPS
    End Sub

    Private Sub OffsetTimer_Tick(sender As Object, e As EventArgs) Handles OffsetTimer.Tick
        GridOffset = IIf(GridOffset < GridSize - 3, GridOffset + 4, GridSize - GridOffset)
    End Sub

    Private Sub miStateSwitch_Click(sender As Object, e As EventArgs) Handles miStateSwitch.Click
        If InitSuccess Then
            Showing = Not Showing
            If Showing Then
                MyTimer.Start()
                OffsetTimer.Start()
            Else
                MyTimer.Stop()
                OffsetTimer.Stop()
            End If
            Play()
            miStateSwitch.Text = IIf(Showing, "暂停(&P)", "开始(&B)")
        End If
    End Sub

    Private Sub tsmiVolumDpwn_Click(sender As Object, e As EventArgs) Handles tsmiVolumDpwn.Click
        If MyWaveDevice.Volume >= 0.1F Then
            MyWaveDevice.Volume -= 0.1F
        End If
    End Sub

    Private Sub tsmiVolumUp_Click(sender As Object, e As EventArgs) Handles tsmiVolumUp.Click
        If MyWaveDevice.Volume <= 0.9F Then
            MyWaveDevice.Volume += 0.1F
        End If
    End Sub

    Private Sub miFrameInfo_Click(sender As Object, e As EventArgs) Handles miFrameInfo.Click
        ShowFrameInfo = Not ShowFrameInfo
    End Sub

    Private Sub miMT_Click(sender As Object, e As EventArgs) Handles miMT.Click
        EnableMT = Not EnableMT
        If EnableMT Then
            k = FrameIndex
            Mtx.ReleaseMutex()
        Else
            Mtx.WaitOne()
            BpQueue.Clear()
        End If
    End Sub

    Private Sub miAutoSync_Click(sender As Object, e As EventArgs) Handles miAutoSync.Click
        AutoSync = Not AutoSync
        If AutoSync And MyAudioReader IsNot Nothing Then
            FPS = FrameLength * 1000 / MyAudioReader.TotalTime.TotalMilliseconds
            MyTimer.Interval = 1000 / FPS
            For Each tsmi In tsmiFPS
                tsmi.CheckState = CheckState.Unchecked
            Next
        End If
    End Sub





    '==============================多线程=====================================
    Private Sub InitWorkThread()
        If Not EnableMT Then
            Mtx.WaitOne()
        End If
        WorkThread = New Thread(AddressOf ThreadProc)
        WorkThread.IsBackground = True
        WorkThread.Start()
    End Sub

    Private Sub ThreadProc()
        Dim tbp As Bitmap
        Dim tImageRender As ImageRender
        While True
            If InitSuccess Then
                Mtx.WaitOne()
                If BpQueue.Count > bpCache Then
                    Mtx.ReleaseMutex()
                    Thread.Sleep(200)
                Else
                    tbp = Image.FromFile(ImgResFolder(SelectedRes) + "\" + k.ToString + ".jpg")
                    tImageRender = New ImageRender(tbp)
                    BpQueue.Enqueue(tImageRender.Render)
                    k = IIf(k < FrameEnd, k + 1, FrameStart)
                    Mtx.ReleaseMutex()
                End If
            Else
                Thread.Sleep(500)
            End If
        End While
    End Sub

End Class
