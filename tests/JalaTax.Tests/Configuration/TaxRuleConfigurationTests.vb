Imports JalaTax.Core.Configuration

<TestClass>
Public Class TaxRuleConfigurationTests

    <TestMethod>
    Public Sub TaxBrackets_WhenCreated_IsEmptyListInsteadOfNothing()
        ' Act
        Dim configuration As New TaxRuleConfiguration()

        ' Assert
        Assert.IsNotNull(configuration.TaxBrackets)
        Assert.IsEmpty(configuration.TaxBrackets)
    End Sub

    <TestMethod>
    Public Sub TaxBracket_WhenMaxNotSet_IsOpenEnded()
        ' Act
        Dim bracket As New TaxBracket With {.Min = 50000D, .Rate = 0.3D}

        ' Assert
        Assert.IsFalse(bracket.Max.HasValue)
    End Sub

End Class
