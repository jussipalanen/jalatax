Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Models
Imports JalaTax.Core.Rules
Imports JalaTax.Core.Validation

Namespace Services

    ''' <summary>
    ''' Validates a tax case and runs the tax rules in order: deductions first, then brackets.
    ''' </summary>
    Public NotInheritable Class TaxCalculationService

        Private ReadOnly _configuration As TaxRuleConfiguration
        Private ReadOnly _validator As New TaxCaseValidator()
        Private ReadOnly _rules As IReadOnlyList(Of ITaxRule) = New ITaxRule() {
            New DeductionRule(),
            New TaxBracketRule()
        }

        ''' <exception cref="ConfigurationException">The configuration is invalid.</exception>
        Public Sub New(configuration As TaxRuleConfiguration)
            ArgumentNullException.ThrowIfNull(configuration)

            ' Configurations built in code bypass the loader, so they are checked here as well.
            Dim configurationCheck = New TaxRuleConfigurationValidator().Validate(configuration)
            If Not configurationCheck.IsValid Then
                Dim messages = configurationCheck.Errors.Select(Function(problem) problem.Message).ToList()
                Throw New ConfigurationException("Configuration is invalid.", messages)
            End If

            _configuration = configuration
        End Sub

        Public Function Calculate(taxCase As TaxCase) As CalculationOutcome
            ArgumentNullException.ThrowIfNull(taxCase)

            Dim validation = _validator.Validate(taxCase)
            If Not validation.IsValid Then
                Return CalculationOutcome.Rejected(taxCase, validation)
            End If

            Dim context As New TaxCalculationContext(taxCase, _configuration)
            For Each rule In _rules
                rule.Apply(context)
            Next

            Dim result As New TaxResult(taxCase.TaxpayerId,
                                        taxCase.AnnualIncome,
                                        taxCase.Deductions,
                                        _configuration.BasicDeduction,
                                        context.TaxableIncome,
                                        context.CalculatedTax)

            Return CalculationOutcome.Succeeded(taxCase, validation, result, context.BracketTaxes)
        End Function

    End Class

End Namespace
