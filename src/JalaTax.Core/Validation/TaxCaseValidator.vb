Imports JalaTax.Core.Localization
Imports JalaTax.Core.Models

Namespace Validation

    ''' <summary>
    ''' Checks a <see cref="TaxCase"/> before any calculation and reports every problem at once.
    ''' </summary>
    Public NotInheritable Class TaxCaseValidator

        Public Function Validate(taxCase As TaxCase) As ValidationResult
            ArgumentNullException.ThrowIfNull(taxCase)

            Dim result As New ValidationResult()

            If String.IsNullOrWhiteSpace(taxCase.TaxpayerId) Then
                result.AddError(NameOf(TaxCase.TaxpayerId), CoreText.Get("Validation_TaxpayerIdRequired"))
            End If

            If taxCase.AnnualIncome < 0D Then
                result.AddError(NameOf(TaxCase.AnnualIncome), CoreText.Get("Validation_AnnualIncomeNegative"))
            End If

            ' Deductions larger than income are allowed; taxable income is limited to zero during calculation.
            If taxCase.Deductions < 0D Then
                result.AddError(NameOf(TaxCase.Deductions), CoreText.Get("Validation_DeductionsNegative"))
            End If

            Return result
        End Function

    End Class

End Namespace
