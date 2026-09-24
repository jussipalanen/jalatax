Imports JalaTax.Core.Localization
Imports JalaTax.Core.Validation

Namespace Configuration

    ''' <summary>
    ''' Checks that a <see cref="TaxRuleConfiguration"/> describes a complete, consistent set of brackets.
    ''' </summary>
    Public NotInheritable Class TaxRuleConfigurationValidator

        Public Function Validate(configuration As TaxRuleConfiguration) As ValidationResult
            ArgumentNullException.ThrowIfNull(configuration)

            Dim result As New ValidationResult()

            If configuration.BasicDeduction < 0D Then
                result.AddError(NameOf(TaxRuleConfiguration.BasicDeduction), CoreText.Get("Config_BasicDeductionNegative"))
            End If

            ValidateBrackets(configuration.TaxBrackets, result)

            Return result
        End Function

        Private Shared Sub ValidateBrackets(brackets As List(Of TaxBracket), result As ValidationResult)
            If brackets Is Nothing OrElse brackets.Count = 0 Then
                result.AddError(NameOf(TaxRuleConfiguration.TaxBrackets), CoreText.Get("Config_NoBrackets"))
                Return
            End If

            If brackets.Any(Function(bracket) bracket Is Nothing) Then
                result.AddError(NameOf(TaxRuleConfiguration.TaxBrackets), CoreText.Get("Config_EmptyBracket"))
                Return
            End If

            If brackets(0).Min <> 0D Then
                result.AddError(BracketPath(0, NameOf(TaxBracket.Min)), CoreText.Get("Config_FirstBracketMin"))
            End If

            For index = 0 To brackets.Count - 1
                ValidateBracket(brackets, index, result)
            Next
        End Sub

        Private Shared Sub ValidateBracket(brackets As List(Of TaxBracket), index As Integer, result As ValidationResult)
            Dim bracket = brackets(index)
            Dim isLast = index = brackets.Count - 1
            Dim bracketNumber = index + 1

            If bracket.Rate < 0D OrElse bracket.Rate > 1D Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Rate)),
                                CoreText.Get("Config_RateOutOfRange", bracketNumber))
            End If

            If bracket.Max.HasValue AndAlso bracket.Max.Value <= bracket.Min Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Max)),
                                CoreText.Get("Config_MaxNotGreaterThanMin", bracketNumber))
            End If

            If isLast AndAlso bracket.Max.HasValue Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Max)),
                                CoreText.Get("Config_LastBracketHasMax", bracketNumber))
            End If

            If Not isLast AndAlso Not bracket.Max.HasValue Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Max)),
                                CoreText.Get("Config_OnlyLastBracketOpen", bracketNumber))
            End If

            If index > 0 Then
                Dim previousMax = brackets(index - 1).Max
                If previousMax.HasValue AndAlso bracket.Min <> previousMax.Value Then
                    result.AddError(BracketPath(index, NameOf(TaxBracket.Min)),
                                    CoreText.Get("Config_BracketGap", bracketNumber, DisplayFormat.Amount(previousMax.Value)))
                End If
            End If
        End Sub

        Private Shared Function BracketPath(index As Integer, propertyName As String) As String
            Return $"{NameOf(TaxRuleConfiguration.TaxBrackets)}[{index}].{propertyName}"
        End Function

    End Class

End Namespace
