Namespace Audit

    ''' <summary>
    ''' Collects the audit entries for processing one tax case, in the order they happened.
    ''' </summary>
    Public NotInheritable Class AuditLog

        Private ReadOnly _timeProvider As TimeProvider
        Private ReadOnly _entries As New List(Of AuditEntry)()

        ''' <param name="timeProvider">Source of timestamps; tests pass a fixed clock.</param>
        Public Sub New(timeProvider As TimeProvider)
            ArgumentNullException.ThrowIfNull(timeProvider)
            _timeProvider = timeProvider
        End Sub

        Public ReadOnly Property Entries As IReadOnlyList(Of AuditEntry)
            Get
                Return _entries.AsReadOnly()
            End Get
        End Property

        Public Sub Record(eventType As AuditEventType, description As String, Optional ruleName As String = Nothing)
            _entries.Add(New AuditEntry(_timeProvider.GetUtcNow(), eventType, description, ruleName))
        End Sub

    End Class

End Namespace
