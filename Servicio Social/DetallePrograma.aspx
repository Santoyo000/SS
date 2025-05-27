<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="DetallePrograma.aspx.cs" Inherits="Servicio_Social.DetallePrograma" %>
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
            <div class="container-fluid">
                <div style="text-align: center">
                    <div class="form-group">
                        <h2 class="text-gray-900 mb-4" style="color: #2e5790">Alumnos Registrados En Programas</h2>
                    </div>
                    <div class="">
                        <div class="row mb-2">
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtPrograma" class="me-2" style="width: 150px;">Programa:</label>
                                <asp:TextBox ID="txtPrograma" runat="server" CssClass="form-control" placeholder="Programa..." />
                            </div>
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtResponsable" class="me-2" style="width: 150px;">Reponsable del Programa:</label>
                                <asp:TextBox ID="txtResponsable" runat="server" CssClass="form-control" placeholder="Reponsable del Programa..." />
                            </div>
                             <div class="col-md-6 d-flex align-items-center">
                                 <label for="txtDependencia" class="me-2" style="width: 150px;">Dependencia:</label>
                                 <asp:TextBox ID="txtDependencia" runat="server" CssClass="form-control" placeholder="Dependencia..." />
                             </div>
                             <div class="col-md-6 d-flex align-items-center">
                                 <label for="txtCorreo" class="me-2" style="width: 150px;">Correo:</label>
                                 <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Correo..." />
                             </div>
                            <div class="col-md-6 d-flex align-items-center">
                                <label for="txtUnidad" class="me-2" style="width: 150px;">Unidad:</label>
                                  <asp:DropDownList ID="DDLUnidad" runat="server" CssClass="form-control" placeholder="Seleccione una Unidad..."  AutoPostBack="true">  
                                  </asp:DropDownList>
                            </div>
                             <div class="col-md-6 d-flex align-items-center">
                                <label for="txtPeriodo" class="me-2" style="width: 150px;">Periodo:</label>
                                  <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control" placeholder="Seleccione un Periodo..." >
                               </asp:DropDownList>
                            </div>
                              <div class="col-md-6 d-flex justify-content-end">
                                 <asp:Button ID="btnExportExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-excel me-3"  /> <%-- OnClick="btnExportarExcel_Click"--%>
                                 <asp:Button ID="btnBorrar" runat="server" Text="Limpiar Filtros" CssClass="btn btn-secondary me-2"  /><%-- OnClick="btnBorrar_Click"--%>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary"  />  <%--OnClick="btnBuscar_Click"--%>
                            </div>
                          </div>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th style="display: none;">ID</th>
                                    <th>Fecha de Registro</th>
                                    <th>Periodo</th>
                                    <th>Nombre del Programa</th>
                                    <th>Responsable</th>
                                    <th>Dependencia</th>
                                    <th>Correo</th>
                                    <th>Unidad</th>
                                    <th>Cupo</th>
                                    <th>Total Alumnos Inscritos</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server" > <%--OnItemDataBound="Repeater1_ItemDataBound"--%>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                                <td style="display: none;"><%# Eval("idPrograma") %></td>
                                                <td><%# Eval("FechaRegistro") %></td>
                                                 <td><%# Eval("Periodo") %></td>
                                                <td><%# Eval("NombrePrograma") %></td>
                                                <td><%# Eval("Responsable") %></td>
                                                 <td><%# Eval("Dependencia") %></td>
                                                 <td><%# Eval("Correo") %></td>
                                                 <td><%# Eval("Unidad") %></td>
                                                <td><%# Eval("Cupo") %></td>
                                                <td><%# Eval("TotalAlumnosInscritos") %></td>
                                                
                                                <td>
                                                    <div class="d-flex justify-content-center">
                                                        <asp:LinkButton ID="btnEvaluar" Visible ="true" runat="server" CommandName="cmdEvaluar" CommandArgument='<%# Eval("idPrograma") %>'  CssClass="btn" style="background-color: #24ba35; color: white; border: none;"><span data-toggle="tooltip" title="Añadir Alumnos" ><i class="fas fa-user-plus"></i></asp:LinkButton>   <%--OnClick="btnEvaluar_Click"--%>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Encuesta" runat="server">
</asp:Content>
