Imports JalaTax.Core.Models
Imports JalaTax.Core.Validation

<TestClass>
Public Class TaxCaseValidatorTests

    Private ReadOnly _validator As New TaxCaseValidator()

    <TestMethod>
    Public Sub Validate_WhenCaseIsValid_ReturnsNoErrors()
        ' Arrange
        Dim taxCase = CreateCase("DEMO-001", 45000D, 2500D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenIncomeAndDeductionsAreZero_ReturnsNoErrors()
        ' Arrange
        Dim taxCase = CreateCase("DEMO-001", 0D, 0D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenDeductionsExceedIncome_ReturnsNoErrors()
        ' Arrange
        Dim taxCase = CreateCase("DEMO-001", 1000D, 5000D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    <DataRow(Nothing)>
    <DataRow("")>
    <DataRow("   ")>
    Public Sub Validate_WhenTaxpayerIdIsEmpty_ReturnsValidationError(taxpayerId As String)
        ' Arrange
        Dim taxCase = CreateCase(taxpayerId, 45000D, 2500D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        AssertSingleError(result, "TaxpayerId", "Taxpayer ID is required.")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenIncomeIsNegative_ReturnsValidationError()
        ' Arrange
        Dim taxCase = CreateCase("DEMO-001", -0.01D, 0D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        AssertSingleError(result, "AnnualIncome", "Annual income must not be negative.")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenDeductionsAreNegative_ReturnsValidationError()
        ' Arrange
        Dim taxCase = CreateCase("DEMO-001", 45000D, -0.01D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        AssertSingleError(result, "Deductions", "Deductions must not be negative.")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenEveryFieldIsInvalid_ReturnsAllErrorsInFieldOrder()
        ' Arrange
        Dim taxCase = CreateCase("", -1D, -1D)

        ' Act
        Dim result = _validator.Validate(taxCase)

        ' Assert
        Dim properties = result.Errors.Select(Function(problem) problem.PropertyName).ToList()
        CollectionAssert.AreEqual(New List(Of String) From {"TaxpayerId", "AnnualIncome", "Deductions"}, properties)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenCaseIsNothing_ThrowsArgumentNullException()
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentNullException)(Function() _validator.Validate(Nothing))
    End Sub

    Private Shared Function CreateCase(taxpayerId As String, annualIncome As Decimal, deductions As Decimal) As TaxCase
        Return New TaxCase With {.TaxpayerId = taxpayerId, .AnnualIncome = annualIncome, .Deductions = deductions}
    End Function

    Private Shared Sub AssertSingleError(result As ValidationResult, expectedPropertyName As String, expectedMessage As String)
        Assert.IsFalse(result.IsValid)
        Assert.HasCount(1, result.Errors)
        Assert.AreEqual(expectedPropertyName, result.Errors(0).PropertyName)
        Assert.AreEqual(expectedMessage, result.Errors(0).Message)
    End Sub

End Class
