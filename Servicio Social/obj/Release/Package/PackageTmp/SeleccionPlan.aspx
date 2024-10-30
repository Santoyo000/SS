<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="SeleccionPlan.aspx.cs" Inherits="Servicio_Social.SeleccionPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <div class="container">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Selecciona Plan para continuar:</h2>
            </div>
        </div>
        <div class="container">
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label>Escuela:</label>
                        <asp:DropDownList ID="ddlEscuela" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlEscuela_SelectedIndexChanged" required="required"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label>Plan de Estudios:</label>
                        <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-control" required="required"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div class="containerbtnRegistrar">
                <asp:Button runat="server" ID="bntSeleccionar" CssClass=" miBoton" Text="Seleccionar" OnClick="bntSeleccionar_Click"/>
            </div>
        </div>
    </div>
</asp:Content>
