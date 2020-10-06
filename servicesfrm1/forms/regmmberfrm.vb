﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.Data.DataTable
Imports System.Data.SqlClient

Public Class regmmberfrm
    Private bitmap As Bitmap 'for print grid
    Dim rdr As SqlDataReader
    Dim colColors As Collection = New Collection 'for color of listbox
    Dim provider As String  'for access and sql same
    Dim dataFile As String  'for access and sql same
    Dim connString As String   'for access and sql same
    ' Dim myConnection As OleDbConnection = New OleDbConnection   'for access replace it  Dim myConnection As SqlConnection = New SqlConnection
    Dim myConnection As SqlConnection = New SqlConnection
    Dim ds As DataSet = New DataSet            'for access and sql same
    ' Dim da As OleDbDataAdapter                'for access replace it with Dim da As SqlDataAdapter
    Dim da As SqlDataAdapter
    Dim tables As DataTableCollection = ds.Tables  'for access and sql same
    Dim source1 As New BindingSource()                    'for access and sql same
    Dim source2 As New BindingSource()
    Dim con As New SqlClient.SqlConnection                      'for sql
    Dim cmd As New SqlClient.SqlCommand                        'for sql

    Dim dt As New DataTable
    Dim cs As String = "Data Source=GEO;Initial Catalog=mainclinicdb;Integrated Security=True"
    Private Sub dbaccessconnection()

        Try
            con.ConnectionString = cs
            cmd.Connection = con
            'MessageBox.Show(con.State.ToString())
            'MsgBox("DataBase connected ")
        Catch ex As Exception
            MsgBox("DataBase not connected due to the reason because " & ex.Message)
            Me.Dispose()
        End Try
    End Sub
    Private Sub regmmberfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getdata()
        memberdata.Refresh()
        autogenerated()
        dbaccessconnection()
        getdata()
        txtboxid()
        autogenerated()
    End Sub

    Private Sub insert()
        dbaccessconnection()
        con.Open()
        cmd.CommandText = "insert into tbl_memberreg(mid,M_ID,m_name,m_contactinfo,m_age,m_address,m_dte)values('" & mid_txt.Text & "','" & midtxt.Text & "','" & nametxt.Text & "','" & cntcttxt.Text & "','" & agetxt.Text & "','" & addresstxt.Text & "','" & m_dtetxt.Value & "')"
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
  
    
    Private Sub getdata()

        Dim con As New SqlConnection(cs)
        con.Open()
        Dim da As New SqlDataAdapter("Select mid,M_ID,m_name,m_contactinfo,m_age,m_address,m_dte from tbl_memberreg ", con)
        Dim dt As New DataTable
        da.Fill(dt)
        source2.DataSource = dt
        memberdata.DataSource = dt
        memberdata.Refresh()
    End Sub
    Private Sub txtboxid()
        Try
            dbaccessconnection()
            con.Open()
            Dim num As New Integer
            cmd.CommandText = "SELECT MAX(mid) FROM tbl_memberreg "
            If (IsDBNull(cmd.ExecuteScalar)) Then
                num = 1
                mid_txt.Text = num.ToString
            Else

                num = cmd.ExecuteScalar + 1
                mid_txt.Text = num.ToString
            End If
            con.Close()
        Catch ex As Exception
            MsgBox("Error" & ex.Message)
            Me.Dispose()
        End Try
    End Sub
    Private Sub autogenerated()

        Dim curValue As Integer
        Dim result As String
        Using con As SqlConnection = New SqlConnection(cs)
            con.Open()
            Dim cmd = New SqlCommand("select Max(M_ID) from tbl_memberreg", con)
            result = cmd.ExecuteScalar().ToString()
            If String.IsNullOrEmpty(result) Then
                result = "MEM-0000"
            End If

            result = result.Substring(3)
            Int32.TryParse(result, curValue)
            curValue = curValue + 1
            result = "MEM" + curValue.ToString("D4")
            midtxt.Text = result
        End Using
    End Sub
    Private Sub clear()
        Try

            nametxt.Text = ""
            cntcttxt.Text = ""
            agetxt.Text = ""

            addresstxt.Text = ""


        Catch ex As Exception
            MsgBox("Error:Some thing is going wrong,Close application and try again")
        End Try
    End Sub
    Private Sub edit()


        Try

            dbaccessconnection()

            con.Open()
            If nametxt.Text = "" Then
                MessageBox.Show("Empty Name")
                TabControl1.SelectedTab = TabPage2
            Else

                cmd.CommandText = ("UPDATE tbl_memberreg SET  mid= '" & mid_txt.Text & "', M_ID= '" & midtxt.Text & "',m_name= '" & nametxt.Text & "',m_contactinfo= '" & cntcttxt.Text & "',m_age= '" & agetxt.Text & "',m_address= '" & addresstxt.Text & "',m_dte= '" & m_dtetxt.Value & "' where mid=" & mid_txt.Text & "")
                cmd.ExecuteNonQuery()
                MessageBox.Show("Data Updated")
                Label25.Text = "Members details updated successfully!"
                memberdata.Refresh()
                con.Close()

            End If
        Catch ex As Exception
            MessageBox.Show("Data Not Updated")
        End Try
    End Sub
    Private Sub svemem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles svemem.Click

        If Len(Trim(nametxt.Text)) = 0 Then
            MessageBox.Show("Please select Product ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            nametxt.Focus()
            Exit Sub
        End If
       

        Try
            MessageBox.Show("Are you sure to add data", "Data Adding", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            insert()
            getdata()
            memberdata.Refresh()
            Label25.Text = "'" & nametxt.Text & "' members details saved successfully!"
            Label25.ForeColor = System.Drawing.Color.DarkGreen

        Catch ex As Exception
            Label25.Text = "Error while saving '" & nametxt.Text & "' members details"
            Label25.ForeColor = System.Drawing.Color.Red
            MsgBox("DataBase not connected due to the reason because " & ex.Message)
            'MessageBox.Show("Data already exist, you again select Ticket Details and Try other entry", "Data Invalid, Application is closing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Dispose()
        End Try

    End Sub


    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click
        getmembrdata.Show()
    End Sub


    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Me.Dispose()
    End Sub

    Private Sub Btnadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnadd.Click
        Clear()
        txtboxid()
        autogenerated()
        memberdata.Refresh()
        svemem.Enabled = True
    End Sub

    Private Sub Btndel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btndel.Click
        TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub btnupdte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnupdte.Click
        edit()
        getdata()
        memberdata.Refresh()
    End Sub

    Private Sub memberdata_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles memberdata.CellContentClick
        TabControl1.SelectedTab = TabPage1
        svemem.Enabled = False
        Btndel.Enabled = True
        btnupdte.Enabled = True
    End Sub

    Private Sub memberdata_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles memberdata.CellMouseClick
        Try

            svemem.Enabled = False
            Btndel.Enabled = True
            btnupdte.Enabled = True
            Me.mid_txt.Text = memberdata.CurrentRow.Cells(0).Value.ToString
            Me.midtxt.Text = memberdata.CurrentRow.Cells(1).Value.ToString
            Me.nametxt.Text = memberdata.CurrentRow.Cells(2).Value.ToString
            Me.cntcttxt.Text = memberdata.CurrentRow.Cells(3).Value.ToString
            Me.agetxt.Text = memberdata.CurrentRow.Cells(4).Value.ToString

            Me.addresstxt.Text = memberdata.CurrentRow.Cells(5).Value.ToString
            Me.m_dtetxt.Value = memberdata.CurrentRow.Cells(6).Value.ToString


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub DeleteSelecedRows()
        Dim ObjConnection As New SqlConnection()
        Dim i As Integer
        Dim mResult
        mResult = MsgBox("Want you really delete the selected records?", _
        vbYesNo + vbQuestion, "Removal confirmation")
        If mResult = vbNo Then
            Exit Sub
        End If
        ObjConnection.ConnectionString = cs
        Dim ObjCommand As New SqlCommand()
        ObjCommand.Connection = ObjConnection
        For i = Me.memberdata.SelectedRows.Count - 1 To 0 Step -1
            ObjCommand.CommandText = "delete from tbl_memberreg where mid='" & memberdata.SelectedRows(i).Cells("mid").Value & "'"
            ObjConnection.Open()
            ObjCommand.ExecuteNonQuery()
            ObjConnection.Close()
            Me.memberdata.Rows.Remove(Me.memberdata.SelectedRows(i))
        Next

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        DeleteSelecedRows()
    End Sub
End Class