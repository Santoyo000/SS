using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Servicio_Social.Dependencias1;

namespace Servicio_Social
{
    public partial class PerfilEstudiante : System.Web.UI.Page
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
                CargarDatos();
                CargarDatosOracle();
            }
        }

        public void CargarDatos()
        {
            string id = Session["tipoUsuario"].ToString().Split('|')[2];
            string conStrin = GlobalConstants.SQL;
            string query = "SELECT UN.sCiudad, UO.SCLAVE + ' - '  + UO.sDescripcion AS sEscuela, PE.sClave + ' - ' + PE.sDescripcion AS sPlan, AL.sMatricula, " +
                "P.sNombres, P.sApellido_paterno, P.sApellido_materno, P.kpSexo, U.sCorreo, P.sTelefono, p.idPersona " +
                "FROM SM_USUARIOS_ALUMNOS UA " +
                "INNER JOIN SM_USUARIO U ON U.idUsuario = UA.kmUsuario " +
                "INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno " +
                "INNER JOIN NM_PERSONA P ON P.idPersona = AL.kmPersona " +
                "INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = UA.kpEscuela " +
                "INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan " +
                "INNER JOIN NP_UNIDAD UN ON UN.idUnidad = UO.kpUnidad " +
                "WHERE UA.ID = @id; ";

            using (SqlConnection connection = new SqlConnection(conStrin))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id); // Asignar valor al parámetro Id

                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Obtener datos de la consulta
                            txtUnidad.Text = reader["sCiudad"].ToString();
                            txtEscuela.Text = reader["sEscuela"].ToString();
                            txtPlanEstudios.Text = reader["sPlan"].ToString();
                            txtMatricula.Text = reader["sMatricula"].ToString();
                            txtNombre.Text = reader["sNombres"].ToString();
                            txtApePat.Text = reader["sApellido_paterno"].ToString();
                            txtApeMat.Text = reader["sApellido_materno"].ToString();
                            ddlSexo.SelectedValue = reader["kpSexo"].ToString();
                            txtCorreo.Text = reader["sCorreo"].ToString();
                            txtTelefono.Text = reader["sTelefono"].ToString();
                        }
                    }
                }
            }
        }

        public void CargarDatosOracle()
        {
            string connectionString = GlobalConstants.ORA;
            string matricula = Session["matricula"].ToString();

            // Query para verificar si el correo existe en la base de datos
            string query = "SELECT DIRECCION, COLONIA, CIUDAD, ESTADO, COD_POS " +
                "FROM AA.GALUMNOS " +
                "WHERE MATRICULA LIKE :matricula";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("matricula", matricula));
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txtDomicilio.Text = reader["DIRECCION"].ToString().Trim();
                            txtColonia.Text = reader["COLONIA"].ToString().Trim();
                            //txtCiudad.Text = reader["CIUDAD"].ToString().Trim();
                            //txtEstado.Text = reader["ESTADO"].ToString().Trim();
                            //txtCP.Text = reader["COD_POS"].ToString().Trim();
                        }
                    }
                }
            }
        }
    }
}