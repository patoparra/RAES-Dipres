Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient

Imports Dipres.CG
Imports Util
Imports System.IO
Partial Class NoVigentes
    Inherits System.Web.UI.Page
    Private _oDtTb As DataTable
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        carga_Tabla("", "")
        carga_combo()
    End Sub
    Private Sub carga_Tabla(ByVal paramMes As String, ByVal paramPeriodo As String)

        _oDtTb = Business.Protocolo.listarArchivosNoVigentes(paramMes, paramPeriodo)
        'For Each row As DataRow In _oDtTb.Rows

        'Dim valor As String = CStr(row("NombreCampo"))

        'Next
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("IdArchivo", GetType(String)), New DataColumn("NombreArchivo", GetType(String)), New DataColumn("Fecha", GetType(String)), New DataColumn("fecha_vigencia", GetType(String)), New DataColumn("NombreUsuario", GetType(String)), New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String)), New DataColumn("urlDocto", GetType(String)), New DataColumn("vigencia", GetType(String))})
        For Each row As DataRow In _oDtTb.Rows
            Dim nomPublicacion As String
            Dim idArchivo As String = CStr(row("IdArchivo"))
            Dim nomArchivo As String = CStr(row("NombreArchivo"))
            Dim idVigencia As String
            'If row("vigencia") Is DBNull.Value Then
            'idVigencia = "-"
            'Else

            'If Integer.Parse(CStr(row("vigencia"))) = 0 Then
            'idVigencia = "No Vigente"
            'Else
            'idVigencia = "Vigente"
            'End If
            'End If

            Dim fechaCreacion As String = CStr(row("Fecha"))
            Dim fechaVigencia As String = CStr(row("fecha_vigencia_txt"))
            'Dim publicacion As Integer = Business.ISSalud.Estado_Archivo(Integer.Parse(idArchivo))
            'If (publicacion = 0) Then
            'nomPublicacion = "No Publicado"
            'Else
            'nomPublicacion = "Publicado"
            'End If
            Dim nomUsuario As String = CStr(row("NombreUsuario"))
            Dim mes As String = seleccion_mes_nombre(CStr(row("mes")))
            Dim periodo As String = CStr(row("periodo"))
            Dim url As String = ConfigurationManager.AppSettings("urlDocs") & CStr(row("NombreArchivo"))
            dt.Rows.Add(idArchivo, nomArchivo, fechaCreacion, fechaVigencia, nomUsuario, mes, periodo, url)
        Next


        GridView1.DataSource = dt
        GridView1.DataBind()
        'GridView1.PagerStyle.CssClass = "pager"

    End Sub
    Private Sub carga_combo()
        'select_periodo.Items.Insert(0, Date.Now.Date.ToString("yyyy"))
        Dim valorMes As Integer = Integer.Parse(Date.Now.Date.ToString("MM"))
        Dim valorAnio As Integer = Integer.Parse(Date.Now.Date.ToString("yyyy"))
        valorMes = valorMes - 1
        Dim array_meses As String() = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}

        For i As Integer = 0 To 11
            select_mes.Items.Insert(i, array_meses(i))
        Next
        select_mes.SelectedIndex = valorMes
        Dim contador As Integer = 0
        Dim contador2 As Integer = 0
        For x As Integer = 2015 To valorAnio
            contador2 = contador
            select_periodo.Items.Insert(contador, x.ToString())
            contador = contador + 1
        Next
        select_periodo.SelectedIndex = contador2
        'select_mes.Items.Insert(0, "Enero")
        'select_mes.Items.Insert(1, "Febrero")
        'select_mes.Items.Insert(2, "Marzo")
        'select_mes.Items.Insert(3, "Abril")
        'select_mes.Items.Insert(4, "Mayo")
        'select_mes.Items.Insert(5, "Junio")
        'select_mes.Items.Insert(6, "Julio")
        'select_mes.Items.Insert(7, "Agosto")
        'select_mes.Items.Insert(8, "Septiembre")
        'select_mes.Items.Insert(9, "Octubre")
        'select_mes.Items.Insert(10, "Noviembre")
        'select_mes.Items.Insert(11, "Diciembre")
    End Sub
    Public Shared Function seleccion_mes_nombre(ByVal mes As String) As String
        Dim valor As String
        Select Case mes
            Case 1
                valor = "Enero"
            Case 2
                valor = "Febrero"
            Case 3
                valor = "Marzo"
            Case 4
                valor = "Abril"
            Case 5
                valor = "Mayo"
            Case 6
                valor = "Junio"
            Case 7
                valor = "Julio"
            Case 8
                valor = "Agosto"
            Case 9
                valor = "Septiembre"
            Case 10
                valor = "Octubre"
            Case 11
                valor = "Noviembre"
            Case 12
                valor = "Diciembre"
        End Select
        Return valor
    End Function
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        carga_Tabla("", "")
    End Sub
    Protected Sub buscar_Click(sender As Object, e As EventArgs) Handles Buscar.Click
        Dim valorCombo = select_periodo.SelectedValue
        Dim valorComboMes = select_mes.Value
        Dim valorMes As Integer
        selBusqueda.Value = "1"
        valorMes = seleccion_mes(valorComboMes)
        carga_Tabla(valorMes.ToString, valorCombo)
    End Sub
    Protected Sub todos_Click(sender As Object, e As EventArgs) Handles Todos.Click
        selBusqueda.Value = "0"
        carga_Tabla("", "")
    End Sub
    Public Shared Function seleccion_mes(ByVal mes As String) As Integer
        Dim valor As Integer
        Select Case mes
            Case "Enero"
                valor = 1
            Case "Febrero"
                valor = 2
            Case "Marzo"
                valor = 3
            Case "Abril"
                valor = 4
            Case "Mayo"
                valor = 5
            Case "Junio"
                valor = 6
            Case "Julio"
                valor = 7
            Case "Agosto"
                valor = 8
            Case "Septiembre"
                valor = 9
            Case "Octubre"
                valor = 10
            Case "Noviembre"
                valor = 11
            Case "Diciembre"
                valor = 12
        End Select
        Return valor
    End Function
End Class
