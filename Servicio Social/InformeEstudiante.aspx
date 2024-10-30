<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="InformeEstudiante.aspx.cs" Inherits="Servicio_Social.InformeEstudiante" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Web" TagPrefix="cr" %>
<cr:CrystalReportViewer ID="crvInformeEstudiante" runat="server" AutoDataBind="true" />

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            $("#<%= txtFechaInicio.ClientID %>").on("focus click", function() {
            this.showPicker();  // Función nativa que abre el selector de fechas
        });
        
        $("#<%= txtFechaFin.ClientID %>").on("focus click", function () {
            this.showPicker();  // Función nativa que abre el selector de fechas
        });
    });
</script>
<style>
        /* Centrar el título y los párrafos */
        .titulo-formulario, .instrucciones {
            text-align: center;
            font-family: Arial, sans-serif;
            color: #2e5790;
        }

        /* Estilo general del formulario */
        .formulario-container {
            width: 60%;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        table {
            width: 100%;
            margin-top: 10px;
        }

        table td {
            padding: 10px;
            vertical-align: top;
        }

        table td:first-child {
            width: 30%;
            text-align: right;
            padding-right: 10px;
        }

        table td:nth-child(2) {
            width: 70%;
        }

        /* Estilo para los inputs */
        input[type="text"], textarea {
            width: 100%;
            padding: 6px;
            margin: 4px 0;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
            font-size: 14px;
        }

        /* Estilo para el botón */
        .boton-guardar {
            width: 100%;
            padding: 10px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .boton-guardar:hover {
            background-color: #45a049;
        }

        /* Centrar el texto del periodo */
        .periodo-realizacion {
            text-align: center;
            font-weight: bold;
            font-size: 16px;
            margin: 20px 0;
        }

        /* Flexbox para fechas: una a la izquierda y otra a la derecha */
        .fechas {
            display: flex;
            justify-content: space-between; /* Separar los elementos */
            align-items: flex-start; /* Alinearlos hacia arriba */
            margin-bottom: 20px; /* Añadir un poco de espacio debajo */
        }

        .fechas div {
            width: 48%; /* Cada fecha ocupa aproximadamente la mitad del contenedor */
        }

        .fechas label {
            display: block;
            margin-bottom: 5px;
            /*font-weight: bold;*/
        }
        /* Clase para reducir el ancho de los campos de fecha */
        .fecha-reducida {
            width: 60%; /* Ajusta este valor según el tamaño deseado */
            padding: 6px;
            font-size: 12px; /* Puedes ajustar este valor si también quieres modificar el tamaño de la letra */
         }
        .txt-estilo {
       /* box-shadow: inset #abacaf 0 0 0 0px;*/ /* Reducido el tamaño de la sombra */
       /* border: 1px solid transparent;*/ /* Grosor del borde reducido */
        /*background: rgba(0, 0, 0, 0);*/
        /*width: 120%;*/
        position: relative;
        /*border-radius: 3px;*/
        /*padding: 9px 12px;*/
        line-height: 1.4;
        color: rgb(0, 0, 0);
        font-size: 12px;
        font-weight: 400;
        /*height: 50px; *//* Ajuste de la altura */
        transition: all .2s ease;
    }

        .txt-estilo:hover {
            box-shadow: 0 0 0 0 #fff inset, #1de9b6 0 0 0 2px;
            border-color: #1de9b6; /* Cambia el color del borde al pasar el mouse */
        }

        .txt-estilo:focus {
            background: #fff;
            outline: 0;
            box-shadow: 0 0 0 0 #fff inset, #1de9b6 0 0 0 3px;
            border-color: #1de9b6; /* Cambia el color del borde al enfocar */
        }
        /* Selector específico para TextMode="MultiLine" (textarea) */
        textarea.txt-estilo {
            font-size: 14px; /* Ajuste específico para los campos multilinea */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
    <br />
    <h2 class="titulo-formulario" >Informe del Estudiante</h2>
    <p class="instrucciones">Favor de llenar la información siguiente</p>
    <p class="instrucciones">Una vez guardado, imprimir, firmar y presentar al Encargado de Escuela de tu facultad.</p>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="formulario-container">
        <asp:Panel ID="PanelFormulario" runat="server">
            <table>
                <tr>
                    <td>Nombre del prestador:</td>
                    <td>
                        <asp:TextBox ID="txtNombrePresentador" CssClass="form-control"  ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Nombre del programa:</td>
                    <td>
                        <asp:TextBox ID="txtNombrePrograma"   CssClass="form-control"  ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Responsable del Programa:</td>
                    <td>
                        <asp:TextBox ID="txtResponsablePrograma"  CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <!-- Centramos el texto del periodo -->
            <div class="periodo-realizacion" style="color: #2e5790 " font-weight: bold>
                Periodo de realización del Servicio Social:
            </div>

            <!-- Usamos flexbox para las fechas -->
            <div class="fechas">
                <div>
                    <label for="txtFechaInicio">Fecha inicio:</label>
                    <asp:TextBox ID="txtFechaInicio"   CssClass="txt-estilo fecha-reducida"  required="required" TextMode="Date" runat="server"></asp:TextBox>
                  
                </div>
                <div>
                    <label for="txtFechaFin">Fecha fin:</label>
                    <asp:TextBox ID="txtFechaFin"  CssClass="txt-estilo fecha-reducida" required="required" type="date"  runat="server"></asp:TextBox>
                  
                </div>
            </div>

            <table>
                <tr>
                    <td>Actividades desarrolladas:</td>
                    <td>
                        <asp:TextBox ID="txtActividades" runat="server"  CssClass="txt-estilo" required="required" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Población beneficiada con el programa:</td>
                    <td>
                        <asp:TextBox ID="txtPoblacionBeneficiada"  CssClass="txt-estilo" required="required" TextMode="MultiLine" Rows="4" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Metas y/o resultados obtenidos:</td>
                    <td>
                        <asp:TextBox ID="txtMetasResultados"  CssClass="txt-estilo" required="required" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnGuardar" CssClass="boton-guardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
</asp:Content>