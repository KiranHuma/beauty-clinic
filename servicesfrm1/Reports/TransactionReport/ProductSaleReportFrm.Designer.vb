﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProductSaleReportFrm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ProductSaleViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.ProductSaleReport1 = New servicesfrm1.ProductSaleReport()
        Me.ProductSaleReport2 = New servicesfrm1.ProductSaleReport()
        Me.SuspendLayout()
        '
        'ProductSaleViewer1
        '
        Me.ProductSaleViewer1.ActiveViewIndex = 0
        Me.ProductSaleViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ProductSaleViewer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.ProductSaleViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProductSaleViewer1.Location = New System.Drawing.Point(0, 0)
        Me.ProductSaleViewer1.Name = "ProductSaleViewer1"
        Me.ProductSaleViewer1.ReportSource = Me.ProductSaleReport1
        Me.ProductSaleViewer1.Size = New System.Drawing.Size(1528, 896)
        Me.ProductSaleViewer1.TabIndex = 0
        Me.ProductSaleViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'ProductSaleReportFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1528, 896)
        Me.Controls.Add(Me.ProductSaleViewer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "ProductSaleReportFrm"
        Me.Text = "Product Sale Report"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ProductSaleViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents ProductSaleReport1 As servicesfrm1.ProductSaleReport
    Friend WithEvents ProductSaleReport2 As servicesfrm1.ProductSaleReport
End Class