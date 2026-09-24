Namespace Configuration

    ''' <summary>
    ''' A progressive tax bracket. <see cref="Rate"/> applies only to the part of
    ''' taxable income from <see cref="Min"/> (inclusive) up to <see cref="Max"/> (exclusive).
    ''' </summary>
    Public Class TaxBracket

        Public Property Min As Decimal

        ''' <summary>Upper bound (exclusive); Nothing for the top bracket.</summary>
        Public Property Max As Decimal?

        ''' <summary>Rate as a fraction, for example 0.20 for 20 %.</summary>
        Public Property Rate As Decimal

    End Class

End Namespace
