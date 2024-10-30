<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="PerfilEstudiante.aspx.cs" Inherits="Servicio_Social.PerfilEstudiante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:Panel ID="pnlEditar" runat="server">
        <div class="container">
            <div style="text-align: center">
                <div class="form-group">
                    <br />
                    <h2 class="text-gray-900 mb-4" style="color: #2e5790">Información alumno</h2>
                </div>
            </div>
            <div class="container">
                <div class="form-group">
                    <br />
                    <h5>Datos Escolares</h5>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Unidad:</label>
                            <asp:TextBox ID="txtUnidad" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Escuela:</label>
                            <asp:TextBox ID="txtEscuela" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Carrera:</label>
                            <asp:TextBox ID="txtPlanEstudios" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Matricula:</label>
                            <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Nombre:</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Apellido Paterno:</label>
                            <asp:TextBox ID="txtApePat" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Apellido Materno:</label>
                            <asp:TextBox ID="txtApeMat" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <br />
                    <h5>Datos Personales</h5>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Sexo:</label>
                            <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control" ReadOnly="true">
                                <asp:ListItem Text="Masculino" Value="1" />
                                <asp:ListItem Text="Femenino" Value="2" />

                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Correo:</label>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Telefono:</label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Domicilio:</label>
                            <asp:TextBox ID="txtDomicilio" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Colonia:</label>
                            <asp:TextBox ID="txtColonia" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Ciudad:</label>
                            <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Estado:</label>
                                <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Codigo Postal:</label>
                                <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
