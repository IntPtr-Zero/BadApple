Public Class IniParser

    ''' <summary>
    ''' Retrieves a string from the specified section in an initialization file.
    ''' </summary>
    ''' <param name="szSection">The name of the section containing the key name. If this parameter is NULL, the GetPrivateProfileString function copies all section names in the file to the supplied buffer</param>
    ''' <param name="szKey">The name of the key whose associated string is to be retrieved. If this parameter is NULL, all key names in the section specified by the lpAppName parameter are copied to the buffer </param>
    ''' <param name="szDefault">A default string. If the lpKeyName key cannot be found in the initialization file, GetPrivateProfileString copies the default string to the buffer. If this parameter is NULL, the default is an empty string, "".</param>
    ''' <param name="bReturn">A pointer to the buffer that receives the retrieved string</param>
    ''' <param name="nSize">The size of the buffer, in characters</param>
    ''' <param name="szPath">The name of the initialization file</param>
    ''' <returns>The return value is the number of characters copied to the buffer, not including the terminating null character</returns>
    ''' <remarks>If lpAppName is NULL, GetPrivateProfileString copies all section names in the specified file to the supplied buffer. If lpKeyName is NULL, the function copies all key names in the specified section to the supplied buffer. An application can use this method to enumerate all of the sections and keys in a file. In either case, each string is followed by a null character and the final string is followed by a second null character. If the supplied destination buffer is too small to hold all the strings, the last string is truncated and followed by two null characters</remarks>
    <DllImport("KERNEL32.DLL")> _
    Private Shared Function GetPrivateProfileString( _
                                                  ByVal szSection As String, _
                                                  ByVal szKey As String, _
                                                  ByVal szDefault As String, _
                                                  ByVal bReturn() As Byte, _
                                                  ByVal nSize As Int32, _
                                                  ByVal szPath As String _
                                                   ) As Int32
    End Function

    ''' <summary>
    ''' Copies a string into the specified section of an initialization file
    ''' </summary>
    ''' <param name="szSection">The name of the section to which the string will be copied. If the section does not exist, it is created. The name of the section is case-independent; the string can be any combination of uppercase and lowercase letters</param>
    ''' <param name="szKey">The name of the key to be associated with a string. If the key does not exist in the specified section, it is created. If this parameter is NULL, the entire section, including all entries within the section, is deleted</param>
    ''' <param name="szWritten">A null-terminated string to be written to the file. If this parameter is NULL, the key is deleted</param>
    ''' <param name="szPath">The name of the initialization file</param>
    ''' <returns>If the function successfully copies the string to the initialization file, the return value is nonzero</returns>
    ''' <remarks></remarks>
    <DllImport("KERNEL32.DLL")> _
    Private Shared Function WritePrivateProfileString( _
                                                     ByVal szSection As String, _
                                                     ByVal szKey As String, _
                                                     ByVal szWritten As String, _
                                                     ByVal szPath As String _
                                                     ) As Boolean
    End Function

    ''' <summary>
    ''' 字节缓冲区长度
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxCharLen = &H7FFF
    Private Shared IniPath As String
    Private Sections As List(Of String)

    ''' <summary>
    ''' 节的数量
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SectionCounts As Integer
        Get
            Return Sections.Count
        End Get
    End Property

    ''' <summary>
    ''' 指定节
    ''' </summary>
    ''' <param name="Name">节名称</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Section(ByVal szName As String) As IniSection
        Get
            Return New IniSection(Me, szName)
        End Get
    End Property

    ''' <summary>
    ''' 指定节
    ''' </summary>
    ''' <param name="Index">节在文档中的次序，从0开始</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Section(ByVal Index As Integer) As IniSection
        Get
            Return New IniSection(Me, Index)
        End Get
    End Property

    Public Sub New(ByVal szPath As String)
        If File.Exists(szPath) And Path.GetExtension(szPath).ToLower = ".ini" Then
            IniPath = szPath
            Dim szReturn As String = Read(vbNullString, vbNullString)
            Sections = New List(Of String)
            If szReturn <> vbNullString Then
                Sections.AddRange(szReturn.Split(vbNullChar))
            End If
        Else
            Throw New FileNotFoundException("Invalid File!")
        End If
    End Sub

    ''' <summary>
    ''' 插入节
    ''' </summary>
    ''' <param name="szSection"></param>
    ''' <param name="szKey"></param>
    ''' <param name="szValue"></param>
    ''' <remarks></remarks>
    Public Sub AddSection(ByVal szSection As String, Optional ByVal szKey As String = "", Optional ByVal szValue As String = vbNullString)
        If (Not Sections.Contains(szSection)) And szSection <> vbNullString Then
            If Write(szSection, szKey, szValue) Then
                Sections.Add(szSection)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 删除节
    ''' </summary>
    ''' <param name="szSection">节名称</param>
    ''' <remarks></remarks>
    Public Sub RemoveSection(ByVal szSection As String)
        If Sections.Contains(szSection) Then
            If Write(szSection, vbNullString, vbNullString) Then
                Sections.Remove(szSection)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 删除节
    ''' </summary>
    ''' <param name="Index">节在文档中的次序，从0开始</param>
    ''' <remarks></remarks>
    Public Sub RemoveSection(ByVal Index As Integer)
        If Index >= 0 And Index <= Sections.Count - 1 Then
            If Write(Sections(Index), vbNullString, vbNullString) Then
                Sections.RemoveAt(Index)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 读取
    ''' </summary>
    ''' <param name="szSection"></param>
    ''' <param name="szKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function Read(ByVal szSection As String, ByVal szKey As String) As String
        Dim szReturn As String = vbNullString
        Dim nReturn As Int32 = 0
        Dim bReturn(MaxCharLen - 1) As Byte
        nReturn = GetPrivateProfileString(szSection, szKey, "", bReturn, MaxCharLen, IniPath)
        If nReturn Then
            If szSection <> vbNullString And szKey <> vbNullString Then
                szReturn = Encoding.Default.GetString(bReturn, 0, nReturn)
            Else
                szReturn = Encoding.Default.GetString(bReturn, 0, nReturn - 1)
            End If
        End If
        Return szReturn
    End Function

    ''' <summary>
    ''' 写入
    ''' </summary>
    ''' <param name="szSection"></param>
    ''' <param name="szKey"></param>
    ''' <param name="szWritten"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function Write(ByVal szSection As String, ByVal szKey As String, ByVal szWritten As String) As Boolean
        Return WritePrivateProfileString(szSection, szKey, szWritten, IniPath)
    End Function

    ''' <summary>
    ''' 节
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IniSection
        Private SectionName As String
        Private Keys As List(Of String)

        ''' <summary>
        ''' 条目的数量
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property KeyCounts As Integer
            Get
                Return Keys.Count
            End Get
        End Property

        ''' <summary>
        ''' 节名称
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Return SectionName
            End Get
        End Property

        ''' <summary>
        ''' 条目
        ''' </summary>
        ''' <param name="Name">键</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Entry(ByVal szKey As String) As IniEntry
            Get
                If Keys.Contains(szKey) Then
                    Return New IniEntry(Me, szKey)
                End If
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' 条目
        ''' </summary>
        ''' <param name="Index">条目在节中的次序，从0开始</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Entry(ByVal Index As Integer) As IniEntry
            Get
                If Index >= 0 And Index < Keys.Count Then
                    Return New IniEntry(Me, Keys(Index))
                End If
                Return Nothing
            End Get
        End Property

        Public Sub New(ByVal Parser As IniParser, ByVal szName As String)
            If Parser Is Nothing Then
                Throw New NullReferenceException
                Exit Sub
            End If
            If Parser.Sections.Contains(szName) Then
                SectionName = szName
                Dim szReturn As String = Read(szName, vbNullString)
                Keys = New List(Of String)
                If szReturn <> vbNullString Then
                    Keys.AddRange(szReturn.Split(vbNullChar))
                End If
            End If
        End Sub

        Public Sub New(ByVal Parser As IniParser, ByVal Index As Integer)
            If Parser Is Nothing Then
                Throw New NullReferenceException
                Exit Sub
            End If
            If Index >= 0 And Index < Parser.Sections.Count Then
                SectionName = Parser.Sections(Index)
                Dim szReturn As String = Read(SectionName, vbNullString)
                Keys = New List(Of String)
                If szReturn <> vbNullString Then
                    Keys.AddRange(szReturn.Split(vbNullChar))
                End If
            End If
        End Sub

        ''' <summary>
        ''' 插入条目
        ''' </summary>
        ''' <param name="szKey"></param>
        ''' <param name="szValue"></param>
        ''' <remarks></remarks>
        Public Sub AddKey(ByVal szKey As String, Optional ByVal szValue As String = "")
            If szKey <> vbNullString And (Not Keys.Contains(szKey)) Then
                If Write(Me.Name, szKey, szValue) Then
                    Keys.Add(szKey)
                End If
            End If
        End Sub

        ''' <summary>
        ''' 删除条目
        ''' </summary>
        ''' <param name="KeyName">键</param>
        ''' <remarks></remarks>
        Public Sub RemoveKey(ByVal szKey As String)
            If Keys.Contains(szKey) Then
                If Write(Me.Name, szKey, vbNullString) Then
                    Keys.Remove(szKey)
                End If
            End If
        End Sub

        ''' <summary>
        ''' 删除条目
        ''' </summary>
        ''' <param name="Index">条目在节中的次序，从0开始</param>
        ''' <remarks></remarks>
        Public Sub RemoveKey(ByVal Index As Integer)
            If Index >= 0 And Index < Keys.Count Then
                If Write(Me.Name, Keys(Index), vbNullString) Then
                    Keys.RemoveAt(Index)
                End If
            End If
        End Sub

        ''' <summary>
        ''' 条目
        ''' </summary>
        ''' <remarks></remarks>
        Public Class IniEntry
            Private KeyName As String
            Private KeyValue As String
            Private BaseSection As IniSection

            ''' <summary>
            ''' 键
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Key As String
                Get
                    Return KeyName
                End Get
            End Property

            ''' <summary>
            ''' 值
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Value As String
                Get
                    Return KeyValue
                End Get
                Set(value As String)
                    KeyValue = value
                    Write(BaseSection.SectionName, KeyName, KeyValue)
                End Set
            End Property

            Public Sub New(ByVal Section As IniSection, ByVal szKey As String)
                If Section Is Nothing Then
                    Throw New NullReferenceException
                    Exit Sub
                End If
                BaseSection = Section
                If BaseSection.Keys.Contains(szKey) Then
                    KeyName = szKey
                    KeyValue = Read(BaseSection.SectionName, szKey)
                End If
            End Sub

            Public Sub New(ByVal Section As IniSection, ByVal Index As Integer)
                If Section Is Nothing Then
                    Throw New NullReferenceException
                    Exit Sub
                End If
                BaseSection = Section
                If Index >= 0 And Index < BaseSection.KeyCounts Then
                    KeyName = BaseSection.Keys(Index)
                    KeyValue = Read(BaseSection.SectionName, KeyName)
                End If
            End Sub
        End Class
    End Class
End Class

