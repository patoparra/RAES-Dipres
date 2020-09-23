<%@ Page Language="VB" MasterPageFile="~/General/GeneralPrincipal.master" AutoEventWireup="false" CodeFile="Usuarios.aspx.vb" Inherits="General_Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {

            //Crea el diálogo para editar la información de contacto.
            $("#dlgUsuarioContacto").dialog({
                autoOpen: false,
                resizable: false,
                width: 900,
                height: 470,
                modal: true,
                closeOnEscape: false,
                dialogClass: "no-close",
                show: { effect: "fade", duration: 100 },
                hide: { effect: "fade", duration: 100 },
                buttons: {
                    "Cerrar": function () {                        
                        $(this).dialog("close");
                    }
                }
            });

        });

        //Despliega el diálogo de contactos.
        function abrirContactos(indice, idUsuario) {
            cargarDetalleUsuario(indice);
            cargarContactos(idUsuario);
            $("#dlgUsuarioContacto").dialog("open");            
            return false;
        }

        //Carga los detalles de la cuenta de usuario.
        function cargarDetalleUsuario(indice) {            
            var usuario = $("#lblUsuario" + indice).html();
            var institucion = $("#lblInst" + indice).html();
            $("#lblUsuario").html(usuario);
            $("#lblInstitucion").html(institucion);
        }

        //Carga los contactos asociados al usuario.
        function cargarContactos(idUsuario) {
            $.ajax({
                type: "POST",
                dataType: "json",
                data: "{'idUsuario':" + idUsuario + "}",
                contentType: "application/json; charset=utf-8",
                url: "Contactos.aspx/obtenerContactos",
                success: function (data, textStatus, jqXHR) {
                    var contactos = JSON.parse(data.d);
                    construirTablaContactos(contactos);
                }
            });
        }

        //Construye las filas de la tabla contactos.
        function construirTablaContactos(contactos) {
            //Limpia la tabla de contactos.
            $("#tbContactos .gridRow").remove();
            $("#tbContactos .gridAltRow").remove();

            var numero = 1;
            if (contactos.length > 0) {
                numero = contactos.length + 1;

                for (var i = 0; i < contactos.length; i++) {
                    var c = contactos[i];
                    var tdNumero = "<td style='text-align:center'>" + (i + 1) + "</td>";
                    var tdNombre = "<td>" + c.nombre + "</td>";
                    var tdCargo = "<td>" + c.cargo + "</td>";
                    var tdTelefono = "<td>" + c.telefono + "</td>";
                    var tdEmail = "<td>" + c.email + "</td>";

                    var clase = "";
                    if ((i % 2) == 0) {
                        clase = "gridRow";
                    } else {
                        clase = "gridAltRow";
                    }

                    var trContacto = "<tr class='"+clase+"' >" + tdNumero + tdNombre + tdCargo + tdTelefono + tdEmail + "</tr>";
                    $("#tbContactos").append(trContacto);
                }
            } else {
                var tdSinContactos = "<tr class='gridRow' align='center'><td colspan='5'>Sin contactos para desplegar.</td></tr>";
                $("#tbContactos").append(tdSinContactos);
            }
        }

    </script>
    
    <div id="main" class="control">
        <div class="seccion">
            <h2>Datos de Contacto</h2>
            <asp:GridView runat="server" ID="gvUsuarios" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="grid" Width="100%">
                <FooterStyle CssClass="AdmResFoot"></FooterStyle>
                <AlternatingRowStyle CssClass="gridAltRow" HorizontalAlign="Center"></AlternatingRowStyle>
                <RowStyle CssClass="gridRow" HorizontalAlign="Center"></RowStyle>
                <HeaderStyle CssClass="gridHeader"></HeaderStyle>
                <EmptyDataTemplate>
                    <table class="grid" style="width:100%">
                        <tr class="gridHeader" >
                            <td>N°</td>
                            <td>Usuario</td>
                            <td>Institución</td>
                            <td>Información de Contacto</td>                    
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align:center;" class="gridRow"> 
                                No existen registros para desplegar.
                            </td>
                        </tr>
                    </table>            
                </EmptyDataTemplate> 
                <Columns>
                    <asp:TemplateField HeaderText="N°" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#(CType(Container, GridViewRow).RowIndex + 1)%>                    
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                                     
                    <asp:TemplateField HeaderText="Usuario" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <label id="lblUsuario<%#(CType(Container, GridViewRow).RowIndex)%>"><%#Eval("usuario")%></label>  
                        </ItemTemplate>
                        <ItemStyle Width="30%" HorizontalAlign="Left"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Institución" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <label id="lblInst<%#(CType(Container, GridViewRow).RowIndex)%>"><%#Eval("nombre_institucion")%></label>  
                        </ItemTemplate>
                        <ItemStyle Width="40%" HorizontalAlign="Left"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Información de Contacto" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width:16px;height:16px;cursor:pointer" class="ui-silk ui-silk-magnifier" title="Ver Información de Contacto" onclick="return abrirContactos(<%#(CType(Container, GridViewRow).RowIndex)%>,<%#Eval("usuario_tbid")%>);" ></div>     
                        </ItemTemplate>
                        <ItemStyle Width="15%" HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField> 
                </Columns>
            </asp:GridView>    
            <div id="dlgUsuarioContacto">
                <table style="width:100%" >
                    <tr>
                        <td colspan="2">
                            <h2>Datos de Contacto</h2>
                        </td>      
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <b>Usuario:</b>             
                        </td>
                        <td style="width:80%">
                            <label id="lblUsuario"></label>
                        </td>        
                    </tr>
                    <tr>
                        <td>
                            <b>Institución:</b>             
                        </td>
                        <td>
                            <label id="lblInstitucion"></label>
                        </td>       
                    </tr>
                </table>
                <br /> 
                <div style="width:100%;height:200px;overflow-y:scroll;border: 1px solid #d3d3d3;">
                    <table id="tbContactos" class="grid">
                        <tr class="gridHeader">
                            <td style="width:10%" >
                                N°
                            </td>
                            <td style="width:30%" >
                                Nombre
                            </td>
                            <td style="width:20%" >
                                Cargo
                            </td>
                            <td style="width:15%" >
                                Teléfono
                            </td>
                            <td style="width:30%" >
                                Correo Electrónico
                            </td>                                                                      
                        </tr>
                    </table>                   
                </div>                
            </div>
        </div>
    </div>
</asp:Content>
