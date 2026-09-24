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
                result.AddError(NameOf(TaxRuleConfiguration.BasicDeduction), "Basic deduction must not be negative.")
            End If

            ValidateBrackets(configuration.TaxBrackets, result)

            Return result
        End Function

        Private Shared Sub ValidateBrackets(brackets As List(Of TaxBracket), result As ValidationResult)
            If brackets Is Nothing OrElse brackets.Count = 0 Then
                result.AddError(NameOf(TaxRuleConfiguration.TaxBrackets), "At least one tax bracket is required.")
                Return
            End If

            If brackets.Any(Function(bracket) bracket Is Nothing) Then
                result.AddError(NameOf(TaxRuleConfiguration.TaxBrackets), "Tax brackets must not contain empty entries.")
                Return
            End If

            If brackets(0).Min <> 0D Then
                result.AddError(BracketPath(0, NameOf(TaxBracket.Min)), "Tax bracket 1: min must be 0 so that all income is covered.")
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
                                $"Tax bracket {bracketNumber}: rate must be between 0 and 1.")
            End If

            If bracket.Max.HasValue AndAlso bracket.Max.Value <= bracket.Min Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Max)),
                                $"Tax bracket {bracketNumber}: max must be greater than min.")
            End If

            If isLast AndAlso bracket.Max.HasValue Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Max)),
                                $"Tax bracket {bracketNumber}: the last bracket must not have a max, so that all income is covered.")
            End If

            If Not isLast AndAlso Not bracket.Max.HasValue Then
                result.AddError(BracketPath(index, NameOf(TaxBracket.Max)),
                                $"Tax bracket {bracketNumber}: only the last bracket may omit max.")
            End If

            If index > 0 Then
                Dim previousMax = brackets(index - 1).Max
                If previousMax.HasValue AndAlso bracket.Min <> previousMax.Value Then
                    result.AddError(BracketPath(index, NameOf(TaxBracket.Min)),
                                    $"Tax bracket {bracketNumber}: min must equal the previous bracket's max ({previousMax.Value}) so that brackets have no gaps or overlaps.")
                End If
            End If
        End Sub

        Private Shared Function BracketPath(index As Integer, propertyName As String) As String
            Return $"{NameOf(TaxRuleConfiguration.TaxBrackets)}[{index}].{propertyName}"
        End Function

    End Class

End Namespace
