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
                string programa = row["kpEstatus"].ToString().Trim();

                if (string.IsNullOrEmpty(programa))
                {
                    btnAnular.Visible = false;
                }
                else
                {
                    if (programa == "7")
                        btnAnular.Visible = false;
                    if (programa == "20707")
                        btnAnular.Visible = true;
                    if (programa == "21522")
                        btnAnular.Visible = false;
                    if (programa == "22113")
                        btnAnular.Visible = false;
                }
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