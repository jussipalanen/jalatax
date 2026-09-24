Imports JalaTax.Core.Services

Namespace Configuration

    ''' <summary>
    ''' Loads the tax rule configuration from JSON and rejects anything that is missing or invalid.
    ''' </summary>
    Public NotInheritable Class TaxRuleConfigurationLoader

        Private Const DataName As String = "Configuration"

        Private ReadOnly _validator As New TaxRuleConfigurationValidator()

        ''' <exception cref="ConfigurationException">The file is missing, unreadable or invalid.</exception>
        Public Function Load(filePath As String) As TaxRuleConfiguration
            Return Validate(JsonFileReader.Read(Of TaxRuleConfiguration)(filePath, DataName, AddressOf CreateException))
        End Function

        ''' <exception cref="ConfigurationException">The JSON is invalid or describes an invalid configuration.</exception>
        Public Function Parse(json As String) As TaxRuleConfiguration
            Return Validate(JsonFileReader.Parse(Of TaxRuleConfiguration)(json, DataName, AddressOf CreateException))
        End Function

        Private Function Validate(configuration As TaxRuleConfiguration) As TaxRuleConfiguration
            Dim validation = _validator.Validate(configuration)
            If Not validation.IsValid Then
                Dim messages = validation.Errors.Select(Function(problem) problem.Message).ToList()
                Throw New ConfigurationException(
                    $"Configuration is invalid:{Environment.NewLine}- {String.Join(Environment.NewLine & "- ", messages)}",
                    messages)
            End If

            Return configuration
        End Function

        Private Shared Function CreateException(message As String, innerException As Exception) As Exception
            Return New ConfigurationException(message, innerException)
        End Function

    End Class

End Namespace
