using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Drawing;
using System.Numerics;
using static Servicio_Social.Programas1;
using System.Text;
using static Servicio_Social.EditarPrograma;


namespace Servicio_Social
{
    public partial class Programas1 : System.Web.UI.Page
    {

        string SQL = GlobalConstants.SQL;

        // Define una clave para el ViewState
        private const string NPE_VIEWSTATE_KEY = "NPE";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idDependencia"] == null)
                Response.Redirect("LoginDependencia.aspx");

            string idDependencia = Session["idDependencia"].ToString().Split('|')[1];
            string dependencia = ObtenerDependencia(idDependencia);
            txtDependencia.Text = dependencia;
            if (!IsPostBack)
            {
                // Este bloque de código se ejecuta solo en la primera carga de la página
                CargarPerido();
                CargarEnfoque();
                CargarModalidad();
                CargarUnidad();
                CargarNivel();
                CargarConfiguracion();
            }
        }
        private void CargarConfiguracion()
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real

            // Query para obtener la configuración específica para Registro de Dependencias
            string query = "SELECT bActivo, dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE sClave = '2'";

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
                            PANELPRINCIPAL.Visible = dentroDelRango;
                            pnlCerrado.Visible = !dentroDelRango;
                            // Asignar el mensaje al control h3 del frontend
                            lblMensajeProgramas.Text = mensaje;
                        }
                    }
                }
            }
        }
        private string ObtenerDependencia(string idDependencia)
        {
            string dependencia = "";
            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;

            // Define la consulta SQL para recuperar la descripción de la dependencia basada en el ID de usuario
            string query = "SELECT SDESCRIPCION FROM SM_DEPENDENCIA_SERVICIO AS DEP WHERE idDependenicaServicio=" + idDependencia;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agrega el parámetro del ID de usuario a la consulta
                    //command.Parameters.AddWithValue("@idUser", idUsuario);
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener la descripción de la dependencia de la consulta
                            dependencia = reader["SDESCRIPCION"].ToString();
                        }
                    }
                }
            }
            return dependencia;
        }
        // Define una clase para representar un registro
        
        
        [Serializable]
        public class NPE
        {
            public string Nivel { get; set; }
            public int kpNivel { get; set; }
            public string Plan { get; set; }
            public int kpPlan { get; set; }
            public string Escuela { get; set; }
            public int kpEscuela { get; set; }
            public string iCupo { get; set; }
        }
        protected void DDLUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {

            DDLNIVEL1.SelectedIndex = 0;
        }
        private bool ValidarFormulario()
        {
            // Verificar cada componente del formulario y las validaciones específicas
            if (string.IsNullOrEmpty(txtPrograma.Text) ||
                string.IsNullOrEmpty(txtResponsable.Text) ||
                string.IsNullOrEmpty(txtLugar.Text) ||
                string.IsNullOrEmpty(txtCargo.Text) ||
                string.IsNullOrEmpty(TxtDomicilio.Text) ||
                string.IsNullOrEmpty(txtHorario.Text) ||
                string.IsNullOrEmpty(txtAreaResp.Text) ||
                string.IsNullOrEmpty(txtObjetivos.Text) ||
                string.IsNullOrEmpty(txtActividades.Text) ||
                DDLUnidad.SelectedIndex == 0 ||
                DDLModalidad.SelectedIndex == 0 ||
                DDLEnfoque.SelectedIndex == 0 ||
                (int.Parse(DDLModalidad.SelectedValue) == 9 && string.IsNullOrEmpty(txtOtM.Text)) ||
                (int.Parse(DDLEnfoque.SelectedValue) == 13 && string.IsNullOrEmpty(txtOE.Text)))
            {
                return false; // Al menos un campo está vacío o las validaciones específicas no pasan
            }

            return true; // Todos los campos están llenos y las validaciones específicas pasan
        }
        protected void btnAnadir_Click(object sender, EventArgs e)
        {

            string idNivelSeleccionado = DDLNIVEL1.SelectedValue;
            // Verificar si el valor seleccionado en DDLNIVEL1 es 1
            if (idNivelSeleccionado == "1")
            {
                // Verificar si alguno de los campos está vacío
                if (string.IsNullOrEmpty(DDLNIVEL1.SelectedValue) || string.IsNullOrEmpty(DDLESC1.SelectedValue) || string.IsNullOrEmpty(tCupo.Text))
                {
                    // Mostrar mensaje de alerta con JavaScript
                    mensajeScriptError("Es necesario seleccionar Nivel,  Escuela y Cupo.");
                }
                else 
                {
                    List<NPE> npe = ViewState[NPE_VIEWSTATE_KEY] as List<NPE> ?? new List<NPE>();
                    // Verificar si ya existe un registro con los mismos valores
                    bool existeRegistro = npe.Any(item =>
                    item.kpNivel == Convert.ToInt32(DDLNIVEL1.SelectedValue) &&
                    item.kpEscuela == Convert.ToInt32(DDLESC1.SelectedValue));

                    if (existeRegistro)
                    {
                        // Mostrar mensaje indicando que ya existe un registro con esos datos
                        mensajeScriptError("Ya se ha añadido un registro de escuela con estos datos.");
                    }
                    else
                    {
                        // Crea un nuevo registro y asigna los valores desde los controles del formulario
                        NPE nuevoRegistro = new NPE();
                        nuevoRegistro.Nivel = DDLNIVEL1.SelectedItem.Text;
                        nuevoRegistro.kpNivel = Convert.ToInt32(DDLNIVEL1.SelectedValue);
                        nuevoRegistro.Escuela = DDLESC1.SelectedItem.Text;
                        nuevoRegistro.kpEscuela = Convert.ToInt32(DDLESC1.SelectedValue);
                        nuevoRegistro.iCupo = tCupo.Text;


                        // Agrega el nuevo registro a la lista
                        npe.Add(nuevoRegistro);

                        // Guarda la lista de registros en el ViewState
                        ViewState[NPE_VIEWSTATE_KEY] = npe;
                        // Asigna la lista de registros como origen de datos del GridView
                        GridView2.DataSource = npe;
                        GridView2.DataBind();

                        DDLNIVEL1.SelectedIndex = -1;
                        DDLESC1.SelectedIndex = -1;
                        DDLPLAN1.SelectedIndex = -1;
                        tCupo.Text = "";
                    }
                }

            }
            else
            { 
                 // Verificar si alguno de los campos está vacío
                 if (string.IsNullOrEmpty(DDLNIVEL1.SelectedValue) || string.IsNullOrEmpty(DDLPLAN1.SelectedValue) || string.IsNullOrEmpty(DDLESC1.SelectedValue) || string.IsNullOrEmpty(tCupo.Text))
                  {
                     // Mostrar mensaje de alerta con JavaScript
                      mensajeScriptError("Es necesario seleccionar Nivel, Plan de Estudios, Escuela y Cupo.");
                  }
                 else
                    {
                         List<NPE> npe = ViewState[NPE_VIEWSTATE_KEY] as List<NPE> ?? new List<NPE>();
                         // Verificar si ya existe un registro con los mismos valores
                        bool existeRegistro = npe.Any(item =>
                        item.kpNivel == Convert.ToInt32(DDLNIVEL1.SelectedValue) &&
                        item.kpPlan == Convert.ToInt32(DDLPLAN1.SelectedValue) &&
                        item.kpEscuela == Convert.ToInt32(DDLESC1.SelectedValue));

                        if (existeRegistro)
                        {
                            // Mostrar mensaje indicando que ya existe un registro con esos datos
                            mensajeScriptError("Ya se ha añadido un registro de escuela con estos datos.");
                        }
                        else
                        {
                            // Crea un nuevo registro y asigna los valores desde los controles del formulario
                            NPE nuevoRegistro = new NPE();
                            nuevoRegistro.Nivel = DDLNIVEL1.SelectedItem.Text;
                            nuevoRegistro.kpNivel = Convert.ToInt32(DDLNIVEL1.SelectedValue);
                            nuevoRegistro.Plan = DDLPLAN1.SelectedItem.Text;
                            nuevoRegistro.kpPlan = Convert.ToInt32(DDLPLAN1.SelectedValue);
                            nuevoRegistro.Escuela = DDLESC1.SelectedItem.Text;
                            nuevoRegistro.kpEscuela = Convert.ToInt32(DDLESC1.SelectedValue);
                            nuevoRegistro.iCupo = tCupo.Text;


                            // Agrega el nuevo registro a la lista
                            npe.Add(nuevoRegistro);

                            // Guarda la lista de registros en el ViewState
                            ViewState[NPE_VIEWSTATE_KEY] = npe;
                            // Asigna la lista de registros como origen de datos del GridView
                            GridView2.DataSource = npe;
                            GridView2.DataBind();

                            DDLNIVEL1.SelectedIndex = -1;
                            DDLESC1.SelectedIndex = -1;
                            DDLPLAN1.SelectedIndex = -1;
                            tCupo.Text = "";
                         }
                  }
             }
        }
        private void MostrarRegistros2()
        {
            // Recupera la lista de registros desde el ViewState
            List<NPE> npe = ViewState[NPE_VIEWSTATE_KEY] as List<NPE> ?? new List<NPE>();

            // Asigna la lista de registros como origen de datos del GridView
            GridView2.DataSource = npe;
            GridView2.DataBind();
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistro")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Obtener la lista de registros desde el ViewState
                List<NPE> npe = ViewState[NPE_VIEWSTATE_KEY] as List<NPE>;

                // Eliminar el registro seleccionado
                npe.RemoveAt(index);

                // Actualizar el ViewState con la lista modificada
                ViewState[NPE_VIEWSTATE_KEY] = npe;

                // Volver a mostrar los registros en el GridView
                MostrarRegistros2();
            }
        }
       
        protected void OTRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLModalidad.SelectedValue == "9")
            {
                pnlOModalidad.Visible = true;
            }
            else
            {
                pnlOModalidad.Visible = false;
            }
        }
        protected void OTRO_SelectedIndexChanged2(object sender, EventArgs e)
        {
            if (DDLEnfoque.SelectedValue == "13")
            {
                pnlOEnfoque.Visible = true;
            }
            else
            {
                pnlOEnfoque.Visible = false;
            }
        }
        protected void btnGuardad_Click(object sender, EventArgs e)
        {
            if (ValidarFormulario())
            {
                if (GridView2.Rows.Count > 0)
                {
                    string nombrePrograma = txtPrograma.Text;
                    string domicilioServicio = TxtDomicilio.Text;
                    string objetivos = txtObjetivos.Text;
                    string actividades = txtActividades.Text;
                    string linkGoogleMaps = txtLinkGoogle.Text;
                    string areaResponsable = txtAreaResp.Text;
                    int periodo = int.Parse(DDLPeriodo.SelectedValue);
                    string horario = txtHorario.Text;
                    string LugarAdscripcion = txtLugar.Text;
                    string Responsable = txtResponsable.Text;
                    string Cargo = txtCargo.Text;
                    int Unidad = int.Parse(DDLUnidad.SelectedValue);
                    int enfoque = int.Parse(DDLEnfoque.SelectedValue);
                    int modalidad = int.Parse(DDLModalidad.SelectedValue);
                    string connectionString = GlobalConstants.SQL;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                     {
                         connection.Open();
                         string idDependencia = Session["idDependencia"].ToString().Split('|')[1];
                         int idDependenciaEntero = int.Parse(idDependencia);
                         string insertProgramaQuery = @"
                          INSERT INTO SM_PROGRAMA 
                          (sNombre_Programa, sDomicilio_Serv,
                          sObjetivos, sActividades_desarollo,
                          slinkMaps, sAreaResponsable, kpModalidad, kpEnfoque, kpPeriodo, sHorario, sLugarAdscripcion, kpEstatus_Programa, sOtraModalidad, sOtroEnfoque, dFechaRegistroP, kpDependencia,sResponsable,sCargoResp,kpUnidad)
                          VALUES 
                          (@sNombre_Programa, @sDomicilio_Serv,
                          @sObjetivos, @sActividades_desarollo,
                          @slinkMaps, @sAreaResponsable, @kpModalidad, @kpEnfoque, @kpPeriodo, @sHorario, @sLugarAdscripcion, @kpEstatus, @sOtraModalidad, @sOtroEnfoque,  GETDATE(), @idDependenciaEntero ,@sResponsable, @sCargoResp, @kpUnidad);
                          SELECT SCOPE_IDENTITY();";
                           using (SqlCommand cmd = new SqlCommand(insertProgramaQuery, connection))
                            {
                                    cmd.Parameters.AddWithValue("@sNombre_Programa", nombrePrograma);
                                    cmd.Parameters.AddWithValue("@sDomicilio_Serv", domicilioServicio);
                                    cmd.Parameters.AddWithValue("@sObjetivos", objetivos);
                                    cmd.Parameters.AddWithValue("@sActividades_desarollo", actividades);
                                    cmd.Parameters.AddWithValue("@slinkMaps", linkGoogleMaps);
                                    cmd.Parameters.AddWithValue("@sAreaResponsable", areaResponsable);
                                    cmd.Parameters.AddWithValue("@kpModalidad", modalidad);
                                    cmd.Parameters.AddWithValue("@kpEnfoque", enfoque);
                                    cmd.Parameters.AddWithValue("@kpPeriodo", periodo);
                                    cmd.Parameters.AddWithValue("@sHorario", horario);
                                    cmd.Parameters.AddWithValue("@sLugarAdscripcion", LugarAdscripcion);
                                    cmd.Parameters.AddWithValue("@kpEstatus", 20707);
                                    cmd.Parameters.AddWithValue("@sOtraModalidad", (modalidad == 9 && !string.IsNullOrEmpty(txtOtM.Text)) ? txtOtM.Text : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@sOtroEnfoque", (enfoque == 13 && !string.IsNullOrEmpty(txtOE.Text)) ? txtOE.Text : (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@idDependenciaEntero", idDependenciaEntero);
                                    cmd.Parameters.AddWithValue("@sResponsable", Responsable);
                                    cmd.Parameters.AddWithValue("@sCargoResp", Cargo);
                                    cmd.Parameters.AddWithValue("@kpUnidad", Unidad);

                                    int idPrograma = Convert.ToInt32(cmd.ExecuteScalar());

                                    if (idPrograma > 0)
                                    {
                                        string insertDetalleQuery = "";
                                        
                                        insertDetalleQuery = @"INSERT INTO SM_DETALLE_PROGRAMA 
                                                    (kpNivel, kpPlanEstudio, kpEscuela, iCupo,kmPrograma)
                                                    VALUES 
                                                    (@idNivel, @idPlan, @idEscuela, @iCupo, @kmPrograma)";
                                        SqlCommand detalleCommand = new SqlCommand(insertDetalleQuery, connection);

                                        foreach (GridViewRow detalleRow in GridView2.Rows)
                                        {
                                            detalleCommand.Parameters.Clear();
                                            // Obtener el valor de idNivel
                                            string idNivel = detalleRow.Cells[1].Text;
                                            int idPlan;
                                            // Verificar el valor de idNivel y ajustar idPlan en consecuencia
                                            if (idNivel == "1")
                                            {
                                                int escuela = Convert.ToInt32(detalleRow.Cells[5].Text);
                                                if (escuela == 244 || escuela == 186 || escuela == 151)
                                                {
                                                    idPlan = 605;
                                                }
                                                else
                                                {
                                                    idPlan = 1171698;
                                                }
                                            }
                                            else if (idNivel == "2")
                                            {
                                                idPlan = Convert.ToInt32(detalleRow.Cells[3].Text);
                                            }
                                            else
                                            {
                                                // Manejo de caso en que idNivel no sea ni 1 ni 2
                                                idPlan = Convert.ToInt32(detalleRow.Cells[3].Text);
                                            }

                                            detalleCommand.Parameters.AddWithValue("@idNivel", idNivel);
                                            detalleCommand.Parameters.AddWithValue("@idPlan", idPlan);
                                            detalleCommand.Parameters.AddWithValue("@idEscuela", Convert.ToInt32(detalleRow.Cells[5].Text));
                                            detalleCommand.Parameters.AddWithValue("@iCupo", Convert.ToInt32(detalleRow.Cells[6].Text));
                                            detalleCommand.Parameters.AddWithValue("@kmPrograma", idPrograma);
                                            detalleCommand.ExecuteNonQuery();
                                        }
                                    }
                                    PANELPRINCIPAL.Visible = false;
                                    pnlProgramaExitoso.Visible = true;
                            }
                    }
                }
                else
                {
                    mensajeScriptError("Es necesario añadir al menos una escuela.");
                }
            }
            else
            {
                mensajeScriptError("Por favor llene todos los campos del formulario.");
            }
        }
        private void CargarNivel()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
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
                DDLNIVEL1.DataSource = ds3;
                DDLNIVEL1.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                DDLNIVEL1.DataValueField = "idTipoNivel";
                DDLNIVEL1.DataBind();
            }

        }
        protected void Unidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLNIVEL1.SelectedIndex = -1;
            DDLPLAN1.SelectedIndex = -1;
            //DDLNIVEL1.Items.Clear();
            //DDLESC1.Items.Clear();
            //DDLPLAN1.Items.Clear();
        }
        protected void DDLPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idNivelSeleccionado = DDLNIVEL1.SelectedValue;
            DDLESC1.Items.Clear();
            DDLPLAN1.Items.Clear();
            // Verificar si el valor seleccionado en DDLNIVEL1 es 1
            if (idNivelSeleccionado == "1")
            {
                PNLPLAN.Visible = false;
                
                string idPlanSeleccionado = DDLPLAN1.SelectedValue;
                string idUnidadSeleccionado = DDLUnidad.SelectedValue;
                string queryString = "";
                switch (idUnidadSeleccionado)
                {
                    case "2":
                        queryString = "SELECT * FROM (SELECT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC FROM SM_PLAN_EST_ESCUELA AS PLA " +
                                    "JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC " +
                                    "WHERE PLA.kpPlan_Estudio IN ( 1171698,1177965) AND ESC.kpUnidad = @idUnidad " +
                                    "UNION ALL " +
                                    " SELECT sClave + ' - ' + sDescripcion AS Descripcion, idEscuelaUAC " +
                                    "FROM SP_ESCUELA_UAC " +
                                    "WHERE idEscuelaUAC IN (244) ) AS X  ORDER BY 1;";
                        break;
                    default:
                        queryString = "SELECT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC FROM SM_PLAN_EST_ESCUELA AS PLA " +
                                    "JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC " +
                                    "WHERE PLA.kpPlan_Estudio IN ( 1171698,1177965) AND ESC.kpUnidad = @idUnidad  ORDER BY 1";
                        break;


                }


                using (SqlConnection con = new SqlConnection(SQL))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(queryString, con);
                    cmd.Parameters.AddWithValue("@PLAN", idPlanSeleccionado);
                    cmd.Parameters.AddWithValue("idUnidad", idUnidadSeleccionado);
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
                    DDLESC1.DataSource = dt;
                    DDLESC1.DataTextField = "Descripcion";
                    DDLESC1.DataValueField = "kpEscuela_UAdeC";
                    DDLESC1.DataBind();
                }

            }
            else
            {
                PNLPLAN.Visible = true;
                // Filtrar la consulta del segundo DropDownList utilizando el valor seleccionado en DDLEscuela
                string queryString = "SELECT sClave + ' - ' + sDescripcion [Descripcion], idPlanEstudio FROM SP_PLAN_ESTUDIO  WHERE bActivo = 1 AND bVigente = 1 AND kpNivel = @NIVEL " +
                                     "AND sClave IS NOT NULL AND LEN(sClave) = 3  ORDER BY sClave";

                DataSet ds = new DataSet();

                using (SqlConnection con = new SqlConnection(SQL))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(queryString, con);
                    cmd.Parameters.AddWithValue("@NIVEL", idNivelSeleccionado);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Crear un nuevo DataTable que incluya el primer elemento "Seleccione el Plan..."
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Descripcion", typeof(string));
                    dt.Columns.Add("idPlanEstudio", typeof(string));
                    dt.Rows.Add("Seleccione el Plan...", ""); // Agregar el primer elemento

                    // Llenar el DataTable con los resultados de la consulta
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["Descripcion"], reader["idPlanEstudio"]);
                    }

                    // Llena DDLPlan con los datos del DataTable
                    DDLPLAN1.DataSource = dt;
                    DDLPLAN1.DataTextField = "Descripcion";
                    DDLPLAN1.DataValueField = "idPlanEstudio";
                    DDLPLAN1.DataBind();
                }
            }

        }

        protected void DDLEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLESC1.Items.Clear();
            string idPlanSeleccionado = DDLPLAN1.SelectedValue;
            string idUnidadSeleccionado = DDLUnidad.SelectedValue;
            string idNivelSeleccionado = DDLNIVEL1.SelectedValue;
            string queryString = "";
            // Verificar si el valor seleccionado en DDLNIVEL1 es 1
            if (idNivelSeleccionado == "2")
            {
                // Filtrar la consulta del segundo DropDownList utilizando el valor seleccionado en DDLEscuela
                queryString = "SELECT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC FROM SM_PLAN_EST_ESCUELA AS PLA " +
                                "JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC " +
                                "WHERE PLA.kpPlan_Estudio = @PLAN AND ESC.kpUnidad = @idUnidad;";
            }
            else
            {
                // Filtrar la consulta del segundo DropDownList utilizando el valor seleccionado en DDLEscuela
                queryString = "SELECT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC FROM SM_PLAN_EST_ESCUELA AS PLA " +
                                "JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC " +
                                "WHERE PLA.kpPlan_Estudio IN ( 1171698,1177965)  AND ESC.kpUnidad = @idUnidad;";
            }

            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(queryString, con);
                cmd.Parameters.AddWithValue("@PLAN", idPlanSeleccionado);
                cmd.Parameters.AddWithValue("idUnidad", idUnidadSeleccionado);
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
                DDLESC1.DataSource = dt;
                DDLESC1.DataTextField = "Descripcion";
                DDLESC1.DataValueField = "kpEscuela_UAdeC";
                DDLESC1.DataBind();
            }

        }

        private void CargarPerido()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = "SELECT sDescripcion, idCiclo FROM SP_CICLO  WHERE bServicioSocial= 1";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds99 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds99);
                }

                // Asigna los resultados al DropDownList
                DDLPeriodo.DataSource = ds99;
                DDLPeriodo.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                DDLPeriodo.DataValueField = "idCiclo";
                DDLPeriodo.DataBind();
            }

        }
        private void CargarEnfoque()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = "SELECT sDescripcion,idenfoque FROM SP_ENFOQUE";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds4 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds4);
                }
                DataTable dt = ds4.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione el Enfoque...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                DDLEnfoque.DataSource = ds4;
                DDLEnfoque.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                DDLEnfoque.DataValueField = "idenfoque";
                DDLEnfoque.DataBind();
            }

        }
        private void CargarModalidad()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = "SELECT sDescripcion, idTipoServicio FROM SP_TIPO_SERVICIO";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds5 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds5);
                }

                DataTable dt = ds5.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione la Modalidad...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                DDLModalidad.DataSource = ds5;
                DDLModalidad.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                DDLModalidad.DataValueField = "idTipoServicio";
                DDLModalidad.DataBind();
            }

        }
        private void CargarUnidad()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = "SELECT sCiudad,idUnidad FROM NP_UNIDAD WHERE IDUNIDAD != 1";

                // Crea un DataSet para almacenar los resultados de la consulta

                DataSet ds6 = new DataSet();


                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds6);
                }
                // Agregar manualmente el primer elemento "Seleccione la unidad"
                DataTable dt = ds6.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sCiudad"] = "Seleccione la unidad...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                DDLUnidad.DataSource = ds6;
                DDLUnidad.DataTextField = "sCiudad"; // Utiliza el alias "Descripcion" como texto visible
                DDLUnidad.DataValueField = "idUnidad";
                DDLUnidad.DataBind();
            }

        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[5].Visible = false;

                    string idNivelSeleccionado = DDLNIVEL1.SelectedValue;
                    if (idNivelSeleccionado == "1")
                    {
                        e.Row.Cells[2].Visible = false;
                    }

                }
               if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Ocultar las celdas en los datos
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    string idNivelSeleccionado = DDLNIVEL1.SelectedValue;
                    if (idNivelSeleccionado == "1")
                    {
                        e.Row.Cells[2].Visible = false;
                    }

                }
            }
            catch { }
        }

        public void mensajeScriptError(string mensaje)
        {
            string scriptText = "$('.alert').remove(); $('body').prepend('<div class=\"alert alert-danger alert-dismissible fade show\" role=\"alert\" style=\"background-color: #f8d7da; color: #721c24;\"><strong>" + mensaje + "</strong><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button></div>'); setTimeout(function() { $('.alert').alert('close'); }, 6000);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }


    }
}