<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="RegistroResponsable.aspx.cs" Inherits="Servicio_Social.RegistroResponsable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= txtEmail.ClientID %>, #<%= txtExpediente.ClientID %>').on('input', function () {
                var query = $('#<%= txtEmail.ClientID %>').val();
                var expediente = $('#<%= txtExpediente.ClientID %>').val();
                if (query.length >= 12 && expediente.length >= 3) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("RegistroResponsable.aspx/buscarCorreo") %>',
                        data: JSON.stringify({ correo: query, exp: expediente }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var results = response.d;
                            if (results.length > 0) {
                                $('#<%= txtNombre.ClientID %>').val('');
                                $('#<%= txtApePat.ClientID %>').val('');
                                $('#<%= txtApeMat.ClientID %>').val('');

                                $('#<%= txtNombre.ClientID %>').val(results[0]);
                                $('#<%= txtApePat.ClientID %>').val(results[1]);
                                $('#<%= txtApeMat.ClientID %>').val(results[2]);
                                $('#<%= lblMensajeEmail.ClientID %>').text(''); // Limpiar el mensaje de error si hay resultados
                            } else {
                                // No se encontraron resultados, mostrar mensaje de error
                                $('#<%= lblMensajeEmail.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                                $('#<%= txtNombre.ClientID %>').val('');
                                $('#<%= txtApePat.ClientID %>').val('');
                                $('#<%= txtApeMat.ClientID %>').val('');
                            }

                        },
                        error: function (xhr, status, error) {
                            console.error("Error: " + error);
                        }
                    });
                }
                else {
                    $('#<%= lblMensajeEmail.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                    $('#<%= txtNombre.ClientID %>').val('');
                    $('#<%= txtApePat.ClientID %>').val('');
                    $('#<%= txtApeMat.ClientID %>').val('');
                }
            });
        });
    </script>
    <style>
        .contenedor {
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:Panel ID="pnlRegistrarResponsable" runat="server" Visible="true">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Completa los siguientes datos:</h2>
            </div>
        </div>
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtExpediente" class="label-derecha">Expediente:</label>
                        <asp:TextBox ID="txtExpediente" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeExpediente"></asp:Label>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtEmail" class="label-derecha">Correo Institucional:</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEmail"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtNombre" class="label-derecha">Nombre:</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtApePat" class="label-derecha">Apellido Paterno:</label>
                        <asp:TextBox ID="txtApePat" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtApeMat" class="label-derecha">Apellido Materno:</label>
                        <asp:TextBox ID="txtApeMat" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtTelefono" class="label-derecha">Teléfono:</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" required="required" MaxLength="10"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeTelefono"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="ddlUnidad" class="label-derecha">Unidad:</label>
                        <asp:DropDownList runat="server" ID="ddlUnidad" CssClass="form-control" required="required"></asp:DropDownList>
                    </div>
                </div>
                <div class="text-center">
                    <asp:Label ID="lblMensajeError" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <div class="row justify-content-center">
                <div class="col-md-4 text-right">
                    <a href="Home.aspx" class="btn miBoton" style="line-height: 40px;">Regresar</a>
                </div>
                <div class="col-md-4 text-left">
                    <asp:Button runat="server" ID="btnRegistrar" CssClass="btn miBoton" Text="Registrar" OnClick="btnRegistrar_Click" />
                </div>
                
            </div>
        </div>
        <br />
        <br />
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlRegistroExitoso" runat="server" Visible="false" CssClass="contenedor">
        <h2 style="color: #333333; font-size: 24px; margin-bottom: 20px;">¡Registro Exitoso!</h2>
        <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Nos complace informarle que ha sido registrado exitosamente en nuestro sistema.</p>
        <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Una vez que se haya autorizado tendrá acceso a la Plataforma.</p>
        <footer style="text-align: center; color: #999999; font-style: italic; font-size: 14px;">¡Gracias por registrarse!</footer>
        <div style="text-align: center; margin-top: 20px;">
            <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
        </div>
    </asp:Panel>

</asp:Content>
