Imports System.IO
Imports System.Text.Json
Imports System.Text.Json.Serialization

Namespace Configuration

    ''' <summary>
    ''' Loads the tax rule configuration from JSON and rejects anything that is missing or invalid.
    ''' </summary>
    Public NotInheritable Class TaxRuleConfigurationLoader

        Private Shared ReadOnly JsonOptions As New JsonSerializerOptions With {
            .PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            .UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            .ReadCommentHandling = JsonCommentHandling.Skip,
            .AllowTrailingCommas = True
        }

        Private ReadOnly _validator As New TaxRuleConfigurationValidator()

        ''' <exception cref="ConfigurationException">The file is missing, unreadable or invalid.</exception>
        Public Function Load(filePath As String) As TaxRuleConfiguration
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath)

            If Not File.Exists(filePath) Then
                Throw New ConfigurationException($"Configuration file was not found: {filePath}")
            End If

            Dim json As String
            Try
                json = File.ReadAllText(filePath)
            Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
                Throw New ConfigurationException($"Configuration file could not be read: {filePath}", ex)
            End Try

            Return Parse(json)
        End Function

        ''' <exception cref="ConfigurationException">The JSON is invalid or describes an invalid configuration.</exception>
        Public Function Parse(json As String) As TaxRuleConfiguration
            ArgumentNullException.ThrowIfNull(json)

            Dim configuration As TaxRuleConfiguration
            Try
                configuration = JsonSerializer.Deserialize(Of TaxRuleConfiguration)(json, JsonOptions)
            Catch ex As JsonException
                Throw New ConfigurationException($"Configuration JSON is invalid: {ex.Message}", ex)
            End Try

            If configuration Is Nothing Then
                Throw New ConfigurationException("Configuration JSON is empty.")
            End If

            Dim validation = _validator.Validate(configuration)
            If Not validation.IsValid Then
                Dim messages = validation.Errors.Select(Function(problem) problem.Message).ToList()
                Throw New ConfigurationException(
                    $"Configuration is invalid:{Environment.NewLine}- {String.Join(Environment.NewLine & "- ", messages)}",
                    messages)
            End If

            Return configuration
        End Function

    End Class

End Namespace
