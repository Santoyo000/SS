<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="RegistroEncargadoEscuela.aspx.cs" Inherits="Servicio_Social.RegistroEncargadoEscuela" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            // Event listener para el cambio en los campos de entrada
            $('#<%= txtEmailEnc.ClientID %>, #<%= txtExpedienteEnc.ClientID %>').on('input', function () {
                // Verificar cuál radio button está seleccionado
                var isOficial = $('#<%= rbtnOficial.ClientID %>').is(':checked');

                // Solo ejecutar la búsqueda si el radio button "Oficial" está seleccionado
                if (isOficial) {
                    var query = $('#<%= txtEmailEnc.ClientID %>').val();
                      var expediente = $('#<%= txtExpedienteEnc.ClientID %>').val();

                      if (query.length >= 12 && expediente.length >= 3) {
                          $.ajax({
                              type: "POST",
                              url: '<%= ResolveUrl("RegistroEncargadoEscuela.aspx/buscarCorreo") %>',
                        data: JSON.stringify({ correo: query, exp: expediente }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var results = response.d;
                            if (results.length > 0) {
                                $('#<%= txtNombreEnc.ClientID %>').val('');
                                $('#<%= txtApePatEnc.ClientID %>').val('');
                                $('#<%= txtApeMatEnc.ClientID %>').val('');

                                $('#<%= txtNombreEnc.ClientID %>').val(results[0]);
                                $('#<%= txtApePatEnc.ClientID %>').val(results[1]);
                                $('#<%= txtApeMatEnc.ClientID %>').val(results[2]);
                                $('#<%= lblMensajeEmailEnc.ClientID %>').text(''); // Limpiar el mensaje de error si hay resultados
                            } else {
                                // No se encontraron resultados, mostrar mensaje de error
                                $('#<%= lblMensajeEmailEnc.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                                $('#<%= txtNombreEnc.ClientID %>').val('');
                                $('#<%= txtApePatEnc.ClientID %>').val('');
                                $('#<%= txtApeMatEnc.ClientID %>').val('');
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error("Error: " + error);
                        }
                    });
                } else {
                    $('#<%= lblMensajeEmailEnc.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                    $('#<%= txtNombreEnc.ClientID %>').val('');
                    $('#<%= txtApePatEnc.ClientID %>').val('');
                    $('#<%= txtApeMatEnc.ClientID %>').val('');
                      }
                  } else {
                      // Si el tipo de usuario no es "Oficial", limpiar el mensaje de error
                      $('#<%= lblMensajeEmailEnc.ClientID %>').text('');
                }
            });
        });
    
    </script>
   
    <script>
        function validarPassword() {
            var password = document.getElementById("<%= txtContrasena.ClientID %>").value;
            var confirmPassword = document.getElementById("<%= txtConfirmarContrasena.ClientID %>").value;
            var btnSubmit = document.getElementById("<%= btnEncargado.ClientID %>");

            if (password === confirmPassword) {
                document.getElementById("mensaje").innerHTML = "";

                btnSubmit.disabled = false; // Habilitar el botón
            } else {
                document.getElementById("mensaje").innerHTML = "Las contraseñas no coinciden";
                btnSubmit.disabled = true; // Deshabilitar el botón
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
<br />
<div class="container">
<%--    <div class="nav nav-tabs" id="TabSelection">
        <asp:LinkButton ID="lbnUsuario" runat="server" CssClass="nav-item nav-link" Text="Dependencias" OnClick="lbnDependencias_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbnResponsable" runat="server" CssClass="nav-item nav-link" Text="Responsables de Unidad" OnClick="lbnResponsable_Click"></asp:LinkButton>
        
    </div>
</div>--%>
   <asp:Panel ID="DATOS_USUARIO" runat="server" Visible="true">
    <div style="text-align: center">
        <div class="form-group">
            <br />
            <h3 class="text-gray-900 mb-4" style="color: #2e5790">Capture los datos del Encargado:</h3>
        </div>
    </div>
    <div class="container">
        <div class="form-group">
            <label style="font-weight: bold;">Seleccione el tipo de Encargado:</label>
            <div class="form-check form-check-inline">
                <asp:RadioButton ID="rbtnOficial" runat="server" GroupName="tipoEncargado" Text=" Encargado Escuela Oficial" AutoPostBack="true" OnCheckedChanged="rbtnOficial_CheckedChanged" />
            </div>
            <div class="form-check form-check-inline">
                <asp:RadioButton ID="rbtnIncorporada" runat="server" GroupName="tipoEncargado" Text=" Encargado Escuela Incorporada" AutoPostBack="true" OnCheckedChanged="rbtnIncorporada_CheckedChanged" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('input[name="tipoEncargado"]').on('change', function () {
                var isOficial = $('#<%= rbtnOficial.ClientID %>').is(':checked');
                $('#<%= pnlEncargadoOficial.ClientID %>').toggle(isOficial);
                $('#<%= pnlIncorporada.ClientID %>').toggle(!isOficial);
            });
        });
</script>
    <div class="container">
        <%-- DATOS DEL ENCArGADO OFICIAL--%>
        <asp:Panel ID="pnlEncargadoOficial" runat="server" Visible="false">
            <%-- LOS COMBOS --%>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="ddlUnidadEnc" class="label-derecha">Unidad:</label>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlUnidadEnc" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlUnidadEnc_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="ddlNivelEnc" class="label-derecha">Nivel:</label>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlNivelEnc" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlNivelEnc_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="ddlEscuelEnc" class="label-derecha">Escuela:</label>
                        <asp:DropDownList runat="server" ID="ddlEscuelEnc" CssClass="form-control" required="required"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
            <%--<asp:Panel ID="pnlDatOficial" runat="server" Visible="false">--%>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtExpedienteEnc" id="lblExpediente" associatedcontrolid="txtExpedienteEnc" runat="server" class="label-derecha">Expediente:</label>
                            <asp:TextBox ID="txtExpedienteEnc" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label5"></asp:Label>
                    </div>
            <%--</asp:Panel>--%>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtEmailEnc" class="label-derecha">Correo Institucional:</label>
                            <asp:TextBox ID="txtEmailEnc" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regexEmail" runat="server"
                                ControlToValidate="txtEmailEnc"
                                ErrorMessage="El formato del correo electrónico no es válido."
                                ValidationExpression="^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$"
                                Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEmailEnc"></asp:Label>
                    </div>
                </div>
            
        <%-- ////////////////////////////////////////// --%>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtNombreEnc" class="label-derecha">Nombre:</label>
                    <asp:TextBox ID="txtNombreEnc" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtApePatEnc" class="label-derecha">Apellido Paterno:</label>
                    <asp:TextBox ID="txtApePatEnc" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtApeMatEnc" class="label-derecha">Apellido Materno:</label>
                    <asp:TextBox ID="txtApeMatEnc" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtTelefonoEnc" class="label-derecha">Teléfono:</label>
                    <asp:TextBox ID="txtTelefonoEnc" runat="server" CssClass="form-control" required="required" MaxLength="10"></asp:TextBox>
                </div>
                <asp:Label runat="server" Style="color: #ff0d0d" ID="Label6"></asp:Label>
            </div>
        </div>
        <%-- DATOS DEL ENCArGADO INCORPORADA --%>
        <asp:Panel ID="pnlIncorporada" runat="server" Visible="false">
        <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblContrasena" runat="server" AssociatedControlID="txtContrasena" CssClass="form-label">Contraseña:</asp:Label>
                        <asp:TextBox ID="txtContrasena" CssClass="form-control" runat="server" required="required" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
        
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblConfirmarContrasena" runat="server" AssociatedControlID="txtConfirmarContrasena" CssClass="form-label">Confirmar contraseña:</asp:Label>
                        <asp:TextBox ID="txtConfirmarContrasena" runat="server" required="required" TextMode="Password" CssClass="form-control" onkeyup="validarPassword()"></asp:TextBox>
                        <div id="mensaje" style="color: #ff0d0d"></div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="row">
            <div class="text-center">
                <asp:Label ID="lblMensajeErrorEnc" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col-md-4 text-center">
                <asp:Button runat="server" ID="btnEncargado" CssClass="btn miBoton" Text="CREAR ENCARGADO" OnClick="btnEncargado_Click" />
            </div>
        </div>
        </asp:Panel>
    </div>
 </asp:Panel>
    <br />
    <br />
    <br />
</asp:Content>
