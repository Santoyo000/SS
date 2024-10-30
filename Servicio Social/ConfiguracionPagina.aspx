<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="ConfiguracionPagina.aspx.cs" Inherits="Servicio_Social.ConfiguracionPagina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Incluir Bootstrap 4 para estilos adicionales -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />

    <script type="text/javascript">
        // Abrir el calendario al hacer clic en el campo de texto
        function abrirCalendario(campo) {
            campo.showPicker(); // Utiliza el método showPicker para mostrar el selector de fechas
        }
        // Confirmación al eliminar una imagen
        function confirmDelete() {
            return confirm("¿Estás seguro de que deseas eliminar esta imagen?");
        }
        document.addEventListener('DOMContentLoaded', function () {
            // Agregar un evento al cambiar el archivo en el file upload
            var fileInput = document.getElementById('<%= fileUpload.ClientID %>');
            var fileLabel = document.querySelector('.custom-file-label');

            fileInput.addEventListener('change', function (event) {
                var fileName = event.target.files[0] ? event.target.files[0].name : 'Seleccionar imagen';
                fileLabel.textContent = fileName;
            });
        });
        // Validar que la fecha de fin no sea anterior a la fecha de inicio
        function validarFechas() {
            var fechaInicioStr = document.getElementById('<%= txtFechaInicio.ClientID %>').value;
            var fechaFinStr = document.getElementById('<%= txtFechaFin.ClientID %>').value;

            if (!fechaInicioStr || !fechaFinStr) {
                mostrarModalError("Por favor ingrese ambas fechas.");
                return false;
            }

            var fechaInicio = new Date(fechaInicioStr);
            var fechaFin = new Date(fechaFinStr);

            if (fechaFin < fechaInicio) {
                mostrarModalError("La fecha de fin no puede ser anterior a la fecha de inicio.");
                return false; // Evita el envío del formulario
            }

            return true; // Permite el envío si todo está correcto
        }
        // Validar que la fecha de fin no sea anterior a la fecha de inicio
        function validarFechas2() {
                var fechaInicioStr = document.getElementById('<%= txtFechaInicioPro.ClientID %>').value;
        var fechaFinStr = document.getElementById('<%= txtFechaFinPro.ClientID %>').value;

        if (!fechaInicioStr || !fechaFinStr) {
            mostrarModalError("Por favor ingrese ambas fechas.");
            return false;
        }

        var fechaInicio = new Date(fechaInicioStr);
        var fechaFin = new Date(fechaFinStr);

        if (fechaFin < fechaInicio) {
            mostrarModalError("La fecha de fin no puede ser anterior a la fecha de inicio.");
            return false; // Evita el envío del formulario
        }

        return true; // Permite el envío si todo está correcto
            }
       // Validar que la fecha de fin no sea anterior a la fecha de inicio
       function validarFechas3() {
                var fechaInicioStr = document.getElementById('<%= txtFechaInicioAlu.ClientID %>').value;
        var fechaFinStr = document.getElementById('<%= txtFechaFinAlu.ClientID %>').value;

        if (!fechaInicioStr || !fechaFinStr) {
            mostrarModalError("Por favor ingrese ambas fechas.");
            return false;
        }

        var fechaInicio = new Date(fechaInicioStr);
        var fechaFin = new Date(fechaFinStr);

        if (fechaFin < fechaInicio) {
            mostrarModalError("La fecha de fin no puede ser anterior a la fecha de inicio.");
            return false; // Evita el envío del formulario
        }

        return true; // Permite el envío si todo está correcto
    }
        // Función para mostrar el modal de error con el mensaje especificado
        function mostrarModalError(mensaje) {
            var modalBody = document.querySelector("#ModalError .modal-body");
            modalBody.textContent = mensaje; // Actualizar el contenido del modal con el mensaje de error
            $('#ModalError').modal('show'); // Mostrar el modal
        }
    </script>
    <style>
        .image-container {
            display: flex;
            justify-content: center;
            flex-wrap: wrap;
            gap: 20px;
            margin-top: 20px;
        }

        .image-box {
            position: relative;
            overflow: hidden;
            transition: transform 0.3s ease-in-out;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        .image-box img {
            width: 100%;
            height: auto;
        }

        .image-box:hover {
            transform: scale(1.05);
        }

        .btnDelete {
            position: absolute;
            top: 10px;
            right: 10px;
            background-color: rgba(255, 0, 0, 0.7);
            color: white;
            border: none;
             padding: 10px 15px; /* Aumentado el padding para hacerlo más grande */
            cursor: pointer;
            border-radius: 0; /* Cambiado a 0 para esquinas rectas */
        }

        .btnDelete:hover {
            background-color: rgba(255, 0, 0, 0.9);
        }

        .tab-container {
            margin-top: 20px;
        }

        .card-custom {
            border-radius: 10px;
            box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
        }

        .custom-file {
            margin-top: 10px;
        }

         /* Estilos generales */
        .form-section {
            margin-top: 40px;
        }

        .form-section h4 {
            color: #2e5790;
            text-align: center;
            margin-bottom: 20px;
            font-weight: bold;
        }

        .form-control {
            border-radius: 10px;
            padding: 10px;
        }

        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
            border-radius: 10px;
            padding: 10px 20px;
            font-size: 18px;
            width: 100%;
        }

        .btn-primary:hover {
            background-color: #0056b3;
            border-color: #0056b3;
        }

        /* Mejora visual de las fechas */
        .date-group {
            display: flex;
            justify-content: space-between;
            gap: 20px;
        }

        .date-group .form-group {
            flex: 1;
        }

        /* Estilos del botón */
        .btnGuardar {
            margin-top: 20px;
            text-align: center;
        }

        .alert-message {
            color: red;
            font-weight: bold;
            text-align: center;
            margin-bottom: 20px;
        }

        #txtMensaje {
        width: 100%; /* Mantiene el ancho como el anterior */
        height: 50px; /* Ajuste a la altura que pediste */
        padding: 8px;
        font-size: 14px;
        border-radius: 5px;
        border: 1px solid #ccc; /* Borde gris estándar */
        transition: border-color 0.3s ease-in-out; /* Transición suave para el borde */
    }

    #txtMensaje:focus {
        border-color: #2e5790; /* Borde azul al seleccionar */
        outline: none; /* Quitar el borde de enfoque predeterminado */


    }

    
   .linea-dorada {
       border-top: 2px solid gold;
       margin: 10px 0;
   }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <div class="container mt-4">

    <%--    <div class="linea-dorada"></div>--%>
        <h2 class="text-center" style="color: #2e5790">Configuración del Sistema</h2>
     <%--   <div class="linea-dorada"></div>--%>
        <hr />

        <!-- Componente de pestañas -->
    <div class="tab-container"> <!-- Contenedor adicional -->
        <ul class="nav nav-tabs" id="configTabsNew" role="tablist">
            <li class="nav-item">
                <a class="nav-link active" id="imagenes-tab" data-toggle="tab" href="#imagenes" role="tab" aria-controls="imagenes" aria-selected="true">Cargar Imágenes</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="dependencias-tab" data-toggle="tab" href="#dependencias" role="tab" aria-controls="dependencias" aria-selected="false">Registro de Dependencias</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="programas-tab" data-toggle="tab" href="#programas" role="tab" aria-controls="programas" aria-selected="false">Registro de Programas</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="alumnos-tab" data-toggle="tab" href="#alumnos" role="tab" aria-controls="alumnos" aria-selected="false">Registro de Alumnos</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="alumnosprogramas-tab" data-toggle="tab" href="#alumnosprogramas" role="tab" aria-controls="alumnosprogramas" aria-selected="false">Registro de Alumnos a Programas</a>
            </li>
        </ul>
    </div>
         <!-- Campo oculto para almacenar la pestaña activa -->
        <asp:HiddenField ID="HiddenActiveTab" runat="server" />

        <!-- Contenido de las pestañas -->
        <div class="tab-content" id="configTabsContentNew">
            <!-- Pestaña 1: Cargar Imágenes -->
            <div class="tab-pane fade show active" id="imagenes" role="tabpanel" aria-labelledby="imagenes-tab">
                <div class="card card-custom mt-4 p-4">
                    <div class="form-group">
                        <label for="fileUpload">Selecciona una imagen (1580 x 450):</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="fileUpload" runat="server" Accept="image/*" CssClass="custom-file-input" />
                            <label class="custom-file-label" for="fileUpload">Seleccionar imagen</label>
                        </div>
                    </div>

                    <!-- Botón de subir imagen con estilo mejorado -->
                    <asp:Button ID="btnUpload" runat="server" Text="Subir Imagen" OnClick="btnUpload_Click" CssClass="btn btn-primary btn-block mt-3" />

                    <!-- Mensaje de resultado -->
                    <asp:Label ID="lblMessage" runat="server" CssClass="mt-3 text-success" />

                    <!-- Repeater para mostrar las imágenes subidas -->
                    <div class="image-container mt-4">
                        <asp:Repeater ID="rptImages" runat="server">
                            <ItemTemplate>
                                <div class="image-box">
                                    <img src='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("ImageData")) %>' alt="Imagen de página principal" />
                                 <asp:Button ID="btnDelete" runat="server" Text="X" CommandArgument='<%# Eval("IdImage") %>' OnClick="btnDelete_Click" CssClass="btnDelete" OnClientClick="return confirmDelete();" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
  <!-- Pestaña 2: Registro de Dependencias -->
            <div class="tab-pane fade" id="dependencias" role="tabpanel" aria-labelledby="dependencias-tab">
                <div class="card card-custom mt-4 p-4">
                    <div class="form-group">
                        <label for="txtMensaje">Mensaje de cierre a mostrar en registro de Dependencias:</label>
                         <asp:TextBox ID="txtMensaje" runat="server"  CssClass="form-control" style="height: 50px; padding: 8px; font-size: 14px; border-radius: 5px; border: 1px solid #ccc;" Enabled="true"></asp:TextBox>
                    </div>
                  <div class="date-group">
                    <div class="form-group">
                        <label for="txtFechaInicio">Fecha de Inicio:</label>
                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                    </div>

                    <div class="form-group">
                        <label for="txtFechaFin">Fecha de Fin:</label>
                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                    </div>
                </div>


                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" OnClick="btnGuardar_Click" CssClass="btn btn-primary btn-block" OnClientClick="return validarFechas();" />
                    <asp:Label ID="lblMessageDep" runat="server" CssClass="mt-3 text-success" />
                </div>
            </div>
 <!-- Pestaña 3: Registro de Programas -->
           <div class="tab-pane fade" id="programas" role="tabpanel" aria-labelledby="programas-tab">
               <div class="card card-custom mt-4 p-4">
                   <div class="form-group">
                    <label for="txtMensaje2">Mensaje de cierre a mostrar en registro de Programas:</label>
                     <asp:TextBox ID="txtMensaje2" runat="server"  CssClass="form-control" style="height: 50px; padding: 8px; font-size: 14px; border-radius: 5px; border: 1px solid #ccc;" Enabled="true"></asp:TextBox>
                </div>
                 <div class="date-group">
                   <div class="form-group">
                       <label for="txtFechaInicioPro">Fecha de Inicio:</label>
                       <asp:TextBox ID="txtFechaInicioPro" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                   </div>

                   <div class="form-group">
                       <label for="txtFechaFinPro">Fecha de Fin:</label>
                       <asp:TextBox ID="txtFechaFinPro" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                   </div>
               </div>


                   <asp:Button ID="btnGuardarPro" runat="server" Text="Guardar Configuración" OnClick="btnGuardarPro_Click" CssClass="btn btn-primary btn-block" OnClientClick="return validarFechas2();" />
                   <asp:Label ID="lblMessagePro" runat="server" CssClass="mt-3 text-success" />
               </div>
           </div>
 <!-- Pestaña 4: Registro de Alumnos -->
           <div class="tab-pane fade" id="alumnos" role="tabpanel" aria-labelledby="alumnos-tab">
               <div class="card card-custom mt-4 p-4">
                <div class="form-group">
                    <label for="txtMensaje3">Mensaje de cierre a mostrar en registro de Alumnos:</label>
                     <asp:TextBox ID="txtMensaje3" runat="server"  CssClass="form-control" style="height: 50px; padding: 8px; font-size: 14px; border-radius: 5px; border: 1px solid #ccc;" Enabled="true"></asp:TextBox>
                </div>
                 <div class="date-group">
                   <div class="form-group">
                       <label for="txtFechaInicioAlu">Fecha de Inicio:</label>
                       <asp:TextBox ID="txtFechaInicioAlu" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                   </div>

                   <div class="form-group">
                       <label for="txtFechaFinAlu">Fecha de Fin:</label>
                       <asp:TextBox ID="txtFechaFinAlu" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                   </div>
               </div>
                   <asp:Button ID="btnGuardarAlu" runat="server" Text="Guardar Configuración" OnClick="btnGuardarAlu_Click" CssClass="btn btn-primary btn-block" OnClientClick="return validarFechas3();" />
                   <asp:Label ID="lblMessageAlu" runat="server" CssClass="mt-3 text-success" />
               </div>
           </div>
 <!-- Pestaña 5: Registro de Alumnos a Programas -->
           <div class="tab-pane fade" id="alumnosprogramas" role="tabpanel" aria-labelledby="alumnosprogramas-tab">
               <div class="card card-custom mt-4 p-4">
                <div class="form-group">
                    <label for="txtMensaje4">Mensaje de cierre a mostrar en registro de Alumnos a Programas:</label>
                     <asp:TextBox ID="txtMensaje4" runat="server"  CssClass="form-control" style="height: 50px; padding: 8px; font-size: 14px; border-radius: 5px; border: 1px solid #ccc;" Enabled="true"></asp:TextBox>
                </div>
                 <div class="date-group">
                   <div class="form-group">
                       <label for="txtFechaInicioAluProg">Fecha de Inicio:</label>
                       <asp:TextBox ID="txtFechaInicioAluProg" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                   </div>

                   <div class="form-group">
                       <label for="txtFechaFinAluProg">Fecha de Fin:</label>
                       <asp:TextBox ID="txtFechaFinAluProg" runat="server" CssClass="form-control" TextMode="Date" OnClick="abrirCalendario(this);" />
                   </div>
               </div>
                   <asp:Button ID="btnGuardarAluPro" runat="server" Text="Guardar Configuración" OnClick="btnGuardarAluPro_Click" CssClass="btn btn-primary btn-block" OnClientClick="return validarFechas4();" />
                   <asp:Label ID="lblMessageAluPro" runat="server" CssClass="mt-3 text-success" />
               </div>
           </div>
       </div>
