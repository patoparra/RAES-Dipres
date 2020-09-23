Imports ServiceAgent.Proxy


Public NotInheritable Class Correo

    Public Shared Function CallSendMail(ByVal strFrom As String, ByVal strTo As String, ByVal strCC As String, ByVal strSubject As String, ByVal strMessage As String, ByVal iImportancia As Integer, ByVal sUrl As String) As Boolean
        Dim DipresEncripto As New ClsEncriptar.Encripto()
        Dim eMail As New SendMail()

        eMail.Url = sUrl

        Return eMail.CallSendMail(strFrom, strTo, strCC, strSubject, strMessage, iImportancia)
    End Function
End Class
