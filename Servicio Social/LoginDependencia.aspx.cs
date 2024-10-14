using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using BCrypt.Net;
using System.Web.Services;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Web.UI;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;

namespace Servicio_Social
{
    public partial class Dependencias1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //cargarUnidades();
                //cargarOrganismos();

                //rbtnBuscar.SelectedIndex = 0;
                //lRegistroExitoso.Visible = false;
                Session.Clear();
                Session.Abandon();
            }

           
        }

        #region Clases
        public class Persona
        {
            public string Programa { get; set; }
            public int Matrícula { get; set; }
            public string Prestador { get; set; }
            public string Estatus { get; set; }
            public string Fecha_Estatus { get; set; }
            //public string Operacion { get; set; }


        }
        #endregion

        

        #region Funciones Operaciones

       
        public bool VerifyPassword(string username, string password)
        {
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;
            string hashedPassword = "", autorizado = "";

            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT U.sPassword, U.idUsuario, DP.idDependenicaServicio, DP.bAutorizado, U.kpTipoUsuario " +
                "FROM SM_DEPENDENCIA_SERVICIO DP " +
                "INNER JOIN SM_USUARIO U ON U.idUsuario = DP.kmUsuario " +
                "WHERE U.sCorreo = @sCorreo AND U.kpTipoUsuario = 2";

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
                            hashedPassword = reader["sPassword"].ToString();
                            Session["idDependencia"] = reader["idUsuario"].ToString() + '|' + reader["idDependenicaServicio"];
                            Session["tipoUsuario"] = reader["kpTipoUsuario"].ToString();
                            autorizado = reader["bAutorizado"].ToString();
                            
                        }
                    }

                    // Verifica si el hash de la contraseña almacenada coincide con la contraseña proporcionada
                    if (hashedPassword != null && hashedPassword != "")
                    {
                        if (autorizado != "11")
                        {
                            lblError.Text = "La Dependencia no ha sido autorizada.";
                            return false;
                        }
                        else
                        {
                            lblError.Text = "";
                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword) == false)
                            {
                                lblError.Text = "Usuario y/o contraseña incorrecta.";
                            }
                            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                            
                                
                        }
                    }
                    else
                    {
                        lblError.Text = "El correo ingresado no se encuentra registrado.";

                    }

                }
            }
            // Si el usuario no existe en la base de datos o no se encuentra la contraseña, devuelve falso
            return false;
        }
       
        

        //public Boolean usuarioAutorizado(string email)
        //{
        //    Boolean result = false;
        //    string conStrin = GlobalConstants.SQL;
        //    string bAutorizado = "";
        //    string query = "SELECT DP.bAutorizado " +
        //        "FROM SM_DEPENDENCIA_SERVICIO DP " +
        //        "INNER JOIN SM_USUARIO U ON U.idUsuario = DP.kmUsuario " +
        //        "WHERE U.sCorreo = @sCorreo";
        //    using (SqlConnection connection = new SqlConnection(conStrin))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@sCorreo", email); // Asignar valor al parámetro Id

        //            // Ejecutar consulta y leer datos
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    // Obtener datos de la consulta
        //                    bAutorizado = reader["bAutorizado"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    if (bAutorizado == "1")
        //        result = true;
        //    else
        //        result = false;
        //    return result;

        //}

        

        

        #endregion

        #region Botones 
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string user, password;
            user = usuario.Text;
            password = inPassword.Text;

            if (VerificarContrasena(user, password))
            {
                //pnlIngreso.Visible = true;
                //pnlBuscar.Visible = false;
                pnlLogin.Visible = false;
                //PnlEjemploModPerfil.Visible = false;
                //int idUser = (int)Convert.ToInt32(Session["idDependencia"].ToString());
                //LlenarDatosUsuario(idUser);
                //LinkButton lkbLogout = (LinkButton)Master.FindControl("lkbLogout");
                //lkbLogout.Visible = true;

                Response.Redirect("PanelDependencia.aspx");
            }

            //else
            //{
            //    lblError.Text = "Correo o contraseña incorrectos";

            //}

            //string conString = GlobalConstants.SQL;
            //string query = "SELECT idUsuario FROM SM_USUARIO WHERE sCorreo = @Correo AND sPassword = @Password";

            //if (!usuarioAutorizado(user))
            //{
            //    lblError.Text = "La Dependencia no ha sido validada. Favor de contactar al responsable correspondiente.";
            //    lblError.Visible = true;
            //}
            //else
            //{
            //    lblError.Text = ""; // Limpiar el mensaje de error si son correctos
            //    lblError.Visible = false;
            //    using (SqlConnection connection = new SqlConnection(conString))
            //    {
            //        using (SqlCommand command = new SqlCommand(query, connection))
            //        {
            //            // Parámetros
            //            command.Parameters.AddWithValue("@Correo", user.Trim());
            //            command.Parameters.AddWithValue("@Password", password.Trim());

            //            // Abre la conexión
            //            connection.Open();

            //            // Ejecuta la consulta y obtiene el número de coincidencias
            //            int idUser = (int)Convert.ToInt32(command.ExecuteScalar());

            //            // Verifica si las credenciales son válidas
            //            if (idUser > 0)
            //            {
            //                // Credenciales válidas - redirige al usuario a la página de inicio
            //                pnlIngreso.Visible = true;
            //                pnlBuscar.Visible = false;
            //                pnlLogin.Visible = false;
            //                PnlEjemploModPerfil.Visible = true;
            //                Session["idUSer"] = idUser;
            //                LlenarDatosUsuario(idUser);
            //            }
            //            else
            //            {
            //                // Credenciales inválidas - muestra un mensaje de error
            //                lblError.Text = "Correo o contraseña incorrectos";
            //                lblError.Visible = true;

            //            }
            //        }
            //    }





            //// Establecer el SelectedIndex en -1
            //rbtnBuscar.SelectedIndex = -1;
            //rbtnIngreso.SelectedIndex = 0;

            //// Verificar si el SelectedIndex es -1 y ocultar los paneles correspondientes
            //if (rbtnBuscar.SelectedIndex == -1)
            //{
            //    pnlIngreso.Visible = true;
            //    pnlBuscar.Visible = false;
            //    Panel1.Visible = false;
            //}
            //if (rbtnIngreso.SelectedIndex == 0)
            //{
            //    pnlIngreso.Visible = true;
            //    pnlBuscar.Visible = false;
            //    Panel1.Visible = false;
            //    PnlEjemploModPerfil.Visible = true;
            //}

        }



        //protected void rbtnIngreso_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rbtnIngreso.SelectedValue == "A")
        //    {
        //        //pnlEstudiantesRegistrados.Visible = false;
        //        pnlLogin.Visible = false;
        //        pnlRegistrarDependencia.Visible = false;
        //        //PnlEjemploModPerfil.Visible = true;
        //        pnlRegistroExitoso.Visible = false;
        //        //pnlPDF.Visible = false;
        //    }
        //    else if (rbtnIngreso.SelectedValue == "B")
        //    {
        //        Response.Redirect("Programas.aspx");
        //    }
        //    else if (rbtnIngreso.SelectedValue == "C")
        //    {
        //        //pnlEstudiantesRegistrados.Visible = true;
        //        pnlLogin.Visible = false;
        //        pnlRegistrarDependencia.Visible = false;
        //        //PnlEjemploModPerfil.Visible = false;
        //        //pnlPDF.Visible = false;
        //        pnlRegistroExitoso.Visible = false;
        //    }


        //}

        protected void lkbRegistro_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegistroDependencia.aspx");
        }

        public bool VerificarContrasena(string usuario, string password)
        {
            bool result = false;
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;
            string hashedPassword = "", autorizado = "";

            // Define la consulta SQL para recuperar el hash de contraseña basado en el nombre de usuario
            string query = "SELECT U.sPassword, U.idUsuario, DP.idDependenicaServicio, DP.bAutorizado, U.kpTipoUsuario " +
                "FROM SM_DEPENDENCIA_SERVICIO DP " +
                "INNER JOIN SM_USUARIO U ON U.idUsuario = DP.kmUsuario " +
                "WHERE U.sCorreo = @sCorreo AND U.kpTipoUsuario = 2";

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
                            Session["idDependencia"] = reader["idUsuario"].ToString() + '|' + reader["idDependenicaServicio"];
                            Session["tipoUsuario"] = reader["kpTipoUsuario"].ToString();
                            autorizado = reader["bAutorizado"].ToString();
                        }
                    }                    
                }
            }
            if (hashedPassword != null && hashedPassword != "")
            {
                if (autorizado != "11")
                {
                    lblError.Text = "La Dependencia no ha sido autorizada.";
                    result = false;
                }
                else
                {
                    lblError.Text = "";
                    string hassh = SeguridadUtils.Desencriptar(hashedPassword);
                    if (password != hassh)
                    {
                        lblError.Text = "Usuario y/o contraseña incorrecta.";
                    }
                    result = (password == hassh);                    
                }
            }
            else
            {
                lblError.Text = "El correo ingresado no se encuentra registrado.";
            }
            return result;
           
            
        }
    }

    #endregion

        #region Eventos Items 
    //protected void rbtnBuscar_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        if (rbtnBuscar.SelectedValue == "A") // Si la opción seleccionada es "Estudiantes Registrados"
    //        {
    //            pnlLogin.Visible = true; // Ocultar Panel1
    //            pnlRegistrarDependencia.Visible = false; // Ocultar pnlRegistrarDependencia
    //            //pnlEstudiantesRegistrados.Visible = false;
    //            PnlEjemploModPerfil.Visible = false;
    //            //pnlPDF.Visible = false;

    //        }
    //        if (rbtnBuscar.SelectedValue == "B") // Si la opción seleccionada es "Estudiantes Registrados"
    //        {
    //            pnlLogin.Visible = false; // Ocultar Panel1
    //            pnlRegistrarDependencia.Visible = true; // Ocultar pnlRegistrarDependencia
    //            //pnlEstudiantesRegistrados.Visible = false;
    //            PnlEjemploModPerfil.Visible = false;
    //            //pnlPDF.Visible = false;
    //        }

    //    }

    //protected void GridViewEstudiantes_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        // Cambiar el color de fondo de la fila de encabezados
    //        e.Row.BackColor = System.Drawing.Color.LightYellow;
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        // Obtener el índice de la fila actual
    //        int rowIndex = e.Row.RowIndex;

    //        // Encontrar los botones en la fila
    //        Button btnEvaluar = (Button)e.Row.FindControl("btnOperacion");
    //        Button btnLiberar = (Button)e.Row.FindControl("Button3");

    //        // Determinar qué botón mostrar basado en el índice de la fila
    //        if (rowIndex % 2 == 0) // Mostrar "Evaluar" en filas pares
    //        {
    //            btnEvaluar.Visible = true;
    //            btnLiberar.Visible = false;
    //        }
    //        else // Mostrar "Liberar Prestador" en filas impares
    //        {
    //            btnEvaluar.Visible = false;
    //            btnLiberar.Visible = true;
    //        }
    //    }
    //if (e.Row.RowType == DataControlRowType.DataRow)
    //{
    //    // Obtener los controles de etiqueta en la fila actual
    //    Label lblPrograma = (Label)e.Row.FindControl("lblPrograma");
    //    Label lblMatricula = (Label)e.Row.FindControl("lblMatricula");
    //    Label lblPrestador = (Label)e.Row.FindControl("lblPrestador");
    //    Label lblEstatus = (Label)e.Row.FindControl("lblEstatus");
    //    Label lblFechaEstatus = (Label)e.Row.FindControl("lblFechaEstatus");

    //    // Llenar los datos en las etiquetas
    //    lblPrograma.Text = "AUTOMATICACION DE ACTIVIDADES DOCENTES";
    //    lblMatricula.Text = "21343236";
    //    lblPrestador.Text = "ROCIO MEDINA HURTADO";
    //    lblEstatus.Text = "En Espera";
    //    lblFechaEstatus.Text = "13/01/2009";
    //}

    //if (e.Row.RowType == DataControlRowType.DataRow)
    //{
    //    // Crear el botón
    //    Button btnOperacion = new Button();
    //    btnOperacion.ID = "btnOperacion";
    //    btnOperacion.Text = "Operación";
    //    btnOperacion.CommandName = "Operacion";
    //    btnOperacion.CommandArgument = e.Row.RowIndex.ToString(); // Obtener el índice de la fila
    //    btnOperacion.CssClass = "btn btn-primary";

    //    // Asignar el evento al botón
    //    btnOperacion.Click += new EventHandler(btnOperacion_Click);

    //    // Agregar el botón a la celda
    //    e.Row.Cells[5].Controls.Add(btnOperacion); // Agregarlo a la última celda

    //    // Asegúrate de que el índice sea correcto según la estructura de tus columnas
    //    // Si la estructura cambia, ajusta el índice correspondientemente
    //}
    //}

    #endregion





}