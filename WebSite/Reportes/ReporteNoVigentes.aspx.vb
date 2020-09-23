Imports System.Xml
Imports System.Xml.Xsl
Imports System.Data

Imports System.Drawing
Imports System.Data.SqlClient

Imports Dipres.CG
Imports Util
Imports System.IO
Partial Class Reportes_ReporteNoVigentes
    Inherits System.Web.UI.Page
    Dim _oDts As New DataSet("ArchivosExcel")
    Dim _otbObs As New DataTable
    Dim _datos As New DataTable
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim _mes As String = Request.QueryString("mes")
        Dim _periodo As String = Request.QueryString("periodo")

        'Dim valorCombo = select_periodo.SelectedValue
        Dim valorComboMes = _mes
        Dim valorMes As Integer

        valorMes = seleccion_mes(valorComboMes)

        CreaDataSet(valorComboMes, valorMes.ToString, _periodo)
        GeneraArchivo(_oDts, "\xsl\ReporteNoVigente.xslt")

    End Sub
    Public Shared Function seleccion_mes(ByVal mes As String) As Integer
        Dim valor As Integer
        Select Case mes
            Case "Enero"
                valor = 1
            Case "Febrero"
                valor = 2
            Case "Marzo"
                valor = 3
            Case "Abril"
                valor = 4
            Case "Mayo"
                valor = 5
            Case "Junio"
                valor = 6
            Case "Julio"
                valor = 7
            Case "Agosto"
                valor = 8
            Case "Septiembre"
                valor = 9
            Case "Octubre"
                valor = 10
            Case "Noviembre"
                valor = 11
            Case "Diciembre"
                valor = 12
        End Select
        Return valor
    End Function
    'Public Sub CreaDataSet(ByVal valorComboMes As String, ByVal _mes As String, ByVal _periodo As String)
    '    Dim iResp As Integer
    '    _datos.Columns.AddRange(New DataColumn() {New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String))})
    '    _datos.Rows.Add(valorComboMes, _periodo)
    '    _otbObs = Business.ISSalud.listarArchivosNoVigentes(_mes, _periodo)
    '    _otbObs.TableName = "EncabezadoReporte"
    '    _datos.TableName = "Test"
    '    _oDts.Tables.Add(_datos)
    '    _oDts.Tables.Add(_otbObs)



    'End Sub

    Public Sub CreaDataSet(ByVal valorComboMes As String, ByVal _mes As String, ByVal _periodo As String)
        Dim iResp As Integer
        _datos.Columns.AddRange(New DataColumn() {New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String))})
        _datos.Rows.Add(valorComboMes, _periodo)
        _otbObs = Business.Protocolo.listarArchivosNoVigentes(_mes, _periodo)
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("NombreArchivo", GetType(String)), New DataColumn("Fecha", GetType(String)), New DataColumn("NombreUsuario", GetType(String)), New DataColumn("Mes", GetType(String)), New DataColumn("Periodo", GetType(String)), New DataColumn("fecha_vigencia", GetType(String))})
        For Each row As DataRow In _otbObs.Rows
            Dim nomPublicacion As String
            Dim idArchivo As String = CStr(row("IdArchivo"))
            Dim nomArchivo As String = CStr(row("NombreArchivo"))
            Dim idVigencia As String
            'If row("vigencia") Is DBNull.Value Then
            'idVigencia = "-"
            'Else

            'If Integer.Parse(CStr(row("vigencia"))) = 0 Then
            'idVigencia = "No Vigente"
            'Else
            'idVigencia = "Vigente"
            'End If
            'End If

            Dim fechaCreacion As String = CStr(row("Fecha"))
            Dim fechaVigencia As String = CStr(row("fecha_vigencia_txt"))
            'Dim publicacion As Integer = Business.ISSalud.Estado_Archivo(Integer.Parse(idArchivo))
            'If (publicacion = 0) Then
            'nomPublicacion = "No Publicado"
            'Else
            'nomPublicacion = "Publicado"
            'End If
            Dim nomUsuario As String = CStr(row("NombreUsuario"))
            Dim mes As String = CStr(row("mes"))
            Dim periodo As String = CStr(row("periodo"))
            'Dim url As String = ConfigurationManager.AppSettings("urlDocs") & CStr(row("NombreArchivo"))
            dt.Rows.Add(nomArchivo, fechaCreacion, nomUsuario, mes, periodo, fechaVigencia)
        Next
        dt.TableName = "EncabezadoReporte"
        _datos.TableName = "Test"
        _oDts.Tables.Add(_datos)
        _oDts.Tables.Add(dt)
    End Sub
    Private Sub GeneraArchivo(ByRef salidaDS As DataSet, ByVal strAppXslfo As String)

        Dim objDatos As XmlDocument = New XmlDocument
        Dim objTransform As XslTransform = New XslTransform
        Dim strAppPath As String = Me.Server.MapPath("")

        Dim xmlDoc As XmlDataDocument = New XmlDataDocument(salidaDS)


        'xmlDoc.Save("d:\aaa.xml")


        objTransform.Load(strAppPath & strAppXslfo)
        Response.AddHeader("Content-Disposition", "attachment; filename=ReporteNoVigentes.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        objTransform.Transform(xmlDoc, Nothing, Response.OutputStream, Nothing)
        Response.End()

    End Sub
End Class
