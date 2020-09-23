Imports System.Xml
Imports System.Xml.Xsl
Imports System.Data

Imports System.Drawing
Imports System.Data.SqlClient

Imports Dipres.CG
Imports Util
Imports System.IO
Partial Class Reportes_ReporteDescargas
    Inherits System.Web.UI.Page
    Dim _oDts As New DataSet("ArchivosExcel")
    Dim _otbObs As New DataTable
    Dim _datos As New DataTable
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim _mes As String = Request.QueryString("mes")
        Dim _periodo As String = Request.QueryString("periodo")
        Dim _usuario As String = Request.QueryString("usuario")
        Dim _fd As String = Request.QueryString("fd")

        Dim valorComboMes = _mes
        Dim valorMes As Integer

        valorMes = seleccion_mes(valorComboMes)
        Dim fd As Date = convertirFechaDescarga(_fd)

        CreaDataSet(valorComboMes, valorMes.ToString, _periodo, _usuario, fd)
        GeneraArchivo(_oDts, "\xsl\ReporteDescarga.xslt")

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

    Public Sub CreaDataSet(ByVal valorComboMes As String, ByVal _mes As String, ByVal _periodo As String, ByVal usuario As String, ByVal fd As Date)
        Dim iResp As Integer
        _datos.Columns.AddRange(New DataColumn() {New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String))})
        _datos.Rows.Add(valorComboMes, _periodo)
        _otbObs = Business.Protocolo.ListarLogDescarga(_mes, _periodo, usuario, fd)
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn() {New DataColumn("NombreArchivo", GetType(String)), New DataColumn("FechaDescarga", GetType(String)), New DataColumn("NombreUsuario", GetType(String)), New DataColumn("mes", GetType(String)), New DataColumn("periodo", GetType(String)), New DataColumn("estado", GetType(String)), New DataColumn("vigencia", GetType(String))})
        For Each row As DataRow In _otbObs.Rows
            Dim nomPublicacion As String
            Dim idArchivo As String = CStr(row("IdArchivo"))
            Dim nomArchivo As String = CStr(row("NombreArchivo"))
            Dim idVigencia As String
            If row("vigencia") Is DBNull.Value Then
                idVigencia = "-"
            Else

                If Integer.Parse(CStr(row("vigencia"))) = 0 Then
                    idVigencia = "No Vigente"
                Else
                    idVigencia = "Vigente"
                End If
            End If

            Dim fechaCreacion As String = CStr(row("Fecha"))
            Dim fechaDescarga As String = CStr(row("FechaDescarga_txt"))
            Dim publicacion As Integer = Business.Protocolo.Estado_Archivo(Integer.Parse(idArchivo))
            If (publicacion = 0) Then
                nomPublicacion = "No Publicado"
            Else
                nomPublicacion = "Publicado"
            End If
            Dim nomUsuario As String = CStr(row("NombreUsuario"))
            Dim periodo As String = CStr(row("periodo"))
            Dim mes As String = CStr(row("mes"))

            dt.Rows.Add(nomArchivo, fechaDescarga, nomUsuario, mes, periodo, nomPublicacion, idVigencia)
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

        objTransform.Load(strAppPath & strAppXslfo)
        Response.AddHeader("Content-Disposition", "attachment; filename=ReporteLogDescargas.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        objTransform.Transform(xmlDoc, Nothing, Response.OutputStream, Nothing)
        Response.End()
    End Sub

    'Procesa la fecha de descarga. 
    Protected Function convertirFechaDescarga(ByVal fdStr As String) As Date
        fdStr = Trim(fdStr)

        If fdStr <> "" Then

            Dim tokens As String() = fdStr.Split("-")
            If tokens.Length = 2 Then

                Dim mes As Int32
                Dim anio As Int32

                Try
                    Integer.TryParse(tokens(0), mes)
                    Integer.TryParse(tokens(1), anio)

                    If mes < 1 Or mes > 12 Then
                        Return Date.MinValue
                    End If

                    If anio < 2015 Or anio > Date.Now.Year Then
                        Return Date.MinValue
                    End If

                Catch ex As Exception
                    Return Date.MinValue
                End Try

                'Retorna fecha de publicación como un objeto Date.
                Return New Date(anio, mes, 1)

            Else
                Return Date.MinValue
            End If
        Else
            'Retorna la fecha mínima.
            Return Date.MinValue
        End If
    End Function

End Class
