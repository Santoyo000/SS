<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="InformeEstudiante.aspx.cs" Inherits="Servicio_Social.InformeEstudiante" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Web" TagPrefix="cr" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <cr:CrystalReportViewer ID="crvInformeEstudiante" runat="server" AutoDataBind="true" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/i18n/datepicker-es.min.js"></script>

    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script>
        $(document).ready(function () {
            // Abre el calendario cuando el campo obtiene el foco
            $("#<%= txtFechaInicio.ClientID %>").on("focus click", function () {
                this.showPicker();  // Función nativa que abre el selector de fechas
            });

            $("#<%= txtFechaFin.ClientID %>").on("focus click", function () {
                this.showPicker();  // Función nativa que abre el selector de fechas
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Cargar el PDF en el iframe cuando se abre el modal
            $('#pdfModal').on('shown.bs.modal', function () {
                var pdfBase64 = document.getElementById('<%= hiddenPdfBase64.ClientID %>').value;
            var pdfIframe = document.getElementById("pdfIframe");
            pdfIframe.src = "data:application/pdf;base64," + pdfBase64;
        });
    });
</script>
<script type="text/javascript">
    function mostrarOverlay() {
        document.getElementById("loadingOverlay").style.display = "block";
    }

    function ocultarOverlay() {
        document.getElementById("loadingOverlay").style.display = "none";
    }

    // Ocultar overlay cuando la página se recargue (por ejemplo, después del postback)
    if (window.addEventListener) {
        window.addEventListener('load', ocultarOverlay, false);
    } else if (window.attachEvent) {
        window.attachEvent('onload', ocultarOverlay);
    }
</script>
<style>
    /* General Styles */
    body {
        font-family: Arial, sans-serif;
        background-color: #f1f3f8;
        margin: 0;
        padding: 0;
    }

    /* Header */
    .titulo-formulario, .instrucciones {
        text-align: center;
        color: #333;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        margin-bottom: 15px;
    }

    .titulo-formulario {
        font-size: 1.8em;
        font-weight: 700;
        color: #2e5e8a;
    }

    /* Form Container */
    .formulario-container {
        width: 60%;
        margin: 20px auto;
        padding: 25px;
        background-color: #ffffff;
        border-radius: 10px;
        box-shadow: 0 6px 18px rgba(0, 0, 0, 0.15);
    }

    /* Table Styling */
    table {
        width: 100%;
        margin-top: 20px;
    }

    table td {
        padding: 10px;
        vertical-align: middle;
        font-size: 14px;
    }

    table td:first-child {
        width: 26%;
        color: #555;
        font-weight: 600;
        text-align: left;
    }

    /* Input Fields */
    input[type="text"], textarea, input[type="date"] {
        width: 100%; /* Cambia el ancho a 100% para que ocupe todo el contenedor */
        max-width: 600px; /* Establece un ancho máximo si lo deseas */
        padding: 8px;
        border: 1px solid #d0d0d0;
        border-radius: 4px;
        box-sizing: border-box; /* Asegura que el padding se incluya en el ancho total */
        font-size: 14px;
        color: #4a4a4a;
        background-color: #fafafa;
       /* margin-bottom: 10px;*/ /* Añade un poco de espacio entre los campos */
    }

    input[type="text"]:focus, textarea:focus, input[type="date"]:focus {
        border-color: #2e5e8a;
        outline: none;
    }

    input[readonly], textarea[readonly] {
        background-color: #e9ecef;
        color: #6c757d;
    }

    /* Button Styling */
    .boton-guardar {
        width: 100%;
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

    .boton-guardar:hover {
        background-color: #234d72;
    }

    /* Date Fields */
    .fechas {
        display: flex;
        justify-content: space-between;
      /*  margin-bottom: 15px;*/
    }

    .fechas div {
        width: 48%;
    }

    .fechas label {
        display: block;
        margin-bottom: 5px;
        color: #4a4a4a;
        font-weight: 600;
    }

    /* Modal Styling */
    .modal-content {
        border-radius: 8px;
        padding: 15px;
        background-color: #f9f9f9;
        color: #333;
    }

    .modal-header, .modal-footer {
        border: none;
        padding: 10px;
    }

    .modal-title {
        font-size: 18px;
        font-weight: bold;
        color: #333;
    }

    .close {
        font-size: 20px;
        color: #666;
    }

    .modal-body {
        padding: 20px;
    }

    .modal-footer button {
        background-color: #1073b0;
        color: #fff;
        border: none;
        padding: 10px 20px;
        border-radius: 4px;
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

    .modal-footer button:hover {
        background-color: #0c5a8c;
    }

    /* Loading Overlay */
    #loadingOverlay {
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(255, 255, 255, 0.85);
        z-index: 1000;
        text-align: center;
        font-size: 16px;
    }

    #loadingOverlay img {
        max-width: 80px;
        margin-top: 20%;
    }

    /* Periodo de Realización Styling */
.periodo-realizacion {
    text-align: center;
    color: #2e5790;
    font-weight: bold;
    margin: 20px 0;
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<!-- Overlay de carga -->
<div id="loadingOverlay" style="display:none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(255, 255, 255, 0.8); z-index: 1000; text-align: center;">
    <img src="Image/loading.gif" alt="Generando formato..." style="max-width: 300px; margin-top: 20%;">
    <div>Generando formato, por favor espere...</div>
</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <h2 class="titulo-formulario">Informe del Estudiante</h2>
            <p class="instrucciones">Favor de llenar la información siguiente</p>
            <p class="instrucciones">Una vez guardado, imprimir, firmar y presentar al Encargado de Escuela de tu facultad.</p>

            <div class="formulario-container">
                <asp:Panel ID="PanelFormulario" runat="server">
                    <table>
                        <tr>
                            <td>Nombre del prestador:</td>
                            <td>
                                <asp:TextBox ID="txtNombrePresentador"  ReadOnly="true" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Nombre del programa:</td>
                            <td>
                                <asp:TextBox ID="txtNombrePrograma"  ReadOnly="true" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Responsable del Programa:</td>
                            <td>
                                <asp:TextBox ID="txtResponsablePrograma"  ReadOnly="true" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <!-- Centramos el texto del periodo -->
                    <div class="periodo-realizacion" style="color: #2e5790" font-weight: bold>
                        Periodo de realización del Servicio Social:
                    </div>

                    <!-- Usamos flexbox para las fechas -->
                    <div class="fechas">
                        <div>
                            <label for="txtFechaInicio">Fecha inicio:</label>
                            <asp:TextBox ID="txtFechaInicio" required="required" TextMode="Date" runat="server"></asp:TextBox>

                        </div>
                        <div>
                            <label for="txtFechaFin">Fecha fin:</label>
                            <asp:TextBox ID="txtFechaFin"  required="required" type="date" runat="server"></asp:TextBox>

                        </div>
                    </div>

                    <table>
                        <tr>
                            <td>Actividades desarrolladas:</td>
                            <td>
                                <asp:TextBox ID="txtActividades" runat="server"  required="required" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Población beneficiada con el programa:</td>
                            <td>
                                <asp:TextBox ID="txtPoblacionBeneficiada"  required="required" TextMode="MultiLine" Rows="4" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Metas y/o resultados obtenidos:</td>
                            <td>
                                <asp:TextBox ID="txtMetasResultados"  required="required" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:Button ID="btnGuardar" CssClass="boton-guardar" runat="server" Text="Guardar y Generar Formato" OnClick="btnGuardar_Click" OnClientClick="mostrarOverlay();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:HiddenField ID="hiddenPdfBase64" runat="server" />
                        <%--<div class="card-body p-3">
            <iframe id="rptVistaPrevia" runat="server" style="width: 100%; height: 600px;" visible="true"></iframe>
        </div>--%>
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
                            <button type="button" class="btn btn-primary" data-dismiss="modal" style="background-color: #1073b0; color: white">Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>
           <!-- Modal PDF -->
<div class="modal fade" id="pdfModal" tabindex="-1" role="dialog" aria-labelledby="pdfModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="pdfModalLabel">Informe Final del Servicio Social</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <iframe id="pdfIframe" style="width: 100%; height: 500px; border: none;" frameborder="0"></iframe>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
            </div>
        </div>
    </div>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
