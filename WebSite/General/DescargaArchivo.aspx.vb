Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient
Imports Dipres.CG
Imports System.IO
Partial Class DescargaArchivo
    Inherits System.Web.UI.Page
    Dim Perfil As Integer
    Dim Usuario As String
    Dim Contrasena As String
    Dim encripto As New ClsEncriptar.Encripto
    Private _oDtTb As DataTable

    Private resultado As Integer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.}
        If IsNothing(Session("Usuario")) Then
            Response.Redirect("http://www.dipres.cl")
        End If

        If Not IsPostBack Then
            Dim param1 As String = Request.QueryString("notificacion")
            Dim elimina As String = Request.QueryString("elimina")
            Dim descarga As String = Request.QueryString("descarga")
            Dim customSetting As String = encripto.Desencriptar(ConfigurationManager.AppSettings("urlUpload"))
            Dim nombreArchivo As String = Request.QueryString("nombreArchivo")
            Dim idArchivo As String = Request.QueryString("idArchivo")
            If descarga = "1" Then

                Business.Protocolo.grabaDescarga(Integer.Parse(idArchivo), nombreArchivo, Session("Usuario").ToString())

            End If

            If paginacionHabilitada() Then
                Session("PaginaActual") = 1
            Else
                Session("PaginaActual") = 0
            End If
            If txtFechaPublicacion.Text <> String.Empty Then
                carga_Tabla(CDate(txtFechaPublicacion.Text))
            Else
                carga_Tabla()
            End If
        End If

        Session("menu") = 2

        crearPaginacion()
        InitializeComponent()
        txtFechaPublicacion.Attributes.Add("readonly", "readonly")
    End Sub
  
    Private Function ConvertBytesToMB(ByVal bytes As Int64) As Double
        Dim mb As Double = bytes / 1048576
        Return mb
    End Function
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
    Private Sub carga_Tabla(Optional ByVal fechaPublicacion As Date = Nothing)

        Dim pagina As Int32 = CInt(Session("PaginaActual"))
        Dim registrosPorPagina As Int32 = getRegistrosPorPagina()
        Dim totalRegistros As Int32 = 0

        'Si no viene la fecha de publicación, esta asume la fecha mínima. 
        If fechaPublicacion.Year = 1 And fechaPublicacion.Month = 1 Then
            fechaPublicacion = New Date(1753, 1, 1)
        End If

        Dim idUsuario As Int32 = Convert.ToInt32(Session("IdUsuario"))
        _oDtTb = Business.Protocolo.Listar_Archivos_Analista(idUsuario, fechaPublicacion, pagina, registrosPorPagina)
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("IdArchivo", GetType(String)), New DataColumn("NombreArchivo", GetType(String)), New DataColumn("peso", GetType(String)), New DataColumn("descripcion", GetType(String)), New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String)), New DataColumn("urlDocto", GetType(String)), New DataColumn("fechaPublicacion", GetType(String))})

        If pagina <> 0 And registrosPorPagina <> 0 And Not IsNothing(_oDtTb) And _oDtTb.Rows.Count > 0 Then
            Session("totalRegistros") = CInt(_oDtTb.Rows(0)("totalRegistros"))
        End If

        For Each row As DataRow In _oDtTb.Rows
            Dim idArchivo As String = CStr(row("IdArchivo"))
            Dim nomArchivo As String = CStr(row("NombreArchivo"))
            Dim peso As String = String.Empty
            Dim descripcion As String = CStr(row("descripcion"))
            If ((CInt(row("peso") / 1024)) > 0) Then
                peso = (CInt(row("peso") / 1024)).ToString() + " KB"
            Else
                peso = (CInt(row("peso") / 1024)).ToString() + " MB"
            End If

            Dim mes As String = seleccion_mes_nombre(CStr(row("mes")))
            Dim periodo As String = CStr(row("periodo"))
            Dim url As String = ConfigurationManager.AppSettings("urlDocs") & CStr(row("NombreArchivo"))
            Dim fechaPub As String
            If row("fecha_publicacion") Is DBNull.Value Then
                fechaPub = "-"
            Else
                fechaPub = CStr(row("fecha_publicacion"))
            End If

            dt.Rows.Add(idArchivo, nomArchivo, peso, descripcion, mes, periodo, url, fechaPub)
        Next
        GridView1.DataSource = dt
        GridView1.DataBind()
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
    Public Shared Function seleccion_mes_nombre(ByVal mes As String) As String
        Dim valor As String = String.Empty
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

    'Busca los archivos cargados aplicando el valor de los filtros. 
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        mostrarMensaje(False)
        Dim fpStr As String = Trim(txtFechaPublicacion.Text)

        If fpStr <> "" Then

            Dim tokens As String() = fpStr.Split("-")
            If tokens.Length = 2 Then

                Dim mes As Int32
                Dim anio As Int32
                Dim msgError As String = ""

                Try
                    Integer.TryParse(tokens(0), mes)
                    Integer.TryParse(tokens(1), anio)

                    If mes < 1 Or mes > 12 Then
                        msgError = "El mes en la fecha de publicación debe oscilar entre 1 y 12."
                        mostrarMensaje(True, msgError, True)
                        Exit Sub
                    End If

                    If anio < 2015 Or anio > Date.Now.Year Then
                        msgError = "El año en la fecha de publicación debe oscilar entre 2015 y " & Date.Now.Year & "."
                        mostrarMensaje(True, msgError, True)
                        Exit Sub
                    End If

                Catch ex As Exception
                    msgError = "El formato de la fecha de publicación es incorrecto. Ejemplo: 01-2016."
                    mostrarMensaje(True, msgError, True)
                    Exit Sub
                End Try

                'Gatilla la búsqueda en base a la fecha de publicación.
                Dim fechaPublicacion As New Date(anio, mes, 1)
                carga_Tabla(fechaPublicacion)

            Else
                Dim msgError As String = "El formato de la fecha de publicación es incorrecto. Ejemplo: 01-2016."
                mostrarMensaje(True, msgError, True)
                Exit Sub
            End If
        Else
            'Busca sin filtrar por la fecha de publicación.
            carga_Tabla()
        End If
    End Sub

    'Despliega un mensaje por pantalla.
    Private Sub mostrarMensaje(ByVal desplegar As Boolean, Optional ByVal mensaje As String = Nothing, Optional ByVal esError As Boolean = False)
        If desplegar Then
            divMensaje.Visible = True
        Else
            divMensaje.Visible = False
            Exit Sub
        End If

        divIcon.Attributes.Remove("class")
        If esError Then
            divIcon.Attributes.Add("class", "ui-silk ui-silk-error")
        Else
            divIcon.Attributes.Add("class", "ui-silk ui-silk-accept")
        End If
        lblMensaje.Visible = True
        lblMensaje.Text = mensaje
    End Sub

    'Registra la descarga de un archivo.
    <Services.WebMethod(EnableSession:=True)> Public Shared Function registrarDescarga(ByVal nombreArchivo As String, ByVal idArchivo As Int32) As String
        Dim r As New Data.Respuesta()
        Dim nombreUsuario As String = HttpContext.Current.Session("Usuario").ToString()
        r.exito = Business.Protocolo.grabaDescarga(Integer.Parse(idArchivo), nombreArchivo, nombreUsuario)
        Dim s As New Script.Serialization.JavaScriptSerializer()
        Return s.Serialize(r)
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

    'Proceso el evento del botón Página Anterior.
    Public Sub previous_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim paginaActual As Int32 = CInt(Session("PaginaActual"))
        Dim totalPaginas As Int32 = CInt(Session("TotalPaginas"))

        If GridView1.Rows.Count <> 0 Then
            If paginaActual > 1 Then
                paginaActual -= 1
                Session("PaginaActual") = paginaActual
                carga_Tabla()
            End If
        End If
    End Sub

    'Procesa el evento Click del botón Página siguiente.
    Public Sub next_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim paginaActual As Int32 = CInt(Session("PaginaActual"))
        Dim totalPaginas As Int32 = CInt(Session("TotalPaginas"))

        If GridView1.Rows.Count <> 0 Then
            If paginaActual < totalPaginas Then
                paginaActual += 1
                Session("PaginaActual") = paginaActual
                carga_Tabla()
            End If
        End If

    End Sub

    'Procesa la selección de un indice de página en el tablero de paginación.
    Public Sub selectPage_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim arg As String = DirectCast(sender, LinkButton).CommandArgument

        If GridView1.Rows.Count <> 0 Then
            Session("PaginaActual") = arg
            carga_Tabla()
        End If
    End Sub

    'Recarga la tabla con el Historial de Carga.
    Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        carga_Tabla()
    End Sub

End Class
