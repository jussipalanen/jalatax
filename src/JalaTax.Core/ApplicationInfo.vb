Imports System.Reflection

''' <summary>
''' Product name and version, shared by the desktop app and the console runner.
''' The version is set once in Directory.Build.props for every project.
''' </summary>
Public Module ApplicationInfo

    Public ReadOnly Property Name As String = "JalaTax"

    ''' <summary>For example 1.0.0, without the source commit suffix the build adds.</summary>
    Public ReadOnly Property Version As String = ReadVersion()

    Private Function ReadVersion() As String
        Dim informationalVersion = GetType(ApplicationInfo).Assembly.
            GetCustomAttribute(Of AssemblyInformationalVersionAttribute)()?.InformationalVersion

        If String.IsNullOrEmpty(informationalVersion) Then
            Return "unknown"
        End If

        Return informationalVersion.Split("+"c)(0)
    End Function

End Module
