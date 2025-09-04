<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="SeleccionarPrograma.aspx.cs" Inherits="Servicio_Social.SeleccionarPrograma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
      <%--  function loadModalData(id) {
           $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("SeleccionarPrograma.aspx/llenarDatosModal") %>',
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    document.getElementById("modalBody<a href="Reportes / ConstanciaFinal.rpt">Reportes/ConstanciaFinal.rpt</a>").innerHTML = response.d;
                },
                failure: function (response) {
                    document.getElementById("modalBody").innerHTML = "Error loading data";
                }
            });
        }--%>


        function loadModalData(id) {
            $("#modalBody").html("Cargando..."); // Mensaje de carga temporal

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("SeleccionarPrograma.aspx/llenarDatosModal") %>',
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#modalBody").html(response.d); // Inserta el HTML recibido
                    $("#myModal").modal("show"); // Asegura que el modal se muestra
                },
                error: function () {
                    $("#modalBody").html("Error al cargar los datos."); // Mensaje de error en el modal
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

        /*AQUI COMIENZA CODIGO NUEVO*/

        .container-tabla {
            margin-top: 50px; /* Para asegurarte de que no se sobreponga al menú */
            padding: 20px;
            max-width: 100%; /* Para asegurar que ocupe todo el ancho disponible */
        }
        /* Tabla */
        .table {
            width: 100%; /* Para que la tabla ocupe todo el espacio disponible */
            margin-bottom: 30px; /* Espacio debajo de la tabla */
            border-collapse: collapse;
            background-color: #fff; /* Fondo blanco para la tabla */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* Sombra para efecto de elevación */
        }
            /* Bordes de la tabla */
            .table th,
            .table td {
                padding: 12px 15px; /* Espaciado interno */
                border: 1px solid #ddd; /* Bordes ligeros */
                text-align: left; /* Texto alineado a la izquierda */
                font-size: 14px; /* Tamaño de fuente */
            }
            /* Cabecera de la tabla */
            .table thead th {
                background-color: #516e96; /*    Color de fondo de la cabecera */
                color: #fff; /* Color de texto blanco */
                font-weight: 400; /* Negrita */
                text-transform: uppercase; /* Texto en mayúsculas */
                text-align: center;
            }
            /* Filas alternas para mayor legibilidad */
            .table tbody tr:nth-child(even) {
                background-color: #f9f9f9;
            }
            /* Resaltar filas al pasar el ratón */
            .table tbody tr:hover {
                background-color: #f1f1f1;
            }

        .form-control {
            /* padding: 10px;*/
            border-radius: 4px;
            border: 1px solid #ddd;
            margin-right: 10px; /* Espacio a la derecha */
            width: 100%;
        }


            .form-control:focus {
                border-color: #4086b1;
                outline: none;
                box-shadow: 0 0 5px rgba(64, 134, 177, 0.5);
            }

        /* Botón de búsqueda */
        .btn-primary {
            /* background-color: #f7d05a;*/
            border-color: #f7d05a;
            color: white;
            padding: 10px 20px;
            font-size: 14px;
            border-radius: 4px;
            transition: background-color 0.3s ease;
        }

            .btn-primary:hover {
                background-color: #f1c40f;
                border-color: #f1c40f;
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
                    <div class="text-center mb-2">
                        Página
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        de
                        <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
                    </div>

                    <div class="d-flex justify-content-center">
                        <div class="pagination d-flex align-items-center">
                            <asp:Button ID="btnPrevious" runat="server" Text="&#9665;" OnClick="btnPrevious_Click" CssClass="btn btn-light me-2" />

                            <asp:Repeater ID="rptPagination" runat="server" OnItemCommand="rptPagination_ItemCommand">
                                <ItemTemplate>
                                    <asp:Button runat="server" Text='<%# Eval("PageNumber") %>'
                                        CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="PageChange"
                                        CssClass='<%# Convert.ToInt32(Eval("PageIndex")) == CurrentPage ? "btn btn-dark me-1" : "btn btn-light me-1" %>' />
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Button ID="btnNext" runat="server" Text="&#9655;" OnClick="btnNext_Click" CssClass="btn btn-light ms-2" />
                        </div>
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
