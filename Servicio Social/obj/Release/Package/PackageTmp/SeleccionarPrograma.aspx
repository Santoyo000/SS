<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="SeleccionarPrograma.aspx.cs" Inherits="Servicio_Social.SeleccionarPrograma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function loadModalData(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("SeleccionarPrograma.aspx/llenarDatosModal") %>',
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
        .table td {
            font-size: 13px; /* Tamaño de fuente más pequeño para las celdas de datos */
        }

        .small-text {
            font-size: 13px; /* Tamaño de letra */
            line-height: 1.5; /* Altura de línea */
            color: #333; /* Color del texto */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
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
              <asp:Panel ID="PanelCerrado" runat="server" Visible="false">
                  <div style="text-align: center">
                <div class="form-group">
                    <br />
                    <h3 id="mensajeCierre" class="text-gray-900 mb-4" style="color: #2e5790">
                     <asp:Label ID="lblMensajeProgramas" runat="server"></asp:Label>
                    </h3>
                     <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
                </div>
            </div>
              </asp:Panel>
            <asp:Panel ID="PanelProgramas" runat="server" Visible="true">
                <div style="text-align: center">
                    <div class="form-group">
                        <br />
                        <h2 class="text-gray-900 mb-4" style="color: #2e5790">Programas registrados</h2>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:TextBox ID="txtDependencias" runat="server" CssClass="form-control" placeholder="Dependencia, Programa, Plan..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="container">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="display: none;">ID</th>
                                <%--<th>Fecha Autorizada</th>--%>
                                <th>Dependencia</th>
                                <th>Programa</th>
                                <th>Escuela Dirigida</th>
                                <th>Plan Dirigido</th>
                                <th>Cupos</th>
                                <th>Disponibles</th>
                                <th>Operación</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="RepeaterProgramas" runat="server" OnItemDataBound="RepeaterProgramas_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                            <td style="display: none;"><%# Eval("idDetallePrograma") %></td>
                                            <%--<td><%# Eval("dFechaRegistroP") %></td>--%>
                                            <td><%# Eval("Dependencia") %></td>
                                            <td><%# Eval("sNombre_Programa") %></td>
                                            <td><%# Eval("Escuela") %></td>
                                            <td><%# Eval("Planes") %></td>
                                            <td><%# Eval("iCupo") %></td>
                                            <td><%# Eval("CuposDisponibles") %></td>
                                            <td style="display: none;"><%# Eval("Inscrito") %></td>
                                            <td>
                                                <div class="d-flex justify-content-center">
                                                    <asp:LinkButton ID="btnDetalle" runat="server" CssClass="btn btn-primary" data-toggle="modal" data-target="#myModal" OnClick='<%# "loadModalData(" + Eval("idDetallePrograma") + ")" %>'><span data-toggle="tooltip" title="Ver detalles" ><i class="fas fa-search"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="btnSeleccionar" runat="server" CommandName="cmdSeleccionar" CommandArgument='<%# Eval("idDetallePrograma") + "|" + Eval("Inscrito")%> ' OnClientClick='return confirm("¿Esta seguro de registrarse en este programa?");' CssClass="btn btn-success btn-sm" OnClick="btnSeleccionar_Click"><span data-toggle="tooltip" title="Aplicar" ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnAnular" runat="server" CommandName="cmAnular" CommandArgument='<%# Eval("idDetallePrograma") %>' OnClientClick='return confirm("El registro cambiará de estatus de NO Autorizado");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="Liberar lugar"><i class="fas fa-window-close"></i></asp:LinkButton>

                                                </div>
                                            </td>
                                        </asp:Panel>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div style="text-align: center">
                        <asp:Button ID="btnPrevious" runat="server" Text="Anterior" CssClass="btn btn-primary" OnClick="btnPrevious_Click" />
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" Text="Siguiente" CssClass="btn btn-primary" OnClick="btnNext_Click" />
                        <br />
                        <br />
                    </div>
                </div>
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
            </asp:Panel>
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
