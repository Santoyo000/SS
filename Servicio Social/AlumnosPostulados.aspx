<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="AlumnosPostulados.aspx.cs" Inherits="Servicio_Social.AlumnosPostulados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function loadModalData(id) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("AlumnosPostulados.aspx/llenarDatosModal") %>',
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

        /*.edit-mode input[type=text] {
        max-width: 200px;*/ /* Ajusta el ancho máximo según sea necesario */
        /*}*/
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
            <div class="container-fluid">
                <div style="text-align: center">
                    <div class="form-group">
                        <h2 class="text-gray-900 mb-4" style="color: #2e5790">Alumnos Registrados En Programas</h2>
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
                                    <th>Fecha de Registro</th>
                                    <th>Matrícula</th>
                                    <th>Alumno</th>
                                    <th>Programa</th>
                                    <th>Plan de Estudios</th>
                                    <th>Escuela</th>
                                    <th>Unidad</th>
                                    <th>Cupo</th>
                                    <th>Estatus</th>
                                    <th>Ver Más</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                                <td style="display: none;"><%# Eval("IDDEPENDENICASERVICIO") %></td>
                                                <td><%# Eval("FECHAREGISTRO") %></td>
                                                <td><%# Eval("MATRICULA") %></td>
                                                <td><%# Eval("NOMBRE_COMPLETO") %></td>
                                                <td><%# Eval("PROGRMA") %></td>
                                                <td><%# Eval("PLANEST") %></td>
                                                <td><%# Eval("ESCUELA") %></td>
                                                <td><%# Eval("UNIDAD") %></td>
                                                <td><%# Eval("ICUPO") %></td>
                                                <td><%# Eval("ESTATUS") %></td>
                                                <td style="display: none;"><%# Eval("idProgramaAlumno") %></td>
                                                <td style="display: none;"><%# Eval("IDALUMNO") %></td>
                                                <td style="display: none;"><%# Eval("idEstatus") %></td>
                                                <td>
                                                    <div class="d-flex justify-content-center">
                                                        <asp:LinkButton ID="btnDetalle" Visible="true" runat="server" CommandName="cmdRechazar" CssClass="btn btn-warning" Style="background-color: orange; border-color: orange; color: white;" data-toggle="modal" data-target="#myModal" OnClick='<%# "loadModalData(" + Eval("IDALUMNO") + ")" %>'><span data-toggle="tooltip" title="Ver detalles" ><i class="fas fa-search"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnAutorizar" runat="server" CommandName="cmdAutorizar" CommandArgument='<%# Eval("IDDEPENDENICASERVICIO") + "|" + Eval("idProgramaAlumno")+ "|" + Eval("IDALUMNO")%> ' OnClick="btnAutorizar_Click" OnClientClick='return confirm("El registro cambiará de estatus de Autorizado");' CssClass="btn btn-success btn-sm"><span data-toggle="tooltip" title="Autorizar" ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="cmdRechazar" CommandArgument='<%# Eval("IDDEPENDENICASERVICIO") + "|" + Eval("idProgramaAlumno")+ "|" + Eval("IDALUMNO")%> ' OnClick="btnEliminar_Click" OnClientClick='return confirm("El alumno será eliminado del Programa Actual, ¿desea continuar?");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="No Autorizar" ><i class="fas fa-window-close"></i></asp:LinkButton>
                                                        <%--<asp:LinkButton ID="btnLiberar" Visible ="false" runat="server" CommandName="cmdLiberar" CommandArgument='<%# Eval("ID") %>' OnClick="btnLiberar_Click"  CssClass="btn  btn-primary"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fas fa-solid fa-file-pdf"></i></asp:LinkButton> --%>
                                                    </div>
                                                </td>
                                            </asp:Panel>
                                            <!-- The Modal -->
                                            <div class="modal" id="myModal">
                                                <div class="modal-dialog modal-xl">
                                                    <div class="modal-content">
                                                        <!-- Modal Header -->
                                                        <div class="modal-header">
                                                            <h4 class="modal-title">Detalle Alumno</h4>
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
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>

                        <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="lnkPrev_Click" CssClass="btn btn-primary" />
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="lnkNext_Click" CssClass="btn btn-primary" />
                        <div style="text-align: left;">
                            <% 
                                if (Session["filtros"] != null)
                                {
                                    string usuario = Session["filtros"].ToString().Split('|')[0];
                                    if (usuario == "1")
                                    {
                                        btnAdmon.Visible = true;
                                    }
                                    else if (usuario == "4")
                                    {
                                        btnEncEs.Visible = true;
                                    }
                                }
                            %>
                            <asp:Button ID="btnAdmon" Visible="false" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelAdministrador.aspx" />
                            <asp:Button ID="btnEncEs" Visible="false" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelEncargadoEscuelaspx.aspx" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
