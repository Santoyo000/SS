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
        <script type="text/javascript">
            $(document).ready(function () {
                // Cargar el PDF en el iframe cuando se abre el modal
                $('#pdfModal').on('shown.bs.modal', function () {
                    var pdfBase64 = document.getElementById('<%= hiddenPdfBase64.ClientID %>').value;
            var pdfIframe = document.getElementById("pdfIframe");
            pdfIframe.src = "data:application/pdf;base64," + pdfBase64;
                });
            });
        </script>
        <script type="text/javascript">
            function mostrarOverlay() {
                document.getElementById("loadingOverlay").style.display = "block";
            }

            function ocultarOverlay() {
                document.getElementById("loadingOverlay").style.display = "none";
            }

            // Ocultar overlay cuando la página se recargue (por ejemplo, después del postback)
            if (window.addEventListener) {
                window.addEventListener('load', ocultarOverlay, false);
            } else if (window.attachEvent) {
                window.attachEvent('onload', ocultarOverlay);
            }
        </script>
    <style>
        /* Contenedor principal del formulario */
    .container-fluid {
        max-width: 100%;
        padding: 10px 20px;
    }

    /* Ajuste de cada fila del formulario */
    .form-group {
        margin-bottom: 5px; /* Espacio entre filas */
    }

    /* Estilo para los labels */
    label {
        text-align: right;
        font-weight: normal;
        width: 160px; /* Mantener uniforme el ancho de los labels */
        margin-right: 10px;
    }

    /* Estilo para los inputs y dropdowns */
    .form-control {
        width: 100%;
        padding: 8px;
        border-radius: 4px;
        border: 1px solid #ccc;
    }

    /* Espaciado entre los campos */
    .row .col-md-6 {
        margin-bottom: 15px;
    }

    /* Alinear botón de búsqueda a la derecha */
    .btn-container {
        display: flex;
        justify-content: flex-end;
        margin-top: 10px;
    }

    /* Botón de búsqueda */
    .btn-primary {
        padding: 10px 20px;
        font-size: 14px;
        border-radius: 4px;
        transition: background-color 0.3s ease;
    }

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
        background-color: #516e96; /* Color de fondo de la cabecera */
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
        /*.btn-primary {*/
        /*    background-color: #f7d05a;*/
            /*border-color: #f7d05a;
            color: white;
            padding: 10px 20px;
            font-size: 14px;
            border-radius: 4px;
            transition: background-color 0.3s ease;
        }

        .btn-primary:hover {
            background-color: #f1c40f;
            border-color: #f1c40f;
        }*/



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

            /* Modal Styling */
            .modal-content {
                border-radius: 8px;
                padding: 15px;
                background-color: #f9f9f9;
                color: #333;
            }

            .modal-header, .modal-footer {
                border: none;
                padding: 10px;
            }

            .modal-title {
                font-size: 18px;
                font-weight: bold;
                color: #333;
            }

            .close {
                font-size: 20px;
                color: #666;
            }

            .modal-body {
                padding: 20px;
            }

            .modal-footer button {
                background-color: #1073b0;
                color: #fff;
                border: none;
                padding: 10px 20px;
                border-radius: 4px;
                cursor: pointer;
                transition: background-color 0.3s ease;
            }

            .modal-footer button:hover {
                background-color: #0c5a8c;
            }
          /* Loading Overlay */
          #loadingOverlay {
              display: none;
              position: fixed;
              top: 0;
              left: 0;
              width: 100%;
              height: 100%;
              background-color: rgba(255, 255, 255, 0.85);
              z-index: 1000;
              text-align: center;
              font-size: 16px;
          }

          #loadingOverlay img {
              max-width: 80px;
              margin-top: 20%;
          }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <br />
    <asp:ScriptManager runat="server"> </asp:ScriptManager>
<!-- Overlay de carga -->
<div id="loadingOverlay" style="display:none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(255, 255, 255, 0.8); z-index: 1000; text-align: center;">
    <img src="Image/loading.gif" alt="Generando formato..." style="max-width: 300px; margin-top: 20%;">
    <div>Generando formato, por favor espere...</div>
