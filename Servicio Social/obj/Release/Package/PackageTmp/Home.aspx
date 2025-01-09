<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Servicio_Social.Home" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Servicio Social UADEC</title>
    <!-- Bootstrap CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <link rel="shortcut icon" type="image/x-icon" href="assets/img/logoUAdeC.png" />

    <!-- Estilos personalizados -->
    <style>
        /* Navbar superior */
        .navbar-upper {
            background-color: #343a40; /* Gris oscuro */
            padding-top: 5px;
            padding-bottom: 5px;
        }

            .navbar-upper .navbar-text {
                color: #ffffff; /* Color blanco */
            }

        /* Navbar inferior */
        .navbar-lower {
            background-color: #2E5790; /* Azul fuerte */
            z-index: 9999; /* Asegura que el menú esté por encima de otros elementos */
        }

            .navbar-lower .navbar-nav .nav-link {
                color: #ffffff; /* Color blanco para el texto de las opciones */
            }

            .navbar-lower .navbar-toggler-icon {
                background-color: #ffffff; /* Color blanco para el icono del botón de hamburguesa */
            }


        .card {
            margin-bottom: 20px; /* Espacio entre tarjetas */
            border: none; /* Sin bordes */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* Sombra sutil */
            border-radius: 10px; /* Bordes redondeados */
            transition: transform 0.3s ease-in-out; /* Transición para efecto hover */
        }

            .card:hover {
                transform: translateY(-10px); /* Efecto hover */
            }

        .card-img-top {
            border-top-left-radius: 10px; /* Bordes redondeados */
            border-top-right-radius: 10px; /* Bordes redondeados */
            height: 200px; /* Altura fija para las imágenes */
            object-fit: cover; /* Ajuste de las imágenes */
        }

        .card-title {
            color: #2E5790; /* Color del título */
            font-weight: bold; /* Negrita */
        }

        .card-text {
            color: #6c757d; /* Color del texto */
        }

        .card-body {
            padding: 20px; /* Espaciado interno */
        }

        /* Estilos personalizados para el carrusel */
        body {
            font-family: 'Arial', sans-serif;
        }

        .carousel-item {
            display: none;
            transition: transform 0.6s ease-in-out;
            -webkit-backface-visibility: hidden;
            backface-visibility: hidden;
            position: relative;
        }

            .carousel-item.active {
                display: flex;
                align-items: center;
            }

        .carousel-text {
            background-color: #f8f9fa;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

            .carousel-text h5 {
                font-weight: bold;
                color: #333;
                margin-bottom: 20px;
            }

            .carousel-text p {
                color: #666;
                line-height: 1.6;
            }

        .carousel-img img {
            max-width: 100%;
            height: auto;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .carousel-control-prev,
        .carousel-control-next {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            width: 5%;
            z-index: 2;
        }

        .carousel-control-prev {
            left: 10px;
        }

        .carousel-control-next {
            right: 10px;
        }

        .carousel-control-prev-icon,
        .carousel-control-next-icon {
            background-color: rgba(0, 0, 0, 0.5);
            border-radius: 50%;
            padding: 10px;
        }

        .carousel-indicators {
            position: absolute;
            bottom: -30px;
            left: 50%;
            transform: translateX(-50%);
        }

            .carousel-indicators li {
                background-color: black;
            }

        /* Estilos para el footer */
        .footer {
            background-color: #000000; /* Fondo negro */
            color: #ffffff; /* Texto blanco */
            padding: 20px 0; /* Espaciado arriba y abajo */
            text-align: center; /* Centrar el texto */
        }

        /* Estilos de animación */
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* Clases para activar animación */
        .animated {
            opacity: 0; /* Elementos ocultos inicialmente */
            animation: fadeInUp 0.8s ease-out forwards; /* Duración y suavidad de animación */
        }

        /* Retrasos para cada sección (ajustados a tiempos más rápidos) */
        #navbarUpper.animated { animation-delay: 0.1s; }
        #navbarLower.animated { animation-delay: 0.3s; }
        #myCarousel.animated { animation-delay: 0.5s; }
        #cardContainer .card.animated { animation-delay: 0.7s; }
        #footer.animated { animation-delay: 0.9s; }
   
    </style>

</head><!-- Navbar inferior con menú desplegable -->
<body>
    <!-- Navbar superior -->
    <nav class="navbar navbar-expand-lg navbar-light navbar-upper animated" id="navbarUpper">
        <div class="container">
            <!-- Contactos alineados a la izquierda -->
            <span class="navbar-text mr-auto text-white">
                Teléfono: (844) 412 4477 | Email: serviciosocial@uadec.edu.mx
            </span>
        </div>
    </nav>

    <!-- Navbar inferior con menú desplegable -->
    <nav class="navbar navbar-expand-lg navbar-light navbar-lower animated " id="navbarLower">
        <div class="container">
            <!-- Brand y botón de hamburguesa -->
            <a class="navbar-brand" href="Home.aspx">
                <img src="assets/img/logoUAdeC.png" alt="Logo" style="max-width: 140px;">
            </a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <!-- Menú desplegable -->
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="LoginAdministrador.aspx">Administrador</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="LoginResponsableUnidad.aspx">Responsable Unidad</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="LoginEncargadoEscuela.aspx">Encargado Escuela</a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown4" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Dependencia
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown4">
                            <a class="dropdown-item" href="LoginDependencia.aspx">Inicia sesión</a>
                            <a class="dropdown-item" href="RegistroDependencia.aspx">Regístrate</a>
                        </div>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown5" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Estudiantes
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown5">
                            <a class="dropdown-item" href="LoginEstudiante.aspx">Inicia sesión</a>
                            <a class="dropdown-item" href="RegistroEstudiante.aspx">Regístrate</a>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <br />

   <div id="myCarousel" class="carousel slide animated" data-ride="carousel">
    <div class="carousel-inner" runat="server" id="carouselInner">
        <asp:Literal ID="carouselLiteral" runat="server"></asp:Literal>
    </div>
    <!-- Botones de control y indicadores -->
    <a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
    </a>
    <a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
    </a>
    <ol class="carousel-indicators">
        <asp:Literal ID="carouselIndicators" runat="server"></asp:Literal>
    </ol>
</div>
    <!-- Botones de control -->
   <%-- <a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
    </a>
    <a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
    </a>--%>
    <!-- Indicadores -->
<%--    <ol class="carousel-indicators">
        <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
        <li data-target="#myCarousel" data-slide-to="1"></li>
        <li data-target="#myCarousel" data-slide-to="2"></li>
    </ol>--%>
    <div class="container mt-5  animated" id="cardContainer">
        <div class="row">
            <!--Primer tarjeta-->
            <div class="col-md-3">
                <div class="card ">
                    <div class="map-container">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d28826.22417714386!2d-100.976662!3d25.428967!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x86880d520e241d55%3A0x83b4e4dfe30412b9!2sUAdeC%20Unidad%20Campo%20Redondo!5e0!3m2!1ses-419!2smx!4v1715652731584!5m2!1ses-419!2smx" width="100%" height="200" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Admon. central</h5>
                        <p class="card-text">Mtro. Marco Antonio Contreras B.</p>
                        <p class="card-text">Edificio G Planta Baja Unidad Camporredondo, Saltillo, Coah., MX.</p>
                    </div>
                </div>
            </div>
            <!--Segunda tarjeta-->
            <div class="col-md-3">
                <div class="card ">
                    <div class="map-container">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d28826.22417714386!2d-100.976662!3d25.428967!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x86880d520e241d55%3A0x83b4e4dfe30412b9!2sUAdeC%20Unidad%20Campo%20Redondo!5e0!3m2!1ses-419!2smx!4v1715654975153!5m2!1ses-419!2smx" width="100%" height="200" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Unidad Saltillo</h5>
                        <p class="card-text">Mario Alberto Najera Hernandez</p>
                        <p class="card-text">Edificio G Planta Alta Unidad Camporredondo,Saltillo, Coah., MX.</p>
                    </div>
                </div>
            </div>
            <!--Tercera tarjeta-->
            <div class="col-md-3">
                <div class="card ">
                    <div class="map-container">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d57601.41206836839!2d-103.442607!3d25.535437!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x868fdbdd84a6f3c3%3A0x5b49bb5310e6efcd!2sUniversidad%20Aut%C3%B3noma%20de%20Coahuila!5e0!3m2!1ses-419!2smx!4v1715655060961!5m2!1ses-419!2smx" width="100%" height="200" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Unidad Torreón</h5>
                        <p class="card-text">L.D.E. Jesús Saucedo Orona</p>
                        <p class="card-text">Blvd. Revolución e Ignacio Comonfort, Zona Centro, Torreón, Coah, MX.</p>
                    </div>
                </div>
            </div>
            <!--Cuarta tarjeta-->
            <div class="col-md-3">
                <div class="card ">
                    <div class="map-container">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d56907.11495453538!2d-101.416461!3d26.944892!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x868bcc3495a98fc7%3A0x481176202fae884e!2sU.%20A.%20de%20C.!5e0!3m2!1ses-419!2smx!4v1715655103581!5m2!1ses-419!2smx" width="100%" height="200" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Unidad Norte</h5>
                        <p class="card-text">Lic. Brenda Sofía Zamora Rosales</p>
                        <p class="card-text">Carretera 57 Km. 5 Zona Universitaria, Monclova, Coah. MX.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer class="footer animated" id="footer">
        <div class="container d-flex justify-content-start">
            <%--<span id="year"></span> UAdeC | Designed by --%>
            <img src="assets/img/03.png" alt="Logo adicional" style="max-width: 250px; margin-left: 10px;"> 
        </div>
    </footer>

    <!-- Bootstrap JS y jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <script>
        // Activar movimiento automático del carrusel
        $('.carousel').carousel({
            interval: 5000 // Tiempo en milisegundos entre transiciones (ejemplo: 5000 = 5 segundos)
        });

        $(document).ready(function () {
            var year = new Date().getFullYear();
            $("#year").text(year);
        });
    </script>
</body>
</html>


