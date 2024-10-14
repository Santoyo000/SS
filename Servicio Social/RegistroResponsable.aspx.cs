using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http.Results;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class RegistroResponsable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUser"] == null)
            {
                Response.Redirect("Home.aspx");
            }
            if (!IsPostBack)
            {
                cargarUnidades();
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string _idExpediente = getIdEmpleado(txtExpediente.Text);
                if (_idExpediente != null || _idExpediente != "")
                {
                    string connectionString = GlobalConstants.SQL;
                    string _correo = txtEmail.Text.Trim();
                    string _telefono = txtTelefono.Text.Trim();
                    string _unidad = ddlUnidad.SelectedValue;
                    string _tipo = "3";
                    if (verificarExistente(_correo) == false)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                insertResponsableUnidad(_correo, _telefono, _tipo, _unidad, _idExpediente, connection, transaction);

                                // Commit de la transacción si todo fue exitoso
                                transaction.Commit();
                                pnlRegistroExitoso.Visible = true;
                                pnlRegistrarResponsable.Visible = false;
                                //string cambio = "1";
                                //enviarCorreo(_correo, cambio);
                                //Response.Redirect("Dependencias.aspx", false);
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
                        lblMensajeError.Text = "El correo ingresado ya se encuentra registrado.";
                    }
                }
                else
                {
                    lblMensajeError.Text = "Ha ocurrido un error al intentar guardar el registro, contecte al administrador";
                }
                
            }
            catch
            {
                
            }

        }

        private void insertResponsableUnidad(string correo, string telefono, string tipo, string unidad, string expediente, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_USUARIO (sCorreo, sTelefono, kpTipoUsuario, kpUnidad, kpEmpleado, bAutorizado) " +
                "VALUES (@sCorreo, @sTelefono, @kpTipoUsuario, @kpUnidad, @kpEmpleado, 0);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sCorreo", correo);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", tipo);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kpEmpleado", expediente);
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public static List<string> buscarCorreo(string correo,string exp)
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

        public void enviarCorreo(string email, string tipo)
        {
            MensajesCorreo mc = new MensajesCorreo();
            string body = mc.tipoTexto(tipo);
            string to = email;
            string from = "noreply@uadec.edu.mx";
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            //message.To.Add(); //correo al administrador (por definir)
            message.Subject = "Registro Plataforma de Servicio Social | UAdeC";
            message.IsBodyHtml = true;
            message.Body = body;
            SmtpClient client = new SmtpClient("mailgate2.uadec.mx");
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send email on the client's behalf.
            client.Host = "mailgate2.uadec.mx";
            client.Port = 25;
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential(from, "");
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                ex.ToString());
            }
        }

        public void cargarUnidades()
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1);";

            // Agregar una opción predeterminada al DropDownList
            ddlUnidad.Items.Add(new ListItem("Selecciona una opción", ""));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                // Ejecutar la consulta y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Añadir cada nombre a un elemento ListItem y agregarlo al DropDownList
                    ddlUnidad.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                }

                // Cerrar la conexión
                con.Close();
            }
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
            string query = "SELECT COUNT(*) FROM SM_USUARIO WHERE sCorreo = @Email and kpTipoUsuario = 3";

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

    }


}