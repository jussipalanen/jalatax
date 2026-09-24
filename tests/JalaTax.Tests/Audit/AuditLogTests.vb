Imports JalaTax.Core.Audit

<TestClass>
Public Class AuditLogTests

    Private Shared ReadOnly FixedTime As New DateTimeOffset(2026, 1, 15, 10, 30, 0, TimeSpan.Zero)

    <TestMethod>
    Public Sub Record_WhenCalled_UsesTimeFromTimeProvider()
        ' Arrange
        Dim log As New AuditLog(New FixedTimeProvider(FixedTime))

        ' Act
        log.Record(AuditEventType.CaseLoaded, "Tax case DEMO-001 loaded")

        ' Assert
        Assert.HasCount(1, log.Entries)
        Assert.AreEqual(FixedTime, log.Entries(0).Timestamp)
        Assert.AreEqual(AuditEventType.CaseLoaded, log.Entries(0).EventType)
        Assert.IsNull(log.Entries(0).RuleName)
    End Sub

    <TestMethod>
    Public Sub Record_WhenCalledSeveralTimes_KeepsEntriesInOrder()
        ' Arrange
        Dim log As New AuditLog(New FixedTimeProvider(FixedTime))

        ' Act
        log.Record(AuditEventType.CaseLoaded, "first")
        log.Record(AuditEventType.RuleApplied, "second", "DeductionRule")

        ' Assert
        Assert.AreEqual("first", log.Entries(0).Description)
        Assert.AreEqual("second", log.Entries(1).Description)
        Assert.AreEqual("DeductionRule", log.Entries(1).RuleName)
    End Sub

    <TestMethod>
    Public Sub Constructor_WhenTimeProviderIsNothing_ThrowsArgumentNullException()
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentNullException)(Function() New AuditLog(Nothing))
    End Sub

End Class
