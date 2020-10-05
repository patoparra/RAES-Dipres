Imports Dipres.CG.Data
Imports System.Data
Imports ServiceAgent
Imports System.Collections.Generic

Public NotInheritable Class Protocolo
    ' Since this class provides only static methods, make the default constructor private to prevent 
    ' instances from being created with "new MyClass()".
    Private Sub New()
    End Sub ' New
    Public Shared Function Listar_Usuarios_Descarga() As DataTable
        Return Data.Protocolo.Listar_Usuarios_Descarga()
    End Function
    Public Shared Function Listar_Archivos(ByVal pagina As Int32, ByVal registrosPorPagina As Int32) As DataTable
        Return Data.Protocolo.Listar_Archivos(pagina, registrosPorPagina)
    End Function
    Public Shared Function Listar_Archivos_Analista(ByVal idUsuario As Int32, ByVal fechaPublicacion As Date, ByVal pagina As Int32, ByVal registrosPorPagina As Int32) As DataTable
        Return Data.Protocolo.Listar_Archivos_Analista(idUsuario, fechaPublicacion, pagina, registrosPorPagina)
    End Function
    Public Shared Function InsertarArchivo(ByVal nombreArchivo As String, ByVal nombreUsuario As String, ByVal periodo As Integer, ByVal mes As Integer, ByVal fechaPer As String, ByVal Peso As Integer, ByVal descripcion As String) As Integer
        Data.Protocolo.Insertar_Archivos(nombreArchivo, nombreUsuario, periodo, mes, fechaPer, Peso, descripcion)
        Return 1
    End Function
    Public Shared Function EliminaArchivo(ByVal idArchivo As Integer) As Integer
        Data.Protocolo.EliminaArchivo(idArchivo)
        Return 1
    End Function
    Public Shared Function Estado_Archivo(ByVal idArchivo As Integer) As Integer
        Return Data.Protocolo.Estado_Archivo(idArchivo)
    End Function
    Public Shared Function Actualiza_Estado_Archivo() As Integer
        Return Data.Protocolo.Actualiza_Estado_Archivo()
    End Function
    Public Shared Sub EnvioCorreo_Aviso(ByVal sTo As String, ByVal sFrom As String, ByVal sCc As String, ByVal sUrl As String, ByVal archivos As String)
        Dim sMensajeCorreo As String
        Dim sSubject As String
        Dim iImportancia As String

        sSubject = "Información de personal Protocolo Dipres"

        iImportancia = 0 'normal

        Try
            sMensajeCorreo = "Se informa que se ha publicado en web Dipres la siguiente información:<br><br>" & archivos & "<br>Atte.<br><br>Sub-departamento de Estadísticas<br>Dirección de Presupuestos"

            If Not Correo.CallSendMail(sFrom, sTo, sCc, sSubject, sMensajeCorreo, iImportancia, sUrl) Then
                Correo.CallSendMail(sFrom, "icg@dipres.cl", "", "PROBLEMA!!!" & sSubject, sMensajeCorreo & "<br>Originalmente para: [" & sTo & "]", iImportancia, sUrl)
            End If
        Catch ex As Exception
            Throw ex
            Exit Sub
        End Try
    End Sub
    Public Shared Function Contabiliza_Estado_Archivo() As Integer
        Return Data.Protocolo.Contabiliza_Estado_Archivo()
    End Function
    Public Shared Function ValidacionCantidadArchivo(ByVal periodo As Integer, ByVal mes As Integer) As Integer
        Return Data.Protocolo.ValidacionCantidadArchivo(periodo, mes)
    End Function

    Public Shared Function grabaDescarga(ByVal idArchivo As Integer, ByVal nombreArchivo As String, ByVal nombreUsuario As String) As Integer
        Data.Protocolo.grabaDescarga(idArchivo, nombreArchivo, nombreUsuario)
        Return 1
    End Function
    Public Shared Function archivosPorPublicar() As DataTable
        Return Data.Protocolo.archivosPorPublicar()
    End Function
    Public Shared Function validaIngresoArchivo(ByVal nombreArchivo As String) As Integer
        Return Data.Protocolo.validaIngresoArchivo(nombreArchivo)
    End Function
    Public Shared Function actualizaVigenciaArchivo(ByVal idArchivo As String, ByVal vigencia As Integer) As Integer
        Data.Protocolo.actualizaVigenciaArchivo(idArchivo, vigencia)
        Return 1
    End Function
    Public Shared Function listarArchivosNoVigentes(ByVal mes As String, ByVal periodo As String) As DataTable
        Return Data.Protocolo.listarArchivosNoVigentes(mes, periodo)
    End Function
    Public Shared Function ListarCargaAnual() As DataTable
        Return Data.Protocolo.ListarCargaAnual()
    End Function
    Public Shared Function ListarLogDescarga(ByVal mes As String, ByVal periodo As String, ByVal usuario As String, ByVal fd As Date, Optional ByVal pagina As Int32 = 0, Optional ByVal registrosPorPagina As Int32 = 0) As DataTable
        Return Data.Protocolo.ListarLogDescarga(mes, periodo, usuario, fd, pagina, registrosPorPagina)
    End Function

    'Lista el detalle de los usuarios del proceso.
    Public Shared Function listarUsuariosProceso(ByVal periodo As Int32) As DataTable
        Return Data.Protocolo.listarUsuariosProceso(periodo)
    End Function

    'Lista el detalle de los usuarios asociados a un Archivo.
    Public Shared Function listarUsuariosArchivo(ByVal idArchivo As Int32) As DataTable
        Return Data.Protocolo.listarUsuariosArchivo(idArchivo)
    End Function

    'Elimina la relación entre un usuario y un archivo.
    Public Shared Function eliminarUsuarioArchivo(ByVal idUsoArc As Int32) As Int32
        Return Data.Protocolo.eliminarUsuarioArchivo(idUsoArc)
    End Function

    'Guarda la relación entre un usuario y un archivo. Si existe la actualiza.
    Public Shared Function guardarUsuarioArchivo(ByVal idUsuario As Integer, ByVal idArchivo As Int32, ByVal vigente As Boolean, Optional ByVal fecha_vigencia As Date = Nothing) As Int32
        Return Data.Protocolo.guardarUsuarioArchivo(idUsuario, idArchivo, vigente, fecha_vigencia)
    End Function

    'Busca el registro de un archivo en base a los filtros especificados.
    Public Shared Function buscarArchivo(ByVal idArchivo As Int32) As DataTable
        Return Data.Protocolo.buscarArchivo(idArchivo)
    End Function

    'Lista los contactos de un usuario.
    Public Shared Function listarContactosUsuario(ByVal idUsuario As Int32) As DataTable
        Return Data.Protocolo.listarContactosUsuario(idUsuario)
    End Function

    'Guarda la relación entre un usuario y un contacto.
    Public Shared Function guardarContacto(ByVal contacto As Contacto) As Int32
        Return Data.Protocolo.guardarContacto(contacto)
    End Function

    'Elimina la relación entre un usuario y un contacto.
    Public Shared Function eliminarContacto(ByVal idContacto As Int32) As Int32
        Return Data.Protocolo.eliminarContacto(idContacto)
    End Function

    'Retorna el nombre de la institución a la que pertenece un usuario.
    Public Shared Function listarUsuarioInstitucion(ByVal idUsuario As Int32) As DataTable
        Return Data.Protocolo.listarUsuarioInstitucion(idUsuario)
    End Function

    'Retorna los archivos no publicados.
    Public Shared Function obtenerArchivosNoPublicados() As DataTable
        'Obtiene todos los archivos.
        Dim dtArc As DataTable = Data.Protocolo.Listar_Archivos(0, 0)

        Dim dtArcPub As New DataTable()
        dtArcPub.Columns.Add("idArchivo", GetType(System.Int32))
        dtArcPub.Columns.Add("NombreArchivo", GetType(System.String))
        dtArcPub.Columns.Add("peso", GetType(System.String))
        dtArcPub.Columns.Add("descripcion", GetType(System.String))

        'Descarta los archivos ya publicados.
        For Each dr As DataRow In dtArc.Rows
            If Convert.ToInt32(dr("estado")) = 0 Then
                Dim newRow As DataRow = dtArcPub.NewRow()
                newRow("idArchivo") = dr("idArchivo")
                newRow("NombreArchivo") = dr("NombreArchivo")

                'Procesa el peso del archivo.
                If ((CInt(dr("peso") / 1024)) > 0) Then
                    newRow("peso") = (CInt(dr("peso") / 1024)).ToString() + " KB"
                Else
                    newRow("peso") = (CInt(dr("peso") / 1024)).ToString() + " MB"
                End If

                newRow("descripcion") = dr("descripcion")
                dtArcPub.Rows.Add(newRow)
            End If
        Next
        Return dtArcPub
    End Function

    'Envia correo en forma masiva a los usuarios asociados a los archivos publicados.
    Public Shared Function enviarCorreoPublicacion(ByVal wcTo As String, ByVal sFrom As String, ByVal sCc As String, ByVal sUrl As String, ByVal dtArc As DataTable) As Int32

        Dim codigo As Int32 = 0
        Dim sMensajeCorreo As String
        Dim sSubject As String = "Traspaso Información Entidades Externas"
        Dim iImportancia As Int32 = 0 'normal

        Dim usuEmail As New Dictionary(Of Int32, String)()
        Dim usuArchivo As New Dictionary(Of Int32, String)()

        'Construye
        Dim idArchivo As Int32
        Dim idUsuario As Int32
        Dim tr, tdArc, tdPes, tdDes As String
        Dim nombre, peso, descripcion, email As String

        For Each drArc As DataRow In dtArc.Rows

            idArchivo = Convert.ToInt32(drArc("idArchivo"))
            nombre = Convert.ToString(drArc("NombreArchivo"))
            peso = Convert.ToString(drArc("peso"))
            descripcion = Convert.ToString(drArc("descripcion"))

            Dim dtUsu As DataTable = Data.Protocolo.listarUsuariosArchivo(idArchivo)

            For Each drUsu As DataRow In dtUsu.Rows

                idUsuario = Convert.ToInt32(drUsu("id_usuario"))
                tdArc = "<td>" & nombre & "</td>"
                tdPes = "<td>" & peso & "</td>"
                tdDes = "<td>" & descripcion & "</td>"
                tr = "<tr>" & tdArc & tdPes & tdDes & "</tr>"

                'Determina los archivos por usuario.
                If usuArchivo.ContainsKey(idUsuario) Then
                    If Not usuArchivo.Item(idUsuario).Contains(nombre) Then
                        usuArchivo.Item(idUsuario) += tr
                    End If
                Else
                    usuArchivo.Add(idUsuario, tr)
                End If

                'Determina los email por usuario.
                Dim dtCon As DataTable = Data.Protocolo.listarContactosUsuario(idUsuario)
                For Each drCon As DataRow In dtCon.Rows
                    email = Convert.ToString(drCon("email"))

                    If usuEmail.ContainsKey(idUsuario) Then
                        If Not usuEmail.Item(idUsuario).Contains(email) Then
                            usuEmail.Item(idUsuario) += ";" & email
                        End If
                    Else
                        usuEmail.Add(idUsuario, email)
                    End If
                Next

            Next

        Next

        Try
            For Each key As Int32 In usuEmail.Keys
                Dim tabla As String = "<table border='1' cellpadding='0' cellspacing='3' rules='all' style='border-collapse: collapse;'>"
                Dim trCabecera As String = "<tr><td align='center'><b>Archivo</b></td><td align='center'><b>Tamaño</b></td><td align='center'><b>Descripción</b></td></tr>"
                tabla += trCabecera & usuArchivo.Item(key) & "</table>"
                sMensajeCorreo = "Se informa que se ha publicado en web Dipres la siguiente información:<br><br>" & tabla & "<br>Atte.<br><br>Sub-departamento de Estadísticas<br>Dirección de Presupuestos"

                Dim sTo As String
                If wcTo <> Nothing And Not String.IsNullOrEmpty(wcTo) Then
                    sTo = usuEmail.Item(key) & ";" & wcTo
                Else
                    sTo = usuEmail.Item(key)
                End If

                If Not Correo.CallSendMail(sFrom, sTo, sCc, sSubject, sMensajeCorreo, iImportancia, sUrl) Then
                    Correo.CallSendMail(sFrom, "icg@dipres.cl", "", "PROBLEMA!!!" & sSubject, sMensajeCorreo & "<br>Originalmente para: [" & sTo & "]", iImportancia, sUrl)
                End If
            Next
            codigo = 1
        Catch ex As Exception
            Throw ex
            Exit Function
        End Try
        Return codigo
    End Function

    'Envia un correo a un conjunto de usuarios específicos que se asocian a un archivo publicado.
    Public Shared Function enviarCorreoUsuario(ByVal wcTo As String, ByVal sFrom As String, ByVal sCc As String, ByVal sUrl As String, ByVal idUsuarios As List(Of Int32), ByVal dtArc As DataTable) As Int32

        Dim codigo As Int32 = 0
        Dim sMensajeCorreo As String
        Dim sSubject As String = "Traspaso Información Entidades Externas"
        Dim iImportancia As Int32 = 0 'normal

        Dim usuEmail As New Dictionary(Of Int32, String)()
        Dim usuArchivo As New Dictionary(Of Int32, String)()
        Dim notificados As Int32 = 0

        'Construye
        Dim idArchivo As Int32
        Dim tr, tdArc, tdPes, tdDes As String
        Dim nombre, peso, descripcion, email As String

        For Each drArc As DataRow In dtArc.Rows
            idArchivo = Convert.ToInt32(drArc("idArchivo"))
            nombre = Convert.ToString(drArc("NombreArchivo"))
            peso = Convert.ToString(drArc("peso"))
            descripcion = Convert.ToString(drArc("descripcion"))

            For Each idUsuario As Int32 In idUsuarios

                tdArc = "<td>" & nombre & "</td>"
                tdPes = "<td>" & peso & "</td>"
                tdDes = "<td>" & descripcion & "</td>"
                tr = "<tr>" & tdArc & tdPes & tdDes & "</tr>"

                'Determina los archivos por usuario.
                If usuArchivo.ContainsKey(idUsuario) Then
                    If Not usuArchivo.Item(idUsuario).Contains(nombre) Then
                        usuArchivo.Item(idUsuario) += tr
                    End If
                Else
                    usuArchivo.Add(idUsuario, tr)
                End If

                'Determina los email por usuario.
                Dim dtCon As DataTable = Data.Protocolo.listarContactosUsuario(idUsuario)
                If dtCon.Rows.Count > 0 Then
                    notificados += 1
                    For Each drCon As DataRow In dtCon.Rows
                        email = Convert.ToString(drCon("email"))

                        If usuEmail.ContainsKey(idUsuario) Then
                            If Not usuEmail.Item(idUsuario).Contains(email) Then
                                usuEmail.Item(idUsuario) += ";" & email
                            End If
                        Else
                            usuEmail.Add(idUsuario, email)
                        End If
                    Next
                End If
            Next

        Next

        Try
            For Each key As Int32 In usuEmail.Keys
                Dim tabla As String = "<table border='1' cellpadding='0' cellspacing='3' rules='all' style='border-collapse: collapse;'>"
                Dim trCabecera As String = "<tr><td align='center'><b>Archivo</b></td><td align='center'><b>Tamaño</b></td><td align='center'><b>Descripción</b></td></tr>"
                tabla += trCabecera & usuArchivo.Item(key) & "</table>"
                sMensajeCorreo = "Se informa que se ha publicado en web Dipres la siguiente información:<br><br>" & tabla & "<br>Atte.<br><br>Sub-departamento de Estadísticas<br>Dirección de Presupuestos"

                Dim sTo As String
                If wcTo <> Nothing And Not String.IsNullOrEmpty(wcTo) Then
                    sTo = usuEmail.Item(key) & ";" & wcTo
                Else
                    sTo = usuEmail.Item(key)
                End If

                If Not Correo.CallSendMail(sFrom, sTo, sCc, sSubject, sMensajeCorreo, iImportancia, sUrl) Then
                    Correo.CallSendMail(sFrom, "icg@dipres.cl", "", "PROBLEMA!!!" & sSubject, sMensajeCorreo & "<br>Originalmente para: [" & sTo & "]", iImportancia, sUrl)
                End If
            Next
            If notificados = idUsuarios.Count Then
                codigo = 1
            Else
                codigo = 0
            End If
        Catch ex As Exception
            codigo = 0
        End Try
        Return codigo
    End Function

    'Retorna el estado de vigencia de un Archivo.
    Public Shared Function getEstadoVigencia(ByVal periodo As Int32, ByVal idArchivo As Int32) As String
        Dim estado As String = "Sin Vigencia"
        Dim dtUsuAso As DataTable = Data.Protocolo.listarUsuariosArchivo(idArchivo)
        Dim numUsuAso = dtUsuAso.Rows.Count
        Dim numUsuVig As Int32 = 0

        If numUsuAso > 0 Then

            For Each dr As DataRow In dtUsuAso.Rows
                If CBool(dr("vigente")) Then
                    numUsuVig += 1
                End If
            Next

            If numUsuVig = 0 Then
                estado = "Sin Vigencia"
            ElseIf numUsuVig = numUsuAso Then
                estado = "Vigencia Total"
            ElseIf numUsuVig < numUsuAso Then
                estado = "Vigencia Parcial"
            End If

        End If

        Return estado
    End Function


    'Retorna el nombre de la institución a la que pertenece un usuario.
    Public Shared Function listarTipoDatoOficial(ByVal idUsuario As Int32) As DataTable
        Return Data.Protocolo.listarTipoDatosOficiales(idUsuario)
    End Function

    'Retorna las fechas de inicio para un tipo de tado oficial
    Public Shared Function listarFechasInicio(ByVal idTipoDatoOficial As Int32) As DataTable
        Return Data.Protocolo.listarFechasInicio(idTipoDatoOficial)
    End Function

    'Retorna las fechas de cierre para un tipo de tado oficial
    Public Shared Function listarFechasCierre(ByVal idTipoDatoOficial As Int32) As DataTable
        Return Data.Protocolo.listarFechasCierre(idTipoDatoOficial)
    End Function
End Class
