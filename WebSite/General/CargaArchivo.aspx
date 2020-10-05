<%@ Page Title="" Language="VB" MasterPageFile="~/General/GeneralPrincipal.master" AutoEventWireup="false" CodeFile="CargaArchivo.aspx.vb" Inherits="CargaArchivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script  type="text/javascript">

        function HabilitaText(file) {
            if (file != '') {
                $("#<%=txtDescripcion.ClientID%>").show();
            }
            else {
                $("#<%=txtDescripcion.ClientID%>").hide;
            }
            document.getElementById('<%=txtDescripcion.ClientID%>').value = '';
            $("#<%=divMensaje.ClientID%>").hide(); 
        }

        $(document).ready(function () {
            
            $("#dialog").dialog({
                autoOpen: false,
                resizable: false,
                width: 500,
                height: 250,
                modal: true,
                buttons: {
                    "Publicar": function () {
                        $.get('<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>', { notificacion: '1' }, function (resp) {
                            $("#dialog3").dialog("widget")            // get the dialog widget element
                            .find(".ui-dialog-titlebar-close") // find the close button for this dialog
                            .hide();
                            //$('#dialog3').dialog('open');
                        });
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
                
            });
            $("#dialog3").dialog({
                autoOpen: false,
                resizable: false,
                width: 500,
                height: 250,
                modal: true,
                closeOnEscape: false,
                dialogClass: "no-close",
                buttons: {
                    "Aceptar": function () {
                        window.location.href = '<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>';
                        $(this).dialog("close");
                    }
                }
            });

            //Crea el diálogo para asociar usuarios con archivos.
            $("#dlgAsociarUsuarioArchivo").dialog({
                autoOpen: false,
                resizable: false,
                width: 700,
                height: 470,
                modal: true,
                closeOnEscape: false,
                dialogClass: "no-close",
                show: { effect: "fade", duration: 100 },
                hide: { effect: "fade", duration: 100 },
                buttons: {
                    "Guardar": function () {
                        guardarUsuariosAsociados();
                    },
                    "Cerrar": function () {
                        $("#dlgAsociarUsuarioArchivo").empty();
                        $(this).dialog("close");
                    }
                }                
            });
                        
            //Crea el diálogo para editar la vigencia de un archivo.
            $("#dlgVigenciaArchivo").dialog({
                autoOpen: false,
                resizable: false,
                width: 700,
                height: 470,
                modal: true,
                closeOnEscape: false,
                dialogClass: "no-close",
                show: { effect: "fade", duration: 100 },
                hide: { effect: "fade", duration: 100 },
                buttons: {
                    "Guardar": function () {
                        guardarArchivoVigencia();                        
                    },
                    "Cerrar": function () {
                        $("#dlgVigenciaArchivo").empty();
                        $(this).dialog("close");
                    }
                }                
            });
                        
        });
        function eliminarArchivos(id, nombre, estado,fecha) {
            $(function () {
                $("#parrafoDialog2").html("");
                if (estado == "No Publicado")
                    $("#parrafoDialog2").append('<p>¿Está seguro que desea eliminar el archivo cargado <b>' + nombre + '</b>, con fecha '+ fecha +'.?</p>');
                else
                    $("#parrafoDialog2").append('<p>¿Está seguro que desea eliminar el archivo publicado <b>' + nombre + '</b>, con fecha ' + fecha + '.?</p>');
                
                $("#dialog2").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 500,
                    height: 250,
                    modal: true,
                    buttons: {
                        "Eliminar": function () {
                            $.get('<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>', { elimina: '1', idArchivo: id, nombreArchivo: nombre }
              , function (resp) {
                  window.location.href = '<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>';
              });
                        $(this).dialog("close");
                    },
                         "Cancelar": function () {
                             $(this).dialog("close");
                         }
                     }
                });
                $('#dialog2').dialog('open');
               
            });
        }
        function verArchivos(nombre) {
            window.open(nombre, "descarga", "directories=no, menubar =no,status=no,toolbar=no,location=no,scrollbars=no,fullscreen=no,width=10,top=10");
        }
        function cambioVigencia(idArchivo, estado, nombre, fecha) {
            if (estado == "Vigente") {
                $(function () {
                    $("#parrafoDialog2").html("");

                    $("#parrafoDialog2").append('<p>¿Está seguro que desea dejar como No Vigente el archivo  <b>' + nombre + '</b>, con fecha ' + fecha + '.?</p>');

                    $("#dialog2").dialog({
                        autoOpen: false,
                        resizable: false,
                        width: 500,
                        height: 250,
                        modal: true,
                        buttons: {
                            "Aceptar": function () {
                                $.get('<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>', { vigencia: '1', idArchivo: idArchivo, nombreArchivo: nombre }, function (resp) {
                                    window.location.href = '<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>';
                                });
                                $(this).dialog("close");
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $('#dialog2').dialog('open');

                });
            }
            else {
                $(function () {
                    $("#parrafoDialog2").html("");

                    $("#parrafoDialog2").append('<p>¿Está seguro que desea dejar como Vigente el archivo  <b>' + nombre + '</b>, con fecha ' + fecha + '.?</p>');

                    $("#dialog2").dialog({
                        autoOpen: false,
                        resizable: false,
                        width: 500,
                        height: 250,
                        modal: true,
                        buttons: {
                            "Aceptar": function () {
                                $.get('<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>', { vigencia: '0', idArchivo: idArchivo, nombreArchivo: nombre }, function (resp) {
                                    window.location.href = '<%=Me.Page.ResolveClientUrl("CargaArchivo.aspx")%>';
                                });
                                $(this).dialog("close");
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $('#dialog2').dialog('open');

                });
            }
        }

        //Muestra el diálogo que permite asociar usuarios con archivos. 
        function showAsociarUsuarioArchivoDialog(idArchivo) {
            $("#idArchivo").val(idArchivo);
            $("#dlgAsociarUsuarioArchivo").empty();
            $("#dlgAsociarUsuarioArchivo").load("AsociarArchivoPopup.html?idArchivo=" + idArchivo, function () {
                $("#dlgAsociarUsuarioArchivo").dialog("open");                
            });            
            return false;
        }

        //Muestra el diálogo que permite editar la vigencia de un archivo. 
        function showVigenciaArchivoDialog(idArchivo) {
            $("#idArchivo").val(idArchivo);
            $("#dlgVigenciaArchivo").empty();
            $("#dlgVigenciaArchivo").load("VigenciaArchivoPopup.html", function () {
                $("#dlgVigenciaArchivo").dialog("open");
            });
            return false;
        }          

        //Despliega una alerta para confirmar la publicación.
        function confirmarPublicacion() {
            var respuesta = confirm("¿Está seguro que desea publicar el o los archivos cargados?");
            if (respuesta) {
                return true;
            } else {
                return false;
            }
        }

        //Recarga la tabla con el historial de carga.
        function recargarTabla() {
            $("#<%=btnReload.ClientID%>").click();
        }

    </script>

    <div id="main" class="control">
        <div class="seccion">
            <asp:Literal ID="LbTxUsuarioPerfil" runat="server" />
            <h2>Carga y Publicación de Archivos</h2>
            <table style="width:100%; " border="0">
                <tr>
                    <td style="width:7%"><b>Tipo de dato oficial:</b></td>
                    <td style="width:83%"><asp:DropDownList ID="select_tipo_dato_oficial" runat="server" AutoPostBack="true" Height="18px" Width="190px"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width:7%"><b>Fecha de inicio:</b></td>
                    <td style="width:83%"><select id="select_inicio" runat="server" name="D1" style="Height:18px;Width:190px;"></select></td>
                </tr>
                <tr>
                    <td style="width:7%"><b>Fecha de corte:</b></td>
                    <td style="width:83%"><select id="select_mes" runat="server" name="D1" style="Height:18px;Width:190px;"></select></td>
                </tr>
                <tr>
                    <td style="width:7%"><b>Año:</b></td>
                    <td style="width:83%"><asp:DropDownList ID="select_periodo" runat="server" AutoPostBack="true" Height="18px" Width="190px"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width:7%"><b>Modificación al dato oficial:</b></td>
                    <td style="width:83%"><asp:TextBox ID="texto_modificacion" runat="server" name="D1" TextMode="MultiLine" Height="90px" Width="380px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chequea_sin_modificaciones" runat="server" Text="sin modificaciones" AutoPostBack="false" /></td>
                    <td><asp:CheckBox ID="chequea_entrega_indirecta" runat="server" Text="entrega indirecta" AutoPostBack="false" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr style="vertical-align:bottom;">
                    <td colspan="2" style="text-align:left; height:24px">
                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="return HabilitaText(this)" style="width: 590px; margin-top:0px; margin-bottom:0px; margin-left: 0px; border: #C3C5C6 1px solid; height:24px" />&nbsp;&nbsp; 
                        <asp:Button ID="cargaArchivo"  runat="server" Text="Cargar Archivo" Width="110px" />&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnPublicar" runat="server" Text="Enviar" OnClientClick="return confirmarPublicacion();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <textarea id="txtDescripcion" cols="99" rows="5" runat="server" style="resize: none; overflow: hidden; display:none;"></textarea>
                    </td>
                </tr>
            </table>
            <div id="divMensaje" runat="server" style="width:100%;text-align:left;height:auto">       
                <div runat="server" id="divIcon" style="width:16px;height:16px"></div>
                <asp:Label runat="server" ID="lblMensaje"></asp:Label>             
            </div>

            <table width="80%">
                <tr>
                    <td align="right">
            
                    </td>
                </tr>
            </table>
            <div id="dialog" title="">
                <p id="textoVentana" runat="server"></p>
            </div>
            <div id="dialog2" title="">
                <p id="parrafoDialog2"></p>
            </div>
            <div id="dialog3" title="">         
                <p>La carga y publicación han sido exitosas.</p>
            </div>
            <div id="dialog4" title="" style="display:none;">
                <p id="P1">Solo puede agregar 2 archivos al periodo seleccionados.</p>
            </div>
            <div id="dialog5" title="" style="display:none;">
                <p id="P2" runat="server"></p>
            </div>
            <div id="dialog6" title="">
                <p id="parrafoDialog6"></p>
            </div>

            <h2>Historial de Carga</h2>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional"  >
                <Triggers>
                    <ajax:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnReload" Text="Reload" CssClass="oculto"  />
                    <asp:GridView ID="GridView1" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
		                CssClass="grid" Width="100%" 
                                runat="server"> 
                                <FooterStyle CssClass="AdmResFoot"></FooterStyle>
                                <AlternatingRowStyle CssClass="gridAltRow" HorizontalAlign="Center"></AlternatingRowStyle>
                                <RowStyle CssClass="gridRow" HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle CssClass="gridHeader"></HeaderStyle>
                                <EmptyDataTemplate>
                                    <table class="grid">
                                        <tr class="gridHeader" >
                                            <td>Nombre Archivo</td>
                                            <td>Tamaño</td>
                                            <td>Descripción</td>
                                            <td>Fecha de Carga</td>
                                            <td>Administrador</td>
                                            <td>Fecha de Corte</td>
                                            <td>Año</td>
                                            <td>Estado</td>
                                            <td>Fecha de Publicación</td>
                                            <td>Vigencia</td>
                                            <td>Estado Vigencia</td>
                                            <td>Asociar</td>
                                            <td>Usuarios Asociados</td>
                                            <td>Ver</td>
                                            <td>Emininar</td>
                                        </tr>
                                        <tr class="gridRow" >
                                            <td colspan="15" style="text-align:center" >
                                                No existen archivos para desplegar.
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate> 
                                <Columns>   
                                    <asp:BoundField DataField="IdArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150" Visible="False" >
                                        <ItemStyle Width="150px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="estado" HeaderText="Id Estado" ItemStyle-Width="150" Visible="False" >
                                        <ItemStyle Width="150px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NombreArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150" >
                                        <ItemStyle Width="150px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="peso" HeaderText="Tamaño" ItemStyle-Width="10" >
                                        <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ItemStyle-Width="150" >
                                        <ItemStyle Width="150px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha de Carga" ItemStyle-Width="80" >    
                                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NombreUsuario" HeaderText="Administrador" ItemStyle-Width="150" >  
                                        <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="mes" HeaderText="Fecha de Corte" ItemStyle-Width="60" >  
                                        <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="periodo" HeaderText="Año" ItemStyle-Width="10" >
                                        <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="estado" HeaderText="Estado" ItemStyle-Width="60" >
                                        <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaPublicacion" HeaderText="Fecha de Publicación" ItemStyle-Width="80" >
                                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="urlDocto" HeaderText="url Archivo" ItemStyle-Width="150" Visible="False" >
                                        <ItemStyle Width="150px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vigencia" HeaderText="Vigencia" ItemStyle-Width="150" Visible="False" >
                                        <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>                    
                                    <asp:BoundField DataField="visible" HeaderText="visible" ItemStyle-Width="150" Visible="False" >
                                        <ItemStyle Width="150px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Asociar" ItemStyle-Width="10">
                                        <ItemTemplate>
                                            <a class="ui-silk ui-silk-folder-go" title="Asociar Archivo" style="cursor:pointer" onclick="return showAsociarUsuarioArchivoDialog(<%#Eval("IdArchivo")%>);"></a> 
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vigencia" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div class="ui-silk ui-silk-page-edit" style="width:16px;height:16px;cursor:pointer" onclick="return showVigenciaArchivoDialog(<%#Eval("IdArchivo")%>);"></div>                            
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="estadoVigencia" HeaderText="Estado Vigencia" ItemStyle-Width="70" Visible="True" >
                                        <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>                              
                                    <asp:BoundField DataField="usuariosAsociados" HeaderText="Usuarios Asociados" ItemStyle-Width="70" Visible="True" >
                                        <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>  
                                    <asp:TemplateField HeaderText="Ver" ItemStyle-Width="10">
                                        <ItemTemplate>                                    
                                            <a id="lbArchivo<%#Eval("IdArchivo")%>" style="cursor:pointer" class="ui-silk ui-silk-magnifier" title="Ver Archivo" href="<%#Eval("urlDocto")%>" target="_blank" ></a> 
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>  
                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-Width="10"   >
                                    <ItemTemplate>
                                        <a class="ui-silk ui-silk-delete" title="Eliminar Archivo" style="cursor:pointer;<%#Eval("visible")%>" id='<%#Eval("IdArchivo")%>_<%#Eval("NombreArchivo")%>' onclick='javascript:eliminarArchivos("<%#Eval("IdArchivo")%>","<%#Eval("NombreArchivo")%>","<%#Eval("estado")%>","<%#Eval("Fecha")%>")'></a>
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle HorizontalAlign ="Left" CssClass = "GridPager"/>
                             </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
                <br/>
             <div id="pager" runat="server"></div>
      
             <br/><br/><br/><br/><br/><br/>
            <div id="dlgAsociarUsuarioArchivo"></div>
            <div id="dlgVigenciaArchivo"></div>
            <input type="hidden" id="idArchivo"/> 
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="popups" Runat="Server">

</asp:Content>
