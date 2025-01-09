using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class EvaluacionPrograma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idProgramaAlumno = Request.QueryString["nst"];
            if (!IsPostBack)
            {

                //ObtenerMatriculaYNombreEstudiante(idAlumno, idDependencia);
                //AlumnoYaHizoEncuesta(idAlumno);

            }
        }
        protected void GuardarRespuestas(object sender, EventArgs e)
        {
            // Número total de preguntas
            int totalPreguntas = 14;

            // Crear una lista para almacenar las respuestas
            List<(int numeroPregunta, string respuesta)> respuestas = new List<(int, string)>();

            for (int i = 1; i <= totalPreguntas; i++)
            {


                // IDs para cada opción en la pregunta actual
                string idRadioButtonPesimo = $"rbPregunta{i}Pesimo";
                string idRadioButtonDeficiente = $"rbPregunta{i}Deficiente";
                string idRadioButtonSuficiente = $"rbPregunta{i}Suficiente";
                string idRadioButtonAdecuado = $"rbPregunta{i}Adecuado";
                string idRadioButtonExcelente = $"rbPregunta{i}Excelente";

                RadioButton rbPesimo = (RadioButton)tbEncuesta.FindControl(idRadioButtonPesimo);
                RadioButton rbDeficiente = (RadioButton)tbEncuesta.FindControl(idRadioButtonDeficiente);
                RadioButton rbSuficiente = (RadioButton)tbEncuesta.FindControl(idRadioButtonSuficiente);
                RadioButton rbAdecuado = (RadioButton)tbEncuesta.FindControl(idRadioButtonAdecuado);
                RadioButton rbExcelente = (RadioButton)tbEncuesta.FindControl(idRadioButtonExcelente);



                // Intentar encontrar el RadioButton seleccionado para cada opción en la pregunta actual
                if (rbPesimo != null && rbPesimo.Checked)
                {
                    respuestas.Add((i, "62071"));
                }
                else if (rbDeficiente != null && rbDeficiente.Checked)
                {
                    respuestas.Add((i, "62072"));
                }
                else if (rbSuficiente != null && rbSuficiente.Checked)
                {
                    respuestas.Add((i, "62073"));
                }
                else if (rbAdecuado != null && rbAdecuado.Checked)
                {
                    respuestas.Add((i, "62074"));
                }
                else if (rbExcelente != null && rbExcelente.Checked)
                {
                    respuestas.Add((i, "62075"));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalMensaje').modal('show');", true);
                    return;
                }
            }
            // Insertar cada respuesta en la base de datos llamando a un procedimiento almacenado
            foreach (var respuesta in respuestas)
            {
                InsertarRespuesta(respuesta.numeroPregunta, respuesta.respuesta);
            }

        }
        public void InsertarRespuesta(int kmRubro_encuesta, string kpOpciones)
        {
            // Recuperar variables de sesión
            
            string idProgramaAlumno = Request.QueryString["nst"];
            string idAlumno = Session["ID"].ToString();
            string Pregunta15 = txtPregunta15.Text;
            string Pregunta16 = txtPregunta16.Text;
            string Pregunta17 = txtPregunta17.Text;

            if (kmRubro_encuesta == 1)
            { kmRubro_encuesta = 280; }
            else if (kmRubro_encuesta == 2)
            { kmRubro_encuesta = 281; }
            else if (kmRubro_encuesta == 3)
            { kmRubro_encuesta = 282; }
            else if (kmRubro_encuesta == 4)
            { kmRubro_encuesta = 283; }
            else if (kmRubro_encuesta == 5)
            { kmRubro_encuesta = 284; }
            else if (kmRubro_encuesta == 6)
            { kmRubro_encuesta = 285; }
            else if (kmRubro_encuesta == 7)
            { kmRubro_encuesta = 286; }
            else if (kmRubro_encuesta == 8)
            { kmRubro_encuesta = 287; }
            else if (kmRubro_encuesta == 9)
            { kmRubro_encuesta = 288; }
            else if (kmRubro_encuesta == 10)
            { kmRubro_encuesta = 289; }
            else if (kmRubro_encuesta == 11)
            { kmRubro_encuesta = 290; }
            else if (kmRubro_encuesta == 12)
            { kmRubro_encuesta = 291; }
            else if (kmRubro_encuesta == 13)
            { kmRubro_encuesta = 292; }
            else if (kmRubro_encuesta == 14)
            { kmRubro_encuesta = 293; }


            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Evaluacion_Programa_ss", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PreguntaId", kmRubro_encuesta);
                    cmd.Parameters.AddWithValue("@Respuesta", kpOpciones);
                    cmd.Parameters.AddWithValue("@kmAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@kmProgramaAlumno", idProgramaAlumno);
                    cmd.Parameters.AddWithValue("@Pregunta15", Pregunta15);
                    cmd.Parameters.AddWithValue("@Pregunta16", Pregunta16);
                    cmd.Parameters.AddWithValue("@Pregunta17", Pregunta17);

                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        //cambiarEstatus(idAlumno, idDependencia);
                        //connection.Close();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

        }
     }
}