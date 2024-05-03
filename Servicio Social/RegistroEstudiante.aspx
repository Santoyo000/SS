<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistroEstudiante.aspx.cs" Inherits="Servicio_Social.RegistroEstudiante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="panelUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlInicioSesion" runat="server">
                <div class="row justify-content-center">
                    <div class="col-sm-2">
                        <asp:Label ID="lblCorreo" runat="server" Text="Correo universitario" class="control-label"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox ID="txtCorreo" runat="server" class="form-control" placeholder="Sin @uadec.edu.mx" required="true"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="uadec" runat="server" Text="@uadec.edu.mx" class="control-label"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row justify-content-center">
                    <div class="col-sm-2">
                        <asp:Label ID="lblPassword" runat="server" Text="Contraseña: " class="control-label"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox ID="txtPassword" runat="server" class="form-control" type="password" required="true"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label1" runat="server" Text="" class="control-label"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row justify-content-center">
                    <div class="col-sm-2">
                        <asp:Button ID="btnIngresar" runat="server" class="btn btn-primary btn-block waves-effect waves-light" Text="Ingresar" OnClick="btnIngresar_Click" />
                    </div>
                </div>
                <br />
                <br />
                <div class="row justify-content-center">
                    <div class="col-md-4">
                        <asp:LinkButton ID="lbtRegistrar" runat="server" class="buttonLink" OnClick="btnRegistrar_Click">¿No estás registrado? Registrate aquí</asp:LinkButton>

                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlRegistro" runat="server">
                <asp:LinkButton ID="lbtIniciarSesion" runat="server" class="buttonLink" OnClick="btnInicio_Click">¿Ya te registraste? Inicia Sesion</asp:LinkButton>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
