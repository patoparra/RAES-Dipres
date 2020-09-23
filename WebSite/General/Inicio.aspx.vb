
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class Inicio
    Inherits System.Web.UI.Page
    Dim Perfil As Integer
    Dim Usuario As String
    Dim Contrasena As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Usuario = Request("usuario")
        Contrasena = Request("password")

        Session("periodo") = CType(ConfigurationManager.AppSettings("Periodo"), Integer)
        Session("Proceso") = 64

        If Usuario <> "" And Contrasena <> "" Then
            If ValidacionGlobal(Usuario, Contrasena, 64, Session("periodo")) = 0 Then
                If Session("Perfil").ToString = "150" Then
                    Response.Redirect("DescargaArchivo.aspx")
                End If
                If Session("Perfil").ToString = "151" Then
                    Response.Redirect("CargaArchivo.aspx")
                End If
            Else
                Response.Redirect("http://www.dipres.cl")
            End If
        Else
            Response.Redirect("http://www.dipres.cl")
        End If
    End Sub

    Private Function ValidacionGlobal(ByVal Usr As String, ByVal Pwd As String, ByVal Proc As Integer, ByVal Periodo As Integer) As Integer
        Try
            'Dim clsValidar As SCA_WS.ValidarUsuario = New SCA_WS.ValidarUsuario
            Dim wbContexto As HttpContext = HttpContext.Current
            Dim hbcNavegador As HttpBrowserCapabilities = wbContexto.Request.Browser

            Dim strUsuario As String = Usr
            Dim strClave As String = Pwd
            Dim intProceso As Integer = Proc
            Dim strNavegador As String = hbcNavegador.Type
            Dim strOS As String = hbcNavegador.Platform
            Dim strIP As String = wbContexto.Request.UserHostAddress

            Dim oValidar As ServiceAgent.Proxy.ValidarUsuario = New ServiceAgent.Proxy.ValidarUsuario
            oValidar.Url = System.Configuration.ConfigurationManager.AppSettings("UrlwsControlAcceso")
            'Dim Datos As SCA_WS.DatosUsuario
            Dim Datos As ServiceAgent.Proxy.DatosUsuario
            'Datos = clsValidar.Cuenta(strUsuario, strClave, intProceso, strNavegador, strOS, strIP, True)
            Datos = oValidar.Cuenta(strUsuario, strClave, intProceso, Periodo, strNavegador, strOS, strIP, True)
            'clsValidar.Dispose()
            oValidar.Dispose()

            If Datos.Retorno = 0 Then
                Session.Add("Usuario", Usr)
                Session.Add("IdUsuario", Datos.IDUsuario)
                Session.Add("Perfil", Datos.Perfil)
                Session.Add("NombrePerfil", Datos.NombrePerfil)
                Session.Add("Clave", strClave)
            End If

            Return Datos.Retorno
        Catch ex As Exception
            Return -1
        End Try
    End Function


End Class
