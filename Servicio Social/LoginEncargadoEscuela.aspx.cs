using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class LoginEncargadoEscuela : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region Metodos
        public Boolean autenticar(string usuario, string password)
        {
            Boolean response = false;
            string scriptErrorUserPass = "alert('Usuario o contraseña incorrectos.'); window.location='" + Request.ApplicationPath + "LoginEstudiante.aspx'";

            string ldapHost = "148.212.9.25";
            int ldapPort = 389;
            string searchBase = "DC=uadec,DC=edu,DC=mx";
            string searchFilter = "mail=" + usuario;
            string[] requiredAttributes = { "mail", "name", "description", "SAMaccountname" };

            if (VerifyPassword(usuario, password))
            {
                try
                {
                    string matricula = "";
                    string nombre = "";
                    LdapConnection conn = new LdapConnection();
                    conn.Connect(ldapHost, ldapPort);
                    conn.Bind(usuario, password);

                    LdapSearchResults lsc = conn.Search(searchBase, LdapConnection.SCOPE_SUB, searchFilter, requiredAttributes, false);
                    LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = lsc.next();
                        string accountName = nextEntry.getAttribute("SAMACCOUNTNAME").StringValue;
                        matricula = nextEntry.getAttribute("description").StringValue;
                        nombre = nextEntry.getAttribute("name").StringValue;

                        Session["expediente"] = matricula;
                        Session["empleado"] = nombre;
                        Session["mail"] = accountName;
                        response = true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Ocurrió una excepción al momento de iniciar sesión, intente más tarde" + ex.Message;
                        response = false;
                    }
                }

                catch (Exception ex)
                {
                    lblError.Text = "Usuario y/o contraseña incorrecta.";
                    response = false;
                }
            }

            return response;
        }

        public bool VerifyPassword(string username, string password)
        {
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;

            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT idUsuario, kpTipoUsuario, kpUnidad,kpEscuela, bAutorizado FROM SM_USUARIO WHERE sCorreo = @sCorreo AND kpTipoUsuario = 4";
            string autorizado = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agrega el parámetro del nombre de usuario a la consulta
                    command.Parameters.AddWithValue("@sCorreo", username);
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener datos de la consulta
                            Session["idUser"] = reader["idUsuario"].ToString();
                            Session["filtros"] = reader["kpTipoUsuario"].ToString() + '|' + reader["kpUnidad"].ToString() + '|' + reader["kpEscuela"].ToString();
                            Session["tipoUsuario"] = reader["kpTipoUsuario"].ToString();
                            autorizado = reader["bAutorizado"].ToString();
                        }
                    }

                    // Verifica si el hash de la contraseña almacenada coincide con la contraseña proporcionada
                    if (Session["idUser"] == null)
                    {
                        lblError.Text = "El correo ingresado no tiene acceso a este módulo.";
                        return false;
                    }
                    else
                    {
                        if (autorizado == "0")
                        {
                            lblError.Text = "El usuario aún no ha sido validado.";
                            return false;
                        }
                        else
                        {
                            lblError.Text = "";
                            return true;

                        }
                    }
                }
            }
        }
        public bool VerificarOf(string usuario)
        {
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;

            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT idUsuario, kpTipoUsuario, kpUnidad,kpEscuela, bAutorizado FROM SM_USUARIO WHERE sCorreo = @sCorreo AND kpTipoUsuario = 4 and kmIdentificador IS NOT NULL ";
            string autorizado = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agrega el parámetro del nombre de usuario a la consulta
                    command.Parameters.AddWithValue("@sCorreo", usuario);
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener datos de la consulta
                            Session["idUser"] = reader["idUsuario"].ToString();
                            Session["filtros"] = reader["kpTipoUsuario"].ToString() + '|' + reader["kpUnidad"].ToString() + '|' + reader["kpEscuela"].ToString();
                            Session["tipoUsuario"] = reader["kpTipoUsuario"].ToString();
                            autorizado = reader["bAutorizado"].ToString();
                        }
                    }

                    // Verifica si el hash de la contraseña almacenada coincide con la contraseña proporcionada
                    if (Session["idUser"] == null)
                    {
                        lblError.Text = "El correo ingresado no tiene acceso a este módulo.";
                        return false;
                    }
                    else
                    {
                        if (autorizado == "0")
                        {
                            lblError.Text = "El usuario aún no ha sido validado.";
                            return false;
                        }
                        else
                        {
                            lblError.Text = "";
                            return true;

                        }
                    }
                }
            }
        }

        public bool autenticarIncorp(string usuario, string password)
        {
            bool result = false;
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;
            string hashedPassword = "", autorizado = "";
            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT idUsuario, sPassword ,kpTipoUsuario, kpUnidad,kpEscuela, bAutorizado FROM SM_USUARIO WHERE sCorreo = @sCorreo AND kpTipoUsuario = 4 and kmIdentificador IS NULL";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agrega el parámetro del nombre de usuario a la consulta
                    command.Parameters.AddWithValue("@sCorreo", usuario);
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener datos de la consulta
                            hashedPassword = reader["sPassword"].ToString();
                            Session["idUser"] = reader["idUsuario"].ToString();
                            Session["filtros"] = reader["kpTipoUsuario"].ToString() + '|' + reader["kpUnidad"].ToString() + '|' + reader["kpEscuela"].ToString();
                            Session["tipoUsuario"] = reader["kpTipoUsuario"].ToString();
                            autorizado = reader["bAutorizado"].ToString();

                        }
                    }

                    // Verifica si el hash de la contraseña almacenada coincide con la contraseña proporcionada
                    if (hashedPassword != null && hashedPassword != "")
                    {
                      
                            lblError.Text = "";
                            string hassh = SeguridadUtils.Desencriptar(hashedPassword);
                            if (password != hassh)
                            {
                                lblError.Text = "Usuario y/o contraseña incorrecta.";
                            }
                            result = (password == hassh);
                        
                    }
                    else
                    {
                        lblError.Text = "El correo ingresado no se encuentra registrado.";
                    }
                    return result;

                }
            }
            // Si el usuario no existe en la base de datos o no se encuentra la contraseña, devuelve falso
            return false;

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    // Abre la conexión a la base de datos
            //    connection.Open();

            //    // Crea un comando SQL con la consulta y los parámetros
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        // Agrega el parámetro del nombre de usuario a la consulta
            //        command.Parameters.AddWithValue("@sCorreo", usuario);
            //        // Ejecutar consulta y leer datos
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                // Obtener datos de la consulta
            //                Session["idUser"] = reader["idUsuario"].ToString();
            //            }
            //        }

            //        // Verifica si el hash de la contraseña almacenada coincide con la contraseña proporcionada
            //        if (Session["idUser"] == null)
            //        {

            //            return false;
            //        }
            //        else
            //        {

            //            lblError.Text = "";
            //            return true;

            //        }
            //    }
            //}
        }
        #endregion

        #region Botones
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = "", password = "";
            usuario = txtCorreo.Text;
            password = txtPassword.Text;
            if (VerificarOf(usuario))
            {
                if (autenticar(usuario, password))
                {
                    Response.Redirect("PanelEncargadoEscuelaspx.aspx");

                }
             

            }
            else  if (autenticarIncorp(usuario, password))
            {
                Response.Redirect("PanelEncargadoEscuelaspx.aspx");

            }
        }
        #endregion
    }

}