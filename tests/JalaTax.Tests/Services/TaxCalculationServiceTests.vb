Imports System.IO
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Models
Imports JalaTax.Core.Services

<TestClass>
Public Class TaxCalculationServiceTests

    Private ReadOnly _service As New TaxCalculationService(CreateConfiguration())

    <TestMethod>
    Public Sub Calculate_WhenCaseIsValid_ReturnsResultWithAllAmounts()
        ' Arrange
        Dim taxCase = CreateCase(45000D, 2500D)

        ' Act
        Dim outcome = _service.Calculate(taxCase)

        ' Assert
        Assert.IsTrue(outcome.IsSuccess)
        Assert.IsTrue(outcome.Validation.IsValid)
        Assert.AreEqual("DEMO-001", outcome.Result.TaxpayerId)
        Assert.AreEqual(45000D, outcome.Result.AnnualIncome)
        Assert.AreEqual(2500D, outcome.Result.Deductions)
        Assert.AreEqual(3000D, outcome.Result.BasicDeduction)
        Assert.AreEqual(39500D, outcome.Result.TaxableIncome)
        Assert.AreEqual(5900D, outcome.Result.CalculatedTax)
        Assert.HasCount(2, outcome.BracketTaxes)
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenDeductionsExceedIncome_ReturnsZeroTax()
        ' Arrange
        Dim taxCase = CreateCase(2000D, 500D)

        ' Act
        Dim outcome = _service.Calculate(taxCase)

        ' Assert
        Assert.IsTrue(outcome.IsSuccess)
        Assert.AreEqual(0D, outcome.Result.TaxableIncome)
        Assert.AreEqual(0D, outcome.Result.CalculatedTax)
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenCaseIsInvalid_ReturnsValidationErrorsWithoutResult()
        ' Arrange
        Dim taxCase As New TaxCase With {.TaxpayerId = "", .AnnualIncome = -1D, .Deductions = 0D}

        ' Act
        Dim outcome = _service.Calculate(taxCase)

        ' Assert
        Assert.IsFalse(outcome.IsSuccess)
        Assert.IsNull(outcome.Result)
        Assert.IsEmpty(outcome.BracketTaxes)
        Assert.HasCount(2, outcome.Validation.Errors)
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenCaseIsNothing_ThrowsArgumentNullException()
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentNullException)(Function() _service.Calculate(Nothing))
    End Sub

    <TestMethod>
    Public Sub Constructor_WhenConfigurationIsInvalid_ThrowsConfigurationException()
        ' Arrange
        Dim configuration = CreateConfiguration()
        configuration.TaxBrackets(1).Min = 25000D

        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() New TaxCalculationService(configuration))

        ' Assert
        Assert.HasCount(1, exception.Errors)
    End Sub

    <TestMethod>
    Public Sub Constructor_WhenConfigurationIsNothing_ThrowsArgumentNullException()
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentNullException)(Function() New TaxCalculationService(Nothing))
    End Sub

    <TestMethod>
    Public Sub Calculate_WithExampleDataFiles_ProducesExpectedResults()
        ' Arrange
        Dim dataDirectory = Path.Combine(AppContext.BaseDirectory, "data")
        Dim configuration = New TaxRuleConfigurationLoader().Load(Path.Combine(dataDirectory, "rules.json"))
        Dim taxCases = TaxCaseLoader.Load(Path.Combine(dataDirectory, "example-taxpayer.json"))
        Dim service As New TaxCalculationService(configuration)

        ' Act
        Dim outcomes = taxCases.Select(Function(taxCase) service.Calculate(taxCase)).ToList()

        ' Assert: DEMO-001 → 39,500 taxable; DEMO-002 → 63,800 taxable; DEMO-003 is invalid.
        Assert.AreEqual(5900D, outcomes(0).Result.CalculatedTax)
        Assert.AreEqual(63800D, outcomes(1).Result.TaxableIncome)
        Assert.AreEqual(12140D, outcomes(1).Result.CalculatedTax)
        Assert.IsFalse(outcomes(2).IsSuccess)
        Assert.AreEqual("Deductions", outcomes(2).Validation.Errors(0).PropertyName)
    End Sub

End Class
