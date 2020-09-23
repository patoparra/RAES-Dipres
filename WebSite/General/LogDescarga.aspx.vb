Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient
Imports Dipres.CG
Imports System.IO
Imports System.Collections.Generic

Partial Class LogDescarga
    Inherits System.Web.UI.Page
    Private _oDtTb As DataTable
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        If IsNothing(Session("Usuario")) Then
            Response.Redirect("http://www.dipres.cl")
        End If
        If Not IsPostBack Then
            carga_combo()

            If paginacionHabilitada() Then
                Session("PaginaActual") = 1
            Else
                Session("PaginaActual") = 0
            End If
        End If
        buscar_Click(sender, e)
        txtFechaDescarga.Attributes.Add("readonly", "readonly")
    End Sub
    Private Sub carga_Tabla(ByVal paramMes As String, ByVal paramPeriodo As String, ByVal paramUsuario As String, ByVal fd As Date)

        'Paginación.
        Dim pagina As Int32 = CInt(Session("PaginaActual"))
        Dim registrosPorPagina As Int32 = getRegistrosPorPagina()
        Dim totalRegistros As Int32 = 0

        _oDtTb = Business.Protocolo.ListarLogDescarga(paramMes, paramPeriodo, paramUsuario, fd, pagina, registrosPorPagina)

        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("IdArchivo", GetType(String)), New DataColumn("IdDescarga", GetType(String)), New DataColumn("NombreArchivo", GetType(String)), New DataColumn("Fecha", GetType(String)), New DataColumn("NombreUsuario", GetType(String)), New DataColumn("FechaDescarga", GetType(String)), New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String)), New DataColumn("estado", GetType(String)), New DataColumn("urlDocto", GetType(String)), New DataColumn("vigencia", GetType(String))})

        'Paginación.
        If pagina <> 0 And registrosPorPagina <> 0 And Not IsNothing(_oDtTb) And _oDtTb.Rows.Count > 0 Then
            Session("totalRegistros") = CInt(_oDtTb.Rows(0)("totalRegistros"))
        Else
            Session("totalRegistros") = 0
        End If

        For Each row As DataRow In _oDtTb.Rows
            Dim nomPublicacion As String
            Dim idArchivo As String = CStr(row("IdArchivo"))
            Dim nomArchivo As String = CStr(row("NombreArchivo"))
            Dim idDescarga As String = CStr(row("id_descarga"))
            Dim idVigencia As String
            If row("vigencia") Is DBNull.Value Then
                idVigencia = "-"
            Else
                If Integer.Parse(CStr(row("vigencia"))) = 0 Then
                    idVigencia = "No Vigente"
                Else
                    idVigencia = "Vigente"
                End If
            End If

            Dim fechaCreacion As String = CStr(row("Fecha"))
            Dim fechaDescarga As String = CStr(row("FechaDescarga_txt"))
            Dim publicacion As Integer = Business.Protocolo.Estado_Archivo(Integer.Parse(idArchivo))
            If (publicacion = 0) Then
                nomPublicacion = "No Publicado"
            Else
                nomPublicacion = "Publicado"
            End If
            Dim nomUsuario As String = CStr(row("NombreUsuario"))
            Dim mes As String = seleccion_mes_nombre(CStr(row("mes")))
            Dim periodo As String = CStr(row("periodo"))
            Dim url As String = ConfigurationManager.AppSettings("urlDocs") & CStr(row("NombreArchivo"))
            dt.Rows.Add(idArchivo, idDescarga, nomArchivo, fechaCreacion, nomUsuario, fechaDescarga, mes, periodo, nomPublicacion, url, idVigencia)
        Next
        GridView1.DataSource = dt
        GridView1.DataBind()
        crearPaginacion()
    End Sub
    Public Shared Function seleccion_mes_nombre(ByVal mes As String) As String
        Dim valor As String = "Otra"
        Select Case mes
            Case "1"
                valor = "31 de marzo"
            Case "2"
                valor = "30 de junio"
            Case "3"
                valor = "30 de septiembre"
            Case "4"
                valor = "31 de diciembre"
            Case "5"
                valor = "Otra"
        End Select
        Return valor
    End Function
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        carga_Tabla("", "", "", Date.MinValue)
    End Sub
    Protected Sub buscar_Click(sender As Object, e As EventArgs) Handles Buscar.Click
        Dim valorCombo = select_periodo.SelectedValue
        Dim valorComboMes = select_mes.SelectedValue
        Dim valorUsuario = select_usuario.SelectedValue
        Dim valorMes As Integer
        selBusqueda.Value = "1"
        valorMes = seleccion_mes(valorComboMes)

        'Obtiene la fecha de descarga.
        Dim fd As Date = obtenerFechaDescarga()

        If sender.GetType() Is GetType(Button) Then
            Session("PaginaActual") = 1
        End If

        carga_Tabla(valorMes.ToString, valorCombo, valorUsuario, fd)
    End Sub
    Protected Sub todos_Click(sender As Object, e As EventArgs) Handles Todos.Click
        selBusqueda.Value = "0"
        Session("PaginaActual") = 1
        carga_Tabla("", "", "", Date.MinValue)
    End Sub
    Public Shared Function seleccion_mes(ByVal mes As String) As Integer
        Dim valor As Integer
        Select Case mes
            Case "31 de marzo"
                valor = 1
            Case "30 de junio"
                valor = 2
            Case "30 de septiembre"
                valor = 3
            Case "31 de diciembre"
                valor = 4
            Case "Otra"
                valor = 5
        End Select
        Return valor
    End Function
    Private Sub carga_combo()
        Dim valorMes As Integer = Integer.Parse(Date.Now.Date.ToString("MM"))
        Dim valorAnio As Integer = Integer.Parse(Date.Now.Date.ToString("yyyy"))
        valorMes = valorMes - 1
        Dim array_meses As String() = {"TODOS", "31 de marzo", "30 de junio", "30 de septiembre", "31 de diciembre", "Otra"}

        For i As Integer = 0 To array_meses.Length - 1
            select_mes.Items.Insert(i, array_meses(i))
        Next
        select_mes.SelectedIndex = 0

        Dim contador As Integer = 0
        Dim contador2 As Integer = 0
        For x As Integer = 2015 To valorAnio
            contador2 = contador
            select_periodo.Items.Insert(contador, x.ToString())
            contador = contador + 1
        Next
        select_periodo.SelectedIndex = contador2

        _oDtTb = Business.Protocolo.Listar_Usuarios_Descarga()

        Dim codigo As String = String.Empty
        Dim nombre As String = String.Empty

        For Each row As DataRow In _oDtTb.Rows
            codigo = CStr(row("codigo"))
            nombre = CStr(row("nombre"))
            select_usuario.Items.Add(New ListItem(nombre, codigo))
        Next
    End Sub

    'Rescata la fecha de descarga. 
    Protected Function obtenerFechaDescarga() As Date
        mostrarMensaje(False)
        Dim errores As New List(Of String)()
        Dim fdStr As String = Trim(txtFechaDescarga.Text)

        If fdStr <> "" Then

            Dim tokens As String() = fdStr.Split("-")
            If tokens.Length = 2 Then

                Dim mes As Int32
                Dim anio As Int32

                Try
                    Integer.TryParse(tokens(0), mes)
                    Integer.TryParse(tokens(1), anio)

                    If mes < 1 Or mes > 12 Then
                        errores.Add("El mes en la fecha de descarga debe oscilar entre 1 y 12.")
                        mostrarMensaje(True, errores, True)
                        Return Date.MinValue
                    End If

                    If anio < 2015 Or anio > Date.Now.Year Then
                        errores.Add("El año en la fecha de descarga debe oscilar entre 2015 y " & Date.Now.Year & ".")
                        mostrarMensaje(True, errores, True)
                        Return Date.MinValue
                    End If

                Catch ex As Exception
                    errores.Add("El formato de la fecha de descarga es incorrecto. Ejemplo: 01-2016.")
                    mostrarMensaje(True, errores, True)
                    Return Date.MinValue
                End Try

                'Gatilla la búsqueda en base a la fecha de publicación.
                Return New Date(anio, mes, 1)

            Else
                errores.Add("El formato de la fecha de descarga es incorrecto. Ejemplo: 01-2016.")
                mostrarMensaje(True, errores, True)
                Return Date.MinValue
            End If
        Else
            'Retorna la fecha mínima.
            Return Date.MinValue
        End If
    End Function

    'Despliega un mensaje por pantalla.
    Private Sub mostrarMensaje(ByVal desplegar As Boolean, Optional ByVal mensajes As List(Of String) = Nothing, Optional ByVal esError As Boolean = False)
        If desplegar Then
            divMensaje.Visible = True
        Else
            divMensaje.Visible = False
            Exit Sub
        End If

        'Determina el icono del mensaje.
        Dim clase As String
        If esError Then
            clase = "class='ui-silk ui-silk-error'"
        Else
            clase = "class='ui-silk ui-silk-accept'"
        End If

        'Construye la lista de mensajes.
        Dim ul As String = "<ul style='list-style-type:none;padding:0'>"
        For Each msg As String In mensajes
            Dim icon As String = "<div style='width:16px;height:16px' " + clase + "></div>"
            Dim li As String = "<li>" + icon + msg + " </li>"
            ul = ul + li
        Next
        ul = ul + "</ul>"
        divMensaje.InnerHtml = ul
    End Sub

    Protected Sub select_periodo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles select_periodo.SelectedIndexChanged
        buscar_Click(sender, e)
    End Sub

    Protected Sub select_mes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles select_mes.SelectedIndexChanged
        buscar_Click(sender, e)
    End Sub

    'Indica si la paginación de registros se encuentra activada.
    Private Function paginacionHabilitada() As Boolean
        Dim habilitada As Boolean = False
        Try
            Dim paginar As Int32 = CInt(ConfigurationManager.AppSettings("paginar"))
            If paginar = 1 Then
                habilitada = True
            End If
        Catch ex As Exception
            habilitada = False
        End Try
        Return habilitada
    End Function

    'Indica los registros por página a desplegar.
    Private Function getRegistrosPorPagina() As Int32
        Dim tamanio As Int32 = 0
        Try
            If paginacionHabilitada() Then
                tamanio = CInt(ConfigurationManager.AppSettings("registrosPorPagina"))
            End If
        Catch ex As Exception
            tamanio = 0
        End Try
        Return tamanio
    End Function

    'Construye el tablero de paginación.
    Public Sub crearPaginacion()

        Dim registrosPorPagina As Int32 = getRegistrosPorPagina()

        If paginacionHabilitada() And registrosPorPagina > 0 Then
            pager.Visible = True
            pager.Controls.Clear()

            Dim totalRegistros As Int32 = CInt(Session("totalRegistros"))
            Dim paginaActual As Int32 = CInt(Session("PaginaActual"))

            'Determina el número de páginas.
            Dim paginas As Int32 = Math.Ceiling(totalRegistros / registrosPorPagina)
            If paginas <= 1 Then
                pager.Visible = False
                Exit Sub
            End If
            Session("TotalPaginas") = paginas

            'Botón Página Previa.
            Dim lbPrevious As New LinkButton()
            lbPrevious.ID = "lbPrevious"
            AddHandler lbPrevious.Click, AddressOf Me.previous_Click
            lbPrevious.Text = "<< Página previa"
            lbPrevious.ToolTip = "Página previa"
            lbPrevious.CssClass = "butt"
            pager.Controls.Add(lbPrevious)

            'Si es la primera página el botón Página Previa se deshabilita.
            If paginaActual = 1 Then
                lbPrevious.Enabled = False
            End If

            'Agrega los índices de página.
            For i As Int32 = 1 To paginas

                Dim lbPagina As New LinkButton()
                lbPagina.ID = "lbIndicePagina" & i
                lbPagina.CommandArgument = CStr(i)
                lbPagina.Text = "" & i

                If i = paginaActual Then
                    lbPagina.CssClass = "butt-selected"
                    lbPagina.ToolTip = "Página Actual"
                Else
                    lbPagina.CssClass = "butt"
                    lbPagina.ToolTip = "Ver página " & i
                    AddHandler lbPagina.Click, AddressOf Me.selectPage_Click
                End If
                pager.Controls.Add(lbPagina)

            Next

            'Botón Página Previa.
            Dim lbNext As New LinkButton()
            lbNext.ID = "lbNext"
            AddHandler lbNext.Click, AddressOf Me.next_Click
            lbNext.Text = "Página siguiente >>"
            lbNext.CssClass = "butt"
            lbNext.ToolTip = "Página siguiente"
            pager.Controls.Add(lbNext)

            'Si es la última página el botón Página siguiente se deshabilita.
            If paginaActual = paginas Then
                lbNext.Enabled = False
            End If

        Else
            pager.Controls.Clear()
            pager.Visible = False
        End If
    End Sub

    'Procesa el evento Click del botón Página siguiente.
    Public Sub next_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim paginaActual As Int32 = CInt(Session("PaginaActual"))
        Dim totalPaginas As Int32 = CInt(Session("TotalPaginas"))

        If paginaActual < totalPaginas Then
            paginaActual += 1
            Session("PaginaActual") = paginaActual
            buscar_Click(sender, e)
        End If
    End Sub

    'Proceso el evento del botón Página Anterior.
    Public Sub previous_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim paginaActual As Int32 = CInt(Session("PaginaActual"))
        Dim totalPaginas As Int32 = CInt(Session("TotalPaginas"))

        If paginaActual > 1 Then
            paginaActual -= 1
            Session("PaginaActual") = paginaActual
            buscar_Click(sender, e)
        End If
    End Sub

    'Procesa la selección de un indice de página en el tablero de paginación.
    Public Sub selectPage_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim arg As String = DirectCast(sender, LinkButton).CommandArgument
        Session("PaginaActual") = arg
        If selBusqueda.Value = "0" Then
            carga_Tabla("", "", "", Date.MinValue)
        Else
            buscar_Click(sender, e)
        End If
    End Sub

End Class
