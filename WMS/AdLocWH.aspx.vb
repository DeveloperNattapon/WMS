Option Explicit On
Option Strict Off
Option Infer On

Public Class AdLocWH
    Inherits System.Web.UI.Page

    Dim db As New LKBWarehouseEntities1
    'Dim db As New LKBWarehouseEntities
    Dim OwnerPN As String
    Dim CustomerLOTNo As String
    Dim OrderNo As String
    Dim invoice As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
           
        End If
    End Sub

    Protected Sub btnFind_ServerClick(sender As Object, e As EventArgs)
        If rdbAdLoc.Checked = True Then
            If txtOwnerPN.Value <> "" And txtCustomerLotNo.Value <> "" And txtCusRefNo.Value <> "" Then
                OwnerPN = txtOwnerPN.Value.Trim
                CustomerLOTNo = txtCustomerLotNo.Value.Trim
                OrderNo = txtCusRefNo.Value.Trim
            ElseIf txtOwnerPN.Value <> "" And txtCustomerLotNo.Value <> "" Then
                OwnerPN = txtOwnerPN.Value.Trim
                CustomerLOTNo = txtCustomerLotNo.Value.Trim
            ElseIf txtOwnerPN.Value <> "" And txtCusRefNo.Value <> "" Then
                OwnerPN = txtOwnerPN.Value.Trim
                OrderNo = txtCusRefNo.Value.Trim
            ElseIf txtCustomerLotNo.Value <> "" And txtCusRefNo.Value <> "" Then
                CustomerLOTNo = txtCustomerLotNo.Value.Trim
                OrderNo = txtCusRefNo.Value.Trim

            ElseIf txtOwnerPN.Value <> "" And txtInvoice.Value <> "" Then
                OwnerPN = txtOwnerPN.Value.Trim
                invoice = txtInvoice.Value.Trim
                'Str = " tblWHStockMovement.OwnerPN = '" & txtFOwnerPN.Text & "' AND " & " tblWHStockMovement.invoice = '" & txtInvoice.Text.Trim & "'"
            ElseIf txtOwnerPN.Value <> "" Then
                OwnerPN = txtOwnerPN.Value.Trim

            ElseIf txtCustomerLotNo.Value <> "" Then
                CustomerLOTNo = txtCustomerLotNo.Value.Trim

            ElseIf txtCusRefNo.Value <> "" Then
                OwnerPN = txtOwnerPN.Value.Trim

            ElseIf txtInvoice.Value <> "" Then

                invoice = txtInvoice.Value.Trim
            End If
            'Str = " tblWHStockMovement.CustomerLOTNo = '" & txtFCustomerLotNo.Text & "' AND " & " tblWHStockMovement.OrderNo = '" & txtFCusRefNo.Text
            'ReadDATA()
            'GroupBox2.Enabled = True
            Dim co = (From ch In db.tblWHConfirmGoodsReceiveDetails Join sm In db.tblWHStockMovements On ch.LOTNo Equals sm.LOTNo _
                  And ch.ReceiveNo Equals sm.ReceiveNo And ch.ItemNo Equals sm.ItemNo And ch.OwnerPN Equals sm.OwnerPN
                  Where (sm.StockType = "Received" And ch.StatusAvailable = 0 And (sm.CustomerLOTNo = CustomerLOTNo Or sm.OrderNo = OrderNo Or _
                         sm.Invoice = invoice) Or (sm.OwnerPN = OwnerPN And sm.CustomerLOTNo = CustomerLOTNo And sm.OrderNo = OrderNo) Or _
                     (sm.OwnerPN = OwnerPN And sm.CustomerLOTNo = CustomerLOTNo) Or (sm.OwnerPN = OwnerPN And sm.OrderNo = OrderNo) Or _
                     (sm.CustomerLOTNo = CustomerLOTNo And OrderNo = OrderNo) Or (sm.OwnerPN = OwnerPN And sm.Invoice = invoice))
                  Select ch.LOTNo, sm.OwnerPN).ToList
        ElseIf rdbAdQTY.Checked = True Then
            'Str = "SELECT * From tblWHConfirmGoodsReceiveDetail WHERE LOTNo = '" & txtJobNo.Text & "'"
            'ReadDATAByJob()
        Else
            'MessageBox.Show("��س����͡��͹��Ҩзӧҹ���� !!!", "�š�õ�Ǩ�ͺ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'txtFOwnerPN.Focus()
            Exit Sub
        End If
    End Sub
    Protected Sub btnSave_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnClear_ServerClick(sender As Object, e As EventArgs)

    End Sub
    Private Sub showTest()
        'Dim sy = (From s In db.tblWHStockMovements)
    End Sub
    Public Function shwo(i As Integer, j As Integer) As Integer
        Dim d As Integer
        d = i + j
        Return d
    End Function
End Class