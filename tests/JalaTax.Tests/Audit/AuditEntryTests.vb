Imports JalaTax.Core.Audit

<TestClass>
Public Class AuditEntryTests

    Private Shared ReadOnly FixedTime As New DateTimeOffset(2026, 1, 15, 10, 30, 0, TimeSpan.Zero)

    <TestMethod>
    Public Sub Constructor_WhenRuleNameGiven_SetsAllProperties()
        ' Act
        Dim entry As New AuditEntry(FixedTime, AuditEventType.RuleApplied, "Deduction rule applied", "DeductionRule")

        ' Assert
        Assert.AreEqual(FixedTime, entry.Timestamp)
        Assert.AreEqual(AuditEventType.RuleApplied, entry.EventType)
        Assert.AreEqual("Deduction rule applied", entry.Description)
        Assert.AreEqual("DeductionRule", entry.RuleName)
    End Sub

    <TestMethod>
    Public Sub Constructor_WhenRuleNameOmitted_LeavesRuleNameEmpty()
        ' Act
        Dim entry As New AuditEntry(FixedTime, AuditEventType.CaseLoaded, "Tax case DEMO-001 loaded")

        ' Assert
        Assert.IsNull(entry.RuleName)
    End Sub

    <TestMethod>
    <DataRow(Nothing)>
    <DataRow("")>
    <DataRow("   ")>
    Public Sub Constructor_WhenDescriptionIsEmpty_ThrowsArgumentException(description As String)
        ' Act and Assert
        Assert.Throws(Of ArgumentException)(
            Function() New AuditEntry(FixedTime, AuditEventType.CaseLoaded, description))
    End Sub

End Class
