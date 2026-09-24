Imports System.IO
Imports System.Text.Json
Imports System.Text.Json.Serialization

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
                                                   dataName As String,
                                                   createException As Func(Of String, Exception, Exception)) As T
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath)

            If Not File.Exists(filePath) Then
                Throw createException($"{dataName} file was not found: {filePath}", Nothing)
            End If

            Dim json As String
            Try
                json = File.ReadAllText(filePath)
            Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
                Throw createException($"{dataName} file could not be read: {filePath}", ex)
            End Try

            Return Parse(Of T)(json, dataName, createException)
        End Function

        Friend Shared Function Parse(Of T As Class)(json As String,
                                                    dataName As String,
                                                    createException As Func(Of String, Exception, Exception)) As T
            ArgumentNullException.ThrowIfNull(json)

            Dim value As T
            Try
                value = JsonSerializer.Deserialize(Of T)(json, JsonOptions)
            Catch ex As JsonException
                Throw createException($"{dataName} JSON is invalid: {ex.Message}", ex)
            End Try

            If value Is Nothing Then
                Throw createException($"{dataName} JSON is empty.", Nothing)
            End If

            Return value
        End Function

    End Class

End Namespace
