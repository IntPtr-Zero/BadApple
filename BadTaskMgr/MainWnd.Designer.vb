<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWnd
    Inherits System.Windows.Forms.Form
    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainWnd))
        Me.SMenu = New System.Windows.Forms.MenuStrip()
        Me.miSetting = New System.Windows.Forms.ToolStripMenuItem()
        Me.miReLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.miWnd = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFPS = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFPSDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS15 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS20 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS25 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS35 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS40 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS50 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FPS60 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFPSUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.miStyle = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiCPU = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMEM = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiHDD = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiNTW = New System.Windows.Forms.ToolStripMenuItem()
        Me.miRes = New System.Windows.Forms.ToolStripMenuItem()
        Me.miMusic = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiVolumDpwn = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiVolumUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFrameInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.miMT = New System.Windows.Forms.ToolStripMenuItem()
        Me.miAutoSync = New System.Windows.Forms.ToolStripMenuItem()
        Me.miStateSwitch = New System.Windows.Forms.ToolStripMenuItem()
        Me.miHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.miAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.miExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.Notify = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.MyContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmiReLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmiShowMain = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmiExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.OffsetTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SMenu.SuspendLayout()
        Me.MyContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'SMenu
        '
        Me.SMenu.BackColor = System.Drawing.Color.Transparent
        Me.SMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.SMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miSetting, Me.miStateSwitch, Me.miHelp, Me.miAbout, Me.miExit})
        Me.SMenu.Location = New System.Drawing.Point(0, 0)
        Me.SMenu.Name = "SMenu"
        Me.SMenu.Size = New System.Drawing.Size(601, 28)
        Me.SMenu.TabIndex = 0
        Me.SMenu.Text = "MenuStrip1"
        '
        'miSetting
        '
        Me.miSetting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miReLoad, Me.miWnd, Me.miFPS, Me.miStyle, Me.miRes, Me.miMusic, Me.miFrameInfo, Me.miMT, Me.miAutoSync})
        Me.miSetting.Name = "miSetting"
        Me.miSetting.Size = New System.Drawing.Size(70, 24)
        Me.miSetting.Text = "设置(&S)"
        '
        'miReLoad
        '
        Me.miReLoad.Name = "miReLoad"
        Me.miReLoad.Size = New System.Drawing.Size(181, 26)
        Me.miReLoad.Text = "刷新(&R)"
        '
        'miWnd
        '
        Me.miWnd.Name = "miWnd"
        Me.miWnd.Size = New System.Drawing.Size(181, 26)
        Me.miWnd.Text = "窗口(&W)"
        '
        'miFPS
        '
        Me.miFPS.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFPSDown, Me.FPS15, Me.FPS20, Me.FPS25, Me.FPS30, Me.FPS35, Me.FPS40, Me.FPS50, Me.FPS60, Me.tsmiFPSUp})
        Me.miFPS.Name = "miFPS"
        Me.miFPS.Size = New System.Drawing.Size(181, 26)
        Me.miFPS.Text = "帧率(F)"
        '
        'tsmiFPSDown
        '
        Me.tsmiFPSDown.Name = "tsmiFPSDown"
        Me.tsmiFPSDown.Size = New System.Drawing.Size(132, 26)
        Me.tsmiFPSDown.Text = "FPS--"
        '
        'FPS15
        '
        Me.FPS15.CheckOnClick = True
        Me.FPS15.Name = "FPS15"
        Me.FPS15.Size = New System.Drawing.Size(132, 26)
        Me.FPS15.Tag = "15"
        Me.FPS15.Text = "15 FPS"
        '
        'FPS20
        '
        Me.FPS20.CheckOnClick = True
        Me.FPS20.Name = "FPS20"
        Me.FPS20.Size = New System.Drawing.Size(132, 26)
        Me.FPS20.Tag = "20"
        Me.FPS20.Text = "20 FPS"
        '
        'FPS25
        '
        Me.FPS25.CheckOnClick = True
        Me.FPS25.Name = "FPS25"
        Me.FPS25.Size = New System.Drawing.Size(132, 26)
        Me.FPS25.Tag = "25"
        Me.FPS25.Text = "25 FPS"
        '
        'FPS30
        '
        Me.FPS30.CheckOnClick = True
        Me.FPS30.Name = "FPS30"
        Me.FPS30.Size = New System.Drawing.Size(132, 26)
        Me.FPS30.Tag = "30"
        Me.FPS30.Text = "30 FPS"
        '
        'FPS35
        '
        Me.FPS35.CheckOnClick = True
        Me.FPS35.Name = "FPS35"
        Me.FPS35.Size = New System.Drawing.Size(132, 26)
        Me.FPS35.Tag = "35"
        Me.FPS35.Text = "35 FPS"
        '
        'FPS40
        '
        Me.FPS40.CheckOnClick = True
        Me.FPS40.Name = "FPS40"
        Me.FPS40.Size = New System.Drawing.Size(132, 26)
        Me.FPS40.Tag = "40"
        Me.FPS40.Text = "40 FPS"
        '
        'FPS50
        '
        Me.FPS50.CheckOnClick = True
        Me.FPS50.Name = "FPS50"
        Me.FPS50.Size = New System.Drawing.Size(132, 26)
        Me.FPS50.Tag = "50"
        Me.FPS50.Text = "50 FPS"
        '
        'FPS60
        '
        Me.FPS60.CheckOnClick = True
        Me.FPS60.Name = "FPS60"
        Me.FPS60.Size = New System.Drawing.Size(132, 26)
        Me.FPS60.Tag = "60"
        Me.FPS60.Text = "60 FPS"
        '
        'tsmiFPSUp
        '
        Me.tsmiFPSUp.Name = "tsmiFPSUp"
        Me.tsmiFPSUp.Size = New System.Drawing.Size(132, 26)
        Me.tsmiFPSUp.Text = "FPS++"
        '
        'miStyle
        '
        Me.miStyle.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiCPU, Me.tsmiMEM, Me.tsmiHDD, Me.tsmiNTW})
        Me.miStyle.Name = "miStyle"
        Me.miStyle.Size = New System.Drawing.Size(181, 26)
        Me.miStyle.Text = "样式(&S)"
        '
        'tsmiCPU
        '
        Me.tsmiCPU.CheckOnClick = True
        Me.tsmiCPU.Name = "tsmiCPU"
        Me.tsmiCPU.Size = New System.Drawing.Size(114, 26)
        Me.tsmiCPU.Tag = "0"
        Me.tsmiCPU.Text = "CPU"
        '
        'tsmiMEM
        '
        Me.tsmiMEM.CheckOnClick = True
        Me.tsmiMEM.Name = "tsmiMEM"
        Me.tsmiMEM.Size = New System.Drawing.Size(114, 26)
        Me.tsmiMEM.Tag = "1"
        Me.tsmiMEM.Text = "内存"
        '
        'tsmiHDD
        '
        Me.tsmiHDD.CheckOnClick = True
        Me.tsmiHDD.Name = "tsmiHDD"
        Me.tsmiHDD.Size = New System.Drawing.Size(114, 26)
        Me.tsmiHDD.Tag = "2"
        Me.tsmiHDD.Text = "磁盘"
        '
        'tsmiNTW
        '
        Me.tsmiNTW.CheckOnClick = True
        Me.tsmiNTW.Name = "tsmiNTW"
        Me.tsmiNTW.Size = New System.Drawing.Size(114, 26)
        Me.tsmiNTW.Tag = "3"
        Me.tsmiNTW.Text = "网络"
        '
        'miRes
        '
        Me.miRes.Name = "miRes"
        Me.miRes.Size = New System.Drawing.Size(181, 26)
        Me.miRes.Text = "资源(&R)"
        '
        'miMusic
        '
        Me.miMusic.Checked = True
        Me.miMusic.CheckOnClick = True
        Me.miMusic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.miMusic.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiVolumDpwn, Me.tsmiVolumUp})
        Me.miMusic.Name = "miMusic"
        Me.miMusic.Size = New System.Drawing.Size(181, 26)
        Me.miMusic.Text = "声音(&M)"
        '
        'tsmiVolumDpwn
        '
        Me.tsmiVolumDpwn.Name = "tsmiVolumDpwn"
        Me.tsmiVolumDpwn.Size = New System.Drawing.Size(125, 26)
        Me.tsmiVolumDpwn.Text = "音量-"
        '
        'tsmiVolumUp
        '
        Me.tsmiVolumUp.Name = "tsmiVolumUp"
        Me.tsmiVolumUp.Size = New System.Drawing.Size(125, 26)
        Me.tsmiVolumUp.Text = "音量+"
        '
        'miFrameInfo
        '
        Me.miFrameInfo.CheckOnClick = True
        Me.miFrameInfo.Name = "miFrameInfo"
        Me.miFrameInfo.Size = New System.Drawing.Size(181, 26)
        Me.miFrameInfo.Text = "帧信息(&I)"
        '
        'miMT
        '
        Me.miMT.CheckOnClick = True
        Me.miMT.Name = "miMT"
        Me.miMT.Size = New System.Drawing.Size(181, 26)
        Me.miMT.Text = "多线程(&T)"
        '
        'miAutoSync
        '
        Me.miAutoSync.CheckOnClick = True
        Me.miAutoSync.Name = "miAutoSync"
        Me.miAutoSync.Size = New System.Drawing.Size(181, 26)
        Me.miAutoSync.Text = "自同步(&A)"
        '
        'miStateSwitch
        '
        Me.miStateSwitch.Name = "miStateSwitch"
        Me.miStateSwitch.Size = New System.Drawing.Size(70, 24)
        Me.miStateSwitch.Text = "开始(&B)"
        '
        'miHelp
        '
        Me.miHelp.Name = "miHelp"
        Me.miHelp.Size = New System.Drawing.Size(73, 24)
        Me.miHelp.Text = "帮助(&H)"
        '
        'miAbout
        '
        Me.miAbout.Name = "miAbout"
        Me.miAbout.Size = New System.Drawing.Size(72, 24)
        Me.miAbout.Text = "关于(&A)"
        '
        'miExit
        '
        Me.miExit.Name = "miExit"
        Me.miExit.Size = New System.Drawing.Size(69, 24)
        Me.miExit.Text = "退出(&E)"
        '
        'Notify
        '
        Me.Notify.ContextMenuStrip = Me.MyContextMenu
        Me.Notify.Icon = CType(resources.GetObject("Notify.Icon"), System.Drawing.Icon)
        Me.Notify.Text = "BadTaskMgr"
        Me.Notify.Visible = True
        '
        'MyContextMenu
        '
        Me.MyContextMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MyContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmiReLoad, Me.cmiShowMain, Me.cmiExit})
        Me.MyContextMenu.Name = "MyContextMenu"
        Me.MyContextMenu.Size = New System.Drawing.Size(139, 76)
        '
        'cmiReLoad
        '
        Me.cmiReLoad.Name = "cmiReLoad"
        Me.cmiReLoad.Size = New System.Drawing.Size(138, 24)
        Me.cmiReLoad.Text = "刷新"
        '
        'cmiShowMain
        '
        Me.cmiShowMain.Name = "cmiShowMain"
        Me.cmiShowMain.Size = New System.Drawing.Size(138, 24)
        Me.cmiShowMain.Text = "主界面"
        '
        'cmiExit
        '
        Me.cmiExit.Name = "cmiExit"
        Me.cmiExit.Size = New System.Drawing.Size(138, 24)
        Me.cmiExit.Text = "退出程序"
        '
        'OffsetTimer
        '
        Me.OffsetTimer.Interval = 1000
        '
        'MainWnd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(601, 429)
        Me.Controls.Add(Me.SMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.SMenu
        Me.MaximizeBox = False
        Me.Name = "MainWnd"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BadTaskMgr"
        Me.SMenu.ResumeLayout(False)
        Me.SMenu.PerformLayout()
        Me.MyContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents miSetting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Notify As System.Windows.Forms.NotifyIcon
    Friend WithEvents MyContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmiReLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiShowMain As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmiExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miReLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miWnd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miMusic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miStyle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiCPU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiMEM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiHDD As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiNTW As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miRes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFPS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS15 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS20 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS25 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS30 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS35 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS40 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFPSDown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFPSUp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OffsetTimer As System.Windows.Forms.Timer
    Friend WithEvents FPS50 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FPS60 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiVolumDpwn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiVolumUp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFrameInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miAutoSync As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miStateSwitch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miMT As System.Windows.Forms.ToolStripMenuItem

End Class
