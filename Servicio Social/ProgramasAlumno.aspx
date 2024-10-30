<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="ProgramasAlumno.aspx.cs" Inherits="Servicio_Social.ProgramasAlumno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function loadModalData(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("ProgramasAlumno.aspx/llenarDatosModal") %>',
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

        function loadModalDataProgram(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("ProgramasAlumno.aspx/loadModalDataProgram") %>',
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    document.getElementById("modalBodyData").innerHTML = response.d;
                },
                failure: function (response) {
                    document.getElementById("modalBodyData").innerHTML = "Error loading data";
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

        .result-table {
            width: 100%;
            border-collapse: collapse;
        }

            .result-table thead {
                background-color: #f2f2f2;
            }

            .result-table th, .result-table td {
                border: 1px solid #ccc;
                padding: 8px;
                text-align: left;
            }

            .result-table th {
                background-color: #e9ecef;
                font-weight: bold;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <div class="container">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Programas seleccionados:</h2>
            </div>
        </div>
        <br />
        <div class="container">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th style="display: none;">ID</th>
                        <th>Dependencia</th>
                        <th>Programa</th>
                        <th>Operación</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RepeaterProgramas" runat="server" OnItemDataBound="RepeaterProgramas_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                    <td style="display: none;"><%# Eval("idProgramaAlumno") %></td>
                                    <td><%# Eval("Dependencia") %></td>
                                    <td><%# Eval("sNombre_Programa") %></td>
                                    <td style="display: none;"><%# Eval("idDetallePrograma") %></td>
                                    <td>
                                        <div class="d-flex justify-content-center">
                                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" data-toggle="modal" data-target="#myModalData" OnClick='<%# "loadModalDataProgram(" + Eval("idDetallePrograma") + ")" %>'><span data-toggle="tooltip" title="Ver detalles" ><i class="fas fa-search"></i></asp:LinkButton>
                                             <asp:LinkButton ID="btnLiberar" runat="server" CommandName="cmLiberar" CommandArgument='<%# Eval("idProgramaAlumno") %>' CssClass="btn btn-primary btn-sm" OnClick="btnLiberar_Click"><span data-toggle="tooltip" title="Informe Servicio"><i class="fas fa-regular fa-file"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnDetalle" runat="server" CssClass="btn btn-primary" data-toggle="modal" data-target="#myModal" OnClick='<%# "loadModalData(" + Eval("idProgramaAlumno") + ")" %>'><span data-toggle="tooltip" title="Ver historial" ><i class="fas fa-history"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnAnular" runat="server" CommandName="cmAnular" CommandArgument='<%# Eval("idProgramaAlumno") %>' OnClientClick='return confirm("Se liberará el lugar ¿Desea continuar?");' CssClass="btn btn-danger btn-sm" OnClick="btnAnular_Click"><span data-toggle="tooltip" title="Liberar lugar"><i class="fas fa-window-close"></i></asp:LinkButton>
                                        </div>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div style="text-align: center">
                <asp:Button ID="btnPrevious" runat="server" Text="Anterior" CssClass="btn btn-primary"/>
                <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                <asp:Button ID="btnNext" runat="server" Text="Siguiente" CssClass="btn btn-primary"/>
                <br />
                <br />
            </div>
        </div>
        <!-- The Modal - Historial de Programa -->
        <div class="modal" id="myModal">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title">Historial de Programa</h4>
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
        <!-- The Modal - Datos del Programa  -->
        <div class="modal" id="myModalData">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title">Datos de la Dependencia</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body" id="modalBodyData">
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
</asp:Content>
