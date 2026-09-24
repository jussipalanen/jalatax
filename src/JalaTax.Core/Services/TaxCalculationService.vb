Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization
Imports JalaTax.Core.Models
Imports JalaTax.Core.Rules
Imports JalaTax.Core.Validation

Namespace Services

    ''' <summary>
    ''' Validates a tax case and runs the tax rules in order: deductions first, then brackets.
    ''' Every step is recorded in an audit log returned with the outcome.
    ''' </summary>
    Public NotInheritable Class TaxCalculationService

        Private ReadOnly _configuration As TaxRuleConfiguration
        Private ReadOnly _timeProvider As TimeProvider
        Private ReadOnly _validator As New TaxCaseValidator()
        Private ReadOnly _rules As IReadOnlyList(Of ITaxRule) = New ITaxRule() {
            New DeductionRule(),
            New TaxBracketRule()
        }

        ''' <param name="timeProvider">Source of audit timestamps; the system clock when omitted.</param>
        ''' <exception cref="ConfigurationException">The configuration is invalid.</exception>
        Public Sub New(configuration As TaxRuleConfiguration, Optional timeProvider As TimeProvider = Nothing)
            ArgumentNullException.ThrowIfNull(configuration)

            ' Configurations built in code bypass the loader, so they are checked here as well.
            Dim configurationCheck = New TaxRuleConfigurationValidator().Validate(configuration)
            If Not configurationCheck.IsValid Then
                Dim messages = configurationCheck.Errors.Select(Function(problem) problem.Message).ToList()
                Throw New ConfigurationException(CoreText.Get("Config_Invalid"), messages)
            End If

            _configuration = configuration
            _timeProvider = If(timeProvider, TimeProvider.System)
        End Sub

        Public Function Calculate(taxCase As TaxCase) As CalculationOutcome
            ArgumentNullException.ThrowIfNull(taxCase)

            Dim auditLog As New AuditLog(_timeProvider)
            auditLog.Record(AuditEventType.CaseLoaded, CoreText.Get("Audit_CaseLoaded", DescribeCase(taxCase)))

            Dim validation = _validator.Validate(taxCase)
            If Not validation.IsValid Then
                For Each problem In validation.Errors
                    auditLog.Record(AuditEventType.ValidationFailed, CoreText.Get("Audit_ValidationFailed", problem.Message))
                Next
                Return CalculationOutcome.Rejected(taxCase, validation, auditLog.Entries)
            End If

            auditLog.Record(AuditEventType.ValidationPassed, CoreText.Get("Audit_ValidationPassed"))
            auditLog.Record(AuditEventType.RuleSetSelected, CoreText.Get("Audit_RuleSetSelected", DisplayText.RuleSetName(_configuration)))

            Dim context As New TaxCalculationContext(taxCase, _configuration, auditLog)
            For Each rule In _rules
                rule.Apply(context)
            Next

            Dim result As New TaxResult(taxCase.TaxpayerId,
                                        taxCase.AnnualIncome,
                                        taxCase.Deductions,
                                        _configuration.BasicDeduction,
                                        context.TaxableIncome,
                                        context.CalculatedTax)

            auditLog.Record(AuditEventType.CalculationCompleted,
                            CoreText.Get("Audit_CalculationCompleted",
                                         DisplayFormat.Amount(result.TaxableIncome),
                                         DisplayFormat.Amount(result.CalculatedTax)))

            Return CalculationOutcome.Succeeded(taxCase, validation, result, context.BracketTaxes, auditLog.Entries)
        End Function

        Private Shared Function DescribeCase(taxCase As TaxCase) As String
            Return If(String.IsNullOrWhiteSpace(taxCase.TaxpayerId), CoreText.Get("Audit_NoTaxpayerId"), taxCase.TaxpayerId)
        End Function

    End Class

End Namespace
