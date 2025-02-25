<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="CartaTerminacion.aspx.cs" Inherits="Servicio_Social.CartaTerminacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/i18n/datepicker-es.min.js"></script>

    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
<style>
    /* General Styles */
    body {
        font-family: Arial, sans-serif;
        background-color: #f1f3f8;
        margin: 0;
        padding: 0;
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
</style>
    <script>
        $(document).ready(function () {
            // Abre el calendario cuando el campo obtiene el foco
            $("#<%= txtFechaInicio.ClientID %>").on("focus click", function () {
                this.showPicker();  // Función nativa que abre el selector de fechas
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
            <h2 class="titulo-formulario text-center">Carta de Liberación</h2>

            <div class="formulario-container container mt-4">
                <asp:Panel ID="PanelFormulario" runat="server" CssClass="p-3 border rounded bg-light">
                    <div class="form-group">
                        <label for="txtFechaInicio">Fecha Terminación:</label>
                        <div class="d-flex align-items-center">
                            <asp:TextBox ID="txtFechaInicio" required="required" TextMode="Date" runat="server" CssClass="form-control w-50 me-2"></asp:TextBox>
                            <asp:Button ID="btnActualizarFecha" runat="server" Text="Actualizar Fecha" CssClass="btn btn-primary" OnClick="ActualizarFecha_Click" />
                        </div>
                    </div>

                    <!-- HiddenField necesario -->
                    <asp:HiddenField ID="HiddenField1" runat="server" />

                    <div class="form-group mt-4">
                        <iframe id="rptVistaPrevia" runat="server" style="width: 100%; height: 600px;" visible="true" class="border"></iframe>
                    </div>
                </asp:Panel>

                <!-- Campo oculto para Base64 del PDF -->
                <asp:HiddenField ID="hiddenPdfBase64" runat="server" />
            </div>

            <!-- Modal exitoso -->
            <div class="modal fade" id="ModalExitoso" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalLabel">Cambios realizados</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                           Fecha actualizada y reporte generado correctamente.
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
<asp:Content ID="Content3" ContentPlaceHolderID="Encuesta" runat="server">
</asp:Content>
