<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="ProgramasDependencias.aspx.cs" Inherits="Servicio_Social.ProgramasDependencias" %>

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
    <div class="container">
        <div style="text-align: center">
            <div class="form-group">
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Programas Registrados</h2>
            </div>

            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th style="display: none;">ID</th>
                        <th>Fecha de Registro</th>
                        <th>Dependencia</th>
                        <th>Correo</th>
                        <th>Nombre del Programa</th>
                        <th>Responsable</th>
                        <th>Estatus</th>
                        <th>Ver Más</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                    <td style="display: none;"><%# Eval("idPrograma") %></td>
                                    <td><%# Eval("FechaRegistro") %></td>
                                    <td><%# Eval("Dependencia") %></td>
                                    <td><%# Eval("Correo") %></td>
                                    <td><%# Eval("NombrePrograma") %></td>
                                    <td><%# Eval("Responsable") %></td>
                                    <td style="display: none;"><%# Eval("kpEstatus_Programa") %></td>
                                    <td><%# Eval("Estatus") %></td>
                                    <td>
                                        <div class="d-flex justify-content-center">
                                            <asp:LinkButton ID="bntEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("idPrograma") %>' OnClick="btnEditar_Click">
                                                <span data-toggle="tooltip" title="Ver Más"><i class="far fa-edit"></i></span>
                                            </asp:LinkButton>
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
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

            <asp:Button ID="btnPrevious" runat="server" Text="Anterior" OnClick="btnPrevious_Click" CausesValidation="false" />
            <asp:Label ID="lblPageNumber" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" Text="Siguiente" OnClick="btnNext_Click" CausesValidation="false" />

            <div style="text-align: left; margin-top:10px;">
                <asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelDependencia.aspx" />
            </div>
        </div>
    </div>
</asp:Content>