</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div style="text-align: center">
                    <div class="form-group">
                        <h2 class="text-gray-900 mb-4" style="color: #2e5790">Alumnos Registrados En Programas</h2>
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
                                <label for="txtPrograma" class="me-2" style="width: 150px;">Programa:</label>
                                <asp:TextBox ID="txtPrograma" runat="server" CssClass="form-control" placeholder="Programa..." />
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
                                 <asp:Button ID="btnExportExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-excel me-3" OnClick="btnExportarExcel_Click" />
                                 <asp:Button ID="btnBorrar" runat="server" Text="Limpiar Filtros" CssClass="btn btn-secondary me-2" OnClick="btnBorrar_Click" />
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>
                          </div>
                          <%--  <div class="col-md-3">
                                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control" placeholder="Buscar..." />
                            </div>--%>   
                              
                         
                         <%--<div class="col-12 d-flex justify-content-end btn-container">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                        </div>--%>
                         
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
                                                       <%-- <asp:LinkButton ID="btnEvaluar" Visible ="true" runat="server" CommandName="cmdEvaluar" CommandArgument='<%# Eval("IDALUMNO") %>' OnClick="btnEvaluar_Click"  CssClass="btn" style="background-color: #49207d; color: white; border: none;"><span data-toggle="tooltip" title="Evaluar Alumno" ><i class="fas fa-pen-to-square"></i></asp:LinkButton> --%>
                                                       <asp:LinkButton ID="btnEvaluar" Visible="true" runat="server" CommandName="cmdEvaluar" 
                                                            CommandArgument='<%# Eval("IDALUMNO") + "|" + Eval("idProgramaAlumno") %>' 
                                                            OnClick="btnEvaluar_Click"  
                                                            CssClass="btn" style="background-color: #49207d; color: white; border: none;">
                                                            <span data-toggle="tooltip" title="Evaluar Alumno"><i class="fas fa-pen-to-square"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnDOC" Visible ="true" runat="server" CommandName="cmdDOC" CommandArgument='<%# Eval("idProgramaAlumno") %>' OnClick="btnDOC_Click"  CssClass="btn" style="background-color: #055a1e; color: white; border: none;"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fas fa-file-pdf"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnLiberar" Visible ="true" runat="server" CommandName="cmdLiberar" CommandArgument='<%# Eval("idProgramaAlumno") %>' OnClick="btnLiberar_Click"  CssClass="btn" style="background-color: #1c40a5; color: white; border: none;"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fas fa-file-pdf"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnLiberarEsc" Visible ="true" runat="server" CommandName="cmdLiberarEsc" CommandArgument='<%# Eval("idProgramaAlumno") %>' OnClick="btnLiberarEsc_Click" OnClientClick='return confirm("¿Desea liberar alumno?");' CssClass="btn" style="background-color: #03be35; color: white; border: none;"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fa-regular fa-square-check"></i></asp:LinkButton> 
                                                        <asp:LinkButton ID="btnLiberarResp" Visible ="true" runat="server" CommandName="cmdLiberarResp" CommandArgument='<%# Eval("idProgramaAlumno") %>' OnClick="btnLiberarResp_Click" OnClientClick='return confirm("¿Desea liberar alumno?");' CssClass="btn" style="background-color: #03be35; color: white; border: none;"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fa-regular fa-square-check"></i></asp:LinkButton> 
                                                        <asp:LinkButton ID="btnLiberarAdmon" Visible ="true" runat="server" CommandName="cmdLiberarAdmon" CommandArgument='<%# Eval("idProgramaAlumno") %>' OnClick="btnLiberarAdmon_Click" OnClientClick='return confirm("¿Desea liberar alumno?");' CssClass="btn" style="background-color: #03be35; color: white; border: none;"><span data-toggle="tooltip" title="Liberar Alumno" ><i class="fa-regular fa-square-check"></i></asp:LinkButton> 
                                                        <asp:LinkButton ID="btnDetalle" Visible="true" runat="server" CommandName="cmdRechazar" CssClass="btn btn-warning" Style="background-color: orange; border-color: orange; color: white;" data-toggle="modal" data-target="#myModal" OnClick='<%# "loadModalData(" + Eval("IDALUMNO") + ")" %>'><span data-toggle="tooltip" title="Ver detalles" ><i class="fas fa-search"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnAutorizar" runat="server" CommandName="cmdAutorizar" CommandArgument='<%# Eval("IDDEPENDENICASERVICIO") + "|" + Eval("idProgramaAlumno")+ "|" + Eval("IDALUMNO")%> ' OnClick="btnAutorizar_Click" OnClientClick='return confirm("El registro cambiará de estatus de Autorizado");' CssClass="btn btn-success btn-sm"><span data-toggle="tooltip" title="Autorizar" ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="cmdRechazar" CommandArgument='<%# Eval("IDDEPENDENICASERVICIO") + "|" + Eval("idProgramaAlumno")+ "|" + Eval("IDALUMNO")%> ' OnClick="btnEliminar_Click" OnClientClick='return confirm("El alumno será eliminado del Programa Actual, ¿desea continuar?");' CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="No Autorizar" ><i class="fas fa-window-close"></i></asp:LinkButton>
                                                       
                                                    </div>
                                                </td>
                                            </asp:Panel>

                                            
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                  
                               <asp:HiddenField ID="hiddenPdfBase64" runat="server" />
                            </tbody>
                        </table>
                       <%---<asp:Button ID="btnExportExcel" runat="server" Text="Exportar a Excel" CssClass="btn-excel" OnClick="btnExportExcel_Click" /> --%>
<%--                        <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="lnkPrev_Click" CssClass="btn btn-primary" />
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
                        <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="lnkNext_Click" CssClass="btn btn-primary" />--%>
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
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
       <Triggers>
    <asp:PostBackTrigger ControlID="btnExportExcel" />
</Triggers>
    </asp:UpdatePanel>
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
         <!-- Modal PDF -->
  <div class="modal fade" id="pdfModal" tabindex="-1" role="dialog" aria-labelledby="pdfModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-xl" role="document">
          <div class="modal-content">
              <div class="modal-header">
                  <h5 class="modal-title" id="pdfModalLabel">Carta Comprobante</h5>
                  <asp:HiddenField ID="hfidPrograma" runat="server" />
                  <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                  </button>
              </div>
              <div class="modal-body">
                   <div class="modal-footer">
                       <asp:Button ID="btnUpdate" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                 <%-- <iframe id="pdfIframe" style="width: 100%; height: 500px; border: none;" frameborder="0"></iframe>--%>
                <iframe id="pdfIframe" style="width: 100%; height: 600px; border: none; display: none;" frameborder="0"></iframe>
              </div>
              <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
              </div>
          </div>
      </div>
  </div>
</div>
</asp:Content>
