<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="ReportesSS.aspx.cs" Inherits="Servicio_Social.ReportesSS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/i18n/datepicker-es.min.js"></script>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <style>
        /* Botón estilizado */
        .boton-generar {
            display: block;
            width: 200px;
            margin: 20px auto;
            padding: 12px;
            font-size: 16px;
            font-weight: bold;
            background-color: #2e5e8a;
            color: #ffffff;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .boton-generar:hover {
            background-color: #234d72;
        }

        /* Contenedor centrado del iframe */
        .iframe-container {
            display: flex;
            justify-content: center;
           /* align-items: center;
            width: 100%;
            height: calc(100vh - 150px);*/ /* Ajustar al alto de la pantalla menos el espacio del encabezado */
            /*margin-top: 20px;
            overflow: hidden;*/
        }

        /* Estilos del iframe */
        #pdfIframe {
            width: 80%; /* Hacer el documento más grande ocupando el 80% del ancho */
            height: 100%;
            border: 2px solid #ccc;
            border-radius: 8px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <div class="container text-center">
        <h2 class="my-4">Generar Reporte</h2>
        <div class="row justify-content-center">
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
            </div>
        </div>
        <asp:Button ID="btnGenerar" CssClass="boton-generar" runat="server" Text="Generar Reporte" OnClick="btnGenerar_Click" />
    </div>
    <!-- Contenedor centrado para el iframe -->
    <div style="display: flex; justify-content: center; align-items: center; margin-top: 20px;">
    <iframe 
        id="pdfIframe" 
        runat="server" 
        style="width: 90%; height: 85vh; border: 2px solid #ccc; border-radius: 8px; box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);">
    </iframe>
</div>
</asp:Content>