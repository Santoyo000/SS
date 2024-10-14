<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="EditarDetallePrograma.aspx.cs" Inherits="Servicio_Social.EditarDetallePrograma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
     <asp:Panel ID="pnlDetallePrograma" runat="server" Visible="true">
          <div style="text-align: center">
     <div class="form-group">
         <br />
         <h2 class="text-gray-900 mb-4" style="color: #2e5790">Editar Cupos del Programa</h2>
     </div>
 </div>
       <div class="container">
           <br />
           <div style="text-align: center">
               <div class="">
                   <table class="table table-bordered">
                       <thead>
                           <tr>
                               <th style="display: none;">idDetallePrograma</th>
                               <th>Nivel</th>
                               <th>Plan de Estudios</th>
                               <th>Escuela</th>
                               <th>Asignados</th>
                               <th>Vacantes</th>
                               <th>Cupo</th>
                               <th>Editar</th>
                           </tr>
                       </thead>
                       <tbody>
                           <asp:Repeater ID="RepeaterDetPro" runat="server" OnItemCommand="RepeaterDetPro_ItemCommand">
                               <ItemTemplate>
                                   <tr>
                                       <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                           <td style="display: none;"><%# Eval("idDetallePrograma") %></td>
                                           <td><%# Eval("Nivel") %></td>
                                           <td><%# Eval("PlanE") %></td>
                                           <td><%# Eval("Escuela") %></td>
                                           <td><%# Eval("Asignados") %></td>
                                           <td><%# Eval("Vacantes") %></td>
                                           <td><%# Eval("iCupo") %></td>
                                           <td>
                                               <asp:LinkButton runat="server" ID="lnkEditDep" CommandName="Edit" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-edit"></i></asp:LinkButton>
                                           </td>
                                       </asp:Panel>
                                       <asp:Panel runat="server" ID="pnlEditMode" Visible="false">
                                           <asp:HiddenField ID="hdnID2" runat="server" Value='<%# Eval("idDetallePrograma") %>' />
                                           <td>
                                               <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="txtNivel"> <%# Eval("Nivel") %></asp:Label>
                                           </td>
                                           <td>
                                               <asp:Label Style="font-size: 0.9em !important;"  runat="server" ID="txtPlan" Text='<%# Eval("PlanE") %>'></asp:Label>
                                           </td>
                                           <td>
                                               <asp:Label Style="font-size: 0.9em !important;"  runat="server" ID="txtEscuela" Text='<%# Eval("Escuela") %>'></asp:Label>
                                           </td>
                                         
                                                                                          <td>
                                                   <asp:Label Style="font-size: 0.9em !important;"  runat="server" ID="Label3" Text='<%# Eval("Asignados") %>'></asp:Label>
                                               </td>
                                                                                          <td>
                                                   <asp:Label Style="font-size: 0.9em !important;"  runat="server" ID="Label4" Text='<%# Eval("Vacantes") %>'></asp:Label>
                                               </td>
                                           <td>
                                               <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtCupo" Text='<%# Eval("iCupo") %>'></asp:TextBox>
                                           </td>
                                           <td>
                                               <asp:LinkButton runat="server" ID="lnkUpdate" CommandName="Update" CommandArgument='<%# Container.ItemIndex %>'><i class="fas fa-save"></i></asp:LinkButton>
                                               <asp:LinkButton runat="server" ID="lnkCancel" CommandName="Cancel" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-window-close"></i></asp:LinkButton>
                                           </td>
                                       </asp:Panel>
                                   </tr>
                               </ItemTemplate>
                           </asp:Repeater>
                       </tbody>
                   </table>
                    <div style="text-align: left;">
                         <asp:Button ID="Button2" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" OnClick="btnRegresar_Click" />
                     </div>
               </div>
           </div>
       </div>
   </asp:Panel>
</asp:Content>
