Imports JalaTax.Core
Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization
Imports JalaTax.Core.Models
Imports JalaTax.Core.Services
Imports JalaTax.Core.Validation

<TestClass>
Public Class FinnishTranslationTests

    Private Shared ReadOnly Nbsp As String = ChrW(&HA0)

    <TestMethod>
    Public Sub Validate_WhenLanguageIsFinnish_ReturnsFinnishMessages()
        ' Arrange
        Dim taxCase As New TaxCase With {.TaxpayerId = "", .AnnualIncome = -1D, .Deductions = -1D}

        Using New LanguageScope(Languages.Finnish)
            ' Act
            Dim messages = New TaxCaseValidator().Validate(taxCase).Errors.Select(Function(problem) problem.Message).ToList()

            ' Assert
            CollectionAssert.AreEqual(
                New List(Of String) From {
                    "Verovelvollisen tunniste on pakollinen.",
                    "Vuositulot eivät saa olla negatiivisia.",
                    "Vähennykset eivät saa olla negatiivisia."
                },
                messages)
        End Using
    End Sub

    <TestMethod>
    Public Sub Validate_WhenLanguageIsFinnish_KeepsEnglishPropertyNames()
        ' Arrange
        Dim taxCase As New TaxCase With {.TaxpayerId = "DEMO-001", .AnnualIncome = 1D, .Deductions = -1D}

        Using New LanguageScope(Languages.Finnish)
            ' Act
            Dim result = New TaxCaseValidator().Validate(taxCase)

            ' Assert: property names identify fields and are never translated.
            Assert.AreEqual("Deductions", result.Errors(0).PropertyName)
        End Using
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenLanguageIsFinnish_WritesFinnishAuditTrailWithFinnishNumbers()
        ' Arrange
        Dim service As New TaxCalculationService(CreateConfiguration())

        Using New LanguageScope(Languages.Finnish)
            ' Act
            Dim descriptions = service.Calculate(CreateCase(45000D, 2500D)).AuditEntries.
                Select(Function(entry) entry.Description).ToList()

            ' Assert
            CollectionAssert.AreEqual(
                New List(Of String) From {
                    "Verotapaus DEMO-001 ladattu",
                    "Tulot ja vähennykset tarkistettu",
                    $"Vähennykset tehty: verotettava tulo 39{Nbsp}500,00 (tulot 45{Nbsp}000,00 − vähennykset 2{Nbsp}500,00 − perusvähennys 3{Nbsp}000,00, vähintään 0,00)",
                    $"Veroporras 0,00–20{Nbsp}000,00, 10 %: verotettu 20{Nbsp}000,00, vero 2{Nbsp}000,00",
                    $"Veroporras 20{Nbsp}000,00–50{Nbsp}000,00, 20 %: verotettu 19{Nbsp}500,00, vero 3{Nbsp}900,00",
                    $"Laskenta valmis: verotettava tulo 39{Nbsp}500,00, laskettu vero 5{Nbsp}900,00"
                },
                descriptions)
        End Using
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenLanguageIsFinnish_ResultAmountsAreUnchanged()
        ' Arrange
        Dim service As New TaxCalculationService(CreateConfiguration())

        Using New LanguageScope(Languages.Finnish)
            ' Act
            Dim outcome = service.Calculate(CreateCase(45000D, 2500D))

            ' Assert: only texts change with the language, never the calculation.
            Assert.AreEqual(5900D, outcome.Result.CalculatedTax)
        End Using
    End Sub

    <TestMethod>
    Public Sub Parse_WhenLanguageIsFinnishAndConfigurationIsInvalid_ReturnsFinnishMessages()
        ' Arrange
        Dim json = "{ ""basicDeduction"": -1, ""taxBrackets"": [ { ""min"": 0, ""rate"": 0.1 } ] }"

        Using New LanguageScope(Languages.Finnish)
            ' Act
            Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() New TaxRuleConfigurationLoader().Parse(json))

            ' Assert
            Assert.StartsWith("Asetukset ovat virheelliset:", exception.Message)
            Assert.AreEqual("Perusvähennys ei saa olla negatiivinen.", exception.Errors(0))
        End Using
    End Sub

    <TestMethod>
    Public Sub Load_WhenLanguageIsFinnishAndFileIsMissing_ReturnsFinnishMessage()
        Using New LanguageScope(Languages.Finnish)
            ' Act
            Dim exception = Assert.ThrowsExactly(Of InputDataException)(Function() TaxCaseLoader.Load("missing-cases.json"))

            ' Assert
            Assert.AreEqual("Verotapaustiedostoa ei löytynyt: missing-cases.json", exception.Message)
        End Using
    End Sub

    <TestMethod>
    Public Sub DisplayFormat_WhenLanguageIsFinnish_UsesSpaceAndDecimalComma()
        Using New LanguageScope(Languages.Finnish)
            ' Act and Assert
            Assert.AreEqual($"45{Nbsp}000,00", DisplayFormat.Amount(45000D))
            Assert.AreEqual("12,5 %", DisplayFormat.Rate(0.125D))
            Assert.AreEqual($"50{Nbsp}000,00+", DisplayFormat.BracketRange(New TaxBracket With {.Min = 50000D, .Rate = 0.3D}))
        End Using
    End Sub

    <TestMethod>
    Public Sub DisplayText_WhenLanguageIsFinnish_ReturnsFinnishLabels()
        Using New LanguageScope(Languages.Finnish)
            ' Act and Assert
            Assert.AreEqual("Vuositulot", DisplayText.AnnualIncome)
            Assert.AreEqual("Laskettu vero", DisplayText.CalculatedTax)
            Assert.AreEqual("Veroporras 0,00–20" & Nbsp & "000,00, 10 %",
                            DisplayText.BracketLabel(New TaxBracket With {.Min = 0D, .Max = 20000D, .Rate = 0.1D}))
        End Using
    End Sub

    <TestMethod>
    Public Sub EventName_ForEveryEventType_HasEnglishAndFinnishText()
        For Each eventType In [Enum].GetValues(Of AuditEventType)()
            Dim english = DisplayText.EventName(eventType)
            Dim finnish As String
            Using New LanguageScope(Languages.Finnish)
                finnish = DisplayText.EventName(eventType)
            End Using

            Assert.AreNotEqual(english, finnish, $"Event {eventType} is not translated.")
        Next
    End Sub

End Class
