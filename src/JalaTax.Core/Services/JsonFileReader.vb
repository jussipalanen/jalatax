Imports System.IO
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports JalaTax.Core.Localization

Namespace Services

    ''' <summary>
    ''' Shared file and JSON handling for the data loaders. Each loader supplies its own
    ''' exception type so callers see a failure specific to the data they asked for.
    ''' </summary>
    Friend NotInheritable Class JsonFileReader

        Private Shared ReadOnly JsonOptions As New JsonSerializerOptions With {
            .PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            .ReadCommentHandling = JsonCommentHandling.Skip,
            .AllowTrailingCommas = True
        }

        Private Sub New()
        End Sub

        Friend Shared Function Read(Of T As Class)(filePath As String,
                                                   dataKey As String,
                                                   createException As Func(Of String, Exception, Exception)) As T
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath)

            If Not File.Exists(filePath) Then
                Throw createException(CoreText.Get($"{dataKey}_FileNotFound", filePath), Nothing)
            End If

            Dim json As String
            Try
                json = File.ReadAllText(filePath)
            Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
                Throw createException(CoreText.Get($"{dataKey}_FileUnreadable", filePath), ex)
            End Try

            Return Parse(Of T)(json, dataKey, createException)
        End Function

        Friend Shared Function Parse(Of T As Class)(json As String,
                                                    dataKey As String,
                                                    createException As Func(Of String, Exception, Exception)) As T
            ArgumentNullException.ThrowIfNull(json)

            Dim value As T
            Try
                value = JsonSerializer.Deserialize(Of T)(json, JsonOptions)
            Catch ex As JsonException
                Throw createException(CoreText.Get($"{dataKey}_JsonInvalid", ex.Message), ex)
            End Try

            If value Is Nothing Then
                Throw createException(CoreText.Get($"{dataKey}_JsonEmpty"), Nothing)
            End If

            Return value
        End Function

    End Class

End Namespace
