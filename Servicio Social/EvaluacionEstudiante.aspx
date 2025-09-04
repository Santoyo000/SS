<%@ Page Title="" Language="C#" MasterPageFile="~/SS.Master" AutoEventWireup="true" CodeBehind="EvaluacionEstudiante.aspx.cs" Inherits="Servicio_Social.EvaluacionEstudiante" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
     <style>
       html,
       body {
         font-family: 'Poppins', sans-serif;

        }
       .tutor-header {
           text-align: left;
           margin-bottom: 20px;
           font-size: 20px; /* Ajusta el tamaño de la fuente según sea necesario */
           color:#284c7e;
          /* font-weight: bold*/
       }
       .table {
           margin: 20px auto;
           width: 100%;
           box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
      
       }
       th, td {
           vertical-align: middle;
           text-align: center;
          /* padding: 10px;*/
       }
       .table th:nth-child(1) {
       background-color: #2e5790;
       color: white;
       font-size: 16px;
       }
       .table th:nth-child(2) {
           background-color:#d57269;
           color: white;
           font-size: 16px;
       }
       .table th:nth-child(3) {
           background-color: #efab6d;
           color: white;
           font-size: 16px;
       }
       .table th:nth-child(4) {
           background-color: #f7e28d;
           color: white;
           font-size: 16px;
       }
       .table th:nth-child(5) {
           background-color: #9be1b7;
           color: white;
           font-size: 16px;
       }
       .table th:nth-child(6) {
           background-color: #68c791;
           color: white;
           font-size: 16px;
       }
  

       .question {
           text-align: left;
            font-size: 14px;
           /*font-weight: bold;*/
       }
       .form-container {
           margin: 20px auto;
           width: 90%;
       }
       h2 {
           text-align: center;
           margin-bottom: 30px;
           font-size: 24px; /* Ajusta el tamaño del H2 */
           color: #2c3e50; /* Cambia el color si lo deseas */
           font-weight: normal; /* Elimina el bold */
        }
         h5 { text-align: center;
         }
       .submit-btn {
           display: block;
           margin: 0 auto;
       }
       .btn-primary {
           background-color: #f2a343; /* Color de fondo del botón */
           border-color: #f2a343; /* Color del borde del botón */
           font-size: 16px; /* Tamaño de la letra */
           color: white; /* Color del texto */
           padding: 10px 20px; /* Tamaño del botón */
       }

       .btn-primary:hover {
           background-color: #f19526; /* Color de fondo cuando el cursor pasa por encima */
           border-color: #f19526; /* Color del borde en hover */
           color: white; /* Color del texto en hover */
       }

       #contact {
           margin-left: 0px; /*Separación del formulario del lado izquierdo*/
          /* padding: 25px 30px;*/
           background-color: #fff; /*Color Formulario*/
           border-radius: 5px; /*Esquinas formulario*/
       }
       .contact-form textarea,
.contact-form input[type="text"],
.contact-form input[type="email"],
.contact-form input[type="number"] {
    color: #2a2a2a;
    font-size: 14px;
    border: 1px solid #ddd;
    background-color: #f7f8fa;
    width: 100%;
    height: 90px;
    border-radius: 5px;
    outline: none;
    padding-top: 3px;
    padding-left: 20px;
    padding-right: 20px;
    margin-bottom: 15px;
}
    .checkbox-list table {
    width: auto !important;
    margin: 0;
}

.checkbox-list td {
    vertical-align: top;
    padding: 4px 20px !important;
    white-space: nowrap;
}

.checkbox-list label {
    font-weight: normal;
    white-space: nowrap;
    display: inline-block;
}

  /*     .contact-form input, .contact-form textarea, .contact-form select {
           color: #2a2a2a;
           font-size: 14px;
           border: 1px solid #ddd;
           background-color: #f7f8fa;
           width: 100%;
           height: 90px;
           border-radius: 5px;
           outline: none;
           padding-top: 3px;
           padding-left: 20px;
           padding-right: 20px;
           -webkit-appearance: none;
           -moz-appearance: none;
           appearance: none;
           margin-bottom: 15px;
       }*/

   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titulo" runat="server">
