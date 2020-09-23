Imports Framework.Configuracion
Imports ServiceAgent.Proxy


Public NotInheritable Class Validacion

    Public Shared Function Cuenta(ByVal strUsuario As String, ByVal strContrasena As String, ByVal intProceso As Integer, ByVal intPeriodo As Integer, ByVal strNavegador As String, ByVal strOS As String, ByVal strIP As String, ByVal bolRegistra As Boolean) As DatosUsuario
        Dim DipresEncripto As New ClsEncriptar.Encripto()
        Dim oconfig As Configuracion = Configuracion.getInstance()
        Dim res As New ValidarUsuario
        Dim datosValidarUsuario As New DatosUsuario


        res.Credentials = New System.Net.NetworkCredential( _
              DipresEncripto.Desencriptar(oconfig.ProxyUser) _
            , DipresEncripto.Desencriptar(oconfig.ProxyPassword) _
            , DipresEncripto.Desencriptar(oconfig.ProxyDomain) _
        )

        res.Url = oconfig.UrlwsControlAcceso

        datosValidarUsuario = res.Cuenta(strUsuario, strContrasena, intProceso, intPeriodo, strNavegador, strOS, strIP, bolRegistra)
        Return datosValidarUsuario


    End Function


    Public Shared Function CuentaBasica(ByVal strUsuario As String, ByVal strContrasena As String) As Boolean
        Dim DipresEncripto As New ClsEncriptar.Encripto()
        Dim oconfig As Configuracion = Configuracion.getInstance()
        Dim res As New ValidarUsuario

        res.Credentials = New System.Net.NetworkCredential( _
              DipresEncripto.Desencriptar(oconfig.ProxyUser) _
            , DipresEncripto.Desencriptar(oconfig.ProxyPassword) _
            , DipresEncripto.Desencriptar(oconfig.ProxyDomain) _
        )

        res.Url = oconfig.UrlwsControlAcceso

        Return res.CuentaBasica(strUsuario, strContrasena)

    End Function

End Class
