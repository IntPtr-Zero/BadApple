Public Class WndFinder

    <DllImport("user32.dll")> _
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWndName As String) As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function EnumChildWindows(ByVal hwndParent As IntPtr, ByVal lpEnumFunc As WndEnumProc, ByVal lParam As Integer) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function GetWindowRect(ByVal hwnd As IntPtr, ByRef rect As Rect) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function GetClassName(ByVal hwnd As IntPtr, ByVal lpClassName As Byte(), ByVal nMaxCount As Integer) As Integer
    End Function

    Private Delegate Function WndEnumProc(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Boolean

    Private Structure Rect
        Dim Left As Integer
        Dim Top As Integer
        Dim Right As Integer
        Dim Bottom As Integer
    End Structure

    Public Structure Window
        Public Handle As IntPtr
        Public Size As Size
    End Structure

    Private Buffer(255) As Byte
    Private MyString As String
    Private nBytes, nChilds As Integer
    Private w, h As Integer
    Private WndHandle As IntPtr
    Private WndSize As Size
    Private MyRect As Rect
    Private ChildWnds() As Window
    Private EnumProc As WndEnumProc

    Public ReadOnly Property Handle As IntPtr
        Get
            Return WndHandle
        End Get
    End Property

    Public ReadOnly Property Size As Size
        Get
            Return WndSize
        End Get
    End Property

    Public ReadOnly Property Child(ByVal index As Integer) As Window
        Get
            If ChildWnds IsNot Nothing Then
                If index >= 0 And index < ChildWnds.Length Then
                    Return ChildWnds(index)
                End If
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property ChildWindowCount As Integer
        Get
            Return nChilds
        End Get
    End Property

    Public Sub New(ByVal ClassName As String, Optional ByVal WndName As String = vbNullString)
        WndHandle = FindWindow(ClassName, WndName)
        If WndHandle Then
            GetWindowRect(WndHandle, MyRect)
            WndSize = New Size(MyRect.Right - MyRect.Left, MyRect.Bottom - MyRect.Top)
            EnumProc = New WndEnumProc(AddressOf EnumChildProc)
            EnumChildWindows(WndHandle, EnumProc, 0)
        End If
    End Sub

    Private Function EnumChildProc(ByVal hwnd As IntPtr, ByVal lParam As Integer) As Boolean
        MyRect = New Rect
        ReDim Buffer(255)
        MyString = vbNullString
        nBytes = GetClassName(hwnd, Buffer, 256)
        MyString = Encoding.Default.GetString(Buffer, 0, nBytes)
        If MyString = "CvChartWindow" Then
            GetWindowRect(hwnd, MyRect)
            w = MyRect.Right - MyRect.Left
            h = MyRect.Bottom - MyRect.Top
            ReDim Preserve ChildWnds(nChilds)
            ChildWnds(nChilds).Handle = hwnd
            ChildWnds(nChilds).Size = New Size(w, h)
            nChilds += 1
        End If
        Return True
    End Function

End Class
