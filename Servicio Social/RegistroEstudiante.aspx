<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="RegistroEstudiante.aspx.cs" Inherits="Servicio_Social.RegistroEstudiante1" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript">
       $(document).ready(function () {
           // Define las funciones de manejo de eventos
           function handleMatriculaInput() {
               var valor = $("#<%= txtMatricula.ClientID %>").val();
            if (valor.length > 7 && valor.length > 0) {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("RegistroEstudiante.aspx/GetAlumnoInfo") %>',
                    data: JSON.stringify({ buscar: valor }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d) {
                            var alumno = response.d;
                            $("#<%= txtNombre.ClientID %>").val(alumno.Nombre);
                            $("#<%= txtApePat.ClientID %>").val(alumno.ApellidoPaterno);
                            $("#<%= txtApeMat.ClientID %>").val(alumno.ApellidoMaterno);

                            var ddlEscuelas = $("#<%= ddlEscuela.ClientID %>");
                            ddlEscuelas.empty();
                            var ddlPlanEstudio = $("#<%= ddlPlanEstudio.ClientID %>");
                            ddlPlanEstudio.empty();
                            ddlEscuelas.append($('<option>', { value: '', text: '-- Seleccione --' }));
                            $.each(alumno.Escuelas, function (index, escuela) {
                                ddlEscuelas.append($('<option>', { value: escuela.Id, text: escuela.Nombre }));
                            });
                            $("#<%= btnRegistrar.ClientID %>").prop("disabled", false);
                            $("#<%= lblError.ClientID %>").text('');
                        } else {
                            $("#<%= btnRegistrar.ClientID %>").prop("disabled", true);
                            $("#<%= lblError.ClientID %>").text('La matrícula ingresada no fue encontrada o cuenta con estatus de Baja/Exalumno, favor de revisar.');
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        $("#<%= btnRegistrar.ClientID %>").prop("disabled", true);
                        $("#<%= lblError.ClientID %>").text('La matrícula ingresada no fue encontrada o cuenta con estatus de Baja/Exalumno, favor de revisar.');
                    }
                });
            } else {
                $("#<%= txtNombre.ClientID %>").val('');
                $("#<%= txtApePat.ClientID %>").val('');
                $("#<%= txtApeMat.ClientID %>").val('');
                var ddlEscuelas = $("#<%= ddlEscuela.ClientID %>");
                ddlEscuelas.empty();
                var ddlPlanEstudio = $("#<%= ddlPlanEstudio.ClientID %>");
                ddlPlanEstudio.empty();
                $("#<%= btnRegistrar.ClientID %>").prop("disabled", true);
                $("#<%= lblError.ClientID %>").text('');
            }
        }

        function handleEscuelaChange() {
            var escuelaId = $("#<%= ddlEscuela.ClientID %>").val();
            var matricula = $("#<%= txtMatricula.ClientID %>").val();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("RegistroEstudiante.aspx/GetPlanesEstudio") %>',
                data: JSON.stringify({ escuelaId: escuelaId, matricula: matricula }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        var planesEstudio = JSON.parse(response.d);
                        var ddlPlanEstudio = $("#<%= ddlPlanEstudio.ClientID %>");
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
        }

        function validarPassword() {
            var password = $("#<%= txtPassword.ClientID %>").val();
            var confirmPassword = $("#<%= txtPasswordConfirm.ClientID %>").val();
            var btnSubmit = $("#<%= btnRegistrar.ClientID %>");

            if (password === confirmPassword) {
                $("#lblErrorPass").text("");
                btnSubmit.prop("disabled", false); // Habilitar el botón
            } else {
                $("#lblErrorPass").text("Las contraseñas no coinciden");
                btnSubmit.prop("disabled", true); // Deshabilitar el botón
            }
        }

        // Asigna los manejadores de eventos iniciales
        $("#<%= txtMatricula.ClientID %>").on("input", handleMatriculaInput);
        $("#<%= ddlEscuela.ClientID %>").on("change", handleEscuelaChange);
        $("#<%= txtPasswordConfirm.ClientID %>").on("keyup", validarPassword);

        // Reasigna los manejadores de eventos después de cada actualización parcial del UpdatePanel
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#<%= txtMatricula.ClientID %>").off("input").on("input", handleMatriculaInput);
            $("#<%= ddlEscuela.ClientID %>").off("change").on("change", handleEscuelaChange);
            $("#<%= txtPasswordConfirm.ClientID %>").off("keyup").on("keyup", validarPassword);
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
            url: "RegistroEstudiante.aspx/ValidarSemestre",
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

<asp:Content ID="content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
      <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
    <ProgressTemplate>
        <div id="overlay">
            <div id="loadingContent">
                <asp:Image ID="imgWaitIcon" runat="server" ImageUrl="Image/loading.gif" AlternateText="Cargando..." style="max-width: 300px;"/>
                <div id="loadingText">Por favor, espere...</div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelCerrado" runat="server" Visible="false">
                <div style="text-align: center">
                    <div class="form-group">
                        <br />
                        <h3 id="mensajeCierre" class="text-gray-900 mb-4" style="color: #2e5790">
                         <asp:Label ID="lblMensajeEstudiante" runat="server"></asp:Label>
                        </h3>
                        <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlRegistro" Visible="true" runat="server">
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
                                <label for="txtMatricula" class="label-derecha">Matrícula:</label>
                                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" required="required" MaxLength="8"></asp:TextBox>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeMatricula"></asp:Label>
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
                                <asp:Label ID="lblCorreo" runat="server" Text="Correo:"></asp:Label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" required="required" type="email"></asp:TextBox>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeCorreo"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlEscuela" class="label-derecha">Escuela/Facultad:</label>
                                <asp:DropDownList ID="ddlEscuela" runat="server" CssClass="form-control" required="required" EnableViewState="false"></asp:DropDownList>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajeEscuela"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlPlanEstudio" class="label-derecha">Plan de estudio:</label>
                                <asp:DropDownList ID="ddlPlanEstudio" runat="server" CssClass="form-control" required="required" EnableViewState="false"></asp:DropDownList>
                            </div>
                            <asp:Label runat="server" Style="color: #ff0d0d" ID="lblMensajePlanEstudio"></asp:Label>
                        </div>
                    </div>
                     
                    <asp:Panel ID="pnlPassword" Visible="false" runat="server">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPassword" class="label-derecha">Contraseña:</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" required="required" type="password"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPasswordConfirm" class="label-derecha">Confirmar Contraseña:</label>
                                    <asp:TextBox ID="txtPasswordConfirm" runat="server" CssClass="form-control" required="required" type="password" onkeyup="validarPassword()"></asp:TextBox>
                                </div>
                                <div id="lblErrorPass" style="color: #ff0d0d"></div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                           <asp:Label ID="lblSemestre" runat="server" Text="Semestre:"></asp:Label>
                           <asp:TextBox ID="txtSemestre" runat="server" CssClass="form-control" required="required"  maxlength="2" oninput="validarSemestre()"  onkeypress="return soloNumeros(event);"></asp:TextBox>
                            <asp:Literal ID="litMensaje" runat="server" />
                        </div>
                    </div>
                </div>
                </div>
                <div class="text-center">
                    <asp:Label runat="server" Style="color: #ff0d0d" ID="lblError"></asp:Label>
                </div>
                <div class="row justify-content-center">
                    <div class="col-md-4 text-right">
                        <a href="Home.aspx" class="btn miBoton" style="line-height: 40px;">Regresar</a>
                    </div>
                    <div class="col-md-4 text-left">
                        <asp:Button runat="server" ID="btnRegistrar" CssClass="btn miBoton" Text="Registrar" OnClick="btnRegistrar_Click" />
                    </div>
                </div>

                <br />
                <br />
                <br />
            </asp:Panel>

            <asp:Panel ID="pnlRegistroExitoso" runat="server" Visible="false" CssClass="contenedor">
                <h2 style="color: #333333; font-size: 24px; margin-bottom: 20px;">¡Registro Exitoso!</h2>
                <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Nos complace informarle que ha sido registrado exitosamente en nuestro sistema.</p>
                <p style="color: #666666; font-size: 16px; margin-bottom: 20px;">Una vez que se haya autorizado tendrá acceso a la Plataforma.</p>
                <footer style="text-align: center; color: #999999; font-style: italic; font-size: 14px;">¡Gracias por registrarse!</footer>
                <div style="text-align: center; margin-top: 20px;">
                    <a href="Home.aspx" cssclass="btn btn-primary">Volver a la página principal</a>
                </div>
           </asp:Panel>
        </ContentTemplate>
       <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlTipoEscuela" EventName="SelectedIndexChanged" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>