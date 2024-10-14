<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="ProgramasDependencias.aspx.cs" Inherits="Servicio_Social.ProgramasDependencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .custom-header {
            background-color: #343a40; /* Color de fondo personalizado */
            color: white; /* Color del texto */
        }

        .table td {
            font-size: 14px; /* Tamaño de fuente más pequeño para las celdas de datos */
        }

        .edit-mode input[type=text] {
            max-width: 200px; /* Ajusta el ancho máximo según sea necesario */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <br />
    <div class="container">
        <div style="text-align: center">
            <div class="form-group">
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Programas Registrados</h2>
            </div>
            <div class="">
                <%-- <div class="row mb-3">
            <div class="col-md-3">
                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control" placeholder="Buscar..." />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
            </div>
        </div>--%>
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="display: none;">ID</th>
                            <th>Fecha de Registro</th>
                            <th>Dependencia</th>
                            <th>Correo</th>
                            <th>Nombre del Programa</th>
                            <th>Responsable</th>
                            <th>Estatus</th>
                            <th>Ver Más</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                        <td style="display: none;"><%# Eval("idPrograma") %></td>
                                        <td><%# Eval("FechaRegistro") %></td>
                                        <td><%# Eval("Dependencia") %></td>
                                        <td><%# Eval("Correo") %></td>
                                        <td><%# Eval("NombrePrograma") %></td>
                                        <td><%# Eval("Responsable") %></td>
                                        <td style="display: none;"><%# Eval("kpEstatus_Programa") %></td>
                                        <td><%# Eval("Estatus") %></td>
                                        <td>
                                            <div class="d-flex justify-content-center">
                                                <asp:LinkButton ID="bntEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("idPrograma") %>' OnClick="btnEditar_Click"> <span data-toggle="tooltip" title="Ver Más" ><i class="far fa-edit"></i></span></asp:LinkButton>
                                            </div>
                                        </td>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlEditMode" Visible="false">
                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("idPrograma") + "|" + Eval("Correo") %>' />
                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtFechaRegistro" Text='<%# Eval("FechaRegistro") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtDependencia" Text='<%# Eval("Dependencia") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtCorreo" Text='<%# Eval("Correo") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtNombrePrograma" Text='<%# Eval("NombrePrograma") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtResponsable" Text='<%# Eval("Responsable") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <%# Eval("Estatus") %>
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkUpdate" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>'><i class="fas fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lnkCancel" CommandName="Cancel" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-window-close"></i></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdnAutorizado" runat="server" Value='<%# Eval("kpEstatus_Programa") %>' />
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                        </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

                <asp:Button ID="btnPrevious" runat="server" Text="Anterior" />
                <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                <asp:Button ID="btnNext" runat="server" Text="Siguiente" />
                <div style="text-align: left;">
                    <asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelDependencia.aspx" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
