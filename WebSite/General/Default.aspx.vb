
Partial Class General_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Add("Usuario", "mriquelme")
        Session.Add("IdUsuario", 8413)
        Session.Add("Perfil", 140)
        Session.Add("NombrePerfil", "Administrador Protocolo Dipres")
        Session.Add("Clave", "")
        Response.Redirect("CargaArchivo.aspx")
    End Sub
End Class
