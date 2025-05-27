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
      <!-- Datepicker -->
<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script>
    $(document).ready(function () {
        $(".datepicker").datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true
        });
    });
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



         .btn-excel {
             display: inline-flex;
             align-items: center;
             justify-content: center;
             background-color: #52be80; 
             color: white;
             padding: 10px 35px;
             border: none;
             border-radius: 5px;
             font-size: 14px;
             cursor: pointer;
             float: right;
             transition: background-color 0.3s ease;
         }

         .btn-excel:hover {
             background-color: #1d633c;
         }

         .btn-excel i {
             margin-right: 10px; /* Espacio entre el ícono y el texto */
         }

         /* Ajuste para el icono de Excel */
         .btn-excel .fa-file-excel {
             font-size: 18px;
             color: white;
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
                        <div class="row mb-2">
                         <div class="col-md-6 d-flex align-items-center">
                             <label for="txtDependencia" class="me-2" style="width: 150px;">Nombre Dependencia:</label>
                             <asp:TextBox ID="txtDependencia" runat="server" CssClass="form-control" placeholder="Nombre Dependencia..." />
                         </div>
                         <div class="col-md-6 d-flex align-items-center">
                            <label for="txtCorreo" class="me-2" style="width: 150px;">Correo Dependencia:</label>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Correo Dependencia..." />
                        </div>
                       <div class="col-md-6 d-flex align-items-center">
                            <label for="txtPrograma" class="me-2" style="width: 150px;">Nombre Programa:</label>
                            <asp:TextBox ID="txtPrograma" runat="server" CssClass="form-control" placeholder="Nombre Programa..." />
                        </div>
                         <div class="col-md-6 d-flex align-items-center">
                              <label for="txtResponsable" class="me-2" style="width: 150px;">Responsable del Programa:</label>
                              <asp:TextBox ID="txtResponsable" runat="server" CssClass="form-control" placeholder="Responsable del Programa..." />
                         </div>
                        <div class="col-md-6 d-flex align-items-center">
                         <label for="txtUnidad" class="me-2" style="width: 150px;">Unidad:</label>
                           <asp:DropDownList ID="DDLUnidad" runat="server" CssClass="form-control" placeholder="Seleccione una Unidad..."  >  
                           </asp:DropDownList>
                       </div>
                              <div class="col-md-6 d-flex align-items-center">
                                <label for="txtFecha" class="me-2" style="width: 150px;">Fecha:</label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control datepicker" placeholder="dd/mm/aa" />
                            </div>
                         <div class="col-md-6 d-flex align-items-center">
                            <label for="txtEstatus" class="me-2" style="width: 150px;">Estatus:</label>
                           <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="form-control" AutoPostBack="true" > 
                           </asp:DropDownList>          
                        </div>

                        <div class="col-md-6 d-flex align-items-center">
                            <label for="txtéropdp" class="me-2" style="width: 150px;">Periodo:</label>
                           <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control" AutoPostBack="true" > 
                           </asp:DropDownList>          
                        </div>
                           <%-- <div class="col-md-3">
                                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control" placeholder="Buscar..." />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>--%>
                        <div class="col-md-12 d-flex justify-content-end align-items-center mt-3">
                            <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-excel me-3" OnClick="btnExportarExcel_Click" />
                            <asp:Button ID="btnBorrar" runat="server" Text="Limpiar Filtros" CssClass="btn btn-secondary me-3" OnClick="btnBorrar_Click" />
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
                       <%-- <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="lnkPrev_Click" CssClass="btn btn-primary"/>
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="lnkNext_Click" CssClass="btn btn-primary"/>

                        <div style="text-align: left;">
                            <%--<asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton"/>--%>
                          <%--  <asp:LinkButton ID="bntRegresar" runat="server" CssClass="btn btn-primary miBoton" CommandName="Edit" Text="Regresar" OnClick="btnRegresar_Click"> </asp:LinkButton>
                        </div>--%>
                    <div class="text-center mb-2">
                             Página <asp:Label ID="lblPageNumber" runat="server"></asp:Label> 
                             de <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
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
                </div>
            </div>
        </ContentTemplate>
                 <Triggers>
    <asp:PostBackTrigger ControlID="btnExportarExcel" />
</Triggers>
    </asp:UpdatePanel>
</asp:Content>
