<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="AlumnosRegistrados.aspx.cs" Inherits="Servicio_Social.AlumnosRegistrados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <div class="container-fluid">
                <div style="text-align: center">
                    <div class="form-group">
                        <h2 class="text-gray-900 mb-4" style="color: #2e5790">Alumnos Registrados</h2>
                    </div>
                    <div class="">
                        <div class="row mb-2">
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtBuscarUsuario" class="me-2" style="width: 150px;">Matrícula:</label>
                                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" placeholder="Matrícula..." />
                            </div>
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtNombre" class="me-2" style="width: 150px;">Nombre Alumno:</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre..." />
                            </div>
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtCorreo" class="me-2" style="width: 150px;">Correo electrónico:</label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Correo..." />
                            </div>
                             <div class="col-md-6 d-flex align-items-center">
                                <label for="txtEstatus" class="me-2" style="width: 150px;">Estatus:</label>
                               <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="form-control" AutoPostBack="true" > 
                               </asp:DropDownList>          
                            </div>
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtUnidad" class="me-2" style="width: 150px;">Unidad:</label>
                                  <asp:DropDownList ID="DDLUnidad" runat="server" CssClass="form-control" placeholder="Seleccione una Unidad..."  OnSelectedIndexChanged="DDLUnidad_SelectedIndexChanged " AutoPostBack="true">  
                                  </asp:DropDownList>
                            </div>
                             <div class="col-md-6 d-flex align-items-center">
                                 <label for="txtPlan" class="me-2" style="width: 150px;">Nivel:</label>
                                   <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-control" placeholder="Seleccione un Nivel..." OnSelectedIndexChanged="DDLPlan_SelectedIndexChanged " AutoPostBack="true">  
                                   </asp:DropDownList>
                             </div>
                             <div class="col-md-6 d-flex align-items-center">
                                 <label for="txtPlan" class="me-2" style="width: 150px;">Plan de estudios:</label>
                                   <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-control" placeholder="Seleccione un Plan de Estudios..." DataTextField="Descripcion" DataValueField="idPlanEstudio" OnSelectedIndexChanged="DDLEscuela_SelectedIndexChanged" AutoPostBack="true">  
                                   </asp:DropDownList>
                             </div>
                             <div class="col-md-6 d-flex align-items-center">
                                 <label for="txtEscuela" class="me-2" style="width: 150px;">Escuela:</label>
                                   <asp:DropDownList ID="ddlEscuela" runat="server" CssClass="form-control" placeholder="Seleccione una Escuela..." >  
                                </asp:DropDownList>
                             </div>
                             <div class="col-md-6 d-flex align-items-center">
                                <label for="txtPeriodo" class="me-2" style="width: 150px;">Periodo:</label>
                                  <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control" placeholder="Seleccione un Periodo..." >
                               </asp:DropDownList>
                            </div>
                             <div class="col-md-6 d-flex justify-content-end">
                                 <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-excel me-3" OnClick="btnExportarExcel_Click" />
                                 <asp:Button ID="btnBorrar" runat="server" Text="Limpiar Filtros" CssClass="btn btn-secondary me-2" OnClick="btnBorrar_Click" />
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>
                          </div>
                    <%--    <div class="row mb-3">
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
                                    <th>Matrícula</th>
                                    <th>Alumno</th>
                                    <th>Correo</th>
                                    <th>Plan de Estudios</th>
                                    <th>Escuela</th>
                                    <th>Unidad</th>
                                    <th>Estatus</th>
                                    <th>Ver Más</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                                <td style="display: none;"><%# Eval("ID") %></td>
                                                <td><%# Eval("dFechaRegistro") %></td>
                                                <td><%# Eval("Matricula") %></td>
                                                <td><%# Eval("Alumno") %></td>
                                                <td><%# Eval("Correo") %></td>
                                                <td><%# Eval("PlanEstudio") %></td>
                                                <td><%# Eval("Escuela") %></td>
                                                <td><%# Eval("UNIDAD") %></td>
                                                <%-- <td style="display: none;"><%# Eval("kpEstatus_Programa") %></td>--%>
                                                <td><%# Eval("EstadoAutorizacion") %></td>
                                                <td style="display: none;"><%# Eval("idEstatus") %></td>
                                                <td>
                                                    <div class="d-flex justify-content-center">
                                                        <asp:LinkButton ID="btnDetalle" Visible="false" runat="server" CommandName="cmdRechazar" CssClass="btn btn-warning" data-toggle="modal" data-target="#myModal"><span data-toggle="tooltip" title="Ver detalles" ><i class="fas fa-search"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnAutorizar" runat="server" CommandName="cmdAutorizar" CommandArgument='<%# Eval("ID") %>' OnClick="btnAutorizar_Click" OnClientClick='return confirm("El registro cambiará de estatus de Autorizado");' CssClass="btn btn-success btn-sm"><span data-toggle="tooltip" title="Autorizar" ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                        <asp:LinkButton ID="btnRechazar" runat="server" CommandName="cmdRechazar" CommandArgument='<%# Eval("ID") %>' OnClick="btnRechazar_Click" OnClientClick='return confirm("El registro cambiará de estatus de NO Autorizado");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="No Autorizar" ><i class="fas fa-window-close"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="cmdEliminar" CommandArgument='<%# Eval("ID") %>' OnClick="btnEliminar_Click" OnClientClick='return confirm("El registro se eliminará, ¿desea continuar?");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="Eliminar alumno" ><i class="fa fa-trash"></i></asp:LinkButton>
                                                       <%-- <asp:LinkButton ID="btnLiberar" Visible="false" runat="server" CommandName="cmdLiberar" CommandArgument='<%# Eval("ID") %>' OnClick="btnLiberar_Click" CssClass="btn  btn-primary"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fas fa-solid fa-file-pdf"></i></asp:LinkButton>--%>
                                                    </div>
                                                </td>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlEditMode" Visible="false">
                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") + "|" + Eval("Correo") %>' />
                                                <td>
                                                    <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtFechaRegistro" Text='<%# Eval("dFechaRegistro") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtMatricula" Text='<%# Eval("Matricula") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtAlumno" Text='<%# Eval("Alumno") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtCorreo" Text='<%# Eval("Correo") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtPlanEstudio" Text='<%# Eval("PlanEstudio") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtEscuela" Text='<%# Eval("Escuela") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkUpdate" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>'><i class="fas fa-save"></i></asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="lnkCancel" CommandName="Cancel" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-window-close"></i></asp:LinkButton>
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

                      <%--  <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="lnkPrev_Click" CssClass="btn btn-primary" />
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="lnkNext_Click" CssClass="btn btn-primary"/>--%>
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
                            <br />
                            <br />
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
