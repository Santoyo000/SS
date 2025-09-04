using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Markup;

namespace Servicio_Social
{
    public partial class RegistroEstudiante1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                CargarConfiguracion();
            }
        }
        private void CargarConfiguracion()
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real

            // Query para obtener la configuración específica para Registro de Dependencias
            string query = "SELECT bActivo, dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE sClave = '3'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool activo = Convert.ToBoolean(reader["bActivo"]);
                            DateTime fechaInicio = Convert.ToDateTime(reader["dFechaInicio"]);
                            DateTime fechaFin = Convert.ToDateTime(reader["dFechaFin"]);
                            DateTime hoy = DateTime.Now;
                            string mensaje = reader["sMensaje"].ToString(); // Obtener el mensaje desde la BD

                            // Determinar si la fecha actual está dentro del rango permitido
                            bool dentroDelRango = hoy >= fechaInicio && hoy <= fechaFin;

                            // Configurar la visibilidad de los paneles de acuerdo a la configuración
                            pnlRegistro.Visible = dentroDelRango;
                            PanelCerrado.Visible = !dentroDelRango;
                            // Asignar el mensaje al control h3 del frontend
                            lblMensajeEstudiante.Text = mensaje;
                        }
                    }
                }
            }
        }
        protected void ddlTipoEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _tipo = ddlTipoEscuela.SelectedValue;
            switch (_tipo)
            {
                case "1":
                    lblCorreo.Text = "Correo Institucional:";
                    pnlPassword.Visible = false;
                    txtMatricula.Text = "";
                    txtCorreo.Text = "";
                    break;
                case "2":
                    lblCorreo.Text = "Correo Personal:";
                    pnlPassword.Visible = true;
                    txtMatricula.Text = "";
                    break;
                default:
                    lblCorreo.Text = "Correo:";
                    pnlPassword.Visible = false;
                    txtMatricula.Text = "";
                    break;
            }
        }

        public class AlumnoDto
        {
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public List<EscuelaDto> Escuelas { get; set; }
            public List<PlanEstudioDto> PlanesEstudio { get; set; } // <-- Agregar esta propiedad
        }

        public class EscuelaDto
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        public class PlanEstudioDto
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        [System.Web.Services.WebMethod]
        public static AlumnoDto GetAlumnoInfo(string buscar)
        {
            string oracleConnectionString = GlobalConstants.ORA;
            string sqlConnectionString = GlobalConstants.SQL;

            AlumnoDto alumno = null;
            List<EscuelaDto> escuelas = new List<EscuelaDto>();
            List<PlanEstudioDto> planesEstudio = new List<PlanEstudioDto>();

            string claveEscuela = "";
            string nombreEscuela = "";
            string clavePlan = "";
            string nombrePlan = "";

            using (OracleConnection connection = new OracleConnection(oracleConnectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(@"
        SELECT A.MATRICULA, A.NOMBRE AS NOMBRE_ALUMNO, A.APE_PAT, A.APE_MAT, 
               A.CURP, CASE WHEN A.SEXO = 1 THEN 'FEMENINO' ELSE 'MASCULINO' END AS SEXO, 
               P.CLAVE, P.NOMBRE AS PLAN_ESTUDIO, U.UNI_ORG, U.NOM_UNI_OR, A.EMAIL
        FROM AA.GALU_ESC E
        JOIN AA.GALUMNOS A ON A.MATRICULA = E.MATRICULA
        JOIN PLANESTUDIO.PLAN P ON P.CLAVE = E.CVE_PLAN
        JOIN AA.GUNI_ORG U ON U.UNI_ORG = E.UNI_ORG
        WHERE A.MATRICULA = :smatricula AND E.ESTATUS IN ('AR', 'AI') AND P.CLAVE NOT LIKE '742'  ORDER BY E.FEC_ALTA DESC", connection))  // SE QUITO EL NI
                {
                    command.Parameters.Add(new OracleParameter("smatricula", buscar ?? string.Empty));
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            alumno = new AlumnoDto
                            {
                                Nombre = reader["NOMBRE_ALUMNO"].ToString(),
                                ApellidoPaterno = reader["APE_PAT"].ToString(),
                                ApellidoMaterno = reader["APE_MAT"].ToString()
                            };
                            claveEscuela = reader["UNI_ORG"].ToString();
                            nombreEscuela = reader["NOM_UNI_OR"].ToString();
                            clavePlan = reader["CLAVE"].ToString();
                            nombrePlan = reader["PLAN_ESTUDIO"].ToString();
                        }
                    }
                }
            }

            if (alumno != null)
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    // Obtener las escuelas
                    using (SqlCommand sqlCommand = new SqlCommand(@"
            SELECT idEscuelaUAC, sClave, sDescripcion 
            FROM SP_ESCUELA_UAC 
            WHERE sClave = @claveEscuela", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@claveEscuela", claveEscuela);
                        using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                escuelas.Add(new EscuelaDto
                                {
                                    Id = sqlReader["idEscuelaUAC"].ToString(),
                                    Nombre = sqlReader["sDescripcion"].ToString()
                                });
                            }
                        }
                    }

                    // Obtener los planes de estudio
                    using (SqlCommand sqlCommand = new SqlCommand(@"
            SELECT idPlanEstudio, sClave, sDescripcion 
            FROM SP_PLAN_ESTUDIO 
            WHERE sClave = @clavePlan", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@clavePlan", clavePlan);
                        using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                planesEstudio.Add(new PlanEstudioDto
                                {
                                    Id = sqlReader["idPlanEstudio"].ToString(),
                                    Nombre = sqlReader["sDescripcion"].ToString()
                                });
                            }
                        }
                    }
                }

                // Asegurar que se devuelvan las listas correctamente
                alumno.Escuelas = escuelas.Count > 0 ? escuelas : new List<EscuelaDto>();
                alumno.PlanesEstudio = planesEstudio.Count > 0 ? planesEstudio : new List<PlanEstudioDto>();
            }

            return alumno;
        }


        //public static AlumnoDto GetAlumnoInfo(string buscar)
        //{
        //    string oracleConnectionString = GlobalConstants.ORA;
        //    string sqlConnectionString = GlobalConstants.SQL;

        //    AlumnoDto alumno = null;
        //    List<EscuelaDto> escuelas = new List<EscuelaDto>();
        //    List<PlanEstudioDto> planesEstudio = new List<PlanEstudioDto>();

        //    string claveEscuela = "";
        //    string nombreEscuela = "";
        //    string clavePlan = "";
        //    string nombrePlan = "";

        //    using (OracleConnection connection = new OracleConnection(oracleConnectionString))
        //    {
        //        connection.Open();
        //        using (OracleCommand command = new OracleCommand(@"
        //    SELECT A.MATRICULA, A.NOMBRE AS NOMBRE_ALUMNO, A.APE_PAT, A.APE_MAT, 
        //           A.CURP, CASE WHEN A.SEXO = 1 THEN 'FEMENINO' ELSE 'MASCULINO' END AS SEXO, 
        //           P.CLAVE, P.NOMBRE AS PLAN_ESTUDIO, U.UNI_ORG, U.NOM_UNI_OR, A.EMAIL
        //    FROM AA.GALU_ESC E
        //    JOIN AA.GALUMNOS A ON A.MATRICULA = E.MATRICULA
        //    JOIN PLANESTUDIO.PLAN P ON P.CLAVE = E.CVE_PLAN
        //    JOIN AA.GUNI_ORG U ON U.UNI_ORG = E.UNI_ORG
        //    WHERE A.MATRICULA = :smatricula AND E.ESTATUS IN ('AR', 'AI', 'NI')", connection))
        //        {
        //            command.Parameters.Add(new OracleParameter("smatricula", buscar ?? string.Empty));
        //            using (OracleDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    alumno = new AlumnoDto
        //                    {
        //                        Nombre = reader["NOMBRE_ALUMNO"].ToString(),
        //                        ApellidoPaterno = reader["APE_PAT"].ToString(),
        //                        ApellidoMaterno = reader["APE_MAT"].ToString()
        //                    };
        //                    claveEscuela = reader["UNI_ORG"].ToString();
        //                    nombreEscuela = reader["NOM_UNI_OR"].ToString();
        //                    clavePlan = reader["CLAVE"].ToString();
        //                    nombrePlan = reader["PLAN_ESTUDIO"].ToString();
        //                }
        //            }
        //        }
        //    }

        //    if (alumno != null)
        //    {
        //        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
        //        {
        //            sqlConnection.Open();

        //            // Obtener las escuelas
        //            using (SqlCommand sqlCommand = new SqlCommand(@"
        //        SELECT idEscuelaUAC, sClave, sDescripcion 
        //        FROM SP_ESCUELA_UAC 
        //        WHERE sClave = @claveEscuela ", sqlConnection))
        //            {
        //                sqlCommand.Parameters.AddWithValue("@claveEscuela", claveEscuela);
        //                sqlCommand.Parameters.AddWithValue("@nombreEscuela", nombreEscuela);
        //                using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
        //                {
        //                    while (sqlReader.Read())
        //                    {
        //                        escuelas.Add(new EscuelaDto
        //                        {
        //                            Id = sqlReader["idEscuelaUAC"].ToString(),
        //                            Nombre = sqlReader["sDescripcion"].ToString()
        //                        });
        //                    }
        //                }
        //            }

        //            // Obtener los planes de estudio
        //            using (SqlCommand sqlCommand = new SqlCommand(@"
        //        SELECT idPlanEstudio, sClave, sDescripcion 
        //        FROM SP_PLAN_ESTUDIO 
        //        WHERE sClave = @clavePlan", sqlConnection))
        //            {
        //                sqlCommand.Parameters.AddWithValue("@clavePlan", clavePlan);
        //                sqlCommand.Parameters.AddWithValue("@nombrePlan", nombrePlan);
        //                using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
        //                {
        //                    while (sqlReader.Read())
        //                    {
        //                        planesEstudio.Add(new PlanEstudioDto
        //                        {
        //                            Id = sqlReader["idPlanEstudio"].ToString(),
        //                            Nombre = sqlReader["sDescripcion"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }

        //        alumno.Escuelas = escuelas;
        //        alumno.PlanesEstudio = planesEstudio;
        //    }

        //    return alumno;
        //}


        //public static AlumnoDto GetAlumnoInfo(string buscar)
        //{
        //    string connectionString = GlobalConstants.SQL;

        //    AlumnoDto alumno = null;
        //    List<EscuelaDto> escuelas = new List<EscuelaDto>();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("sp_obtenerInformacionAlumno_ss", connection))
        //        {
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@matricula", buscar);

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    alumno = new AlumnoDto
        //                    {
        //                        Nombre = reader.GetString(0),
        //                        ApellidoPaterno = reader.GetString(1),
        //                        ApellidoMaterno = reader.GetString(2)
        //                    };
        //                }
        //            }
        //        }
        //        if (alumno != null)
        //        {
        //            using (SqlCommand command = new SqlCommand("sp_obtenerEscuelasAlumno_ss", connection))
        //            {
        //                command.CommandType = System.Data.CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@matricula", buscar);

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        escuelas.Add(new EscuelaDto
        //                        {
        //                            Id = Convert.ToString(reader[0]),
        //                            Nombre = reader.GetString(1)
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (alumno != null)
        //    {
        //        alumno.Escuelas = escuelas;
        //    }

        //    return alumno;
        //}

        [System.Web.Services.WebMethod]
        public static string GetPlanesEstudio(string escuelaId, string matricula)
        {
            List<PlanEstudioDto> planesEstudio = new List<PlanEstudioDto>();

            string connectionString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_obtenerPlanesAlumno_ss", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@matricula", matricula);
                    command.Parameters.AddWithValue("@idEscuelaUAC", escuelaId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            planesEstudio.Add(new PlanEstudioDto
                            {
                                Id = reader["idPlanEstudio"].ToString(),
                                Nombre = reader["PlanEstudio"].ToString()
                            });
                        }
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(planesEstudio);

        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string tipoEscuela = ddlTipoEscuela.SelectedValue;
            string escuela = Request.Form[ddlEscuela.UniqueID];
            string plan = Request.Form[ddlPlanEstudio.UniqueID];
            string matricula = txtMatricula.Text;
            string correo = txtCorreo.Text.Trim();
            string idAlumno = getidAlumno(escuela, plan, matricula); // Obtener el id del alumno
            string password = txtPasswordConfirm.Text.Trim();
            string connectionString = GlobalConstants.SQL;
            string semestre = txtSemestre.Text.Trim();

            // **Nueva validación**: Si el idAlumno es null o vacío, no continuar con el registro
            if (string.IsNullOrEmpty(idAlumno))
            {
             //   lblError.Text = "No se pudo obtener el ID del alumno. Verifique los datos e intente nuevamente.";
                return; // Detener la ejecución del método
            }

            if ((!isIncorporada(escuela) && tipoEscuela == "1") || (isIncorporada(escuela) && tipoEscuela == "2"))
            {
                if (tipoEscuela == "1")
                {
                    if (verificarCorreoExistente(correo, matricula))
                    {
                        if (VerificarCorreo(correo, escuela, plan, matricula, idAlumno))
                        {
                            lblError.Text = "Los datos ingresados ya se encuentran registrados";
                            txtMatricula.Text = "";
                            txtCorreo.Text = "";
                            ddlTipoEscuela.SelectedValue = "";
                            ddlTipoEscuela.Focus();
                        }
                        else
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                SqlTransaction transaction = connection.BeginTransaction();

                                try
                                {
                                    // Insertar en la tabla SM_USUARIO
                                    int idUsuarioInsertado = InsertarUsuario(correo, idAlumno, "", tipoEscuela, connection, transaction);

                                    // Insertar en la tabla SM_DEPENDENCIA_SERVICIO
                                    InsertarUsuarioAlumno(idUsuarioInsertado, idAlumno, plan, escuela, matricula, semestre, connection, transaction);

                                    // Commit de la transacción
                                    transaction.Commit();

                                    pnlRegistro.Visible = false;
                                    pnlRegistroExitoso.Visible = true;
                                }
                                catch
                                {
                                    // Rollback si hay un error
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                    else
                    {
                        lblError.Text = "La matrícula ingresada no cuenta con correo institucional o el correo ingresado no corresponde a la matrícula.";
                        txtMatricula.Text = "";
                        ddlTipoEscuela.Focus();
                    }
                }
                else
                {
                    if (VerificarCorreo(correo, escuela, plan, matricula, idAlumno))
                    {
                        lblError.Text = "Los datos ingresados ya se encuentran registrados";
                        txtMatricula.Text = "";
                        txtCorreo.Text = "";
                        ddlTipoEscuela.SelectedValue = "";
                        ddlTipoEscuela.Focus();
                    }
                    else
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                // Insertar en la tabla SM_USUARIO
                                int idUsuarioInsertado = InsertarUsuario(correo, idAlumno, password, tipoEscuela, connection, transaction);

                                // Insertar en la tabla SM_DEPENDENCIA_SERVICIO
                                InsertarUsuarioAlumno(idUsuarioInsertado, idAlumno, plan, escuela, matricula, semestre, connection, transaction);

                                // Commit de la transacción
                                transaction.Commit();

                                pnlRegistro.Visible = false;
                                pnlRegistroExitoso.Visible = true;
                            }
                            catch
                            {
                                // Rollback si hay un error
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "La escuela seleccionada no corresponde al tipo de escuela.";
                txtMatricula.Text = "";
                txtCorreo.Text = "";
                ddlTipoEscuela.SelectedValue = "";
                ddlTipoEscuela.Focus();
            }
        }

        //protected void btnRegistrar_Click(object sender, EventArgs e)
        //{
        //    string tipoEscuela = ddlTipoEscuela.SelectedValue;
        //    string escuela = Request.Form[ddlEscuela.UniqueID];
        //    string plan = Request.Form[ddlPlanEstudio.UniqueID];
        //    string matricula = txtMatricula.Text;
        //    string correo = txtCorreo.Text.Trim();
        //    string idAlumno = getidAlumno(escuela, plan, matricula);
        //    string password = txtPasswordConfirm.Text.Trim();
        //    string connectionString = GlobalConstants.SQL;
        //    string semestre = txtSemestre.Text.Trim();

        //    if ((!isIncorporada(escuela) && tipoEscuela == "1") || (isIncorporada(escuela) && tipoEscuela == "2"))
        //    {
        //        if (tipoEscuela == "1")
        //        {
        //            if (verificarCorreoExistente(correo, matricula))
        //            {
        //                if (VerificarCorreo(correo, escuela, plan, matricula))
        //                {
        //                    lblError.Text = "Los datos ingresados ya se encuentran registrados";
        //                    txtMatricula.Text = "";
        //                    txtCorreo.Text = "";
        //                    ddlTipoEscuela.SelectedValue = "";
        //                    ddlTipoEscuela.Focus();
        //                }
        //                else
        //                {
        //                    using (SqlConnection connection = new SqlConnection(connectionString))
        //                    {
        //                        connection.Open();

        //                        SqlTransaction transaction = connection.BeginTransaction();

        //                        try
        //                        {
        //                            // Primero, insertar en la tabla SM_USUARIO
        //                            int idUsuarioInsertado = InsertarUsuario(correo, idAlumno, "", tipoEscuela, connection, transaction);

        //                            // Luego, insertar en la tabla SM_DEPENDENCIA_SERVICIO usando el ID del usuario insertado
        //                            InsertarUsuarioAlumno(idUsuarioInsertado, idAlumno, plan, escuela, matricula, semestre, connection, transaction);

        //                            // Commit de la transacción si todo fue exitoso
        //                            transaction.Commit();


        //                            pnlRegistro.Visible = false;
        //                            pnlRegistroExitoso.Visible = true;
        //                            //string cambio = "1";
        //                            //enviarCorreo(_email, cambio);
        //                            ////Response.Redirect("Dependencias.aspx", false);
        //                        }
        //                        catch
        //                        {
        //                            // Si ocurre algún error, realizar un rollback de la transacción
        //                            transaction.Rollback();

        //                            // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
        //                            //Response.Write("Error: " + ex.Message);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                lblError.Text = "La matrícula ingresada no cuenta con correo institucional o el correo ingresado no corresponde a la matrícula.";
        //                txtMatricula.Text = "";
        //                ddlTipoEscuela.Focus();
        //            }
        //        }
        //        else
        //        {
        //            if (VerificarCorreo(correo, escuela, plan, matricula))
        //            {
        //                lblError.Text = "Los datos ingresados ya se encuentran registrados";
        //                txtMatricula.Text = "";
        //                txtCorreo.Text = "";
        //                ddlTipoEscuela.SelectedValue = "";
        //                ddlTipoEscuela.Focus();
        //            }
        //            else
        //            {
        //                using (SqlConnection connection = new SqlConnection(connectionString))
        //                {
        //                    connection.Open();

        //                    SqlTransaction transaction = connection.BeginTransaction();

        //                    try
        //                    {
        //                        // Primero, insertar en la tabla SM_USUARIO
        //                        int idUsuarioInsertado = InsertarUsuario(correo, idAlumno, password, tipoEscuela, connection, transaction);

        //                        // Luego, insertar en la tabla SM_DEPENDENCIA_SERVICIO usando el ID del usuario insertado
        //                        InsertarUsuarioAlumno(idUsuarioInsertado, idAlumno, plan, escuela, matricula, semestre,connection, transaction);

        //                        // Commit de la transacción si todo fue exitoso
        //                        transaction.Commit();


        //                        pnlRegistro.Visible = false;
        //                        pnlRegistroExitoso.Visible = true;
        //                    }
        //                    catch
        //                    {
        //                        // Si ocurre algún error, realizar un rollback de la transacción
        //                        transaction.Rollback();

        //                        // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
        //                        //Response.Write("Error: " + ex.Message);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        lblError.Text = "La escuela seleccionada no corresponde al tipo de escuela.";
        //        txtMatricula.Text = "";
        //        txtCorreo.Text = "";
        //        ddlTipoEscuela.SelectedValue = "";
        //        ddlTipoEscuela.Focus();
        //    }
        //}

        //public string getidAlumno(string escuela, string plan, string matricula)
        //{
        //    string idAlumno = "";
        //    string conString = GlobalConstants.SQL;
        //    using (SqlConnection connection = new SqlConnection(conString))
        //    {
        //        string query = "SELECT idAlumno FROM SM_ALUMNO WHERE sMatricula = @sMatricula AND kpPlan_estudios = @kpPlan_estudios AND kpEscuelasUadeC = @kpEscuelasUadeC; ";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@sMatricula", matricula);
        //            command.Parameters.AddWithValue("@kpPlan_estudios", plan);
        //            command.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);
        //            connection.Open();
        //            object result = command.ExecuteScalar();
        //            if (result != null)
        //            {
        //                idAlumno = result.ToString();
        //            }
        //            else
        //                idAlumno = null;
        //        }
        //    }

        //    return idAlumno;
        //}

        //public string getidAlumno(string escuela, string plan, string matricula)
        //{
        //    string idAlumno = "";
        //    string conString = GlobalConstants.SQL;

        //    using (SqlConnection connection = new SqlConnection(conString))
        //    {
        //        string query = "SELECT idAlumno FROM SM_ALUMNO WHERE sMatricula = @sMatricula AND kpPlan_estudios = @kpPlan_estudios AND kpEscuelasUadeC = @kpEscuelasUadeC;";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@sMatricula", matricula);
        //            command.Parameters.AddWithValue("@kpPlan_estudios", plan);
        //            command.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);

        //            connection.Open();
        //            object result = command.ExecuteScalar();

        //            if (result != null)
        //            {
        //                idAlumno = result.ToString();
        //            }
        //            else
        //            {
        //                // Si no se encuentra el alumno, se inserta
        //                string insertQuery = "EXEC Oracle_Importar_Alumno_WS @sMatricula, @kpEscuelasUadeC, @kpPlan_estudios;";
        //                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
        //                {
        //                    insertCommand.Parameters.AddWithValue("@sMatricula", matricula);
        //                    insertCommand.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);
        //                    insertCommand.Parameters.AddWithValue("@kpPlan_estudios", plan);
        //                    insertCommand.ExecuteNonQuery();
        //                }

        //                // Intentamos obtener nuevamente el idAlumno
        //                using (SqlCommand retryCommand = new SqlCommand(query, connection))
        //                {
        //                    retryCommand.Parameters.AddWithValue("@sMatricula", matricula);
        //                    retryCommand.Parameters.AddWithValue("@kpPlan_estudios", plan);
        //                    retryCommand.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);

        //                    object retryResult = retryCommand.ExecuteScalar();
        //                    if (retryResult != null)
        //                    {
        //                        idAlumno = retryResult.ToString();
        //                    }
        //                    else
        //                    {
        //                        idAlumno = null; // Si por alguna razón no se inserta correctamente
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return idAlumno;
        //}

        public string getidAlumno(string escuela, string plan, string matricula)
        {
            string idAlumno = "";
            string conString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

             

                // Consultar si el alumno ya existe
                string query = "SELECT idAlumno FROM SM_ALUMNO WHERE sMatricula = @sMatricula AND kpPlan_estudios = @kpPlan_estudios AND kpEscuelasUadeC = @kpEscuelasUadeC;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sMatricula", matricula);
                    command.Parameters.AddWithValue("@kpPlan_estudios", plan);
                    command.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        idAlumno = result.ToString();
                    }
                    else
                    {
                        // Si no se encuentra el alumno, se inserta con el procedimiento almacenado

                        // Obtener la clave de la escuela
                        string sClaveEscuela = null;
                        string queryEscuela = "SELECT sClave FROM SP_ESCUELA_UAC WHERE idEscuelaUAC = @idEscuelaUAC;";
                        using (SqlCommand commandEscuela = new SqlCommand(queryEscuela, connection))
                        {
                            commandEscuela.Parameters.AddWithValue("@idEscuelaUAC", escuela);
                            object resultEscuela = commandEscuela.ExecuteScalar();
                            if (resultEscuela != null)
                            {
                                sClaveEscuela = resultEscuela.ToString();
                            }
                        }

                        // Obtener la clave del plan de estudios
                        string sClavePlan = null;
                        string queryPlan = "SELECT sClave FROM SP_PLAN_ESTUDIO WHERE idPlanEstudio = @idPlanEstudio;";
                        using (SqlCommand commandPlan = new SqlCommand(queryPlan, connection))
                        {
                            commandPlan.Parameters.AddWithValue("@idPlanEstudio", plan);
                            object resultPlan = commandPlan.ExecuteScalar();
                            if (resultPlan != null)
                            {
                                sClavePlan = resultPlan.ToString();
                            }
                        }

                        // Validar que se obtuvieron las claves antes de continuar
                        if (string.IsNullOrEmpty(sClaveEscuela) || string.IsNullOrEmpty(sClavePlan))
                        {
                            return null; // Si alguna clave no se encontró, retornar null
                        }

                        string insertQuery = "EXEC Oracle_Importar_Alumno @sMatricula, @sClaveEscuela, @sClavePlan;";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@sMatricula", matricula);
                            insertCommand.Parameters.AddWithValue("@sClaveEscuela", sClaveEscuela);
                            insertCommand.Parameters.AddWithValue("@sClavePlan", sClavePlan);
                            insertCommand.ExecuteNonQuery();
                        }

                        // Intentamos obtener nuevamente el idAlumno
                        using (SqlCommand retryCommand = new SqlCommand(query, connection))
                        {
                            retryCommand.Parameters.AddWithValue("@sMatricula", matricula);
                            retryCommand.Parameters.AddWithValue("@kpPlan_estudios", plan);
                            retryCommand.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);

                            object retryResult = retryCommand.ExecuteScalar();
                            if (retryResult != null)
                            {
                                idAlumno = retryResult.ToString();
                            }
                            else
                            {
                                idAlumno = null; // Si por alguna razón no se inserta correctamente
                            }
                        }
                    }
                }
            }

            return idAlumno;
        }
        [System.Web.Services.WebMethod]
        public static bool ValidarSemestre(string semestre)
        {
            // Aquí puedes hacer la lógica para validar si el semestre es correcto (por ejemplo, verificar en la base de datos)
            // Por ejemplo:
            if (int.TryParse(semestre, out int resultado))
            {
                if (resultado >= 1 && resultado <= 20)  // Ejemplo: Validar que el semestre sea un número entre 1 y 20
                {
                    return true; // Semestre válido
                }
            }
            return false; // Semestre inválido
        }
        private int InsertarUsuario(string _correo, string _idAlumno, string _password, string _tipo ,SqlConnection connection, SqlTransaction transaction)
        {
            string password = SeguridadUtils.Encriptar(_password);
            if (_tipo == "1")
            {
                string query = "INSERT INTO SM_USUARIO (sCorreo, kpTipoUsuario, kmIdentificador) VALUES (@sCorreo, @kpTipoUsuario, @kmIdentificador); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@sCorreo", _correo);
                    cmd.Parameters.AddWithValue("@kpTipoUsuario", 5);
                    cmd.Parameters.AddWithValue("@kmIdentificador", _idAlumno);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            else
            {
                string query = "INSERT INTO SM_USUARIO (sCorreo, kpTipoUsuario, sPassword, kmIdentificador) VALUES (@sCorreo, @kpTipoUsuario, @sPassword, @kmIdentificador); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@sCorreo", _correo);
                    cmd.Parameters.AddWithValue("@kpTipoUsuario", 5);
                    cmd.Parameters.AddWithValue("@sPassword", password);
                    cmd.Parameters.AddWithValue("@kmIdentificador", _idAlumno);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private void InsertarUsuarioAlumno(int _idUsuario, string _idalumno, string _PlanEstudio, string _Escuela, string _sMatricula, string _semestre,
            SqlConnection connection, SqlTransaction transaction)
        {
            // Primero, insertamos los datos proporcionados al método
            InsertarSiNoExiste(_idUsuario, _idalumno, _PlanEstudio, _Escuela, _semestre,connection, transaction);

            // Buscar registros en SM_ALUMNO con diferente kpEscuela y kpPlan
            string searchQuery = "SELECT idAlumno, kpPlan_estudios, kpEscuelasUadeC " +
                "FROM SM_ALUMNO " +
                "WHERE sMatricula = @sMatricula AND (kpEscuelasUadeC <> @kpEscuela OR kpPlan_estudios <> @kpPlan) AND kpPlan_estudios <> 584 AND kpEstatus_Alumno IN (1,2,6); ";

            List<(string idAlumno, string kpPlan, string kpEscuela)> registros = new List<(string idAlumno, string kpPlan, string kpEscuela)>();


            using (SqlCommand searchCmd = new SqlCommand(searchQuery, connection, transaction))
            {
                searchCmd.Parameters.AddWithValue("@sMatricula", _sMatricula);
                searchCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);
                searchCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);

                using (SqlDataReader reader = searchCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Obtener los valores de kpPlan y kpEscuela encontrados
                        string foundIdAlumno = reader["idAlumno"].ToString();
                        string foundPlan = reader["kpPlan_estudios"].ToString();
                        string foundEscuela = reader["kpEscuelasUadeC"].ToString();

                        registros.Add((foundIdAlumno, foundPlan, foundEscuela));
                    }
                }
            }
            // Insertar los registros encontrados
            foreach (var (foundIdAlumno, foundPlan, foundEscuela) in registros)
            {
                InsertarSiNoExiste(_idUsuario, foundIdAlumno, foundPlan, foundEscuela, _semestre,connection, transaction);
            }
        }

        //private void InsertarSiNoExiste(int _idUsuario, string _idalumno, string _PlanEstudio, string _Escuela, SqlConnection connection, SqlTransaction transaction)
        //{
        //    // Verificar si el registro ya existe en SM_USUARIOS_ALUMNOS
        //    string checkQuery = "SELECT COUNT(*) FROM SM_USUARIOS_ALUMNOS WHERE kmAlumno = @kmAlumno AND kpPlan = @kpPlan AND kpEscuela = @kpEscuela AND bAutorizado <> 99 ";

        //    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction))
        //    {
        //        checkCmd.Parameters.AddWithValue("@kmAlumno", _idalumno);
        //        checkCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);
        //        checkCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);

        //        int count = (int)checkCmd.ExecuteScalar();

        //        // Si no existe, insertar el nuevo registro
        //        if (count == 0)
        //        {
        //            string insertQuery = "INSERT INTO SM_USUARIOS_ALUMNOS (kmUsuario, kmAlumno, kpPlan, kpEscuela, bAutorizado) " +
        //                                 "VALUES (@kmUsuario, @kmAlumno, @kpPlan, @kpEscuela, @bAutorizado);";

        //            using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction))
        //            {
        //                insertCmd.Parameters.AddWithValue("@kmUsuario", _idUsuario);
        //                insertCmd.Parameters.AddWithValue("@kmAlumno", _idalumno);
        //                insertCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);
        //                insertCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);
        //                insertCmd.Parameters.AddWithValue("@bAutorizado", 1);
        //                insertCmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //}
        private void InsertarSiNoExiste(int _idUsuario, string _idalumno, string _PlanEstudio, string _Escuela, string _semestre, SqlConnection connection, SqlTransaction transaction)
        {
            // Verificar si el registro ya existe en SM_USUARIOS_ALUMNOS
            string checkQuery = "SELECT COUNT(*) FROM SM_USUARIOS_ALUMNOS WHERE kmAlumno = @kmAlumno AND kpPlan = @kpPlan AND kpEscuela = @kpEscuela AND bAutorizado <> 99 ";

            using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@kmAlumno", _idalumno);
                checkCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);
                checkCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);

                int count = (int)checkCmd.ExecuteScalar();

                // Si no existe, insertar el nuevo registro
                if (count == 0)
                {
                    // Obtener el valor de idCiclo desde SP_CICLO
                    string cicloQuery = "SELECT idCiclo FROM SP_CICLO WHERE bServicioSocial = 1";
                    int? kpPeriodo = null;

                    using (SqlCommand cicloCmd = new SqlCommand(cicloQuery, connection, transaction))
                    {
                        object result = cicloCmd.ExecuteScalar();
                        if (result != null)
                        {
                            kpPeriodo = Convert.ToInt32(result);
                        }
                    }

                    // Insertar el nuevo registro en SM_USUARIOS_ALUMNOS
                    string insertQuery = "INSERT INTO SM_USUARIOS_ALUMNOS (kmUsuario, kmAlumno, kpPlan, kpEscuela, kpPeriodo,iSemestre, bAutorizado) " +
                                         "VALUES (@kmUsuario, @kmAlumno, @kpPlan, @kpEscuela, @kpPeriodo, @iSemestre,@bAutorizado);";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@kmUsuario", _idUsuario);
                        insertCmd.Parameters.AddWithValue("@kmAlumno", _idalumno);
                        insertCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);
                        insertCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);
                        insertCmd.Parameters.AddWithValue("@iSemestre", _semestre);


                        if (kpPeriodo.HasValue)
                        {
                            insertCmd.Parameters.AddWithValue("@kpPeriodo", kpPeriodo.Value);
                        }
                        else
                        {
                            insertCmd.Parameters.AddWithValue("@kpPeriodo", DBNull.Value);
                        }

                        insertCmd.Parameters.AddWithValue("@bAutorizado", 20707);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public bool VerificarCorreo(string correo, string escuela, string plan, string matricula, string idAlumno)
        {
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real
            //string idAlumno = getidAlumno(escuela, plan, matricula);

            // Query para verificar si el correo existe en la base de datos
            string query = "SELECT COUNT(*) " +
            "FROM SM_USUARIO U " +
            "INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.kmUsuario = U.idUsuario " +
            "WHERE U.sCorreo = @Email AND UA.kpEscuela = @kpEscuela AND UA.kpPlan = @kpPlan AND U.kpTipoUsuario = 5 AND UA.bAutorizado <> 99";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", correo);
                    command.Parameters.AddWithValue("@kpEscuela", escuela);
                    command.Parameters.AddWithValue("@kpPlan", plan);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public bool isIncorporada(string escuela)
        {
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real
            string result = "";
            // Query para verificar si el correo existe en la base de datos
            string query = "SELECT (CASE WHEN sclave LIKE '1%' THEN 2 ELSE 1 END) INC " +
                "FROM SP_ESCUELA_UAC " +
                "WHERE idEscuelaUAC =@idEscuelaUAC ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idEscuelaUAC", escuela);

                    connection.Open();
                    result = command.ExecuteScalar().ToString();
                }
            }

            //1 - Oficial ; 2 - Inc
            if (result == "1")
                return false;
            else return true;
        }

        public bool verificarCorreoExistente(string correo, string matricula)
        {
            string connectionString = GlobalConstants.ORA;

            string _correoLogin = correo.Split('@')[0];
            string _correoDominio = "@" + correo.Split('@')[1].Trim();
            string result = "";

            if (_correoDominio.Contains("@uadec.edu.mx"))
            {
                // Query para verificar si el correo existe en la base de datos
                string query = @"SELECT LOGIN 
                                FROM MAILEDU.V_ALUMNOS_CON_CORREO 
                               WHERE MATRICULA = :matricula";

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("matricula", matricula));

                        connection.Open();
                        try
                        {
                            result = command.ExecuteScalar().ToString();
                            if (result == _correoLogin)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            else
                return false;
        }
    }
}