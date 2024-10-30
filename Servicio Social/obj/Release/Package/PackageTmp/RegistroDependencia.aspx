<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="RegistroDependencia.aspx.cs" Inherits="Servicio_Social.RegistroDependencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function validarPassword() {
            var password = document.getElementById("<%= txtContrasena.ClientID %>").value;
            var confirmPassword = document.getElementById("<%= txtConfirmarContrasena.ClientID %>").value;
            var btnSubmit = document.getElementById("<%= btnRegistrar.ClientID %>");

            if (password === confirmPassword) {
                document.getElementById("mensaje").innerHTML = "";

                btnSubmit.disabled = false; // Habilitar el botón
            } else {
                document.getElementById("mensaje").innerHTML = "Las contraseñas no coinciden";
                btnSubmit.disabled = true; // Deshabilitar el botón
            }
        }
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
  <asp:Panel ID="PanelCerrado" runat="server" Visible="true">
      <div style="text-align: center">
    <div class="form-group">
        <br />
        <h3 id="mensajeCierre" class="text-gray-900 mb-4" style="color: #2e5790">
         <asp:Label ID="lblMensajeDependencia" runat="server"></asp:Label>
        </h3>
         <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
    </div>
</div>
  </asp:Panel>
 <asp:Panel ID="pnlRegistrarDependencia" runat="server" Visible="false">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h3 class="text-gray-900 mb-4" style="color: #2e5790">Llene los siguientes datos que se muestran a continuación:</h3>
            </div>
        </div>
 <div class="container">
    <div class="row mb-3">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblDependencia" runat="server" AssociatedControlID="txtDependencia" CssClass="form-label">Dependencia:</asp:Label>
                <asp:TextBox ID="txtDependencia" runat="server" CssClass="form-control" required="required"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="form-label">E-mail:</asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regexEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="El formato del correo electrónico no es válido."
                    ValidationExpression="^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$"
                    Display="Dynamic">
                </asp:RegularExpressionValidator>
                <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensaje"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row mb-3">
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
    <div class="row mb-3">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblUnidad" runat="server" AssociatedControlID="ddlUnidad2" CssClass="form-label">Unidad:</asp:Label>
                <asp:DropDownList ID="ddlUnidad2" runat="server" CssClass="form-select" required="required"></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblOrganismo" runat="server" AssociatedControlID="ddlOrganismo" CssClass="form-label">Organismo:</asp:Label>
                <asp:DropDownList ID="ddlOrganismo" runat="server" CssClass="form-select" required="required"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row mb-3">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblResponsable" runat="server" AssociatedControlID="txtResponsable" CssClass="form-label">Responsable:</asp:Label>
                <asp:TextBox ID="txtResponsable" runat="server" CssClass="form-control" MaxLength="50" required="required"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblAreaResponsable" runat="server" AssociatedControlID="txtAreaResponsable" CssClass="form-label">Área responsable:</asp:Label>
                <asp:TextBox ID="txtAreaResponsable" runat="server" CssClass="form-control" MaxLength="50" required="required"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row mb-3">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblTelefono" runat="server" AssociatedControlID="txtTelefono" CssClass="form-label">Teléfono:</asp:Label>
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" required="required" MaxLength="10" onkeypress="return validarSoloNumeros(event)"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblDomicilio" runat="server" AssociatedControlID="txtDomicilio" CssClass="form-label">Domicilio:</asp:Label>
                <asp:TextBox ID="txtDomicilio" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" required="required"></asp:TextBox>
            </div>
        </div>
    </div>
           <div class="row justify-content-center mt-4">
                <div class="col text-start">
                    <a href="Home.aspx" class="btn btn-secondary" role="button">Regresar</a>
                </div>
                <div class="col text-center">
                    <asp:Button runat="server" ID="btnRegistrar" CssClass="btn btn-primary" Text="Registrar" OnClick="btnRegistrar_Click" />
                </div>
    </div>
</div>
       
    </asp:Panel>
    <asp:Panel ID="pnlRegistroExitoso" runat="server" Visible="false" CssClass="contenedor">
        <h2 style="color: #333333; font-size: 24px; margin-bottom: 20px;">¡Registro Exitoso!</h2>
        <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Nos complace informarle que ha sido registrado exitosamente en nuestro sistema.</p>
        <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Para completar el proceso de autorización, le solicitamos que valide los datos proporcionados dentro de las próximas 48 horas. Por favor, póngase en contacto con nuestro equipo de validación llamando a los siguientes números telefónicos:</p>
        <ul style="list-style-type: none; padding-left: 0; margin-bottom: 20px;">
            <li style="margin-bottom: 10px;"><span style="color: #2e5790; font-weight: bold;">Unidad Saltillo:</span> <a href="tel:8444124477" style="color: #2e5790; text-decoration: none;">844 412 44 77</a></li>
            <li style="margin-bottom: 10px;"><span style="color: #2e5790; font-weight: bold;">Unidad Norte:</span> <a href="tel:8666496026" style="color: #2e5790; text-decoration: none;">866 649 60 26</a></li>
            <li style="margin-bottom: 10px;"><span style="color: #2e5790; font-weight: bold;">Unidad Torreón:</span> <a href="tel:8717293208" style="color: #2e5790; text-decoration: none;">871 729 32 08</a></li>
        </ul>
        <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Una vez que se haya completado el proceso de validación, recibirá un correo electrónico de confirmación.</p>
        <footer style="text-align: center; color: #999999; font-style: italic; font-size: 14px;">¡Gracias por registrarse!</footer>
        <div style="text-align: center; margin-top: 20px;">
            <a href="LoginDependencia.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
        </div>
    </asp:Panel>
     <asp:Panel ID="pnlCalendario" runat="server" Visible="false" CssClass="contenedor">
     <h2 style="color: #333333; font-size: 24px; margin-bottom: 20px; text-align: center;">Estimadas empresas y dependencias</h2>
     <p style="color: #666666; font-size: 16px; margin-bottom: 20px; text-align: justify;">Nos complace informarles que el proceso de Registro para Dependencias comenzará el próximo jueves 23 de mayo</p>
      
     <p style="color: #666666; font-size: 16px; margin-bottom: 20px; text-align: justify;">A partir de esta fecha, todos los interesados podrán acceder a nuestra plataforma oficial para completar su registro. Les solicitamos estar atentos a las instrucciones detalladas que se proporcionarán en dicho portal.</p>
     
     <p style="color: #666666; font-size: 16px; margin-bottom: 20px; text-align: justify;">Agradecemos de antemano su cooperación y estamos a su disposición para cualquier consulta adicional que puedan tener.</p><br />
     <footer style="text-align: center; color: #999999; font-style: italic; font-size: 14px; text-align: center;">¡Atentamente, Departamento de Servicio Social!</footer>
     <div style="text-align: center; margin-top: 20px;">
         <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
     </div>
 </asp:Panel>

    <script src="Validaciones.js"></script>
</asp:Content>

