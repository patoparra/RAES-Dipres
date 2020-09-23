
Partial Class SSaludGeneralPrincipal
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblTitulo.Text = "Traspaso Información Entidades Externas"

        If Not Session("Usuario") Is Nothing Then
            LbTxUsuarioLogin.Text = Session("Usuario").ToString.ToUpper
            LbTxUsuarioPerfil.Text = TraducePerfil(Session("Perfil"))
        Else
            LbTxUsuarioLogin.Text = ""
        End If

        Page.Title = "Traspaso Información Entidades Externas"
        CargaMenu()
    End Sub

    Private Sub CargaMenu()

        Dim argumentos As New System.Xml.Xsl.XsltArgumentList
        Dim doc As New System.Xml.XmlDocument()

        If (Session("estado") = Nothing) Then
            Session("estado") = "01"
        End If

        Dim pInstrumento As String = Session("menu")

        If Not IsNothing(Session("IdUsuario")) Then
            Dim pUsuario As String = Session("IdUsuario").ToString()
            Dim pPerfil As String = Session("Perfil").ToString().PadLeft(3, "0")
            Dim pEstado As String = Session("estado").ToString().PadLeft(2, "0")


            doc.Load(Server.MapPath("~/Menu/MenuNavegacion.xml"))

            argumentos.AddParam("instrumento", String.Empty, pInstrumento)
            argumentos.AddParam("usuario", String.Empty, pUsuario)
            argumentos.AddParam("perfil", String.Empty, pPerfil)
            argumentos.AddParam("estado", String.Empty, pEstado)

            Me.xmlMenu.TransformArgumentList = argumentos
            Me.xmlMenu.TransformSource = Server.MapPath("~/Menu/Menu.xslt")
            Me.xmlMenu.DocumentContent = doc.InnerXml
        End If

    End Sub

    Private Function TraducePerfil(ByVal perfil As Integer) As String
        Dim perfiltxt As String = ""

        Select Case perfil
            Case 140
                perfiltxt = "ADMINISTRADOR"
            Case 141
                perfiltxt = "CONSULTA"
            Case Else
                perfiltxt = "DESCONOCIDO"
        End Select
        TraducePerfil = perfiltxt
    End Function

    Protected Sub lbCerrarSesion_Click(sender As Object, e As EventArgs) Handles lbCerrarSesion.Click
        Session.Abandon()
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("http://www.dipres.cl")
    End Sub

End Class

