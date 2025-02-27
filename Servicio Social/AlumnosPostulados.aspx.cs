using System;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Linq;
using DocumentFormat.OpenXml;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using Table = CrystalDecisions.CrystalReports.Engine.Table;
using CrystalDecisions.CrystalReports.Engine;
using DataSet = System.Data.DataSet;
using ConnectionInfo = CrystalDecisions.Shared.ConnectionInfo;
//using QRCoder;
using ZXing;
using CrystalDecisions.ReportAppServer.DataDefModel;


namespace Servicio_Social

{
    public partial class AlumnosPostulados : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                CargarDatos(0, "","","");
                CargarEstatus();
                CargarUnidad();
                CargarPlan();
                CargarEscuela();
                CargarNivel();
                CargarPeriodo();
            }
        }
        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 0; }
            set { ViewState["CurrentPage"] = value; }
        }

        private int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
        }
        public class AlumnosReg
        {
            public DateTime FechaRegistro { get; set; }
            public string Matricula { get; set; }
            public string Alumno { get; set; }
            public string Programa { get; set; }
            public string PlanEstudios { get; set; }
            public string Escuela { get; set; }
            public string Cupo { get; set; }
            public string Estatus { get; set; }
        }
        #region Operaciones
        protected void CargarDatos(int pageIndex, string matricula, string programa, string nombre)
        {
            int pageSize = 20; // Cantidad de registros por página
            int totalRecords;
            string selectedEstatus = ddlEstatus.SelectedValue;
            string selectedUnidad = DDLUnidad.SelectedValue;
            string selectedNivel = ddlNivel.SelectedValue;
            string selectedEscuela = ddlEscuela.SelectedValue;
            string selectedPlan = ddlPlan.SelectedValue;
            string selectedPeriodo = ddlPeriodo.SelectedValue;



            DataTable dt = ObtenerDatos(pageIndex, pageSize, matricula, programa, nombre, selectedEstatus,  selectedUnidad, selectedNivel, selectedEscuela, selectedPlan, selectedPeriodo, out totalRecords);
                
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPrevious.Enabled = pageIndex > 0;
            btnNext.Enabled = pageIndex < TotalPages - 1;

            // Actualiza la etiqueta de número de página
            lblPageNumber.Text = $"Página {pageIndex + 1} de {TotalPages}";
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                // Obtener referencias a los botones
                LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
                LinkButton btnEvaluar = (LinkButton)e.Item.FindControl("btnEvaluar");
                LinkButton btnLiberar = (LinkButton)e.Item.FindControl("btnLiberar");
                LinkButton btnLiberarEsc = (LinkButton)e.Item.FindControl("btnLiberarEsc");
                LinkButton btnLiberarResp = (LinkButton)e.Item.FindControl("btnLiberarResp");
                LinkButton btnLiberarAdmon = (LinkButton)e.Item.FindControl("btnLiberarAdmon");
                LinkButton btnDOC = (LinkButton)e.Item.FindControl("btnDOC");

                // Obtener valores de estatus y tipo de usuario
                string estatus = row["idEstatus"].ToString().Trim();
                string usuario = Session["tipoUsuario"].ToString();

                // Inicializar visibilidad de todos los botones como false
                btnAutorizar.Visible = false;
                btnEliminar.Visible = false;
                btnEvaluar.Visible = false;
                btnLiberar.Visible = false;
                btnLiberarEsc.Visible = false;
                btnLiberarResp.Visible = false;
                btnLiberarAdmon.Visible = false;
                btnDOC.Visible = false;

                // Diccionario de configuraciones por tipo de usuario y estatus
                var configuraciones = new Dictionary<string, Dictionary<string, Action>>
        {
            // Configuración para ADMINISTRADOR (usuario == "1")
            { "1", new Dictionary<string, Action>
                {
                    { "20707", () => { btnEliminar.Visible = true;     btnAutorizar.Visible = true; } }, // EN ESPERA
                    { "7", () => {     btnEliminar.Visible = false;    btnAutorizar.Visible = true; } }, // CANCELADO
                    { "21522", () => { btnEliminar.Visible = true;     btnAutorizar.Visible = false; } }, // AUTORIZADO POR DEPENDENCIA
                    { "22113", () => { btnEliminar.Visible = false;    btnAutorizar.Visible = true; } }, // NO AUTORIZADO POR DEPENDENCIA
                    { "22114", () => { btnEliminar.Visible = false;    btnAutorizar.Visible = false; } }, // NO AUTORIZADO POR ENCARGADO ESCUELA
                    { "42187", () => { btnLiberarAdmon.Visible = true; btnEliminar.Visible = false; btnAutorizar.Visible = false; } }, // LIBERADO UNI
                    { "42188", () => { btnDOC.Visible = true; } }, // LIBERADO DSS
                }
            },
            // Configuración para DEPENDENCIA (usuario == "2")
            { "2", new Dictionary<string, Action>
                {
                    { "20707", () => { btnEliminar.Visible = true;     btnAutorizar.Visible = true; } }, // EN ESPERA
                    { "21522", () => { btnEliminar.Visible = true; btnAutorizar.Visible = false; btnEvaluar.Visible = true; } }, // AUTORIZADO POR DEPENDENCIA
                    { "22113", () => { btnEliminar.Visible = false; btnAutorizar.Visible = true; btnEvaluar.Visible = false; } }, // NO AUTORIZADO POR DEPENDENCIA
                    { "22115", () => { btnEliminar.Visible = false; btnAutorizar.Visible = false; btnLiberar.Visible = true; } }, // EVALUADO
                }
            },
            // Configuración para ENCARGADO ESCUELA (usuario == "4")
            { "4", new Dictionary<string, Action>
                {
                    { "22116", () => { btnLiberarEsc.Visible = true; } } // LIBERADO DEP
                }
            },
            // Configuración para RESPONSABLE UNIDAD (usuario == "3")
            { "3", new Dictionary<string, Action>
                {
                    { "42186", () => { btnLiberarResp.Visible = true; } } // LIBERADO ESC
                }
            }
        };

                // Aplicar configuración correspondiente
                if (configuraciones.ContainsKey(usuario) && configuraciones[usuario].ContainsKey(estatus))
                {
                    configuraciones[usuario][estatus].Invoke();
                }
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
        private void CargarPlan()
        {

            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = @"SELECT sClave + ' - ' + sDescripcion [Descripcion], idPlanEstudio 
                                        FROM SP_PLAN_ESTUDIO  
                                        WHERE bActivo = 1 AND bVigente = 1  
                                        AND sClave IS NOT NULL AND LEN(sClave) = 3 AND kpNivel != 3  
										AND idPlanEstudio  NOT IN (132,2354817)  ORDER BY sClave";

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
                newRow["Descripcion"] = "Seleccione Plan...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlPlan.DataSource = ds6;
                ddlPlan.DataTextField = "Descripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlPlan.DataValueField = "idplanEstudio";
                ddlPlan.DataBind();
            }

        }

        private void CargarEscuela()
        {

            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = @" SELECT DISTINCT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], ESC.idEscuelaUAC, PLA.kpEscuela_UAdeC 
                                        FROM SM_PLAN_EST_ESCUELA AS PLA 
										JOIN SP_PLAN_ESTUDIO AS PE ON PLA.kpPlan_Estudio = PE.idPlanEstudio
                                        JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC 
										WHERE PE.kpNivel != 3
                                        ORDER BY [Descripcion]";

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
                newRow["Descripcion"] = "Seleccione Escuela...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlEscuela.DataSource = ds6;
                ddlEscuela.DataTextField = "Descripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlEscuela.DataValueField = "idEscuelaUAC";
                ddlEscuela.DataBind();
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
                ddlNivel.DataSource = ds3;
                ddlNivel.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlNivel.DataValueField = "idTipoNivel";
                ddlNivel.DataBind();
            }

        }
        private void CargarPeriodo()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = @"SELECT idCiclo, sDescripcion FROM SP_CICLO 
                                        WHERE dFecha_Inicio >='2024-08-05 00:00:00.000' 
                                        AND idCiclo NOT IN (0,34)  ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione el Periodo...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlPeriodo.DataSource = ds3;
                ddlPeriodo.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlPeriodo.DataValueField = "idCiclo";
                ddlPeriodo.DataBind();
            }

        }
        protected void DDLUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idUnidadSeleccionado = DDLUnidad.SelectedValue;

            // 🚀 Si el usuario selecciona "Seleccione Unidad...", se recargan los valores iniciales
            if (string.IsNullOrEmpty(idUnidadSeleccionado) || idUnidadSeleccionado == "0")
            {
                CargarPlan(); // Recupera todos los planes originales
                CargarEscuela(); // Recupera todas las escuelas originales
                return;
            }

            // 🔹 Query corregida con alias AS y mejor estructura
            string queryStringEscuela = @"
        SELECT DISTINCT ESC.sClave + ' - ' + ESC.sDescripcion AS Descripcion, 
                        PLA.kpEscuela_UAdeC 
        FROM SM_PLAN_EST_ESCUELA AS PLA 
        JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC 
        WHERE ESC.idEscuelaUAC NOT IN (149,150,151,153)
          AND ESC.kpUnidad = @idUnidad 
        ORDER BY 1";

            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();

                using (SqlCommand cmdEscuela = new SqlCommand(queryStringEscuela, con))
                {
                    cmdEscuela.Parameters.Add("@idUnidad", SqlDbType.Int).Value = Convert.ToInt32(idUnidadSeleccionado);

                    using (SqlDataReader readerEscuela = cmdEscuela.ExecuteReader())
                    {
                        DataTable dtEscuela = new DataTable();
                        dtEscuela.Columns.Add("Descripcion", typeof(string));
                        dtEscuela.Columns.Add("kpEscuela_UAdeC", typeof(string));
                        dtEscuela.Rows.Add("Seleccione la Escuela...", "");

                        while (readerEscuela.Read())
                        {
                            dtEscuela.Rows.Add(readerEscuela["Descripcion"], readerEscuela["kpEscuela_UAdeC"]);
                        }

                        ddlEscuela.DataSource = dtEscuela;
                        ddlEscuela.DataTextField = "Descripcion";
                        ddlEscuela.DataValueField = "kpEscuela_UAdeC";
                        ddlEscuela.DataBind();
                    }
                }
            }
        }
        protected void DDLPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idNivelSeleccionado = ddlNivel.SelectedValue;
            string idUnidadSeleccionado = DDLUnidad.SelectedValue;

            ddlEscuela.Items.Clear();
            ddlPlan.Items.Clear();

            // 🚀 Si el usuario selecciona "Seleccione Nivel...", se recargan los valores iniciales
            if (idNivelSeleccionado == "" || idNivelSeleccionado == "0")
            {
                CargarPlan(); // Recupera todos los planes originales
                CargarEscuela(); // Recupera todas las escuelas originales
                return;
            }
            string queryStringEscuela;
            string queryStringPlan;

            if (!string.IsNullOrEmpty(idUnidadSeleccionado) && idUnidadSeleccionado != "0")
            {
                queryStringEscuela = @"
                    SELECT DISTINCT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC , P.kpNivel
                    FROM SM_PLAN_EST_ESCUELA AS PLA 
                    JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC 
                    JOIN SP_PLAN_ESTUDIO AS P ON PLA.kpPlan_Estudio = P.idPlanEstudio
                    WHERE P.kpNivel= @idNivel  AND ESC.idEscuelaUAC NOT IN (149,150,151,153)
                    AND ESC.kpUnidad = @idUnidad 
                    ORDER BY 1";

                queryStringPlan = @"
                    SELECT DISTINCT sClave + ' - ' + sDescripcion AS Descripcion, idPlanEstudio , kpNivel
                    FROM SP_PLAN_ESTUDIO  
                    WHERE bActivo = 1 
                    AND bVigente = 1  
                    AND sClave IS NOT NULL 
                    AND LEN(sClave) = 3 
                    AND kpNivel = @idNivel  
                    AND idPlanEstudio NOT IN (132,2354817,1177965)
                    AND idPlanEstudio IN (
                        SELECT DISTINCT kpPlan_Estudio 
                        FROM SM_PLAN_EST_ESCUELA 
                        WHERE kpEscuela_UAdeC IN (
                            SELECT idEscuelaUAC FROM SP_ESCUELA_UAC WHERE kpUnidad = @idUnidad
                        )
                    )
                    ORDER BY Descripcion";
            }
            else
            {
                queryStringEscuela = @"
                        SELECT DISTINCT ESC.sClave + ' - ' + ESC.sDescripcion [Descripcion], PLA.kpEscuela_UAdeC , P.kpNivel
                        FROM SM_PLAN_EST_ESCUELA AS PLA 
                        JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC
                        JOIN SP_PLAN_ESTUDIO AS P ON PLA.kpPlan_Estudio = P.idPlanEstudio
                        WHERE P.kpNivel= @idNivel  AND ESC.idEscuelaUAC NOT IN (149,150,151,153)
                        ORDER BY 1";

                queryStringPlan = @"
                        SELECT DISTINCT sClave + ' - ' + sDescripcion AS Descripcion, idPlanEstudio, kpNivel 
                        FROM SP_PLAN_ESTUDIO  
                        WHERE bActivo = 1 
                        AND bVigente = 1  
                        AND sClave IS NOT NULL 
                        AND LEN(sClave) = 3 
                        AND kpNivel = @idNivel 
                        AND idPlanEstudio NOT IN (132,2354817,1177965)
                        ORDER BY Descripcion";

            }
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();

                // Cargar escuelas filtradas
                SqlCommand cmdEscuela = new SqlCommand(queryStringEscuela, con);
                if (!string.IsNullOrEmpty(idUnidadSeleccionado) && idUnidadSeleccionado != "0")
                {
                    cmdEscuela.Parameters.AddWithValue("@idUnidad", idUnidadSeleccionado);
                }

                
                 cmdEscuela.Parameters.AddWithValue("@idNivel", idNivelSeleccionado);
                
                SqlDataReader readerEscuela = cmdEscuela.ExecuteReader();

                DataTable dtEscuela = new DataTable();
                dtEscuela.Columns.Add("Descripcion", typeof(string));
                dtEscuela.Columns.Add("kpEscuela_UAdeC", typeof(string));
                dtEscuela.Rows.Add("Seleccione la Escuela...", "");

                while (readerEscuela.Read())
                {
                    dtEscuela.Rows.Add(readerEscuela["Descripcion"], readerEscuela["kpEscuela_UAdeC"]);
                }
                readerEscuela.Close();

                ddlEscuela.DataSource = dtEscuela;
                ddlEscuela.DataTextField = "Descripcion";
                ddlEscuela.DataValueField = "kpEscuela_UAdeC";
                ddlEscuela.DataBind();


                SqlCommand cmdPlan = new SqlCommand(queryStringPlan, con);

                if (!string.IsNullOrEmpty(idUnidadSeleccionado) && idUnidadSeleccionado != "0")
                {
                    cmdPlan.Parameters.AddWithValue("@idUnidad", idUnidadSeleccionado);
                }

                // Agregar el parámetro @idNivel
                cmdPlan.Parameters.AddWithValue("@idNivel", idNivelSeleccionado);

                SqlDataReader readerPlan = cmdPlan.ExecuteReader();

                DataTable dtPlan = new DataTable();
                dtPlan.Columns.Add("Descripcion", typeof(string));
                dtPlan.Columns.Add("idPlanEstudio", typeof(string));
                dtPlan.Rows.Add("Seleccione el Plan...", ""); // Agregar la opción inicial

                while (readerPlan.Read())
                {
                    dtPlan.Rows.Add(readerPlan["Descripcion"], readerPlan["idPlanEstudio"]);
                }
                readerPlan.Close();

                ddlPlan.DataSource = dtPlan;
                ddlPlan.DataTextField = "Descripcion";
                ddlPlan.DataValueField = "idPlanEstudio";
                ddlPlan.DataBind();
            }

        }
        protected void DDLEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEscuela.Items.Clear();
            string idPlanSeleccionado = ddlPlan.SelectedValue;
            string idUnidadSeleccionado = DDLUnidad.SelectedValue;
            string idNivelSeleccionado = ddlNivel.SelectedValue;

            string queryString = "";

            // 🚀 **Nuevo supuesto: Si el usuario selecciona "Seleccione Plan..."**
            if (string.IsNullOrEmpty(idPlanSeleccionado) || idPlanSeleccionado == "0")
            {
                CargarPlan(); // Recupera todos los planes originales
                CargarEscuela(); // Recupera todas las escuelas originales
                return;
            }

            // 🚀 **Selecciona la consulta correcta según las opciones elegidas**
            if (!string.IsNullOrEmpty(idPlanSeleccionado) && idPlanSeleccionado != "0")
            {
                if (!string.IsNullOrEmpty(idNivelSeleccionado) && idNivelSeleccionado != "0" &&
                    !string.IsNullOrEmpty(idUnidadSeleccionado) && idUnidadSeleccionado != "0")
                {
                    // **Si selecciona Unidad, Plan y Nivel**
                    queryString = @"
            SELECT ESC.sClave + ' - ' + ESC.sDescripcion AS Descripcion, PLA.kpEscuela_UAdeC  
            FROM SM_PLAN_EST_ESCUELA AS PLA
            INNER JOIN SP_PLAN_ESTUDIO AS P ON PLA.kpPlan_Estudio = P.idPlanEstudio
            INNER JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC
            WHERE P.idPlanEstudio = @idPlan 
            AND P.kpNivel = @idNivel 
            AND ESC.kpUnidad = @idUnidad";
                }
                else if (!string.IsNullOrEmpty(idUnidadSeleccionado) && idUnidadSeleccionado != "0")
                {
                    // **Si selecciona Unidad y Plan**
                    queryString = @"
            SELECT ESC.sClave + ' - ' + ESC.sDescripcion AS Descripcion, PLA.kpEscuela_UAdeC  
            FROM SM_PLAN_EST_ESCUELA AS PLA
            INNER JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC 
            WHERE PLA.kpPlan_Estudio = @idPlan 
            AND ESC.kpUnidad = @idUnidad";
                }
                else if (!string.IsNullOrEmpty(idNivelSeleccionado) && idNivelSeleccionado != "0")
                {
                    // **Si selecciona Nivel y Plan**
                    queryString = @"
            SELECT ESC.sClave + ' - ' + ESC.sDescripcion AS Descripcion, PLA.kpEscuela_UAdeC  
            FROM SM_PLAN_EST_ESCUELA AS PLA
            INNER JOIN SP_PLAN_ESTUDIO AS P ON PLA.kpPlan_Estudio = P.idPlanEstudio
            INNER JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC
            WHERE P.idPlanEstudio = @idPlan 
            AND P.kpNivel = @idNivel";
                }
                else
                {
                    // **Si solo se tiene un plan de estudios seleccionado**
                    queryString = @"
            SELECT ESC.sClave + ' - ' + ESC.sDescripcion AS Descripcion, PLA.kpEscuela_UAdeC  
            FROM SM_PLAN_EST_ESCUELA AS PLA
            INNER JOIN SP_PLAN_ESTUDIO AS P ON PLA.kpPlan_Estudio = P.idPlanEstudio
            INNER JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC
            WHERE P.idPlanEstudio = @idPlan";
                }
            }

            // Si no hay consulta definida, salir del método para evitar errores
            if (string.IsNullOrEmpty(queryString))
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(queryString, con);

                // **Añadir parámetros dinámicamente según la consulta elegida**
                cmd.Parameters.AddWithValue("@idPlan", idPlanSeleccionado);
                if (!string.IsNullOrEmpty(idUnidadSeleccionado) && idUnidadSeleccionado != "0")
                {
                    cmd.Parameters.AddWithValue("@idUnidad", idUnidadSeleccionado);
                }
                if (!string.IsNullOrEmpty(idNivelSeleccionado) && idNivelSeleccionado != "0")
                {
                    cmd.Parameters.AddWithValue("@idNivel", idNivelSeleccionado);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                // **Crear un nuevo DataTable que incluya el primer elemento "Seleccione la Escuela..."**
                DataTable dt = new DataTable();
                dt.Columns.Add("Descripcion", typeof(string));
                dt.Columns.Add("kpEscuela_UAdeC", typeof(string));
                dt.Rows.Add("Seleccione la Escuela...", ""); // Agregar el primer elemento

                // **Llenar el DataTable con los resultados de la consulta**
                while (reader.Read())
                {
                    dt.Rows.Add(reader["Descripcion"], reader["kpEscuela_UAdeC"]);
                }

                reader.Close();

                // **Llenar ddlEscuela con los datos obtenidos**
                ddlEscuela.DataSource = dt;
                ddlEscuela.DataTextField = "Descripcion";
                ddlEscuela.DataValueField = "kpEscuela_UAdeC";
                ddlEscuela.DataBind();
            }
        }
        //protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        DataRowView row = (DataRowView)e.Item.DataItem;
        //        LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
        //        LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
        //        string estatus = row["idEstatus"].ToString().Trim();
        //        LinkButton btnEvaluar = (LinkButton)e.Item.FindControl("btnEvaluar");
        //        LinkButton btnLiberar = (LinkButton)e.Item.FindControl("btnLiberar");
        //        LinkButton btnLiberarEsc = (LinkButton)e.Item.FindControl("btnLiberarEsc");
        //        LinkButton btnLiberarResp = (LinkButton)e.Item.FindControl("btnLiberarResp");
        //        LinkButton btnLiberarAdmon = (LinkButton)e.Item.FindControl("btnLiberarAdmon");

        //        HashSet<string> EstatusValidos = new HashSet<string> { "20707", "21522", "21523" };

        //        string usuario = Session["tipoUsuario"].ToString();
        //        if ((usuario == "1")) //(usuario == "1" || usuario == "3")  // ADMINISTRADOR
        //        {
        //            btnEvaluar.Visible = false;
        //            btnLiberar.Visible = false;
        //            btnLiberarEsc.Visible = false;
        //            btnLiberarResp.Visible = false;
        //            btnLiberarAdmon.Visible = false;
        //            if (estatus == "7") //CANCELADO
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = true;
        //            }
        //            else if (estatus == "21522") //AUTORIZADO POR DEPENDENCIA
        //            {
        //                btnEliminar.Visible = true;
        //                btnAutorizar.Visible = false;
        //            }
        //            else if (estatus == "22114") // NO AUTORIZADO POR ENCARGADO DE ESCUELA
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //            }
        //            else if (estatus == "22113") //NO AUTORIZADO POR DEPENDENCIA
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = true;
        //            }
        //            else if (estatus == "22115") // EVALUADO
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //            }
        //            else if (estatus == "42187") //LIBERADO UNI
        //            {
        //                btnLiberarAdmon.Visible = true;
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //            }
        //            else if (estatus == "42186") // LIBERADO ESC
        //            {

        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //            }
        //            else if (estatus == "42188") // LIBERADO DSS
        //            {

        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //            }
        //            else if (estatus == "22116") //LIBERADO DEP
        //            {

        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //            }



        //        }
        //        else if (usuario == "2") // DEPENDENCIA
        //        {
        //            btnLiberar.Visible = false;
        //            btnLiberarEsc.Visible = false;
        //            btnLiberarResp.Visible = false;
        //            btnLiberarAdmon.Visible = false;
        //            if (estatus == "21522") // AUTORIZADO POR DEPENDENCIA
        //            {
        //                btnEliminar.Visible = true;
        //                btnAutorizar.Visible = false;
        //                btnEvaluar.Visible = true;
        //                btnLiberar.Visible = false;
        //            }
        //            else if (estatus == "22113") //NO AUTORIZADO POR DEPENDENCIA
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = true;
        //                btnEvaluar.Visible = false;
        //                btnLiberar.Visible = false;
        //            }
        //            else if (estatus == "22115") //NO AUTORIZADO POR ENCARGADO DE ESCUELA
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //                btnLiberar.Visible = false;
        //                btnEvaluar.Visible = false;
        //            }
        //            else if (estatus == "22116") // LIBERADO DEP
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //                btnLiberar.Visible = false;
        //                btnEvaluar.Visible = false;
        //            }
        //            else if (estatus == "22115") //EVALUADO
        //            {
        //                btnEliminar.Visible = false;
        //                btnAutorizar.Visible = false;
        //                btnLiberar.Visible = true;
        //                btnEvaluar.Visible = false;
        //            }
        //        }
        //        else if (usuario == "4") // ENCARGADO DE ESCUELA
        //        {
        //            btnEvaluar.Visible = false;
        //            btnEliminar.Visible = false;
        //            btnAutorizar.Visible = false;
        //            btnLiberar.Visible = false;
        //            btnLiberarEsc.Visible = false;
        //            btnLiberarResp.Visible = false;
        //            btnLiberarAdmon.Visible = false;

        //            if (estatus == "22116")  // LIBERADO DEP
        //            {
        //                btnLiberarEsc.Visible = true;
        //            }
        //        }
        //        else if (usuario == "3") // RESPONSABLE UNIDAD
        //        {
        //            btnEvaluar.Visible = false;
        //            btnEliminar.Visible = false;
        //            btnAutorizar.Visible = false;
        //            btnLiberar.Visible = false;
        //            btnLiberarEsc.Visible = false;
        //            btnLiberarResp.Visible = false;
        //            btnLiberarAdmon.Visible = false;

        //            if (estatus == "42186")  // LIBERADO ESC
        //            {
        //                btnLiberarResp.Visible = true;
        //            }
        //        }


        //    }
        //}
        [WebMethod]
        public static string llenarDatosModal(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string query =
                            @"SELECT  ALU.SMATRICULA MATRICULA , sCorreo_institucional,
	                                    PER.SNOMBRE_COMPLETO,
		                                PLA.SCLAVE + ' - ' + PLA.SDESCRIPCION PLAN_ESTUDIOS ,
		                                ESC.SCLAVE + ' - ' + ESC.SDESCRIPCION ESCUELAUAC,
		                                PER.STELEFONO,
	                                    CONVERT(varchar, dFecha_nacimiento, 103) AS dFecha_nacimiento
                                FROM SM_ALUMNO AS ALU
                                LEFT JOIN	 NM_PERSONA AS PER ON ALU.KMPERSONA = PER.IDPERSONA
                                LEFT JOIN	 SP_PLAN_ESTUDIO AS PLA ON ALU.KPPLAN_ESTUDIOS = PLA.IDPLANESTUDIO
                                LEFT JOIN	 SP_ESCUELA_UAC   AS ESC ON ALU.KPESCUELASUADEC = ESC.IDEsCUELAUAC
                                LEFT JOIN	 SP_ESTATUS_ALUMNO AS EST ON ALU.KPESTATUS_ALUMNO = EST.IDESTATUSALUMNO
                                LEFT JOIN	 SP_CICLO AS C ON ALU.kpCiclo = C.idCiclo
                                LEFT JOIN	 SP_FORMA_INGRESO AS FI ON ALU.kpForma_ingreso =  FI.idFormaIngreso 
	                                WHERE   
	                                 ALU.IDALUMNO =@Id";

            string htmlResult = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        htmlResult += "<table style='width: 100%; table-layout: fixed;'>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Matricula:</strong> {reader["MATRICULA"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Nombre:</strong> {reader["SNOMBRE_COMPLETO"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Escuela:</strong> {reader["ESCUELAUAC"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Plan de Estudios:</strong> {reader["PLAN_ESTUDIOS"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Correo:</strong> {reader["sCorreo_institucional"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Teléfono:</strong> {reader["STELEFONO"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Fecha de nacimiento:</strong> {reader["dFecha_nacimiento"]}</td>";
                        htmlResult += "</tr>"; 

                        htmlResult += "</table>";
                    }

                    con.Close();
                }
            }

            return htmlResult;
        }
        private void CargarEstatus()
        {
            string query = "SELECT idEstatus,sClave, sDescripcion FROM NP_ESTATUS WHERE sClave IN ('23','24','27','29','30','34','35','36')"; // Ajusta la condición según tu criterio
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlEstatus.DataSource = reader;
                    ddlEstatus.DataTextField = "sDescripcion";
                    ddlEstatus.DataValueField = "idEstatus";
                    ddlEstatus.DataBind();
                    ddlEstatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione un estatus......", "")); // Agrega una opción por defecto
            }
        }
        //protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int pageSize = 20; // Cantidad de registros por página
        //    string searchTerm = txtBusqueda.Text.Trim();
        //    string selectedEstatus = ddlEstatus.SelectedValue;
        //    int totalRecords;
        //    int pageIndex = 0;
        //    DataTable data = ObtenerDatos(pageIndex, pageSize, searchTerm, selectedEstatus, out totalRecords);

        //    Repeater1.DataSource = data;
        //    Repeater1.DataBind();

        //    // Calcula el número total de páginas
        //    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        //    // Configura el estado de los botones
        //    btnPrevious.Enabled = pageIndex > 0;
        //    btnNext.Enabled = pageIndex < TotalPages - 1;

        //    // Actualiza la etiqueta de número de página
        //    lblPageNumber.Text = $"Página {pageIndex + 1} de {TotalPages}";

        //}

        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string matricula , string programa, string nombre, string selectedEstatus, string selectedUnidad, string selectedNivel,string selectedEscuela,string selectedPlan, string selectedPeriodo , out int totalRecords)
        { 
            //string idDependencia = Session["idDependencia"].ToString().Split('|')[1];
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;
            string tipoUsuario = Session["tipoUsuario"].ToString();
            
            string filtroquery = "";

            string baseQuery = @"FROM SM_PROGRAMA_ALUMNO PA
                            INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
                            INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
                            INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
                            INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.ID = PA.kmAlumno
                            INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan
                            INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = UA.kpEscuela
                            INNER JOIN SP_PLAN_ESTUDIO PE2 ON PE2.idPlanEstudio = DP.kpPlanEstudio
                            INNER JOIN SP_ESCUELA_UAC UO2 ON UO2.idEscuelaUAC = DP.kpEscuela
                            INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno
                            INNER JOIN NM_PERSONA PER ON PER.idPersona = AL.kmPersona
                            INNER JOIN NP_ESTATUS NPEST ON PA.KPESTATUS = NPEST.IDESTATUS
                            INNER JOIN NP_UNIDAD UN ON UO.KPUNIDAD = UN.IDUNIDAD
                            WHERE PA.KPESTATUS != 7";


            List<string> F = new List<string>();

            // Filtros generales
            if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                F.Add(" UO.kpUnidad = @selectedUnidad");
            if (!string.IsNullOrEmpty(selectedNivel) && selectedNivel != "0")
                F.Add(" PE.kpNivel = @selectedNivel");
            if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                F.Add(" PA.KPESTATUS = @selectedEstatus");
            if (!string.IsNullOrEmpty(selectedEscuela) && selectedEscuela != "0")
                F.Add(" AL.kpEscuelasUadeC = @selectedEscuela");
            if (!string.IsNullOrEmpty(selectedPlan) && selectedPlan != "0")
                F.Add(" AL.kpPlan_estudios = @selectedPlan");
            if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                F.Add("  P.kpPeriodo = @selectedPeriodo");
            if (!string.IsNullOrEmpty(matricula))
                F.Add(" AL.sMatricula LIKE @matricula");
            if (!string.IsNullOrEmpty(programa))
                F.Add(" P.sNombre_Programa LIKE @programa");
            if (!string.IsNullOrEmpty(nombre))
                F.Add(" PER.sNombre_completo LIKE @nombre");

            // Aplicar filtros específicos según el tipo de usuario
            if (tipoUsuario == "2") // USUARIO DEPENDENCIA
            {
                string filtros = Session["idDependencia"].ToString();
                string idDependencia = filtros.Split('|')[1];

                // Agregar filtro para dependencia de forma segura
                F.Add(" DS.IDDEPENDENICASERVICIO = @idDependencia");
            }

            if (tipoUsuario == "3") // USUARIO RESPONSABLE
            {
                string filtros = Session["filtros"].ToString();
                string unidadUsuario = filtros.Split('|')[1];

                // Agregar filtro para unidad del usuario responsable
                F.Add(" P.kpUnidad = @unidadUsuario");
            }

            if (tipoUsuario == "4") // USUARIO ENCARGADO DE ESCUELA
            {
                string filtros = Session["filtros"].ToString();
                string Escuela = filtros.Split('|')[2];

                // Agregar filtro para escuela
                F.Add(" UA.kpEscuela = @Escuela");
            }

            // Construcción de la cláusula WHERE final
            string filtroQuery = F.Count > 0 ? " AND " + string.Join(" AND ", F) : "";


            string query = $@"SELECT DS.IDDEPENDENICASERVICIO, PA.idProgramaAlumno,AL.IDALUMNO,
                             CONVERT(varchar, PA.DFECHAREGISTRO, 103) AS FECHAREGISTRO,
                             AL.SMATRICULA AS MATRICULA ,PER.SNOMBRE_COMPLETO AS NOMBRE_COMPLETO, P.SNOMBRE_PROGRAMA AS PROGRMA,
                             PE.sClave + ' - ' + PE.sDescripcion AS PLANEST, UO.sClave + ' - ' + UO.sDescripcion AS ESCUELA,
                             UN.SCIUDAD AS UNIDAD,
                             DP.ICUPO,
                             NPEST.SDESCRIPCION AS ESTATUS, NPEST.idEstatus
                              {baseQuery} {filtroQuery}
                              ORDER BY PA.DFECHAREGISTRO ASC
                              OFFSET @rowsToSkip ROWS FETCH NEXT @pageSize ROWS ONLY;";


            string countQuery = "SELECT COUNT(*) " + baseQuery + filtroQuery;

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlCommand countCmd = new SqlCommand(countQuery, con))
                {

                    if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                    {
                        cmd.Parameters.AddWithValue("@selectedUnidad", selectedUnidad);
                        countCmd.Parameters.AddWithValue("@selectedUnidad", selectedUnidad);
                    }

                    if (!string.IsNullOrEmpty(selectedNivel) && selectedNivel != "0")
                    {
                        cmd.Parameters.AddWithValue("@selectedNivel", selectedNivel);
                        countCmd.Parameters.AddWithValue("@selectedNivel", selectedNivel);
                    }

                    if (!string.IsNullOrEmpty(selectedEscuela) && selectedEscuela != "0")
                    {
                        cmd.Parameters.AddWithValue("@selectedEscuela", selectedEscuela);
                        countCmd.Parameters.AddWithValue("@selectedEscuela", selectedEscuela);
                    }

                    if (!string.IsNullOrEmpty(selectedPlan) && selectedPlan != "0")
                    {
                        cmd.Parameters.AddWithValue("@selectedPlan", selectedPlan);
                        countCmd.Parameters.AddWithValue("@selectedPlan", selectedPlan);
                    }
                    if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                    {
                        cmd.Parameters.AddWithValue("@selectedPeriodo", selectedPeriodo);
                        countCmd.Parameters.AddWithValue("@selectedPeriodo", selectedPeriodo);
                    }
                    if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                    {
                        cmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                        countCmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                    }

                    if (!string.IsNullOrEmpty(matricula))
                    {
                        cmd.Parameters.AddWithValue("@matricula", $"%{matricula}%");
                        countCmd.Parameters.AddWithValue("@matricula", $"%{matricula}%");
                    }

                    if (!string.IsNullOrEmpty(programa))
                    {
                        cmd.Parameters.AddWithValue("@programa", $"%{programa}%");
                        countCmd.Parameters.AddWithValue("@programa", $"%{programa}%");
                    }
                    if (!string.IsNullOrEmpty(nombre))
                    {
                        cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
                        countCmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
                    }

                    if (tipoUsuario == "2") // Agregar parámetro para Dependencia
                    {
                        string filtros = Session["idDependencia"].ToString();
                        string idDependencia = filtros.Split('|')[1];
                        cmd.Parameters.AddWithValue("@idDependencia", idDependencia);
                        countCmd.Parameters.AddWithValue("@idDependencia", idDependencia);
                    }

                    if (tipoUsuario == "3") // Agregar parámetro para Responsable
                    {
                        string filtros = Session["filtros"].ToString();
                        string unidadUsuario = filtros.Split('|')[1];
                        cmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                        countCmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                    }

                    if (tipoUsuario == "4") // Agregar parámetro para Encargado de Escuela
                    {
                        string filtros = Session["filtros"].ToString();
                        string Escuela = filtros.Split('|')[2];
                        cmd.Parameters.AddWithValue("@Escuela", Escuela);
                        countCmd.Parameters.AddWithValue("@Escuela", Escuela);
                    }
                    cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    con.Open();

                    totalRecords = (int)countCmd.ExecuteScalar();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;

        }
        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }
        protected void cambiarEstatus( string idPrograma_Alumno, string idUsuario, string cambio)
        {
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ActualizarProgramaAlumnoYBitacora_ss", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Parámetros
                    command.Parameters.AddWithValue("@kpEstatus", cambio);
                    command.Parameters.AddWithValue("@idPrograma_Alumno", idPrograma_Alumno);
                    command.Parameters.AddWithValue("@kmAutorizo", idUsuario);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        protected void cambiarEstatusLiberado(string idPrograma_Alumno, string idUsuario, string cambio)
        {
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ActualizarEstatusAlumno_ss", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Parámetros
                    command.Parameters.AddWithValue("@kpEstatus", cambio);
                    command.Parameters.AddWithValue("@idPrograma_Alumno", idPrograma_Alumno);
                    command.Parameters.AddWithValue("@kmLiberoEstudiante", idUsuario);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        protected void btnLiberar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string nst = btn.CommandArgument;
            Response.Redirect("LiberarEstudiante.aspx?nst=" + nst);


        }
        //protected void btnLiberar_Click(object sender, EventArgs e)
        //{
        //    // Obtén el ID del registro desde el CommandArgument
        //    LinkButton btn = (LinkButton)sender;
        //    string id = btn.CommandArgument;
        //    string connectionString = GlobalConstants.SQL;

        //    try
        //    {
        //        // Conexión a la base de datos
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            string query = " SELECT USA.ID ,US.dFechaRegistro, US.sCorreo AS Correo, ALU.sMatricula AS Matricula, PER.sNombre_completo AS Alumno, PLA.sClave + ' - ' + PLA.sDescripcion AS PlanEstudio, " +
        //                           " ESC.sClave + ' - ' + ESC.sDescripcion AS Escuela, EST.sDescripcion AS EstadoAutorizacion " +
        //                           " FROM SM_USUARIO AS US JOIN SM_USUARIOS_ALUMNOS AS USA ON US.idUsuario = USA.kmUsuario JOIN SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno " +
        //                           " JOIN NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona JOIN SP_ESCUELA_UAC AS ESC ON USA.kpEscuela = ESC.idEscuelaUAC " +
        //                           " JOIN SP_PLAN_ESTUDIO AS PLA ON USA.kpPlan = PLA.idPlanEstudio JOIN NP_ESTATUS AS EST ON USA.bAutorizado = EST.idEstatus WHERE ALU.idAlumno = @ID";
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            cmd.Parameters.AddWithValue("@ID", id);

        //            conn.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                // Datos requeridos para el documento
        //                string director = "(Nombre del Director de la Escuela o Facultad)"; // Deberías obtenerlo también
        //                string escuela = reader["Escuela"].ToString();
        //                string alumno = reader["Alumno"].ToString();
        //                string matricula = reader["Matricula"].ToString();
        //                string planEstudios = reader["PlanEstudio"].ToString();
        //                string fechaInicio = "(día, mes y año del inicio)"; // Deberías obtenerlo también
        //                string fechaFin = "(día, mes y año de término)"; // Deberías obtenerlo también
        //                string numHoras = "´número de horas obligatorias)"; // Deberías obtenerlo también
        //                string actividades ="(descripción general de las tareas o actividades realizadas en la prestación)"; // Deberías obtenerlo también

        //                // Generar el documento Word
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true))
        //                    {
        //                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

        //                        // Crear las propiedades de sección
        //                        SectionProperties sectionProps = new SectionProperties(
        //                            new PageMargin()
        //                            {
        //                                Top = 1440,     // 1 pulgada (en twips)
        //                                Right = 1440,   // 1 pulgada
        //                                Bottom = 1440,  // 1 pulgada
        //                                Left = 1440,    // 1 pulgada
        //                                Header = 720,   // 0.5 pulgada
        //                                Footer = 720,   // 0.5 pulgada
        //                                Gutter = 0      // Sin espacio adicional
        //                            }
        //                        );

        //                        // Crear el documento con el contenido
        //                        mainPart.Document = new Document(new Body(
        //                            sectionProps,

        //                            new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new RunProperties(new Bold()), new Text(director))),

        //                            new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new Text($"Director de {escuela}"))),

        //                            new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new Text("P r e s e n t e .-"))),

        //                            new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new Text($"Por medio de la presente ")),
        //                                new Run(new RunProperties(new Bold()), new Text(" HACE CONSTAR ")),
        //                                new Run(new Text($" que el/la C. {alumno} con matrícula ")),
        //                                new Run(new RunProperties(new Bold()), new Text(matricula)),
        //                                new Run(new Text($" estudiante de {planEstudios} de la UNIVERSIDAD AUTÓNOMA DE COAHUILA, realizó su servicio social en (nombre de la institución donde se prestó el Servicio Social), durante el periodo comprendido del {fechaInicio} al {fechaFin} cubriendo un total de {numHoras} horas efectivas, desarrollando como actividades {actividades}."))),

        //                            new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new Text("Sin mas por el momento, y agradeciendo de antemano sus intenciones, me despido."))),

        //                            new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new Text("Atentamente"))),

        //                             new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Both }),
        //                                new Run(new Text("(Nombre y puesto de quien responde a nombre de la institución donde se prestó el servicio)")))
        //                        ));
        //                    }

        //                    // Enviar el archivo al cliente
        //                    Response.Clear();
        //                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //                    Response.AddHeader("content-disposition", "attachment;filename=DocumentoGenerado.docx");
        //                    Response.BinaryWrite(ms.ToArray());
        //                    Response.Flush();  // Envía el contenido al cliente sin abortar el hilo
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log del error para depuración
        //        //LogError("Error en btnLiberar_Click: " + ex.Message);
        //        Response.Write("<script>alert('Ha ocurrido un error al generar el documento. Inténtelo nuevamente.');</script>");
        //    }
        //}

        //private void ExportToExcel()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter sw = new StringWriter(sb);
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    DocumentFormat.OpenXml.Spreadsheet.Page page = new DocumentFormat.OpenXml.Spreadsheet.Page();
        //    HtmlForm form = new HtmlForm();

        //    RepeaterTemp.DataSource = GetAllData();
        //    RepeaterTemp.DataBind();
        //    RepeaterTemp.RenderControl(htw);

        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Buffer = true;
        //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Sanciones.xls");
        //    HttpContext.Current.Response.Charset = "";
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //    HttpContext.Current.Response.ContentEncoding = Encoding.Default;
        //    HttpContext.Current.Response.Write(sb.ToString());
        //    HttpContext.Current.Response.Flush();
        //    HttpContext.Current.Response.End();
        //}

        private DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            string connectionString = GlobalConstants.SQL; // Tu cadena de conexión aquí
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT S.sIdentificador AS Identificador ,P.sNombre_completo AS Nombre, 
                        U.sDescripcion AS Unidad, S.SCONDUCTAS AS Conductas,S.SRESOLUCION AS Resolucion,
                        TS.Descripcion AS TipoSancion, S.SANIO AS Anio
                        FROM SM_SANCIONES AS S
                        INNER JOIN NM_PERSONA AS P ON P.IDPERSONA=S.kmPersona
                        INNER JOIN NP_UNIDAD AS U ON U.idUnidad=S.kpUnidad
                        INNER JOIN SP_TIPO_SANCION AS TS ON TS.idTipoSancion=S.kpTipo_sancion  
                        WHERE S.kpEstatus != 7";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(dt);
            }
            return dt;
        }
        //public byte[] GenerarCodigoQR(string texto)
        //{
        //    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        //    {
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
        //        using (QRCode qrCode = new QRCode(qrCodeData))
        //        {
        //            using (Bitmap qrBitmap = qrCode.GetGraphic(20))
        //            {
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    qrBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //                    return ms.ToArray();
        //                }
        //            }
        //        }
        //    }
        //}
        //public byte[] GenerarCodigoQR(string texto)
        //{
        //    using (var qrGenerator = new QRCodeGenerator())
        //    using (var qrCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q))
        //    using (var qrCode = new QRCode(qrCodeData))
        //    using (var bitmap = qrCode.GetGraphic(20)) // Tamaño 20
        //    using (var stream = new MemoryStream())
        //    {
        //        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return stream.ToArray(); // Convertir a byte[]
        //    }
        //}
        //public DataTable CrearTablaConQR(string texto)
        //{
        //    var table = new DataTable();
        //    table.Columns.Add("QR", typeof(byte[])); // Columna para el QR
        //    table.Columns.Add("OtroCampo", typeof(string)); // Ejemplo de otro campo

        //    var qrBytes = GenerarCodigoQR(texto);
        //    table.Rows.Add(qrBytes, "Ejemplo de otro dato");
        //    return table;
        //}
        //public System.Drawing.Image GenerarQRCode(string texto)
        //{
        //    // Crear un generador de códigos QR
        //    BarcodeWriter barcodeWriter = new BarcodeWriter();
        //    barcodeWriter.Format = BarcodeFormat.QR_CODE;  // Especificar que es un código QR
        //    barcodeWriter.Options = new ZXing.Common.EncodingOptions
        //    {
        //        Width = 300,  // Establecer el tamaño
        //        Height = 300
        //    };

        //    // Generar la imagen del código QR
        //    Bitmap qrCodeImage = barcodeWriter.Write(texto);

        //    return qrCodeImage;
        //}
        // ESTE ES EL PRIMERO
        private void GenerarReporte(int idProgramaAlumno)
        {
            try
            {
                // Ruta del archivo .rpt de Crystal Reports
                string reportPath = Server.MapPath("~/Reportes/ConstanciaFinal.rpt");

                using (ReportDocument reporte = new ReportDocument())
                {
                    // Cargar el reporte
                    reporte.Load(reportPath);

                    // Establecer el login para la conexión de base de datos
                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = "148.212.19.202",
                        DatabaseName = "PDU202",
                        UserID = "sa",
                        Password = "PDU2021*."
                    };

                    // Iterar a través de las tablas del reporte y establecer la conexión
                    foreach (Table table in reporte.Database.Tables)
                    {
                        TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                        tableLogOnInfo.ConnectionInfo = connectionInfo;
                        table.ApplyLogOnInfo(tableLogOnInfo);
                    }

                    // Pasar el parámetro al reporte
                    reporte.SetParameterValue("@kmProgramaAlumno", idProgramaAlumno);

                    // Exportar el reporte a un Stream en formato PDF
                    using (Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat))
                    {
                        // Convierte el Stream en un array de bytes
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);

                        // Convierte los bytes a una cadena Base64
                        string base64String = Convert.ToBase64String(bytes);

                        // Renderiza el PDF dentro de un iframe en la página
                         

                        // Asignar el PDF a un control oculto en la página
                        hiddenPdfBase64.Value = base64String;


                        // Mostrar el modal con el PDF

                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPdfModal", "ocultarOverlay(); $('#pdfModal').modal('show');", true);

                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // Mostrar detalles del error en caso de que ocurra una excepción
                Response.Write("<script>alert('Error al generar el reporte: " + ex.Message + "');</script>");
            }
            catch (Exception ex)
            {
                // Capturar cualquier otro tipo de error
                Response.Write("<script>alert('Ha ocurrido un error inesperado: " + ex.Message + "');</script>");
            }


        }
     
        #endregion

        #region Botones
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfidPrograma.Value))
            {
             
                // Obtener el valor del HiddenField como string y convertirlo a entero
                int idProgramaAlumno = Convert.ToInt32(hfidPrograma.Value);

                // Generar el reporte después de guardar
                GenerarReporte(idProgramaAlumno);

                string script = @"
        document.getElementById('pdfIframe').style.display = 'block';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowIframe", script, true);
            }
            
        }
            protected void btnDOC_Click(object sender, EventArgs e)
        {
            // Limpiar el HiddenField hiddenPdfBase64 antes de proceder
            //hiddenPdfBase64.Value = string.Empty;

            LinkButton btn = (LinkButton)(sender);
            string nst = btn.CommandArgument;
           

         
            //hfidPrograma.Value =   Convert.ToString(idProgramaAlumno);
            string connectionString = GlobalConstants.SQL; // Tu cadena de conexión aquí
            string sContenidoCarta = "Por medio de la presente hacemos constar de C. ";

            // Obtener los valores desde la base de datos con la consulta SQL
            string query = @"
		                SELECT  
                        AL.SMATRICULA AS MATRICULA,
                        PER.sNombres + ' ' + 
                        ISNULL(NULLIF(PER.sApellido_paterno, ''), '') + ' ' + 
                        ISNULL(NULLIF(PER.sApellido_materno, ''), '') AS NOMBRE_COMPLETO, 
                        PE.sDescripcion AS PLANEST,
                        GETDATE() AS FECHA_HOY,
                        DATENAME(YEAR, GETDATE()) AS ANIO,
                        CASE DAY(GETDATE())
                            WHEN 1 THEN 'primero'
                            WHEN 2 THEN 'dos'
                            WHEN 3 THEN 'tres'
                            WHEN 4 THEN 'cuatro'
                            WHEN 5 THEN 'cinco'
                            WHEN 6 THEN 'seis'
                            WHEN 7 THEN 'siete'
                            WHEN 8 THEN 'ocho'
                            WHEN 9 THEN 'nueve'
                            WHEN 10 THEN 'diez'
                            WHEN 11 THEN 'once'
                            WHEN 12 THEN 'doce'
                            WHEN 13 THEN 'trece'
                            WHEN 14 THEN 'catorce'
                            WHEN 15 THEN 'quince'
                            WHEN 16 THEN 'dieciséis'
                            WHEN 17 THEN 'diecisiete'
                            WHEN 18 THEN 'dieciocho'
                            WHEN 19 THEN 'diecinueve'
                            WHEN 20 THEN 'veinte'
                            WHEN 21 THEN 'veintiuno'
                            WHEN 22 THEN 'veintidós'
                            WHEN 23 THEN 'veintitrés'
                            WHEN 24 THEN 'veinticuatro'
                            WHEN 25 THEN 'veinticinco'
                            WHEN 26 THEN 'veintiséis'
                            WHEN 27 THEN 'veintisiete'
                            WHEN 28 THEN 'veintiocho'
                            WHEN 29 THEN 'veintinueve'
                            WHEN 30 THEN 'treinta'
                            WHEN 31 THEN 'treinta y uno'
                        END AS DIA_TEXTO,
                        CASE MONTH(GETDATE())
                            WHEN 1 THEN 'Enero'
                            WHEN 2 THEN 'Febrero'
                            WHEN 3 THEN 'Marzo'
                            WHEN 4 THEN 'Abril'
                            WHEN 5 THEN 'Mayo'
                            WHEN 6 THEN 'Junio'
                            WHEN 7 THEN 'Julio'
                            WHEN 8 THEN 'Agosto'
                            WHEN 9 THEN 'Septiembre'
                            WHEN 10 THEN 'Octubre'
                            WHEN 11 THEN 'Noviembre'
                            WHEN 12 THEN 'Diciembre'
                        END AS MES_TEXTO
                    FROM SM_PROGRAMA_ALUMNO PA
                    INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.idDetallePrograma = PA.kmDetallePrograma
                    INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
                    INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
                    INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.ID = PA.kmAlumno
                    INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan
                    INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = UA.kpEscuela
                    INNER JOIN SP_PLAN_ESTUDIO PE2 ON PE2.idPlanEstudio = DP.kpPlanEstudio
                    INNER JOIN SP_ESCUELA_UAC UO2 ON UO2.idEscuelaUAC = DP.kpEscuela
                    INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno
                    INNER JOIN NM_PERSONA PER ON PER.idPersona = AL.kmPersona
                    WHERE PA.idProgramaAlumno = @idProgramaAlumno";

            // Asumimos que ya tienes el idProgramaAlumno
            string alumnoNombre = "", plan = "", diaTexto = "", mes = "", anio = "";
            SqlDataReader reader = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idProgramaAlumno", nst);
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    alumnoNombre = reader["NOMBRE_COMPLETO"].ToString();
                    plan  = reader["PLANEST"].ToString();
                    diaTexto = reader["DIA_TEXTO"].ToString();
                    mes = reader["MES_TEXTO"].ToString();
                    anio = reader["ANIO"].ToString();
                }
                conn.Close();
            }

            // Ahora concatenamos el texto con los valores obtenidos
            sContenidoCarta += alumnoNombre + ", quien cursó la carrera de " + plan + " de esta Universidad, ";
            sContenidoCarta += "cumplió satisfactoriamente con su Servicio Social, según lo dispuesto en el Artículo 10 de la Ley General de Educación así como en los Artículos 9 y 10 del reglamento para la Prestación del Servicio Social de los Estudiantes de las instituciones de Educación Superior en la República Mexicana y en los Artículos 4°, 5° y 6° del reglamento para la Prestación del Servicio Social de la Universidad Autónoma de Coahuila. ";
            sContenidoCarta += "\n\nSe extiende la presente con las facuiltades que confiere la fracción III del Artículo 15 del Reglamento para la Prestación del Servicio Social de la Universidad Autónoma de Coahuila.";
            sContenidoCarta += "\n\nSaltillo Coahuila de Zaragoza, a los " + diaTexto + " días de " + mes +" del " + anio + ".";

            // Guardamos el texto concatenado en la base de datos
            string updateQuery = "UPDATE SM_PROGRAMA_ALUMNO SET sContenidoCarta = @sContenidoCarta WHERE idProgramaAlumno = @idProgramaAlumno";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@sContenidoCarta", sContenidoCarta);
                cmd.Parameters.AddWithValue("@idProgramaAlumno", nst);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Generar el reporte después de guardar
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showOverlay", "mostrarOverlay();", true);
            // ScriptManager.RegisterStartupScript(this, GetType(), "ShowPdfModal", "ocultarOverlay(); $('#pdfModal').modal('show');", true);
            //GenerarReporte(idProgramaAlumno);
            // Response.Redirect("CartaTerminacion.aspx?nst=" + nst);
            Response.Redirect("CartaLiberacion.aspx?nst=" + nst);
        }
        //protected void btnExportExcel_Click(object sender, EventArgs e)
        //{
        //    ExportToExcel();
        //}
        protected void btnLiberarAdmon_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            LinkButton lnkUpdate = (LinkButton)sender;
            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
            string idPrograma_Alumno = lnkUpdate.CommandArgument.Split('|')[0];
            string idUsuario = Session["idUser"].ToString();

            string cambio = "42188"; //LIBERADO UNI

            cambiarEstatusLiberado(idPrograma_Alumno, idUsuario, cambio);
            
            //string searchTerm = txtBusqueda.Text.Trim();
            string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            int page = CurrentPage;
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(programa) || string.IsNullOrEmpty(nombre))
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, "","","");
            }
            else
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, matricula, programa, nombre);
            }

        }
        protected void btnLiberarResp_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            LinkButton lnkUpdate = (LinkButton)sender;
            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
            string idPrograma_Alumno = lnkUpdate.CommandArgument.Split('|')[0];
            string idUsuario = Session["idUser"].ToString();
            
            string cambio = "42187"; //LIBERADO UNI

            cambiarEstatusLiberado(idPrograma_Alumno, idUsuario, cambio);
            //string searchTerm = txtBusqueda.Text.Trim();
            string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            int page = CurrentPage;
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(programa) || string.IsNullOrEmpty(nombre))
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, "", "", "");
            }
            else
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, matricula, programa, nombre);
            }

        }
        protected void btnLiberarEsc_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            LinkButton lnkUpdate = (LinkButton)sender;
            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
            string idPrograma_Alumno = lnkUpdate.CommandArgument.Split('|')[0];
            string idUsuario = Session["idUser"].ToString();

            string cambio = "42186"; //LIBERADO ESC

            cambiarEstatusLiberado(idPrograma_Alumno, idUsuario, cambio);
            //string searchTerm = txtBusqueda.Text.Trim();
            string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            int page = CurrentPage;
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(programa) || string.IsNullOrEmpty(nombre))
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, "", "", "");
            }
            else
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, matricula, programa, nombre);
            }

        }
        protected void btnEliminar_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            LinkButton lnkUpdate = (LinkButton)sender;
            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
            string idUsuario = Session["tipoUsuario"].ToString();
            //string idDependencia = lnkUpdate.CommandArgument.Split('|')[0];
            string idPrograma_Alumno = lnkUpdate.CommandArgument.Split('|')[1]; 
            //string estatusProgramaAlumno = obtenerEstatusPrograma(idPrograma_Alumno);
            string cambio ="";

            string tipoUsuario = Session["tipoUsuario"].ToString();
            if (tipoUsuario == "2") // USUARIO DEPENDENCIA
            {
                cambio = "22113"; //NO AUTORIZADO POR DEPENDENCIA

            }
            else if (tipoUsuario == "4") //USUARIO ENCARGADO DE ESCUELA
            {
                cambio = "22114"; //NO AUTORIZADO POR ENCARGADO ESCUELA
            }
            //HashSet<string> estatusActivos = new HashSet<string> { "20707","21522","21523" };

            if (tipoUsuario == "1")
            {
                //if (estatusActivos.Contains(estatusProgramaAlumno))
                //{
                    cambio = "7";
                //}
            }
           
            cambiarEstatus( idPrograma_Alumno, idUsuario, cambio);
            mensajeScript("Registrado No Autorizado con éxito");

            //string searchTerm = txtBusqueda.Text.Trim();
             string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            int page = CurrentPage;
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(programa) || string.IsNullOrEmpty(nombre))
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, "", "", "");
            }
            else
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, matricula,programa, nombre);
            }
        }
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            LinkButton lnkUpdate = (LinkButton)sender;
            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
            string idUsuario = Session["tipoUsuario"].ToString();
            //string idDependencia = lnkUpdate.CommandArgument.Split('|')[0];
            string idPrograma_Alumno = lnkUpdate.CommandArgument.Split('|')[1];
            //string idAlumno = lnkUpdate.CommandArgument.Split('|')[2];
            string cambio = "";
            string tipoUsuario = Session["tipoUsuario"].ToString();
            if (tipoUsuario == "2") // USUARIO DEPENDENCIA
            {
                cambio = "21522"; //AUTORIZADO POR DEPENDENCIA

            }
            else
            if (tipoUsuario == "4") //USUARIO ENCARGADO DE ESCUELA
            {
                cambio = "21523"; //AUTORIZADO POR ENCARGADO ESCUELA
            }
            if (tipoUsuario == "1")
            {
                //if (estatusActivos.Contains(estatusProgramaAlumno))
                //{
                cambio = "21522";
                //}
            }
            // tipo_correo = "5";
            cambiarEstatus( idPrograma_Alumno, idUsuario, cambio);
            mensajeScript("Registrado Autorizado con éxito");

            //string searchTerm = txtBusqueda.Text.Trim();
            string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            int page = CurrentPage;
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(programa) || string.IsNullOrEmpty(nombre))
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, "", "", "");
            }
            else
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, matricula, programa, nombre);
            }
        }
       protected void btnBuscar_Click(object sender, EventArgs e)
        {

            // Obtén el término de búsqueda del cuadro de texto
            //string searchTerm = txtBusqueda.Text.Trim();
            string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            //CargarDatos(CurrentPage, searchTerm);

            CargarDatos(CurrentPage, matricula, programa, nombre);



          
        }
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.Trim();
            string programa= txtPrograma.Text.Trim();
            string nombre =txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(matricula))
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatos(CurrentPage, "","","");
                }
            }
            else
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatos(CurrentPage, matricula,programa,nombre);
                }
            }

            //string searchTerm = txtBusqueda.Text.Trim();
            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
            //    {
            //        CurrentPage += 1;
            //        CargarDatos(CurrentPage, "");
            //    }
            //}
            //else
            //{
            //    if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
            //    {
            //        CurrentPage += 1;
            //        CargarDatos(CurrentPage, searchTerm);
            //    }
            //}
        }
        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.Trim();
            string programa = txtPrograma.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(matricula))
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatos(CurrentPage, "","","");
                }
            }
            else
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatos(CurrentPage, matricula, programa, nombre);
                }
            }


            //string searchTerm = txtBusqueda.Text.Trim();
            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    if (CurrentPage > 0)
            //    {
            //        CurrentPage -= 1;
            //        CargarDatos(CurrentPage, "");
            //    }
            //}
            //else
            //{
            //    if (CurrentPage > 0)
            //    {
            //        CurrentPage -= 1;
            //        CargarDatos(CurrentPage, searchTerm);
            //    }
            //}


        }

        protected void btnEvaluar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string nst = btn.CommandArgument;
            Response.Redirect("EvaluacionEstudiante.aspx?nst=" + nst);
        }
        #endregion
    }
}

