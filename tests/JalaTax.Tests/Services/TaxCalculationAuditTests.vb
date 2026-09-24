Imports JalaTax.Core.Audit
Imports JalaTax.Core.Models
Imports JalaTax.Core.Services

<TestClass>
Public Class TaxCalculationAuditTests

    Private Shared ReadOnly FixedTime As New DateTimeOffset(2026, 1, 15, 10, 30, 0, TimeSpan.Zero)

    Private ReadOnly _service As New TaxCalculationService(CreateConfiguration(), New FixedTimeProvider(FixedTime))

    <TestMethod>
    Public Sub Calculate_WhenCaseIsValid_RecordsEveryStepInOrder()
        ' Arrange
        Dim taxCase = CreateCase(45000D, 2500D)

        ' Act
        Dim entries = _service.Calculate(taxCase).AuditEntries

        ' Assert
        Dim eventTypes = entries.Select(Function(entry) entry.EventType).ToList()
        CollectionAssert.AreEqual(
            New List(Of AuditEventType) From {
                AuditEventType.CaseLoaded,
                AuditEventType.ValidationPassed,
                AuditEventType.RuleApplied,
                AuditEventType.RuleApplied,
                AuditEventType.RuleApplied,
                AuditEventType.CalculationCompleted
            },
            eventTypes)
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenCaseIsValid_DescribesEachStepWithAmounts()
        ' Arrange
        Dim taxCase = CreateCase(45000D, 2500D)

        ' Act
        Dim descriptions = _service.Calculate(taxCase).AuditEntries.Select(Function(entry) entry.Description).ToList()

        ' Assert
        CollectionAssert.AreEqual(
            New List(Of String) From {
                "Tax case DEMO-001 loaded",
                "Income and deductions validated",
                "Deductions applied: taxable income 39,500.00 (income 45,000.00 − deductions 2,500.00 − basic deduction 3,000.00, not below 0.00)",
                "Tax bracket 0.00–20,000.00 at 10 %: 20,000.00 taxed, tax 2,000.00",
                "Tax bracket 20,000.00–50,000.00 at 20 %: 19,500.00 taxed, tax 3,900.00",
                "Calculation completed: taxable income 39,500.00, calculated tax 5,900.00"
            },
            descriptions)
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenRulesRun_RecordsRuleNames()
        ' Arrange
        Dim taxCase = CreateCase(45000D, 2500D)

        ' Act
        Dim entries = _service.Calculate(taxCase).AuditEntries

        ' Assert
        Dim ruleNames = entries.Where(Function(entry) entry.EventType = AuditEventType.RuleApplied).
            Select(Function(entry) entry.RuleName).ToList()
        CollectionAssert.AreEqual(New List(Of String) From {"DeductionRule", "TaxBracketRule", "TaxBracketRule"}, ruleNames)
        Assert.IsTrue(entries.Where(Function(entry) entry.EventType <> AuditEventType.RuleApplied).All(Function(entry) entry.RuleName Is Nothing))
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenIncomeIsInTopBracket_DescribesOpenEndedBracket()
        ' Arrange
        Dim taxCase = CreateCase(68000D, 1200D)

        ' Act
        Dim entries = _service.Calculate(taxCase).AuditEntries

        ' Assert
        Assert.IsTrue(entries.Any(Function(entry) entry.Description = "Tax bracket 50,000.00+ at 30 %: 13,800.00 taxed, tax 4,140.00"))
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenTaxableIncomeIsZero_RecordsThatNoBracketApplied()
        ' Arrange
        Dim taxCase = CreateCase(1000D, 0D)

        ' Act
        Dim entries = _service.Calculate(taxCase).AuditEntries

        ' Assert
        Dim bracketEntries = entries.Where(Function(entry) entry.RuleName = "TaxBracketRule").ToList()
        Assert.HasCount(1, bracketEntries)
        Assert.AreEqual("No taxable income, no bracket applied: tax 0.00", bracketEntries(0).Description)
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenCaseIsInvalid_RecordsEachValidationErrorAndStops()
        ' Arrange
        Dim taxCase As New TaxCase With {.TaxpayerId = "   ", .AnnualIncome = -1D, .Deductions = 0D}

        ' Act
        Dim entries = _service.Calculate(taxCase).AuditEntries

        ' Assert
        CollectionAssert.AreEqual(
            New List(Of String) From {
                "Tax case (no taxpayer ID) loaded",
                "Validation failed: Taxpayer ID is required.",
                "Validation failed: Annual income must not be negative."
            },
            entries.Select(Function(entry) entry.Description).ToList())
        Assert.IsFalse(entries.Any(Function(entry) entry.EventType = AuditEventType.RuleApplied))
        Assert.IsFalse(entries.Any(Function(entry) entry.EventType = AuditEventType.CalculationCompleted))
    End Sub

    <TestMethod>
    Public Sub Calculate_WithFixedClock_TimestampsEveryEntry()
        ' Arrange
        Dim taxCase = CreateCase(45000D, 2500D)

        ' Act
        Dim entries = _service.Calculate(taxCase).AuditEntries

        ' Assert
        Assert.IsTrue(entries.All(Function(entry) entry.Timestamp = FixedTime))
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenCalledTwice_KeepsSeparateAuditTrails()
        ' Act
        Dim first = _service.Calculate(CreateCase(45000D, 2500D))
        Dim second = _service.Calculate(CreateCase(1000D, 0D))

        ' Assert
        Assert.HasCount(6, first.AuditEntries)
        Assert.HasCount(5, second.AuditEntries)
    End Sub

End Class