</div>
 <!-- Modal exitoso-->
 <div class="modal fade" id="ModalExitoso" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
     <div class="modal-dialog" role="document">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="modalLabel">Registro exitoso</h5>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">&times;</span>
                 </button>
             </div>
             <div class="modal-body">
                 Los datos se han guardado correctamente.
             </div>
             <div class="modal-footer">
                 <button type="button" class="btn btn-primary" data-dismiss="modal" style="background-color:#1073b0;color:white">Aceptar</button>
             </div>
         </div>
     </div>
 </div>
     <!-- Modal errores-->
     <div class="modal fade" id="ModalError" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
         <div class="modal-dialog" role="document">
             <div class="modal-content">
                 <div class="modal-header">
                     <h5 class="modal-title" id="modalLabel1">Datos erroneos</h5>
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                         <span aria-hidden="true">&times;</span>
                     </button>
                 </div>
                 <div class="modal-body">
                     Favor de llenar los datos.
                 </div>
                 <div class="modal-footer">
                     <button type="button" class="btn btn-primary" data-dismiss="modal" style="background-color:#1073b0;color:white">Aceptar</button>
                 </div>
             </div>
         </div>
     </div>
    <!-- Modal de confirmación -->
    <div class="modal fade" id="ModalConfirmacionEliminar" tabindex="-1" role="dialog" aria-labelledby="modalLabelConfirmacion" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabelConfirmacion">Confirmación de Eliminación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    ¿Estás seguro de que deseas eliminar esta imagen?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-danger" id="btnEliminarConfirmado">Eliminar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Scripts de Bootstrap -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</asp:Content>