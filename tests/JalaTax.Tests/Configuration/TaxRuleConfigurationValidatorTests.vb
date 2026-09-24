Imports JalaTax.Core.Configuration

<TestClass>
Public Class TaxRuleConfigurationValidatorTests

    Private ReadOnly _validator As New TaxRuleConfigurationValidator()

    <TestMethod>
    Public Sub Validate_WhenConfigurationIsValid_ReturnsNoErrors()
        ' Arrange
        Dim configuration = CreateValidConfiguration()

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenSingleOpenEndedBracket_ReturnsNoErrors()
        ' Arrange
        Dim configuration As New TaxRuleConfiguration With {
            .TaxBrackets = New List(Of TaxBracket) From {New TaxBracket With {.Min = 0D, .Rate = 0.2D}}
        }

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenConfigurationIsNothing_ThrowsArgumentNullException()
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentNullException)(Function() _validator.Validate(Nothing))
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBasicDeductionIsNegative_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.BasicDeduction = -0.01D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "BasicDeduction")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBasicDeductionIsZero_ReturnsNoErrors()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.BasicDeduction = 0D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBracketListIsEmpty_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets.Clear()

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBracketListIsNothing_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets = Nothing

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBracketEntryIsNothing_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(1) = Nothing

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenFirstBracketDoesNotStartAtZero_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(0).Min = 100D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[0].Min")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBracketsHaveGap_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(1).Min = 20001D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[1].Min")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenBracketsOverlap_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(2).Min = 49999D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[2].Min")
    End Sub

    <TestMethod>
    <DataRow("-0.01")>
    <DataRow("1.01")>
    Public Sub Validate_WhenRateIsOutsideZeroToOne_ReturnsError(rate As String)
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(1).Rate = Decimal.Parse(rate, Globalization.CultureInfo.InvariantCulture)

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[1].Rate")
    End Sub

    <TestMethod>
    <DataRow("0")>
    <DataRow("1")>
    Public Sub Validate_WhenRateIsAtLimit_ReturnsNoErrors(rate As String)
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(1).Rate = Decimal.Parse(rate, Globalization.CultureInfo.InvariantCulture)

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        Assert.IsTrue(result.IsValid)
    End Sub

    <TestMethod>
    Public Sub Validate_WhenMaxIsNotGreaterThanMin_ReturnsError()
        ' Arrange
        Dim configuration As New TaxRuleConfiguration With {
            .TaxBrackets = New List(Of TaxBracket) From {
                New TaxBracket With {.Min = 0D, .Max = 0D, .Rate = 0.1D},
                New TaxBracket With {.Min = 0D, .Rate = 0.2D}
            }
        }

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[0].Max")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenMiddleBracketHasNoMax_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(1).Max = Nothing

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[1].Max")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenLastBracketHasMax_ReturnsError()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.TaxBrackets(2).Max = 100000D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        AssertSingleError(result, "TaxBrackets[2].Max")
    End Sub

    <TestMethod>
    Public Sub Validate_WhenSeveralProblems_ReturnsAllErrors()
        ' Arrange
        Dim configuration = CreateValidConfiguration()
        configuration.BasicDeduction = -1D
        configuration.TaxBrackets(0).Rate = 2D
        configuration.TaxBrackets(2).Min = 60000D

        ' Act
        Dim result = _validator.Validate(configuration)

        ' Assert
        Dim properties = result.Errors.Select(Function(problem) problem.PropertyName).ToList()
        CollectionAssert.AreEquivalent(
            New List(Of String) From {"BasicDeduction", "TaxBrackets[0].Rate", "TaxBrackets[2].Min"},
            properties)
    End Sub

    Private Shared Function CreateValidConfiguration() As TaxRuleConfiguration
        Return New TaxRuleConfiguration With {
            .BasicDeduction = 3000D,
            .TaxBrackets = New List(Of TaxBracket) From {
                New TaxBracket With {.Min = 0D, .Max = 20000D, .Rate = 0.1D},
                New TaxBracket With {.Min = 20000D, .Max = 50000D, .Rate = 0.2D},
                New TaxBracket With {.Min = 50000D, .Rate = 0.3D}
            }
        }
    End Function

    Private Shared Sub AssertSingleError(result As Core.Validation.ValidationResult, expectedPropertyName As String)
        Assert.IsFalse(result.IsValid)
        Assert.HasCount(1, result.Errors)
        Assert.AreEqual(expectedPropertyName, result.Errors(0).PropertyName)
    End Sub

End Class
