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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Servicio_Social.ProgramasRegistrados;
using iText.Layout.Element;
using OracleInternal.Secure.Network;
using WebGrease.Activities;
using static Servicio_Social.ProgramasDependencias;

namespace Servicio_Social
{
    public partial class EditarPrograma : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        private const string NPE_VIEWSTATE_KEY = "NPE";

            protected void Page_Load(object sender, EventArgs e)
            {
                string idPrograma = Request.QueryString["idPrograma"];
                if (!IsPostBack)
                {
                    if (Request.QueryString["idPrograma"] != null)
                    {

                        // Usa el valor de idPrograma como necesites
                        Editar(idPrograma);
                    }
                    // Este bloque de código se ejecuta solo en la primera carga de la página
                    CargarPerido();
                    CargarEnfoque();
                    CargarModalidad();
                    CargarUnidad();
                    CargarNivel();
               

                    //ViewState[NPE_VIEWSTATE_KEY] = new List<NPE>();
                }

            }
        [Serializable]
        public class NPE
        {
            public string Nivel { get; set; }
            public int kpNivel { get; set; }
            public string PlanE { get; set; }
            public int kpPlanEstudio { get; set; }
            public string Escuela { get; set; }
            public int kpEscuela { get; set; }
            public string iCupo { get; set; }


        }
        
