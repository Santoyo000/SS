using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace Servicio_Social
{
    public partial class LiberarEstudiante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idProgramaAlumno = Request.QueryString["nst"];
            if (!string.IsNullOrEmpty(idProgramaAlumno))
            {
                CargarDatos(int.Parse(idProgramaAlumno));
            }
        }
        private void CargarDatos(int idProgramaAlumno)
        {
            // Cadena de conexión desde web.config
            string connectionString = GlobalConstants.SQL;
            //string kmUserAlumno = Session["ID"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Primera consulta: siempre carga los datos básicos del programa y alumno
                string queryPrograma = @"SELECT PA.dfechaRegistro, 
                                       ISNULL(PER.sNombres, '') + ' ' + ISNULL(PER.sApellido_Paterno, '') + ' ' + ISNULL(PER.sApellido_Materno, '') AS sNombre_Completo,
                                       P.sNombre_Programa, 
                                       sResponsable
                                FROM SM_PROGRAMA AS P
                                INNER JOIN SM_DETALLE_PROGRAMA AS DP ON P.idPrograma = DP.kmPrograma
                                INNER JOIN SM_PROGRAMA_ALUMNO AS PA ON DP.idDetallePrograma = PA.kmDetallePrograma
                                INNER JOIN SM_USUARIOS_ALUMNOS AS USA ON PA.kmAlumno = USA.ID
                                INNER JOIN SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno
                                INNER JOIN NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona
                                WHERE PA.idProgramaAlumno = @idProgramaAlumno";

                using (SqlCommand commandPrograma = new SqlCommand(queryPrograma, connection))
                {
                    commandPrograma.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);

                    using (SqlDataReader readerPrograma = commandPrograma.ExecuteReader())
                    {
                        if (readerPrograma.Read())
                        {
                            // Cargar los datos básicos en los controles de la página
                            txtNombrePresentador.Text = readerPrograma["sNombre_Completo"].ToString();
                            txtNombrePrograma.Text = readerPrograma["sNombre_Programa"].ToString();
                            txtResponsablePrograma.Text = readerPrograma["sResponsable"].ToString();
                            txtFechaInicio.Text = Convert.ToDateTime(readerPrograma["dfechaRegistro"]).ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
        }
        protected void btnLiberarEstudiante_Click(object sender, EventArgs e)
        {
            string idProgramaAlumno = Request.QueryString["nst"];
            string connectionString = GlobalConstants.SQL;

            string updateQuery = "UPDATE SM_PROGRAMA_ALUMNO SET kpEstatus = @Estatus WHERE idProgramaAlumno = @idProgramaAlumno";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@Estatus", 22116);
                    command.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        // Redirige a la página AlumnosPostulados.aspx si el método se ejecuta exitosamente
                        Response.Redirect("AlumnosPostulados.aspx");
                    }
                        catch (Exception ex)
                        {
                    //Console.WriteLine("Error: " + ex.Message);
                }
            }


        }
        }
            protected void btnDescargarFormatoWord_Click(object sender, EventArgs e)
        {
            // Obtén el ID del registro desde el CommandArgument
            string idProgramaAlumno = Request.QueryString["nst"];
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            string connectionString = GlobalConstants.SQL;

            // Validación de fechas
            if (string.IsNullOrEmpty(txtFechaInicio.Text) || string.IsNullOrEmpty(txtFechaFin.Text))
            {
                // Mostrar el modal si las fechas no están llenas
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", "$('#ModalDatosFaltantes').modal('show');", true);
                return; // No continuar con la generación si las fechas no están completas
            }

            try
            {
                // Conexión a la base de datos
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @" SELECT USA.ID ,US.dFechaRegistro, US.sCorreo AS Correo, ALU.sMatricula AS Matricula, PER.sNombre_completo AS Alumno, PLA.sClave + ' - ' + PLA.sDescripcion AS PlanEstudio, 
                                     ESC.sDescripcion AS Escuela, EST.sDescripcion AS EstadoAutorizacion 
                                    FROM SM_PROGRAMA_ALUMNO AS PA
									INNER JOIN SM_USUARIOS_ALUMNOS AS USA ON PA.kmAlumno = USA.ID
									INNER JOIN SM_USUARIO AS US ON USA.kmUsuario = US.idUsuario
									INNER JOIN SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno 
                                    INNER JOIN NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona INNER JOIN SP_ESCUELA_UAC AS ESC ON USA.kpEscuela = ESC.idEscuelaUAC 
                                    INNER JOIN SP_PLAN_ESTUDIO AS PLA ON USA.kpPlan = PLA.idPlanEstudio INNER JOIN NP_ESTATUS AS EST ON USA.bAutorizado = EST.idEstatus WHERE 
									PA.idProgramaAlumno =" + idProgramaAlumno;
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Obtener los valores de las fechas desde el formulario
                        string fechaInicio = DateTime.Parse(txtFechaInicio.Text).ToString("dd/MM/yyyy");
                        string fechaFin = DateTime.Parse(txtFechaFin.Text).ToString("dd/MM/yyyy");
                        // Datos requeridos para el documento
                        string director = "(Nombre del Director de la Escuela o Facultad)"; // Deberías obtenerlo también
                        string escuela = reader["Escuela"].ToString();
                        string alumno = reader["Alumno"].ToString();
                        string matricula = reader["Matricula"].ToString();
                        string planEstudios = reader["PlanEstudio"].ToString();
                        //string fechaInicio = "(día, mes y año del inicio)"; // Deberías obtenerlo también
                        //string fechaFin = "(día, mes y año de término)"; // Deberías obtenerlo también
                        string numHoras = "´número de horas obligatorias)"; // Deberías obtenerlo también
                        string actividades = "(descripción general de las tareas o actividades realizadas en la prestación)"; // Deberías obtenerlo también
                                                                                                                              // Formato de la fecha actual
                        string fechaHoy = "(Lugar y fecha)"; /*DateTime.Now.ToString("dddd, dd MMMM yyyy");*/
                        // Generar el documento Word
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true))
                            {
                                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                                // Crear las propiedades de sección
                                SectionProperties sectionProps = new SectionProperties(
                                    new PageMargin()
                                    {
                                        Top = 1440,     // 1 pulgada (en twips)
                                        Right = 1440,   // 1 pulgada
                                        Bottom = 1440,  // 1 pulgada
                                        Left = 1440,    // 1 pulgada
                                        Header = 720,   // 0.5 pulgada
                                        Footer = 720,   // 0.5 pulgada
                                        Gutter = 0      // Sin espacio adicional
                                    }
                                );

                                // Crear el documento con el contenido
                                mainPart.Document = new Document(new Body(
                                    sectionProps,
                                    new Paragraph(
                                new ParagraphProperties(
                                    new Justification() { Val = JustificationValues.Right }),
                                new Run(new Text(fechaHoy))),
                                    new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new RunProperties(new Bold()), new Text(director))),

                                    new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new Text($"Director de {escuela}"))),

                                    new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new Text("P r e s e n t e .-"))),

                                    new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new Text($"Por medio de la presente ")),
                                        new Run(new RunProperties(new Bold()), new Text(" HACE CONSTAR ")),
                                        new Run(new Text($" que el/la C. {alumno} con matrícula ")),
                                        new Run(new RunProperties(new Bold()), new Text(matricula)),
                                        new Run(new Text($" estudiante de {planEstudios} de la UNIVERSIDAD AUTÓNOMA DE COAHUILA, realizó su servicio social en (nombre de la institución donde se prestó el Servicio Social), durante el periodo comprendido del {fechaInicio} al {fechaFin} cubriendo un total de {numHoras} horas efectivas, desarrollando como actividades {actividades}."))),

                                    new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new Text("Sin más por el momento, y agradeciendo de antemano sus intenciones, me despido."))),

                                    new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new Text("Atentamente"))),

                                     new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
                                        new Run(new Text("(Nombre y puesto de quien responde a nombre de la institución donde se prestó el servicio)")))
                                ));
                            }

                            // Enviar el archivo al cliente
                            Response.Clear();
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            Response.AddHeader("content-disposition", "attachment;filename=DocumentoGenerado.docx");
                            Response.BinaryWrite(ms.ToArray());
                            Response.Flush();  // Envía el contenido al cliente sin abortar el hilo
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error para depuración
                //LogError("Error en btnLiberar_Click: " + ex.Message);
                Response.Write("<script>alert('Ha ocurrido un error al generar el documento. Inténtelo nuevamente.');</script>");
            }
        }
    }
}