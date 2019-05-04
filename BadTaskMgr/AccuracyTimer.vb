Public NotInheritable Class AccuracyTimer
    Implements IDisposable

    <DllImport("winmm.dll")>
    Private Shared Function timeGetDevCaps(ByRef LPTIMECAPS As TimeCaps, ByVal cbSize As Integer) As Integer
    End Function

    <DllImport("winmm.dll")>
    Private Shared Function timeBeginPeriod(ByVal uPeriod As UInteger) As Integer
    End Function

    <DllImport("winmm.dll")>
    Private Shared Function timeSetEvent(ByVal uDelay As UInteger, ByVal uResolution As UInteger, ByVal lpTimeProc As TimeProc, ByRef lpdwuser As UInteger, ByVal fuEvebt As UInteger) As UInteger
    End Function

    <DllImport("winmm.dll")>
    Private Shared Function timeKillEvent(ByVal uTimerID As UInteger) As Integer
    End Function

    <DllImport("winmm.dll")>
    Private Shared Function timeEndPeriod(ByVal uPeriod As UInteger) As Integer
    End Function

    Private Delegate Sub TimeProc(ByVal uId As UInteger, ByVal uMsg As UInteger, ByVal dwUser As UInteger, ByVal dw1 As UInteger, ByVal dw2 As UInteger)

    <StructLayout(LayoutKind.Sequential)>
    Private Structure TimeCaps
        Public PeriodMin As UInteger
        Public PeriodMax As UInteger
    End Structure


    Private Shared caps As TimeCaps
    Private itv As UInteger
    Private resolution As UInteger
    Private running As Boolean
    Private TimerCallBack As TimeProc
    Private timerID As UInteger
    Public Event Tick As EventHandler


    Shared Sub New()
        timeGetDevCaps(caps, Marshal.SizeOf(caps))
    End Sub

    Public Sub New()
        itv = caps.PeriodMax / 2
        resolution = caps.PeriodMin
        running = False
        TimerCallBack = New TimeProc(AddressOf TimerEvent)
    End Sub

    Public Property Interval As UInteger
        Get
            Return itv
        End Get
        Set(value As UInteger)
            If value >= caps.PeriodMin And value <= caps.PeriodMax Then
                itv = value
                If running Then
                    timeKillEvent(timerID)
                    timerID = timeSetEvent(itv, resolution, TimerCallBack, 0, 1)
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property PeriodMin As UInteger
        Get
            Return caps.PeriodMin
        End Get
    End Property

    Public ReadOnly Property PeriodMax As UInteger
        Get
            Return caps.PeriodMax
        End Get
    End Property

    Public ReadOnly Property IsRunning As Boolean
        Get
            Return running
        End Get
    End Property

    Private Sub TimerEvent(ByVal uId As UInteger, ByVal uMsg As UInteger, ByVal dwUser As UInteger, ByVal dw1 As UInteger, ByVal dw2 As UInteger)
        RaiseEvent Tick(Me, Nothing)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        timeKillEvent(timerID)
    End Sub

    Public Sub Start()
        If Not running Then
            timerID = timeSetEvent(itv, resolution, TimerCallBack, 0, 1)
            running = True
        End If
    End Sub

    Public Sub [Stop]()
        If running Then
            timeKillEvent(timerID)
            running = False
        End If
    End Sub

End Class
