<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.CaseGroupBox = New System.Windows.Forms.GroupBox()
        Me.ValidationSummaryLabel = New System.Windows.Forms.Label()
        Me.AboutButton = New System.Windows.Forms.Button()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.CalculateButton = New System.Windows.Forms.Button()
        Me.DeductionsTextBox = New System.Windows.Forms.TextBox()
        Me.DeductionsLabel = New System.Windows.Forms.Label()
        Me.AnnualIncomeTextBox = New System.Windows.Forms.TextBox()
        Me.AnnualIncomeLabel = New System.Windows.Forms.Label()
        Me.TaxpayerIdTextBox = New System.Windows.Forms.TextBox()
        Me.TaxpayerIdLabel = New System.Windows.Forms.Label()
        Me.ExampleCaseComboBox = New System.Windows.Forms.ComboBox()
        Me.ExampleCaseLabel = New System.Windows.Forms.Label()
        Me.ResultGroupBox = New System.Windows.Forms.GroupBox()
        Me.ResultListView = New System.Windows.Forms.ListView()
        Me.ResultItemColumn = New System.Windows.Forms.ColumnHeader()
        Me.ResultAmountColumn = New System.Windows.Forms.ColumnHeader()
        Me.AuditGroupBox = New System.Windows.Forms.GroupBox()
        Me.AuditListView = New System.Windows.Forms.ListView()
        Me.AuditTimeColumn = New System.Windows.Forms.ColumnHeader()
        Me.AuditEventColumn = New System.Windows.Forms.ColumnHeader()
        Me.AuditRuleColumn = New System.Windows.Forms.ColumnHeader()
        Me.AuditDescriptionColumn = New System.Windows.Forms.ColumnHeader()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.RuleSetDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.LanguageDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.FinnishMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnglishMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InputErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CaseGroupBox.SuspendLayout()
        Me.ResultGroupBox.SuspendLayout()
        Me.AuditGroupBox.SuspendLayout()
        Me.MainStatusStrip.SuspendLayout()
        CType(Me.InputErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CaseGroupBox
        '
        Me.CaseGroupBox.Controls.Add(Me.AboutButton)
        Me.CaseGroupBox.Controls.Add(Me.ValidationSummaryLabel)
        Me.CaseGroupBox.Controls.Add(Me.ClearButton)
        Me.CaseGroupBox.Controls.Add(Me.CalculateButton)
        Me.CaseGroupBox.Controls.Add(Me.DeductionsTextBox)
        Me.CaseGroupBox.Controls.Add(Me.DeductionsLabel)
        Me.CaseGroupBox.Controls.Add(Me.AnnualIncomeTextBox)
        Me.CaseGroupBox.Controls.Add(Me.AnnualIncomeLabel)
        Me.CaseGroupBox.Controls.Add(Me.TaxpayerIdTextBox)
        Me.CaseGroupBox.Controls.Add(Me.TaxpayerIdLabel)
        Me.CaseGroupBox.Controls.Add(Me.ExampleCaseComboBox)
        Me.CaseGroupBox.Controls.Add(Me.ExampleCaseLabel)
        Me.CaseGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.CaseGroupBox.Name = "CaseGroupBox"
        Me.CaseGroupBox.Size = New System.Drawing.Size(370, 280)
        Me.CaseGroupBox.TabIndex = 0
        Me.CaseGroupBox.TabStop = False
        Me.CaseGroupBox.Text = "Tax case"
        '
        'ExampleCaseLabel
        '
        Me.ExampleCaseLabel.AutoSize = True
        Me.ExampleCaseLabel.Location = New System.Drawing.Point(16, 32)
        Me.ExampleCaseLabel.Name = "ExampleCaseLabel"
        Me.ExampleCaseLabel.Size = New System.Drawing.Size(80, 15)
        Me.ExampleCaseLabel.TabIndex = 0
        Me.ExampleCaseLabel.Text = "Example case:"
        '
        'ExampleCaseComboBox
        '
        Me.ExampleCaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ExampleCaseComboBox.Location = New System.Drawing.Point(140, 28)
        Me.ExampleCaseComboBox.Name = "ExampleCaseComboBox"
        Me.ExampleCaseComboBox.Size = New System.Drawing.Size(180, 23)
        Me.ExampleCaseComboBox.TabIndex = 1
        '
        'TaxpayerIdLabel
        '
        Me.TaxpayerIdLabel.AutoSize = True
        Me.TaxpayerIdLabel.Location = New System.Drawing.Point(16, 72)
        Me.TaxpayerIdLabel.Name = "TaxpayerIdLabel"
        Me.TaxpayerIdLabel.Size = New System.Drawing.Size(72, 15)
        Me.TaxpayerIdLabel.TabIndex = 2
        Me.TaxpayerIdLabel.Text = "Taxpayer ID:"
        '
        'TaxpayerIdTextBox
        '
        Me.TaxpayerIdTextBox.Location = New System.Drawing.Point(140, 68)
        Me.TaxpayerIdTextBox.Name = "TaxpayerIdTextBox"
        Me.TaxpayerIdTextBox.Size = New System.Drawing.Size(180, 23)
        Me.TaxpayerIdTextBox.TabIndex = 3
        '
        'AnnualIncomeLabel
        '
        Me.AnnualIncomeLabel.AutoSize = True
        Me.AnnualIncomeLabel.Location = New System.Drawing.Point(16, 108)
        Me.AnnualIncomeLabel.Name = "AnnualIncomeLabel"
        Me.AnnualIncomeLabel.Size = New System.Drawing.Size(88, 15)
        Me.AnnualIncomeLabel.TabIndex = 4
        Me.AnnualIncomeLabel.Text = "Annual income:"
        '
        'AnnualIncomeTextBox
        '
        Me.AnnualIncomeTextBox.Location = New System.Drawing.Point(140, 104)
        Me.AnnualIncomeTextBox.Name = "AnnualIncomeTextBox"
        Me.AnnualIncomeTextBox.Size = New System.Drawing.Size(180, 23)
        Me.AnnualIncomeTextBox.TabIndex = 5
        Me.AnnualIncomeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'DeductionsLabel
        '
        Me.DeductionsLabel.AutoSize = True
        Me.DeductionsLabel.Location = New System.Drawing.Point(16, 144)
        Me.DeductionsLabel.Name = "DeductionsLabel"
        Me.DeductionsLabel.Size = New System.Drawing.Size(68, 15)
        Me.DeductionsLabel.TabIndex = 6
        Me.DeductionsLabel.Text = "Deductions:"
        '
        'DeductionsTextBox
        '
        Me.DeductionsTextBox.Location = New System.Drawing.Point(140, 140)
        Me.DeductionsTextBox.Name = "DeductionsTextBox"
        Me.DeductionsTextBox.Size = New System.Drawing.Size(180, 23)
        Me.DeductionsTextBox.TabIndex = 7
        Me.DeductionsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CalculateButton
        '
        Me.CalculateButton.Location = New System.Drawing.Point(140, 180)
        Me.CalculateButton.Name = "CalculateButton"
        Me.CalculateButton.Size = New System.Drawing.Size(96, 30)
        Me.CalculateButton.TabIndex = 8
        Me.CalculateButton.Text = "&Calculate"
        Me.CalculateButton.UseVisualStyleBackColor = True
        '
        'ClearButton
        '
        Me.ClearButton.Location = New System.Drawing.Point(242, 180)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(78, 30)
        Me.ClearButton.TabIndex = 9
        Me.ClearButton.Text = "C&lear"
        Me.ClearButton.UseVisualStyleBackColor = True
        '
        'ValidationSummaryLabel
        '
        Me.ValidationSummaryLabel.ForeColor = System.Drawing.Color.Firebrick
        Me.ValidationSummaryLabel.Location = New System.Drawing.Point(16, 222)
        Me.ValidationSummaryLabel.Name = "ValidationSummaryLabel"
        Me.ValidationSummaryLabel.Size = New System.Drawing.Size(250, 50)
        Me.ValidationSummaryLabel.TabIndex = 10
        '
        'AboutButton
        '
        Me.AboutButton.Location = New System.Drawing.Point(282, 238)
        Me.AboutButton.Name = "AboutButton"
        Me.AboutButton.Size = New System.Drawing.Size(74, 30)
        Me.AboutButton.TabIndex = 11
        Me.AboutButton.Text = "&About"
        Me.AboutButton.UseVisualStyleBackColor = True
        '
        'ResultGroupBox
        '
        Me.ResultGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResultGroupBox.Controls.Add(Me.ResultListView)
        Me.ResultGroupBox.Location = New System.Drawing.Point(394, 12)
        Me.ResultGroupBox.Name = "ResultGroupBox"
        Me.ResultGroupBox.Padding = New System.Windows.Forms.Padding(10, 6, 10, 10)
        Me.ResultGroupBox.Size = New System.Drawing.Size(514, 280)
        Me.ResultGroupBox.TabIndex = 1
        Me.ResultGroupBox.TabStop = False
        Me.ResultGroupBox.Text = "Result"
        '
        'ResultListView
        '
        Me.ResultListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ResultItemColumn, Me.ResultAmountColumn})
        Me.ResultListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResultListView.FullRowSelect = True
        Me.ResultListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ResultListView.Location = New System.Drawing.Point(10, 22)
        Me.ResultListView.Name = "ResultListView"
        Me.ResultListView.Size = New System.Drawing.Size(494, 248)
        Me.ResultListView.TabIndex = 0
        Me.ResultListView.View = System.Windows.Forms.View.Details
        '
        'ResultItemColumn
        '
        Me.ResultItemColumn.Text = "Item"
        Me.ResultItemColumn.Width = 320
        '
        'ResultAmountColumn
        '
        Me.ResultAmountColumn.Text = "Amount"
        Me.ResultAmountColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ResultAmountColumn.Width = 150
        '
        'AuditGroupBox
        '
        Me.AuditGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AuditGroupBox.Controls.Add(Me.AuditListView)
        Me.AuditGroupBox.Location = New System.Drawing.Point(12, 302)
        Me.AuditGroupBox.Name = "AuditGroupBox"
        Me.AuditGroupBox.Padding = New System.Windows.Forms.Padding(10, 6, 10, 10)
        Me.AuditGroupBox.Size = New System.Drawing.Size(896, 300)
        Me.AuditGroupBox.TabIndex = 2
        Me.AuditGroupBox.TabStop = False
        Me.AuditGroupBox.Text = "Audit trail"
        '
        'AuditListView
        '
        Me.AuditListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.AuditTimeColumn, Me.AuditEventColumn, Me.AuditRuleColumn, Me.AuditDescriptionColumn})
        Me.AuditListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AuditListView.FullRowSelect = True
        Me.AuditListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.AuditListView.Location = New System.Drawing.Point(10, 22)
        Me.AuditListView.Name = "AuditListView"
        Me.AuditListView.Size = New System.Drawing.Size(876, 268)
        Me.AuditListView.TabIndex = 0
        Me.AuditListView.View = System.Windows.Forms.View.Details
        '
        'AuditTimeColumn
        '
        Me.AuditTimeColumn.Text = "Time"
        Me.AuditTimeColumn.Width = 80
        '
        'AuditEventColumn
        '
        Me.AuditEventColumn.Text = "Event"
        Me.AuditEventColumn.Width = 150
        '
        'AuditRuleColumn
        '
        Me.AuditRuleColumn.Text = "Rule"
        Me.AuditRuleColumn.Width = 110
        '
        'AuditDescriptionColumn
        '
        Me.AuditDescriptionColumn.Text = "Description"
        Me.AuditDescriptionColumn.Width = 900
        '
        'MainStatusStrip
        '
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.RuleSetDropDownButton, Me.LanguageDropDownButton})
        Me.MainStatusStrip.Location = New System.Drawing.Point(0, 614)
        Me.MainStatusStrip.Name = "MainStatusStrip"
        Me.MainStatusStrip.Size = New System.Drawing.Size(920, 22)
        Me.MainStatusStrip.TabIndex = 3
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(789, 17)
        Me.StatusLabel.Spring = True
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RuleSetDropDownButton
        '
        Me.RuleSetDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.RuleSetDropDownButton.Name = "RuleSetDropDownButton"
        Me.RuleSetDropDownButton.Size = New System.Drawing.Size(140, 20)
        Me.RuleSetDropDownButton.Text = "Rules"
        '
        'LanguageDropDownButton
        '
        Me.LanguageDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LanguageDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FinnishMenuItem, Me.EnglishMenuItem})
        Me.LanguageDropDownButton.Name = "LanguageDropDownButton"
        Me.LanguageDropDownButton.Size = New System.Drawing.Size(116, 20)
        Me.LanguageDropDownButton.Text = "Language: English"
        '
        'FinnishMenuItem
        '
        Me.FinnishMenuItem.Name = "FinnishMenuItem"
        Me.FinnishMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FinnishMenuItem.Text = "Suomi"
        '
        'EnglishMenuItem
        '
        Me.EnglishMenuItem.Name = "EnglishMenuItem"
        Me.EnglishMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.EnglishMenuItem.Text = "English"
        '
        'InputErrorProvider
        '
        Me.InputErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.InputErrorProvider.ContainerControl = Me
        '
        'MainForm
        '
        Me.AcceptButton = Me.CalculateButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(920, 636)
        Me.Controls.Add(Me.MainStatusStrip)
        Me.Controls.Add(Me.AuditGroupBox)
        Me.Controls.Add(Me.ResultGroupBox)
        Me.Controls.Add(Me.CaseGroupBox)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MinimumSize = New System.Drawing.Size(900, 560)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "JalaTax – tax calculation demo"
        Me.CaseGroupBox.ResumeLayout(False)
        Me.CaseGroupBox.PerformLayout()
        Me.ResultGroupBox.ResumeLayout(False)
        Me.AuditGroupBox.ResumeLayout(False)
        Me.MainStatusStrip.ResumeLayout(False)
        Me.MainStatusStrip.PerformLayout()
        CType(Me.InputErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CaseGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ExampleCaseLabel As System.Windows.Forms.Label
    Friend WithEvents ExampleCaseComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents TaxpayerIdLabel As System.Windows.Forms.Label
    Friend WithEvents TaxpayerIdTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AnnualIncomeLabel As System.Windows.Forms.Label
    Friend WithEvents AnnualIncomeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DeductionsLabel As System.Windows.Forms.Label
    Friend WithEvents DeductionsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CalculateButton As System.Windows.Forms.Button
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Friend WithEvents ValidationSummaryLabel As System.Windows.Forms.Label
    Friend WithEvents AboutButton As System.Windows.Forms.Button
    Friend WithEvents ResultGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ResultListView As System.Windows.Forms.ListView
    Friend WithEvents ResultItemColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents ResultAmountColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents AuditGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AuditListView As System.Windows.Forms.ListView
    Friend WithEvents AuditTimeColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents AuditEventColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents AuditRuleColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents AuditDescriptionColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents RuleSetDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents LanguageDropDownButton As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents FinnishMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnglishMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InputErrorProvider As System.Windows.Forms.ErrorProvider
End Class
