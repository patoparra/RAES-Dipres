<%@ Page Title="" Language="VB" MasterPageFile="~/General/GeneralPrincipal.master" AutoEventWireup="false" CodeFile="NoVigentes.aspx.vb" Inherits="NoVigentes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script  type="text/javascript">
        
        function verArchivos(nombre) {
            window.open(nombre, "descarga", "directories=no, menubar =no,status=no,toolbar=no,location=no,scrollbars=no,fullscreen=no,width=10,top=10");
        }
        function verArchivos2() {
            if (document.getElementById('<%= selBusqueda.ClientID%>').value == 1) {
                var mes = document.getElementById('<%=select_mes.ClientID%>').value;
                var periodo = document.getElementById('<%=select_periodo.ClientID%>').value;
                location.href = '../Reportes/ReporteNoVigentes.aspx?mes=' + mes + '&periodo=' + periodo;
            } else {
                location.href = '../Reportes/ReporteNoVigentes.aspx';
            }
        }
    </script>
    <div id="main" class="control">
        <div class="seccion">
            <a class="ui-silk ui-silk-page-white-excel" style="cursor:pointer;" runat="server" id="iReporte" onclick="verArchivos2();" target="_blank"></a>
            <h2>Filtros de Búsqueda</h2>
            <table style="width:100%;"  border="0">
                <tr>
                    <td style="width:15%"><b>Fecha de Corte:</b></td>
                    <td style="width:15%"><select id="select_mes" runat="server" name="D1" style="Height:18px;Width:190px;"></select></td>
                    <td style="width:70%">&nbsp;</td>        </tr>
                <tr>
                    <td style="width:15%"><b>Año:</b></td>
                    <td style="width:15%"><asp:DropDownList ID="select_periodo" runat="server" AutoPostBack="true" Height="18px" Width="190px"></asp:DropDownList></td>
                    <td style="width:70%">&nbsp;</td>
                </tr> 
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width:15%"></td>
                    <td style="width:15%"></td>
                    <td style="width:70%"><asp:Button ID="Buscar" runat="server" Text="Buscar" />&nbsp;&nbsp;<asp:Button ID="Todos" runat="server" Text="Ver Todos" /></td>
                </tr>
            </table>
            <h2>Archivos No Vigentes</h2>
            <input type="hidden" runat="server" id="selBusqueda" value="0"/>
            <asp:GridView ID="GridView1" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
            CssClass="grid" Width="100%"   pageSize= "10" 
                    runat="server"> 
                    <FooterStyle CssClass="AdmResFoot"></FooterStyle>
                    <AlternatingRowStyle CssClass="gridAltRow"></AlternatingRowStyle>
                    <RowStyle CssClass="gridRow"></RowStyle>
                    <HeaderStyle CssClass="gridHeader"></HeaderStyle>
                    <Columns>   
                        <asp:BoundField DataField="IdArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150" Visible="False" />
                        <asp:BoundField DataField="estado" HeaderText="Id Estado" ItemStyle-Width="150" Visible="False" />
                        <asp:BoundField DataField="NombreArchivo" HeaderText="Nombre Archivo" ItemStyle-Width="150" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha de Carga" ItemStyle-Width="150" Visible="True"/>    
                        <asp:BoundField DataField="NombreUsuario" HeaderText="Administrador" ItemStyle-Width="150" />  
                        <asp:BoundField DataField="mes" HeaderText="Mes" ItemStyle-Width="150" />  
                        <asp:BoundField DataField="periodo" HeaderText="Periodo" ItemStyle-Width="150" />
                        <asp:BoundField DataField="urlDocto" HeaderText="url Archivo" ItemStyle-Width="150" Visible="False" />
                            <asp:BoundField DataField="fecha_vigencia" HeaderText="Fecha No Vigencia" ItemStyle-Width="150" />
                        <asp:TemplateField HeaderText="Ver" ItemStyle-Width="30"  >
                        <ItemTemplate>
                            <a class="ui-silk ui-silk-magnifier" style="cursor: hand" id='V_<%#Eval("IdArchivo")%>_<%#Eval("NombreArchivo")%>' onclick='javascript:verArchivos("<%#Eval("urlDocto")%>")'></a>
                        </ItemTemplate>
                        </asp:TemplateField>  
                    
                    </Columns>
                    <PagerStyle HorizontalAlign = "center" CssClass = "GridPager" />
             </asp:GridView>   
        </div>
    </div> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="popups" Runat="Server">

</asp:Content>