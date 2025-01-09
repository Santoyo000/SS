<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="LiberarEstudiante.aspx.cs" Inherits="Servicio_Social.LiberarEstudiante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/i18n/datepicker-es.min.js"></script>

    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" integrity="sha384-dy7oWpw7o+K48KZIZ4yEFbGrOh6e3ty3RmCpKmoJxXys7p6IB0Qy1FU4D9nt4XTR" crossorigin="anonymous">
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
    <contenttemplate>
        <br />
        <h2 class="titulo-formulario">Liberar Estudiante</h2>
        <p class="instrucciones">Puede usar el siguiente formato de ejemplo de Constancia de Terminación dando Click en el botón DescargarFormatoWord </p>
        <p class="instrucciones">impreso en papel oficial de la dependencia, firmado y sellado </p>
        <%--<p class="instrucciones">Una vez guardado, imprimir, firmar y presentar al Encargado de Escuela de tu facultad.</p>--%>

        <div class="formulario-container">
            <asp:Panel ID="PanelLiberar" runat="server">
                <table>
                    <tr>
                        <td>Nombre del prestador:</td>
                        <td>
                            <asp:TextBox ID="txtNombrePresentador" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Nombre del programa:</td>
                        <td>
                            <asp:TextBox ID="txtNombrePrograma" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Responsable del Programa:</td>
                        <td>
                            <asp:TextBox ID="txtResponsablePrograma" ReadOnly="true" runat="server"></asp:TextBox>
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
                        <asp:TextBox ID="txtFechaFin" required="required" type="date" runat="server"></asp:TextBox>

                    </div>
                </div>
                <div class="text-center mt-4">
                                    <!-- Botón Descargar Formato Word con ícono -->
                <asp:LinkButton ID="btnDescargarFormatoWord" runat="server" CssClass="btn btn-success" OnClick="btnDescargarFormatoWord_Click">
    <i class="fas fa-file-word"></i> Descargar Formato Word
</asp:LinkButton>
                    <!-- Botón Liberar Estudiante -->
                    <asp:LinkButton ID="btnLiberarEstudiante" runat="server" CssClass="btn btn-primary mr-2" OnClick="btnLiberarEstudiante_Click"
                       OnClientClick="mostrarModalConfirmacion(); return false;">
        Liberar Estudiante
    </asp:LinkButton>

                   
                </div>

            </asp:Panel>
            <asp:HiddenField ID="hiddenPdfBase64" runat="server" />
            <%--        <div class="card-body p-3">
            <iframe id="rptVistaPrevia" runat="server" style="width: 100%; height: 600px;" visible="true"></iframe>
        </div>--%>
        </div>
    <!-- Modal de confirmación -->
<!-- Modal de Confirmación -->
<div class="modal fade" id="ModalConfirmacion" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalLabel">Confirmación de Liberación</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                ¿Estás seguro de que deseas liberar al estudiante?
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-primary" style="background-color: #1073b0; color: white" onclick="confirmarLiberacion()">Aceptar</button>
            </div>
        </div>
    </div>
</div>

<!-- Script JavaScript para abrir el modal y redirigir después de la confirmación -->
<script type="text/javascript">
    // Función para abrir el modal de confirmación
    function mostrarModalConfirmacion() {
        $('#ModalConfirmacion').modal('show');
    }

    // Función para confirmar la liberación y ejecutar el método del servidor
    function confirmarLiberacion() {
        // Llama al método del servidor usando __doPostBack
        __doPostBack('<%= btnLiberarEstudiante.UniqueID %>', '');

        // Escucha el cierre del modal para redirigir
        $('#ModalConfirmacion').on('hidden.bs.modal', function () {
            window.location.href = 'AlumnosPostulados.aspx';
        });

        // Cierra el modal después de la confirmación
        $('#ModalConfirmacion').modal('hide');
    }
</script>

        <!-- Modal datos faltantes-->
<div class="modal fade" id="ModalDatosFaltantes" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
    <<div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="faltantemodalLabel">Datos incompletos</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                Por favor, complete las fechas de inicio y fin del servicio social antes de generar el documento.
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
    </contenttemplate>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Encuesta" runat="server">
</asp:Content>
