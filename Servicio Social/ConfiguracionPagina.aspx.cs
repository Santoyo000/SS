using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class ConfiguracionPagina : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipoUsuario"] == null)
                Response.Redirect("LoginAdministrador.aspx");

            if (!IsPostBack)
            {
                LoadImages();
                CargarFechas();
                CargarFechas2();
                CargarFechas3();
                // LoadRegistroDependenciasState();
            }
        }
        // Métodos de gestión del registro de dependencias
        private void LoadRegistroDependenciasState()
        {
            //string connectionString = GlobalConstants.SQL;
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    string query = "SELECT RegistroDependenciasAbierto FROM Configuracion WHERE Id = 1";
            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        bool registroAbierto = (bool)cmd.ExecuteScalar();
            //        chkConcluirRegistro.Checked = !registroAbierto;
            //    }
            //}
        }

        //protected void chkConcluirRegistro_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool registroAbierto = !chkConcluirRegistro.Checked;
        //    SaveRegistroDependenciasState(registroAbierto);
        //}

        private void CargarFechas()
        {
            string connectionString = GlobalConstants.SQL;
            string query = "SELECT dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE SCLAVE = '1'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFechaInicio.Text = Convert.ToDateTime(reader["dFechaInicio"]).ToString("yyyy-MM-dd"); 
                        txtFechaFin.Text = Convert.ToDateTime(reader["dFechaFin"]).ToString("yyyy-MM-dd");
                        // Cargar el mensaje
                        txtMensaje.Text = reader["sMensaje"] != DBNull.Value ? reader["sMensaje"].ToString() : string.Empty;
                    }
                    else
                    {
                        // Manejar el caso en que no se devuelven resultados
                        txtFechaInicio.Text = string.Empty;
                        txtFechaFin.Text = string.Empty;
                        txtMensaje.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes mostrar un mensaje de error en tu interfaz o registrar el error
                //lblMessageDep.Text = "Error al cargar fechas: " + ex.Message; // Asegúrate de tener un Label para mostrar mensajes de error
            }
        }
        private void CargarFechas2()
        {
            string connectionString = GlobalConstants.SQL;
            string query = "SELECT dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE SCLAVE = '2'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFechaInicioPro.Text = Convert.ToDateTime(reader["dFechaInicio"]).ToString("yyyy-MM-dd");
                        txtFechaFinPro.Text = Convert.ToDateTime(reader["dFechaFin"]).ToString("yyyy-MM-dd");
                        // Cargar el mensaje
                        txtMensaje2.Text = reader["sMensaje"] != DBNull.Value ? reader["sMensaje"].ToString() : string.Empty;
                    }
                    else
                    {
                        // Manejar el caso en que no se devuelven resultados
                        txtFechaInicioPro.Text = string.Empty;
                        txtFechaFinPro.Text = string.Empty;
                        txtMensaje2.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes mostrar un mensaje de error en tu interfaz o registrar el error
                //lblMessageDep.Text = "Error al cargar fechas: " + ex.Message; // Asegúrate de tener un Label para mostrar mensajes de error
            }
        }
        private void CargarFechas3()
        {
            string connectionString = GlobalConstants.SQL;
            string query = "SELECT dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE SCLAVE = '3'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFechaInicioAlu.Text = Convert.ToDateTime(reader["dFechaInicio"]).ToString("yyyy-MM-dd");
                        txtFechaFinAlu.Text = Convert.ToDateTime(reader["dFechaFin"]).ToString("yyyy-MM-dd");
                        // Cargar el mensaje
                        txtMensaje3.Text = reader["sMensaje"] != DBNull.Value ? reader["sMensaje"].ToString() : string.Empty;
                    }
                    else
                    {
                        // Manejar el caso en que no se devuelven resultados
                        txtFechaInicioAlu.Text = string.Empty;
                        txtFechaFinAlu.Text = string.Empty;
                        txtMensaje3.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes mostrar un mensaje de error en tu interfaz o registrar el error
                //lblMessageDep.Text = "Error al cargar fechas: " + ex.Message; // Asegúrate de tener un Label para mostrar mensajes de error
            }
        }
        private void SaveRegistroDependenciasState(bool registroAbierto)
        {
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Configuracion SET RegistroDependenciasAbierto = @RegistroAbierto WHERE Id = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RegistroAbierto", registroAbierto);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void LoadImages()
        {
            string connectionString = GlobalConstants.SQL;
            DataTable dtImages = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IdImage, ImageData FROM SM_ImagesSS ORDER BY IdImage";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtImages);
                    }
                }
            }

            rptImages.DataSource = dtImages;
            rptImages.DataBind();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btnDelete = (Button)sender;
            int imageId = Convert.ToInt32(btnDelete.CommandArgument);

            DeleteImage(imageId);

            // Recargar las imágenes después de la eliminación
            LoadImages();
        }

        private void DeleteImage(int id)
        {
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM SM_ImagesSS WHERE IdImage = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //[WebMethod]
        //public static void EliminarImagen(int idImagen)
        //{
        //    DeleteImage(idImagen);
        //}

        //// Método para eliminar la imagen
        //private static void DeleteImage(int id)
        //{
        //    string connectionString = GlobalConstants.SQL;

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        string query = "DELETE FROM SM_ImagesSS WHERE IdImage = @Id";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Id", id);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
        private void SaveImageToDatabase(byte[] imageData)
        {
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Obtener el número de imágenes existentes para generar el nuevo nombre
                string countQuery = "SELECT COUNT(*) FROM SM_ImagesSS";
                int imageCount;

                using (SqlCommand countCmd = new SqlCommand(countQuery, conn))
                {
                    imageCount = (int)countCmd.ExecuteScalar(); // Contar las imágenes
                }

                // Generar el nombre de la imagen basado en el conteo
                string imageName = $"Image_{imageCount + 1}.jpg"; // Sumar 1 al conteo para el nuevo nombre

                // Insertar la imagen
                string query = "INSERT INTO SM_ImagesSS (ImageData, ImageName) VALUES (@ImageData, @ImageName)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ImageData", imageData);
                    cmd.Parameters.AddWithValue("@ImageName", imageName);
                    cmd.ExecuteNonQuery();
                
                
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                // Aquí va el código para guardar la imagen en la base de datos
                // Por ejemplo:
                byte[] imageData = new byte[fileUpload.PostedFile.ContentLength];
                fileUpload.PostedFile.InputStream.Read(imageData, 0, fileUpload.PostedFile.ContentLength);

                // Llamar al método que guarda la imagen en la base de datos
                SaveImageToDatabase(imageData);

                // Mostrar la última imagen insertada
                LoadImages();
                //ShowLastImage();
            }
            else
            {
                lblMessage.Text = "Por favor, selecciona una imagen.";
            }
        }
        private void ShowLastImage()
        {
            byte[] imageData = GetLastImageFromDatabase();

            if (imageData != null)
            {
                // Convertir los bytes a una cadena base64 para mostrar en el control de imagen
                string base64String = Convert.ToBase64String(imageData, 0, imageData.Length);
                //imgPreview.ImageUrl = "data:image/jpeg;base64," + base64String;
            }
            else
            {
                lblMessage.Text = "No se encontró ninguna imagen.";
            }
        }
        private string GetImageData(int imageId)
        {
            string base64String = "";
            string connectionString = GlobalConstants.SQL;
            //string connString = ConfigurationManager.ConnectionStrings["TuCadenaDeConexion"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ImagesSS FROM SM_ImagesSS WHERE IdImage = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", imageId);
                    conn.Open();
                    byte[] imageData = (byte[])cmd.ExecuteScalar();
                    base64String = Convert.ToBase64String(imageData);
                }
            }
            return "data:image/png;base64," + base64String; // Cambia el tipo según el formato de tu imagen
        }

        private byte[] GetLastImageFromDatabase()
        {
            byte[] imageData = null;
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Obtener la última imagen insertada
                string query = "SELECT TOP 1 ImageData FROM SM_ImagesSS ORDER BY IdImage DESC"; // Asumiendo que 'Id' es el campo autoincremental

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["ImageData"] as byte[];
                        }
                    }
                }
            }

            return imageData;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtFechaInicio.Text) ||
                string.IsNullOrWhiteSpace(txtFechaFin.Text) ||
                string.IsNullOrWhiteSpace(txtMensaje.Text))
            {
                // Mostrar mensaje de error

                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#ModalError').modal('show');", true);
                return; // Salir del método si hay campos vacíos
            }

            DateTime fechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
            DateTime fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            string mensaje = txtMensaje.Text;

            // Query para actualizar las fechas de inicio y fin en la tabla SP_CONFIGURACION_PAG_SS para la clave '1'
            string query = "UPDATE SP_CONFIGURACION_PAG_SS SET dFechaInicio = @FechaInicio, dFechaFin = @FechaFin, sMensaje = @Mensaje WHERE sClave = '1'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Asignar los parámetros
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);
                    command.Parameters.AddWithValue("@Mensaje", mensaje);

                    // Abrir la conexión
                    connection.Open();

                    // Ejecutar la consulta
                    command.ExecuteNonQuery();
                }
            }

            // Mensaje de confirmación
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        }
        protected void btnGuardarPro_Click(object sender, EventArgs e)
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtFechaInicioPro.Text) ||
                string.IsNullOrWhiteSpace(txtFechaFinPro.Text) ||
                string.IsNullOrWhiteSpace(txtMensaje2.Text))
            {
                // Mostrar mensaje de error

                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#ModalError').modal('show');", true);
                return; // Salir del método si hay campos vacíos
            }

            DateTime fechaInicio = Convert.ToDateTime(txtFechaInicioPro.Text);
            DateTime fechaFin = Convert.ToDateTime(txtFechaFinPro.Text);
            string mensaje = txtMensaje2.Text;

            // Query para actualizar las fechas de inicio y fin en la tabla SP_CONFIGURACION_PAG_SS para la clave '1'
            string query = "UPDATE SP_CONFIGURACION_PAG_SS SET dFechaInicio = @FechaInicio, dFechaFin = @FechaFin, sMensaje = @Mensaje WHERE sClave = '2'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Asignar los parámetros
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);
                    command.Parameters.AddWithValue("@Mensaje", mensaje);

                    // Abrir la conexión
                    connection.Open();

                    // Ejecutar la consulta
                    command.ExecuteNonQuery();
                }
            }

            // Mensaje de confirmación
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        }

        protected void btnGuardarAlu_Click(object sender, EventArgs e)
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtFechaInicioAlu.Text) ||
                string.IsNullOrWhiteSpace(txtFechaFinAlu.Text) ||
                string.IsNullOrWhiteSpace(txtMensaje3.Text))
            {
                // Mostrar mensaje de error

                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#ModalError').modal('show');", true);
                return; // Salir del método si hay campos vacíos
            }

            DateTime fechaInicio = Convert.ToDateTime(txtFechaInicioAlu.Text);
            DateTime fechaFin = Convert.ToDateTime(txtFechaFinAlu.Text);
            string mensaje = txtMensaje3.Text;

            // Query para actualizar las fechas de inicio y fin en la tabla SP_CONFIGURACION_PAG_SS para la clave '1'
            string query = "UPDATE SP_CONFIGURACION_PAG_SS SET dFechaInicio = @FechaInicio, dFechaFin = @FechaFin , sMensaje = @Mensaje WHERE sClave = '3'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Asignar los parámetros
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);
                    command.Parameters.AddWithValue("@Mensaje", mensaje);

                    // Abrir la conexión
                    connection.Open();

                    // Ejecutar la consulta
                    command.ExecuteNonQuery();
                }
            }

            // Mensaje de confirmación
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "$('#ModalExitoso').modal('show');", true);
        }

    }


}