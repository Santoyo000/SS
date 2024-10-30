using System;
using Novell.Directory.Ldap;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;

namespace Servicio_Social
{
    public partial class RegistroEstudiante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Metodos
        public Boolean autenticarAD(string usuario, string password)
        {
            Boolean response = false;

            string ldapHost = "148.212.9.25";
            int ldapPort = 389;
            string searchBase = "DC=uadec,DC=edu,DC=mx";
            string searchFilter = "mail=" + usuario;
            string[] requiredAttributes = { "mail", "name", "description", "SAMaccountname" };

            if (VerificarUsuario(usuario))
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

                        Session["alumno"] = nombre;
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
                    lblError.Text = "Usuario y/o contraseña incorrecta." + ex.Message;
                    response = false;
                }
            }

            return response;
        }

        public bool VerificarUsuario(string username)
        {
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;
            string tipoUsuario = ""; string idAlumno = "";

            int totalPlanEscuela = 0;

            List<(string sMatricula, string Autorizado)> registros = new List<(string sMatricula, string Autorizado)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_ConsultarDatosUsuario_ss", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@sCorreo", username);
                    command.Parameters.AddWithValue("@kpTipoUsuario", "5"); // 5 => TipoAlumno

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string matricula = reader["sMatricula"].ToString();
                            string Autorizado = reader["bAutorizado"].ToString();
                            Session["matricula"] = matricula;
                            Session["ID"] = reader["ID"].ToString();
                            tipoUsuario = reader["kpTipoUsuario"].ToString();
                            idAlumno = reader["ID"].ToString();
                            registros.Add((matricula, Autorizado));
                        }
                    }
                    totalPlanEscuela = registros.Count;

                    //Si tiene más de una escuela la variable Session["plan"] es nula para que se seleccione que plan elige el alumno
                    if (totalPlanEscuela > 1)
                    {
                        Session["tipoUsuario"] = tipoUsuario + '|' + totalPlanEscuela;
                        Session["plan"] = "";
                    }
                    //Si solo tiene una escuela se obtiene el plan registrado
                    else
                    {
                        Session["tipoUsuario"] = tipoUsuario + '|' + totalPlanEscuela + '|' + idAlumno;
                        Session["plan"] = obtenerPlan(idAlumno);
                    }

                    if (Session["matricula"] == null)
                    {
                        lblError.Text = "El correo ingresado no tiene acceso a este módulo.";
                        return false;
                    }
                    else
                    {
                        string autorizado = "";
                        foreach (var (sMatricula, Autorizado) in registros)
                        {
                            if (Autorizado == "11")
                                autorizado = Autorizado;
                        }
                        if (autorizado == "11")
                        {
                            lblError.Text = "";
                            return true;
                        }
                        else
                        {
                            lblError.Text = "El usuario aún no ha sido autorizado.";
                            return false;
                        }
                    }
                }
            }
        }

        public string VerificarUsuarioInc(string username)
        {
            string connectionString = GlobalConstants.SQL;
            string result = "", autorizado = "", matricula = "", tipoUsuario = "", idAlumno = "";
            int totalPlanEscuela = 0;

            List<(string sMatricula, string Autorizado)> registros = new List<(string sMatricula, string Autorizado)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_ConsultarDatosUsuario_ss", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@sCorreo", username);
                    command.Parameters.AddWithValue("@kpTipoUsuario", "5"); // 5 => TipoAlumno

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            matricula = reader["sMatricula"].ToString();
                            autorizado = reader["bAutorizado"].ToString();
                            Session["alumno"] = reader["sNombre_completo"].ToString();
                            Session["matricula"] = matricula;
                            Session["ID"] = reader["ID"].ToString();
                            tipoUsuario = reader["kpTipoUsuario"].ToString();
                            result = reader["sPassword"].ToString();
                            idAlumno = reader["ID"].ToString();
                            registros.Add((matricula, autorizado));
                        }
                    }
                    totalPlanEscuela = registros.Count;
                    //Si tiene más de una escuela la variable Session["plan"] es nula para que se seleccione que plan elige el alumno
                    if (totalPlanEscuela > 1)
                    {
                        Session["tipoUsuario"] = tipoUsuario + '|' + totalPlanEscuela;
                        Session["plan"] = "";
                    }
                    //Si solo tiene una escuela se obtiene el plan registrado
                    else
                    {
                        Session["tipoUsuario"] = tipoUsuario + '|' + totalPlanEscuela + '|' + idAlumno;
                        Session["plan"] = obtenerPlan(idAlumno);
                    }

                    if (Session["matricula"] == null)
                    {
                        lblError.Text = "El correo ingresado no tiene acceso a este módulo.";
                        return "";
                    }
                    else
                    {
                        if (autorizado == "0")
                        {
                            lblError.Text = "Su usuario aún no ha sido autorizado";
                            return "";
                        }
                        else
                        {
                            lblError.Text = "";
                            return result;
                        }
                    }

                }
            }
        }
        #endregion

        #region Botones

        #region Boton Inicio Sesion
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario, password;
            usuario = txtUsuario.Text;
            password = txtPassword.Text;

            //correo institucional y es esc. oficial.
            if (usuario.Contains("@uadec.edu.mx"))
            {
                if (autenticarAD(usuario, password))
                {
                    lblError.Text = ""; // Limpiar el mensaje de error si son correctos
                    lblError.Visible = false; // Ocultar la etiqueta de error

                    int totalEscuelasPlanes = (int)Convert.ToInt32(Session["tipoUsuario"].ToString().Split('|')[1]);

                    if (totalEscuelasPlanes > 1)
                    {
                        Response.Redirect("SeleccionPlan.aspx");
                    }
                    else
                    {
                        Response.Redirect("PanelEstudiante");
                    }
                }
            }
            //Alumnos incorporados
            else
            {
                string result = VerificarUsuarioInc(usuario);
                if (!string.IsNullOrEmpty(result))
                {

                    if (password == SeguridadUtils.Desencriptar(result))
                    {
                        
                        int totalEscuelasPlanes = (int)Convert.ToInt32(Session["tipoUsuario"].ToString().Split('|')[1]);

                        if (totalEscuelasPlanes > 1)
                        {
                            Response.Redirect("SeleccionPlan.aspx");
                        }
                        else
                        {
                            Response.Redirect("PanelEstudiante");
                        }
                    }
                    else
                    {
                        lblError.Text = "Usuario y/o contraseña incorrectos.";
                    }
                }
            }
        }
        #endregion

        #endregion
        protected void lkbRegistro_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegistroEstudiante.aspx");
        }

        public string obtenerPlan(string id)
        {
            string plan = "";
            string connectionString = GlobalConstants.SQL;

            List<(string sMatricula, string Autorizado)> registros = new List<(string sMatricula, string Autorizado)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_ObtenerPlanAlumno_ss", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            plan = reader["planes"].ToString();
                        }
                    }
                }
            }

            return plan;
        }
    }
}