﻿Option Explicit On
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.Data.DataTable
Imports System.Data.SqlClient   'FOR SQL CONNECTION AND COMMAND.
Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel      ' EXCEL APPLICATION.
Imports ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat      ' TO AUTOFORMAT THE SHEET.
Imports System.Drawing.Imaging
Imports System.Security.Cryptography
Imports System.Text

Public Class prodcutfrm
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
    Dim cs As String = "Data Source=ADMINRG-FFQIQKT;Initial Catalog=mainclinicdb;Integrated Security=True"
    Private Sub dbaccessconnection()

        Try
            con.ConnectionString = "Data Source=ADMINRG-FFQIQKT;Initial Catalog=mainclinicdb;Integrated Security=True"
            cmd.Connection = con

        Catch ex As Exception
            MsgBox("DataBase not connected due to the reason because " & ex.Message)
            Me.Dispose()
        End Try
    End Sub
    Private Sub insert()
        dbaccessconnection()
        con.Open()
        cmd.CommandText = "insert into tbl_products(pro_id,P_id,p_name,p_price,p_description,p_typ,p_dte,photo)values('" & pro_txt.Text & "','" & pid_txt.Text & "','" & name_txt.Text & "','" & price_txt.Text & "','" & des_txt.Text & "','" & salpr_txt.Text & "','" & p_dtetxt.Value & "',@photo)"
        Dim ms As New MemoryStream()
        Dim bmpImage As New Bitmap(photo.Image)
        bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
        Dim data As Byte() = ms.GetBuffer()
        Dim p As New SqlParameter("@photo", SqlDbType.Image)
        p.Value = data
        cmd.Parameters.Add(p)
        cmd.ExecuteNonQuery()
       
        con.Close()
    End Sub


    '"insert into tbl_products(pro_id,P_id,p_name,p_price,p_description,p_typ,p_dte,photo)values
    '('" & pro_txt.Text & "','" & pid_txt.Text & "','" & name_txt.Text & "','" & price_txt.Text & "','" & des_txt.Text & "','" & salpr_txt.Text & "','" & p_dtetxt.Value & "',@photo)"

    Private Sub edit()


        Try

        dbaccessconnection()
        con.Open()
        If pid_txt.Text = "" Then
            MessageBox.Show("Empty Id")
            TabControl1.SelectedTab = TabPage2
        Else

            cmd.CommandText = ("UPDATE tbl_products SET  pro_id= '" & pro_txt.Text & "', P_id= '" & pid_txt.Text & "',p_name= '" & name_txt.Text & "',p_price= '" & price_txt.Text & "',p_description= '" & des_txt.Text & "',p_typ= '" & salpr_txt.Text & "',p_dte= '" & p_dtetxt.Value & "',photo=@photo where pro_id=" & pro_txt.Text & "")
            Dim ms As New MemoryStream()
            Dim bmpImage As New Bitmap(photo.Image)
            bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlParameter("@photo", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Data Updated")
            Label25.Text = "Product details updated successfully!"
            con.Close()

        End If
         Catch ex As Exception
            MessageBox.Show("Data Not Updated")
        End Try
    End Sub

    Private Sub autogenerated()

        Dim curValue As Integer
        Dim result As String
        Using con As SqlConnection = New SqlConnection("Data Source=ADMINRG-FFQIQKT;Initial Catalog=mainclinicdb;Integrated Security=True ")
            con.Open()
            Dim cmd = New SqlCommand("select Max(P_id) from tbl_products", con)
            result = cmd.ExecuteScalar().ToString()
            If String.IsNullOrEmpty(result) Then
                result = "PRO-0000"
            End If

            result = result.Substring(3)
            Int32.TryParse(result, curValue)
            curValue = curValue + 1
            result = "PRO" + curValue.ToString("D4")
            pid_txt.Text = result
        End Using
    End Sub
    Private Sub getdata()

        Dim con As New SqlConnection("Data Source=ADMINRG-FFQIQKT;Initial Catalog=mainclinicdb;Integrated Security=True")
        con.Open()
        Dim da As New SqlDataAdapter("Select pro_id,P_id,p_name,p_price,p_description,p_typ,p_dte,photo from tbl_products ", con)
        Dim dt As New DataTable
        da.Fill(dt)
        source2.DataSource = dt
        get_productdata.DataSource = dt
        get_productdata.Refresh()
    End Sub

    Private Sub txtboxid()
        Try
            dbaccessconnection()
            con.Open()
            Dim num As New Integer
            cmd.CommandText = "SELECT MAX(pro_id) FROM tbl_products "
            If (IsDBNull(cmd.ExecuteScalar)) Then
                num = 1
                pro_txt.Text = num.ToString
            Else

                num = cmd.ExecuteScalar + 1
                pro_txt.Text = num.ToString
            End If
            con.Close()
        Catch ex As Exception
            MsgBox("Error" & ex.Message)
            Me.Dispose()
        End Try
    End Sub
    Private Sub prodcutfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        get_productdata.Refresh()
        autogenerated()
        dbaccessconnection()
        getdata()
        txtboxid()
    End Sub

    Private Sub svemem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles svemem.Click

        If Len(Trim(pro_txt.Text)) = 0 Then
            MessageBox.Show("Please select Product ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            pro_txt.Focus()
            Exit Sub
        End If
        If Len(Trim(pid_txt.Text)) = 0 Then
            MessageBox.Show("Please enter product id", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            pid_txt.Focus()
            Exit Sub
        End If

        Try
            MessageBox.Show("Are you sure to add data", "Data Adding", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            insert()
            getdata()
            Label25.Text = "'" & pid_txt.Text & "' products details saved successfully!"
            Label25.ForeColor = System.Drawing.Color.DarkGreen

        Catch ex As Exception
            Label25.Text = "Error while saving '" & pid_txt.Text & "' products details"
            Label25.ForeColor = System.Drawing.Color.Red
            MsgBox("DataBase not connected due to the reason because " & ex.Message)
            'MessageBox.Show("Data already exist, you again select Ticket Details and Try other entry", "Data Invalid, Application is closing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Dispose()
        End Try


    End Sub

    Private Sub uploadbtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uploadbtn.Click
        Try
            With OpenFileDialog1
                .Filter = ("Images |*.png; *.bmp; *.jpg;*.jpeg; *.gif;")
                .FilterIndex = 4
            End With
            'Clear the file name
            OpenFileDialog1.FileName = ""
            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                photo.Image = Image.FromFile(OpenFileDialog1.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click
        productreporfrm.Show()
    End Sub

    Private Sub btnupdte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnupdte.Click
        edit()
        getdata()
    End Sub

    Private Sub Btndel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btndel.Click
        TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub get_productdata_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles get_productdata.CellContentClick
        TabControl1.SelectedTab = TabPage1
        svemem.Enabled = False
        Btndel.Enabled = True
        btnupdte.Enabled = True
    End Sub

    Private Sub get_productdata_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles get_productdata.CellMouseClick
        Try

            svemem.Enabled = False
            Btndel.Enabled = True
            btnupdte.Enabled = True
            Me.pro_txt.Text = get_productdata.CurrentRow.Cells(0).Value.ToString
            Me.pid_txt.Text = get_productdata.CurrentRow.Cells(1).Value.ToString
            Me.name_txt.Text = get_productdata.CurrentRow.Cells(2).Value.ToString
            Me.price_txt.Text = get_productdata.CurrentRow.Cells(3).Value.ToString
            Me.des_txt.Text = get_productdata.CurrentRow.Cells(4).Value.ToString
            Me.salpr_txt.Text = get_productdata.CurrentRow.Cells(5).Value.ToString
            Me.p_dtetxt.Value = get_productdata.CurrentRow.Cells(6).Value.ToString
         

            'image
            Dim i As Integer
            i = get_productdata.CurrentRow.Index
            Dim bytes As [Byte]() = (Me.get_productdata.Item(7, i).Value)
            Dim ms As New MemoryStream(bytes)
            photo.Image = Image.FromStream(ms)
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
        For i = Me.get_productdata.SelectedRows.Count - 1 To 0 Step -1
            ObjCommand.CommandText = "delete from tbl_products where pro_id='" & get_productdata.SelectedRows(i).Cells("pro_id").Value & "'"
            ObjConnection.Open()
            ObjCommand.ExecuteNonQuery()
            ObjConnection.Close()
            Me.get_productdata.Rows.Remove(Me.get_productdata.SelectedRows(i))
        Next

    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        DeleteSelecedRows()
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Me.Dispose()
    End Sub
    
    Private Sub clear()
        Try
            pid_txt.Text = ""
            name_txt.Text = ""
            price_txt.Text = ""
            des_txt.Text = ""
            salpr_txt.Text = ""
            p_dtetxt.Value = ""
            photo.Image = Nothing

        Catch ex As Exception
            MsgBox("Error:Some thing is going wrong,Close application and try again")
        End Try
    End Sub
    Private Sub Btnadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnadd.Click
        clear()
        txtboxid()
    End Sub
End Class
'("Select pro_id,P_id,p_name,p_price,p_description,p_typ,p_dte,photo from tbl_products ", con)