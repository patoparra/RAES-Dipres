<%@ Page Title="" Language="VB" MasterPageFile="~/General/GeneralPrincipal.master" AutoEventWireup="false" CodeFile="LogDescarga.aspx.vb" Inherits="LogDescarga" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <script  type="text/javascript">
        
        $(document).ready(function () {
            $(".botonMenu").mouseover(function () {
                $(".listaReportes").show();
                $(".listaReportes").unbind("show");
            });

            $(".botonReportes").mouseleave(function () {
                $(".listaReportes").toggle(function () {
                    //Animation complete.
                });
            })
        });

        function verArchivos2() {
            if (document.getElementById('<%= selBusqueda.ClientID%>').value == 1) {
                var mes = document.getElementById('<%=select_mes.ClientID%>').value;
                var periodo = document.getElementById('<%=select_periodo.ClientID%>').value;
                var usuario = document.getElementById('<%=select_usuario.ClientID%>').value;
                var fd = document.getElementById('<%=txtFechaDescarga.ClientID%>').value;
                location.href = '../Reportes/ReporteDescargas.aspx?mes='+mes+'&periodo='+periodo+'&usuario='+usuario+'&fd='+fd;
            }else{
                location.href = '../Reportes/ReporteDescargas.aspx';
            }
        }

        $(document).ready(function () {

            //Crea el datepicker para el campo Fecha de Descarga.
            var idfd = '<%=txtFechaDescarga.ClientID%>';
            var minDate = new Date(2015, 0, 1);
            var maxDate = new Date();
            $("#" + idfd).datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: 'mm-yy',
                minDate: minDate,
                maxDate: maxDate,
                onClose: function (dateText, inst) {
                    var anio = inst.selectedYear;
                    var mes = inst.selectedMonth;
                    $("#" + idfd).val(getNumeroMes(mes) + "-" + anio);
                }
            });

        });

        //Limpia el valor de los filtros del formulario.
        function limpiarFiltros() {
            $("#<%=txtFechaDescarga.ClientID%>").val("");
            $("#<%=divMensaje.ClientID%>").fadeOut("fast");
            return false;
        }

        //Retorna el texto de un mes en base a su índice en el datepicker.
        function getNumeroMes(mes) {
            mes += 1;
            if (mes < 10) {
                return "0" + mes;
            } else {
                return "" + mes
            }
        }

    </script>
    <div id="main" class="control">
        <div class="seccion">
            

            <div id="botonReportes" runat="server" class="botonReportes" title="Presione aquí para visualizar Reporte.">
                <div class="botonMenu">
                    <span>Reporte Log de Descarga</span>
                    <div class="ui-silk ui-silk-printer">
                        &nbsp;
                    </div>
                </div>
                <div class="listaReportes" style="display: none">
                    <ul>
                        <li class="tipoDoc">
                            <div>
                                <div class="ui-silk ui-silk-page-excel">
                                    &nbsp;
                                </div>
                                <div>Formato Excel</div>
                            </div>
                            <ul>
                                <li id="tblInformeValidacion" runat="server">
                                    <a style="cursor:pointer;" runat="server" id="iReporte" onclick="verArchivos2();" target="_blank">Reporte Log de Descarga</a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>

            <h2>Filtros de Búsqueda</h2>
            <table style="width:100%;"  border="0">
                <tr>
                    <td style="width:10%"><b>Fecha de Corte:</b></td>
                    <td style="width:10%"><asp:DropDownList ID="select_mes" runat="server" AutoPostBack="true" style="Height:18px;Width:190px;"></asp:DropDownList></td>
                    <td style="width:80%">&nbsp;</td>        </tr>
                <tr>
                    <td style="width:10%"><b>Año:</b></td>
                    <td style="width:10%"><asp:DropDownList ID="select_periodo" runat="server" AutoPostBack="true" Height="18px" Width="190px"></asp:DropDownList></td>
                    <td style="width:80%">&nbsp;</td>
                </tr> 
                <tr>
                    <td style="width: 10%"><b>Usuario:</b></td>
                    <td style="width:10%"><asp:DropDownList ID="select_usuario" runat="server" AutoPostBack="true" Height="18px" Width="190px"></asp:DropDownList></td>
                    <td style="width:80%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"><b>Fecha de Descarga:</b></td>
                    <td style="width:10%">
                        <asp:TextBox runat="server" ID="txtFechaDescarga" MaxLength="7" Width="50px" ></asp:TextBox>
                        <asp:Button runat="server" ID="btnLimpiar" OnClientClick="return limpiarFiltros();"  Text="Limpiar"/>
                    </td>
                    <td style="width:80%">&nbsp;</td>
                </tr>             
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width:10%"></td>
                    <td style="width:10%"></td>
                    <td style="width:80%"><asp:Button ID="Buscar" runat="server" Text="Buscar" />&nbsp;&nbsp;<asp:Button ID="Todos" runat="server" Text="Ver Todos" /></td>
                </tr>
            </table>
            <div id="divMensaje" runat="server" style="width:100%;text-align:left;height:auto">       
                <div runat="server" id="divIcon" style="width:16px;height:16px"></div>
                <asp:Label runat="server" ID="lblMensaje"></asp:Label>             
            </div>

            <h2>Log de Descarga</h2>
            <input type="hidden" runat="server" id="selBusqueda" value="0"/>
             <asp:GridView ID="GridView1" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" 
		        CssClass="grid" Width="100%"   pageSize= "10" 
                        runat="server"> 
                        <FooterStyle CssClass="AdmResFoot"></FooterStyle>
                        <AlternatingRowStyle CssClass="gridAltRow"></AlternatingRowStyle>
                        <RowStyle CssClass="gridRow"></RowStyle>
                        <HeaderStyle CssClass="gridHeader"></HeaderStyle>
                        <EmptyDataTemplate>
                            <table class="grid">
                                <tr class="gridHeader" >
                                    <td>Nombre Archivo</td>
                                    <td>Fecha de Descarga</td>
                                    <td>Usuario</td>
                                    <td>Fecha de Corte</td>
                                    <td>Año</td>
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
                            <asp:BoundField DataField="IdArchivo" HeaderText="Id Archivo" ItemStyle-Width="150" Visible="False" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="IdDescarga" HeaderText="Id Descarga" ItemStyle-Width="150" Visible="False" />
                            <asp:BoundField DataField="estado" HeaderText="Id Estado" ItemStyle-Width="150" Visible="False" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha de Carga" ItemStyle-Width="150" Visible="False">    
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaDescarga" HeaderText="Fecha de Descarga" ItemStyle-Width="150" >    
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" ItemStyle-Width="150" >  
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Mes" HeaderText="Fecha de Corte" ItemStyle-Width="150" >  
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Periodo" HeaderText="Año" ItemStyle-Width="150" >
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="estado" HeaderText="Estado" ItemStyle-Width="150" Visible="False">
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="urlDocto" HeaderText="url Archivo" ItemStyle-Width="150" Visible="False" >
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="vigencia" HeaderText="Vigencia" ItemStyle-Width="150" Visible="False">
                                <ItemStyle Width="150px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Ver" ItemStyle-Width="30"  >
                                <ItemTemplate>
                                    <a class="ui-silk ui-silk-magnifier" title="Ver Archivo" id='lbArchivo<%#Eval("IdArchivo")%>' href="<%#Eval("urlDocto")%>" target="_blank"></a>
                                </ItemTemplate>
                                <ItemStyle Width="30px" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>  
                   
                        </Columns>
                        <PagerStyle HorizontalAlign = "center" CssClass = "GridPager" />
 
                    </asp:GridView>
             <br />
             <div id="pager" runat="server"></div>
        </div>
    </div>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="popups" Runat="Server">

</asp:Content>
