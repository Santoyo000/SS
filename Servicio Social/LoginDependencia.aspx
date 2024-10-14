<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="LoginDependencia.aspx.cs" Inherits="Servicio_Social.Dependencias1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #f8f9fa;
            /*font-family: Arial, sans-serif;*/
        }
    </style>
    <script>
        function togglePasswordVisibility() {
            var passwordInput = document.getElementById('<%= inPassword.ClientID %>');
            var toggleIcon = document.getElementById('togglePasswordIcon');

            if (passwordInput.type === "password") {
                passwordInput.type = "text";
                toggleIcon.className = "fas fa-eye-slash";
            } else {
                passwordInput.type = "password";
                toggleIcon.className = "fas fa-eye";
            }
        }

        <%--function validarFormatoCorreo() {
            var regexValidator = document.getElementById("<%= regexEmail.ClientID %>");
             var btnSubmit = document.getElementById("<%= btnIniciar.ClientID %>");

             // Verificar si la validación del correo electrónico es válida
             if (regexValidator.style.display === "none" || regexValidator.innerHTML === "") {
                 // Si la validación es válida, habilitar el botón
                 btnSubmit.disabled = false;
             } else {
                 // Si la validación es inválida, inhabilitar el botón
                 btnSubmit.disabled = true;
             }
         }--%>


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
    <div class="container">
        <asp:Panel runat="server" ID="pnlLogin" Visible="true">
            <div class="login-container">
                <div class="text-center mb-4">
                  <img src="Image/UADEC_AZUL2.png" alt="Logo" width="300">
                </div>
                <h2 class="login-title">Iniciar sesión: Dependencia</h2>
                <div class="form-group login-form">
                    <asp:TextBox ID="usuario" runat="server" CssClass="form-control" type="email" placeholder="Correo electrónico" required="required"></asp:TextBox>
                </div>
                <div class="form-group login-form">
                    <div class="input-group">
                        <asp:TextBox ID="inPassword" runat="server" CssClass="form-control" type="Password" placeholder="Contraseña" required="required"></asp:TextBox>
                        <div class="input-group-append">
                            <span class="btn btn-outline-secondary password-toggle" onclick="togglePasswordVisibility()">
                                <i id="togglePasswordIcon" class="fas fa-eye"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="text-center">
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <br />
                <div class="text-center">
                    <asp:Button ID="btnIniciar" runat="server" Text="Continuar" CssClass="btn btn-primary" OnClick="btnIngresar_Click" />
                </div>
                <div class="register">
                    <p>¿Eres nuevo?&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lkbRegistro" OnClick="lkbRegistro_Click" Text="Registrate aquí."></asp:LinkButton></p>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCalendario" runat="server" Visible="false" CssClass="contenedor">
            <br />
            <br />
            <h2 style="color: #333333; font-size: 24px; margin-bottom: 20px; text-align: center;">Estimadas empresas y dependencias</h2>
            <p style="color: #666666; font-size: 16px; margin-bottom: 20px; text-align: justify;">Nos complace informarles que el proceso de Registro para los Programas comenzará el próximo miércoles 29 de mayo</p>

            <p style="color: #666666; font-size: 16px; margin-bottom: 20px; text-align: justify;">A partir de esta fecha, todos los interesados podrán acceder a nuestra plataforma oficial para completar el registro de Programas. Les solicitamos estar atentos a las instrucciones detalladas que se proporcionarán en dicho portal.</p>

            <p style="color: #666666; font-size: 16px; margin-bottom: 20px; text-align: justify;">Agradecemos de antemano su cooperación y estamos a su disposición para cualquier consulta adicional que puedan tener.</p>
            <br />
            <footer style="text-align: center; color: #999999; font-style: italic; font-size: 14px; text-align: center;">¡Atentamente, Departamento de Servicio Social!</footer>
            <div style="text-align: center; margin-top: 20px;">
                <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
