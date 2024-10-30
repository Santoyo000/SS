<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SitioMantenimiento.aspx.cs" Inherits="Servicio_Social.SitioMantenimiento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>UADEC Trabajo Social - Mantenimiento</title>
    <link rel="stylesheet" href="bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link rel="stylesheet" type="text/css" href="css/Estilos.css" />
    <link rel="stylesheet" href="assets/css/templatemo.css">
    <link rel="stylesheet" href="assets/css/custom.css">
    <script src="assets/js/templatemo.js"></script>
    <script src="assets/js/custom.js"></script>
    <link rel="shortcut icon" type="image/x-icon" href="Imagen/icono_uadec.png" />
    <style>
        body {
            background-color: #f8f9fa; /* Fondo gris claro */
            color: #333;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
            display: flex;
            flex-direction: column;
            min-height: 100vh;
        }

        .navbar-lower {
            background-color: #2E5790; /* Azul fuerte */
            width: 100%;
            position: fixed;
            top: 0;
            left: 0;
            z-index: 1000; /* Asegura que la barra de navegación esté sobre otros contenidos */
        }

        .navbar-lower .navbar-nav .nav-link {
            color: #ffffff; /* Color blanco para el texto de las opciones */
        }

        .navbar-lower .navbar-toggler-icon {
            background-color: #ffffff; /* Color blanco para el icono del botón de hamburguesa */
        }

        .maintenance-container {
            text-align: center;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.2);
            max-width: 600px;
            margin: 100px auto; /* Margen superior para que el contenido esté debajo de la barra de navegación */
        }

        .maintenance-container img {
            max-width: 80%;
            height: auto;
            display: block;
            margin: 20px auto 0 auto;
        }

        .maintenance-message {
            font-size: 28px;
            font-weight: bold;
            color: #003366; /* Azul marino */
            margin-bottom: 20px;
            text-transform: uppercase;
        }

        .maintenance-description {
            font-size: 18px;
            color: #555;
            margin-top: 0;
            margin-bottom: 30px;
        }

        .btn-return {
            display: inline-block;
            font-size: 16px;
            font-weight: bold;
            color: #ffffff;
            background-color: #007bff;
            padding: 10px 20px;
            border-radius: 5px;
            text-decoration: none;
            transition: background-color 0.3s ease;
        }

        .btn-return:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light navbar-lower">
            <div class="container">
                <!-- Brand y botón de hamburguesa -->
                <a class="navbar-brand">
                    <img src="assets/img/UAdeC-blanco.png" alt="Logo" style="max-width: 100px;">
                </a>
            </div>
        </nav>

        <!-- Sección de mantenimiento -->
        <div class="maintenance-container">
            <div class="maintenance-message">
                Estamos realizando mejoras en nuestro sitio web
            </div>
            <div class="maintenance-description">
                Estamos trabajando para ofrecerte una mejor experiencia. Favor de consultar a partir del martes 29 de Octubre a las 08:00 a.m.
            </div>
             <img src="/Image/mantenimiento.jpeg" alt="Mantenimiento">
           
           
        </div>
    </form>
</body>
</html>