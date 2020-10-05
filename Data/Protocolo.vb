Imports System.Data
Imports System.Data.SqlClient
Imports Dipres.CG.Data.Comun
Imports Dipres.CG

Public NotInheritable Class Protocolo
    ' Since this class provides only static methods, make the default constructor private to prevent 
    ' instances from being created with "new MyClass()".
    Private Sub New()
    End Sub ' New
    Public Shared Function Listar_Usuarios_Descarga() As DataTable

        Dim sSQL As String

        sSQL = "SELECT '' as codigo, 'TODOS' as nombre UNION ALL SELECT distinct [da].[NombreUsuario] as codigo, [da].[NombreUsuario] as nombre FROM [tb_protcolab_descarga_archivos] da, [tb_protcolab_archivos] a WHERE [da].[IdArchivo]=[a].[IdArchivo] order by 1"

        Return Data.Comun.CGCommonDb.ExecuteQryToDataTable(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), sSQL)

    End Function

    Public Shared Function Listar_Archivos(ByVal pagina As Int32, ByVal registrosPorPagina As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_ARCHIVOS_PERFIL_ADMINISTRADOR", pagina, registrosPorPagina)
    End Function
    Public Shared Function Listar_Archivos_Analista(ByVal idUsuario As Int32, ByVal fechaPublicacion As Date, ByVal pagina As Int32, ByVal registrosPorPagina As Int32) As DataTable
        Dim dt As DataTable = Nothing
        Try
            Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
            dt = Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_ARCHIVOS_PERFIL_CONSULTA", idUsuario, fechaPublicacion, pagina, registrosPorPagina)

        Catch ex As Exception
        End Try
        Return dt
    End Function
    Public Shared Function Insertar_Archivos(ByVal nombreArchivo As String, ByVal nombreUsuario As String, ByVal periodo As Integer, ByVal mes As Integer, ByVal fechaPer As String, ByVal peso As Integer, ByVal descripcion As String) As Integer
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("NombreArchivo", GetType(String)),
                                              New DataColumn("NombreUsuario", GetType(String)),
                                              New DataColumn("periodo", GetType(Integer)),
                                              New DataColumn("mes", GetType(Integer)),
                                              New DataColumn("fechaPeriodo", GetType(String)),
                                              New DataColumn("peso", GetType(Integer)),
                                              New DataColumn("descripcion", GetType(String))})
        dt.Rows.Add(nombreArchivo, nombreUsuario, periodo, mes, fechaPer, peso, descripcion)
        Comun.CGCommonDbTx.ExecuteNonQueryTxForMoreRowAffected("[INSERTA_ARCHIVOS]", dt)
        Return 1
    End Function
    Public Shared Function EliminaArchivo(ByVal idArchivo As Integer) As Integer
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("IdArchivo", GetType(Integer))})
        dt.Rows.Add(idArchivo)
        Comun.CGCommonDbTx.ExecuteNonQueryTxForMoreRowAffected("[ELIMINA_ARCHIVOS]", dt)
        Return 1
    End Function

    Public Shared Function Estado_Archivo(ByVal idArchivo As Integer) As Integer

        Dim sSQL As String

        sSQL = "SELECT estado FROM [tb_protcolab_archivos] WHERE IdArchivo=" & idArchivo

        Return Integer.Parse(CGCommonDb.ExecuteScalar(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), CommandType.Text, sSQL))

    End Function
    Public Shared Function Actualiza_Estado_Archivo() As Integer
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("IdArchivo", GetType(Integer))})
        dt.Rows.Add(1)
        Comun.CGCommonDbTx.ExecuteNonQueryTxForMoreRowAffected("[MODIFICA_ESTADO_ARCHIVO]", dt)
        Return 1
    End Function
    Public Shared Function Contabiliza_Estado_Archivo() As Integer

        Dim sSQL As String

        sSQL = " SELECT count(IdArchivo)                                    " & _
               " FROM [tb_protcolab_archivos] WHERE estado=0  "

        Return Integer.Parse(CGCommonDb.ExecuteScalar(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), CommandType.Text, sSQL))

    End Function

    Public Shared Function ValidacionCantidadArchivo(ByVal periodo As Integer, ByVal mes As Integer) As Integer

        Dim sSQL As String

        sSQL = "select count(*) from [tb_protcolab_archivos] where periodo =" & periodo & " And mes=" & mes & " AND ([vigencia] is NULL or [vigencia]!=0 ) "

        Return Integer.Parse(CGCommonDb.ExecuteScalar(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), CommandType.Text, sSQL))

    End Function


    Public Shared Function grabaDescarga(ByVal idArchivo As Integer, ByVal nombreArchivo As String, ByVal nombreUsuario As String) As Integer
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("idArchivo", GetType(Integer)), New DataColumn("NombreArchivo", GetType(String)),
                                                    New DataColumn("NombreUsuario", GetType(String))
                                                   })
        dt.Rows.Add(idArchivo, nombreArchivo, nombreUsuario)
        Comun.CGCommonDbTx.ExecuteNonQueryTxForMoreRowAffected("[INGRESA_REGISTRO_DESCARGA]", dt)
        Return 1
    End Function
    Public Shared Function archivosPorPublicar() As DataTable

        Dim sSQL As String

        sSQL = "select NombreArchivo from [tb_protcolab_archivos] where estado=1"

        Return Data.Comun.CGCommonDb.ExecuteQryToDataTable(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), sSQL)

    End Function
    Public Shared Function validaIngresoArchivo(ByVal nombreArchivo As String) As Integer

        Dim sSQL As String
        sSQL = " SELECT COUNT([IdArchivo])                                   " & _
               " FROM [tb_protcolab_archivos] WHERE NombreArchivo LIKE     '" & nombreArchivo & "'"

        Return Integer.Parse(CGCommonDb.ExecuteScalar(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), CommandType.Text, sSQL))

    End Function
    Public Shared Function actualizaVigenciaArchivo(ByVal idArchivo As String, ByVal vigencia As Integer) As Integer

        Dim sSQL As String
        If vigencia = 1 Then
            sSQL = " update [tb_protcolab_archivos] SET vigencia=0,fecha_vigencia=GETDATE(),estado=0 WHERE IdArchivo=" & idArchivo
        Else
            sSQL = " update [tb_protcolab_archivos] SET vigencia=1,fecha_vigencia=GETDATE(),estado=1 WHERE IdArchivo=" & idArchivo
        End If

        CGCommonDb.ExecuteScalar(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), CommandType.Text, sSQL)
        Return 1
    End Function
    Public Shared Function listarArchivosNoVigentes(ByVal mes As String, ByVal periodo As String) As DataTable

        Dim sSQL As String
        If mes = "" And periodo = "" Then
            sSQL = "SELECT [IdArchivo],[NombreArchivo],CONVERT(VARCHAR(10), [Fecha], 105) + ' ' + CONVERT(VARCHAR(8), [Fecha], 108) AS Fecha,[NombreUsuario],[Mes],[Periodo],CONVERT(VARCHAR(10), [fecha_vigencia], 105) + ' ' + CONVERT(VARCHAR(8), [fecha_vigencia], 108) AS fecha_vigencia_txt FROM [tb_protcolab_archivos] where [vigencia]=0  order by [fecha_vigencia] DESC"
        Else
            If mes = 0 Then
                sSQL = "SELECT [IdArchivo],[NombreArchivo],CONVERT(VARCHAR(10), [Fecha], 105) + ' ' + CONVERT(VARCHAR(8), [Fecha], 108)AS Fecha,[NombreUsuario],[Mes],[Periodo],CONVERT(VARCHAR(10), [fecha_vigencia], 105) + ' ' + CONVERT(VARCHAR(8), [fecha_vigencia], 108) AS fecha_vigencia_txt FROM [tb_protcolab_archivos] where [vigencia]=0  order by [fecha_vigencia] DESC"
            Else
                sSQL = "SELECT [IdArchivo],[NombreArchivo],CONVERT(VARCHAR(10), [Fecha], 105) + ' ' + CONVERT(VARCHAR(8), [Fecha], 108)AS Fecha,[NombreUsuario],[Mes],[Periodo],CONVERT(VARCHAR(10), [fecha_vigencia], 105) + ' ' + CONVERT(VARCHAR(8), [fecha_vigencia], 108) AS fecha_vigencia_txt FROM [tb_protcolab_archivos] where [vigencia]=0 AND mes=" & mes & " AND periodo=" & periodo & "   order by [fecha_vigencia] DESC"
            End If

        End If
        Return Data.Comun.CGCommonDb.ExecuteQryToDataTable(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), sSQL)

    End Function
    Public Shared Function ListarCargaAnual() As DataTable

        Dim sSQL As String

        sSQL = "SELECT [IdArchivo],[NombreArchivo], [NombreUsuario],[periodo],[mes],[estado],CONVERT(VARCHAR(10), [fecha_publicacion], 105) as fecha_publicacion,[vigencia] FROM [tb_protcolab_archivos] where ([vigencia] is NULL or [vigencia]!=0 ) AND estado=1 AND mes=12  order by [Fecha] desc"

        Return Data.Comun.CGCommonDb.ExecuteQryToDataTable(Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True), sSQL)

    End Function
    Public Shared Function ListarLogDescarga(ByVal mes As String, ByVal periodo As String, ByVal usuario As String, ByVal fd As Date, Optional ByVal pagina As Int32 = 0, Optional ByVal registrosPorPagina As Int32 = 0) As DataTable

        If fd.Year = 1 Then
            Dim minDate As New Date(1753, 1, 1)
            fd = minDate
        End If
        If periodo = "" Then
            periodo = 0
        End If
        If mes = "" Then
            mes = "0"
        End If
        If usuario = Nothing Then
            usuario = ""
        End If

        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_LOG_DESCARGA", mes, periodo, usuario, fd, pagina, registrosPorPagina)
    End Function

    'Lista el detalle de los usuarios del proceso.
    Public Shared Function listarUsuariosProceso(ByVal periodo As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_USUARIOS_PROCESO", periodo)
    End Function

    'Lista el detalle de los usuarios asociados a un archivo.
    Public Shared Function listarUsuariosArchivo(ByVal idArchivo As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_USUARIOS_ARCHIVO", idArchivo)
    End Function

    'Elimina la relación entre un usuario y un archivo.
    Public Shared Function eliminarUsuarioArchivo(ByVal idUsuArc As Int32) As Int32
        Dim codigo As Int32
        codigo = 0
        Try
            Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
            Data.Comun.CGCommonDb.ExecuteNonQuery(conStr, "ELIMINAR_USUARIO_ARCHIVO", idUsuArc)
            codigo = 1
        Catch ex As Exception
        End Try
        Return codigo
    End Function

    'Guarda la relación de un usuario con un archivo.
    Public Shared Function guardarUsuarioArchivo(ByVal idUsuario As Int32, ByVal idArchivo As Int32, ByVal vigente As Boolean, Optional ByVal fecha_vigencia As Date = Nothing) As Int32
        Dim codigo As Int32
        codigo = 0
        Try
            Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
            If fecha_vigencia = Date.MinValue Then
                Data.Comun.CGCommonDb.ExecuteNonQuery(conStr, "GUARDAR_USUARIO_ARCHIVO", idArchivo, idUsuario, vigente, DBNull.Value)
            Else
                Data.Comun.CGCommonDb.ExecuteNonQuery(conStr, "GUARDAR_USUARIO_ARCHIVO", idArchivo, idUsuario, vigente, fecha_vigencia)
            End If

            codigo = 1
        Catch ex As Exception
        End Try
        Return codigo
    End Function

    'Busca el registro de un archivo en base a los filtros especificados.
    Public Shared Function buscarArchivo(ByVal idArchivo As Int32) As DataTable
        Dim dt As DataTable = Nothing
        Try
            Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
            dt = Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "BUSCAR_ARCHIVO", idArchivo)

        Catch ex As Exception
        End Try
        Return dt
    End Function

    'Lista los contactos de un usuario.
    Public Shared Function listarContactosUsuario(ByVal idUsuario As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
        Dim dt As DataTable = Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_CONTACTOS_USUARIO", idUsuario)
        Return dt
    End Function

    'Guarda el contacto de un usuario.
    Public Shared Function guardarContacto(ByVal contacto As Contacto) As Int32
        Dim codigo As Int32 = 0
        Try
            Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
            Data.Comun.CGCommonDb.ExecuteNonQuery(conStr, "GUARDAR_USUARIO_CONTACTO", contacto.idContacto, contacto.idUsuario, contacto.email, contacto.nombre, contacto.cargo, contacto.telefono)
            codigo = 1
        Catch ex As Exception
            codigo = 0
        End Try
        Return codigo
    End Function

    'Elimina la relación entre un usuario y un contacto.
    Public Shared Function eliminarContacto(ByVal idContacto As Int32) As Int32
        Dim codigo As Int32 = 0
        Try
            Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
            Dim dt As DataTable = Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "ELIMINAR_USUARIO_CONTACTO", idContacto)
            codigo = 1
        Catch ex As Exception
            codigo = 0
        End Try
        Return codigo
    End Function

    'Retorna el nombre de la institución a la que pertenece un usuario.
    Public Shared Function listarUsuarioInstitucion(ByVal idUsuario As Int32) As DataTable
        Dim dt As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB, True)
        dt = Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_USUARIO_INSTITUCION", idUsuario)
        Return dt
    End Function

    'Lista tipo de datos oficiales.
    Public Shared Function listarTipoDatosOficiales(ByVal idUsuarioContacto As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB2, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_TIPO_DATO_OFICIAL", idUsuarioContacto)
    End Function

    'Lista fecha de incio de un tipo de datos oficial.
    Public Shared Function listarFechasInicio(ByVal idTipoDatoOficial As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB2, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_FECHA_INICIO", idTipoDatoOficial)
    End Function

    'Lista fecha de incio de un tipo de datos oficial.
    Public Shared Function listarFechasCierre(ByVal idTipoDatoOficial As Int32) As DataTable
        Dim conStr As String = Comun.CGCommonDb.GetConnectionStringByKeyConfig(Data.Comun.CGCommonDb.KEY_CONFIG_DB2, True)
        Return Data.Comun.CGCommonDb.ExecuteSpToDataTable(conStr, "LISTAR_FECHA_CIERRE", idTipoDatoOficial)
    End Function
End Class
