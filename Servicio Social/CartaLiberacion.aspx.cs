using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Table = CrystalDecisions.CrystalReports.Engine.Table;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using ZXing;

namespace Servicio_Social
{
    public partial class CartaLiberacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUser"] == null)
                Response.Redirect("Home.aspx");
            string idProgramaAlumno = Request.QueryString["nst"];
            if (!IsPostBack)
            {
                GenerarReporte(Convert.ToInt32(idProgramaAlumno));
            }
        }
            private void GenerarReporte(int idProgramaAlumno)
            {

                try
                {
                    // Ruta del archivo .rpt de Crystal Reports
                    string reportPath = Server.MapPath("~/Reportes/CartaLiberacion.rpt");

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
                        reporte.SetParameterValue("@kmProgramaAlumno", idProgramaAlumno);

                        // Exportar el reporte a un Stream en formato PDF
                        using (Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat))
                        {
                            // Convierte el Stream en un array de bytes
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, (int)stream.Length);

                            // Convierte los bytes a una cadena Base64
                            string base64String = Convert.ToBase64String(bytes);

                            // Renderiza el PDF dentro de un iframe en la página
                            string iframeContent = $"data:application/pdf;base64,{base64String}";
                            rptVistaPrevia.Attributes["src"] = iframeContent;
                            rptVistaPrevia.Visible = true;

                            // Asignar el PDF a un control oculto en la página
                            hiddenPdfBase64.Value = base64String;

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
        protected void ActualizarFecha_Click(object sender, EventArgs e)
        {
            try
            {
                // Muestra el overlay de carga
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showOverlay", "mostrarOverlay();", true);

                string fechaTexto = txtFechaInicio.Text.Trim();
                string idProgramaAlumno = Request.QueryString["nst"];
                string sContenidoCarta = "Por medio de la presente hacemos constar de C. ";

                // Validar si se ingresó una fecha válida en el TextBox
                DateTime fechaSeleccionada;
                bool fechaEsValida = DateTime.TryParseExact(fechaTexto, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fechaSeleccionada);

                string diaTexto = "", mes = "", anio = "", alumnoNombre = "", plan = "";
                string connectionString = GlobalConstants.SQL; // Tu cadena de conexión

                // Consulta SQL con cálculo de día en palabras y manejo de fecha seleccionada o actual
                string query = @"
                         SELECT  
             AL.SMATRICULA AS MATRICULA,
             PER.sNombres + ' ' + 
             ISNULL(NULLIF(PER.sApellido_paterno, ''), '') + ' ' + 
             ISNULL(NULLIF(PER.sApellido_materno, ''), '') AS NOMBRE_COMPLETO, 
             PE.sDescripcion AS PLANEST,
             CASE 
                 WHEN @FechaSeleccionada IS NOT NULL THEN @FechaSeleccionada 
                 ELSE GETDATE() 
             END AS FECHA_UTILIZADA,
             DATENAME(YEAR, CASE 
                 WHEN @FechaSeleccionada IS NOT NULL THEN @FechaSeleccionada 
                 ELSE GETDATE() 
             END) AS ANIO,
             CASE MONTH(CASE 
                 WHEN @FechaSeleccionada IS NOT NULL THEN @FechaSeleccionada 
                 ELSE GETDATE() 
             END)
                 WHEN 1 THEN 'Enero'
                 WHEN 2 THEN 'Febrero'
                 WHEN 3 THEN 'Marzo'
                 WHEN 4 THEN 'Abril'
                 WHEN 5 THEN 'Mayo'
                 WHEN 6 THEN 'Junio'
                 WHEN 7 THEN 'Julio'
                 WHEN 8 THEN 'Agosto'
                 WHEN 9 THEN 'Septiembre'
                 WHEN 10 THEN 'Octubre'
                 WHEN 11 THEN 'Noviembre'
                 WHEN 12 THEN 'Diciembre'
             END AS MES_TEXTO,
             CASE DAY(CASE 
                 WHEN @FechaSeleccionada IS NOT NULL THEN @FechaSeleccionada 
                 ELSE GETDATE() 
             END)
                 WHEN 1 THEN 'primero'
                 WHEN 2 THEN 'dos'
                 WHEN 3 THEN 'tres'
                 WHEN 4 THEN 'cuatro'
                 WHEN 5 THEN 'cinco'
                 WHEN 6 THEN 'seis'
                 WHEN 7 THEN 'siete'
                 WHEN 8 THEN 'ocho'
                 WHEN 9 THEN 'nueve'
                 WHEN 10 THEN 'diez'
                 WHEN 11 THEN 'once'
                 WHEN 12 THEN 'doce'
                 WHEN 13 THEN 'trece'
                 WHEN 14 THEN 'catorce'
                 WHEN 15 THEN 'quince'
                 WHEN 16 THEN 'dieciséis'
                 WHEN 17 THEN 'diecisiete'
                 WHEN 18 THEN 'dieciocho'
                 WHEN 19 THEN 'diecinueve'
                 WHEN 20 THEN 'veinte'
                 WHEN 21 THEN 'veintiuno'
                 WHEN 22 THEN 'veintidós'
                 WHEN 23 THEN 'veintitrés'
                 WHEN 24 THEN 'veinticuatro'
                 WHEN 25 THEN 'veinticinco'
                 WHEN 26 THEN 'veintiséis'
                 WHEN 27 THEN 'veintisiete'
                 WHEN 28 THEN 'veintiocho'
                 WHEN 29 THEN 'veintinueve'
                 WHEN 30 THEN 'treinta'
                 WHEN 31 THEN 'treinta y uno'
             END AS DIA_TEXTO
         FROM SM_PROGRAMA_ALUMNO PA
         INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
         INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
         INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.ID = PA.kmAlumno
         INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan
         INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno
         INNER JOIN NM_PERSONA PER ON PER.idPersona = AL.kmPersona
         WHERE PA.idProgramaAlumno = @idProgramaAlumno";

                // Obtener datos del estudiante y la fecha
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idProgramaAlumno", Convert.ToInt32(idProgramaAlumno));
                    cmd.Parameters.AddWithValue("@FechaSeleccionada", fechaEsValida ? (object)fechaSeleccionada : DBNull.Value);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            alumnoNombre = reader["NOMBRE_COMPLETO"].ToString();
                            plan = reader["PLANEST"].ToString();
                            diaTexto = reader["DIA_TEXTO"].ToString();
                            mes = reader["MES_TEXTO"].ToString();
                            anio = reader["ANIO"].ToString();
                        }
                    }
                }

                // Construir el contenido de la carta
                sContenidoCarta += alumnoNombre + ", quien cursó la carrera de " + plan + " de esta Universidad, ";
                sContenidoCarta += "cumplió satisfactoriamente con su Servicio Social, según lo dispuesto en el Artículo 10 de la Ley General de Educación así como en los Artículos 9 y 10 del reglamento para la Prestación del Servicio Social de los Estudiantes de las instituciones de Educación Superior en la República Mexicana y en los Artículos 4°, 5° y 6° del reglamento para la Prestación del Servicio Social de la Universidad Autónoma de Coahuila. ";
                sContenidoCarta += "\n\nSe extiende la presente con las facultades que confiere la fracción III del Artículo 15 del Reglamento para la Prestación del Servicio Social de la Universidad Autónoma de Coahuila.";
                sContenidoCarta += "\n\nSaltillo Coahuila de Zaragoza, a los " + diaTexto + " días del mes de " + mes + " del " + anio + ".";

                // Actualizar la carta en la base de datos
                string updateQuery = "UPDATE SM_PROGRAMA_ALUMNO SET sContenidoCarta = @sContenidoCarta WHERE idProgramaAlumno = @idProgramaAlumno";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@sContenidoCarta", sContenidoCarta);
                    cmd.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Generar reporte con la fecha actualizada
                GenerarReporte(Convert.ToInt32(idProgramaAlumno));

                // Ocultar el overlay después de completar la acción
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideOverlay", "ocultarOverlay();", true);

                // Mostrar el modal exitoso
                ScriptManager.RegisterStartupScript(this, GetType(), "MostrarModalExitoso", "$('#ModalExitoso').modal('show');", true);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorActualizarFecha", $"alert('Error: {ex.Message}');", true);
            }
        }
    }
}