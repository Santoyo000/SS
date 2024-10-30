<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="PerfilDependencia.aspx.cs" Inherits="Servicio_Social.PerfilDependencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:Panel ID="pnlEditarInfo" runat="server" Visible="true">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Información dependencia</h2>
            </div>
        </div>
        <div class="row">
            <table class="tabla-formulario">
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" AssociatedControlID="txtDependenciaPerfil" CssClass="label-derecha">Dependencia:</asp:Label>
                        <asp:TextBox ID="txtDependenciaPerfil" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </td>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" AssociatedControlID="txtEmailPerfil" CssClass="label-derecha">E-mail:</asp:Label>
                            <asp:TextBox ID="txtEmailPerfil" runat="server" CssClass="textbox-estilo" Enabled="false"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" AssociatedControlID="ddlUnidadPerfil" CssClass="label-derecha">Unidad:</asp:Label>
                            <asp:DropDownList ID="ddlUnidadPerfil" runat="server" CssClass="DropDownList-estilo">
                            </asp:DropDownList>

                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label8" runat="server" AssociatedControlID="ddlOrganimoPerfil" CssClass="label-derecha">Organismo:</asp:Label>
                            <asp:DropDownList ID="ddlOrganimoPerfil" runat="server" CssClass="DropDownList-estilo">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label10" runat="server" AssociatedControlID="txtResponsablePerfil" CssClass="label-derecha">Responsable:</asp:Label>
                            <asp:TextBox ID="txtResponsablePerfil" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label11" runat="server" AssociatedControlID="txtAreaResponsablePerfil" CssClass="label-derecha">Área responsable:</asp:Label>
                            <asp:TextBox ID="txtAreaResponsablePerfil" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label12" runat="server" AssociatedControlID="txtTelefonoPerfil" CssClass="label-derecha">Teléfono:</asp:Label>
                            <asp:TextBox ID="txtTelefonoPerfil" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label13" runat="server" AssociatedControlID="txtDomicilioPerfil" CssClass="label-derecha">Domicilio:</asp:Label>
                            <asp:TextBox ID="txtDomicilioPerfil" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3"></asp:TextBox>
                        </div>
                    </td>
                </tr>

            </table>
        </div>
        <div class="text-center">
            <asp:Label ID="lblResult" runat="server"></asp:Label>
        </div>
        <br />
        <div class="row justify-content-center mt-4">
            <div class="col text-center">
                <asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelDependencia.aspx" />
            </div>
            <div class="col text-center">
                <asp:Button runat="server" ID="btnActualizar" CssClass="btn btn-secondary miBoton" Text="Guardar cambios" OnClick="btnActualizar_Click" />
            </div>
        </div>


        <br />
        <br />
    </asp:Panel>
</asp:Content>
