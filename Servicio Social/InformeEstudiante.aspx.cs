using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using Table = CrystalDecisions.CrystalReports.Engine.Table;
using System.Data;

namespace Servicio_Social
{
    public partial class InformeEstudiante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipoUsuario"] == null)
            {
                Response.Redirect("LoginEstudiante.aspx");
            }
            //else 
            //{
            //    Response.Redirect("InformeEstudiante.aspx");
            //}
            if (!IsPostBack) // Solo cargar datos en la primera carga
            {
                string idProgramaAlumno = Request.QueryString["idProgramaAlumno"];
                if (!string.IsNullOrEmpty(idProgramaAlumno))
                {
                    CargarDatos(int.Parse(idProgramaAlumno));
                }
            }
        }
        private void CargarDatos(int idProgramaAlumno)
        {
            // Cadena de conexión desde web.config
            string connectionString = GlobalConstants.SQL;
            string kmUserAlumno = Session["ID"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Primera consulta: siempre carga los datos básicos del programa y alumno
                string queryPrograma = @"SELECT PA.dfechaRegistro, 
                                        COALESCE(PER.sNombres, '') + ' ' + 
                                        COALESCE(PER.sApellido_Paterno, '') + ' ' + 
                                        COALESCE(PER.sApellido_Materno, '') AS sNombre_Completo,
                                        P.sNombre_Programa, sResponsable
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


                // Consultar si el registro ya existe en SM_DATOS_INFORME_PROGRAMA
                string queryInforme = @"SELECT sActividades_Des, sProblacion_Programa, sMetas_Resul, dFecha_inicio, dFecha_fin
                            FROM SM_DATOS_INFORME_PROGRAMA
                            WHERE kmUserAlumno = @kmUserAlumno AND kmProgramaAlumno = @idProgramaAlumno";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand commandInforme = new SqlCommand(queryInforme, connection))
                {
                    commandInforme.Parameters.AddWithValue("@kmUserAlumno", kmUserAlumno);
                    commandInforme.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);

                    using (SqlDataReader readerInforme = commandInforme.ExecuteReader())
                    {
                        if (readerInforme.Read())
                        {
                            // Cargar datos del informe ya existente
                            txtActividades.Text = readerInforme["sActividades_Des"].ToString();
                            txtPoblacionBeneficiada.Text = readerInforme["sProblacion_Programa"].ToString();
                            txtMetasResultados.Text = readerInforme["sMetas_Resul"].ToString();
                            txtFechaInicio.Text = Convert.ToDateTime(readerInforme["dFecha_inicio"]).ToString("yyyy-MM-dd");
                            txtFechaFin.Text = Convert.ToDateTime(readerInforme["dFecha_fin"]).ToString("yyyy-MM-dd");

                            return; // Salir del método si los datos del informe se cargaron
                        }
                    }
                }

                // Si no hay datos en SM_DATOS_INFORME_PROGRAMA, cargar los datos del programa y del alumno
                string queryPrograma = @"SELECT PA.dfechaRegistro, PER.sNombres + ' ' + PER.sApellido_Paterno + ' ' + PER.sApellido_Materno AS sNombre_Completo,
                                 P.sNombre_Programa, sResponsable
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
                            txtNombrePresentador.Text = readerPrograma["sNombre_Completo"].ToString();
                            txtNombrePrograma.Text = readerPrograma["sNombre_Programa"].ToString();
                            txtResponsablePrograma.Text = readerPrograma["sResponsable"].ToString();
                            txtFechaInicio.Text = Convert.ToDateTime(readerPrograma["dfechaRegistro"]).ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            
            // Obtener los datos del formulario
            string actividades = txtActividades.Text;
            string poblacionBeneficiada = txtPoblacionBeneficiada.Text;
            string metasResultados = txtMetasResultados.Text;

            // Obtener el id del programa alumno de la query string
            string idProgramaAlumno = Request.QueryString["idProgramaAlumno"];
            string kmUserAlumno = Session["ID"].ToString();
            int kmProgramaAlumno = !string.IsNullOrEmpty(idProgramaAlumno) ? int.Parse(idProgramaAlumno) : 0;

            // Guardar los datos en la base de datos
            GuardarDatosInforme(kmUserAlumno, kmProgramaAlumno, actividades, poblacionBeneficiada, metasResultados);

            // Generar el reporte después de guardar
            GenerarReporte(int.Parse(idProgramaAlumno));
        }
        private void GuardarDatosInforme(string kmUserAlumno,int kmProgramaAlumno, string actividades, string poblacionBeneficiada, string metasResultados)
        {
            // Cadena de conexión desde web.config
            string connectionString = GlobalConstants.SQL;

            // Consulta para verificar si ya existe un registro
            string checkQuery = @"SELECT COUNT(*) FROM SM_DATOS_INFORME_PROGRAMA 
                          WHERE kmUserAlumno = @kmUserAlumno AND kmProgramaAlumno = @kmProgramaAlumno";

            // Consultas para insertar o actualizar
            string insertQuery = @"INSERT INTO SM_DATOS_INFORME_PROGRAMA (kmUserAlumno, kmProgramaAlumno, sActividades_Des, sProblacion_Programa, sMetas_Resul, dFecha_inicio, dFecha_fin) 
                           VALUES (@kmUserAlumno, @kmProgramaAlumno, @sActividades_Des, @sProblacion_Programa, @sMetas_Resul, @dFecha_inicio, @dFecha_fin)";

            string updateQuery = @"UPDATE SM_DATOS_INFORME_PROGRAMA 
                           SET sActividades_Des = @sActividades_Des, 
                               sProblacion_Programa = @sProblacion_Programa, 
                               sMetas_Resul = @sMetas_Resul, 
                               dFecha_inicio = @dFecha_inicio, 
                               dFecha_fin = @dFecha_fin 
                           WHERE kmUserAlumno = @kmUserAlumno AND kmProgramaAlumno = @kmProgramaAlumno";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el registro ya existe
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@kmUserAlumno", kmUserAlumno);
                    checkCommand.Parameters.AddWithValue("@kmProgramaAlumno", kmProgramaAlumno);

                    int recordExists = (int)checkCommand.ExecuteScalar();

                    // Decidir si realizar un INSERT o UPDATE
                    string query = recordExists > 0 ? updateQuery : insertQuery;

                    // Ejecutar el INSERT o UPDATE
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kmUserAlumno", kmUserAlumno);
                        command.Parameters.AddWithValue("@kmProgramaAlumno", kmProgramaAlumno);
                        command.Parameters.AddWithValue("@sActividades_Des", actividades);
                        command.Parameters.AddWithValue("@sProblacion_Programa", poblacionBeneficiada);
                        command.Parameters.AddWithValue("@sMetas_Resul", metasResultados);
                        command.Parameters.AddWithValue("@dFecha_inicio", Convert.ToDateTime(txtFechaInicio.Text));
                        command.Parameters.AddWithValue("@dFecha_fin", Convert.ToDateTime(txtFechaFin.Text));

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Mostrar el modal de éxito
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        }

        private void GenerarReporte(int kmProgramaAlumno)
        {
            //string connectionString = GlobalConstants.SQL;
            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(connectionString))
            //    {
            //        connection.Open();

            //        // Configurar el DataSet para el reporte
            //        DataSet dsInforme = new DataSet();
            //        SqlCommand cmd = connection.CreateCommand();
            //        SqlDataAdapter data = new SqlDataAdapter();

            //        // Configurar el procedimiento almacenado y sus parámetros
            //        cmd.CommandText = "sp_Informe_Estudiante_ss";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("kmProgramaAlumno", SqlDbType.Int).Value = kmProgramaAlumno;
            //        data.SelectCommand = cmd;

            //        // Llenar el DataSet
            //        data.Fill(dsInforme);
            //        connection.Close();

            //        if (dsInforme.Tables[0].Rows.Count > 0)
            //        {
            //            try
            //            {
            //                using (ReportDocument reporte = new ReportDocument())
            //                {
            //                    // Cargar el reporte de Crystal Reports
            //                    string reportPath = Server.MapPath("~/Reportes/InformeFinalSS.rpt");
            //                    reporte.Load(reportPath);



            //                    // Asignar la fuente de datos al informe
            //                    reporte.SetDataSource(dsInforme);

            //                    // Establecer el valor del parámetro después de asignar la fuente de datos
            //                    if (reporte.ParameterFields["@kmProgramaAlumno"] != null)
            //                    {
            //                        reporte.SetParameterValue("@kmProgramaAlumno", kmProgramaAlumno);
            //                    }
            //                     // Configurar información de conexión para cada tabla en el informe
            //                    ConnectionInfo connectionInfo = new ConnectionInfo
            //                    {
            //                        ServerName = "148.212.19.202",
            //                        DatabaseName = "PDU202",
            //                        UserID = "sa",
            //                        Password = "PDU2021*."
            //                    };

            //                    foreach (Table table in reporte.Database.Tables)
            //                    {
            //                        TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
            //                        tableLogOnInfo.ConnectionInfo = connectionInfo;
            //                        table.ApplyLogOnInfo(tableLogOnInfo);
            //                    }

            //                    // Exportar el reporte a un Stream en formato PDF
            //                    using (Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat))
            //                    {
            //                        // Convertir el Stream en un array de bytes
            //                        byte[] bytes = new byte[stream.Length];
            //                        stream.Read(bytes, 0, (int)stream.Length);

            //                        // Convertir los bytes a una cadena Base64
            //                        string base64String = Convert.ToBase64String(bytes);

            //                        // Renderizar el PDF en un iframe
            //                        rptVistaPrevia.Attributes["src"] = "data:application/pdf;base64," + base64String;
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                // Manejar errores al generar el reporte
            //                Response.Write("<script>alert('Error al generar el reporte: " + ex.Message + "');</script>");
            //            }
            //        }
            //        else
            //        {
            //            // Mostrar mensaje de error si no hay datos
            //            rptVistaPrevia.Visible = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Manejar errores de conexión o SQL
            //    Response.Write("<script>alert('Ha ocurrido un error: " + ex.Message + "');</script>");
            //}


            try
            {
                // Ruta del archivo .rpt de Crystal Reports
                string reportPath = Server.MapPath("~/Reportes/InformeFinalSS.rpt");

                using (ReportDocument reporte = new ReportDocument())
                {
                    // Cargar el reporte
                    reporte.Load(reportPath);

                    // Establecer el login para la conexión de base de datos
                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = "148.212.19.202",
                        DatabaseName = "PDU202",
                        UserID = "sa",
                        Password = "PDU2021*."
                    };

                    // Iterar a través de las tablas del reporte y establecer la conexión
                    foreach (Table table in reporte.Database.Tables)
                    {
                        TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                        tableLogOnInfo.ConnectionInfo = connectionInfo;
                        table.ApplyLogOnInfo(tableLogOnInfo);
                    }

                    // Pasar el parámetro al reporte
                    reporte.SetParameterValue("@kmProgramaAlumno", kmProgramaAlumno);

                    // Exportar el reporte a un Stream en formato PDF
                    using (Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat))
                    {
                        // Convierte el Stream en un array de bytes
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);

                        // Convierte los bytes a una cadena Base64
                        string base64String = Convert.ToBase64String(bytes);

                        // Renderiza el PDF dentro de un iframe en la página


                        // Asignar el PDF a un control oculto en la página
                        hiddenPdfBase64.Value = base64String;

                        // Mostrar el modal con el PDF

                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPdfModal", "ocultarOverlay(); $('#pdfModal').modal('show');", true);

                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // Mostrar detalles del error en caso de que ocurra una excepción
                Response.Write("<script>alert('Error al generar el reporte: " + ex.Message + "');</script>");
            }
            catch (Exception ex)
            {
                // Capturar cualquier otro tipo de error
                Response.Write("<script>alert('Ha ocurrido un error inesperado: " + ex.Message + "');</script>");
            }


        }

    }
    
}
    

    
