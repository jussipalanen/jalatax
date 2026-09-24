Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports JalaTax.Core
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization
Imports JalaTax.Core.Models
Imports JalaTax.Core.Services

''' <summary>
''' Main window. Reads the case from the input fields, asks JalaTax.Core to process it and
''' shows validation messages, the result and the audit trail. No tax rules live here.
''' All texts come from resources, so the window can switch between Finnish and English.
''' </summary>
Public Class MainForm

    Private _service As TaxCalculationService
    Private _exampleCases As IReadOnlyList(Of TaxCase) = Array.Empty(Of TaxCase)()
    Private _inputFields As Dictionary(Of String, Control)

    ' Status text is kept as a function so it can be shown again in a newly selected language.
    Private _status As Func(Of String) = Function() String.Empty

    ' True after Calculate, so a language switch can re-run the calculation in the new language.
    Private _hasOutput As Boolean

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _inputFields = New Dictionary(Of String, Control) From {
            {NameOf(TaxCase.TaxpayerId), TaxpayerIdTextBox},
            {NameOf(TaxCase.AnnualIncome), AnnualIncomeTextBox},
            {NameOf(TaxCase.Deductions), DeductionsTextBox}
        }

        ApplyTexts()
        LoadRules()
        LoadExampleCases()
    End Sub

    Private Sub LoadRules()
        Try
            _service = New TaxCalculationService(New TaxRuleConfigurationLoader().Load(DataPath("rules.json")))
            SetStatus(Function() UiText.Get("Status_RulesLoaded"))
        Catch ex As ConfigurationException
            CalculateButton.Enabled = False
            SetStatus(Function() UiText.Get("Status_ConfigError"))
            MessageBox.Show(ex.Message, UiText.Get("Title_ConfigError"), MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadExampleCases()
        Try
            _exampleCases = TaxCaseLoader.Load(DataPath("example-taxpayer.json"))
            ExampleCaseComboBox.Items.AddRange(_exampleCases.Select(Function(taxCase) CObj(taxCase.TaxpayerId)).ToArray())
        Catch ex As InputDataException
            ExampleCaseComboBox.Enabled = False
            Dim message = ex.Message
            SetStatus(Function() UiText.Get("Status_CasesNotLoaded", message))
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
        _hasOutput = True

        Dim taxCase = ReadTaxCase()
        If taxCase Is Nothing Then
            ValidationSummaryLabel.Text = UiText.Get("Message_AmountsAsNumbers")
            Return
        End If

        Dim outcome = _service.Calculate(taxCase)
        ShowAuditTrail(outcome)

        If outcome.IsSuccess Then
            ShowResult(outcome)
            SetStatus(Function() UiText.Get("Status_Calculated", taxCase.TaxpayerId))
        Else
            ShowValidationErrors(outcome)
            SetStatus(Function() UiText.Get("Status_Rejected"))
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
                   $"{UiText.Get("About_Version", ApplicationInfo.Version)}{Environment.NewLine}{Environment.NewLine}" &
                   $"{UiText.Get("About_Description")}{Environment.NewLine}" &
                   UiText.Get("About_Fictional")
        MessageBox.Show(text, UiText.Get("About_Title", ApplicationInfo.Name), MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub FinnishMenuItem_Click(sender As Object, e As EventArgs) Handles FinnishMenuItem.Click
        SwitchLanguage(Languages.Finnish)
    End Sub

    Private Sub EnglishMenuItem_Click(sender As Object, e As EventArgs) Handles EnglishMenuItem.Click
        SwitchLanguage(Languages.English)
    End Sub

    Private Sub SwitchLanguage(languageCode As String)
        Languages.Use(languageCode)
        ApplyTexts()

        ' Results, messages and the audit trail were written in the previous language.
        If _hasOutput AndAlso CalculateButton.Enabled Then
            CalculateButton.PerformClick()
        End If
    End Sub

    ''' <summary>Shows every fixed text of the window in the current language.</summary>
    Private Sub ApplyTexts()
        Text = UiText.Get("Form_Title")
        CaseGroupBox.Text = UiText.Get("Group_Case")
        ResultGroupBox.Text = UiText.Get("Group_Result")
        AuditGroupBox.Text = UiText.Get("Group_Audit")

        ExampleCaseLabel.Text = UiText.Get("Label_ExampleCase")
        TaxpayerIdLabel.Text = DisplayText.TaxpayerId & ":"
        AnnualIncomeLabel.Text = DisplayText.AnnualIncome & ":"
        DeductionsLabel.Text = DisplayText.Deductions & ":"

        CalculateButton.Text = UiText.Get("Button_Calculate")
        ClearButton.Text = UiText.Get("Button_Clear")
        AboutButton.Text = UiText.Get("Button_About")

        ResultItemColumn.Text = UiText.Get("Column_Item")
        ResultAmountColumn.Text = UiText.Get("Column_Amount")
        AuditTimeColumn.Text = UiText.Get("Column_Time")
        AuditEventColumn.Text = UiText.Get("Column_Event")
        AuditRuleColumn.Text = UiText.Get("Column_Rule")
        AuditDescriptionColumn.Text = UiText.Get("Column_Description")

        LanguageDropDownButton.Text = UiText.Get("Language_Selector", Languages.DisplayName(Languages.Current))
        FinnishMenuItem.Checked = Languages.Current = Languages.Finnish
        EnglishMenuItem.Checked = Languages.Current = Languages.English

        StatusLabel.Text = _status()
    End Sub

    Private Sub SetStatus(status As Func(Of String))
        _status = status
        StatusLabel.Text = status()
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

        InputErrorProvider.SetError(field, UiText.Get("Message_EnterNumber"))
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
        AddResultRow(DisplayText.AnnualIncome, result.AnnualIncome)
        AddResultRow(DisplayText.Deductions, result.Deductions)
        AddResultRow(DisplayText.BasicDeduction, result.BasicDeduction)
        AddResultRow(DisplayText.TaxableIncome, result.TaxableIncome)

        For Each bracketTax In outcome.BracketTaxes
            AddResultRow("  " & DisplayText.BracketLabel(bracketTax.Bracket), bracketTax.Tax)
        Next

        Dim totalRow = AddResultRow(DisplayText.CalculatedTax, result.CalculatedTax)
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
            Dim row As New ListViewItem(DisplayFormat.Time(entry.Timestamp))
            row.SubItems.Add(DisplayText.EventName(entry.EventType))
            row.SubItems.Add(If(entry.RuleName, String.Empty))
            row.SubItems.Add(entry.Description)
            AuditListView.Items.Add(row)
        Next
    End Sub

    Private Sub ClearOutput()
        _hasOutput = False
        InputErrorProvider.Clear()
        ValidationSummaryLabel.Text = String.Empty
        ResultListView.Items.Clear()
        AuditListView.Items.Clear()
    End Sub

    Private Shared Function DataPath(fileName As String) As String
        Return Path.Combine(AppContext.BaseDirectory, "data", fileName)
    End Function

End Class
