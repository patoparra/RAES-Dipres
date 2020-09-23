<%@ Page Title="" Language="VB" MasterPageFile="~/General/GeneralPrincipal.master" AutoEventWireup="false" CodeFile="DescargaArchivo.aspx.vb" Inherits="DescargaArchivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script  type="text/javascript">
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
                            // window.location.href = '<%=Me.Page.ResolveClientUrl("ExitoCargaArchivo.aspx")%>';
                            $('#dialog3').dialog('open');
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

            //Crea el DatePicker para el filtro Fecha Publicación. 
            var idtxtfdp = "<%=txtFechaPublicacion.ClientID%>";
            var minDate = new Date(2015, 0, 1);
            var maxDate = new Date();
            $("#" + idtxtfdp).datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'mm-yy',
                minDate: minDate,
                maxDate: maxDate,
                onClose: function (dateText, inst) {
                    var anio = inst.selectedYear;
                    var mes = inst.selectedMonth;
                    $("#"+idtxtfdp).val(getNumeroMes(mes)+"-"+anio);                    
                }
            });

        });

        //Retorna el texto de un mes en base a su índice en el datepicker.
        function getNumeroMes(mes) {
            mes += 1;
            if (mes < 10) {
                return "0" + mes;
            } else {
                return ""+mes
            }            
        }

        function eliminarArchivos(id, nombre, estado) {
            
            $(function () {
                $("#parrafoDialog2").html("");
                if (estado == "No Publicado")
                    $("#parrafoDialog2").append('<p><strong>¿Está seguro que desea eliminar el archivo cargado?</strong></p>');
                else
                    $("#parrafoDialog2").append('<p><strong>¿Está seguro que desea eliminar el archivo publicado?</strong></p>');

                $("#dialog2").dialog({
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
            //location.reload();
}
        
        //Limpia el valor de los filtros del formulario.
        function limpiarFiltros() {
            $("#<%=txtFechaPublicacion.ClientID%>").val("");
            $("#<%=divMensaje.ClientID%>").fadeOut("fast");
            return false;
        }

        function descargarArchivo(nombreArchivo, idArchivo) {
            $.ajax({
                type: "POST",
                dataType: "json",
                data: "{'nombreArchivo':'" + nombreArchivo + "','idArchivo':" + idArchivo + "}",
                contentType: "application/json; charset=utf-8",
                url: "DescargaArchivo.aspx/registrarDescarga",
                success: function (data, textStatus, jqXHR) {
                    var r = JSON.parse(data.d);
                    if (r.exito == 0) {
                        mostrarMsg(true, r.mensaje, true);
                    }
                }
            });
            return true;
        }

        //Recarga la tabla con el historial de carga.
        function recargarTabla() {
            $("#<%=btnReload.ClientID%>").click();
        }

    </script>

    <div id="main" class="control">
        <div class="seccion">
             <asp:Literal ID="LbTxUsuarioPerfil" runat="server" />
            <div id="dialog" title="Publicar de Archivos">
                <p id="textoVentana" runat="server"></p>
            </div>
            <div id="dialog2" title="Publicar Archivos">
                <p id="parrafoDialog2"></p>
            </div>
            <div id="dialog3" title="Publicar Archivos">
                <p>La carga y publicación han sido exitosas.</p>
            </div>
            <div id="dialog4" title="Alerta" style="display:none;">
                <p id="P1" runat="server">Solo puede agregar 2 archivos al periodo seleccionados</p>
            </div>

            <h2>Archivos Publicados</h2>

            <table style="width:auto"> 
                <tr>
                    <td>
                        <b>Fecha de Publicación: </b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaPublicacion" runat="server" CssClass="date-picker" /> 
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnLimpiar" Text="Limpiar" OnClientClick="return limpiarFiltros();" />
                        <asp:Button runat="server" ID="btnBuscar" Text="Buscar"/> 
                    </td>
                </tr>
            </table>
            <br />
            <div id="divMensaje" runat="server" style="width:100%;text-align:left;height:30px">       
                <div runat="server" id="divIcon" style="width:16px;height:16px"></div>
                <asp:Label runat="server" ID="lblMensaje"></asp:Label>             
            </div>   
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  >
                <Triggers>
                    <ajax:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnReload" Text="Reload" CssClass="oculto"  />
                    <asp:GridView ID="GridView1" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
		                CssClass="grid" PageSize="15" Width="100%"  
                        runat="server"> 
                        <FooterStyle CssClass="AdmResFoot"></FooterStyle>
                        <AlternatingRowStyle CssClass="gridAltRow"></AlternatingRowStyle>
                        <RowStyle CssClass="gridRow"></RowStyle>
                        <HeaderStyle CssClass="gridHeader"></HeaderStyle>
                        <EmptyDataTemplate>
                            <table class="grid">
                                <tr class="gridHeader" >
                                    <td>Nombre Archivo</td>
                                    <td>Tamaño</td>
                                    <td>Descripción</td>
                                    <td>Fecha de Corte</td>
                                    <td>Año</td>
                                    <td>Fecha de Publicación</td>
                                    <td>Ver</td>
                                </tr>
                                <tr class="gridRow" >
                                    <td colspan="7" style="text-align:center" >
                                        No existen archivos para desplegar.
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate> 
                        <Columns>   
                            <asp:BoundField DataField="IdArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150" Visible="False"  >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="estado" HeaderText="Id Estado" ItemStyle-Width="150" Visible="False" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150"  >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="peso" HeaderText="Tamaño" ItemStyle-Width="10" >
                                <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="mes" HeaderText="Fecha de Corte" ItemStyle-Width="60" >  
                                <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="periodo" HeaderText="Año" ItemStyle-Width="10" >
                                <ItemStyle Width="10px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="urlDocto" HeaderText="url Archivo" ItemStyle-Width="150" Visible="False" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="fechaPublicacion" HeaderText="Fecha de Publicación" ItemStyle-Width="80"  >
                                <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Ver" ItemStyle-Width="10"  >
                            <ItemTemplate>                        
                                <a class="ui-silk ui-silk-magnifier" title="Ver Archivo" style="cursor:pointer" id='lbArchivo<%#Eval("IdArchivo")%>' onclick='return descargarArchivo("<%#Eval("urlDocto")%>","<%#Eval("IdArchivo")%>")' href="<%#Eval("urlDocto")%>" target="_blank"></a>
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
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="popups" Runat="Server">

</asp:Content>
