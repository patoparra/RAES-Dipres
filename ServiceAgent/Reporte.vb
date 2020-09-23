Imports Framework.Configuracion
Imports ServiceAgent


Public NotInheritable Class Reporte

    Public Shared Function ReportExecute(ByVal NombreReporte As String, ByVal CodigoFormato As FormatoArchivo, ByVal parametrosReporte As Hashtable) As Byte()
        Dim DipresEncripto As New ClsEncriptar.Encripto()
        Dim oconfig As Configuracion = Configuracion.getInstance()
        Dim res As New Proxy.ReportExecutionService
        Dim InParameters(parametrosReporte.Count) As Proxy.ParameterValue
        Dim historyID As String = Nothing
        Dim formato As String = Nothing
        Dim warnings As Proxy.Warning() = Nothing
        Dim streamIDs As String() = Nothing
        Dim executeInfo As New Proxy.ExecutionInfo
        Dim executeHeader As New Proxy.ExecutionHeader
        Dim sSessionID As String = Nothing
        Dim idx As Integer = 0

        Select Case CodigoFormato
            Case FormatoArchivo.Word
                formato = "WORD"
            Case FormatoArchivo.Excel
                formato = "EXCEL"
            Case FormatoArchivo.Pdf
                formato = "PDF"
        End Select

        For Each prm As DictionaryEntry In parametrosReporte
            InParameters(idx) = New Proxy.ParameterValue()
            InParameters(idx).Label = prm.Key
            InParameters(idx).Name = prm.Key
            InParameters(idx).Value = prm.Value

            idx += 1
        Next prm

        res.Credentials = New System.Net.NetworkCredential( _
              DipresEncripto.Desencriptar(oconfig.ProxyUser) _
            , DipresEncripto.Desencriptar(oconfig.ProxyPassword) _
            , DipresEncripto.Desencriptar(oconfig.ProxyDomain) _
        )

        res.Url = oconfig.UrlwsReportExecution2005
        res.ExecutionHeaderValue = executeHeader

        executeInfo = res.LoadReport(DipresEncripto.Desencriptar(oconfig.RutaReporte) & NombreReporte, historyID)
        sSessionID = res.ExecutionHeaderValue.ExecutionID()

        res.SetExecutionParameters(InParameters, "es-cl")

        Return res.Render(formato, Nothing, Nothing, Nothing, Nothing, warnings, streamIDs)
    End Function

End Class
