using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class PerfilDependencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idDependencia"] == null)
                Response.Redirect("LoginDependencia.aspx");

            if (!IsPostBack)
            {
                cargarUnidades();
                cargarOrganismos();
                LlenarDatosUsuario((int)Convert.ToInt32(Session["idDependencia"].ToString().Split('|')[0]));
            }
        }

        #region Funciones Llenar DropDownList
        public void cargarUnidades()
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1);";

            // Agregar una opción predeterminada al DropDownList
            ddlUnidadPerfil.Items.Add(new ListItem("Selecciona una opción", "-1"));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                // Ejecutar la consulta y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Añadir cada nombre a un elemento ListItem y agregarlo al DropDownList
                    ddlUnidadPerfil.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
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
            ddlOrganimoPerfil.Items.Add(new ListItem("Selecciona una opción", "-1"));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                // Ejecutar la consulta y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Añadir cada nombre a un elemento ListItem y agregarlo al DropDownList
                    ddlOrganimoPerfil.Items.Add(new ListItem(reader["sDescripcion"].ToString(), reader["idOrganismo"].ToString()));
                    //ddlOrganimoPerfil.Items.Add(new ListItem(reader["sDescripcion"].ToString(), reader["idOrganismo"].ToString()));

                }

                // Cerrar la conexión
                con.Close();
            }
        }
        #endregion

        #region Funciones Operacionales
        public void LlenarDatosUsuario(int idUser)
        {
            string conStrin = GlobalConstants.SQL;
            string query = "SELECT DP.sResponsable, DP.sTelefono, DP.kpUnidad, DP.kpOrganismo, " +
            "DP.sDescripcion, DP.sDomicilio, U.sCorreo, DP.sAreaResponsable " + // Agregar espacio al final
            "FROM SM_DEPENDENCIA_SERVICIO DP " +
            "INNER JOIN SM_USUARIO U ON U.idUsuario = DP.kmUsuario " + // Agregar espacio al final
            "WHERE U.idUsuario = @idUsuario;";

            using (SqlConnection connection = new SqlConnection(conStrin))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUser); // Asignar valor al parámetro Id

                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Obtener datos de la consulta
                            txtResponsablePerfil.Text = reader["sResponsable"].ToString();
                            txtTelefonoPerfil.Text = reader["sTelefono"].ToString();
                            ddlUnidadPerfil.SelectedValue = reader["kpUnidad"].ToString();
                            ddlOrganimoPerfil.SelectedValue = reader["kpOrganismo"].ToString();
                            txtDependenciaPerfil.Text = reader["sDescripcion"].ToString();
                            txtDomicilioPerfil.Text = reader["sDomicilio"].ToString();
                            txtEmailPerfil.Text = reader["sCorreo"].ToString();
                            txtAreaResponsablePerfil.Text = reader["sAreaResponsable"].ToString();
                        }
                    }
                }
            }
        }

        public bool updateDatos(string _responsable, string _telefono, int _unidad, int _organismo, string _dependencia, string _domicilio, string _arearesponsable)
        {
            bool result = false;
            string _idUsuario = Session["idDependencia"].ToString().Split('|')[0];
            string _idDependencia = Session["idDependencia"].ToString().Split('|')[1];
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Inicia una transacción
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Update SM_DEPENDENCIA_SERVICIO
                    using (SqlCommand cmd1 = new SqlCommand("UPDATE SM_DEPENDENCIA_SERVICIO SET sResponsable = @sResponsable, sTelefono = @sTelefono, kpUnidad = @kpUnidad, " +
                        "kpOrganismo = @kpOrganismo, sDescripcion = @sDescripcion, sDomicilio = @sDomicilio, sAreaResponsable = @sAreaResponsable WHERE idDependenicaServicio = @id", connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@sResponsable", _responsable);
                        cmd1.Parameters.AddWithValue("@sTelefono", _telefono);
                        cmd1.Parameters.AddWithValue("@kpUnidad", _unidad);
                        cmd1.Parameters.AddWithValue("@kpOrganismo", _organismo);
                        cmd1.Parameters.AddWithValue("@sDescripcion", _dependencia);
                        cmd1.Parameters.AddWithValue("@sDomicilio", _domicilio);
                        cmd1.Parameters.AddWithValue("@sAreaResponsable", _arearesponsable);
                        cmd1.Parameters.AddWithValue("@id", _idDependencia);
                        cmd1.ExecuteNonQuery();
                    }

                    // Update SM_USUARIO
                    using (SqlCommand cmd2 = new SqlCommand("UPDATE SM_USUARIO SET sTelefono = @sTelefono WHERE idUsuario = @id", connection, transaction))
                    {
                        cmd2.Parameters.AddWithValue("@sTelefono", _telefono);
                        cmd2.Parameters.AddWithValue("@id", _idUsuario);
                        cmd2.ExecuteNonQuery();
                    }

                    // Confirma la transacción
                    transaction.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, realiza un rollback
                    transaction.Rollback();
                    result = false;
                }
            }
            return result;
        }
        #endregion

        #region Botones
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string _resonsable = txtResponsablePerfil.Text;
            string _telefono = txtTelefonoPerfil.Text;
            int _unidad = (int)Convert.ToInt32(ddlUnidadPerfil.SelectedValue);
            int _organismo = (int)Convert.ToInt32(ddlOrganimoPerfil.SelectedValue);
            string _dependencia = txtDependenciaPerfil.Text;
            string _domicilio = txtDomicilioPerfil.Text;
            string _arearesponsable = txtAreaResponsablePerfil.Text;
            if (updateDatos(_resonsable, _telefono, _unidad, _organismo, _dependencia, _domicilio, _arearesponsable))
            {
                lblResult.Text = "Datos actualizados con éxito";
                lblResult.ForeColor = System.Drawing.Color.Green;
            }

            else
            {
                lblResult.Text = "Ha ocurrido un error con la actualización de los datos, intente más tarde.";
                lblResult.ForeColor = System.Drawing.Color.Red;
            }


        }
        #endregion
    }
}