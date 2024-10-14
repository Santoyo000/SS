using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Web.Services;

namespace Servicio_Social
{
    public partial class RegistroEncargadoEscuela : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUser"] == null)
            {
                Response.Redirect("Home.aspx");
            }

            if (!IsPostBack)
            {
                CargarUnidadEnc();
                CargarNivelEnc();
                CargaEscuelaEnc();
            }
        }
        protected void rbtnOficial_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnOficial.Checked)
            {
                // Limpiar todos los campos al seleccionar "Oficial"
                LimpiarCampos();
            }
            pnlEncargadoOficial.Visible = true;
            pnlIncorporada.Visible = false;
            ToggleEditableFields();
            //pnlDatOficial.Visible = true;
            //pnlCorreoIncorp.Visible = false;
            txtExpedienteEnc.Visible = true;
            lblExpediente.Visible = true;
        }

        protected void rbtnIncorporada_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnIncorporada.Checked)
            {
                // Limpiar todos los campos al seleccionar "Incorporada"
                LimpiarCampos();
            }
            pnlEncargadoOficial.Visible = true;
            pnlIncorporada.Visible = true;
            ToggleEditableFields();
            //pnlDatOficial.Visible = false;
            //pnlCorreoIncorp.Visible = true;
            txtExpedienteEnc.Visible = false;
            lblExpediente.Visible = false;
        }
        private void ToggleEditableFields()
        {
            bool isIncorporadaSelected = rbtnIncorporada.Checked;
            txtNombreEnc.Enabled = isIncorporadaSelected;
            txtApePatEnc.Enabled = isIncorporadaSelected;
            txtApeMatEnc.Enabled = isIncorporadaSelected;
        }
        private void LimpiarCampos()
        {
            // Limpiar campos de texto
            txtNombreEnc.Text = "";
            txtApePatEnc.Text = "";
            txtApeMatEnc.Text = "";
            txtTelefonoEnc.Text = "";
            txtEmailEnc.Text = "";
            txtExpedienteEnc.Text = "";
            txtContrasena.Text = "";
            txtConfirmarContrasena.Text = "";
            //txtCorreoIncorp.Text = "";

            // Limpiar labels
            lblMensajeEmailEnc.Text = "";
            lblMensajeErrorEnc.Text = "";
            Label6.Text = "";

            // Limpiar selección de DropDownLists
            LimpiarDropDownList(ddlUnidadEnc);
            LimpiarDropDownList(ddlNivelEnc);
            LimpiarDropDownList(ddlEscuelEnc);
        }
        private void LimpiarDropDownList(DropDownList ddl)
        {
            ddl.ClearSelection(); // Limpia la selección actual
        }
        [WebMethod]
        public static List<string> buscarCorreo(string correo, string exp)
        {
            List<string> results = new List<string>();
            string connectionString = GlobalConstants.ORA;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string sqlQuery = "SELECT TRIM(GE.NOMBRE) NOMBRE, TRIM(GE.APE_PAT) APE_PAT, TRIM(GE.APE_MAT) APE_MAT, EE.UADEC_EMAIL AS EMAIL " +
                    "FROM MAILEDU.EMPLEADO_EMAIL EE " +
                    "INNER JOIN OM.GEMPLEADO GE ON GE.EXPEDIENTE = EE.EXPEDIENTE " +
                    "WHERE EE.UADEC_EMAIL LIKE :email AND GE.EXPEDIENTE = :exp " +
                    "UNION ALL " +
                    "SELECT TRIM(NOMBRE) NOMBRE, TRIM(APE_PAT) APE_PAT, TRIM(APE_MAT) APE_MAT, EMAIL " +
                    "FROM PLANTADOCENTE.DOCENTEINCORPORADO " +
                    "WHERE EMAIL LIKE :email AND EXPEDIENTE = :exp";
                using (OracleCommand cmd = new OracleCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("email", correo));
                    cmd.Parameters.Add(new OracleParameter("exp", exp));
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader["NOMBRE"].ToString());
                            results.Add(reader["APE_PAT"].ToString());
                            results.Add(reader["APE_MAT"].ToString());
                        }
                    }
                }
            }
            return results;
        }
        public string getIdEmpleado(string expediente)
        {
            string idExpediente = "";
            string conString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                string query = "SELECT idEmpleado FROM NM_EMPLEADO WHERE sExpediente = @Expediente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Expediente", expediente);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        idExpediente = result.ToString();
                    }
                    else
                        idExpediente = null;
                }
            }

            return idExpediente;
        }

        public bool verificarExistente(string correo)
        {
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real

            // Query para verificar si el correo existe en la base de datos
            string query = "SELECT COUNT(*) FROM SM_USUARIO WHERE sCorreo = @Email and kpTipoUsuario = 4";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", correo);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }

        }
        //Filtro  UNIDAD NIVEL ESCUELA
        private void CargarUnidadEnc()
        {
            // Define la conexión SQL y la consulta
            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string queryString = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1) ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sCiudad"] = "Seleccione la Unidad...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlUnidadEnc.DataSource = ds3;
                ddlUnidadEnc.DataTextField = "sCiudad"; // Utiliza el alias "Descripcion" como texto visible
                ddlUnidadEnc.DataValueField = "idUnidad";
                ddlUnidadEnc.DataBind();
            }

        }

        private void CargarNivelEnc()
        {
            // Define la conexión SQL y la consulta
            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string queryString = "SELECT sDescripcion, idTipoNivel FROM SP_TIPO_NIVEL  WHERE idTipoNivel != 666 AND idTipoNivel!= 3  ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione el Nivel...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlNivelEnc.DataSource = ds3;
                ddlNivelEnc.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlNivelEnc.DataValueField = "idTipoNivel";
                ddlNivelEnc.DataBind();
            }

        }


        private void CargaEscuelaEnc()
        {
            ddlEscuelEnc.Items.Clear();
            string idUnidadSeleccionado = ddlUnidadEnc.SelectedValue;
            string idNivelSeleccionado = ddlNivelEnc.SelectedValue;

            // CARGAR ESCUELAS CON ENFOQUE DIFERENTE PBG
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT DISTINCT ESC.sClave + ' - ' + ESC.sDescripcion Descripcion, ");
            sb.AppendLine(@"       PLA.kpEscuela_UAdeC,PE.kpNivel, kpUnidad, ESC.idEscuelaUAC ");
            sb.AppendLine(@"FROM SM_PLAN_EST_ESCUELA AS PLA ");
            sb.AppendLine(@"INNER JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC ");
            sb.AppendLine(@"INNER JOIN SP_PLAN_ESTUDIO AS PE ON PE.idPlanEstudio = PLA.kpPlan_Estudio ");
            sb.AppendLine(@"WHERE  PE.kpNivel=@idNivel AND ESC.kpUnidad = @idUnidad ");
            sb.AppendLine(@"AND    PE.bVigente=1 AND PE.bActivo=1 ");
            sb.AppendLine(@"ORDER BY ESC.sClave + ' - ' + ESC.sDescripcion");

            string queryString = sb.ToString();

            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(queryString, con);
                cmd.Parameters.AddWithValue("@idNivel", idNivelSeleccionado);
                cmd.Parameters.AddWithValue("@idUnidad", idUnidadSeleccionado);
                SqlDataReader reader = cmd.ExecuteReader();

                // Crear un nuevo DataTable que incluya el primer elemento "Seleccione la Escuela..."
                DataTable dt = new DataTable();
                dt.Columns.Add("Descripcion", typeof(string));
                dt.Columns.Add("kpEscuela_UAdeC", typeof(string));
                dt.Rows.Add("Seleccione la Escuela...", ""); // Agregar el primer elemento

                // Llenar el DataTable con los resultados de la consulta
                while (reader.Read())
                {
                    dt.Rows.Add(reader["Descripcion"], reader["kpEscuela_UAdeC"]);
                }

                // Llena DDLEscuela con los datos del DataTable
                ddlEscuelEnc.DataSource = dt;
                ddlEscuelEnc.DataTextField = "Descripcion";
                ddlEscuelEnc.DataValueField = "kpEscuela_UAdeC";
                ddlEscuelEnc.DataBind();
            }

        }


        protected void ddlNivelEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaEscuelaEnc();
        }
        protected void ddlUnidadEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivelEnc.SelectedIndex = -1;
        }

        public void mensajeScriptExito(string mensaje)
        {
            string scriptText = "$('.alert').remove(); $('body').prepend('<div class=\"alert alert-success alert-dismissible fade show\" role=\"alert\" style=\"background-color: #d4edda; color: #155724;\"><strong>" + mensaje + "</strong><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button></div>'); setTimeout(function() { $('.alert').alert('close'); }, 6000);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }
        protected void btnEncargado_Click(object sender, EventArgs e)
        {
            try
            {
                string _idExpediente = getIdEmpleado(txtExpedienteEnc.Text);
                if (_idExpediente != null || _idExpediente != "")
                {
                    string connectionString = GlobalConstants.SQL;
                    string _correo = txtEmailEnc.Text.Trim();
                    string _telefono = txtTelefonoEnc.Text.Trim();
                    string _unidad = ddlUnidadEnc.SelectedValue;
                    string _tipo = "4"; //ddlUser.SelectedValue;
                    string _escuela = ddlEscuelEnc.SelectedValue;
                    //string _correoIncorp = txtCorreoIncorp.Text.Trim();
                    string _Pass = SeguridadUtils.Encriptar(txtConfirmarContrasena.Text);
                    string _Nombre = txtNombreEnc.Text.Trim();
                    string _ApePat = txtApePatEnc.Text.Trim();
                    string _ApeMat = txtApeMatEnc.Text.Trim();


                    string _nombre = txtNombreEnc.Text.Trim() + txtApePatEnc.Text.Trim() + txtApeMatEnc.Text.Trim();
                    if (verificarExistente(_correo) == false)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                if (rbtnOficial.Checked)
                                {
                                    insertEncargado(_correo, _telefono, _tipo, _unidad, _idExpediente, _escuela, _Nombre, _ApePat, _ApeMat, connection, transaction);
                                }
                                else if (rbtnIncorporada.Checked)
                                {
                                    insertEncargadoIncorporada(_correo, _Pass, _telefono, _tipo, _unidad, _escuela, _Nombre,_ApePat,_ApeMat, connection, transaction);
                                }
                                else
                                {
                                    throw new Exception("Debe seleccionar un tipo de encargado.");
                                }

                                transaction.Commit();
                                // pnlRegistrarEncargado.Visible = false;
                                LimpiarCampos();
                                mensajeScriptExito("El usuario ha sido creado con éxito.");
                                
                            }
                            catch (Exception ex)
                            {
                                // Si ocurre algún error, realizar un rollback de la transacción
                                transaction.Rollback();

                                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                Response.Write("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        lblMensajeErrorEnc.Text = "El correo ingresado ya se encuentra registrado.";
                    }
                }
                else
                {
                    lblMensajeErrorEnc.Text = "Ha ocurrido un error al intentar guardar el registro, contecte al administrador";
                }

            }
            catch (Exception ex)
            {
                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                Response.Write("Error: " + ex.Message);
            }
        }

        private void insertEncargado(string correo, string telefono, string tipo, string unidad, string expediente, string escuela, string Nombre, string ApePat, string ApeMat, SqlConnection connection, SqlTransaction transaction)
        {
            ////////////////////////////////////////
            string query = "INSERT INTO SM_USUARIO (sCorreo, sTelefono, kpTipoUsuario, kpUnidad, kmIdentificador, bAutorizado, kpEscuela) " +
                "VALUES (@sCorreo, @sTelefono, @kpTipoUsuario, @kpUnidad, @kpEmpleado, 1, @kpEscuela);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sCorreo", correo);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", tipo);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kpEmpleado", expediente);
                cmd.Parameters.AddWithValue("@kpEscuela", escuela);
                cmd.Parameters.AddWithValue("@sNombreUsuario", Nombre);
                cmd.Parameters.AddWithValue("@sApellidoPat", ApePat);
                cmd.Parameters.AddWithValue("@sApellidoMat", ApeMat);
                cmd.ExecuteNonQuery();
            }
        }

        private void insertEncargadoIncorporada(string correo, string Pass ,string telefono, string tipo, string unidad, string escuela, string Nombre, string ApePat, string ApeMat, SqlConnection connection, SqlTransaction transaction)
        {
            ////////////////////////////////////////
            string query = "INSERT INTO SM_USUARIO (sCorreo, sPassword, sTelefono, kpTipoUsuario, kpUnidad, bAutorizado, kpEscuela,sNombreUsuario, sApellido_Pat, sApellido_Mat) " +
                "VALUES (@sCorreo, @sPassword,@sTelefono, @kpTipoUsuario, @kpUnidad, 1, @kpEscuela, @sNombreUsuario, @sApellidoPat,@sApellidoMat);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {

                cmd.Parameters.AddWithValue("@sCorreo", correo);
                cmd.Parameters.AddWithValue("@sPassword", Pass);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", tipo);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kpEscuela", escuela);
                cmd.Parameters.AddWithValue("@sNombreUsuario", Nombre);
                cmd.Parameters.AddWithValue("@sApellidoPat", ApePat);
                cmd.Parameters.AddWithValue("@sApellidoMat", ApeMat);
                cmd.ExecuteNonQuery();
            }
        }
    }
}