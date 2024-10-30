<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="PanelEncargadoEscuelaspx.aspx.cs" Inherits="Servicio_Social.PanelEncargadoEscuelaspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <style>
     .card {
         margin-bottom: 20px; /* Espacio entre tarjetas */
         border: none; /* Sin bordes */
         border-radius: 10px; /* Bordes redondeados */
         transition: transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out; /* Transición para efecto hover */
         color: #fff; /* Color del texto blanco */
         cursor: pointer; /* Cursor de puntero para indicar clickeable */
         position: relative; /* Necesario para el texto emergente */
         height: 200px; /* Altura fija para las tarjetas */
         overflow: hidden; /* Evita desbordamiento */
         text-align: center; /* Centrar el contenido */
     }

     .card:hover {
         transform: translateY(-10px); /* Efecto hover */
         box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2); /* Sombra en hover */
     }

     .card-icon {
         font-size: 3.5rem; /* Tamaño del ícono aumentado */
         margin-bottom: 10px; /* Espacio debajo del ícono */
         transition: opacity 0.3s ease-in-out; /* Transición para ocultar el ícono */
     }

     .card-title {
         transition: opacity 0.3s ease-in-out; /* Transición para ocultar el título */
     }

     .card:hover .card-icon,
     .card:hover .card-title {
         opacity: 0; /* Ocultar ícono y título en hover */
     }

     .card-body {
         padding: 30px; /* Espaciado interno */
         position: relative; /* Posicionamiento relativo para contenido dentro de la tarjeta */
     }

     .bg-primary-custom {
         background-color: #4d79ff; /* Azul menos intenso */
     }

     .bg-success-custom {
         background-color: #66cc66; /* Verde menos intenso */
     }

     .bg-warning-custom {
         background-color: #ffcc66; /* Amarillo menos intenso */
     }

     .bg-danger-custom {
         background-color: #ff6666; /* Rojo menos intenso */
     }

     .card-hover-text {
         position: absolute;
         bottom: 20px;
         left: 50%;
         transform: translateX(-50%);
         opacity: 0;
         transition: opacity 0.3s ease-in-out;
         text-align: center;
         font-size: 0.9rem; /* Tamaño de fuente del texto emergente */
         width: 100%; /* Asegura que el texto no se desborde */
         padding: 0 10px; /* Añade espacio de relleno para el texto */
     }

     .card:hover .card-hover-text {
         opacity: 1;
     }
     .bg-lighter {
         background-color: #f8f9fa; /* Este es un tono de gris aún más ligero */
     }
 </style>
    <asp:Panel runat="server" ID="pnlIncio" Visible="true">
    <div class="container mt-5">
        <div class="p-5 mb-4 bg-lighter rounded-3 shadow-sm">
            <div class="container-fluid py-5">
                <h2 class="display-5 fw-bold fs-4 text-center mb-4" style="margin-top: -20px; color: #004085;">Conoce las funcionalidades del Sistema de Servicio Social</h2>
                <div class="row mt-4">
                    <!--Primer tarjeta-->
                    <div class="col-md-4">
                        <div class="card bg-primary-custom" onclick="location.href='#'">
                            <div class="card-body">
                                <i class="fas fa-address-card card-icon"></i>
                                <h5 class="card-title">Perfil</h5>
                                <p class="card-hover-text text-center justify-content-center">Información del Encargado Escuela</p>
                            </div>
                        </div>
                    </div>
                    <!--Segunda tarjeta-->
                       <!-- Programas Registrados -->
                    <div class="col-md-4">
                        <div class="card bg-warning-custom" onclick="location.href='ProgramasRegistrados.aspx'">
                            <div class="card-body">
                                <i class="fas fa-list-alt card-icon"></i>
                                <h5 class="card-title">Programas Registrados</h5>
                                <p class="card-hover-text text-center justify-content-center">Permite la validación y/o autorización de los programas registrados</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card bg-danger-custom" onclick="location.href='AlumnosRegistrados.aspx'"> 
                            <div class="card-body">
                                <i class="fas fa-id-badge card-icon"></i>
                                <h5 class="card-title">Alumnos Registrados a la Plataforma</h5>
                                 <p class="card-hover-text text-center justify-content-center">Permite ver los Alumnos registrados a la Plataforma de Servicio Social</p>
                            </div>
                        </div>
                    </div>
                     <div class="col-md-4">
                         <div class="card bg-success-custom" onclick="location.href='AlumnosPostulados.aspx'"> 
                             <div class="card-body">
                                 <i class="fas fa-user-group card-icon"></i>
                                 <h5 class="card-title">Alumnos Asignados en Programas</h5>
                                  <p class="card-hover-text text-center justify-content-center">Alumnos autorizados por las Dependencias registrados a Programas</p>
                             </div>
                         </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Bootstrap JS y jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</asp:Panel>
</asp:Content>
