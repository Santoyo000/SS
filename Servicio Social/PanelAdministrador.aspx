<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="PanelAdministrador.aspx.cs" Inherits="Servicio_Social.PanelAdministrador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script>
        window.onload = function () {
            document.getElementById('overlay').style.display = 'none'; // Ocultar el overlay al cargar la página
        };
        window.addEventListener('pageshow', function (event) {
            if (event.persisted) {
                document.getElementById('overlay').style.display = 'none'; // Ocultar el overlay si la página es cargada desde caché
            }
        });
        function showLoadingAndRedirect(url) {
            document.getElementById('overlay').style.display = 'flex'; // Mostrar el overlay
            setTimeout(function () {
                window.location.href = url; // Redirigir después de un breve retraso
            }, 500); // Retraso opcional de 500ms
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">

    <asp:Panel runat="server" ID="pnlIncio" Visible="true">
        <div class="container mt-5">
            <div class="p-5 mb-4 bg-lighter rounded-3 shadow-sm">
                <div class="container-fluid py-5">
                    <h2 class="display-5 fw-bold fs-4 text-center mb-4" style="margin-top: -20px; color: #004085;">Conoce las funcionalidades del Sistema de Servicio Social</h2>
                    <div class="row mt-4">
                        <!-- Home -->
                        <!-- <div class="col-md-4">
                            <div class="card bg-primary-custom" onclick="location.href='#'">
                                <div class="card-body">
                                    <i class="fas fa-home card-icon"></i>
                                    <h5 class="card-title">Home</h5>
                                    <p class="card-hover-text">Muestra un resumen de las funcionalidades del Sistema de Servicio Social.</p>
                                </div>
                            </div>
                        </div> -->
                        <!-- Gestión de Usuarios -->
                        <div class="col-md-4">
                            <div class="card bg-primary-custom" onclick="showLoadingAndRedirect('UsuariosRegistrados.aspx')">
                                <div class="card-body">
                                    <i class="fas fa-solid fa-users-gear card-icon"></i>
                                    <h5 class="card-title">Gestión de Usuarios</h5>
                                    <p class="card-hover-text text-center justify-content-center">Permite modificar los accesos de las Dependencias y realizar la autorización de los usuarios</p>
                                </div>
                            </div>
                        </div>
                        <!-- Dependencias Registradas -->
                        <div class="col-md-4">
                            <div class="card bg-success-custom" onclick="showLoadingAndRedirect('DependenciasRegistradas.aspx')">
                                <div class="card-body">
                                    <i class="fas fa-building card-icon"></i>
                                    <h5 class="card-title">Dependencias Registradas</h5>
                                    <p class="card-hover-text text-center justify-content-center">Permite la validación y/o autorización de las dependencias registradas para brindar su acceso al Sistema</p>
                                </div>
                            </div>
                        </div>
                        <!-- Programas Registrados -->
                        <div class="col-md-4">
                            <div class="card bg-warning-custom" onclick="showLoadingAndRedirect('ProgramasRegistrados.aspx')">
                                <div class="card-body">
                                    <i class="fas fa-list-alt card-icon"></i>
                                    <h5 class="card-title">Programas Registrados</h5>
                                    <p class="card-hover-text text-center justify-content-center">Permite la validación y/o autorización de los programas registrados</p>
                                </div>
                            </div>
                        </div>
                        <!-- Alumnos Registrados -->
                        <div class="col-md-4">
                            <div class="card bg-danger-custom" onclick="showLoadingAndRedirect('AlumnosRegistrados.aspx')" >
                                <%--AlumnosRegistrados.aspx--%>
                                <div class="card-body">
                                    <i class="fas fa-id-badge card-icon"></i>
                                    <h5 class="card-title">Alumnos Registrados</h5>
                                    <p class="card-hover-text text-center justify-content-center">Listado de alumnos registrados a la Plataforma de Servicio Social</p>
                                </div>
                            </div>
                        </div>
                        <!-- Alumnos Registrados a Programas-->
                        <div class="col-md-4">
                            <div class="card bg-danger-custom" style="background-color: #78288C;" onclick="showLoadingAndRedirect('AlumnosPostulados.aspx')">
                                <%--AlumnosRegistrados.aspx--%>
                                <div class="card-body">
                                    <i class="fas fa-user-group card-icon"></i>
                                    <h5 class="card-title">Alumnos Asignados en Programas</h5>
                                    <p class="card-hover-text text-center justify-content-center">Alumnos Asignados en Programas</p>
                                </div>
                            </div>
                        </div>
                         <!-- Configuración de página -->
                   <%--  <div class="col-md-4">
                         <div class="card" style="background-color: #09aaa1;" onclick="showLoadingAndRedirect('ConfiguracionPagina.aspx')">
                             <div class="card-body">
                                 <i class="fas fa-screwdriver-wrench card-icon"></i>
                                 <h5 class="card-title">Configuración de página</h5>
                                 <p class="card-hover-text text-center justify-content-center"> Configuración de la página de Servicio Social</p>
                             </div>
                         </div>
                     </div>--%>
                        <!-- Configuración de página -->
                      <%--  <div class="col-md-4">
                            <div class="card" style="background-color: #6f42c1;"" > --%> <%--onclick="showLoadingAndRedirect('ReportesSS.aspx') " --%>
                               <%-- <div class="card-body">
                                    <i class="fas fa-chart-bar card-icon"></i>
                                    <h5 class="card-title">Reportes</h5>
                                    <p class="card-hover-text text-center justify-content-center">Crea reportes personalizados sobre diversas áreas del sistema, incluyendo dependencias y programas (actualmente en desarrollo)</p>
                                </div>
                            </div>
                        </div>--%>
                       <%-- <div class="col-md-4">
                        <div class="card" style="background-color: #6f42c1;" onclick="showLoadingAndRedirect('DetallePrograma.aspx')"> --%> <%--onclick="showLoadingAndRedirect('ReportesSS.aspx') " --%>
                           <%-- <div class="card-body">
                                <i class="fas fa-chart-bar card-icon"></i>
                                <h5 class="card-title">Detalle Programa</h5>
                                <p class="card-hover-text text-center justify-content-center">Crea reportes personalizados sobre diversas áreas del sistema, incluyendo dependencias y programas (actualmente en desarrollo)</p>
                            </div>
                        </div>--%>
                   <%-- </div>--%>
                    </div>
                </div>
            </div>
        </div>
        <div id="overlay" style="display: none">
            <div id="loadingContent">
                <img id="imgWaitIcon" src="Image/loading.gif" alt="Cargando..." style="max-width: 300px;" />
                <div id="loadingText">Por favor, espere...</div>
            </div>
        </div>
        <!-- Bootstrap JS y jQuery -->
        <%-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
                <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>--%>
    </asp:Panel>
</asp:Content>
