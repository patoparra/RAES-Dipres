Imports Framework.Configuracion
Imports ServiceAgent


Public NotInheritable Class Archivo

    Public Shared Function NormalizaArchivos(ByVal NombreArchivo As String) As String
        Dim DipresEncripto As New ClsEncriptar.Encripto()
        Dim oconfig As Configuracion = Configuracion.getInstance()
        Dim res As New Proxy.Archivos
        Dim resultado As String

        resultado = res.NormalizaNombre(NombreArchivo)

        Return resultado
    End Function
End Class
