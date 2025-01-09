//using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class ProgramasAlumno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipoUsuario"] == null)
            {
                Response.Redirect("LoginEstudiante.aspx");
            }
            else
            {
                try
                {
                    string id = Session["tipoUsuario"].ToString().Split('|')[2];
                }
                catch
                {
                    Response.Redirect("SeleccionPlan.aspx");
                }
            }
            if (!IsPostBack)
            {
                CargarDatos(0, "");
            }
        }

        #region Botones Paginado
        private int CurrentPage
        {
            get
            {
                return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 0;
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        private int TotalPages
        {
            get
            {
                return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0;
            }
            set
            {
                ViewState["TotalPages"] = value;
            }
        }
        #endregion

        #region Llenar Grid

        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;
            string idAlumno = Session["tipoUsuario"].ToString().Split('|')[2];

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString)) 
            {
                SqlCommand countCmd = new SqlCommand("sp_ContarProgramasSeleccionados_ss", con);
                countCmd.CommandType = CommandType.StoredProcedure;
                countCmd.Parameters.AddWithValue("@idAlumno", idAlumno);

                con.Open();

                totalRecords = (int)countCmd.ExecuteScalar();

                SqlCommand cmd = new SqlCommand("sp_ObtenerProgramasSeleccionados_ss", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            return dt;
        }
        protected void CargarDatos(int pageIndex, string searchTerm)
        {
            int pageSize = 10; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatos(pageIndex, pageSize, searchTerm, out totalRecords);

            RepeaterProgramas.DataSource = dt;
            RepeaterProgramas.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPrevious.Enabled = pageIndex > 0;
            btnNext.Enabled = pageIndex < TotalPages - 1;

            // Actualiza la etiqueta de número de página
            lblPageNumber.Text = $"Página {pageIndex + 1} de {TotalPages}";
        }

        protected void RepeaterProgramas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                LinkButton btnAnular = (LinkButton)e.Item.FindControl("btnAnular");
                LinkButton btnEvaluacion = (LinkButton)e.Item.FindControl("btnEvaluacion");
                LinkButton btnLiberar = (LinkButton)e.Item.FindControl("btnLiberar");
                string programa = row["kpEstatus"].ToString().Trim();

                // Diccionario para mapear las configuraciones de visibilidad según el estado
                var visibilidadBotones = new Dictionary<string, Action>
        {
            { "7", () => { btnAnular.Visible = false; btnEvaluacion.Visible = false; btnLiberar.Visible = false; } },     // CANCELADO
            { "20707", () => { btnAnular.Visible = true; btnEvaluacion.Visible = false; btnLiberar.Visible = false; } }, // EN ESPERA
            { "21522", () => { btnAnular.Visible = false; btnEvaluacion.Visible = false; btnLiberar.Visible = false; } }, // AUTORIZADO POR DEPENDENCIA
            { "22113", () => { btnAnular.Visible = false; btnEvaluacion.Visible = false; btnLiberar.Visible = false; } }, // NO AUTORIZADO POR DEPENDENCIA
            { "22116", () => {
                bool encuestaRealizada = AlumnoYaHizoEncuesta();
                btnAnular.Visible = false;
                btnEvaluacion.Visible = !encuestaRealizada; // Visible si no hizo encuesta
                btnLiberar.Visible = encuestaRealizada;    // Visible si ya hizo encuesta
            } }, // LIBERADO DEP
            { "42186", () => {
                bool encuestaRealizada = AlumnoYaHizoEncuesta();
                btnAnular.Visible = false;
                btnEvaluacion.Visible = !encuestaRealizada;
                btnLiberar.Visible = encuestaRealizada; // Visible si ya hizo encuesta
            } }, // LIBERADO ESC
            { "42187", () => {
                bool encuestaRealizada = AlumnoYaHizoEncuesta();
                btnAnular.Visible = false;
                btnEvaluacion.Visible = !encuestaRealizada;
                btnLiberar.Visible = encuestaRealizada; // Visible si ya hizo encuesta
            } }, // LIBERADO UNI
            { "42188", () => { btnAnular.Visible = false; btnEvaluacion.Visible = false; btnLiberar.Visible = false; } }  // LIBERADO DSS
        };

                // Configuración por defecto si el programa es nulo o no está en el diccionario
                if (string.IsNullOrEmpty(programa) || !visibilidadBotones.ContainsKey(programa))
                {
                    btnAnular.Visible = false;
                    btnEvaluacion.Visible = false;
                    btnLiberar.Visible = false;
                }
                else
                {
                    // Ejecutar la configuración correspondiente al estatus
                    visibilidadBotones[programa].Invoke();
                }
            }
        

        //protected void RepeaterProgramas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        DataRowView row = (DataRowView)e.Item.DataItem;
        //        LinkButton btnAnular = (LinkButton)e.Item.FindControl("btnAnular");
        //        LinkButton btnEvaluacion = (LinkButton)e.Item.FindControl("btnEvaluacion");
        //        LinkButton btnLiberar = (LinkButton)e.Item.FindControl("btnLiberar");
        //        string programa = row["kpEstatus"].ToString().Trim();

        //        // Diccionario para mapear las configuraciones de visibilidad según el estado
        //        var visibilidadBotones = new Dictionary<string, (bool Anular, bool Evaluacion, bool Liberar)>
        //{
        //    { "7", (false, false, false) },      // CANCELADO
        //    { "20707", (true, false, false) },  // EN ESPERA
        //    { "21522", (false, false, false) }, // AUTORIZADO POR DEPENDENCIA
        //    { "22113", (false, false, false) }, // NO AUTORIZADO POR DEPENDENCIA
        //    { "22116", (false, true, false) }  , // LIBERADO DEP
        //    { "42186", (false, false, false) } ,  // LLIBERADO ESC
        //    { "42187", (false, false, false) }  , // LIBERADO UNI
        //    { "42188", (false, false, false) }   // LIBERADO DSS
        //};

        //        // Configuración por defecto si programa es nulo o no está en el diccionario
        //        (bool Anular, bool Evaluacion, bool Liberar) configuracion = (false, false, false);

        //        // Obtener configuración según el estado
        //        if (!string.IsNullOrEmpty(programa) && visibilidadBotones.ContainsKey(programa))
        //        {
        //            configuracion = visibilidadBotones[programa];
        //        }

        //        // Asignar visibilidad por defecto
        //        btnAnular.Visible = configuracion.Anular;
        //        btnEvaluacion.Visible = configuracion.Evaluacion;
        //        btnLiberar.Visible = configuracion.Liberar;

        //        // Manejo especial para "42188" (LIBERADO DSS)
        //        if (programa == "22116")
        //        {
        //            bool encuestaRealizada = AlumnoYaHizoEncuesta();
        //            btnEvaluacion.Visible = !encuestaRealizada;
        //            btnLiberar.Visible = encuestaRealizada;
        //        }
        //    }


        //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //{
        //    DataRowView row = (DataRowView)e.Item.DataItem;
        //    LinkButton btnAnular = (LinkButton)e.Item.FindControl("btnAnular");
        //    string programa = row["kpEstatus"].ToString().Trim();
        //    LinkButton btnEvaluacion = (LinkButton)e.Item.FindControl("btnEvaluacion");
        //    LinkButton btnLiberar = (LinkButton)e.Item.FindControl("btnLiberar");

        //    if (string.IsNullOrEmpty(programa))
        //    {
        //        btnAnular.Visible = false;
        //        btnEvaluacion.Visible = false;
        //        btnLiberar.Visible = false;
        //    }
        //    else
        //    {
        //        if (programa == "7") //7	CANCELADO
        //        { 
        //            btnAnular.Visible = false;
        //            btnEvaluacion.Visible = false;
        //            btnLiberar.Visible = false;
        //        }
        //        if (programa == "20707") //23	EN ESPERA
        //        { 
        //            btnAnular.Visible = true;
        //            btnEvaluacion.Visible = false;
        //            btnLiberar.Visible = false;
        //        }
        //        if (programa == "21522") //24	AUTORIZADO POR DEPENDENCIA
        //        { 
        //            btnAnular.Visible = false;
        //            btnEvaluacion.Visible = false;
        //            btnLiberar.Visible = false;
        //        }
        //        if (programa == "22113") //27	NO AUTORIZADO POR DEPENDENCIA
        //        { 
        //            btnAnular.Visible = false;
        //            btnEvaluacion.Visible = false;
        //            btnLiberar.Visible = false;
        //        }
        //        if (programa == "42188") //36	LIBERADO DSS
        //        {
        //            btnAnular.Visible = false;
        //            btnEvaluacion.Visible = true;
        //            btnLiberar.Visible = false;

        //            // Ocultar o mostrar btnEvaluacion según el resultado de AlumnoYaHizoEncuesta
        //            bool encuestaRealizada = AlumnoYaHizoEncuesta();
        //            btnEvaluacion.Visible = !encuestaRealizada;
        //            btnLiberar.Visible = encuestaRealizada;
        //        }


        //    }



        //}
    }

        private bool AlumnoYaHizoEncuesta()
        {
            string idAlumno = Session["ID"].ToString();
            string connectionString = GlobalConstants.SQL;
            string query = "SELECT COUNT(1) FROM SM_EVALUACION_PROGRAMA_SS WHERE kmAlumno = @kmAlumno";

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
        #endregion

        #region Llenar modal 
        [WebMethod]
        public static string llenarDatosModal(int id)
        {
            string connectionString = GlobalConstants.SQL;
            var idAlumno = HttpContext.Current.Session["tipoUsuario"].ToString().Split('|')[2] as string;
            
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table class='result-table'>");
            htmlTable.Append("<thead><tr><th>Estatus</th><th>Fecha</th></tr></thead>");
            htmlTable.Append("<tbody>");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerDatosBitacora_ss", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string estatus = reader["ESTATUS"].ToString();
                            string fecha = reader["Fecha"].ToString();
                            htmlTable.Append("<tr>");
                            htmlTable.AppendFormat("<td>{0}</td>", estatus);
                            htmlTable.AppendFormat("<td>{0}</td>", fecha);
                            htmlTable.Append("</tr>");
                        }
                    }
                }
            }

            htmlTable.Append("</tbody>");
            htmlTable.Append("</table>");

            return htmlTable.ToString();
        }

        [WebMethod]
        public static string loadModalDataProgram(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string result = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_obtenerInformacionProgramaModal_ss", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        result += $"<p class='small-text'><strong>Dependencia:</strong> {reader["Dependencia"]}</p>";
                        result += $"<p class='small-text'><strong>Domicilio:</strong> {reader["sDomicilio"]}</p>";
                        result += $"<p class='small-text'><strong>Programa:</strong> {reader["sNombre_Programa"]}</p>";
                        result += $"<p class='small-text'><strong>Responsable:</strong> {reader["sResponsable"]}</p>";
                        result += $"<p class='small-text'><strong>Teléfono:</strong> {reader["sTelefono"]}</p>";
                        result += $"<p class='small-text'><strong>Correo:</strong> {reader["sCorreo"]}</p>";
                        result += $"<p class='small-text'><strong>Area Responsable:</strong> {reader["sAreaResponsable"]}</p>";
                        result += $"<p class='small-text'><strong>Objetivos:</strong> {reader["sObjetivos"]}</p>";
                        result += $"<p class='small-text'><strong>Actividades:</strong> {reader["sActividades_desarollo"]}</p>";
                        result += $"<p class='small-text'><strong>Horario:</strong> {reader["sHorario"]}</p>";

                        string mapaUrl = reader["slinkMaps"].ToString();
                        if (!string.IsNullOrEmpty(mapaUrl))
                        {
                            result += $"<p class='small-text'><strong>Mapa:</strong> <a href='{mapaUrl}' target='_blank'>Ver en Google Maps</a></p>";
                        }
                    }
                    con.Close();
                }
            }

            return result;

        }
        #endregion


        #region Botones
        protected void btnLiberar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string idPrograma = btn.CommandArgument;
            Response.Redirect("InformeEstudiante.aspx?idProgramaAlumno=" + idPrograma);
        }
        protected void btnEvaluarPrograma_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string nst = btn.CommandArgument;
            Response.Redirect("EvaluacionPrograma.aspx?nst=" + nst);
        }
        protected void btnAnular_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdate = (LinkButton)sender;
            string idPrograma = lnkUpdate.CommandArgument;
            string idAlumno = Session["tipoUsuario"].ToString().Split('|')[2];
            string estatus = "7";
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    insertarBitacora(idPrograma, idAlumno, estatus, connection, transaction);
                    updateProgramaAlumno(idPrograma, idAlumno, estatus, connection, transaction);
                    // Commit de la transacción si todo fue exitoso
                    transaction.Commit();
                    mensajeScript("Se ha liberado lugar del programa existosamente.");
                    CargarDatos(0, "");
                }
                catch (Exception ex)
                {
                    // Si ocurre algún error, realizar un rollback de la transacción
                    transaction.Rollback();

                }
            }
        }
        #endregion


        #region Metodos Bases de Datos 
        public void insertarBitacora(String idProgramaAlumno, string idAlumno, string estatus, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_BITACORA_PA (kmProgramaAlumno, kpEstatus, kpUsuario) " +
                "VALUES (@kmProgramaAlumno, @kpEstatus, @kpUsuario);";


            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kmProgramaAlumno", idProgramaAlumno);
                cmd.Parameters.AddWithValue("@kpEstatus", estatus);
                cmd.Parameters.AddWithValue("@kpUsuario", idAlumno);
                cmd.ExecuteNonQuery();
            }
        }
        public void updateProgramaAlumno(String idProgramaAlumno, string idAlumno, string estatus, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "UPDATE SM_PROGRAMA_ALUMNO SET kpEstatus = @kpEstatus, dFechaRegistro = GETDATE() " +
                "WHERE idProgramaAlumno = @idProgramaAlumno AND kmAlumno = @kmAlumno";


            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@idProgramaAlumno", idProgramaAlumno);
                cmd.Parameters.AddWithValue("@kpEstatus", estatus);
                cmd.Parameters.AddWithValue("@kmAlumno", idAlumno);
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Eventos 
        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }
        #endregion
    }
}