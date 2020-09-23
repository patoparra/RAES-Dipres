
Partial Class MenuInterno
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'lbltitulo.Text = "Página Inicio a Aplicaciones Internas	(Control de Gestion)"

    End Sub
    Protected Sub Aceptar_Click(sender As Object, e As System.EventArgs) Handles Aceptar.Click
        Dim strpage As String
        strpage = "<html><body>" & _
         "<form name=paso action='Inicio.aspx' method=post>" & _
         "<input type=hidden name=usuario value='" & usuario.Text & "'/><input type=hidden name=password value='" & password.Value & "'/>" & _
         "</form>" & _
         "<script>document.paso.submit();</script>" & _
         "</body></Html>"

        Response.Write(strpage)
    End Sub
End Class
