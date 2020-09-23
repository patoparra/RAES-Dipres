<%@ Page Language="VB" MasterPageFile="~/General/GeneralPrincipal.master" AutoEventWireup="false" CodeFile="Contactos.aspx.vb" Inherits="General_Contactos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            cargarContactos();
        });

        //Retorna el identificador del usuario.
        function getIdUsuario() {
            return parseInt($("#<%=txtIdUsuario.ClientID%>").val());            
        }

        //Carga los contactos asociados al usuario.
        function cargarContactos() {            
            var idUsuario = getIdUsuario();

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
            var clase = "gridRow";
            if (contactos.length > 0) {
                numero = contactos.length + 1;

                for (var i = 0; i < contactos.length; i++) {
                    var c = contactos[i];
                    var tdNumero = "<td style='text-align:center'>" + (i + 1) + "</td>";
                    var tdNombre = "<td><input type='text' id='txtNombre" + i + "' maxlength='50' value='" + c.nombre + "'></input></td>";
                    var tdCargo = "<td><input type='text' id='txtCargo" + i + "' maxlength='50' value='" + c.cargo + "'></input></td>";
                    var tdTelefono = "<td><input type='text' id='txtTelefono" + i + "' maxlength='10' value='" + c.telefono + "' style='width:100px'></input></td>";
                    var tdEmail = "<td><input type='text' id='txtEmail" + i + "' maxlength='50' value='" + c.email + "'></input></td>";

                    var guardar = "<div style='width:16px;height:16px;cursor:pointer' class='ui-silk ui-silk-disk' title='Guardar Contacto.' onclick='return guardarContacto(" + i + "," + c.idContacto + ");'/>";
                    var eliminar = "<div style='width:16px;height:16px;cursor:pointer' class='ui-silk ui-silk-cancel' title='Eliminar Contacto.' onclick='return eliminarContacto(" + c.idContacto + ");'/>";
                    var tdAcciones = "<td>" + guardar + eliminar + "</td>";

                    clase = "gridRow";
                    if ((i % 2) != 0) {
                        clase = "gridAltRow";                    
                    }

                    var trContacto = "<tr class='"+clase+"' >" + tdNumero + tdNombre + tdCargo + tdTelefono + tdEmail + tdAcciones + "</tr>";
                    $("#tbContactos").append(trContacto);
                }
            }

            //Agregar la fila para un nuevo contacto.
            var tdNumeroVacio = "<td style='text-align:center'>" + numero + "</td>";
            var tdNombreVacio = "<td><input type='text' id='txtNombre" + (numero - 1) + "' maxlength='50' value=''></input></td>";
            var tdCargoVacio = "<td><input type='text' id='txtCargo" + (numero - 1) + "' maxlength='50' value=''></input></td>";
            var tdTelefonoVacio = "<td><input type='text' id='txtTelefono" + (numero - 1) + "' maxlength='10' value='' style='width:100px'></input></td>";
            var tdEmailVacio = "<td><input type='text' id='txtEmail" + (numero - 1) + "' maxlength='50' value=''></input></td>";
            var guardarVacio = "<div style='width:16px;height:16px;cursor:pointer' class='ui-silk ui-silk-disk' title='Guardar Contacto.' onclick='return guardarContacto(" + (numero - 1) + ");'/>";
            var tdAccionesVacio = "<td>" + guardarVacio + "</td>";

            if (clase == "gridRow")
                clase = "gridAltRow";
            else
                clase = "gridRow";

            var trContactoVacio = "<tr class='"+clase+"'>" + tdNumeroVacio + tdNombreVacio + tdCargoVacio + tdTelefonoVacio + tdEmailVacio + tdAccionesVacio + "</tr>";
            $("#tbContactos").append(trContactoVacio);
        }

        //Guarda la información de un contacto.
        function guardarContacto(indice, idContacto) {
            mostrarMsg(false, null, false);
            var c = getContacto(indice);
            c.idContacto = idContacto;
            $.ajax({
                type: "POST",
                dataType: "json",
                data: "{'contacto':" + JSON.stringify(c) + "}",
                contentType: "application/json; charset=utf-8",
                url: "Contactos.aspx/guardarContacto",
                success: function (data, textStatus, jqXHR) {
                    var r = JSON.parse(data.d);
                    if (r.exito == 1) {
                        cargarContactos();
                        mostrarMsg(true, r.mensaje, false);
                    } else {
                        mostrarMsg(true, r.mensaje, true);
                    }
                }
            });
            return false;
        }

        //Rescata la información de un contacto.
        function getContacto(indice) {
            var c = new Object();
            c.idUsuario = getIdUsuario();
            c.nombre = $("#txtNombre" + indice).val();
            c.cargo = $("#txtCargo" + indice).val();
            c.telefono = $("#txtTelefono" + indice).val();
            c.email = $("#txtEmail" + indice).val();
            return c;
        }

        //Elimina el registro de un contacto.
        function eliminarContacto(idContacto) {
            mostrarMsg(false, null, false);
            $.ajax({
                type: "POST",
                dataType: "json",
                data: "{'idContacto':" + idContacto + "}",
                contentType: "application/json; charset=utf-8",
                url: "Contactos.aspx/eliminarContacto",
                success: function (data, textStatus, jqXHR) {
                    var r = JSON.parse(data.d);
                    if (r.exito == 1) {
                        cargarContactos();
                        mostrarMsg(true, r.mensaje, false);
                    } else {
                        mostrarMsg(true, r.mensaje, true);
                    }
                }
            });
            return false;
        }

        //Despliega un mensaje de exito o error por pantalla.
        function mostrarMsg(mostrar, msg, error) {

            //Determina la clase del icono.
            var clase = "ui-silk ui-silk-cancel";
            if (!error)
                clase = "ui-silk ui-silk-accept";

            $("#divIconMsgContacto").removeClass();
            $("#divIconMsgContacto").addClass(clase);

            if (mostrar) {
                $("#lblMsgContacto").html(msg);
                $("#divMsgContacto").fadeIn("fast");
            } else {
                $("#divMsgContacto").fadeOut("fast");
            }
        }

    </script>

    <div id="main" class="control">
        <div class="seccion">    
            <h2>Datos de Contacto</h2>
            <asp:HiddenField runat="server" ID="txtIdUsuario"/> 
            <table>
                <tr>
                    <td>
                        <table id="tbContactos" class="grid" style="width:100%" >
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
                                <td style="width:10%" >
                                    Acciones
                                </td>                                         
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divMsgContacto" style="display:none;vertical-align:central;width:750px">
                            <table style="width:auto" >
                                <tr>
                                    <td>
                                        <div id="divIconMsgContacto" style="width:16px;height:16px" class="ui-silk ui-silk-cancel"/>
                                    </td>
                                    <td style="text-align:left" >
                                        <label id="lblMsgContacto"></label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>   
        </div>
    </div>           
</asp:Content>
