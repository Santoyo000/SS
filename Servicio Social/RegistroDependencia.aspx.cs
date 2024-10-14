using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCrypt.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Servicio_Social
{
    public partial class RegistroDependencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                cargarUnidades();
                cargarOrganismos();
                CargarConfiguracion();
            }
        }


        private void CargarConfiguracion()
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real

            // Query para obtener la configuración específica para Registro de Dependencias
            string query = "SELECT bActivo, dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE sClave = '1'";

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
                            pnlRegistrarDependencia.Visible =  dentroDelRango;
                            PanelCerrado.Visible =  !dentroDelRango;

                            // Asignar el mensaje al control h3 del frontend
                            lblMensajeDependencia.Text = mensaje;
                        }
                    }
                }
            }
        }
        public string passwordHash(string _password)
        {
            return BCrypt.Net.BCrypt.HashPassword(_password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool VerificarCorreo(string correo)
        {
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real

            // Query para verificar si el correo existe en la base de datos
            string query = "SELECT COUNT(*) FROM SM_USUARIO WHERE sCorreo = @Email and kpTipoUsuario = 2";

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

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Obtener los valores del formulario
            string _password, _email, _dependencia, _responsable, _sAreaResponsable, _telefono, _domicilio;
            int _unidad, _organismo;
            //_password = passwordHash(txtConfirmarContrasena.Text);
            _password = SeguridadUtils.Encriptar(txtConfirmarContrasena.Text);
            _email = txtEmail.Text;
            _unidad = (int)Convert.ToInt64(ddlUnidad2.SelectedValue); //agregar en dependencia
            _organismo = (int)Convert.ToInt64(ddlOrganismo.SelectedValue); //agregar en dependencia
            _dependencia = txtDependencia.Text; //agregar en dependencia
            _responsable = txtResponsable.Text;
            _telefono = txtTelefono.Text;
            _domicilio = txtDomicilio.Text;
            _sAreaResponsable = txtAreaResponsable.Text;

            // Cadena de conexión a tu base de datos SQL Server
            string connectionString = GlobalConstants.SQL;

            if (VerificarCorreo(_email))
            {
                lblMensaje.Text = "El correo ya se encuentra registrado.";
                btnRegistrar.Enabled = false;
            }

            else
            {
                btnRegistrar.Enabled = true;
                // Inicializar la conexión y la transacción
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Primero, insertar en la tabla SM_USUARIO
                        int idUsuarioInsertado = InsertarUsuario(_password, _email, _telefono, connection, transaction);

                        // Luego, insertar en la tabla SM_DEPENDENCIA_SERVICIO usando el ID del usuario insertado
                        InsertarDependenciaServicio(idUsuarioInsertado, _sAreaResponsable, _responsable, _telefono, _unidad, _organismo, _dependencia, _domicilio, connection, transaction);

                        // Commit de la transacción si todo fue exitoso
                        transaction.Commit();
                        pnlRegistrarDependencia.Visible = false;
                        pnlRegistroExitoso.Visible = true;
                        string cambio = "1";
                        enviarCorreo(_email, cambio);
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
        }

        private int InsertarUsuario(string _password, string _correo, string _telefono, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_USUARIO (sPassword, sCorreo, sTelefono, kpTipoUsuario) VALUES (@sPassword, @sCorreo, @sTelefono, @kpTipoUsuario); SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sPassword", _password);
                cmd.Parameters.AddWithValue("@sCorreo", _correo);
                cmd.Parameters.AddWithValue("@sTelefono", _telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", 2);
                //cmd.Parameters.AddWithValue("@sNombreUsuario", _nombreUsuario);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void InsertarDependenciaServicio(int _idUsuario, string sAreaResponsable, string _responsable,
            string _telefono, int _unidad, int _organismo, string _dependencia, string _domicilio, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_DEPENDENCIA_SERVICIO (kmUsuario, sAreaResponsable, sResponsable, sTelefono, kpUnidad, kpOrganismo, sDescripcion, sDomicilio, bAutorizado , bVigente, dFechaRegistroDep ) " +
                "VALUES (@kmUsuario, @sAreaResponsable, @sResponsable,@sTelefono, @kpUnidad,  @kpOrganismo, @sDescripcion, @sDomicilio, 20707, 1, GETDATE());";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kmUsuario", _idUsuario);
                cmd.Parameters.AddWithValue("@sAreaResponsable", sAreaResponsable);
                cmd.Parameters.AddWithValue("@sResponsable", _responsable);
                cmd.Parameters.AddWithValue("@sTelefono", _telefono);
                cmd.Parameters.AddWithValue("@kpUnidad", _unidad);
                cmd.Parameters.AddWithValue("@kpOrganismo", _organismo);
                cmd.Parameters.AddWithValue("@sDescripcion", _dependencia);
                cmd.Parameters.AddWithValue("@sDomicilio", _domicilio);
                cmd.ExecuteNonQuery();
            }
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

        #region Funciones Llenar DropDownList
        public void cargarUnidades()
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1);";

            // Agregar una opción predeterminada al DropDownList
            ddlUnidad2.Items.Add(new ListItem("Selecciona una opción", "-1"));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                // Ejecutar la consulta y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Añadir cada nombre a un elemento ListItem y agregarlo al DropDownList
                    ddlUnidad2.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                    //ddlUnidadPerfil.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                }

                // Cerrar la conexión
                con.Close();
            }
        }

        public void cargarOrganismos()
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idOrganismo, sDescripcion FROM SP_ORGANISMO";

            // Agregar una opción predeterminada al DropDownList
            ddlOrganismo.Items.Add(new ListItem("Selecciona una opción", "-1"));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                // Ejecutar la consulta y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Añadir cada nombre a un elemento ListItem y agregarlo al DropDownList
                    ddlOrganismo.Items.Add(new ListItem(reader["sDescripcion"].ToString(), reader["idOrganismo"].ToString()));
                    //ddlOrganimoPerfil.Items.Add(new ListItem(reader["sDescripcion"].ToString(), reader["idOrganismo"].ToString()));

                }

                // Cerrar la conexión
                con.Close();
            }
        }
        #endregion

        

    }
}