<div class="form-container">
       <asp:Label ID="lbEstudiante" runat="server" CssClass="tutor-header"></asp:Label>
       <br />
       <br />
       <br />
        <h2>ENCUESTA DE EVALUACIÓN DE ESTUDIANTE</h2>
        <h5>Conteste las preguntas seleccionando el valor que usted considere de acuerdo a la escala.</h5>
         <table class="table table-bordered table-hover" id="tbEncuesta" runat="server">
     <thead >
         <tr>
             <th>Pregunta</th>
             <th>Pésimo</th>
             <th>Deficiente</th>
             <th>Suficiente</th>
             <th>Adecuado</th>
             <th>Excelente</th>
         </tr>
     </thead>
     <tbody>
         <!-- Pregunta 1 -->
         <tr>
             <td class="question" id="P267">1. ¿En qué medida el alumno está realizando las actividades para dar cumplimiento con el objetivo del Programa?</td>
             <td><asp:RadioButton id="rbPregunta1Pesimo" runat="server" GroupName="pregunta1" /></td>
             <td><asp:RadioButton id="rbPregunta1Deficiente" runat="server" GroupName="pregunta1" /></td>
             <td><asp:RadioButton id="rbPregunta1Suficiente" runat="server" GroupName="pregunta1" /></td>
             <td><asp:RadioButton id="rbPregunta1Adecuado" runat="server" GroupName="pregunta1" /></td>
             <td><asp:RadioButton id="rbPregunta1Excelente" runat="server" GroupName="pregunta1" /></td>
         </tr>
            <!-- Pregunta 2 -->
        <tr>
            <td class="question" id="P268">2. ¿En qué medida considera que el alumno cuenta con las habilidades necesarias para realizar las actividades comprendidas en el programa?</td>
            <td><asp:RadioButton id="rbPregunta2Pesimo" runat="server" GroupName="pregunta2" /></td>
            <td><asp:RadioButton id="rbPregunta2Deficiente" runat="server" GroupName="pregunta2" /></td>
            <td><asp:RadioButton id="rbPregunta2Suficiente" runat="server" GroupName="pregunta2" /></td>
            <td><asp:RadioButton id="rbPregunta2Adecuado" runat="server" GroupName="pregunta2" /></td>
            <td><asp:RadioButton id="rbPregunta2Excelente" runat="server" GroupName="pregunta2" /></td>
        </tr>
              <!-- Pregunta 3 -->
          <tr>
             <td class="question" id="P269">3. ¿En qué medida considera que el alumno cuenta con los conocimientos necesarios para realizar las actividades comprendidas en el programa?</td>
             <td><asp:RadioButton id="rbPregunta3Pesimo" runat="server" GroupName="pregunta3" /></td>
             <td><asp:RadioButton id="rbPregunta3Deficiente" runat="server" GroupName="pregunta3" /></td>
             <td><asp:RadioButton id="rbPregunta3Suficiente" runat="server" GroupName="pregunta3" /></td>
             <td><asp:RadioButton id="rbPregunta3Adecuado" runat="server" GroupName="pregunta3" /></td>
             <td><asp:RadioButton id="rbPregunta3Excelente" runat="server" GroupName="pregunta3" /></td>
          </tr>
              <!-- Pregunta 4 -->
          <tr>
              <td class="question" id="P270">4. ¿En qué medida considera que el alumno cuenta con la actitud necesaria para realizar las actividades comprendidas en el programa?</td>
              <td><asp:RadioButton id="rbPregunta4Pesimo" runat="server" GroupName="pregunta4" /></td>
              <td><asp:RadioButton id="rbPregunta4Deficiente" runat="server" GroupName="pregunta4" /></td>
              <td><asp:RadioButton id="rbPregunta4Suficiente" runat="server" GroupName="pregunta4" /></td>
              <td><asp:RadioButton id="rbPregunta4Adecuado" runat="server" GroupName="pregunta4" /></td>
              <td><asp:RadioButton id="rbPregunta4Excelente" runat="server" GroupName="pregunta4" /></td>
          </tr>
              <!-- Pregunta 5 -->
          <tr>
              <td class="question" id="P271">5. ¿En qué medida el alumno es puntual con el horario establecido?</td>
              <td><asp:RadioButton id="rbPregunta5Pesimo" runat="server" GroupName="pregunta5" /></td>
              <td><asp:RadioButton id="rbPregunta5Deficiente" runat="server" GroupName="pregunta5" /></td>
              <td><asp:RadioButton id="rbPregunta5Suficiente" runat="server" GroupName="pregunta5" /></td>
              <td><asp:RadioButton id="rbPregunta5Adecuado" runat="server" GroupName="pregunta5" /></td>
              <td><asp:RadioButton id="rbPregunta5Excelente" runat="server" GroupName="pregunta5" /></td>
          </tr>
               <!-- Pregunta 6 -->
            <tr>
               <td class="question" id="P272">6. ¿En qué medida el alumno cumple sus actividades en los tiempos establecidos?</td>
               <td><asp:RadioButton id="rbPregunta6Pesimo" runat="server" GroupName="pregunta6" /></td>
               <td><asp:RadioButton id="rbPregunta6Deficiente" runat="server" GroupName="pregunta6" /></td>
               <td><asp:RadioButton id="rbPregunta6Suficiente" runat="server" GroupName="pregunta6" /></td>
               <td><asp:RadioButton id="rbPregunta6Adecuado" runat="server" GroupName="pregunta6" /></td>
               <td><asp:RadioButton id="rbPregunta6Excelente" runat="server" GroupName="pregunta6" /></td>
            </tr>
                <!-- Pregunta 7 -->
            <tr>
               <td class="question" id="P273">7. ¿En qué medida le proporciona al alumno espacios físicos adecuados para desempeñar sus actividades asignadas al servicio social?</td>
               <td><asp:RadioButton id="rbPregunta7Pesimo" runat="server" GroupName="pregunta7" /></td>
               <td><asp:RadioButton id="rbPregunta7Deficiente" runat="server" GroupName="pregunta7" /></td>
               <td><asp:RadioButton id="rbPregunta7Suficiente" runat="server" GroupName="pregunta7" /></td>
               <td><asp:RadioButton id="rbPregunta7Adecuado" runat="server" GroupName="pregunta7" /></td>
               <td><asp:RadioButton id="rbPregunta7Excelente" runat="server" GroupName="pregunta7" /></td>
            </tr>
                 <!-- Pregunta 8 -->
            <tr>
               <td class="question" id="P274">8. ¿En qué medida le brinda mobiliario y herramientas necesarias para desempeñar sus actividades?</td>
               <td><asp:RadioButton id="rbPregunta8Pesimo" runat="server" GroupName="pregunta8" /></td>
               <td><asp:RadioButton id="rbPregunta8Deficiente" runat="server" GroupName="pregunta8" /></td>
               <td><asp:RadioButton id="rbPregunta8Suficiente" runat="server" GroupName="pregunta8" /></td>
               <td><asp:RadioButton id="rbPregunta8Adecuado" runat="server" GroupName="pregunta8" /></td>
               <td><asp:RadioButton id="rbPregunta8Excelente" runat="server" GroupName="pregunta8" /></td>
            </tr>
                 <!-- Pregunta 9 -->
            <tr>
                <td class="question" id="P275">9. ¿En qué medida el alumno hace uso adecuado del espacio y herramientas que le proporcionan para realizar sus actividades?</td>
                <td><asp:RadioButton id="rbPregunta9Pesimo" runat="server" GroupName="pregunta9" /></td>
                <td><asp:RadioButton id="rbPregunta9Deficiente" runat="server" GroupName="pregunta9" /></td>
                <td><asp:RadioButton id="rbPregunta9Suficiente" runat="server" GroupName="pregunta9" /></td>
                <td><asp:RadioButton id="rbPregunta9Adecuado" runat="server" GroupName="pregunta9" /></td>
                <td><asp:RadioButton id="rbPregunta9Excelente" runat="server" GroupName="pregunta9" /></td>
            </tr>
                 <!-- Pregunta 10 -->
             <tr>
                 <td class="question" id="P276">10. ¿En qué medida atiende al alumno cuando requiere de alguna asesoría?</td>
                 <td><asp:RadioButton id="rbPregunta10Pesimo" runat="server" GroupName="pregunta10" /></td>
                 <td><asp:RadioButton id="rbPregunta10Deficiente" runat="server" GroupName="pregunta10" /></td>
                 <td><asp:RadioButton id="rbPregunta10Suficiente" runat="server" GroupName="pregunta10" /></td>
                 <td><asp:RadioButton id="rbPregunta10Adecuado" runat="server" GroupName="pregunta10" /></td>
                 <td><asp:RadioButton id="rbPregunta10Excelente" runat="server" GroupName="pregunta10" /></td>
             </tr>
                  <!-- Pregunta 11 -->
               <tr>
                  <td class="question" id="P277">11. ¿Cómo es la relación de trabajo con el prestador de servicio social que se le asignó?</td>
                  <td><asp:RadioButton id="rbPregunta11Pesimo" runat="server" GroupName="pregunta11" /></td>
                  <td><asp:RadioButton id="rbPregunta11Deficiente" runat="server" GroupName="pregunta11" /></td>
                  <td><asp:RadioButton id="rbPregunta11Suficiente" runat="server" GroupName="pregunta11" /></td>
                  <td><asp:RadioButton id="rbPregunta11Adecuado" runat="server" GroupName="pregunta11" /></td>
                  <td><asp:RadioButton id="rbPregunta11Excelente" runat="server" GroupName="pregunta11" /></td>
               </tr>
                 
     </tbody>
 </table>
       <div class="contact-form" id="contact">
           <asp:Label Text="12. ¿Cuáles son las necesidades que usted como responsable del programa de servicio social detecta que hacen falta en el perfil del alumno asignado a su dependencia?" ID="Label1" runat="server" > </asp:Label>
           <asp:TextBox runat="server"  ID="txtPregunta12" type="text" TextMode="MultiLine"></asp:TextBox>
           <asp:Label Text="13.- ¿Qué características requiere el prestador de servicio social para el desempeño de sus actividades?" ID="Label2" runat="server" > </asp:Label>
           <asp:TextBox runat="server"  ID="txtPregunta13" type="text" TextMode="MultiLine"></asp:TextBox>
        </div>
    <asp:Panel ID="panelHabilidades" runat="server" CssClass="contact-form">
    <asp:Label ID="lblHabilidades" runat="server" Text="14. Seleccione las habilidades que el estudiante adquirió durante su servicio social:"></asp:Label>
    <br />
