Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient

Imports Dipres.CG
Imports Dipres.CG.Business
Imports System.IO
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Imports ServiceAgent.Proxy

Partial Class CargaArchivo
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

        If IsNothing(Session("Usuario")) Then
            Response.Redirect("http://www.dipres.cl")
        End If
        txtDescripcion.Attributes.Add("onclick", "javascript:Oculta();")
        If Not IsPostBack Then
            Dim param1 As String = Request.QueryString("notificacion")
            Dim elimina As String = Request.QueryString("elimina")
            Dim descarga As String = Request.QueryString("descarga")
            Dim vigencia As String = Request.QueryString("vigencia")
            Dim customSetting As String = encripto.Desencriptar(ConfigurationManager.AppSettings("urlUpload"))
            Dim idArchivo As String = String.Empty

            If elimina = "1" Then

                Dim nombreArchivo As String = Request.QueryString("nombreArchivo")
                idArchivo = Request.QueryString("idArchivo")

                Business.Protocolo.EliminaArchivo(Integer.Parse(idArchivo))
                My.Computer.FileSystem.DeleteFile(customSetting & nombreArchivo)
            End If

            If descarga = "1" Then
                Dim proceso As New System.Diagnostics.Process
                Dim nombreArchivo As String = Request.QueryString("nombreArchivo")
            End If

            idArchivo = Request.QueryString("idArchivo")
            If idArchivo <> Nothing Then
                Business.Protocolo.actualizaVigenciaArchivo(idArchivo, vigencia)
            End If
            carga_combo()
            carga_tipo_dato_oficial()

            If paginacionHabilitada() Then
                Session("PaginaActual") = 1
            Else
                Session("PaginaActual") = 0
            End If

            Session("menu") = 1

        End If
        carga_Tabla()
        InitializeComponent()
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

    Private Function ConvertBytesToMB(ByVal bytes As Int64) As Double
        Dim mb As Double = bytes / 1048576
        Return mb
    End Function
    'Protected Sub CargaArchivo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cargaArchivo.Click
    '    Dim mensajes As New List(Of String)()
    '    mostrarMensaje(False)
    '    If FileUpload1.HasFile Then
    '        Try
    '            Dim FileName As String
    '            Dim extensiones As String = ConfigurationManager.AppSettings("ExtArchivos").ToLower()
    '            Dim tamanio_extensiones As String = ConfigurationManager.AppSettings("TamanoExtArchivos").ToLower()
    '            Dim array_extensiones As String()
    '            Dim array_tam_extensiones As String()
    '            array_extensiones = extensiones.Split(",")
    '            array_tam_extensiones = tamanio_extensiones.Split(",")
    '            Dim largo As Integer = array_extensiones.Length - 1

    '            Dim valorCombo = select_periodo.SelectedValue
    '            Dim valorComboMes = select_mes.Value
    '            Dim valorMes As Integer

    '            valorMes = seleccion_mes(valorComboMes)
    '            Dim customSetting As String = encripto.Desencriptar(ConfigurationManager.AppSettings("urlUpload"))
    '            Dim val As Double = ConvertBytesToMB(FileUpload1.FileBytes.Length)
    '            Dim Peso As Integer = FileUpload1.FileBytes.Length

    '            Dim valorInt As Integer = CInt(Int(val))
    '            Dim strFilePath As String
    '            Dim strFileExtension As String
    '            strFilePath = customSetting & FileUpload1.FileName
    '            strFileExtension = System.IO.Path.GetExtension(strFilePath)
    '            Dim ing As Integer = 0
    '            Dim archivo As New Archivos()
    '            'FileName = archivo.NormalizaNombre(FileUpload1.FileName)
    '            FileName = FileUpload1.FileName
    '            Dim validaNombre As Integer = validaNombreArchivo(FileName)

    '            If validaNombre > 0 Then
    '                mensajes.Add("El archivo " & FileUpload1.FileName & " ya existe.")
    '                mostrarMensaje(True, mensajes, True)
    '            Else
    '                For i As Integer = 0 To largo
    '                    array_extensiones(i) = array_extensiones(i).Trim()
    '                    array_tam_extensiones(i) = array_tam_extensiones(i).Trim()
    '                    Dim ext As String = array_extensiones(i)
    '                    Dim tamExt As String = array_tam_extensiones(i)

    '                    If valorInt <= Integer.Parse(tamExt) And strFileExtension.ToLower() = ext.ToLower() Then
    '                        ing = 1
    '                        Dim thisDay As Integer = Microsoft.VisualBasic.DateAndTime.Day(Now)
    '                        Dim mes As String
    '                        If (valorMes.ToString().Length = 1) Then
    '                            mes = "0" & valorMes.ToString()
    '                        Else
    '                            mes = valorMes.ToString()
    '                        End If
    '                        Dim fechaPer As String = valorCombo & "-" & mes & "-" & "01" 'thisDay.ToString()
    '                        Convert.ToDateTime(fechaPer)

    '                        Dim customSetting1 As String = encripto.Desencriptar(ConfigurationManager.AppSettings("urlUpload"))
    '                        FileUpload1.SaveAs(customSetting1 & FileName)
    '                        If File.Exists(customSetting1 & FileName) Then
    '                            Business.Protocolo.InsertarArchivo(FileName, Session("Usuario"), valorCombo, valorMes, fechaPer, Peso, txtDescripcion.Value)
    '                            mensajes.Add("El archivo " & FileUpload1.FileName & " fue cargado exitosamente.")
    '                            mostrarMensaje(True, mensajes, False)
    '                            carga_Tabla()
    '                        Else
    '                            mensajes.Add("Ha ocurrido un error al guardar el archivo " & FileUpload1.FileName & ".")
    '                            mostrarMensaje(True, mensajes, True)
    '                        End If
    '                    End If
    '                Next
    '                If ing = 0 Then
    '                    mensajes.Add("El archivo " & FileUpload1.FileName & " no posee un formato valido o su peso es superior al permitido.")
    '                    mostrarMensaje(True, mensajes, True)
    '                End If
    '            End If
    '        Catch ex As Exception
    '            mensajes.Add("ERROR: " & ex.Message.ToString())
    '            mostrarMensaje(True, mensajes, True)
    '        End Try
    '    Else
    '        mensajes.Add("No has seleccionado un archivo.")
    '        mostrarMensaje(True, mensajes, True)
    '    End If
    'End Sub

    Protected Sub CargaArchivo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cargaArchivo.Click
        Dim mensajes As New List(Of String)()
        mostrarMensaje(False)
        If FileUpload1.HasFile Then
            Try
                Dim FileName As String
                Dim extensiones As String = ConfigurationManager.AppSettings("ExtArchivos").ToLower()
                Dim tamanio_extensiones As String = ConfigurationManager.AppSettings("TamanoExtArchivos").ToLower()
                Dim array_extensiones As String()
                Dim array_tam_extensiones As String()
                array_extensiones = extensiones.Split(",")
                array_tam_extensiones = tamanio_extensiones.Split(",")
                Dim largo As Integer = array_extensiones.Length - 1

                Dim valorCombo = select_periodo.SelectedValue
                Dim valorComboMes = select_mes.Value
                Dim valorMes As Integer

                valorMes = seleccion_mes(valorComboMes)
                Dim customSetting As String = encripto.Desencriptar(ConfigurationManager.AppSettings("urlUpload"))
                Dim val As Double = ConvertBytesToMB(FileUpload1.FileBytes.Length)
                Dim Peso As Integer = FileUpload1.FileBytes.Length

                Dim valorInt As Integer = CInt(Int(val))
                Dim strFilePath As String
                Dim strFileExtension As String
                strFilePath = customSetting & FileUpload1.FileName
                strFileExtension = System.IO.Path.GetExtension(strFilePath)
                Dim ing As Integer = 0
                'Dim archivo As New Archivos()
                'FileName = archivo.NormalizaNombre(FileUpload1.FileName)
                FileName = FileUpload1.FileName
                Dim validaNombre As Integer = validaNombreArchivo(FileName)

                If validaNombre > 0 Then
                    mensajes.Add("El archivo " & FileUpload1.FileName & " ya existe.")
                    mostrarMensaje(True, mensajes, True)
                Else
                    For i As Integer = 0 To largo
                        array_extensiones(i) = array_extensiones(i).Trim()
                        array_tam_extensiones(i) = array_tam_extensiones(i).Trim()
                        Dim ext As String = array_extensiones(i)
                        Dim tamExt As String = array_tam_extensiones(i)

                        If valorInt <= Integer.Parse(tamExt) And strFileExtension.ToLower() = ext.ToLower() Then
                            ing = 1
                            Dim thisDay As Integer = Microsoft.VisualBasic.DateAndTime.Day(Now)
                            Dim mes As String
                            If (valorMes.ToString().Length = 1) Then
                                mes = "0" & valorMes.ToString()
                            Else
                                mes = valorMes.ToString()
                            End If
                            Dim fechaPer As String = valorCombo & "-" & mes & "-" & "01" 'thisDay.ToString()
                            Convert.ToDateTime(fechaPer)

                            Dim customSetting1 As String = encripto.Desencriptar(ConfigurationManager.AppSettings("urlUpload"))
                            FileUpload1.SaveAs(customSetting1 & FileName)
                            If File.Exists(customSetting1 & FileName) Then
                                Business.Protocolo.InsertarArchivo(FileName, Session("Usuario"), valorCombo, valorMes, fechaPer, Peso, txtDescripcion.Value)
                                mensajes.Add("El archivo " & FileUpload1.FileName & " fue cargado exitosamente.")
                                mostrarMensaje(True, mensajes, False)
                                carga_Tabla()
                            Else
                                mensajes.Add("Ha ocurrido un error al guardar el archivo " & FileUpload1.FileName & ".")
                                mostrarMensaje(True, mensajes, True)
                            End If
                        End If
                    Next
                    If ing = 0 Then
                        mensajes.Add("El archivo " & FileUpload1.FileName & " no posee un formato valido o su peso es superior al permitido.")
                        mostrarMensaje(True, mensajes, True)
                    End If
                End If
            Catch ex As Exception
                mensajes.Add("ERROR: " & ex.Message.ToString())
                mostrarMensaje(True, mensajes, True)
            End Try
        Else
            mensajes.Add("No has seleccionado un archivo.")
            mostrarMensaje(True, mensajes, True)
        End If
    End Sub

    Public Shared Function validaNombreArchivo(ByVal nombre As String) As Integer
        Dim valor As Integer = Business.Protocolo.validaIngresoArchivo(nombre)
        Return valor
    End Function
    Private Sub carga_Tabla()

        Dim pagina As Int32 = CInt(Session("PaginaActual"))
        Dim registrosPorPagina As Int32 = getRegistrosPorPagina()
        Dim totalRegistros As Int32 = 0

        _oDtTb = Business.Protocolo.Listar_Archivos(pagina, registrosPorPagina)
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("IdArchivo", GetType(String)), New DataColumn("NombreArchivo", GetType(String)), New DataColumn("peso", GetType(String)), New DataColumn("descripcion", GetType(String)), New DataColumn("Fecha", GetType(String)), New DataColumn("NombreUsuario", GetType(String)), New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String)), New DataColumn("estado", GetType(String)), New DataColumn("fechaPublicacion", GetType(String)), New DataColumn("urlDocto", GetType(String)), New DataColumn("vigencia", GetType(String)), New DataColumn("visible", GetType(String)), New DataColumn("estadoVigencia", GetType(String)), New DataColumn("usuariosAsociados", GetType(String))})

        If pagina <> 0 And registrosPorPagina <> 0 And Not IsNothing(_oDtTb) And _oDtTb.Rows.Count > 0 Then
            Session("totalRegistros") = CInt(_oDtTb.Rows(0)("totalRegistros"))
        End If
        Dim periodoActual As Int32 = CInt(ConfigurationManager.AppSettings("periodo"))

        For Each row As DataRow In _oDtTb.Rows

            Dim nomPublicacion As String
            Dim idArchivo As String = CStr(row("IdArchivo"))
            Dim nomArchivo As String = CStr(row("NombreArchivo"))
            Dim peso As String = String.Empty
            Dim descripcion As String = CStr(row("descripcion"))
            Dim estadoVigencia As String = "Vigente"
            Dim usuariosAsociados As String = "5"

            If ((CInt(row("peso") / 1024)) > 0) Then
                peso = (CInt(row("peso") / 1024)).ToString() + " KB"
            Else
                peso = (CInt(row("peso") / 1024)).ToString() + " MB"
            End If

            Dim idVigencia As String
            Dim fechaPub As String
            Dim visible As String
            If row("vigencia") Is DBNull.Value Then
                idVigencia = "-"
            Else
                If Integer.Parse(CStr(row("vigencia"))) = 0 Then
                    idVigencia = "No Vigente"
                Else
                    idVigencia = "Vigente"
                End If
            End If

            If row("Fecha_Publicacion") Is DBNull.Value Then
                fechaPub = "-"
            Else
                fechaPub = CStr(row("Fecha_Publicacion"))
            End If

            Dim fechaCreacion As String = CStr(row("Fecha"))
            Dim fechaPublicacion As String = fechaPub
            Dim publicacion As Integer = Business.Protocolo.Estado_Archivo(Integer.Parse(idArchivo))
            If (publicacion = 0) Then
                nomPublicacion = "No Publicado"
                visible = ""
            Else
                nomPublicacion = "Publicado"
                visible = "display:none;"
            End If
            Dim nomUsuario As String = CStr(row("NombreUsuario"))
            Dim mes As String = seleccion_mes_nombre(CStr(row("mes")))
            Dim periodo As String = CStr(row("periodo"))
            Dim url As String = ConfigurationManager.AppSettings("urlDocs") & CStr(row("NombreArchivo"))
            'Dim url As String = Server.MapPath(ConfigurationManager.AppSettings("urlDocs")) & CStr(row("NombreArchivo"))

            'Estado Vigencia.
            estadoVigencia = Business.Protocolo.getEstadoVigencia(periodoActual, idArchivo)

            'Usuarios Asociados.
            usuariosAsociados = Business.Protocolo.listarUsuariosArchivo(idArchivo).Rows.Count

            dt.Rows.Add(idArchivo, nomArchivo, peso, descripcion, fechaCreacion, nomUsuario, mes, periodo, nomPublicacion, fechaPublicacion, url, idVigencia, visible, estadoVigencia, usuariosAsociados)
        Next

        GridView1.DataSource = dt
        GridView1.DataBind()
        crearPaginacion()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        carga_Tabla()
    End Sub

    Private Sub carga_tipo_dato_oficial()
        Dim idUsuario As Int32 = Convert.ToInt32(Session("id_usuario"))
        select_tipo_dato_oficial.DataSource = Business.Protocolo.listarTipoDatoOficial(1)
        select_tipo_dato_oficial.DataTextField = "nombre_tipo_dato_oficial"
        select_tipo_dato_oficial.DataValueField = "id_tipo_dato_oficial"
        select_tipo_dato_oficial.DataBind()

        Dim idTipoDatoOficial As Int32 = Convert.ToInt64(select_tipo_dato_oficial.SelectedItem.Value)

        select_inicio.DataSource = Business.Protocolo.listarFechasInicio(idTipoDatoOficial)
        select_inicio.DataTextField = "fecha_inicio"
        select_inicio.DataValueField = "id_fecha_inicio"
        select_inicio.DataBind()

        select_mes.DataSource = Business.Protocolo.listarFechasCierre(idTipoDatoOficial)
        select_mes.DataTextField = "fecha_cierre"
        select_mes.DataValueField = "id_fecha_cierre"
        select_mes.DataBind()
    End Sub

    Private Sub carga_combo()
        Dim valorAnio As Integer = Integer.Parse(Date.Now.Date.ToString("yyyy"))
        Dim array_meses As String() = {"31 de marzo", "30 de junio", "30 de septiembre", "31 de diciembre", "Otra"}

        For i As Integer = 0 To array_meses.Length - 1
            select_mes.Items.Insert(i, array_meses(i))
        Next
        select_mes.SelectedIndex = 0
        Dim contador As Integer = 0
        Dim contador2 As Integer = 0
        For x As Integer = 2013 To valorAnio
            contador2 = contador
            select_periodo.Items.Insert(contador, x.ToString())
            contador = contador + 1
        Next
        select_periodo.SelectedIndex = contador2
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

    Protected Sub select_periodo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles select_periodo.SelectedIndexChanged
        'Dim array_meses As String() = {"31 de marzo", "30 de junio", "30 de septiembre", "31 de diciembre", "Otra"}
        'If select_periodo.SelectedValue < Integer.Parse(Date.Now.Date.ToString("yyyy")) Then
        '    Dim valorMes As Integer = array_meses.Length - 1
        '    select_mes.Items.Clear()

        '    For i As Integer = 0 To valorMes - 1
        '        select_mes.Items.Insert(i, array_meses(i))
        '    Next
        '    select_mes.SelectedIndex = valorMes
        'Else
        '    Dim valorMes As Integer = Integer.Parse(Date.Now.Date.ToString("MM"))
        '    valorMes = valorMes - 1
        '    select_mes.Items.Clear()

        '    For i As Integer = 0 To valorMes - 1
        '        select_mes.Items.Insert(i, array_meses(i))
        '    Next
        '    select_mes.SelectedIndex = valorMes
        'End If

    End Sub

    'Lista los usuarios asociados a un archivo.
    <WebMethod()> Public Shared Function listarUsuariosArchivo(ByVal idArchivo As Int32) As String

        Dim periodo As Int32 = Convert.ToInt32(ConfigurationManager.AppSettings("Periodo"))
        Dim usuProc As DataTable = Business.Protocolo.listarUsuariosProceso(periodo)
        Dim usuArch As DataTable = Business.Protocolo.listarUsuariosArchivo(idArchivo)
        Dim usuarios As New List(Of Object)()

        For Each dr1 As DataRow In usuProc.Rows

            Dim found As Boolean = False
            For Each dr2 As DataRow In usuArch.Rows
                If dr1("usuario_tbid") = dr2("id_usuario") Then
                    found = True
                    Dim u = New With {.usuario = dr1("usuario"), .id = dr1("usuario_tbid"), .vigente = dr2("vigente"), .asociado = 1, .fecha_vigencia = dr2("fecha_vigencia")}
                    usuarios.Add(u)
                    Exit For
                End If
            Next
            If Not found Then
                Dim u = New With {.usuario = dr1("usuario"), .id = dr1("usuario_tbid"), .vigente = 0, .asociado = 0, .fecha_vigencia = ""}
                usuarios.Add(u)
            End If
        Next

        Dim s As New JavaScriptSerializer()
        Return s.Serialize(usuarios)
    End Function

    'Guarda los usuarios asociados a un archivo.
    <WebMethod()> Public Shared Function guardarUsuariosArchivo(ByVal idArchivo As Int32, ByVal idUsuarios As List(Of Int32)) As String
        'Obtiene los usuarios asociados al archivo.
        Dim usuArc As DataTable = Business.Protocolo.listarUsuariosArchivo(idArchivo)
        Dim msgGuardar As String = ""
        Dim msgEmail As String = ""
        Dim codigoGuardar As Int32 = 1
        Dim codigoEmail As Int32 = 0
        Dim archivoPublicado As Boolean = False
        'Elimina las relaciones que ya no aplican. 

        For Each dr As DataRow In usuArc.Rows
            Dim id_usuario As Int32 = Convert.ToInt32(dr("id_usuario"))
            If Not idUsuarios.Contains(id_usuario) Then
                'Elimina la relación.
                codigoGuardar = Business.Protocolo.eliminarUsuarioArchivo(Convert.ToInt32(dr("id_usuario_archivo")))

                If codigoGuardar = 0 Then
                    Exit For
                End If
            Else
                idUsuarios.Remove(id_usuario)
            End If
        Next

        'Guarda las nuevas relaciones.
        If codigoGuardar = 1 Then

            For Each id As Int32 In idUsuarios
                codigoGuardar = Business.Protocolo.guardarUsuarioArchivo(id, idArchivo, True, Date.Now)
                If codigoGuardar = 0 Then
                    Exit For
                End If
            Next

        End If

        If codigoGuardar = 1 And idUsuarios.Count > 0 Then

            'Si el archivo objetivo se encuentra publicado.
            Dim dtArc As DataTable = Business.Protocolo.buscarArchivo(idArchivo)
            If Not IsNothing(dtArc) And dtArc.Rows.Count > 0 Then
                Dim drArc As DataRow = dtArc.Rows(0)
                Dim estado As Int32 = CInt(drArc("estado"))
                If estado = 1 Then
                    archivoPublicado = True
                    Dim wcTo As String = CStr(ConfigurationManager.AppSettings("to"))
                    Dim wcFrom As String = ConfigurationManager.AppSettings("from")
                    Dim wcScc As String = ConfigurationManager.AppSettings("cCopia")
                    Dim wsUrl As String = ConfigurationManager.AppSettings("UrlwsSendMail")

                    'Envia un email a los nuevos usuarios asociados.
                    codigoEmail = Business.Protocolo.enviarCorreoUsuario(wcTo, wcFrom, wcScc, wsUrl, idUsuarios, dtArc)
                End If
            End If
        End If

        If codigoGuardar = 1 Then
            msgGuardar = "La información fue guardada exitosamente."

            If idUsuarios.Count > 0 And archivoPublicado Then
                If codigoEmail = 1 Then
                    msgEmail = "Los nuevos usuarios asociados fueron notificados."
                Else
                    msgEmail = "No fue posible notificar a todos los usuarios."
                End If
            End If
            
        Else
            msgGuardar = "Ha ocurrido un error al guardar la información. Por favor contacte al administrador del Sistema."
            msgEmail = ""
        End If

        Dim s As New JavaScriptSerializer()

        Dim r = New With {.exito = codigoGuardar, .msg = msgGuardar & " " & msgEmail}
        Return s.Serialize(r)
    End Function

    'Busca los datos de un archivo en base a los filtros especificados.
    <WebMethod()> Public Shared Function buscarArchivo(ByVal idArchivo As Int32) As String
        Dim dt As DataTable = Business.Protocolo.buscarArchivo(idArchivo)

        Dim exito As Int32 = 0
        Dim nombre As String = String.Empty
        Dim descripcion As String = String.Empty

        If Not IsNothing(dt) And dt.Rows.Count > 0 Then
            exito = 1
            Dim dr As DataRow = dt.Rows(0)
            nombre = dr("NombreArchivo")
            descripcion = dr("descripcion")
        End If

        Dim a = New With {.exito = exito, .nombre = nombre, .descripcion = descripcion}
        Dim s As New JavaScriptSerializer()
        Return s.Serialize(a)
    End Function

    'Guarda la vigencia que posee un archivo y los usuarios que tiene asociado. 
    <WebMethod()> Public Shared Function guardarArchivoVigencia(ByVal idArchivo As Int32, ByVal usuarios As List(Of Int32), ByVal vigencias As List(Of Int32)) As String
        'Obtiene los usuarios asociados al archivo.
        Dim usuArc As DataTable = Business.Protocolo.listarUsuariosArchivo(idArchivo)
        Dim codigo As Int32 = 1

        For i As Int32 = 0 To usuarios.Count - 1

            Dim id1 As Int32 = usuarios(i)
            Dim vi1 As Int32 = vigencias(i)

            For Each dr As DataRow In usuArc.Rows
                Dim id2 As Int32 = Convert.ToInt32(dr("id_usuario"))
                Dim vi2 As Int32 = Convert.ToInt32(dr("vigente"))

                If id1 = id2 And vi1 <> vi2 Then

                    Dim fecha_vigencia As Date = Date.Now
                    If vi1 = 0 Then
                        fecha_vigencia = Date.MinValue
                    End If
                    codigo = Business.Protocolo.guardarUsuarioArchivo(id1, idArchivo, vi1, fecha_vigencia)

                    Exit For
                End If
            Next

            If codigo = 0 Then
                Exit For
            End If

        Next

        Dim mensaje As String = "La información fue guardada exitosamente."
        If codigo = 0 Then
            mensaje = "Ha ocurrido un error al guardar la información. Por favor contacte al Administrados del Sistema."
        End If

        Dim r = New With {.exito = codigo, .mensaje = mensaje}
        Dim s As New JavaScriptSerializer()
        Return s.Serialize(r)
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

    'Publica los archivos cargados (si existen).
    Protected Sub btnPublicar_Click(sender As Object, e As EventArgs) Handles btnPublicar.Click
        mostrarMensaje(False)
        Dim mensajes As New List(Of String)()
        Dim dtNoPublicados As DataTable = Business.Protocolo.obtenerArchivosNoPublicados()

        'Si existen archivos para publicar.
        If dtNoPublicados.Rows.Count > 0 Then

            'Valida que cada archivo a publicar posee a lo menos un usuario asociado.
            For Each dr As DataRow In dtNoPublicados.Rows

                Dim idArchivo As Int32 = CInt(dr("idArchivo"))
                Dim dtUsuarios As DataTable = Business.Protocolo.listarUsuariosArchivo(idArchivo)
                If dtUsuarios.Rows.Count > 0 Then

                    'Rescata los usuarios del proceso.
                    Dim periodo As Int32 = Convert.ToInt32(ConfigurationManager.AppSettings("periodo"))
                    Dim dtUsuProc As DataTable = Business.Protocolo.listarUsuariosProceso(periodo)
                    Dim dictUsu As New Dictionary(Of Int32, String)()
                    For Each drUsu As DataRow In dtUsuProc.Rows
                        dictUsu.Add(Convert.ToInt32(drUsu("usuario_tbid")), Convert.ToString(drUsu("usuario")))
                    Next

                    'Valida que cada usuario posee a lo menos un email registrado.
                    For Each drUsu As DataRow In dtUsuarios.Rows

                        Dim idUsuario As Int32 = Convert.ToInt32(drUsu("id_usuario"))
                        Dim dtCon As DataTable = Business.Protocolo.listarContactosUsuario(idUsuario)
                        If dtCon.Rows.Count = 0 Then
                            Dim msg As String = "El usuario <b>" + dictUsu.Item(idUsuario) + "</b> no posee un correo electrónico de contacto registrado."
                            If Not mensajes.Contains(msg) Then
                                mensajes.Add(msg)
                            End If
                        End If

                    Next

                Else
                    Dim nombreArchivo As String = Convert.ToString(dr("NombreArchivo"))
                    mensajes.Add("El archivo <b>" + nombreArchivo + "</b> no posee usuarios asociados.")
                End If

            Next

            'Si existen errores.
            If mensajes.Count > 0 Then
                'Se muestra al usuario.
                mostrarMensaje(True, mensajes, True)
                Exit Sub
            End If

            'Cambia el estado de los archivos.
            Data.Protocolo.Actualiza_Estado_Archivo()

            'Envía el correo de notificación.
            Try
                Dim wcTo As String = ConfigurationManager.AppSettings("to")
                Dim wcFrom As String = ConfigurationManager.AppSettings("from")
                Dim wcScc As String = ConfigurationManager.AppSettings("cCopia")
                Dim wsUrl As String = ConfigurationManager.AppSettings("UrlwsSendMail")
                Business.Protocolo.enviarCorreoPublicacion(wcTo, wcFrom, wcScc, wsUrl, dtNoPublicados)
            Catch ex As Exception
                mensajes.Add("La publicación de los archivos fue realizada correctamente, sin embargo ha ocurrido un error al intentar notificar a los usuarios. Por favor contacte al administrador del Sistema. El detalle del error es el siguiente: " & ex.Message)
                mostrarMensaje(True, mensajes, True)
                Exit Sub
            End Try

            If dtNoPublicados.Rows.Count > 1 Then
                mensajes.Add("Los archivos fueron publicados exitosamente.")
            Else
                mensajes.Add("El archivo fue publicado exitosamente.")
            End If

            mostrarMensaje(True, mensajes, False)
            carga_Tabla()
        Else
            'Indica al usuario que no existen archivos para publicar.
            mensajes.Add("No existen archivos para publicar.")
            mostrarMensaje(True, mensajes, True)
            Exit Sub
        End If
    End Sub

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

        If paginaActual > 1 Then
            paginaActual -= 1
            Session("PaginaActual") = paginaActual
            carga_Tabla()
        End If
    End Sub

    'Procesa el evento Click del botón Página siguiente.
    Public Sub next_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim paginaActual As Int32 = CInt(Session("PaginaActual"))
        Dim totalPaginas As Int32 = CInt(Session("TotalPaginas"))

        If paginaActual < totalPaginas Then
            paginaActual += 1
            Session("PaginaActual") = paginaActual
            carga_Tabla()
        End If
    End Sub

    'Procesa la selección de un indice de página en el tablero de paginación.
    Public Sub selectPage_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim arg As String = DirectCast(sender, LinkButton).CommandArgument
        Session("PaginaActual") = arg
        carga_Tabla()
    End Sub

    'Recarga la tabla con el Historial de Carga.
    Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        carga_Tabla()
    End Sub

    Protected Sub select_tipo_dato_oficial_SelectedIndexChanged(sender As Object, e As EventArgs) Handles select_tipo_dato_oficial.SelectedIndexChanged
        Dim idTipoDatoOficial As Int32 = Convert.ToInt64(select_tipo_dato_oficial.SelectedItem.Value)

        select_inicio.DataSource = Business.Protocolo.listarFechasInicio(idTipoDatoOficial)
        select_inicio.DataTextField = "fecha_inicio"
        select_inicio.DataValueField = "id_fecha_inicio"
        select_inicio.DataBind()

        select_mes.DataSource = Business.Protocolo.listarFechasCierre(idTipoDatoOficial)
        select_mes.DataTextField = "fecha_cierre"
        select_mes.DataValueField = "id_fecha_cierre"
        select_mes.DataBind()
    End Sub
End Class
