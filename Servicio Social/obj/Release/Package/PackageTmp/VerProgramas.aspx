<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="VerProgramas.aspx.cs" Inherits="Servicio_Social.VerProgramas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <asp:Panel ID="PANELPRINCIPAL" runat="server" Visible="true">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Datos del Programa</h2>
            </div>
        </div>

        <table class="tabla-formulario">
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label33" runat="server" AssociatedControlID="" CssClass="label-derecha">Periodo:</asp:Label>
                        <asp:TextBox ID="txtPeriodo" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" AssociatedControlID="" CssClass="label-derecha">Dependencia:</asp:Label>
                        <asp:TextBox ID="txtDependencia" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label35" runat="server" AssociatedControlID="" CssClass="label-derecha">Nombre del Programa:</asp:Label>
                        <asp:TextBox ID="txtPrograma" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label34" runat="server" AssociatedControlID="" CssClass="label-derecha">Coordinación de unidad:</asp:Label>
                        <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label36" runat="server" AssociatedControlID="" CssClass="label-derecha">Responsable:</asp:Label>
                        <asp:TextBox ID="txtResponsable" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label37" runat="server" AssociatedControlID="" CssClass="label-derecha">Cargo actual:</asp:Label>
                        <asp:TextBox ID="txtCargo" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label10" runat="server" AssociatedControlID="" CssClass="label-derecha">Horario:</asp:Label>
                        <asp:TextBox ID="txtHorario" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label11" runat="server" AssociatedControlID="" CssClass="label-derecha">Lugar de adscripción:</asp:Label>
                        <asp:TextBox ID="txtLugar" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>

        </table>

        <table class="tabla-formulario3">
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label41" runat="server" AssociatedControlID="" CssClass="label-derecha">Modalidad:</asp:Label>
                        <asp:TextBox ID="txtModalidad" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>

                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" AssociatedControlID="" CssClass="label-derecha">Enfoque general:</asp:Label>
                        <asp:TextBox ID="txtEnfoque" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>

                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label9" runat="server" AssociatedControlID="" CssClass="label-derecha">Area responsable:</asp:Label>
                        <asp:TextBox ID="txtAreaResp" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="" CssClass="label-derecha">Otra Modalidad:</asp:Label>
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" Visible="false">
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="" CssClass="label-derecha">Otro Enfoque:</asp:Label>
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                </asp:Panel>
            </tr>
            <tr>
                <asp:Panel ID="pnlOModalidad" runat="server" Visible="false">
                    <td>
                        <div class="form-group">
                            <asp:Label ID="lblOtM" runat="server" AssociatedControlID="" CssClass="label-derecha">Otra Modalidad:</asp:Label>
                            <asp:TextBox ID="txtOtM" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                        </div>
                    </td>
                </asp:Panel>
                <asp:Panel ID="pnlOEnfoque" runat="server" Visible="false">
                    <td>
                        <div class="form-group">
                            <asp:Label ID="lblOE" runat="server" AssociatedControlID="" CssClass="label-derecha">Otro Enfoque:</asp:Label>
                            <asp:TextBox ID="txtOE" runat="server" CssClass="textbox-estilo" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                        </div>
                    </td>
                </asp:Panel>
            </tr>
        </table>
        <asp:Panel ID="pnl3registros" runat="server" Visible="true">
            <table class="tabla-formulario4">
                <tr>
                    <td>
                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="gridview-estilo" ClientIDMode="Static" OnRowDataBound="GridView2_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="idDetallePrograma" HeaderText="Nivel" />
                                <asp:BoundField DataField="Nivel" HeaderText="Nivel" />
                                <asp:BoundField DataField="kpNivel" HeaderText="Nivel" />
                                <asp:BoundField DataField="PlanE" HeaderText="Plan de Estudios" />
                                <asp:BoundField DataField="kpPlanEstudio" HeaderText="Plan de Estudios" />
                                <asp:BoundField DataField="Escuela" HeaderText="Escuela" />
                                <asp:BoundField DataField="kpEscuela" HeaderText="Escuela" />
                                <asp:BoundField DataField="iCupo" HeaderText="Cupo" />
                            </Columns>
                        </asp:GridView>

                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table class="tabla-formulario">
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label39" runat="server" AssociatedControlID="" CssClass="label-derecha">Objetivos:</asp:Label>
                        <asp:TextBox ID="txtObjetivos" runat="server" TextMode="MultiLine" CssClass="textbox-estilo2" Style="background-color: #f2f2f2" Rows="3" Enabled="false"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label40" runat="server" AssociatedControlID="" CssClass="label-derecha">Actividades:</asp:Label>
                        <asp:TextBox ID="txtActividades" runat="server" TextMode="MultiLine" CssClass="textbox-estilo2" Style="background-color: #f2f2f2" Rows="3" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" AssociatedControlID="" CssClass="label-derecha">Domicilio Servicio Social:</asp:Label>
                        <asp:TextBox ID="TxtDomicilio" runat="server" TextMode="MultiLine" CssClass="textbox-estilo2" Style="background-color: #f2f2f2" Rows="3" Enabled="false"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">

                        <asp:Label ID="Label8" runat="server" AssociatedControlID="" CssClass="label-derecha">Link Google Maps donde se realizará el servicio social:</asp:Label>
                        <asp:TextBox ID="txtLinkGoogle" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3" Style="background-color: #f2f2f2" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
