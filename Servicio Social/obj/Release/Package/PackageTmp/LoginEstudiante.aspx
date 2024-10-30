<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="LoginEstudiante.aspx.cs" Inherits="Servicio_Social.RegistroEstudiante" %>

<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="head">
    <style>
        body {
            background-color: #f8f9fa;
            /*font-family: Arial, sans-serif;*/
        }

        em {
            font-size: 11px;
        }
    </style>
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
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="titulo">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
  <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
    <ProgressTemplate>
        <div id="overlay">
            <div id="loadingContent">
                <asp:Image ID="imgWaitIcon" runat="server" ImageUrl="Image/loading.gif" AlternateText="Cargando..." style="max-width: 300px;"/>
                <div id="loadingText">Por favor, espere...</div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlLogin" Visible="true">
                <div class="login-container">
                    <div class="text-center mb-4">
                       <img src="Image/UADEC_AZUL2.png" alt="Logo" width="300">
                    </div>


                    <h2 class="login-title">Iniciar sesión: Alumno</h2>
                    <i style="font-style: italic;">Alumnos escuela oficial ingresan con correo institucional registrado.</i><br />
                    <i style="font-style: italic;">Alumnos escuela incorporada ingresan con correo personal.</i><br />
                    <br />
                    <div class="form-group login-form">
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Correo" required="required"></asp:TextBox>
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
                    <%--                <div id="mensaje" class="mt-3 text-center"></div>--%>
                    <div class="register">
                        <p>¿Eres nuevo?&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lkbRegistro" Text="Registrate aquí." OnClick="lkbRegistro_Click"></asp:LinkButton></p>

                    </div>
                </div>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
