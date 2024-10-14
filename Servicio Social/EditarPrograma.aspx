<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="EditarPrograma.aspx.cs" Inherits="Servicio_Social.EditarPrograma" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <asp:Panel ID="PANELPRINCIPAL" runat="server" Visible="true">
        <div style="text-align: center">
            <div class="form-group">
                <br />
                <h2 class="text-gray-900 mb-4" style="color: #2e5790">Editar Programa</h2>
            </div>
        </div>

        <table class="tabla-formulario">
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label33" runat="server" AssociatedControlID="" CssClass="label-derecha">Periodo:</asp:Label>
                        <asp:DropDownList ID="DDLPeriodo" runat="server" CssClass="DropDownList-estilo DropDownList-con-fondo-gris" Enabled="false">
                        </asp:DropDownList>
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
                        <asp:TextBox ID="txtPrograma" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label34" runat="server" AssociatedControlID="" CssClass="label-derecha">Coordinación de unidad:</asp:Label>
                        <asp:DropDownList ID="DDLUnidad" runat="server" CssClass="DropDownList-estilo" AutoPostBack="true" OnSelectedIndexChanged="DDLUnidad_SelectedIndexChanged" required="true">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label36" runat="server" AssociatedControlID="" CssClass="label-derecha">Responsable:</asp:Label>
                        <asp:TextBox ID="txtResponsable" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label37" runat="server" AssociatedControlID="" CssClass="label-derecha">Cargo actual:</asp:Label>
                        <asp:TextBox ID="txtCargo" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label10" runat="server" AssociatedControlID="" CssClass="label-derecha">Horario:</asp:Label>
                        <asp:TextBox ID="txtHorario" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label11" runat="server" AssociatedControlID="" CssClass="label-derecha">Lugar de adscripción:</asp:Label>
                        <asp:TextBox ID="txtLugar" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
            </tr>

        </table>

        <table class="tabla-formulario3">
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label41" runat="server" AssociatedControlID="" CssClass="label-derecha">Modalidad:</asp:Label>
                        <asp:DropDownList ID="DDLModalidad" runat="server" CssClass="DropDownList-estilo" AutoPostBack="true" OnSelectedIndexChanged="OTRO_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" AssociatedControlID="" CssClass="label-derecha">Enfoque general:</asp:Label>
                        <asp:DropDownList ID="DDLEnfoque" runat="server" CssClass="DropDownList-estilo" AutoPostBack="true" OnSelectedIndexChanged="OTRO_SelectedIndexChanged2">
                        </asp:DropDownList>
                    </div>

                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label9" runat="server" AssociatedControlID="" CssClass="label-derecha">Area responsable:</asp:Label>
                        <asp:TextBox ID="txtAreaResp" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                    </div>
                </td>
            </tr>

            <tr>
                <asp:Panel ID="pnlOModalidad" runat="server" Visible="false">
                    <td>
                        <div class="form-group">
                            <asp:Label ID="lblOtM" runat="server" AssociatedControlID="" CssClass="label-derecha">Otra Modalidad:</asp:Label>
                            <asp:TextBox ID="txtOtM" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                </asp:Panel>
                <asp:Panel ID="pnlOEnfoque" runat="server" Visible="false">
                    <td>
                        <div class="form-group">
                            <asp:Label ID="lblOE" runat="server" AssociatedControlID="" CssClass="label-derecha">Otro Enfoque:</asp:Label>
                            <asp:TextBox ID="txtOE" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                </asp:Panel>
            </tr>
        </table>
        <asp:Panel ID="pnl2regtistros" runat="server" Visible="true">
            <div style="text-align: center">
                <asp:Label ID="Label6" runat="server" AssociatedControlID="" CssClass="label-derecha" Font-Bold="True">Seleccione Unidad, Nivel, Plan de estudios, Escuela y Cupo y de click en el botón para añadir uno o más.</asp:Label>
            </div>
            <table class="tabla-formulario2">
                <tr>
                    <td>
                        <div class="form-group">

                            <asp:Label ID="LBLNIVEL1" runat="server" AssociatedControlID="" CssClass="label-derecha">Nivel:</asp:Label>
                            <asp:DropDownList ID="DDLNIVEL1" runat="server" CssClass="DropDownList-estilo" AutoPostBack="true" OnSelectedIndexChanged="DDLPlan_SelectedIndexChanged"></asp:DropDownList>

                        </div>

                    </td>

                    <td>
                        <div class="form-group">
                            <asp:Label ID="LBLPLAN1" runat="server" AssociatedControlID="" CssClass="label-derecha">Plan de Estudios:</asp:Label>
                            <asp:DropDownList ID="DDLPLAN1" runat="server" CssClass="DropDownList-estilo" OnSelectedIndexChanged="DDLEscuela_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <div class="form-group">
                                <asp:Label ID="LBLESC1" runat="server" AssociatedControlID="" CssClass="label-derecha">Escuela UAdeC:</asp:Label>
                                <asp:DropDownList ID="DDLESC1" runat="server" CssClass="DropDownList-estilo" onchange="updateTitle(this)">
                                </asp:DropDownList>
                            </div>
                            <script>
                                function updateTitle(ddl) {
                                    var selectedOption = ddl.options[ddl.selectedIndex];
                                    ddl.setAttribute('title', selectedOption.text);
                                }
                            </script>
                        </div>
                    </td>
                    <td>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="" CssClass="label-derecha">Cupo:</asp:Label>
                            <asp:TextBox ID="tCupo" runat="server" CssClass="textbox-estilo"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnl3registros" runat="server" Visible="true">
            <table class="tabla-formulario4">
                <tr>
                    <td>
                        <div style="text-align: center">
                            <asp:Button runat="server" ID="btnAnadir" CssClass="miBoton" OnClick="btnAnadir_Click" Text="Añadir" />
                        </div>
                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="gridview-estilo" OnRowDataBound="GridView2_RowDataBound" OnRowCommand="GridView2_RowCommand" ClientIDMode="Static">
                            <Columns>
                                <asp:BoundField DataField="idDetallePrograma" HeaderText="Nivel" />
                                <asp:BoundField DataField="Nivel" HeaderText="Nivel" />
                                <asp:BoundField DataField="kpNivel" HeaderText="Nivel" />
                                <asp:BoundField DataField="PlanE" HeaderText="Plan de Estudios" />
                                <asp:BoundField DataField="kpPlanEstudio" HeaderText="Plan de Estudios" />
                                <asp:BoundField DataField="Escuela" HeaderText="Escuela" />
                                <asp:BoundField DataField="kpEscuela" HeaderText="Escuela" />
                                <asp:BoundField DataField="iCupo" HeaderText="Cupo" />
                               <%-- <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbl_Eliminar" runat="server" Text="Eliminar" OnClientClick='return confirm("El registro se eliminará, esta acción no se puede deshacer.");' OnClick="lbl_Eliminar_Click" CssClass="miBoton2" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
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
                        <asp:TextBox ID="txtObjetivos" runat="server" TextMode="MultiLine" CssClass="textbox-estilo2" Rows="3"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label40" runat="server" AssociatedControlID="" CssClass="label-derecha">Actividades:</asp:Label>
                        <asp:TextBox ID="txtActividades" runat="server" TextMode="MultiLine" CssClass="textbox-estilo2" Rows="3"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" AssociatedControlID="" CssClass="label-derecha">Domicilio Servicio Social:</asp:Label>
                        <asp:TextBox ID="TxtDomicilio" runat="server" TextMode="MultiLine" CssClass="textbox-estilo2" Rows="3"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="form-group">

                        <asp:Label ID="Label8" runat="server" AssociatedControlID="" CssClass="label-derecha">Link Google Maps donde se realizará el servicio social:</asp:Label>
                        <asp:TextBox ID="txtLinkGoogle" runat="server" TextMode="MultiLine" CssClass="textbox-estilo" Rows="3" AutoPostBack="true"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <div class="containerbtnIniciar">
            <asp:Button runat="server" ID="btnGuardar" CssClass="miBoton" Text="Guardar Cambios" OnClick="btnGuardad_Click" />


        </div>
    </asp:Panel>

    <asp:Panel ID="pnlProgramaExitoso" runat="server" Visible="false" Style="text-align: center">
        <div class="text-align: center">
            <div class="texto">
                <br />
                <h2 style="color: #333333; font-size: 24px; margin-bottom: 20px;">Los datos del programa han sido guardados con éxito</h2>
                <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Se realizará la revisión de los datos del programa.</p>
                <a href="LoginDependencia.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
            </div>
            <br />
            <a class="miBoton" href="Dependencias.aspx">Regresar</a>
        </div>
    </asp:Panel>
</asp:Content>
