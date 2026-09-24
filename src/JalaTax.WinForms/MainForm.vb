Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports JalaTax.Core
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Models
Imports JalaTax.Core.Services

''' <summary>
''' Main window. Reads the case from the input fields, asks JalaTax.Core to process it and
''' shows validation messages, the result and the audit trail. No tax rules live here.
''' </summary>
Public Class MainForm

    Private _service As TaxCalculationService
    Private _exampleCases As IReadOnlyList(Of TaxCase) = Array.Empty(Of TaxCase)()
    Private _inputFields As Dictionary(Of String, Control)

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _inputFields = New Dictionary(Of String, Control) From {
            {NameOf(TaxCase.TaxpayerId), TaxpayerIdTextBox},
            {NameOf(TaxCase.AnnualIncome), AnnualIncomeTextBox},
            {NameOf(TaxCase.Deductions), DeductionsTextBox}
        }

        LoadRules()
        LoadExampleCases()
    End Sub

    Private Sub LoadRules()
        Try
            _service = New TaxCalculationService(New TaxRuleConfigurationLoader().Load(DataPath("rules.json")))
            StatusLabel.Text = "Rules loaded from data\rules.json. All tax rules are fictional."
        Catch ex As ConfigurationException
            CalculateButton.Enabled = False
            StatusLabel.Text = "Configuration error: calculation is disabled."
            MessageBox.Show(ex.Message, "Configuration error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadExampleCases()
        Try
            _exampleCases = TaxCaseLoader.Load(DataPath("example-taxpayer.json"))
            ExampleCaseComboBox.Items.AddRange(_exampleCases.Select(Function(taxCase) CObj(taxCase.TaxpayerId)).ToArray())
        Catch ex As InputDataException
            ExampleCaseComboBox.Enabled = False
            StatusLabel.Text = $"Example cases could not be loaded: {ex.Message}"
        End Try
    End Sub

    Private Sub ExampleCaseComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ExampleCaseComboBox.SelectedIndexChanged
        If ExampleCaseComboBox.SelectedIndex < 0 Then
            Return
        End If

        Dim taxCase = _exampleCases(ExampleCaseComboBox.SelectedIndex)
        TaxpayerIdTextBox.Text = taxCase.TaxpayerId
        AnnualIncomeTextBox.Text = taxCase.AnnualIncome.ToString(CultureInfo.CurrentCulture)
        DeductionsTextBox.Text = taxCase.Deductions.ToString(CultureInfo.CurrentCulture)
        ClearOutput()
    End Sub

    Private Sub CalculateButton_Click(sender As Object, e As EventArgs) Handles CalculateButton.Click
        ClearOutput()

        Dim taxCase = ReadTaxCase()
        If taxCase Is Nothing Then
            ValidationSummaryLabel.Text = "Enter amounts as numbers, for example 45000."
            Return
        End If

        Dim outcome = _service.Calculate(taxCase)
        ShowAuditTrail(outcome)

        If outcome.IsSuccess Then
            ShowResult(outcome)
            StatusLabel.Text = $"Case {taxCase.TaxpayerId} calculated."
        Else
            ShowValidationErrors(outcome)
            StatusLabel.Text = "Case rejected. Correct the highlighted fields."
        End If
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        ExampleCaseComboBox.SelectedIndex = -1
        TaxpayerIdTextBox.Clear()
        AnnualIncomeTextBox.Clear()
        DeductionsTextBox.Clear()
        ClearOutput()
        TaxpayerIdTextBox.Focus()
    End Sub

    Private Sub AboutButton_Click(sender As Object, e As EventArgs) Handles AboutButton.Click
        Dim text = $"{ApplicationInfo.Name}{Environment.NewLine}" &
                   $"Version {ApplicationInfo.Version}{Environment.NewLine}{Environment.NewLine}" &
                   $"A VB.NET demo of configurable business rules, validation and auditability.{Environment.NewLine}" &
                   "All tax rules and data are fictional."
        MessageBox.Show(text, $"About {ApplicationInfo.Name}", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' Converts the input fields into a tax case. Returns Nothing when an amount is not a number;
    ''' all other checks are left to JalaTax.Core validation.
    ''' </summary>
    Private Function ReadTaxCase() As TaxCase
        Dim annualIncome = ReadAmount(AnnualIncomeTextBox)
        Dim deductions = ReadAmount(DeductionsTextBox)
        If Not annualIncome.HasValue OrElse Not deductions.HasValue Then
            Return Nothing
        End If

        Return New TaxCase With {
            .TaxpayerId = TaxpayerIdTextBox.Text.Trim(),
            .AnnualIncome = annualIncome.Value,
            .Deductions = deductions.Value
        }
    End Function

    Private Function ReadAmount(field As TextBox) As Decimal?
        Dim value As Decimal
        If Decimal.TryParse(field.Text, NumberStyles.Number, CultureInfo.CurrentCulture, value) Then
            Return value
        End If

        InputErrorProvider.SetError(field, "Enter a number.")
        Return Nothing
    End Function

    Private Sub ShowValidationErrors(outcome As CalculationOutcome)
        For Each problem In outcome.Validation.Errors
            Dim field As Control = Nothing
            If _inputFields.TryGetValue(problem.PropertyName, field) Then
                InputErrorProvider.SetError(field, problem.Message)
            End If
        Next

        ValidationSummaryLabel.Text = String.Join(Environment.NewLine, outcome.Validation.Errors.Select(Function(problem) problem.Message))
    End Sub

    Private Sub ShowResult(outcome As CalculationOutcome)
        Dim result = outcome.Result
        AddResultRow("Annual income", result.AnnualIncome)
        AddResultRow("Deductions", result.Deductions)
        AddResultRow("Basic deduction", result.BasicDeduction)
        AddResultRow("Taxable income", result.TaxableIncome)

        For Each bracketTax In outcome.BracketTaxes
            AddResultRow($"  Bracket {DisplayFormat.BracketRange(bracketTax.Bracket)} at {DisplayFormat.Rate(bracketTax.Bracket.Rate)}",
                         bracketTax.Tax)
        Next

        Dim totalRow = AddResultRow("Calculated tax", result.CalculatedTax)
        totalRow.Font = New Drawing.Font(ResultListView.Font, Drawing.FontStyle.Bold)
    End Sub

    Private Function AddResultRow(label As String, amount As Decimal) As ListViewItem
        Dim row As New ListViewItem(label)
        row.SubItems.Add(DisplayFormat.Amount(amount))
        ResultListView.Items.Add(row)
        Return row
    End Function

    Private Sub ShowAuditTrail(outcome As CalculationOutcome)
        For Each entry In outcome.AuditEntries
            Dim row As New ListViewItem(entry.Timestamp.ToLocalTime().ToString("HH:mm:ss", CultureInfo.CurrentCulture))
            row.SubItems.Add(entry.EventType.ToString())
            row.SubItems.Add(If(entry.RuleName, String.Empty))
            row.SubItems.Add(entry.Description)
            AuditListView.Items.Add(row)
        Next
    End Sub

    Private Sub ClearOutput()
        InputErrorProvider.Clear()
        ValidationSummaryLabel.Text = String.Empty
        ResultListView.Items.Clear()
        AuditListView.Items.Clear()
    End Sub

    Private Shared Function DataPath(fileName As String) As String
        Return Path.Combine(AppContext.BaseDirectory, "data", fileName)
    End Function

End Class
