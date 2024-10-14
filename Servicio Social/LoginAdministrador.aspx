<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="LoginAdministrador.aspx.cs" Inherits="Servicio_Social.LoginAdministrador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function togglePasswordVisibility() {
            var passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            var toggleIcon = document.getElementById('togglePasswordIcon');

            if (passwordInput.type === "password") {
                passwordInput.type = "text";
                toggleIcon.className = "fas fa-eye-slash";
            } else {
                passwordInput.type = "password";
                toggleIcon.className = "fas fa-eye";
            }
        }
    </script>
    <style>
        body {
            background-color: #f8f9fa;
            font-family: Arial, sans-serif;
        }

        .login-container {
            max-width: 400px;
            margin: auto;
            margin-top: 30px;
            padding: 20px;
            border: 1px solid #ced4da;
            border-radius: 5px;
            background-color: #fff;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            transition: box-shadow 0.3s ease-in-out;
        }

            .login-container:hover {
                box-shadow: 0 6px 10px rgba(0, 0, 0, 0.2);
            }

        .login-title {
            text-align: center;
            font-size: 20px;
            color: #333;
            margin-bottom: 20px;
        }

        .login-form {
            transition: border-color 0.3s ease-in-out;
        }

            .login-form:focus-within {
                border-color: #007bff;
            }

        .password-toggle {
            cursor: pointer;
        }

        .forgot-password {
            text-align: center;
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div id="overlay">
                <div id="loadingContent">
                    <asp:Image ID="imgWaitIcon" runat="server" ImageUrl="Image/loading.gif" AlternateText="Cargando..." Style="max-width: 300px;" />
                    <div id="loadingText">Por favor, espere...</div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <asp:Panel runat="server" ID="pnlLogin" Visible="true">
                    <div class="container login-container">
                        <div class="text-center mb-4">
                            <img src="Image/UADEC_AZUL2.png" alt="Logo" width="300">
                        </div>
                        <h2 class="login-title">Iniciar sesión: Administrador</h2>
                        <div class="form-group login-form">
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" type="email" placeholder="Correo Institucional" required="required"></asp:TextBox>
                        </div>
                        <div class="form-group login-form">
                            <div class="input-group">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" type="Password" placeholder="Contraseña" required="required"></asp:TextBox>
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
                        <%--            <div id="mensaje" class="mt-3 text-center"></div>--%>
                        <div class="forgot-password">
                            <a href="http://www.uadec.mx/mail" target="_blank">¿Olvidaste tu contraseña?</a>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
