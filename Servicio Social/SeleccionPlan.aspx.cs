using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace Servicio_Social
{
    public partial class SeleccionPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarEscuela();

            }
        }

        public void cargarEscuela()
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT UO.idEscuelaUAC, UO.sClave + ' - ' + UO.sDescripcion AS Escuela " +
                "FROM SM_USUARIOS_ALUMNOS UA " +
                "INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno " +
                "INNER JOIN NM_PERSONA P ON P.idPersona = AL.kmPersona " +
                "INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = UA.kpEscuela " +
                "WHERE AL.sMatricula = @sMatricula AND UA.bAutorizado <> 99 ORDER BY 2";
            string matricula = Session["matricula"].ToString();

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sMatricula", matricula);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlEscuela.DataSource = reader;
                        ddlEscuela.DataValueField = "idEscuelaUAC";
                        ddlEscuela.DataTextField = "Escuela";
                        ddlEscuela.DataBind();
                    }
                }
            }
            ddlEscuela.Items.Insert(0, new ListItem("Seleccione una escuela", ""));
        }

        private void cargarPlan(int escuela)
        {
            string connectionString = GlobalConstants.SQL;
            string query = "SELECT PE.idPlanEstudio, " +
                "(CASE UA.bAutorizado WHEN 11 THEN PE.sClave + ' - ' + PE.sDescripcion + ' (Autorizado)' ELSE  PE.sClave + ' - ' + PE.sDescripcion + ' (No Autorizado)' END) AS Planes " +
                "FROM SM_USUARIOS_ALUMNOS UA " +
                "INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno " +
                "INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan " +
                "WHERE AL.sMatricula LIKE @matricula AND UA.kpEscuela = @escuela AND UA.bAutorizado <> 99 ORDER BY 2";
            string matricula = Session["matricula"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    cmd.Parameters.AddWithValue("@escuela", escuela);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlPlan.DataSource = reader;
                        ddlPlan.DataValueField = "idPlanEstudio";
                        ddlPlan.DataTextField = "Planes";
                        ddlPlan.DataBind();
                    }
                }
            }

            // Agregar una opción de selección por defecto si es necesario
            ddlPlan.Items.Insert(0, new ListItem("Seleccione un plan", ""));
        }

        protected void ddlEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEscuela.SelectedIndex > 0)
            {
                int escuela = int.Parse(ddlEscuela.SelectedValue);
                cargarPlan(escuela);
                lblError.Text = "";
            }
            else
            {
                ddlPlan.Items.Clear();
                lblError.Text = "";
            }
        }

        protected void bntSeleccionar_Click(object sender, EventArgs e)
        {
            string idEscuela = ddlEscuela.SelectedValue;
            string idPlan = ddlPlan.SelectedValue;
            string idAlumno = getidAlumno(idEscuela, idPlan);
            if (string.IsNullOrEmpty(idAlumno))
            {
                lblError.Text = "Necesita ser autorizado en su Escuela para ingresar con los datos seleccionados.";
            }
            else
            {
                string sesion = Session["tipoUsuario"].ToString();
                Session["tipoUsuario"] = sesion + '|' + idAlumno;
                Session["plan"] = obtenerPlan(idAlumno);
                Response.Redirect("PanelEstudiante.aspx");
            }

        }
        public string obtenerPlan(string id)
        {
            string plan = "";
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;

            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT PE.sClave + ' - ' + PE.sDescripcion AS planes " +
                "FROM SM_USUARIOS_ALUMNOS UA " +
                "INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan " +
                "WHERE UA.ID = @id";

            List<(string sMatricula, string Autorizado)> registros = new List<(string sMatricula, string Autorizado)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agrega el parámetro del nombre de usuario a la consulta
                    command.Parameters.AddWithValue("@id", id);
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Obtener los valores 
                            plan = reader["planes"].ToString();
                        }
                    }
                }
            }

            return plan;
        }
        public string getidAlumno(string escuela, string plan)
        {
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;
            string matricula = Session["matricula"].ToString();
            string idAlumno = "";

            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT UA.ID " +
                "FROM SM_USUARIO U " +
                "INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.kmUsuario = U.idUsuario " +
                "INNER JOIN SM_ALUMNO AL ON AL.idAlumno = U.kmIdentificador " +
                "WHERE AL.sMatricula = @sMatricula AND UA.kpEscuela = @escuela AND UA.kpPlan = @plan " +
                "AND UA.bAutorizado = 11; ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agrega el parámetro del nombre de usuario a la consulta
                    command.Parameters.AddWithValue("@sMatricula", matricula);
                    command.Parameters.AddWithValue("@escuela", escuela);
                    command.Parameters.AddWithValue("@plan", plan);
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Obtener los valores de kpPlan y kpEscuela encontrados
                            idAlumno = reader["ID"].ToString();
                        }
                    }
                }
            }
            return idAlumno;
        }
    }
}