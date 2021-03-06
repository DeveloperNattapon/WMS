﻿Option Explicit On
Option Strict On
Option Infer On

Public Class MasterCode
    Inherits System.Web.UI.Page
    'Dim db As New LKBwarehouseEntities
    Dim db As New LKBWarehouseEntities1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            showMasterCode()
        End If
    End Sub
    Private Sub showMasterCode()
       
        Dim codeType = (From ct In db.tblMasterCode2
         Select New With {ct.MasterCodeID,
                 ct.Type,
                 ct.Code,
                 ct.Description,
                 ct.Note,
                 ct.FilterInd}).ToList()

        If codeType.Count > 0 Then
            Repeater1.DataSource = codeType
            Repeater1.DataBind()
        Else
            Me.Repeater1.DataSource = Nothing
            Me.Repeater1.DataBind()
        End If
    End Sub

    Protected Sub Repeater1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles Repeater1.ItemCommand
        Dim id As String = Session("UserName").ToString
        Dim menu As String = "frmUserProfile"
        Dim index As String = CStr(e.CommandArgument)
        If e.CommandName.Equals("UpdateGroup") Then
            Dim ds1 = From c In db.tblUserMenus Where c.UserName = id And c.Form = menu And c.Edit_ = 1
            If ds1.Any Then
                Response.Write("<script>window.open('UpdateGroup.aspx?ID=" & index & "',target='_self');</script>")
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('คุณไม่มีสิทธ์การแก้ไข')", True)
            End If
            'ElseIf e.CommandName.Equals("viewprofile") Then
            '    Dim ds1 = From c In db.tblUserMenus Where c.UserName = id And c.Form = menu And c.Read_ = 1
            '    If ds1.Any Then
            '        Response.Write("<script>window.open('ViewUserProfile.aspx?UserName=" & index & "',target='_self');</script>")
            '    Else
            '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('คุณไม่มีสิทธ์การแก้ไข')", True)
            '    End If
        End If
    End Sub

End Class