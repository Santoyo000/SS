using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http.Results;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iText.Signatures.LtvVerification;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Windows.Controls;
using System.IO;
using System.Text;


namespace Servicio_Social
{
    public partial class SeleccionarPrograma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipoUsuario"] == null)
            {
                Response.Redirect("LoginEstudiante.aspx");
            }
            else
            {
                try
                {
                    string id = Session["tipoUsuario"].ToString().Split('|')[2];
                }
                catch
                {
                    Response.Redirect("SeleccionPlan.aspx");
                }
            }
            if (!IsPostBack)
            {
                CargarDatos(0, "");
                CargarConfiguracion();
                //cargarPlan();
            }
        }
        protected int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 0; }
            set { ViewState["CurrentPage"] = value; }
        }
        private int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
        }
        private void CargarConfiguracion()
        {
            string connectionString = GlobalConstants.SQL; // Asegúrate de reemplazar esto con tu cadena de conexión real

            // Query para obtener la configuración específica para Registro de Dependencias
            string query = "SELECT bActivo, dFechaInicio, dFechaFin, sMensaje FROM SP_CONFIGURACION_PAG_SS WHERE sClave = '4'";

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
                            PanelProgramas.Visible = dentroDelRango;
                            PanelCerrado.Visible = !dentroDelRango;
                            // Asignar el mensaje al control h3 del frontend
                            lblMensajeProgramas.Text = mensaje;
                        }
                    }
                }
            }
        }
        #region Botones Paginado
        //private int CurrentPage
        //{
        //    get
        //    {
        //        return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 0;
        //    }
        //    set
        //    {
        //        ViewState["CurrentPage"] = value;
        //    }
        //}
        //private int TotalPages
        //{
        //    get
        //    {
        //        return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0;
        //    }
        //    set
        //    {
        //        ViewState["TotalPages"] = value;
        //    }
        //}
       
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            string searchTerm = txtDependencias.Text.Trim();
            if (CurrentPage > 0)
            {
                CurrentPage -= 1;
                CargarDatos(CurrentPage, searchTerm);
            }
            //string searchTerm = txtDependencias.Text.Trim();
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
        protected void btnNext_Click(object sender, EventArgs e)
        {
            string searchTerm = txtDependencias.Text.Trim();
            
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage += 1;
                CargarDatos(CurrentPage, searchTerm);
            }
        }
        #endregion

        #region Llenar Grid
        //protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        //{
        //    string conString = GlobalConstants.SQL;
        //    int rowsToSkip = pageIndex * pageSize;
        //    string idAlumno = Session["tipoUsuario"].ToString().Split('|')[2];
        //    string nivel = obtenerNivelAcad(idAlumno);
        //    string unidad = obtenerUnidad(idAlumno).Split('|')[0];
        //    string escuela = obtenerUnidad(idAlumno).Split('|')[1];
        //    string plan = Session["plan"].ToString().Split('-')[1].Trim();

        //    DataTable dt = new DataTable();
        //    totalRecords = 0;

        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        // Llamada al procedimiento almacenado para contar registros
        //        SqlCommand countCmd = new SqlCommand("sp_ContarProgramasElegirAlumno_ss", con);
        //        countCmd.CommandType = CommandType.StoredProcedure;
        //        countCmd.Parameters.AddWithValue("@nivel", nivel);
        //        countCmd.Parameters.AddWithValue("@unidad", unidad);
        //        countCmd.Parameters.AddWithValue("@escuela", escuela);
        //        countCmd.Parameters.AddWithValue("@plan", plan);
        //        if (!string.IsNullOrEmpty(searchTerm))
        //        {
        //            countCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
        //        }

        //        con.Open();

        //        // Obtener el número total de registros
        //        totalRecords = (int)countCmd.ExecuteScalar();

        //        // Llamada al procedimiento almacenado para obtener los datos paginados
        //        SqlCommand cmd = new SqlCommand("sp_obtenerProgramasElegirAlumno_ss", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@nivel", nivel);
        //        cmd.Parameters.AddWithValue("@unidad", unidad);
        //        cmd.Parameters.AddWithValue("@escuela", escuela);
        //        cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
        //        cmd.Parameters.AddWithValue("@plan", plan);
        //        cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
        //        cmd.Parameters.AddWithValue("@pageSize", pageSize);
        //        if (!string.IsNullOrEmpty(searchTerm))
        //        {
        //            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
        //        }

        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dt);
        //    }

        //    return dt;

        //}
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;
            string idAlumno = Session["tipoUsuario"].ToString().Split('|')[2];
            string nivel = obtenerNivelAcad(idAlumno);
            string unidad = obtenerUnidad(idAlumno).Split('|')[0];
            string escuela = obtenerUnidad(idAlumno).Split('|')[1];
            string plan = Session["plan"].ToString().Split('-')[1].Trim();

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // 🔹 1. Contar registros
                string countQuery = @"
            SELECT COUNT(DISTINCT DP.idDetallePrograma)
            FROM SM_PROGRAMA P
            INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.kmPrograma = P.idPrograma
            INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
            INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = DP.kpEscuela
            INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = DP.kpPlanEstudio
            INNER JOIN SP_CICLO AS CICL ON P.kpPeriodo = CICL.idCiclo
            WHERE PE.kpNivel = @nivel 
              AND UO.kpUnidad = @unidad 
              AND DP.kpEscuela = @escuela 
              AND P.kpEstatus_Programa = 11
              AND CICL.bServicioSocial = 1 ";

                if (nivel == "2")
                {
                    countQuery += @"AND (PE.sDescripcion LIKE @plan
                              OR PE.sDescripcion LIKE REPLACE(@plan,'INGENIERO','INGENIERÍA')
                              OR PE.sDescripcion LIKE REPLACE(@plan,'INGENIERÍA','INGENIERO')
                              OR PE.sDescripcion LIKE REPLACE(@plan,'LICENCIADO','LICENCIATURA')
                              OR PE.sDescripcion LIKE REPLACE(@plan,'LICENCIATURA','LICENCIADO')) ";
                }
                else
                {
                    countQuery += "AND PE.sDescripcion LIKE '%BACHILLERATO%' ";
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    countQuery += @"AND (
                               DS.sDescripcion LIKE @searchTerm 
                            OR P.sNombre_Programa LIKE @searchTerm 
                            OR (UO.sClave + ' - ' + UO.sDescripcion) LIKE @searchTerm
                            OR (PE.sClave + ' - ' + PE.sDescripcion) LIKE @searchTerm
                         ) ";
                }

                using (SqlCommand countCmd = new SqlCommand(countQuery, con))
                {
                    countCmd.Parameters.AddWithValue("@nivel", nivel);
                    countCmd.Parameters.AddWithValue("@unidad", unidad);
                    countCmd.Parameters.AddWithValue("@escuela", escuela);
                    countCmd.Parameters.AddWithValue("@plan", "%" + plan + "%");

                    if (!string.IsNullOrEmpty(searchTerm))
                        countCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    totalRecords = (int)countCmd.ExecuteScalar();
                }

                // 🔹 2. Obtener registros paginados
                string dataQuery = @"
            SELECT DP.idDetallePrograma, 
                   DS.sDescripcion AS Dependencia, 
                   P.sNombre_Programa, 
                   UO.sClave + ' - ' + UO.sDescripcion AS Escuela, 
                   PE.sClave + ' - ' + PE.sDescripcion AS Planes,  
                   DP.iCupo,  
                   DP.iCupo - COUNT(CASE WHEN PA.kpEstatus IN (20707,21522,21523,21526) THEN 1 END) AS CuposDisponibles,
                   (SELECT TOP 1 PA2.kpEstatus 
                    FROM SM_PROGRAMA_ALUMNO PA2 
                    WHERE PA2.kmDetallePrograma = DP.idDetallePrograma AND PA2.kmAlumno = @idAlumno) AS Inscrito
            FROM SM_PROGRAMA P  
            INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.kmPrograma = P.idPrograma  
            INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia  
            INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = DP.kpEscuela  
            INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = DP.kpPlanEstudio  
            LEFT JOIN SM_PROGRAMA_ALUMNO PA ON PA.kmDetallePrograma = DP.idDetallePrograma
            INNER JOIN SP_CICLO AS CICL ON P.kpPeriodo = CICL.idCiclo
            WHERE PE.kpNivel = @nivel 
              AND UO.kpUnidad = @unidad 
              AND DP.kpEscuela = @escuela 
              AND P.kpEstatus_Programa= 11  
              AND CICL.bServicioSocial = 1 ";

                if (nivel == "2")
                {
                    dataQuery += @"AND (PE.sDescripcion LIKE @plan
                             OR PE.sDescripcion LIKE REPLACE(@plan,'INGENIERO','INGENIERÍA')
                             OR PE.sDescripcion LIKE REPLACE(@plan,'INGENIERÍA','INGENIERO')
                             OR PE.sDescripcion LIKE REPLACE(@plan,'LICENCIADO','LICENCIATURA')
                             OR PE.sDescripcion LIKE REPLACE(@plan,'LICENCIATURA','LICENCIADO')) ";
                }
                else
                {
                    dataQuery += "AND PE.sDescripcion LIKE '%BACHILLERATO%' ";
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    dataQuery += @"AND (
                               DS.sDescripcion LIKE @searchTerm 
                            OR P.sNombre_Programa LIKE @searchTerm 
                            OR (UO.sClave + ' - ' + UO.sDescripcion) LIKE @searchTerm
                            OR (PE.sClave + ' - ' + PE.sDescripcion) LIKE @searchTerm
                         ) ";
                }

                dataQuery += @"GROUP BY DP.idDetallePrograma, DS.sDescripcion, 
                               P.sNombre_Programa, UO.sClave, UO.sDescripcion, 
                               PE.sClave, PE.sDescripcion, DP.iCupo 
                        ORDER BY DP.iCupo DESC
                        OFFSET @rowsToSkip ROWS FETCH NEXT @pageSize ROWS ONLY;";

                using (SqlCommand cmd = new SqlCommand(dataQuery, con))
                {
                    cmd.Parameters.AddWithValue("@nivel", nivel);
                    cmd.Parameters.AddWithValue("@unidad", unidad);
                    cmd.Parameters.AddWithValue("@escuela", escuela);
                    cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@plan", "%" + plan + "%");
                    cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    if (!string.IsNullOrEmpty(searchTerm))
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        private void BindPagination()
        {
            List<object> pagination = new List<object>();
            int maxPagesToShow = 10; // Máximo de números a mostrar en la paginación

            int startPage = Math.Max(0, CurrentPage - (maxPagesToShow / 2));
            int endPage = Math.Min(TotalPages, startPage + maxPagesToShow);

            for (int i = startPage; i < endPage; i++)
            {
                pagination.Add(new { PageNumber = i + 1, PageIndex = i });
            }

            rptPagination.DataSource = pagination;
            rptPagination.DataBind();

            lblTotalPages.Text = TotalPages.ToString();

            // Habilita o deshabilita los botones de anterior y siguiente
            btnPrevious.Enabled = CurrentPage > 0;
            btnNext.Enabled = CurrentPage < TotalPages - 1;
        }
        protected void rptPagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "PageChange")
            {
                // Convertir el argumento de la página seleccionada
                int newPage;
                if (int.TryParse(e.CommandArgument.ToString(), out newPage))
                {
                    CurrentPage = newPage; // Actualiza la página actual
                    string searchTerm = txtDependencias.Text.Trim();
                  

                    // Recargar los datos con los filtros actuales
                    CargarDatos(CurrentPage, searchTerm);
                }
            }
        }
        protected void CargarDatos(int pageIndex, string searchTerm)
        {
            //int pageSize = 10; // Cantidad de registros por página
            //int totalRecords;

            //DataTable dt = ObtenerDatos(pageIndex, pageSize, searchTerm, out totalRecords);

            //RepeaterProgramas.DataSource = dt;
            //RepeaterProgramas.DataBind();

            //// Calcula el número total de páginas
            //TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            //// Configura el estado de los botones
            //btnPrevious.Enabled = pageIndex > 0;
            //btnNext.Enabled = pageIndex < TotalPages - 1;

            //// Actualiza la etiqueta de número de página
            //lblPageNumber.Text = $"Página {pageIndex + 1} de {TotalPages}";
            int pageSize = 10; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatos(pageIndex, pageSize, searchTerm, out totalRecords);

            RepeaterProgramas.DataSource = dt;
            RepeaterProgramas.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPrevious.Enabled = pageIndex > 0;
            btnNext.Enabled = pageIndex < TotalPages - 1;

            // Etiquetas de paginación
            lblPageNumber.Text = (pageIndex + 1).ToString();
            lblTotalPages.Text = TotalPages.ToString();

            // 🔹 Actualiza la paginación
            BindPagination();
        }
        protected void RepeaterProgramas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                LinkButton btnSeleccionar = (LinkButton)e.Item.FindControl("btnSeleccionar");
                LinkButton btnAnular = (LinkButton)e.Item.FindControl("btnAnular");
                string idAlumno = Session["tipoUsuario"].ToString().Split('|')[2];
                int cupo = Int32.Parse(row["CuposDisponibles"].ToString().Trim());
                string programaSeleccionado = "";
                if (TieneProgramaSeleccionado(idAlumno)) //Programa seleccionado sin importar el estatus
                {
                    programaSeleccionado = "1";
                }

                switch (programaSeleccionado)
                {
                    case "1": //SELECCIONADO
                        btnSeleccionar.Visible = false;
                        btnAnular.Visible = false;
                        break;

                    default:
                        btnSeleccionar.Visible = true;
                        btnAnular.Visible = false;
                        break;
                }

                if (cupo == 0)
                {
                    btnSeleccionar.Visible = false;
                    btnAnular.Visible = false;
                }
            }
        }
        #endregion

        #region Llenar Modal
        [WebMethod]
        public static string llenarDatosModal(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string result = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_obtenerInformacionProgramaModal_ss", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        result += $"<p class='small-text'><strong>Dependencia:</strong> {reader["Dependencia"]}</p>";
                        result += $"<p class='small-text'><strong>Teléfono:</strong> {reader["sTelefono"]}</p>";
                        result += $"<p class='small-text'><strong>Domicilio:</strong> {reader["sDomicilio"]}</p>";
                        result += $"<p class='small-text'><strong>Programa:</strong> {reader["sNombre_Programa"]}</p>";
                        result += $"<p class='small-text'><strong>Responsable:</strong> {reader["sResponsable"]}</p>";
                        result += $"<p class='small-text'><strong>Area Responsable:</strong> {reader["sAreaResponsable"]}</p>";
                        result += $"<p class='small-text'><strong>Objetivos:</strong> {reader["sObjetivos"]}</p>";
                        result += $"<p class='small-text'><strong>Actividades:</strong> {reader["sActividades_desarollo"]}</p>";
                        result += $"<p class='small-text'><strong>Horario:</strong> {reader["sHorario"]}</p>";

                        string mapaUrl = reader["slinkMaps"].ToString();
                        result += $"<p class='small-text'><strong>Mapa:</strong> <a href='{mapaUrl}' target='_blank'>Ver en Google Maps</a></p>";
                    }
                    con.Close();
                }
            }

            return result;
        }
        #endregion

        #region Métodos obtener Datos de Variables 
        public string obtenerNivelAcad(string id)
        {
            string nivel = "";
            string query = "SELECT PE.kpNivel " +
                "FROM SM_USUARIO U " +
                "INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.kmUsuario = U.idUsuario " +
                "INNER JOIN SP_PLAN_ESTUDIO PE ON PE.idPlanEstudio = UA.kpPlan " +
                "WHERE UA.ID = @id; ";
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        nivel = reader["kpNivel"].ToString();
                    }
                    con.Close();
                }
            }
            return nivel;
        }

        public int obteneridProgramaAlumno(string idPrograma, string idAlumno)
        {
            int id = 0;
            string query = "SELECT PA.idProgramaAlumno " +
                "FROM SM_PROGRAMA_ALUMNO PA " +
                "WHERE PA.kmDetallePrograma = @kmDetallePrograma AND PA.kmAlumno = @kmAlumno ";
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@kmDetallePrograma", idPrograma);
                    cmd.Parameters.AddWithValue("@kmAlumno", idAlumno);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        id = (int)Convert.ToInt32(reader["idProgramaAlumno"]);
                    }
                    con.Close();
                }
            }
            return id;
        }
        
        public bool TieneProgramaSeleccionado(String idAlumno)
        {
            string connectionString = GlobalConstants.SQL;
            bool tieneSeleccionado = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ConsultarAlumnoProgramaSeleccionado_ss", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idAlumno", idAlumno);

                    con.Open();
                    tieneSeleccionado = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                }
            }

            return tieneSeleccionado;
        }

        public string obtenerUnidad(String idAlumno)
        {
            string unidad = "";
            string connectionString = GlobalConstants.SQL;
            string query = "SELECT UO.kpUnidad,UO.idEscuelaUAC " +
                "FROM SM_USUARIOS_ALUMNOS UA " +
                "INNER JOIN SM_ALUMNO AL ON AL.idAlumno = UA.kmAlumno " +
                "INNER JOIN SP_ESCUELA_UAC UO ON UO.idEscuelaUAC = AL.kpEscuelasUadeC " +
                "WHERE UA.ID = @idAlumno ";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string kpUnidad = reader["kpUnidad"].ToString();
                        string idEscuelaUAC = reader["idEscuelaUAC"].ToString();
                        unidad = kpUnidad + '|' + idEscuelaUAC;
                    }
                    con.Close();
                }
            }
            return unidad;
        }

        public bool cuposDisponibles(string idPrograma, SqlConnection connection, SqlTransaction transaction)
        {
            bool flag = false;

            using (SqlCommand cmd = new SqlCommand("sp_ConsultarCuposDisponibles_ss", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idDetallePrograma", idPrograma);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Si el lector devuelve filas, significa que hay cupos disponibles
                    if (reader.Read())
                    {
                        flag = true;
                    }
                }
            }

            return flag;
        }

        public string obtenerCorreoDependencia(string idDetallePrograma)
        {
            string correo = "";
            string query = @"SELECT U.sCorreo
         FROM SM_DETALLE_PROGRAMA DP
         INNER JOIN SM_PROGRAMA P ON P.idPrograma = DP.kmPrograma
         INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.idDependenicaServicio = P.kpDependencia
         INNER JOIN SM_USUARIO U ON U.idUsuario = DS.kmUsuario
         WHERE DP.idDetallePrograma = @idDetallePrograma ";
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idDetallePrograma", idDetallePrograma);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        correo = reader["sCorreo"].ToString();
                    }
                    con.Close();
                }
            }
            return correo;
        }

        #endregion

        #region Botones
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtDependencias.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatos(CurrentPage, searchTerm);
        }
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdate = (LinkButton)sender;
            string idPrograma = lnkUpdate.CommandArgument.Split('|')[0];
            string idAlumno = Session["tipoUsuario"].ToString().Split('|')[2];
            string estatus = "20707";
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

                try
                {
                    string flag = lnkUpdate.CommandArgument.Split('|')[1];
                    // Verifica si hay cupos disponibles antes de proceder
                    if (cuposDisponibles(idPrograma, connection, transaction))
                    {
                        if (!string.IsNullOrEmpty(flag))
                        {
                            int id = obteneridProgramaAlumno(idPrograma, idAlumno);
                            insertarBitacora(id, idAlumno, estatus, connection, transaction);
                            updateProgramaAlumno(id, estatus, connection, transaction);
                            transaction.Commit();
                            mensajeScript("Se ha registrado exitosamente en el programa.");
                        }
                        else
                        {
                            int idProgramaAlumno = insertarProgramaAlumno(idPrograma, idAlumno, connection, transaction);
                            insertarBitacora(idProgramaAlumno, idAlumno, estatus, connection, transaction);
                            transaction.Commit();
                            enviarCorreo(obtenerCorreoDependencia(idPrograma), "23");
                            mensajeScript("Se ha registrado exitosamente en el programa.");
                        }
                    }
                    else
                    {
                        // Si no hay cupos disponibles, no realizar la inserción
                        transaction.Rollback();
                        mensajeScript("No quedan cupos disponibles para este programa.");
                    }

                }
                catch (Exception ex)
                {
                    // Si ocurre algún error, realizar un rollback de la transacción
                    transaction.Rollback();

                    // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                    Response.Write("Error: " + ex.Message);
                }
            }

            string searchTerm = txtDependencias.Text.Trim();
            int page = CurrentPage;
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, "");
            }
            else
            {
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, searchTerm);
            }
        }
        #endregion

        #region Metodos insert Base Datos
        private int insertarProgramaAlumno(string idPrograma, string idAlumno, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_PROGRAMA_ALUMNO (kmDetallePrograma, kmAlumno, dFechaRegistro, kpEstatus) " +
                "VALUES (@kmDetallePrograma, @kmAlumno, GETDATE(), 20707);  SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kmDetallePrograma", idPrograma);
                cmd.Parameters.AddWithValue("@kmAlumno", idAlumno);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void insertarBitacora(int idProgramaAlumno, string idAlumno, string estatus, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_BITACORA_PA (kmProgramaAlumno, kpEstatus, kpUsuario) " +
                "VALUES (@kmProgramaAlumno, @kpEstatus, @kpUsuario);";


            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kmProgramaAlumno", idProgramaAlumno);
                cmd.Parameters.AddWithValue("@kpEstatus", estatus);
                cmd.Parameters.AddWithValue("@kpUsuario", idAlumno);
                cmd.ExecuteNonQuery();
            }
        }

        public void updateProgramaAlumno(int idProgramaAlumno, string estatus, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "UPDATE SM_PROGRAMA_ALUMNO SET kpEstatus = @kpEstatus WHERE idProgramaAlumno = @kmProgramaAlumno; ";
            
            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kmProgramaAlumno", idProgramaAlumno);
                cmd.Parameters.AddWithValue("@kpEstatus", estatus);
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Eventos 
        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }

        public void enviarCorreo(string email, string tipo)
        {
            MensajesCorreo mc = new MensajesCorreo();
            string body = mc.tipoTexto(tipo);
            string to = email; // "panssh@hotmail.com";
            string from = "noreply@uadec.edu.mx";
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            //message.To.Add(); //correo al administrador (por definir)
            message.Subject = "Alumno registrado en Programa de Servicio Social | UAdeC";
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

        
        #endregion
    }
}