        public void Editar(string idPrograma)
        {

            string NombreP = " ";
            string Dependencia = " ";
            string Domicilio = "";
            string Objetivos = "";
            string Actividades = "";
            string Horario = "";
            string Link = "";
            string Area = "";
            string Lugar = "";
            string Cargo = "";
            string Responsable = "";
            int Modalidad = 0;
            int Enfoque = 0;
            int Periodo = 0;
            int Unidad = 0;

            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;

            // Define la consulta SQL para recuperar la descripción de la dependencia basada en el ID de usuario
            string query = "SELECT P.sNombre_Programa, DEP.sDescripcion ,P.sDomicilio_Serv, P.sObjetivos,P.sActividades_desarollo," +
                            "P.sHorario, P.slinkMaps, P.kpModalidad,P.kpEnfoque, P.kpPeriodo, P.sAreaResponsable, P.sLugarAdscripcion, P.sOtraModalidad, P.sOtroEnfoque," +
                            " P.sResponsable, P.sCargoResp, P.kpUnidad" +
                            " FROM SM_PROGRAMA AS P JOIN SM_DEPENDENCIA_SERVICIO AS DEP ON P.kpDependencia = dep.idDependenicaServicio" +
                            " WHERE idPrograma=" + idPrograma;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abre la conexión a la base de datos
                connection.Open();

                // Crea un comando SQL con la consulta y los parámetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutar consulta y leer datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener la descripción de la dependencia de la consulta
                            NombreP = reader["sNombre_Programa"].ToString();
                            Dependencia = reader["sDescripcion"].ToString();
                            Domicilio = reader["sDomicilio_Serv"].ToString();
                            Objetivos = reader["sObjetivos"].ToString();
                            Actividades = reader["sActividades_desarollo"].ToString();
                            Horario = reader["sHorario"].ToString();
                            Link = reader["slinkMaps"].ToString();
                            Area = reader["sAreaResponsable"].ToString();
                            Lugar = reader["sLugarAdscripcion"].ToString();
                            Modalidad = Convert.ToInt32(reader["kpModalidad"]);
                            Enfoque = Convert.ToInt32(reader["kpEnfoque"]);
                            Periodo = Convert.ToInt32(reader["kpPeriodo"]);
                            Responsable = reader["sResponsable"].ToString();
                            Cargo = reader["sCargoResp"].ToString();
                            Unidad = Convert.ToInt32(reader["kpUnidad"]);

                        }
                    }
                    txtPrograma.Text = NombreP;
                    txtDependencia.Text = Dependencia;
                    TxtDomicilio.Text = Domicilio;
                    txtObjetivos.Text = Objetivos;
                    txtActividades.Text = Actividades;
                    txtHorario.Text = Horario;
                    txtLinkGoogle.Text = Link;
                    txtAreaResp.Text = Area;
                    txtLugar.Text = Lugar;
                    DDLModalidad.SelectedValue = Modalidad.ToString();
                    DDLEnfoque.SelectedValue = Enfoque.ToString();
                    DDLPeriodo.SelectedValue = Periodo.ToString();
                    txtResponsable.Text = Responsable;
                    txtCargo.Text = Cargo;
                    DDLUnidad.SelectedValue = Unidad.ToString();

                }

                // Dentro de tu método Editar(string idPrograma)
                string queryDetallePrograma = "SELECT DP.idDetallePrograma, N.sDescripcion AS Nivel, DP.kpNivel,PE.sClave + ' - ' + PE.sDescripcion AS PlanE, DP.kpPlanEstudio, " +
                                              "ES.sClave +' - ' + ES.sDescripcion Escuela,  DP.kpEscuela,iCupo " +
                                              "FROM SM_DETALLE_PROGRAMA AS DP " +
                                              "JOIN SP_TIPO_NIVEL AS N ON DP.kpNivel = N.idTipoNivel " +
                                              "JOIN SP_PLAN_ESTUDIO AS PE ON DP.kpPlanEstudio = PE.idPlanEstudio " +
                                              "JOIN SP_ESCUELA_UAC AS ES ON DP.kpEscuela = ES.idEscuelaUAC WHERE DP.kmPrograma =" + idPrograma;

                using (SqlCommand commandDetallePrograma = new SqlCommand(queryDetallePrograma, connection))
                {
                    commandDetallePrograma.Parameters.AddWithValue("@idPrograma", idPrograma);

                    using (SqlDataReader readerDetallePrograma = commandDetallePrograma.ExecuteReader())
                    {
                        // Crear DataTable para almacenar los datos del SqlDataReader
                        DataTable dtDetallePrograma = new DataTable();
                        dtDetallePrograma.Load(readerDetallePrograma);

                        // Guardar la DataTable en ViewState
                        ViewState[NPE_VIEWSTATE_KEY] = dtDetallePrograma;

                        // Asignar el DataTable como fuente de datos para el GridView
                        GridView2.DataSource = dtDetallePrograma;
                        GridView2.DataBind();
                    }
                }
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
        private void CargarPerido()
        {
            // Define la cadena de conexión
            using (SqlConnection con = new SqlConnection(SQL))
            {
                try
                {
                    // Abre la conexión
                    con.Open();

                    // Define la consulta SQL
                    string query = "SELECT sDescripcion, idCiclo FROM SP_CICLO WHERE bServicioSocial = 1";

                    // Utiliza un SqlDataAdapter para llenar un DataTable
                    DataTable dtPeriodos = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                    {
                        adapter.Fill(dtPeriodos);
                    }

                    // Verifica si hay datos antes de asignarlos al DropDownList
                    if (dtPeriodos.Rows.Count > 0)
                    {
                        DDLPeriodo.DataSource = dtPeriodos;
                        DDLPeriodo.DataTextField = "sDescripcion"; // Lo que se mostrará en la lista
                        DDLPeriodo.DataValueField = "idCiclo";     // El valor subyacente (idCiclo)
                        DDLPeriodo.DataBind();

                        // Selecciona el primer elemento por defecto
                        DDLPeriodo.SelectedIndex = 0;
                    }
                    else
                    {
                        // Si no hay datos, limpia el DropDownList y muestra un mensaje predeterminado
                        DDLPeriodo.Items.Clear();
                        //DDLPeriodo.Items.Add(new ListItem("No hay períodos disponibles", ""));
                    }
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción
                    System.Diagnostics.Debug.WriteLine($"Error en CargarPeriodo: {ex.Message}");

                    // Opcional: muestra un mensaje en el DropDownList
                    DDLPeriodo.Items.Clear();
                    //DDLPeriodo.Items.Add(new ListItem("Error al cargar los períodos", ""));
                }
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
        protected void DDLPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idNivelSeleccionado = DDLNIVEL1.SelectedValue;

            // Filtrar la consulta del segundo DropDownList utilizando el valor seleccionado en DDLEscuela
            string queryString = "SELECT sClave + ' - ' + sDescripcion [Descripcion], idPlanEstudio FROM SP_PLAN_ESTUDIO  WHERE bActivo = 1 AND bVigente = 1 AND kpNivel = @NIVEL " +
                                 "AND sClave IS NOT NULL ORDER BY sClave";

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

        protected void DDLEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idPlanSeleccionado = DDLPLAN1.SelectedValue;
            string idUnidadSeleccionado = DDLUnidad.SelectedValue;

            // Filtrar la consulta del segundo DropDownList utilizando el valor seleccionado en DDLEscuela
            string queryString = "SELECT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC FROM SM_PLAN_EST_ESCUELA AS PLA " +
                                 "JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC " +
                                 "WHERE PLA.kpPlan_Estudio = @PLAN AND ESC.kpUnidad = @idUnidad";
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
        protected void DDLUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {

            DDLNIVEL1.SelectedIndex = 0;
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

            // Verificar si alguno de los campos está vacío
            if (string.IsNullOrEmpty(DDLNIVEL1.SelectedValue) || string.IsNullOrEmpty(DDLPLAN1.SelectedValue) || string.IsNullOrEmpty(DDLESC1.SelectedValue) || string.IsNullOrEmpty(tCupo.Text))
            {
                // Mostrar mensaje de alerta con JavaScript
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Es necesario seleccionar Nivel, Plan de Estudios, Escuela y Cupo.');", true);
            }
            else
            {
                // Obtener la DataTable actual desde el ViewState
                DataTable dtDetallePrograma = ViewState[NPE_VIEWSTATE_KEY] as DataTable;

                if (dtDetallePrograma == null)
                {
                    dtDetallePrograma = new DataTable();
                    dtDetallePrograma.Columns.Add("Nivel");
                    dtDetallePrograma.Columns.Add("kpNivel");
                    dtDetallePrograma.Columns.Add("PlanE");
                    dtDetallePrograma.Columns.Add("kpPlanEstudio");
                    dtDetallePrograma.Columns.Add("Escuela");
                    dtDetallePrograma.Columns.Add("kpEscuela");
                    dtDetallePrograma.Columns.Add("iCupo");
                }

                // Crear una nueva fila y asignar los valores desde los controles del formulario
                DataRow newRow = dtDetallePrograma.NewRow();
                newRow["Nivel"] = DDLNIVEL1.SelectedItem.Text;
                newRow["kpNivel"] = int.Parse(DDLNIVEL1.SelectedValue);
                newRow["PlanE"] = DDLPLAN1.SelectedItem.Text;
                newRow["kpPlanEstudio"] = int.Parse(DDLPLAN1.SelectedValue);
                newRow["Escuela"] = DDLESC1.SelectedItem.Text;
                newRow["kpEscuela"] = int.Parse(DDLESC1.SelectedValue);
                newRow["iCupo"] = tCupo.Text;

                // Verificar si la fila ya existe en la DataTable
                bool exists = false;
                foreach (DataRow row in dtDetallePrograma.Rows)
                {
                    if (row["kpNivel"].ToString() == newRow["kpNivel"].ToString() &&
                        row["kpPlanEstudio"].ToString() == newRow["kpPlanEstudio"].ToString() &&
                        row["kpEscuela"].ToString() == newRow["kpEscuela"].ToString())
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Los datos seleccionas ya se encuentran registrados.');", true);
                }
                else
                {
                    // Agregar la nueva fila a la DataTable
                    dtDetallePrograma.Rows.Add(newRow);

                    // Guardar la DataTable actualizada en ViewState
                    ViewState[NPE_VIEWSTATE_KEY] = dtDetallePrograma;

                    // Asignar la DataTable actualizada como fuente de datos para el GridView
                    GridView2.DataSource = dtDetallePrograma;
                    GridView2.DataBind();

                    string connectionString = GlobalConstants.SQL;
                    string query = "INSERT INTO SM_DETALLE_PROGRAMA  (kpNivel, kpPlanEstudio, kpEscuela, iCupo,kmPrograma) VALUES (@kpNivel, @kpPlanEstudio, @kpEscuela, @iCupo, @kmPrograma)";
                    string idPrograma = Request.QueryString["idPrograma"];

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Abre la conexión
                        connection.Open();

                        // Crea un comando SQL con la consulta y los parámetros
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agrega los parámetros
                            command.Parameters.AddWithValue("@kpNivel", newRow["kpNivel"]);
                            command.Parameters.AddWithValue("@kpPlanEstudio", newRow["kpPlanEstudio"]);
                            command.Parameters.AddWithValue("@kpEscuela", newRow["kpEscuela"]);
                            command.Parameters.AddWithValue("@iCupo", newRow["iCupo"]);
                            command.Parameters.AddWithValue("@kmPrograma", idPrograma);

                            // Ejecuta el comando
                            command.ExecuteNonQuery();
                        }
                    }

                    // Limpiar los controles del formulario
                    DDLNIVEL1.SelectedIndex = -1;
                    DDLESC1.SelectedIndex = -1;
                    DDLPLAN1.SelectedIndex = -1;
                    tCupo.Text = string.Empty;

                    // Ocultar mensaje de error
                    //lblError.Visible = false;
                }
            }

        }
        protected void btnGuardad_Click(object sender, EventArgs e)
        {
            string idPrograma = Request.QueryString["idPrograma"];
            int ID = Convert.ToInt32(idPrograma);
            string NombreP = txtPrograma.Text;
            string Unidad = DDLUnidad.SelectedValue;
            string Responsable = txtResponsable.Text;
            string Cargo = txtCargo.Text;
            string Horario = txtHorario.Text;
            string Lugar = txtLugar.Text;
            string Modalidad = DDLModalidad.SelectedValue;
            string Enfoque = DDLEnfoque.SelectedValue;
            string Area = txtAreaResp.Text;
            string Objetivos = txtObjetivos.Text;
            string Actividades = txtActividades.Text;
            string Domicilio = TxtDomicilio.Text;
            string Linkk = txtLinkGoogle.Text;

            UpdateDataProgramas(ID, NombreP, Unidad, Responsable, Cargo, Horario, Lugar, Modalidad, Enfoque, Area, Objetivos, Actividades, Domicilio, Linkk);
        }

        private void UpdateDataProgramas(int ID, string NombreP, string Unidad, string Responsable, string Cargo, string Horario, string Lugar, string Modalidad, string Enfoque, string Area, string Objetivos, string Actividades, string Domicilio, string Linkk)
        {
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Actualización del programa en la tabla SM_PROGRAMA
                    using (SqlCommand cmd1 = new SqlCommand("UPDATE SM_PROGRAMA SET sNombre_Programa = @sNombre_Programa, kpUnidad = @kpUnidad, sResponsable = @sResponsable, sCargoResp = @sCargoResp, sHorario = @sHorario, sLugarAdscripcion = @sLugarAdscripcion, kpModalidad = @kpModalidad, kpEnfoque = @kpEnfoque, sObjetivos = @sObjetivos, sActividades_desarollo = @sActividades_desarollo, sDomicilio_Serv = @sDomicilio_Serv, slinkMaps = @slinkMaps WHERE idPrograma = @ID", connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@sNombre_Programa", NombreP);
                        cmd1.Parameters.AddWithValue("@kpUnidad", Unidad);
                        cmd1.Parameters.AddWithValue("@sResponsable", Responsable);
                        cmd1.Parameters.AddWithValue("@sCargoResp", Cargo);
                        cmd1.Parameters.AddWithValue("@sHorario", Horario);
                        cmd1.Parameters.AddWithValue("@sLugarAdscripcion", Lugar);
                        cmd1.Parameters.AddWithValue("@kpModalidad", Modalidad);
                        cmd1.Parameters.AddWithValue("@kpEnfoque", Enfoque);
                        cmd1.Parameters.AddWithValue("@sObjetivos", Objetivos);
                        cmd1.Parameters.AddWithValue("@sActividades_desarollo", Actividades);
                        cmd1.Parameters.AddWithValue("@sDomicilio_Serv", Domicilio);
                        cmd1.Parameters.AddWithValue("@slinkMaps", Linkk);
                        cmd1.Parameters.AddWithValue("@ID", ID);
                        cmd1.ExecuteNonQuery();
                    }

                    // Insertar detalles en SM_DETALLE_PROGRAMA, pero solo si no existen ya
                    foreach (GridViewRow detalleRow in GridView2.Rows)
                    {
                        string idNivel = detalleRow.Cells[2].Text;
                        int idPlan;
                        int idEscuela = Convert.ToInt32(detalleRow.Cells[6].Text);

                        if (idNivel == "1")
                        {
                            idPlan = (idEscuela == 244 || idEscuela == 186 || idEscuela == 151) ? 605 : 1171698;
                        }
                        else
                        {
                            idPlan = Convert.ToInt32(detalleRow.Cells[4].Text);
                        }

                        // Comprobar si el registro ya existe en la base de datos
                        string checkQuery = @"SELECT COUNT(*) FROM SM_DETALLE_PROGRAMA 
                                      WHERE kpNivel = @idNivel AND kpPlanEstudio = @idPlan 
                                      AND kpEscuela = @idEscuela AND kmPrograma = @kmPrograma";

                        SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction);
                        checkCmd.Parameters.AddWithValue("@idNivel", idNivel);
                        checkCmd.Parameters.AddWithValue("@idPlan", idPlan);
                        checkCmd.Parameters.AddWithValue("@idEscuela", idEscuela);
                        checkCmd.Parameters.AddWithValue("@kmPrograma", ID);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0) // Solo insertar si el registro no existe
                        {
                            // Preparar la consulta de inserción
                            string insertQuery = @"INSERT INTO SM_DETALLE_PROGRAMA 
                                           (kpNivel, kpPlanEstudio, kpEscuela, iCupo, kmPrograma) 
                                           VALUES (@idNivel, @idPlan, @idEscuela, @iCupo, @kmPrograma)";
                            SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction);
                            insertCmd.Parameters.AddWithValue("@idNivel", idNivel);
                            insertCmd.Parameters.AddWithValue("@idPlan", idPlan);
                            insertCmd.Parameters.AddWithValue("@idEscuela", idEscuela);
                            insertCmd.Parameters.AddWithValue("@iCupo", Convert.ToInt32(detalleRow.Cells[7].Text));
                            insertCmd.Parameters.AddWithValue("@kmPrograma", ID);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    // Confirmar transacción
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, revertir la transacción
                    transaction.Rollback();
                    // Manejar el error aquí (podrías agregar logging o mostrar un mensaje al usuario)
                }
                finally
                {
                    // Cerrar la conexión
                    connection.Close();
                }

                mensajeScript("Los datos se han actualizado con éxito");
            }
        }
        public void mensajeScript(string mensaje)
        {
            string scriptText = "$('.alert').remove(); $('body').prepend('<div class=\"alert alert-success alert-dismissible fade show\" role=\"alert\" style=\"background-color: #d4edda; color: #155724;\"><strong>" + mensaje + "</strong><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button></div>');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }


        private void MostrarRegistros2()
        {
            // Recupera la lista de registros desde el ViewState
            List<NPE> npe = ViewState[NPE_VIEWSTATE_KEY] as List<NPE> ?? new List<NPE>();

            // Asigna la lista de registros como origen de datos del GridView
            GridView2.DataSource = npe;
            GridView2.DataBind();
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[6].Visible = false;

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Ocultar las celdas en los datos
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[6].Visible = false;

                    LinkButton lbtEliminar = e.Row.FindControl("lbl_Eliminar") as LinkButton;
                    string id = (e.Row.DataItem as DataRowView)["idDetallePrograma"].ToString();
                    //lbtEliminar.CommandArgument = id;
                }
            }
            catch { }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Obtiene el índice de la fila que desencadenó el comando
            int index = Convert.ToInt32(e.CommandArgument);

            // Obtiene la fila seleccionada
            GridViewRow row = GridView2.Rows[index];

            // Obtiene los valores del registro seleccionado
            string nivel = row.Cells[0].Text;
            int kpNivel = Convert.ToInt32(row.Cells[1].Text);
            string planEstudio = row.Cells[2].Text;
            int kpPlanEstudio = Convert.ToInt32(row.Cells[3].Text);
            string escuela = row.Cells[4].Text;
            int kpEscuela = Convert.ToInt32(row.Cells[5].Text);
            string cupo = row.Cells[6].Text;

            // Verifica si el ViewState[NPE_VIEWSTATE_KEY] es nulo y, si lo es, inicialízalo como una nueva lista
            List<NPE> npe = ViewState[NPE_VIEWSTATE_KEY] as List<NPE>;
            if (npe == null)
            {
                npe = new List<NPE>();
            }

            // Busca el registro correspondiente en la lista utilizando los valores de la fila seleccionada
            NPE itemToDelete = npe.FirstOrDefault(item =>
                item.Nivel == nivel &&
                item.kpNivel == kpNivel &&
                item.PlanE == planEstudio &&
                item.kpPlanEstudio == kpPlanEstudio &&
                item.Escuela == escuela &&
                item.kpEscuela == kpEscuela &&
                item.iCupo == cupo
            );

            // Si se encuentra el registro, elimínalo de la lista
            if (itemToDelete != null)
            {
                npe.Remove(itemToDelete);

                // Actualiza el ViewState con la lista modificada
                ViewState[NPE_VIEWSTATE_KEY] = npe;

                // Vuelve a mostrar los registros en el GridView
                GridView2.DataSource = npe;
                GridView2.DataBind();
            }


        }

        protected void lbl_Eliminar_Click(object sender, EventArgs e)
        {
            LinkButton lbEliminar = (sender as LinkButton);
            string id = lbEliminar.CommandArgument;

            string connectionString = GlobalConstants.SQL;
            string deleteQuery = "DELETE SM_DETALLE_PROGRAMA WHERE idDetallePrograma = @idDetallePrograma";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@idDetallePrograma", id);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        string idPrograma = Request.QueryString["idPrograma"];
                        Editar(idPrograma);
                    }
                }
            }
        }
    }
}     