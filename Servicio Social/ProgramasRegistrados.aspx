<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="ProgramasRegistrados.aspx.cs" Inherits="Servicio_Social.ProgramasRegistrados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function loadModalData(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("ProgramasRegistrados.aspx/llenarDatosModal") %>',
            data: JSON.stringify({ id: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                document.getElementById("modalBody2").innerHTML = response.d;
            },
            failure: function (response) {
                document.getElementById("modalBody2").innerHTML = "Error loading data";
            }
        });
        }
        function loadModalData2(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("ProgramasRegistrados.aspx/llenarDatosModalDet") %>',
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    document.getElementById("modalBody").innerHTML = response.d;
                },
                failure: function (response) {
                    document.getElementById("modalBody").innerHTML = "Error loading data";
                }
            });
        }
    </script>
    <style>
        .custom-header {
            background-color: #343a40; /* Color de fondo personalizado */
            color: white; /* Color del texto */
        }

        .table td {
            font-size: 12px; /* Tamaño de fuente más pequeño para las celdas de datos */
        }

        .table tr {
            font-size: 13px; /* Tamaño de fuente más pequeño para las celdas de datos */
        }

        .edit-mode input[type=text] {
            max-width: 200px; /* Ajusta el ancho máximo según sea necesario */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <br />
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
            <div style="text-align: center">
                <div class="form-group">
                    <h2 class="text-gray-900 mb-4" style="color: #2e5790">Programas Registrados</h2>
                </div>
                <div class="">
                    <div class="container-fluid">
                        <div class="row mb-3">
                            <div class="col-md-3">
                                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control" placeholder="Buscar..." />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th style="display: none;">ID</th>
                                    <th>Fecha Registro</th>
                                    <th>Dependencia</th>
                                    <th>Correo</th>
                                    <th>Nombre del Programa</th>
                                    <th>Responsable</th>
                                    <th>Unidad</th>
                                    <th>Estatus</th>
                                    <%-- <th>Editar</th>--%>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                                <td style="display: none;"><%# Eval("idPrograma") %></td>
                                                <td><%# Eval("FechaRegistro") %></td>
                                                <td><%# Eval("Dependencia") %></td>
                                                <td><%# Eval("Correo") %></td>
                                                <td><%# Eval("NombrePrograma") %></td>
                                                <td><%# Eval("Responsable") %></td>
                                                <td><%# Eval("Unidad") %></td>
                                                <td style="display: none;"><%# Eval("kpEstatus_Programa") %></td>
                                                <td><%# Eval("Estatus") %></td>
                                                <%-- <td>
                                        <div class="d-flex justify-content-center">
                                            <asp:LinkButton ID="bntEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("idPrograma") %>' OnClick="btnEditar_Click"> <span data-toggle="tooltip" title="Ver Más" ><i class="fas fa-edit"></i></span></asp:LinkButton>

                                        </div>
                                    </td>--%>
                                                <td>
                                                    <div class="d-flex justify-content-center">
                                                        <asp:LinkButton ID="btnDetalle2" runat="server" CommandName="cmdDet"  CssClass="btn btn btn btn-sm" style="background-color: #e5cd47; color: white;" data-toggle="modal" data-target="#EditModal" OnClick='<%# "loadModalData2(" + Eval("idPrograma") + ")" %>'><span data-toggle="tooltip" title="Ver Cupos en Programas" ><i class="fas fa-search"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEditarCupo" runat="server" CommandName="cmdDetalle2" CssClass="btn btn btn-sm" style="background-color: #ca63ca; color: white;" CommandArgument='<%# Eval("idPrograma") %>' OnClick="bntEditarDetallePrograma"><span data-toggle="tooltip" title="Editar Cupos del Programa" ><i class="fas fa-solid fa-pen"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnDetalle" runat="server" CommandName="cmdDetalle" CssClass="btn btn btn btn-sm" style="background-color: #e5cd47; color: white;" data-toggle="modal" data-target="#myModal" OnClick='<%# "loadModalData(" + Eval("idPrograma") + ")" %>'><span data-toggle="tooltip" title="Ver detalles Programa" ><i class="fas fa-search"></i></asp:LinkButton>
                                                         <asp:LinkButton ID="bntEdit" runat="server" CssClass="btn btn-primary" CommandName="Edit" CommandArgument='<%# Eval("idPrograma") %>' OnClick="btnEditar_Click"> <span data-toggle="tooltip" title="Editar" ><i class="fas fa-edit"></i></span></asp:LinkButton>
                                                        <asp:LinkButton ID="btnAutorizar" runat="server" CommandName="cmdAutorizar" CommandArgument='<%# Eval("idPrograma") %>' OnClick="btnAutorizar_Click" OnClientClick='return confirm("El registro cambiará de estatus de Autorizado");' CssClass="btn btn-success btn-sm"><span data-toggle="tooltip" title="Autorizar" ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="cmdRechazar" CommandArgument='<%# Eval("idPrograma") %>' OnClick="btnEliminar_Click" OnClientClick='return confirm("El registro cambiará de estatus de NO Autorizado");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="No Autorizar" ><i class="fas fa-window-close"></i></asp:LinkButton>
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
                                            <!-- The Modal -->
                                            <div class="modal" id="EditModal">
                                                <div class="modal-dialog modal-xl">
                                                    <div class="modal-content">
                                                        <!-- Modal Header -->
                                                        <div class="modal-header">
                                                            <h4 class="modal-title">Escuelas Programa</h4>   
                                                            <div class="row">
                                                                </div>
                                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                        </div>

                                                        <!-- Modal body -->
                                                        <div class="modal-body" id="modalBody">
                                                            Loading...
                                                        </div>

                                                        <!-- Modal footer -->
                                                        <div class="modal-footer">
                                                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- The Modal -->
                                            <div class="modal" id="myModal">
                                                <div class="modal-dialog modal-xl">
                                                    <div class="modal-content">
                                                        <!-- Modal Header -->
                                                        <div class="modal-header">
                                                            <h4 class="modal-title">Datos del Programa</h4>
                                                           
                                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                        </div>

                                                        <!-- Modal body -->
                                                        <div class="modal-body" id="modalBody2">
                                                            Loading...
                                                        </div>

                                                        <!-- Modal footer -->
                                                        <div class="modal-footer">
                                                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            </div>
                            </div>
                                        </tr>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <%-- <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar a Excel" OnClick ="btnExportarExcel_Click" CssClass="btn btn-success" />--%>
                        <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="lnkPrev_Click" CssClass="btn btn-primary"/>
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="lnkNext_Click" CssClass="btn btn-primary"/>

                        <div style="text-align: left;">
                            <%--<asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton"/>--%>
                            <asp:LinkButton ID="bntRegresar" runat="server" CssClass="btn btn-primary miBoton" CommandName="Edit" Text="Regresar" OnClick="btnRegresar_Click"> </asp:LinkButton>
                        </div>


                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
