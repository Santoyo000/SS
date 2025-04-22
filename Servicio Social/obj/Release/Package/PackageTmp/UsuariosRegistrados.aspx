<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="UsuariosRegistrados.aspx.cs" Inherits="Servicio_Social.UsuariosRegistrados" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #TabSelection .nav-link {
            cursor: pointer;
            background-color: transparent;
            border: none;
            border-bottom: 2px solid transparent;
        }

            #TabSelection .nav-link:hover {
                background-color: #ccc; /* Color de fondo al pasar el mouse */
            }

            #TabSelection .nav-link:focus {
                box-shadow: none;
                outline: none;
            }

            #TabSelection .nav-link.active {
                background-color: #28a745; /* Color de fondo del enlace activo */
                border-color: #dee2e6 #dee2e6 #fff;
                color: #2e5790; /* Color del texto del enlace activo */
            }

        .table td {
            font-size: 13px; /* Tamaño de fuente más pequeño para las celdas de datos */
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
                background-color: #495057; /*#516e96; Color de fondo de la cabecera */
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
            background-color: #f7d05a;
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
    <script type="text/javascript">
        function setActiveNavLink(activeId) {
            // Obtener todos los enlaces de navegación
            var navLinks = document.querySelectorAll('.nav-link');

            // Desactivar todos los enlaces
            navLinks.forEach(function (link) {
                link.classList.remove('active');
            });

            // Activar el enlace que ha sido clickeado
            var activeLink = document.getElementById(activeId);
            if (activeLink) {
                activeLink.classList.add('active');
            }
        }
    </script>
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
                        url: '<%= ResolveUrl("UsuariosRegistrados.aspx/buscarCorreo") %>',
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

    <script> /* ALUMNOS*/
        $(document).ready(function () {
            // Define las funciones de manejo de eventos
            function handleMatriculaInput() {
                var valor = $("#<%= txtMatriculaAl.ClientID %>").val();
         if (valor.length > 7 && valor.length > 0) {
             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("UsuariosRegistrados.aspx/GetAlumnoInfo") %>',
              data: JSON.stringify({ buscar: valor }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (response) {
                  if (response.d) {
                      var alumno = response.d;
                      $("#<%= txtNombreAl.ClientID %>").val(alumno.Nombre);
                      $("#<%= txtApePatAl.ClientID %>").val(alumno.ApellidoPaterno);
                      $("#<%= txtApeMatAl.ClientID %>").val(alumno.ApellidoMaterno);

                      var ddlEscuelas = $("#<%= ddlEscuelaAl.ClientID %>");
                      ddlEscuelas.empty();
                      var ddlPlanEstudio = $("#<%= ddlPlanEstudioAl.ClientID %>");
                      ddlPlanEstudio.empty();
                      ddlEscuelas.append($('<option>', { value: '', text: '-- Seleccione --' }));
                      $.each(alumno.Escuelas, function (index, escuela) {
                          ddlEscuelas.append($('<option>', { value: escuela.Id, text: escuela.Nombre }));
                      });
                      ddlPlanEstudio.append($('<option>', { value: '', text: '-- Seleccione --' }));
                      $.each(alumno.PlanesEstudio, function (index, plan) {
                          ddlPlanEstudio.append($('<option>', { value: plan.Id, text: plan.Nombre, text: plan.Nombre }));
                      });
                      $("#<%= btnRegistrarAl.ClientID %>").prop("disabled", false);
                      $("#<%= lblErrorAl.ClientID %>").text('');
                  } else {
                      $("#<%= btnRegistrarAl.ClientID %>").prop("disabled", true);
                      $("#<%= lblErrorAl.ClientID %>").text('La matrícula ingresada no fue encontrada o cuenta con estatus de Baja/Exalumno, favor de revisar.');
                  }
              },
              error: function (error) {
                  console.log(error);
                  $("#<%= btnRegistrarAl.ClientID %>").prop("disabled", true);
                  $("#<%= lblErrorAl.ClientID %>").text('La matrícula ingresada no fue encontrada o cuenta con estatus de Baja/Exalumno, favor de revisar.');
              }
          });
      } else {
          $("#<%= txtNombreAl.ClientID %>").val('');
          $("#<%= txtApePatAl.ClientID %>").val('');
          $("#<%= txtApeMatAl.ClientID %>").val('');
          var ddlEscuelas = $("#<%= ddlEscuelaAl.ClientID %>");
          ddlEscuelas.empty();
          var ddlPlanEstudio = $("#<%= ddlPlanEstudioAl.ClientID %>");
          ddlPlanEstudio.empty();
          $("#<%= btnRegistrarAl.ClientID %>").prop("disabled", true);
             $("#<%= lblErrorAl.ClientID %>").text('');
         }
     }

           <%-- function handleEscuelaChange() {
                var escuelaId = $("#<%= ddlEscuelaAl.ClientID %>").val();
            var matricula = $("#<%= txtMatriculaAl.ClientID %>").val();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("UsuariosRegistrados.aspx/GetPlanesEstudio") %>',
                data: JSON.stringify({ escuelaId: escuelaId, matricula: matricula }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        var planesEstudio = JSON.parse(response.d);
                        var ddlPlanEstudio = $("#<%= ddlPlanEstudioAl.ClientID %>");
                        ddlPlanEstudio.empty();
                        ddlPlanEstudio.append($('<option>', { value: '', text: '-- Seleccione --' }));
                        $.each(planesEstudio, function (index, planEstudio) {
                            ddlPlanEstudio.append($('<option>', { value: planEstudio.Id, text: planEstudio.Nombre }));
                        });
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
            }--%>

            function validarPassword() {
                var password = $("#<%= txtPasswordAl.ClientID %>").val();
            var confirmPassword = $("#<%= txtPasswordConfirmAl.ClientID %>").val();
            var btnSubmit = $("#<%= btnRegistrarAl.ClientID %>");

                if (password === confirmPassword) {
                    $("#lblErrorPassAl").text("");
                    btnSubmit.prop("disabled", false); // Habilitar el botón
                } else {
                    $("#lblErrorPassAl").text("Las contraseñas no coinciden");
                    btnSubmit.prop("disabled", true); // Deshabilitar el botón
                }
            }

            // Asigna los manejadores de eventos iniciales
            $("#<%= txtMatriculaAl.ClientID %>").on("input", handleMatriculaInput);
         <%--   $("#<%= ddlEscuelaAl.ClientID %>").on("change", handleEscuelaChange);--%>
            $("#<%= txtPasswordConfirmAl.ClientID %>").on("keyup", validarPassword);

            // Reasigna los manejadores de eventos después de cada actualización parcial del UpdatePanel
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                $("#<%= txtMatriculaAl.ClientID %>").off("input").on("input", handleMatriculaInput);
           <%-- $("#<%= ddlEscuelaAl.ClientID %>").off("change").on("change", handleEscuelaChange);--%>
            $("#<%= txtPasswordConfirmAl.ClientID %>").off("keyup").on("keyup", validarPassword);
        });
        });


        // Función para permitir solo números en el campo  NUEVO AÑADIDO 19/01/2025
        function soloNumeros(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            // Permite solo los números del 0 al 9
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        }

        // Función para validar que la longitud no exceda de 2 caracteres
        function validarSemestre() {
            var txtSemestre = document.getElementById('<%= txtSemestre.ClientID %>');
           var semestre = txtSemestre.value;

           // Limitar a 2 caracteres
           if (semestre.length > 2) {
               txtSemestre.value = semestre.substring(0, 2);
           }

           // Usar AJAX para enviar el valor al servidor si necesitas hacer una validación
           if (semestre.length === 2) {
               // Llamada AJAX para validar el semestre en el servidor (si es necesario)
               $.ajax({
                   type: "POST",
                   url: "UsuariosRegistrados.aspx/ValidarSemestre",
                   data: JSON.stringify({ semestre: semestre }),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (response) {
                       if (response.d) {
                           // Validación exitosa
                           document.getElementById('<%= litMensaje.ClientID %>').innerText = 'Semestre válido';
                } else {
                    // Mostrar mensaje de error si el semestre no es válido
                    document.getElementById('<%= litMensaje.ClientID %>').innerText = 'Semestre inválido';
                }
            },
            failure: function (response) {
                alert('Error en la validación del semestre.');
            }
        });
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
    <%--   <div class="container">--%>
    <div class="nav nav-tabs" id="TabSelection">
        <asp:LinkButton ID="lbnAlumnosIncorp" runat="server" CssClass="nav-item nav-link" Text="Alumnos Incorporadas" OnClick="lbnAlumnosIncorp_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbnResponsable" runat="server" CssClass="nav-item nav-link" Text="Responsables de Unidad" OnClick="lbnResponsable_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbnEncargadoEsc" runat="server" CssClass="nav-item nav-link" Text="Encargado de Escuela" OnClick="lbnEncargadoEsc_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbnDependencias" runat="server" CssClass="nav-item nav-link" Text="Dependencias" OnClick="lbnDependencias_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbnUsuarios" runat="server" CssClass="nav-item nav-link" Text="+ Crear Usuarios" OnClick="lbnUsuarios_Click"></asp:LinkButton>
    </div>
    <%--    </div>--%>
    <div style="display: inline-block;">
    </div>
    <asp:Panel ID="pnlResponsables" runat="server" Visible="false">
        <br />
        <%--        <div class="container">--%>
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
        <%--      </div>--%>
    </asp:Panel>
    <asp:Panel ID="pnlDependencias" runat="server" Visible="false">
        <%--      <div class="container">--%>
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
                            <th>Fecha Registro</th>
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
                                        <td><%# Eval("FechaRegistro") %></td>
                                        <td><%# Eval("sDescripcion") %></td>
                                        <td><%# Eval("sCorreo") %></td>
                                        <td>
                                            <p>••••••••</p>
                                        </td>
                                        <td style="display: none;"><%# Eval("sPassword") %></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkEditDep" CommandName="Edit" CssClass="btn btn-success btn-sm" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-edit"></i></asp:LinkButton>
                                        </td>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlEditModeDep" Visible="false">
                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("idUsuario") %>' />
                                        <td>
                                            <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="Label9"> <%# Eval("FechaRegistro") %></asp:Label>
                                        </td>
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
                                            <asp:LinkButton runat="server" ID="lnkUpdate" CommandName="Update" CssClass="btn btn-primary btn-sm" CommandArgument='<%# Container.ItemIndex %>'><i class="fas fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lnkCancel" CommandName="Cancel" CssClass="btn btn-danger btn-sm" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-window-close"></i></asp:LinkButton>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
                <asp:Button ID="btnPrevDep" runat="server" Text="Anterior" OnClick="btnPrevDep_Click" CssClass="btn btn-primary" />
                <asp:Label ID="lblPageDep" runat="server"></asp:Label>
                <asp:Button ID="btnNextDep" runat="server" Text="Siguiente" OnClick="btnNextDep_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <%--        </div>--%>
    </asp:Panel>
    <asp:Panel ID="PanelEncargadosEscuela" runat="server" Visible="false">
        <%--      <div class="container">--%>
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
                <asp:Button ID="btnPreviousEncarg" runat="server" Text="Anterior" OnClick="btnPrevEncarg_Click" CssClass="btn btn-primary" />
                <asp:Label ID="lblPageNumberEncarg" runat="server"></asp:Label>
                <asp:Button ID="btnNextEncarg" runat="server" Text="Siguiente" OnClick="btnNextEncarg_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <%--        </div>--%>
    </asp:Panel>
    <asp:Panel ID="pnlAlumnosIncorp" runat="server" Visible="true">
        <%--        <div class="container">--%>
        <br />
        <div style="text-align: center">
            <div class="">
                <div class="row mb-3">
                    <div class="col-md-3">
                        <asp:TextBox ID="txtBuscarAlumInc" runat="server" CssClass="form-control" placeholder="Buscar..." />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnBuscarAlumInc" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscarAlumInc_Click" />
                    </div>
                </div>
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="display: none;">ID</th>
                            <th>Fecha Registro</th>
                            <th>Matricula</th>
                            <th>Alumno</th>
                            <th>Correo</th>
                            <th>Contraseña</th>
                            <th style="display: none;">sPassword</th>
                            <th>Plan de Estudios</th>
                            <th>Escuela</th>
                            <th>Editar</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="RepeaterAlumnosIncorp" runat="server" OnItemDataBound="RepeaterAlumnosIncorp_ItemDataBound" OnItemCommand="RepeaterAlumnosIncorp_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <asp:Panel runat="server" ID="pnlViewModeAlumInc" Visible="true">
                                        <td style="display: none;"><%# Eval("ID") %></td>
                                        <td><%# Eval("FechaRegistro") %></td>
                                        <td><%# Eval("Matricula") %></td>
                                        <td><%# Eval("sNombreCompleto") %></td>
                                        <td><%# Eval("sCorreo") %></td>
                                        <td>
                                            <p>••••••••</p>
                                        </td>
                                        <td style="display: none;"><%# Eval("sPassword") %></td>
                                        <td><%# Eval("PlanEst") %></td>
                                        <td><%# Eval("Escuela") %></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" CssClass="btn btn-success btn-sm" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-edit"></i></asp:LinkButton>
                                        </td>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlEditModeAlumInc" Visible="false">
                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>' />
                                        <td>
                                            <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="Label9"> <%# Eval("FechaRegistro") %></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="Label1"> <%# Eval("Matricula") %></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="Label8"> <%# Eval("sNombreCompleto") %></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtCorreo" Text='<%# Eval("sCorreo") %>'></asp:TextBox>
                                        </td>

                                        <td>
                                            <asp:TextBox Style="font-size: 0.9em !important;" CssClass="form-control" runat="server" ID="txtPassword" Text='<%# Eval("sPassword") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="TextBox1"> <%# Eval("PlanEst") %></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label Style="font-size: 0.9em !important;" runat="server" ID="TextBox2"><%# Eval("Escuela") %>></asp:Label>
                                        </td>
                                        <td>
                                            <div class="d-flex justify-content-center">
                                                <asp:LinkButton runat="server" ID="lnkUpdate" CommandName="Update" CssClass="btn btn-primary btn-sm" CommandArgument='<%# Container.ItemIndex %>'><i class="fas fa-save"></i></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="lnkCancel" CommandName="Cancel" CssClass="btn btn-danger btn-sm" CommandArgument='<%# Container.ItemIndex %>'><i class="far fa-window-close"></i></asp:LinkButton>
                                            </div>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
                <asp:Button ID="btnPrevAluInc" runat="server" Text="Anterior" OnClick="btnPrevAluInc_Click" CssClass="btn btn-primary" />
                <asp:Label ID="lblPageAluInc" runat="server"></asp:Label>
                <asp:Button ID="btnNextAluInc" runat="server" Text="Siguiente" OnClick="btnNextAluIn_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <%--        </div>--%>
    </asp:Panel>
    <asp:Panel ID="pnlUsuarios" runat="server" Visible="false" CssClass="panel-container">
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="lblUser" class="label-derecha"><strong>Seleccione el tipo de Usuario</strong></label>
                        <asp:DropDownList runat="server" ID="ddlUser" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <%-- USUARIO ADMINISTRADOR --%>
        <asp:Panel ID="pnlRegistrarAdmon" runat="server" Visible="false" CssClass="panel-container">
            <div class="container" style="border: 1px solid #ccc; padding: 20px; border-radius: 10px; background-color: #f9f9f9;">
                <div style="text-align: center">
                    <div class="form-group">
                        <br />
                        <h3 class="text-gray-900 mb-4" style="color: #2e5790">Capture los datos del Administrador:</h3>
                    </div>
                </div>
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
                    <div class="col-md-4 text-center">
                        <asp:Button runat="server" ID="Button1" CssClass="btn miBoton" Text="CREAR USUARIO" OnClick="btnRegistrarAdmon_Click" />
                    </div>
                </div>
                <br />
                <br />
                <br />
            </div>
        </asp:Panel>
        <%-- USUARIO RESPONSABLE --%>
        <asp:Panel ID="pnlRegistrarResponsable" runat="server" Visible="false">
            <div class="container" style="border: 1px solid #ccc; padding: 20px; border-radius: 10px; background-color: #f9f9f9;">
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
            </div>
        </asp:Panel>
        <%-- USUARIO ENCARGADO --%>
        <asp:Panel ID="pnlRegistrarEncargado" runat="server" Visible="false">
            <div class="container" style="border: 1px solid #ccc; padding: 20px; border-radius: 10px; background-color: #f9f9f9;">
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
            </div>
        </asp:Panel>
        <%-- USUARIO DEPENDENCIAS --%>
        <asp:Panel ID="pnlRegistrarDependencias" runat="server" Visible="false">
            <div class="container" style="border: 1px solid #ccc; padding: 20px; border-radius: 10px; background-color: #f9f9f9;">
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
                    <div class="row">
                        <div class="text-center">
                            <asp:Label ID="lblMensajeErrorDep" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-md-4 text-center">
                            <asp:Button runat="server" ID="btnGeneraDep" CssClass="btn miBoton" Text="CREAR DEPENDENCIA" OnClick="btnDependencia_Click" />
                        </div>
                    </div>
                </div>
                </div>
        </asp:Panel>
        <%-- ALUMNOS --%>
        <asp:Panel ID="pnlRegistroAlumnos" Visible="false" runat="server">
            <div class="container" style="border: 1px solid #ccc; padding: 20px; border-radius: 10px; background-color: #f9f9f9;">
                <div style="text-align: center">
                    <div class="form-group">
                        <br />
                        <h2 class="text-gray-900 mb-4" style="color: #2e5790">Completa los siguientes datos:</h2>
                        <i style="font-style: italic;">Alumnos de escuelas oficiales se registrarán con su Correo Institucional.</i>
                        <br />
                        <i style="font-style: italic;">Alumnos de escuelas incorporadas  se registrarán con su Matrícula y contraseña.</i>
                        <br />
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlTipoEscuela" class="label-derecha">Tipo de escuela:</label>
                                <asp:DropDownList ID="ddlTipoEscuela" runat="server" CssClass="form-control" required="required" OnSelectedIndexChanged="ddlTipoEscuela_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="-- Seleccione --" Value="" />
                                    <asp:ListItem Text="Oficial" Value="1" />
                                    <asp:ListItem Text="Incorporada" Value="2" />
                                </asp:DropDownList>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeTipoEscuela"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtMatriculaAl" class="label-derecha">Matrícula:</label>
                                <asp:TextBox ID="txtMatriculaAl" runat="server" CssClass="form-control" required="required" MaxLength="8"></asp:TextBox>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeMatricula"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtNombreAl" class="label-derecha">Nombre:</label>
                                <asp:TextBox ID="txtNombreAl" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtApePatAl" class="label-derecha">Apellido Paterno:</label>
                                <asp:TextBox ID="txtApePatAl" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtApeMatAl" class="label-derecha">Apellido Materno:</label>
                                <asp:TextBox ID="txtApeMatAl" runat="server" CssClass="form-control" required="required" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblCorreoAl" runat="server" Text="Correo:"></asp:Label>
                                <asp:TextBox ID="txtCorreoAl" runat="server" CssClass="form-control" required="required" type="email"></asp:TextBox>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeCorreo"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlEscuelaAl" class="label-derecha">Escuela/Facultad:</label>
                                <asp:DropDownList ID="ddlEscuelaAl" runat="server" CssClass="form-control" ViewStateMode="Enabled"></asp:DropDownList>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEscuela"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlPlanEstudioAl" class="label-derecha">Plan de estudio:</label>
                                <asp:DropDownList ID="ddlPlanEstudioAl" runat="server" CssClass="form-control" ViewStateMode="Enabled"></asp:DropDownList>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajePlanEstudio"></asp:Label>
                        </div>
                    </div>

                    <asp:Panel ID="pnlPasswordAl" Visible="false" runat="server">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPasswordAl" class="label-derecha">Contraseña:</label>
                                    <asp:TextBox ID="txtPasswordAl" runat="server" CssClass="form-control" required="required" type="password"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPasswordConfirmAl" class="label-derecha">Confirmar Contraseña:</label>
                                    <asp:TextBox ID="txtPasswordConfirmAl" runat="server" CssClass="form-control" required="required" type="password" onkeyup="validarPassword()"></asp:TextBox>
                                </div>
                                <div id="lblErrorPassAl" style="color: #ff0d0d"></div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblSemestre" runat="server" Text="Semestre:"></asp:Label>
                                <asp:TextBox ID="txtSemestre" runat="server" CssClass="form-control" required="required" MaxLength="2" oninput="validarSemestre()" onkeypress="return soloNumeros(event);"></asp:TextBox>
                                <asp:Literal ID="litMensaje" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="text-center">
                    <asp:Label runat="server" Style="color: #ff0d0d" ID="lblErrorAl"></asp:Label>
                </div>
                <div class="row justify-content-center">
                    <div class="col-md-4 text-right">
                        <asp:Button runat="server" ID="btnRegistrarAl" CssClass="btn miBoton" Text="CREAR ESTUDIANTE" OnClick="btnRegistrarAl_Click" />
                    </div>
                </div>

                <br />
                <br />
                <br />
            </div>
        </asp:Panel>
    </asp:Panel>
    <div class="container">
        <asp:Button ID="miBoton" runat="server" Text="Regresar" CssClass="btn btn-primary miBoton" PostBackUrl="PanelAdministrador.aspx" />
    </div>
</asp:Content>
