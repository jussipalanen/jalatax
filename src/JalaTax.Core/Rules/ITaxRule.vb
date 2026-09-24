Namespace Rules

    ''' <summary>
    ''' One step of the tax calculation. Rules run in a fixed order and communicate
    ''' only through the <see cref="TaxCalculationContext"/>.
    ''' </summary>
    Public Interface ITaxRule

        ''' <summary>Short, stable name used in audit entries.</summary>
        ReadOnly Property Name As String

        Sub Apply(context As TaxCalculationContext)

    End Interface

End Namespace
