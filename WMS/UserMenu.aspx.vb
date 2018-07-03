﻿Option Explicit On
Option Strict On

Imports System.Linq
Imports System.Management
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Transactions

Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration

Public Class UserMenu
    Inherits System.Web.UI.Page
    Dim db As New LKBWarehouseEntities1_Test

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            showUser()
            ShowCopyUser()
            showMenuList()
            showGroupList()
            showUserList()
            'Else
            '    MsgBox("เกิดความผิดพลาดในการทำงาน", MsgBoxStyle.OkCancel)
        End If

    End Sub

    '----------------------------------------------------Show Data Dropdown UserName Method in User Tab-------------------------------------
    Private Sub showUser()
        Dim user = From u In db.tblUsers
                   Select u.UserName, u.Name
        Try

            ddlUser.DataSource = user.ToList
            ddlUser.DataTextField = "Name"
            ddlUser.DataValueField = "UserName"
            ddlUser.DataBind()
            If ddlUser.Items.Count > 1 Then
                ddlUser.Enabled = True
            Else
                ddlUser.Enabled = False
            End If

        Catch ex As Exception
            'MsgBox("เกิดเหตุผิดพลาด")
        End Try
    End Sub
    '----------------------------------------------------Show Data Dropdown CopyUser Method in User Tab-------------------------------------
    Private Sub ShowCopyUser()
        Dim user = From u In db.tblUsers
                  Select u.UserName, u.Name
        Try

            ddlCopyUser.DataSource = user.ToList
            ddlCopyUser.DataTextField = "UserName"
            ddlCopyUser.DataValueField = "UserName"
            ddlCopyUser.DataBind()
            If ddlCopyUser.Items.Count > 1 Then
                ddlCopyUser.Enabled = True
            Else
                ddlCopyUser.Enabled = False
            End If

        Catch ex As Exception
            'MsgBox("เกิดเหตุผิดพลาด")
        End Try
    End Sub
    Protected Sub Repeater2_ItemCommand(source As Object, e As RepeaterCommandEventArgs)

    End Sub
    Public Sub showUserList()
       
        'Dim formlist = (From u In db.tblMenus
        '                Group By Form = u.Form
        '                Into f = Group, Count())
        Dim formlist = (From u In db.tblUserMenus
                 Select New With {
                     u.Form,
                     u.Read_,
                     u.Save_,
                     u.Edit_,
                     u.Delete_}).ToList

        If formlist.Count > 0 Then
            Repeater1.DataSource = formlist
            Repeater1.DataBind()
        Else
            Me.Repeater1.DataSource = Nothing
            Me.Repeater1.DataBind()
        End If
    End Sub




    '-----------------------------------------------------Start Method Menu Tab-------------------------------------------------
    '---------------------------------------------------Repeater Menu Tab-------------------------------------------------------
    Protected Sub Repeater1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles Repeater1.ItemCommand
        'Dim id As String = Session("UserName").ToString
        'Dim menu As String = "frmUserProfile"
        'Dim index As String = CStr(e.CommandArgument)
        'If e.CommandName.Equals("edituser") Then
        '    Dim ds1 = From c In db.tblUserMenus Where c.UserName = id And c.Form = menu And c.Edit_ = 1
        '    If ds1.Any Then
        '        Response.Write("<script>window.open('UpdateUserProfile.aspx?UserName=" & index & "',target='_self');</script>")
        '    Else
        '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('คุณไม่มีสิทธ์การแก้ไข')", True)
        '    End If
        'ElseIf e.CommandName.Equals("viewprofile") Then
        '    Dim ds1 = From c In db.tblUserMenus Where c.UserName = id And c.Form = menu And c.Read_ = 1
        '    If ds1.Any Then
        '        Response.Write("<script>window.open('ViewUserProfile.aspx?UserName=" & index & "',target='_self');</script>")
        '    Else
        '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('คุณไม่มีสิทธ์การแก้ไข')", True)
        '    End If
        'End If
    End Sub
    '---------------------------------------------------Show Data Method in Menu Tab--------------------------------------
    Public Sub showMenuList()
        Dim formlist = (From u In db.tblMenus
                    Select New With {u.Form,
                                     u.Menu,
                                     u.UserBy}).ToList()

        If formlist.Count > 0 Then
            Repeater2.DataSource = formlist
            Repeater2.DataBind()
        Else
            Me.Repeater2.DataSource = Nothing
            Me.Repeater2.DataBind()
        End If
    End Sub
    '--------------------------------------Click Add Form Method in Menu Tab------------------------------------ 
    Protected Sub btnAddUp1_Click(sender As Object, e As EventArgs) Handles btnAddUp1.ServerClick

        Try
            Using db = New LKBWarehouseEntities1_Test
                Dim Form = (From u In db.tblMenus Where u.Form = txtForm.Value
                Select u).FirstOrDefault

                Dim Menu = (From u In db.tblMenus Where u.Menu = txtMenu.Value
                Select u).FirstOrDefault

                If Not Form Is Nothing Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('ชื่อ Form ซ้ำ กรุณาเปลี่ยนใหม่');", True)
                ElseIf Not Menu Is Nothing Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('ชื่อ Menu ซ้ำ กรุณาเปลี่ยนใหม่');", True)
                Else
                    AddForm()
                End If
            End Using
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('เกิดข้อผิดพลาด');", True)
            Clear()

        End Try
    End Sub
    '-------------------------------------- Add Form Method in Menu Tab------------------------------------ 
    Private Sub AddForm()
        Using tran As New TransactionScope()
            Try
                db.Database.Connection.Open()
                db.tblMenus.Add(New tblMenu() With { _
                                .Form = txtForm.Value.Trim, _
                                .Menu = txtMenu.Value.Trim, _
                                .UserBy = CStr(Session("UserName")), _
                                .UpdateBy = Now})
                db.SaveChanges()
                tran.Complete()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('เพิ่ม user สำเร็จ !');", True)
                Response.Redirect("UserMenu.aspx")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('เกิดข้อผิดพลาด กรุณาบันทึกข้อมูลใหม่อีกครั้ง');", True)
            Finally
                db.Database.Connection.Close()
                db.Dispose()
                tran.Dispose()
            End Try
        End Using
    End Sub
    '----------------------------------------Clear Data Method------------------------------- 
    Private Sub Clear()
        txtForm.Value = ""
        txtMenu.Value = ""
    End Sub
    '-----------------------------------------------------Begin Group Tab Method----------------------------------------------
    '-----------------------------------------------------Show Group Data in Group Tab--------------------------------------
    Public Sub showGroupList()


        Dim grouplist = (From u In db.tblGroupMenus
                    Select New With {u.Form,
                                     u.Menu}).ToList()

        If grouplist.Count > 0 Then
            Repeater3.DataSource = grouplist
            Repeater3.DataBind()

        Else
            Me.Repeater3.DataSource = Nothing
            Me.Repeater3.DataBind()
        End If
    End Sub

    Protected Sub Repeater3_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles Repeater3.ItemCommand

    End Sub

    Private Sub showStatuslist()
        'dcbBranch.Items.Clear()
        'dcbBranch.Items.Add(New ListItem("--select Branch--", ""))
        'dcbBranch.AppendDataBoundItems = True

        'Dim d = From p In db.tblGroupMenus
        '      Select p.Status
        'Try
        '    lblStatus.DataSource = d.ToList
        '    dcbBranch.DataTextField = "Status"
        '    dcbBranch.DataValueField = "Status"
        '    dcbBranch.DataBind()
        '    If dcbBranch.Items.Count > 1 Then
        '        dcbBranch.Enabled = True
        '    Else
        '        dcbBranch.Enabled = False
        '    End If
        'Catch ex As Exception
        '    'Throw ex
        'End Try
    End Sub
    Protected Sub Repeater1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles Repeater1.ItemDataBound
        Dim Read As String
        Dim Save As String
        Dim Edit As String
        Dim Delete As String
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblForm As Label = CType(e.Item.FindControl("lblForm"), Label)
            'Dim cbRead As CheckBox = CType(e.Item.FindControl("cbRead"), CheckBox)
            Dim lblRead As Label = CType(e.Item.FindControl("lblRead"), Label)
            Dim lblRead2 As Label = CType(e.Item.FindControl("lblRead2"), Label)
            Dim lblSave As Label = CType(e.Item.FindControl("lblSave"), Label)
            Dim lblSave2 As Label = CType(e.Item.FindControl("lblSave2"), Label)
            Dim lblEdit As Label = CType(e.Item.FindControl("lblEdit"), Label)
            Dim lblEdit2 As Label = CType(e.Item.FindControl("lblEdit2"), Label)
            Dim lblDelete As Label = CType(e.Item.FindControl("lblDelete"), Label)
            Dim lblDelete2 As Label = CType(e.Item.FindControl("lblDelete2"), Label)
            Read = DataBinder.Eval(e.Item.DataItem, "Read_").ToString()
            Save = DataBinder.Eval(e.Item.DataItem, "Save_").ToString
            Edit = DataBinder.Eval(e.Item.DataItem, "Edit_").ToString
            Delete = DataBinder.Eval(e.Item.DataItem, "Delete_").ToString


            If Not IsNothing(lblForm) Then
                lblForm.Text = DataBinder.Eval(e.Item.DataItem, "Form").ToString

            End If

            If Read = "1" Then
                lblRead.Visible = True
                lblRead2.Visible = False
            ElseIf Read = "0" Then
                lblRead2.Visible = True
                lblRead.Visible = False
            End If

            If Save = "1" Then
                lblSave.Visible = True
                lblSave2.Visible = False
            ElseIf Save = "0" Then
                lblSave2.Visible = True
                lblSave.Visible = False
            End If
            If Edit = "1" Then
                lblEdit.Visible = True
                lblEdit2.Visible = False
            ElseIf Edit = "0" Then
                lblEdit2.Visible = True
                lblEdit.Visible = False
            End If
            If Delete = "1" Then
                lblDelete.Visible = True
                lblDelete2.Visible = False
            ElseIf Delete = "0" Then
                lblDelete2.Visible = True
                lblDelete.Visible = False
            End If
            Dim lblStatus As DropDownList = (TryCast(e.Item.FindControl("lblStatus"), DropDownList))

            If Not IsNothing(lblStatus) Then
                With lblStatus
                    .Items.Add("None")
                    .Items.Add("Read")
                    .Items.Add("Save")
                    .Items.Add("Edit")
                    .Items.Add("Delete")
                End With
            End If

        End If

    End Sub
    'Private Function GetData(ByVal query As String) As DataTable
    '    Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    '    Using con As SqlConnection = New SqlConnection(constr)
    '        Using cmd As SqlCommand = New SqlCommand(query, con)
    '            Using sda As SqlDataAdapter = New SqlDataAdapter(cmd)
    '                Dim dt As DataTable = New DataTable()
    '                sda.Fill(dt)
    '                Return dt
    '            End Using
    '        End Using
    '    End Using
    'End Function

    Protected Sub Repeater2_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles Repeater2.ItemCreated
       
    End Sub

    Protected Sub ddlUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUser.SelectedIndexChanged
  
        Dim formlist = From u In db.tblUserMenus Where u.UserName = ddlUser.Text
                  Select
                     u.Form,
                     u.Read_,
                     u.Save_,
                     u.Edit_,
                     u.Delete_



        If formlist.Count > 0 Then
            Repeater1.DataSource = formlist.ToList
            Repeater1.DataBind()
        Else
            Me.Repeater1.DataSource = Nothing
            Me.Repeater1.DataBind()
        End If
    End Sub

    Protected Sub btnSave_ServerClick(sender As Object, e As EventArgs)
        'If String.IsNullOrEmpty(ddlUser.Text) Then
        '    MsgBox("0")
        'Else
        '    MsgBox(ddlUser.Text)
        'End If
    End Sub


    Protected Sub btnCopy_ServerClick(sender As Object, e As EventArgs)
        Dim Form As String
        Dim Read As Integer
        Dim Save As Integer
        Dim Edit As Integer
        Dim Delete As Integer
        Dim UserCopy As String
        Try
            Dim copy = (From c In db.tblUserMenus Where c.UserName = ddlCopyUser.Text
                 Select c).FirstOrDefault
            UserCopy = copy.UserName
            Form = copy.Form
            Read = CInt(copy.Read_)
            Save = CInt(copy.Save_)
            Edit = CInt(copy.Edit_)
            Delete = CInt(copy.Delete_)
            copyUser(UserCopy, Form, Read, Save, Edit, Delete)
        Catch ex As Exception

        End Try
       
    End Sub

    Private Sub copyUser(UserCopy As String, Form As String, Read As Integer, Save As Integer, Edit As Integer, Delete As Integer)

        'If ddlCopyUser.Text = UserCopy Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('คุณ copy'" + UserCopy + "');", True)
        'Else

        'End If
        Try
            db.tblUserMenus.Add(New tblUserMenu With { _
                                .UserName = ddlUser.Text, _
                                .Form = Form, _
                                .Read_ = Read, _
                                .Save_ = Save, _
                                .Edit_ = Edit, _
                                .Delete_ = Delete, _
                                .UserBy = CStr(Session("UserName")), _
                                .LastUpdate = Now
                            })
        Catch ex As Exception

        End Try

    End Sub
End Class