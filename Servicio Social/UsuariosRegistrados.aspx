<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="UsuariosRegistrados.aspx.cs" Inherits="Servicio_Social.UsuariosRegistrados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #TabSelection .nav-link {
            cursor: pointer;
            background-color: transparent;
            border: none;
            border-bottom: 2px solid transparent;
        }

            #TabSelection .nav-link:hover {
                background-color: #f8f9fa;
            }

            #TabSelection .nav-link:focus {
                box-shadow: none;
                outline: none;
            }

            #TabSelection .nav-link.active {
                background-color: #fff;
                border-color: #dee2e6 #dee2e6 #fff;
                color: #495057;
            }

        .table td {
            font-size: 13px; /* Tamaño de fuente más pequeño para las celdas de datos */
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= txtEmail.ClientID %>, #<%= txtExpediente.ClientID %>').on('input', function () {
                var query = $('#<%= txtEmail.ClientID %>').val();
                var expediente = $('#<%= txtExpediente.ClientID %>').val();
                if (query.length >= 12 && expediente.length >= 3) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("RegistroResponsable.aspx/buscarCorreo") %>',
                        data: JSON.stringify({ correo: query, exp: expediente }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var results = response.d;
                            if (results.length > 0) {
                                $('#<%= txtNombre.ClientID %>').val(results[0]);
                                $('#<%= txtApePat.ClientID %>').val(results[1]);
                                $('#<%= txtApeMat.ClientID %>').val(results[2]);
                                $('#<%= lblMensajeEmail.ClientID %>').text(''); // Limpiar el mensaje de error si hay resultados
                            } else {
                                // No se encontraron resultados, mostrar mensaje de error
                                $('#<%= lblMensajeEmail.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                                $('#<%= txtNombre.ClientID %>').val('');
                                $('#<%= txtApePat.ClientID %>').val('');
                                $('#<%= txtApeMat.ClientID %>').val('');
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error("Error: " + error);
                        }
                    });
                } else {
                    $('#<%= lblMensajeEmail.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                    $('#<%= txtNombre.ClientID %>').val('');
                    $('#<%= txtApePat.ClientID %>').val('');
                    $('#<%= txtApeMat.ClientID %>').val('');
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= txtEmailEnc.ClientID %>, #<%= txtExpedienteEnc.ClientID %>').on('input', function () {
                var query = $('#<%= txtEmailEnc.ClientID %>').val();
                var expediente = $('#<%= txtExpedienteEnc.ClientID %>').val();
                if (query.length >= 12 && expediente.length >= 3) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("RegistroResponsable.aspx/buscarCorreo") %>',
                          data: JSON.stringify({ correo: query, exp: expediente }),
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (response) {
                              var results = response.d;
                              if (results.length > 0) {
                                  $('#<%= txtNombreEnc.ClientID %>').val('');
                                  $('#<%= txtApePatEnc.ClientID %>').val('');
                                  $('#<%= txtApeMatEnc.ClientID %>').val('');

                                  $('#<%= txtNombreEnc.ClientID %>').val(results[0]);
                                  $('#<%= txtApePatEnc.ClientID %>').val(results[1]);
                                  $('#<%= txtApeMatEnc.ClientID %>').val(results[2]);
                                  $('#<%= lblMensajeEmailEnc.ClientID %>').text(''); // Limpiar el mensaje de error si hay resultados
                              } else {
                                  // No se encontraron resultados, mostrar mensaje de error
                                  $('#<%= lblMensajeEmailEnc.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                                  $('#<%= txtNombreEnc.ClientID %>').val('');
                                  $('#<%= txtApePatEnc.ClientID %>').val('');
                                  $('#<%= txtApeMatEnc.ClientID %>').val('');
                              }

                          },
                          error: function (xhr, status, error) {
                              console.error("Error: " + error);
                          }
                      });
                  }
                  else {
                      $('#<%= lblMensajeEmailEnc.ClientID %>').text('El expediente ingresado no coincide con el correo ingresado');
                      $('#<%= txtNombreEnc.ClientID %>').val('');
                      $('#<%= txtApePatEnc.ClientID %>').val('');
                      $('#<%= txtApeMatEnc.ClientID %>').val('');
                }
            });
        });
    </script>

    <script>
        function cambia() {
            var pass1 = document.getElementById('<%= txtContraDep.ClientID %>');
            var pass2 = document.getElementById('<%= txtReContraDep.ClientID %>');
            pass1.removeAttribute('readonly');
            pass2.removeAttribute('readonly');
        }
    </script>

    <script>
        function validarPassword() {
            var password = document.getElementById('<%= txtContraDep.ClientID %>').value;
            var confirmPassword = document.getElementById('<%= txtReContraDep.ClientID %>').value;
     <%--       var elColor = document.getElementById('<%= txtReContraDep.ClientID %>');--%>
            var btnSubmit = document.getElementById('<%= btnGeneraDep.ClientID %>');

            if (password == confirmPassword) {
                document.getElementById('<%= lblPasswords.ClientID %>').innerHTML = "";
                //elColor.style.backgroundColor = "green";
                //elColor.style.color = "white";
                btnSubmit.disabled = false; // Habilitar el botón
            } else {
                document.getElementById('<%= lblPasswords.ClientID %>').innerHTML = "Las contraseñas no coinciden";
                btnSubmit.disabled = true; // Deshabilitar el botón
            }
        }
    </script>
    <style>
        .contenedor {
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <br />
    <div class="container">
        <div class="nav nav-tabs" id="TabSelection">
            <asp:LinkButton ID="lbnDependencias" runat="server" CssClass="nav-item nav-link" Text="Dependencias" OnClick="lbnDependencias_Click"></asp:LinkButton>
            <asp:LinkButton ID="lbnResponsable" runat="server" CssClass="nav-item nav-link" Text="Responsables de Unidad" OnClick="lbnResponsable_Click"></asp:LinkButton>
            <asp:LinkButton ID="lbnEncargadoEsc" runat="server" CssClass="nav-item nav-link" Text="Encargado de Escuela" OnClick="lbnEncargadoEsc_Click"></asp:LinkButton>
            <asp:LinkButton ID="lbnUsuarios" runat="server" CssClass="nav-item nav-link" Text="+ Crear Usuarios" OnClick="lbnUsuarios_Click"></asp:LinkButton>

        </div>
    </div>
    <div style="display: inline-block;">
    </div>
    <asp:Panel ID="pnlResponsables" runat="server" Visible="false">
        <br />
        <div class="container">
            <div style="text-align: center">
                <div class="">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <asp:TextBox ID="txtBusquedaResponsables" runat="server" CssClass="form-control" placeholder="Buscar..." />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnBuscarResponsable" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscarResponsable_Click" />
                        </div>
                    </div>
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="display: none;">ID</th>
                                <th>Expediente</th>
                                <th>Nombre</th>
                                <th>Correo Institucional</th>
                                <th>Rol</th>
                                <th>Unidad</th>
                                <th>Estatus</th>
                                <%--<th>Acción</th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repeaterResp" runat="server">
                                <%--OnItemDataBound="repeaterResp_ItemDataBound"--%>
                                <ItemTemplate>
                                    <tr>
                                        <asp:Panel runat="server" ID="pnlViewMode" Visible="true">
                                            <td style="display: none;"><%# Eval("idUsuario") %></td>
                                            <td><%# Eval("sExpediente") %></td>
                                            <td><%# Eval("Nombre") %></td>
                                            <td><%# Eval("sCorreo") %></td>
                                            <td><%# Eval("TipoUsuario") %></td>
                                            <td><%# Eval("Unidad") %></td>
                                            <td><%# Eval("Estatus") %></td>
                                            <%--<td>
                                                <div class="d-flex justify-content-center">
                                                    <asp:LinkButton ID="btnAutorizar" runat="server" CommandName="cmdAutorizar" CommandArgument='<%# Eval("idUsuario") %>' OnClientClick='return confirm("El registro cambiará de estatus de Autorizado");' OnClick="btnAutorizar_Click" CssClass="btn btn-success btn-sm"><span data-toggle="tooltip" title="Autorizar"  ><i class="fas fa-check-square"></i></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnEliminar" runat="server" CommandName="cmdRechazar" CommandArgument='<%# Eval("idUsuario") %>' OnClientClick='return confirm("El registro cambiará de estatus de NO Autorizado");' OnClick="btnEliminar_Click" CssClass="btn btn-danger btn-sm"><span data-toggle="tooltip" title="No Autorizar" ><i class="fas fa-window-close"></i></asp:LinkButton>

                                                </div>
                                            </td>--%>
                                        </asp:Panel>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <asp:Button ID="btnPreviousResp" runat="server" Text="Anterior" CssClass="btn btn-primary" OnClick="btnPreviousResp_Click" />
                    <asp:Label ID="lblPageNumberResp" runat="server"></asp:Label>
                    <asp:Button ID="btnNextResp" runat="server" Text="Siguiente" CssClass="btn btn-primary" OnClick="btnNextResp_Click" />
                </div>
            </div>
        </div>

    </asp:Panel>
    <asp:Panel ID="pnlDependencias" runat="server" Visible="true">
        <div class="container">
            <br />
            <div style="text-align: center">
                <div class="">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <asp:TextBox ID="txtBusquedaDependencias" runat="server" CssClass="form-control" placeholder="Buscar..." />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnBuscarDependencia" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscarDependencia_Click" />
                        </div>
                    </div>
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="display: none;">ID</th>
                                <th>Dependencia</th>
                                <th>Correo</th>
                                <th>Contraseña</th>
                                <th style="display: none;">sPassword</th>
                                <th>editar</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="RepeaterDep" runat="server" OnItemCommand="RepeaterDep_ItemCommand" OnItemDataBound="RepeaterDep_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Panel runat="server" ID="pnlViewModeDep" Visible="true">
                                            <td style="display: none;"><%# Eval("idUsuario") %></td>
                                            <td><%# Eval("sDescripcion") %></td>
                                            <td><%# Eval("sCorreo") %></td>
                                            <td>
                                                <p>••••••••</p>
                                            </td>
                                            <td style="display: none;"><%# Eval("sPassword") %></td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkEditDep" CommandName="Edit" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-edit"></i></asp:LinkButton>
                                            </td>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlEditModeDep" Visible="false">
                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("idUsuario") %>' />
                                            <td>
                                                <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="lblDescripcion"> <%# Eval("sDescripcion") %></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtCorreo" Text='<%# Eval("sCorreo") %>'></asp:TextBox>
                                            </td>

                                            <td>
                                                <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtPassword" Text='<%# Eval("sPassword") %>'></asp:TextBox>
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
                    <asp:Button ID="btnPrevDep" runat="server" Text="Anterior" OnClick="btnPrevDep_Click" CssClass="btn btn-primary"/>
                    <asp:Label ID="lblPageDep" runat="server"></asp:Label>
                    <asp:Button ID="btnNextDep" runat="server" Text="Siguiente" OnClick="btnNextDep_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelEncargadosEscuela" runat="server" Visible="false">
        <div class="container">
            <br />
            <div style="text-align: center">
                <div class="">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <asp:TextBox ID="txtBuscarEncargado" runat="server" CssClass="form-control" placeholder="Buscar..." />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnBuscarEncargado" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscarEncargado_Click" />
                        </div>
                    </div>
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="display: none;">ID</th>
                                <%-- <th>Expediente</th>--%>
                                <th>Fecha de Registro</th>
                                <th>Nombre</th>
                                <th>Correo Institucional</th>
                                <th>Rol</th>
                                <th>Escuela</th>
                                <th>Unidad</th>
                                <th>Estatus</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="RepeaterEncarg" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Panel runat="server" ID="pnlViewModeDep" Visible="true">
                                            <td style="display: none;"><%# Eval("idUsuario") %></td>
                                            <%--<td><%# Eval("sExpediente") %></td>--%>
                                            <td><%# Eval("FechaRegistro") %></td>
                                            <td><%# Eval("Nombre") %></td>
                                            <td><%# Eval("sCorreo") %></td>
                                            <td><%# Eval("TipoUsuario") %></td>
                                            <td><%# Eval("Escuela") %></td>
                                            <td><%# Eval("Unidad") %></td>
                                            <td><%# Eval("Estatus") %></td>
                                        </asp:Panel>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                        </tbody>
                    </table>
                    <asp:Button ID="btnPreviousEncarg" runat="server" Text="Anterior" OnClick="btnPrevEncarg_Click" CssClass="btn btn-primary"/>
                    <asp:Label ID="lblPageNumberEncarg" runat="server"></asp:Label>
                    <asp:Button ID="btnNextEncarg" runat="server" Text="Siguiente" OnClick="btnNextEncarg_Click" CssClass="btn btn-primary"/>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlUsuarios" runat="server" Visible="false">
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="lblUser" class="label-derecha"><strong>Tipo de Usuario</strong></label>
                        <asp:DropDownList runat="server" ID="ddlUser" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <%-- USUARIO ADMINISTRADOR --%>
        <asp:Panel ID="pnlRegistrarAdmon" runat="server" Visible="false">
            <div style="text-align: center">
                <div class="form-group">
                    <br />
                    <h3 class="text-gray-900 mb-4" style="color: #2e5790">Capture los datos del Administrador:</h3>
                </div>
            </div>
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtExpediente" class="label-derecha">Expediente:</label>
                            <asp:TextBox ID="txt1" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label1"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtEmail" class="label-derecha">Correo Institucional:</label>
                            <asp:TextBox ID="txt2" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label2"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtNombre" class="label-derecha">Nombre:</label>
                            <asp:TextBox ID="txt3" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtApePat" class="label-derecha">Apellido Paterno:</label>
                            <asp:TextBox ID="txt4" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtApeMat" class="label-derecha">Apellido Materno:</label>
                            <asp:TextBox ID="txt5" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtTelefono" class="label-derecha">Teléfono:</label>
                            <asp:TextBox ID="txt6" runat="server" CssClass="form-control" required="required" MaxLength="10"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label3"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlUnidad" class="label-derecha">Unidad:</label>
                            <asp:DropDownList runat="server" ID="DDL1" CssClass="form-control" required="required"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="text-center">
                        <asp:Label ID="Label4" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row justify-content-center">
                    <%-- <div class="col-md-4 text-right">
                <a href="index.html" class="btn miBoton" style="line-height: 40px;">Regresar</a>
            </div>--%>
                    <div class="col-md-4 text-center">
                        <asp:Button runat="server" ID="Button1" CssClass="btn miBoton" Text="CREAR USUARIO" OnClick="btnRegistrarAdmon_Click" />
                    </div>
                </div>
            </div>
            <br />
            <br />
            <br />
        </asp:Panel>
        <%-- USUARIO RESPONSABLE --%>
        <asp:Panel ID="pnlRegistrarResponsable" runat="server" Visible="false">
            <div style="text-align: center">
                <div class="form-group">
                    <br />
                    <h3 class="text-gray-900 mb-4" style="color: #2e5790">Capture los datos del Responsable de Unidad:</h3>
                </div>
            </div>
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtExpediente" class="label-derecha">Expediente:</label>
                            <asp:TextBox ID="txtExpediente" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeExpediente"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtEmail" class="label-derecha">Correo Institucional:</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEmail"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtNombre" class="label-derecha">Nombre:</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtApePat" class="label-derecha">Apellido Paterno:</label>
                            <asp:TextBox ID="txtApePat" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtApeMat" class="label-derecha">Apellido Materno:</label>
                            <asp:TextBox ID="txtApeMat" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtTelefono" class="label-derecha">Teléfono:</label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" required="required" MaxLength="10"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeTelefono"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlUnidad" class="label-derecha">Unidad:</label>
                            <asp:DropDownList runat="server" ID="ddlUnidad" CssClass="form-control" required="required"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="text-center">
                        <asp:Label ID="lblMensajeError" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row justify-content-center">
                    <%-- <div class="col-md-4 text-right">
                        <a href="index.html" class="btn miBoton" style="line-height: 40px;">Regresar</a>
                    </div>--%>
                    <div class="col-md-4 text-center">
                        <asp:Button runat="server" ID="btnRegistrar" CssClass="btn miBoton" Text="CREAR USUARIO" OnClick="btnRegistrar_Click" />
                    </div>
                </div>
            </div>
            <br />
            <br />
            <br />
        </asp:Panel>
        <%-- USUARIO ENCARGADO --%>
        <asp:Panel ID="pnlRegistrarEncargado" runat="server" Visible="false">
            <div style="text-align: center">
                <div class="form-group">
                    <br />
                    <h3 class="text-gray-900 mb-4" style="color: #2e5790">Capture los datos del Encargado:</h3>
                </div>
            </div>
            <div class="container">

                <%-- DATOS DEL ENCArGADO --%>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtExpedienteEnc" class="label-derecha">Expediente:</label>
                            <asp:TextBox ID="txtExpedienteEnc" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label5"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtEmailEnc" class="label-derecha">Correo Institucional:</label>
                            <asp:TextBox ID="txtEmailEnc" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEmailEnc"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtNombreEnc" class="label-derecha">Nombre:</label>
                            <asp:TextBox ID="txtNombreEnc" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtApePatEnc" class="label-derecha">Apellido Paterno:</label>
                            <asp:TextBox ID="txtApePatEnc" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtApeMatEnc" class="label-derecha">Apellido Materno:</label>
                            <asp:TextBox ID="txtApeMatEnc" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtTelefonoEnc" class="label-derecha">Teléfono:</label>
                            <asp:TextBox ID="txtTelefonoEnc" runat="server" CssClass="form-control" required="required" MaxLength="10"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label6"></asp:Label>
                    </div>
                </div>
                <%-- LOS COMBOS --%>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlUnidadEnc" class="label-derecha">Unidad:</label>
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlUnidadEnc" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlUnidadEnc_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlNivelEnc" class="label-derecha">Nivel:</label>
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlNivelEnc" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlNivelEnc_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlEscuelEnc" class="label-derecha">Escuela:</label>
                            <asp:DropDownList runat="server" ID="ddlEscuelEnc" CssClass="form-control" required="required"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <%--  --%>
                <div class="row">
                    <div class="text-center">
                        <asp:Label ID="lblMensajeErrorEnc" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row justify-content-center">
                    <div class="col-md-4 text-center">
                        <asp:Button runat="server" ID="btnEncargado" CssClass="btn miBoton" Text="CREAR ENCARGADO" OnClick="btnEncargado_Click" />
                    </div>
                </div>
            </div>
            <br />
            <br />
            <br />
        </asp:Panel>
        <%-- USUARIO DEPENDENCIAS --%>
        <asp:Panel ID="pnlRegistrarDependencias" runat="server" Visible="false">
            <div style="text-align: center">
                <div class="form-group">
                    <h3 class="text-gray-900 mb-4" style="color: #2e5790">Capture los datos de la Dependencia:</h3>
                </div>
            </div>
            <div class="container">
                <%-- DATOS DEPENDENCIA --%>
                <%--  --%>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtDependenciaDep" class="label-derecha">Dependencia:</label>
                            <asp:TextBox ID="txtDependenciaDep" runat="server" CssClass="form-control" required="required" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtCorreoDep" class="label-derecha">Correo:</label>
                            <asp:TextBox ID="txtCorreoDep" runat="server" CssClass="form-control" required="required" autocomplete="off"></asp:TextBox>
                            <div style="color: red">
                                <asp:RegularExpressionValidator ID="regexEmail" runat="server"
                                    ControlToValidate="txtCorreoDep"
                                    ErrorMessage="El formato del correo electrónico no es válido."
                                    ValidationExpression="^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$"
                                    Display="Dynamic">
                                </asp:RegularExpressionValidator>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEmailDep"></asp:Label>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtContraDep" class="label-derecha">Contraseña:</label>
                            <asp:TextBox ID="txtContraDep" runat="server" CssClass="form-control" required="required" ReadOnly="true" TextMode="Password" onclick="cambia()" onfocus="cambia()"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtReContraDep" class="label-derecha">Confirme Contraseña:</label>
                            <asp:TextBox ID="txtReContraDep" runat="server" CssClass="form-control" required="required" ReadOnly="true" TextMode="Password" onkeyup="validarPassword()"></asp:TextBox>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblPasswords"></asp:Label>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlUnidadDep" class="label-derecha">Unidad:</label>
                            <asp:DropDownList runat="server" ID="ddlUnidadDep" CssClass="form-control" required="required"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlOrganismoDep" class="label-derecha">Organismo:</label>
                            <asp:DropDownList runat="server" ID="ddlOrganismoDep" CssClass="form-control" required="required"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtResponsableDep" class="label-derecha">Responsable:</label>
                            <asp:TextBox ID="txtResponsableDep" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtAreaDep" class="label-derecha">Área Responsable:</label>
                            <asp:TextBox ID="txtAreaDep" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtTelefonoDep" class="label-derecha">Teléfono:</label>
                            <asp:TextBox ID="txtTelefonoDep" runat="server" CssClass="form-control" required="required" MaxLength="10" AutoComplete="off"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Style="color: #ff0d0d" ID="Label7"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtDomicilioDep" class="label-derecha">Domicilio:</label>
                            <asp:TextBox ID="txtDomicilioDep" runat="server" CssClass="form-control" required="required" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%--  --%>
                <div class="row">
                    <div class="text-center">
                        <asp:Label ID="lblMensajeErrorDep" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row justify-content-center">
                    <div class="col-md-4 text-center">
                        <asp:Button runat="server" ID="btnGeneraDep" CssClass="btn miBoton" Text="CREAR DEPENDENCIA" OnClick="btnEncargado_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <div class="container">
        <asp:Button ID="miBoton" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelAdministrador.aspx" />
    </div>
</asp:Content>
