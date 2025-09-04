using iText.StyledXmlParser.Jsoup.Select;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class EvaluacionEstudiante : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string filtros = Session["idDependencia"].ToString();

            string idDependencia = filtros.Split('|')[1];
            //string idAlumno = Request.QueryString["nst"];
            if (!IsPostBack)
            {
                string idAlumno = Request.QueryString["nst1"];
                string idProgramaAlumno = Request.QueryString["nst2"];

                ObtenerMatriculaYNombreEstudiante(idAlumno, idDependencia);
                AlumnoYaHizoEncuesta(idAlumno);

            }
        }

        private bool AlumnoYaHizoEncuesta(string idAlumno)
        {

            string connectionString = GlobalConstants.SQL;
            string query = "SELECT COUNT(1) FROM SM_EVALUACION_ESTUDIANTE_SS WHERE kmAlumno = @kmAlumno";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@kmAlumno", idAlumno));
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Si el alumno ya hizo la encuesta, redirige a alumnospostulados.aspx
                        if (count > 0)
                        {
                            Response.Redirect("AlumnosPostulados.aspx");
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción, se puede registrar el error si es necesario
                return false;
            }
        }
        //protected void GuardarRespuestas(object sender, EventArgs e)
        //{
        //    // Número total de preguntas
        //    int totalPreguntas = 11;

        //    // Crear una lista para almacenar las respuestas
        //    List<(int numeroPregunta, string respuesta)> respuestas = new List<(int, string)>();

        //    for (int i = 1; i <= totalPreguntas; i++)
        //    {


        //        // IDs para cada opción en la pregunta actual
        //        string idRadioButtonPesimo = $"rbPregunta{i}Pesimo";
        //        string idRadioButtonDeficiente = $"rbPregunta{i}Deficiente";
        //        string idRadioButtonSuficiente = $"rbPregunta{i}Suficiente";
        //        string idRadioButtonAdecuado = $"rbPregunta{i}Adecuado";
        //        string idRadioButtonExcelente = $"rbPregunta{i}Excelente";

        //        RadioButton rbPesimo = (RadioButton)tbEncuesta.FindControl(idRadioButtonPesimo);
        //        RadioButton rbDeficiente = (RadioButton)tbEncuesta.FindControl(idRadioButtonDeficiente);
        //        RadioButton rbSuficiente = (RadioButton)tbEncuesta.FindControl(idRadioButtonSuficiente);
        //        RadioButton rbAdecuado = (RadioButton)tbEncuesta.FindControl(idRadioButtonAdecuado);
        //        RadioButton rbExcelente = (RadioButton)tbEncuesta.FindControl(idRadioButtonExcelente);



        //        // Intentar encontrar el RadioButton seleccionado para cada opción en la pregunta actual
        //        if (rbPesimo != null && rbPesimo.Checked)
        //        {
        //            respuestas.Add((i, "62071"));
        //        }
        //        else if (rbDeficiente != null && rbDeficiente.Checked)
        //        {
        //            respuestas.Add((i, "62072"));
        //        }
        //        else if (rbSuficiente != null && rbSuficiente.Checked)
        //        {
        //            respuestas.Add((i, "62073"));
        //        }
        //        else if (rbAdecuado != null && rbAdecuado.Checked)
        //        {
        //            respuestas.Add((i, "62074"));
        //        }
        //        else if (rbExcelente != null && rbExcelente.Checked)
        //        {
        //            respuestas.Add((i, "62075"));
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalMensaje').modal('show');", true);
        //            return;
        //        }
        //    }

        //    // Insertar cada respuesta en la base de datos llamando a un procedimiento almacenado
        //    foreach (var respuesta in respuestas)
        //    {
        //        InsertarRespuesta(respuesta.numeroPregunta, respuesta.respuesta);
        //    }
        //}

        //ESTE ES EL ANTERIOR
        //protected void GuardarRespuestas(object sender, EventArgs e)
        //{
        //    // Número total de preguntas
        //    int totalPreguntas = 11;

        //    // Crear una lista para almacenar las respuestas
        //    List<(int numeroPregunta, string respuesta)> respuestas = new List<(int, string)>();

        //    for (int i = 1; i <= totalPreguntas; i++)
        //    {
        //        // IDs para cada opción en la pregunta actual
        //        string idRadioButtonPesimo = $"rbPregunta{i}Pesimo";
        //        string idRadioButtonDeficiente = $"rbPregunta{i}Deficiente";
        //        string idRadioButtonSuficiente = $"rbPregunta{i}Suficiente";
        //        string idRadioButtonAdecuado = $"rbPregunta{i}Adecuado";
        //        string idRadioButtonExcelente = $"rbPregunta{i}Excelente";

        //        RadioButton rbPesimo = (RadioButton)tbEncuesta.FindControl(idRadioButtonPesimo);
        //        RadioButton rbDeficiente = (RadioButton)tbEncuesta.FindControl(idRadioButtonDeficiente);
        //        RadioButton rbSuficiente = (RadioButton)tbEncuesta.FindControl(idRadioButtonSuficiente);
        //        RadioButton rbAdecuado = (RadioButton)tbEncuesta.FindControl(idRadioButtonAdecuado);
        //        RadioButton rbExcelente = (RadioButton)tbEncuesta.FindControl(idRadioButtonExcelente);

        //        // Intentar encontrar el RadioButton seleccionado para cada opción en la pregunta actual
        //        if (rbPesimo != null && rbPesimo.Checked)
        //        {
        //            respuestas.Add((i, "62071"));
        //        }
        //        else if (rbDeficiente != null && rbDeficiente.Checked)
        //        {
        //            respuestas.Add((i, "62072"));
        //        }
        //        else if (rbSuficiente != null && rbSuficiente.Checked)
        //        {
        //            respuestas.Add((i, "62073"));
        //        }
        //        else if (rbAdecuado != null && rbAdecuado.Checked)
        //        {
        //            respuestas.Add((i, "62074"));
        //        }
        //        else if (rbExcelente != null && rbExcelente.Checked)
        //        {
        //            respuestas.Add((i, "62075"));
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalMensaje').modal('show');", true);
        //            return;
        //        }
        //    }

        //    // Insertar cada respuesta en la base de datos llamando a un procedimiento almacenado
        //    foreach (var respuesta in respuestas)
        //    {
        //        InsertarRespuesta(respuesta.numeroPregunta, respuesta.respuesta);
        //    }

        //    // Llamar al método cambiarEstatus después de insertar todas las respuestas
        //    string filtros = Session["idDependencia"].ToString();
        //    string idDependencia = filtros.Split('|')[1];
        //    string idUsuario = filtros.Split('|')[0];
        //    string idAlumno = Request.QueryString["nst"];

        //    cambiarEstatus(idAlumno, idDependencia, idUsuario);
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        //}

        //public void InsertarRespuesta(int kmRubro_encuesta, string kpOpciones)
        //{
        //    // Recuperar variables de sesión
        //    string filtros = Session["idDependencia"].ToString();
        //    string idDependencia = filtros.Split('|')[1];
        //    string idUsuario = filtros.Split('|')[0];
        //    string idAlumno = Request.QueryString["nst1"];
        //    string idProgramaAlumno = Request.QueryString["nst2"];

        //    string Pregunta12 = txtPregunta12.Text;
        //    string Pregunta13 = txtPregunta13.Text;

        //    if (kmRubro_encuesta == 1)
        //    { kmRubro_encuesta = 267; }
        //    else if (kmRubro_encuesta == 2)
        //    { kmRubro_encuesta = 268; }
        //    else if (kmRubro_encuesta == 3)
        //    { kmRubro_encuesta = 269; }
        //    else if (kmRubro_encuesta == 4)
        //    { kmRubro_encuesta = 270; }
        //    else if (kmRubro_encuesta == 5)
        //    { kmRubro_encuesta = 271; }
        //    else if (kmRubro_encuesta == 6)
        //    { kmRubro_encuesta = 272; }
        //    else if (kmRubro_encuesta == 7)
        //    { kmRubro_encuesta = 273; }
        //    else if (kmRubro_encuesta == 8)
        //    { kmRubro_encuesta = 274; }
        //    else if (kmRubro_encuesta == 9)
        //    { kmRubro_encuesta = 275; }
        //    else if (kmRubro_encuesta == 10)
        //    { kmRubro_encuesta = 276; }
        //    else if (kmRubro_encuesta == 11)
        //    { kmRubro_encuesta = 277; }


        //    string connectionString = GlobalConstants.SQL;
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("sp_Evaluacion_Estudiante_ss", connection))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@PreguntaId", kmRubro_encuesta);
        //            cmd.Parameters.AddWithValue("@Respuesta", kpOpciones);
        //            cmd.Parameters.AddWithValue("@idDependencia", idDependencia);
        //            cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
        //            cmd.Parameters.AddWithValue("@Pregunta12", Pregunta12);
        //            cmd.Parameters.AddWithValue("@Pregunta13", Pregunta13);

        //            try
        //            {
        //                connection.Open();
        //                cmd.ExecuteNonQuery();
        //                cambiarEstatus(idAlumno, idDependencia, idUsuario, idProgramaAlumno);
        //                //connection.Close();
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        //            }

        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error: " + ex.Message);
        //            }
        //        }
        //    }
        //}

        public void InsertarRespuesta(int kmRubro_encuesta, string kpOpciones)
        {
            string filtros = Session["idDependencia"].ToString();
            string idDependencia = filtros.Split('|')[1];
            string idAlumno = Request.QueryString["nst1"];

            string Pregunta12 = txtPregunta12.Text;
            string Pregunta13 = txtPregunta13.Text;

            string idProgramaAlumno = Request.QueryString["nst2"];

            if (kmRubro_encuesta >= 1 && kmRubro_encuesta <= 11)
            {
                kmRubro_encuesta = 266 + kmRubro_encuesta;
            }

            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Evaluacion_Estudiante_ss", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PreguntaId", kmRubro_encuesta);
                    cmd.Parameters.AddWithValue("@Respuesta", kpOpciones);
                    cmd.Parameters.AddWithValue("@idDependencia", idDependencia);
                    cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@Pregunta12", Pregunta12);
                    cmd.Parameters.AddWithValue("@Pregunta13", Pregunta13);

                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        protected void GuardarRespuestas(object sender, EventArgs e)
        {
            int totalPreguntas = 11;
            List<(int numeroPregunta, string respuesta)> respuestas = new List<(int, string)>();

            for (int i = 1; i <= totalPreguntas; i++)
            {
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

                if (rbPesimo != null && rbPesimo.Checked)
                    respuestas.Add((i, "62071"));
                else if (rbDeficiente != null && rbDeficiente.Checked)
                    respuestas.Add((i, "62072"));
                else if (rbSuficiente != null && rbSuficiente.Checked)
                    respuestas.Add((i, "62073"));
                else if (rbAdecuado != null && rbAdecuado.Checked)
                    respuestas.Add((i, "62074"));
                else if (rbExcelente != null && rbExcelente.Checked)
                    respuestas.Add((i, "62075"));
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalMensaje').modal('show');", true);
                    return;
                }
            }

            // Guardar respuestas de evaluación
            foreach (var respuesta in respuestas)
            {
                InsertarRespuesta(respuesta.numeroPregunta, respuesta.respuesta);
            }

            // ✅ Nueva forma de obtener habilidades seleccionadas desde <asp:CheckBox> individuales
            List<int> habilidadesSeleccionadas = new List<int>();

            if (chk1.Checked) habilidadesSeleccionadas.Add(1);  // Gestión del tiempo
            if (chk2.Checked) habilidadesSeleccionadas.Add(5);  // Puntualidad
            if (chk3.Checked) habilidadesSeleccionadas.Add(9);  // Creatividad
            if (chk4.Checked) habilidadesSeleccionadas.Add(2);  // Negociación
            if (chk5.Checked) habilidadesSeleccionadas.Add(6);  // Inteligencia emocional
            if (chk6.Checked) habilidadesSeleccionadas.Add(10); // Flexibilidad
            if (chk7.Checked) habilidadesSeleccionadas.Add(3);  // Manejo del estrés
            if (chk8.Checked) habilidadesSeleccionadas.Add(7);  // Trabajo en equipo
            if (chk9.Checked) habilidadesSeleccionadas.Add(11); // Liderazgo e iniciativa
            if (chk10.Checked) habilidadesSeleccionadas.Add(4); // Comunicación efectiva
            if (chk11.Checked) habilidadesSeleccionadas.Add(8); // Orientación a resultados

            GuardarHabilidadesSeleccionadas(habilidadesSeleccionadas);

            // Finalizar evaluación
            string filtros = Session["idDependencia"].ToString();
            string idDependencia = filtros.Split('|')[1];
            string idUsuario = filtros.Split('|')[0];
            string idAlumno = Request.QueryString["nst1"];
            string idProgramaAlumno = Request.QueryString["nst2"];


            cambiarEstatus(idAlumno, idDependencia, idUsuario, idProgramaAlumno);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        }

        public void ObtenerMatriculaYNombreEstudiante(string idAlumno, string idDependencia)
{

    string connectionString = GlobalConstants.SQL;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = @"
            SELECT 
                AL.SMATRICULA + ' - ' + PER.SNOMBRE_COMPLETO AS Estudiante
            FROM SM_PROGRAMA_ALUMNO PA
            INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
            INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
            INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
            INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.ID = PA.kmAlumno
            INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno
            INNER JOIN NM_PERSONA PER ON PER.idPersona = AL.kmPersona
            WHERE UA.kmAlumno = @idAlumno AND DS.idDependenicaServicio = @idDependencia";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@idAlumno", idAlumno);
        command.Parameters.AddWithValue("@idDependencia", idDependencia);

        try
        {
            connection.Open();
            string estudiante = (string)command.ExecuteScalar();

            // Asigna el resultado a lbEstudiante
            lbEstudiante.Text = "TU ESTUDIANTE A EVALUAR ES: " + estudiante;
        }
        catch (Exception ex)
        {
            // Manejo de errores
            lbEstudiante.Text = "Error al obtener los datos del estudiante";
            Console.WriteLine(ex.Message);
        }
    }
}

        protected void cambiarEstatus(string idAlumno, string idDependencia, string idUsuario, string idProgramaAlumno)
        {
            string connectionString = GlobalConstants.SQL;

            string updateQuery = "UPDATE SM_PROGRAMA_ALUMNO SET kpEstatus = @Estatus WHERE idProgramaAlumno = @idProgramaAlumno";
            string insertQuery = @"INSERT INTO SM_BITACORA_PA (kmProgramaAlumno, kpEstatus, kmEvaluoEncuesta) 
                           VALUES (@kmProgramaAlumno, @kpEstatus, @kmEvaluoEncuesta);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // ✅ Actualizar el estatus
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction))
                        {
                            updateCommand.Parameters.AddWithValue("@Estatus", 22115); // ESTATUS EVALUADO
                            updateCommand.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);
                            updateCommand.ExecuteNonQuery();
                        }

                        // ✅ Insertar en SM_BITACORA_PA
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@kmProgramaAlumno", idProgramaAlumno);
                            insertCommand.Parameters.AddWithValue("@kpEstatus", 22115); // ESTATUS EVALUADO
                            insertCommand.Parameters.AddWithValue("@kmEvaluoEncuesta", idUsuario);
                            insertCommand.ExecuteNonQuery();
                        }

                        // ✅ Confirmar la transacción
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Aquí puedes loguear el error si lo deseas
                        // Ejemplo: Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        //protected void cambiarEstatus(string idAlumno, string idDependencia, string idUsuario, int idProgramaAlumno)
        //{
        //    string connectionString = GlobalConstants.SQL;

        //    string selectQuery = @"
        //SELECT PA.idProgramaAlumno
        //FROM SM_PROGRAMA_ALUMNO PA
        //INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
        //INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
        //INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
        //INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.ID = PA.kmAlumno
        //INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno
        //INNER JOIN NM_PERSONA PER ON PER.idPersona = AL.kmPersona
        //WHERE UA.kmAlumno = @idAlumno AND DS.idDependenicaServicio = @idDependencia";

        // string updateQuery = "UPDATE SM_PROGRAMA_ALUMNO SET kpEstatus = @Estatus WHERE idProgramaAlumno = @idProgramaAlumno";
        // string insertQuery = @"INSERT INTO SM_BITACORA_PA (kmProgramaAlumno, kpEstatus, kmEvaluoEncuesta) 
        //VALUES (@kmProgramaAlumno, @kpEstatus, @kmEvaluoEncuesta);";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlTransaction transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Obtener idProgramaAlumno
        //                int idProgramaAlumno = 0;
        //                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection, transaction))
        //                {
        //                    selectCommand.Parameters.AddWithValue("@idAlumno", idAlumno);
        //                    selectCommand.Parameters.AddWithValue("@idDependencia", idDependencia);

        //                    object result = selectCommand.ExecuteScalar();
        //                    if (result != null)
        //                    {
        //                        idProgramaAlumno = Convert.ToInt32(result);
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("No se encontró el idProgramaAlumno para los parámetros proporcionados.");
        //                    }
        //                }

        //                // Actualizar el estatus
        //                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction))
        //                {
        //                    updateCommand.Parameters.AddWithValue("@Estatus", 22115); // ESTATUS EVALUADO
        //                    updateCommand.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);
        //                    updateCommand.ExecuteNonQuery();
        //                }

        //                // Insertar en SM_BITACORA_PA
        //                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
        //                {
        //                    insertCommand.Parameters.AddWithValue("@kmProgramaAlumno", idProgramaAlumno);
        //                    insertCommand.Parameters.AddWithValue("@kpEstatus", 22115); // ESTATUS EVALUADO
        //                    insertCommand.Parameters.AddWithValue("@kmEvaluoEncuesta", idUsuario);
        //                    insertCommand.ExecuteNonQuery();
        //                }

        //                // Confirmar la transacción si todo fue exitoso
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                // Revertir la transacción en caso de error
        //                transaction.Rollback();
        //                // Manejo de excepciones, se puede registrar el error si es necesario
        //                // Console.WriteLine("Error: " + ex.Message);
        //            }
        //        }
        //    }
        //}

        private void GuardarHabilidadesSeleccionadas(List<int> idsHabilidades)
        {
            string filtros = Session["idDependencia"].ToString();
            string idDependencia = filtros.Split('|')[1];
            string idAlumno = Request.QueryString["nst1"];
            string idProgramaAlumno =  Request.QueryString["nst2"];
            //int idPrograma = ObtenerProgramaId(Convert.ToInt32(idAlumno), Convert.ToInt32(idDependencia));

            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (int idHabilidad in idsHabilidades)
                {
                    string insertQuery = @"INSERT INTO SM_HABILIDADES_ALUMNO 
                                   (kmAlumno, kmDependencia, kmProgramaAlumno, kpHabilidad)
                                   VALUES (@idAlumno, @idDependencia, @idProgramaAlumno, @idHabilidad)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                        cmd.Parameters.AddWithValue("@idDependencia", idDependencia);
                        cmd.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);
                        cmd.Parameters.AddWithValue("@idHabilidad", idHabilidad);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        private int ObtenerProgramaId(int idAlumno, int idDependencia)
        {
            string connectionString = GlobalConstants.SQL;
            string query = @"
        SELECT TOP 1 P.idPrograma
        FROM SM_USUARIOS_ALUMNOS AS UA
        INNER JOIN SM_PROGRAMA_ALUMNO PA ON UA.ID = PA.kmAlumno
        INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
        INNER JOIN SM_PROGRAMA P ON DP.kmPrograma = P.idPrograma
        INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON P.kpDependencia = DS.idDependenicaServicio
        WHERE UA.kmAlumno = @idAlumno AND DS.idDependenicaServicio = @idDependencia";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                cmd.Parameters.AddWithValue("@idDependencia", idDependencia);
                connection.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        //protected void cambiarEstatus(string idAlumno, string idDependencia)
        //{
        //    string connectionString = GlobalConstants.SQL;
        //    string selectQuery = @"
        //SELECT PA.idProgramaAlumno
        //FROM SM_PROGRAMA_ALUMNO PA
        //INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
        //INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
        //INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
        //INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.ID = PA.kmAlumno
        //INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno
        //INNER JOIN NM_PERSONA PER ON PER.idPersona = AL.kmPersona
        //WHERE UA.kmAlumno = @idAlumno AND DS.idDependenicaServicio = @idDependencia";

        //    string updateQuery = "UPDATE SM_PROGRAMA_ALUMNO SET kpEstatus = @Estatus WHERE idProgramaAlumno = @idProgramaAlumno";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();

        //            // Obtener idProgramaAlumno
        //            using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
        //            {
        //                selectCommand.Parameters.AddWithValue("@idAlumno", idAlumno);
        //                selectCommand.Parameters.AddWithValue("@idDependencia", idDependencia);

        //                object result = selectCommand.ExecuteScalar();
        //                if (result != null)
        //                {
        //                    int idProgramaAlumno = Convert.ToInt32(result);

        //                    // Actualizar el estatus
        //                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@Estatus", 22115);
        //                        updateCommand.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);

        //                        updateCommand.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones, se puede registrar el error si es necesario
        //            // Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}
    }
}