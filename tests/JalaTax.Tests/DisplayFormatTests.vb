Imports System.Globalization
Imports JalaTax.Core
Imports JalaTax.Core.Configuration

<TestClass>
Public Class DisplayFormatTests

    <TestMethod>
    <DataRow("45000", "45,000.00")>
    <DataRow("0", "0.00")>
    <DataRow("1999.999", "2,000.00")>
    <DataRow("-500", "-500.00")>
    Public Sub Amount_WhenFormatted_UsesTwoDecimalsAndThousandsSeparator(value As String, expected As String)
        ' Act
        Dim formatted = DisplayFormat.Amount(Decimal.Parse(value, CultureInfo.InvariantCulture))

        ' Assert
        Assert.AreEqual(expected, formatted)
    End Sub

    <TestMethod>
    Public Sub Amount_WhenUserCultureIsFinnish_StillUsesTheSameFormat()
        ' Arrange
        Dim originalCulture = CultureInfo.CurrentCulture
        CultureInfo.CurrentCulture = New CultureInfo("fi-FI")

        Try
            ' Act
            Dim formatted = DisplayFormat.Amount(45000D)

            ' Assert
            Assert.AreEqual("45,000.00", formatted)
        Finally
            CultureInfo.CurrentCulture = originalCulture
        End Try
    End Sub

    <TestMethod>
    <DataRow("0.1", "10 %")>
    <DataRow("0.125", "12.5 %")>
    <DataRow("0", "0 %")>
    <DataRow("1", "100 %")>
    Public Sub Rate_WhenFormatted_ShowsPercentage(value As String, expected As String)
        ' Act
        Dim formatted = DisplayFormat.Rate(Decimal.Parse(value, CultureInfo.InvariantCulture))

        ' Assert
        Assert.AreEqual(expected, formatted)
    End Sub

    <TestMethod>
    Public Sub BracketRange_WhenBracketHasMax_ShowsBothBounds()
        ' Arrange
        Dim bracket As New TaxBracket With {.Min = 20000D, .Max = 50000D, .Rate = 0.2D}

        ' Act
        Dim formatted = DisplayFormat.BracketRange(bracket)

        ' Assert
        Assert.AreEqual("20,000.00–50,000.00", formatted)
    End Sub

    <TestMethod>
    Public Sub BracketRange_WhenBracketIsOpenEnded_ShowsPlusSign()
        ' Arrange
        Dim bracket As New TaxBracket With {.Min = 50000D, .Rate = 0.3D}

        ' Act
        Dim formatted = DisplayFormat.BracketRange(bracket)

        ' Assert
        Assert.AreEqual("50,000.00+", formatted)
    End Sub

End Class
