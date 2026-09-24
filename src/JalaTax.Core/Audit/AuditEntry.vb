Namespace Audit

    ''' <summary>
    ''' One recorded rule-processing event.
    ''' </summary>
    Public NotInheritable Class AuditEntry

        Public Sub New(timestamp As DateTimeOffset,
                       eventType As AuditEventType,
                       description As String,
                       Optional ruleName As String = Nothing)
            ArgumentException.ThrowIfNullOrWhiteSpace(description)

            Me.Timestamp = timestamp
            Me.EventType = eventType
            Me.Description = description
            Me.RuleName = ruleName
        End Sub

        Public ReadOnly Property Timestamp As DateTimeOffset

        Public ReadOnly Property EventType As AuditEventType

        Public ReadOnly Property Description As String

        ''' <summary>Name of the rule that produced the event; Nothing when no rule is involved.</summary>
        Public ReadOnly Property RuleName As String

    End Class

End Namespace
