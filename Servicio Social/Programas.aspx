<%@ Page Title="" Language="C#" MasterPageFile="~/Programas.Master" AutoEventWireup="true" CodeBehind="Programas.aspx.cs" Inherits="Servicio_Social.Programas1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <table class="tabla-formulario">
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label33" runat="server" AssociatedControlID="" CssClass="label-derecha" >Periodo:</asp:Label>
                        <asp:DropDownList ID="DDLPeriodo" runat="server" CssClass="DropDownList-estilo" Enabled="false">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label34" runat="server" AssociatedControlID="" CssClass="label-derecha">Coordinación de unidad:</asp:Label>
                        <asp:DropDownList ID="DropDownList14" runat="server" CssClass="DropDownList-estilo">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label35" runat="server" AssociatedControlID="" CssClass="label-derecha">Nombre del Programa:</asp:Label>
                        <asp:TextBox ID="TextBox17" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label36" runat="server" AssociatedControlID="" CssClass="label-derecha">Responsable:</asp:Label>
                        <asp:TextBox ID="TextBox18" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label37" runat="server" AssociatedControlID="" CssClass="label-derecha">Cargo actual:</asp:Label>
                        <asp:TextBox ID="TextBox19" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label38" runat="server" AssociatedControlID="" CssClass="label-derecha">Enfoque general:</asp:Label>
                        <asp:DropDownList ID="DDLEnfoque" runat="server" CssClass="DropDownList-estilo">
                        </asp:DropDownList>
                    </div>

                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label41" runat="server" AssociatedControlID="" CssClass="label-derecha">Modalidad:</asp:Label>
                        <asp:DropDownList ID="DDLModalidad" runat="server" CssClass="DropDownList-estilo">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label42" runat="server" AssociatedControlID="" CssClass="label-derecha">Plan de Estudios:</asp:Label>
                        <asp:DropDownList ID="DDLPlan" runat="server" CssClass="DropDownList-estilo">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label43" runat="server" AssociatedControlID="" CssClass="label-derecha">Escuela:</asp:Label>
                        <asp:DropDownList ID="DDLEscuela" runat="server" CssClass="DropDownList-estilo">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label39" runat="server" AssociatedControlID="" CssClass="label-derecha">Objetivos:</asp:Label>
                        <asp:TextBox ID="TextBox20" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label40" runat="server" AssociatedControlID="" CssClass="label-derecha">Actividades:</asp:Label>
                        <asp:TextBox ID="TextBox22" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3"></asp:TextBox>
                    </div>
                </td>
            </tr>

        </table>
    </div>
    <%--<div class="row">
  <div class="col-sm-6">
      <div class="form-group">
            <asp:Label ID="Label35" runat="server" AssociatedControlID="" CssClass="label-derecha">Nombre del Programa:</asp:Label>
            <asp:TextBox ID="TextBox17" runat="server" CssClass="textbox-estilo" ></asp:TextBox>
    </div>

</div>
  <div class="col-sm-6">
      <div class="form-group">
        <asp:Label ID="Label36" runat="server" AssociatedControlID="" CssClass="label-derecha">Responsable:</asp:Label>
        <asp:TextBox ID="TextBox18" runat="server" CssClass="textbox-estilo" ></asp:TextBox>
    </div>
  </div>
    --%>
    
</asp:Content>
