Imports Dipres.CG
Imports System.Data
Imports System.Net.Mail
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports Dipres.CG.Data

Partial Class General_Contactos
    Inherits System.Web.UI.Page

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Comprueba que la sesión del usuario está vigente.
        If IsNothing(Session("Usuario")) Then
            Response.Redirect("http://www.dipres.cl")
        End If

        If Not IsPostBack Then
            txtIdUsuario.Value = Session("idUsuario")
        End If

    End Sub

    'Chequea si el formato de un email es válido.
    Private Shared Function esEmail(ByVal email As String) As Boolean
        Dim loEs As Boolean = True
        Try
            'Borra posibles espacios.
            email = Trim(email)

            Dim emailValidator As New MailAddress(email)

            'Valida que el email contenga @ y termine en punto (.) y algo (cl, com, etc.)
            If Not IsNothing(email) And email <> "" Then

                Dim tokens1 As String() = email.Split("@")
                If tokens1.Length <> 2 Then
                    loEs = False
                Else
                    Dim dominio As String = tokens1(1)
                    If IsNothing(dominio) Or dominio = "" Or dominio.Length = 0 Then
                        loEs = False
                    Else
                        Dim tokens2 As String() = dominio.Split(".")
                        If tokens2.Length < 2 Then
                            loEs = False
                        Else
                            Dim lastPart As String = tokens2(tokens2.Length - 1)
                            If IsNothing(lastPart) Or lastPart = "" Or lastPart.Length = 0 Then
                                loEs = False
                            End If
                        End If

                    End If
                End If
                
            Else
                loEs = False
            End If

        Catch ex As Exception
            loEs = False
        End Try
        Return loEs
    End Function

    'Carga la tabla de usuarios.
    Private Sub cargarUsuarios()
        Dim periodo As Int32 = CInt(ConfigurationManager.AppSettings("periodo"))
        Dim dtUsuarios As DataTable = Business.Protocolo.listarUsuariosProceso(periodo)
        'gvUsuarios.DataSource = dtUsuarios
        'gvUsuarios.DataBind()
    End Sub

    'Retorna los contactos de una cuenta de usuario.
    <WebMethod()> Public Shared Function obtenerContactos(ByVal idUsuario As Int32) As String
        Dim dt As DataTable = Business.Protocolo.listarContactosUsuario(idUsuario)

        Dim contactos As New List(Of Object)()
        Dim s As New JavaScriptSerializer()

        'Crea la lista de contactos.
        For Each dr As DataRow In dt.Rows
            Dim idContacto As Int32 = CInt(dr("id_usuario_contacto"))

            Dim nombre As String = String.Empty
            If Not IsDBNull(dr("nombre")) Then
                nombre = CStr(dr("nombre"))
            End If

            Dim cargo As String = String.Empty
            If Not IsDBNull(dr("cargo")) Then
                cargo = CStr(dr("cargo"))
            End If

            Dim telefono As String = String.Empty
            If Not IsDBNull(dr("telefono")) Then
                telefono = CStr(dr("telefono"))
            End If

            Dim email As String = String.Empty
            If Not IsDBNull(dr("email")) Then
                email = CStr(dr("email"))
            End If

            contactos.Add(New With {.idContacto = idContacto, .nombre = nombre, .cargo = cargo, .telefono = telefono, .email = email})
        Next

        Return s.Serialize(contactos)
    End Function

    <WebMethod()> Public Shared Function guardarContacto(ByVal contacto As Contacto) As String
        Dim c As New Contacto()
        Dim r As New Respuesta()
        Dim s As New JavaScriptSerializer()

        'Validaciones a los datos de contacto.

        'Nombre.
        If IsNothing(contacto.nombre) Or Trim(contacto.nombre) = "" Then
            r.mensaje = "Debe especificar el nombre del contacto."
            Return s.Serialize(r)
        Else
            If contacto.nombre.Length > 50 Then
                r.mensaje = "La cantidad de caracteres del nombre del contacto supera el máximo permitido (50)."
                Return s.Serialize(r)
            End If
        End If

        'Cargo.
        If IsNothing(contacto.cargo) Or Trim(contacto.cargo) = "" Then
            r.mensaje = "Debe especificar el cargo del contacto."
            Return s.Serialize(r)
        Else
            If contacto.cargo.Length > 50 Then
                r.mensaje = "La cantidad de caracteres del cargo del contacto supera el máximo permitido (50)."
                Return s.Serialize(r)
            End If
        End If

        'Teléfono.
        If IsNothing(contacto.telefono) Or Trim(contacto.telefono) = "" Then
            r.mensaje = "Debe especificar el teléfono del contacto."
            Return s.Serialize(r)

        ElseIf contacto.telefono.Length > 10 Then
            r.mensaje = "La cantidad de caracteres del teléfono del contacto supera el máximo permitido (10)."
            Return s.Serialize(r)

        ElseIf Not IsNumeric(contacto.telefono) Then
            r.mensaje = "El formato del teléfono del contacto es incorrecto. Ingrese sólo números."
            Return s.Serialize(r)

        End If

        'Correo Electrónico.
        If IsNothing(contacto.email) Or Trim(contacto.email) = "" Then
            r.mensaje = "Debe especificar el correo electrónico del contacto."
            Return s.Serialize(r)

        ElseIf contacto.email.Length > 50 Then
            r.mensaje = "La cantidad de caracteres del email del contacto supera el máximo permitido (50)."
            Return s.Serialize(r)

        ElseIf Not esEmail(contacto.email) Then
            r.mensaje = "El formato del email del contacto es incorrecto."
            Return s.Serialize(r)

        Else
            'Valida que el correo no se repita.
            Dim dtContactos As DataTable = Business.Protocolo.listarContactosUsuario(contacto.idUsuario)
            If Not IsNothing(dtContactos) And dtContactos.Rows.Count > 0 Then

                For Each dr As DataRow In dtContactos.Rows
                    Dim idContacto As Int32 = CInt(dr("id_usuario_contacto"))
                    If contacto.idContacto <> idContacto Then
                        Dim email As String = CStr(dr("email"))
                        If contacto.email = email Then
                            r.mensaje = "El email ingresado ya está asociado a un contacto registrado, por favor ingrese otro."
                            Return s.Serialize(r)
                        End If
                    End If
                Next

            End If
        End If

        Dim codigo As Int32 = Business.Protocolo.guardarContacto(contacto)

        If codigo = 1 Then
            r.exito = 1
            r.mensaje = "El contacto fue guardado exitosamente."
        Else
            r.exito = 0
            r.mensaje = "Ha ocurrido un error al intentar guardar el contacto. Por favor consulte al administrador del Sistema."
        End If

        Return s.Serialize(r)
    End Function

    <WebMethod()> Public Shared Function eliminarContacto(ByVal idContacto As Int32) As String
        Dim c As New Contacto()
        Dim codigo As Int32 = Business.Protocolo.eliminarContacto(idContacto)
        Dim r As New Respuesta()
        If codigo = 1 Then
            r.exito = 1
            r.mensaje = "El contacto fue eliminado exitosamente."
        Else
            r.exito = 0
            r.mensaje = "Ha ocurrido un error al intentar eliminar el contacto. Por favor consulte al administrador del Sistema."
        End If
        Dim s As New JavaScriptSerializer()
        Return s.Serialize(r)
    End Function
End Class