<table class="table table-borderless">
    <tr>
        <td><asp:CheckBox ID="chk1" runat="server" Text="Gestión del tiempo" /></td>
        <td><asp:CheckBox ID="chk2" runat="server" Text="Puntualidad" /></td>
        <td><asp:CheckBox ID="chk3" runat="server" Text="Creatividad" /></td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="chk4" runat="server" Text="Negociación" /></td>
        <td><asp:CheckBox ID="chk5" runat="server" Text="Inteligencia emocional" /></td>
        <td><asp:CheckBox ID="chk6" runat="server" Text="Flexibilidad" /></td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="chk7" runat="server" Text="Manejo del estrés" /></td>
        <td><asp:CheckBox ID="chk8" runat="server" Text="Trabajo en equipo" /></td>
        <td><asp:CheckBox ID="chk9" runat="server" Text="Liderazgo e iniciativa" /></td>
    </tr>
    <tr>
        <td><asp:CheckBox ID="chk10" runat="server" Text="Comunicación efectiva" /></td>
        <td><asp:CheckBox ID="chk11" runat="server" Text="Orientación a resultados" /></td>
        <td></td>
    </tr>
</table>
</asp:Panel>
       <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary submit-btn" Text="Enviar encuesta" OnClick="GuardarRespuestas"/>   
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
                La evaluación se ha enviado con éxito.
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" onclick="redirect()" style="background-color:#1073b0;color:white">Aceptar</button>
            </div>
        </div>
    </div>
</div>

<script>
    function redirect() {
        // Redirige a la página AlumnosPostulados.aspx
        window.location.href = 'AlumnosPostulados.aspx';
    }
</script>
   <!-- Modal información requerida -->
      <div class="modal fade" id="ModalMensaje" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
              <div class="modal-content">
                  <div class="modal-header">
                      <h5 class="modal-title" id="modalLabel1">Información requerida</h5>
                      <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">&times;</span>
                      </button>
                  </div>
                  <div class="modal-body">
                      Favor de contestar todas las preguntas de la encuesta antes de enviar.
                  </div>
                  <div class="modal-footer">
                      <button type="button" id="BotonModal" class="btn btn-primary" style="background-color:#1073b0;color:white" data-dismiss="modal">Aceptar</button>
                  </div>
              </div>
          </div>
      </div>

<!-- Bootstrap JS, Popper.js y jQuery -->
   <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
   <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
   <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

  <%-- <script>
       function redirectCrear() {
           window.location.href = 'Encuesta.aspx';
       }

       function redirect() {
           window.location.href = 'EvidenciaEncuesta.aspx';
       }

       $('#BotonModal').click(function () {
           $('#ModalMensaje').modal('hide');
       });
   </script>--%>
</asp:Content>
