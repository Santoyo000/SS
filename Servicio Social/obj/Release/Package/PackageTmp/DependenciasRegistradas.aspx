<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="DependenciasRegistradas.aspx.cs" Inherits="Servicio_Social.DependenciasRegistradas1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function loadModalData(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("DependenciasRegistradas.aspx/llenarDatosModal") %>',
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

            <asp:Panel ID="pnlDependencias" runat="server" Visible="true">
                <div class="container-fluid">
                    <div style="text-align: center">
                        <div class="form-group">
                            <br />
                            <h2 class="text-gray-900 mb-4" style="color: #2e5790">Dependencias Registradas</h2>
                        </div>
                        <div class="">
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
                                        <%--<th>Responsable</th>--%>
                                        <%--<th>Área Responsable</th>--%>
                                        <th>Unidad</th>
                                        <%-- <th>Organismo</th>--%>
                                        <th>Telefono</th>
                                        <th>Correo</th>
                                        <%-- <th>Domicilio</th>--%>
                                        <th>Estatus</th>
                                        <th>Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                                    <td style="display: none;"><%# Eval("idDependenicaServicio") %></td>
                                                    <td><%# Eval("dFechaRegistroDep") %></td>
                                                    <td><%# Eval("sDescripcion") %></td>
                                                    <%--                                        <td><%# Eval("sResponsable") %></td>--%>
                                                    <%--                                        <td><%# Eval("sAreaResponsable") %></td>--%>
                                                    <td><%# Eval("sUnidad") %></td>
                                                    <%--                                        <td><%# Eval("sOrganismo") %></td>--%>
                                                    <td><%# Eval("sTelefono") %></td>
                                                    <td><%# Eval("sCorreo") %></td>
                                                    <%--                                        <td><%# Eval("sDomicilio") %></td>--%>
                                                    <td style="display: none;"><%# Eval("bAutorizado") %></td>
                                                    <td><%# Eval("Estatus") %></td>
                                                    <td>
                                                        <div class="d-flex justify-content-center">

                                                            <asp:LinkButton ID="btnDetalle" runat="server" CommandName="cmdRechazar" CssClass="btn btn-warning" data-toggle="modal" data-target="#myModal" OnClick='<%# "loadModalData(" + Eval("idDependenicaServicio") + ")" %>'><span data-toggle="tooltip" title="Ver detalles" ><i class="fas fa-search"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm" CommandArgument='<%# Eval("idDependenicaServicio") %>' OnClick="btnEdit_Click"><span data-toggle="tooltip" title="Editar" ><i class="fas fa-edit"></i></span></asp:LinkButton>
                                                            <asp:LinkButton ID="btnAutorizar" runat="server" CommandName="cmdAutorizar" CommandArgument='<%# Eval("idDependenicaServicio") %>' OnClick="btnAutorizar_Click" OnClientClick='return confirm("El registro cambiará de estatus de Autorizado");' CssClass="btn btn-success btn-sm"><span data-toggle="tooltip" title="Autorizar" ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="cmdRechazar" CommandArgument='<%# Eval("idDependenicaServicio") %>' OnClick="btnEliminar_Click" OnClientClick='return confirm("El registro cambiará de estatus de NO Autorizado");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="No Autorizar" ><i class="fas fa-window-close"></i></asp:LinkButton>

                                                        </div>
                                                    </td>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlEditMode" Visible="false">
                                                    <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("idDependenicaServicio") + "|" + Eval("sCorreo") %>' />
                                                    <td>
                                                        <%# Eval("dFechaRegistroDep") %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtDescripcion" Text='<%# Eval("sDescripcion") %>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtResponsable" Text='<%# Eval("sResponsable") %>'></asp:TextBox>
                                                    </td>

                                                    <td>
                                                        <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtAreaResponsable" Text='<%# Eval("sAreaResponsable") %>'></asp:TextBox>
                                                    </td>

                                                    <td>
                                                        <asp:DropDownList ID="ddlUnidad" CssClass="form-control" runat="server" Style="font-size: 0.9em !important;" required="required"></asp:DropDownList>
                                                    </td>

                                                    <td>
                                                        <asp:DropDownList ID="ddlOrganismo" CssClass="form-control" runat="server" Style="font-size: 0.9em !important;" required="required">
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>
                                                        <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtTelefono" Text='<%# Eval("sTelefono") %>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <%# Eval("sCorreo") %>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtDomicilio" Text='<%# Eval("sDomicilio") %>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <%# Eval("Estatus") %>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnkUpdate" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>'><i class="fas fa-save"></i></asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lnkCancel" CommandName="Cancel" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-window-close"></i></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hdnAutorizado" runat="server" Value='<%# Eval("bAutorizado") %>' />
                                                    </td>
                                                </asp:Panel>
                                                <!-- The Modal -->
                                                <div class="modal" id="myModal">
                                                    <div class="modal-dialog modal-xl">
                                                        <div class="modal-content">
                                                            <!-- Modal Header -->
                                                            <div class="modal-header">
                                                                <h4 class="modal-title">Datos de la Dependencia</h4>
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
                                                </div>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                            <%-- <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar a Excel" OnClick="btnExportarExcel_Click" CssClass="btn btn-success" />--%>
                            <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="lnkPrev_Click" CssClass="btn btn-primary"/>
                            <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                            <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="lnkNext_Click" CssClass="btn btn-primary"/>
                            <div style="text-align: left;">
                                <asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" OnClick="btnRegresar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal -->
                <div class="modal" id="editModal">
                    <div class="modal-dialog modal-xl">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">Editar Dependencia</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <asp:HiddenField ID="hfDependenciaId" runat="server" />
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="txtNombreDependencia">Dependencia:</label>
                                            <asp:TextBox ID="txtNombreDependencia" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtAreaResponsable">Área Responsable:</label>
                                            <asp:TextBox ID="txtAreaResponsable" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="txtResponsable">Responsable:</label>
                                            <asp:TextBox ID="txtResponsable" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="ddlUnidadModal">Unidad:</label>
                                            <asp:DropDownList ID="ddlUnidadModal" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="ddlOrganismoModal">Organismo:</label>
                                            <asp:DropDownList ID="ddlOrganismoModal" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="txtTelefono">Teléfono:</label>
                                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label for="txtDomicilio">Domicilio:</label>
                                            <asp:TextBox ID="txtDomicilio" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnUpdate" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
