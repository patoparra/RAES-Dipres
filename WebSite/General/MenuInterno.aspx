<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MenuInterno.aspx.vb" Inherits="MenuInterno" MasterPageFile="~/General/GeneralPrincipal.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            $(".logout").hide();
        });

    </script>
    <div class="main-dipres">
        <div class="head-dipres">
            <div class="logo-dipres">
            </div>
            <div class="app-dipres">
                <div class="appInfo-dipres" id="nombreApp">
                    <asp:Label ID="lbltitulo" runat="server" Text="" />
                </div>
            </div>
        </div>
        <div class="workspace-dipres">
            <div class="page-dipres">
                <div class="ContenidoGeneral_Ant" style="width:400px; margin:60px auto 0;">
                    <div class="seccion">
                    
                    <fieldset>
                    <legend>Ingrese con su usuario y contraseña</legend>
                    <table style="text-align: left; margin-top:30px" align="center">
                            <tr>
                                <td>
                                    <b>Usuario</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="usuario" runat="server" CssClass="normal" style="width:170px;height:15px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Password</b>
                                </td>
                                <td>
                                    <input type="password" id="password" name="password" runat="server" style="width:170px;height:15px" size="24" class="normal" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    
                                </td>
                                <td align="right">
                                    <asp:Button ID="Aceptar" runat="server" CssClass="boton" Text="Ingresar" />
                                    <br /><br />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                   
                    </div>
                        
                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>
