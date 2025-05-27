using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class AlumnosRegistrados : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["idUser"] == null || Session["idDependencia"] == null)
            //{
            //    Response.Redirect("Home.aspx");
            //}
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
        public class AlumnosReg
        {
            public DateTime FechaRegistro { get; set; }
            public string Matricula { get; set; }
            public string Alumno { get; set; }
            public string Correo { get; set; }
            public string PlanEstudios { get; set; }
            public string Escuela { get; set; }
            public string Estatus { get; set; }
        }
        #region Operaciones
        protected void CargarDatos(int pageIndex, string matricula, string nombre, string correo)
        {
            int pageSize = 20; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatos(pageIndex, pageSize, matricula, nombre, correo,
                                        ddlEstatus.SelectedValue, DDLUnidad.SelectedValue,
                                        ddlNivel.SelectedValue, ddlEscuela.SelectedValue,
                                        ddlPlan.SelectedValue, ddlPeriodo.SelectedValue, out totalRecords);

            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPrevious.Enabled = pageIndex > 0;
            btnNext.Enabled = pageIndex < TotalPages - 1;

            lblPageNumber.Text = (pageIndex + 1).ToString(); // Solo el número de página
            lblTotalPages.Text = TotalPages.ToString();

            // 🔹 Actualiza la paginación después de cargar datos
            BindPagination();
        }
        //protected DataTable ObtenerDatos(int pageIndex, int pageSize, string matricula, string nombre, string correo, string selectedEstatus, string selectedUnidad, string selectedNivel, string selectedEscuela, string selectedPlan, string selectedPeriodo, out int totalRecords)
        //{
        //    string conString = GlobalConstants.SQL;
        //    int rowsToSkip = pageIndex * pageSize;

        //    string filtros = Session["filtros"].ToString();
        //    string tipoUsuario = filtros.Split('|')[0];
        //    string unidadUsuario = filtros.Split('|')[1];
        //    string Escuela = tipoUsuario == "4" ? filtros.Split('|')[2] : null;
        //    DataTable dt = new DataTable();
        //    totalRecords = 0;

        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        con.Open();

        //        // Llamada al SP para contar el total de registros
        //        using (SqlCommand countCmd = new SqlCommand("sp_ContarAlumnosRegistrados_ss", con))
        //        {
        //            countCmd.CommandType = CommandType.StoredProcedure;
        //            countCmd.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);
        //            countCmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
        //            countCmd.Parameters.AddWithValue("@Escuela", Escuela ?? (object)DBNull.Value);
        //            countCmd.Parameters.AddWithValue("@searchTerm", !string.IsNullOrEmpty(searchTerm) ? $"%{searchTerm}%" : (object)DBNull.Value);

        //            totalRecords = (int)countCmd.ExecuteScalar();
        //        }

        //        // Llamada al SP para obtener los datos paginados
        //        using (SqlCommand cmd = new SqlCommand("sp_ObtenerAlumnosRegistrados_ss", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
        //            cmd.Parameters.AddWithValue("@pageSize", pageSize);
        //            cmd.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);
        //            cmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
        //            cmd.Parameters.AddWithValue("@Escuela", Escuela ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@searchTerm", !string.IsNullOrEmpty(searchTerm) ? $"%{searchTerm}%" : (object)DBNull.Value);

        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            adapter.Fill(dt);
        //        }
        //    }

        //    return dt;

        //}
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string matricula, string nombre, string correo, string selectedEstatus, string selectedUnidad, string selectedNivel, string selectedEscuela, string selectedPlan, string selectedPeriodo, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
           
           

            string filtroquery = "";

            string baseQuery = @" FROM SM_USUARIOS_ALUMNOS AS USA 
                                    INNER JOIN SM_USUARIO AS U ON USA.kmUsuario = U.idusuario
                                    INNER JOIN SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno 
                                    INNER JOIN NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona 
                                    INNER JOIN SP_ESCUELA_UAC AS ESC ON USA.kpEscuela = ESC.idEscuelaUAC 
                                    INNER JOIN SP_PLAN_ESTUDIO AS PLA ON USA.kpPlan = PLA.idPlanEstudio 
                                    INNER JOIN NP_ESTATUS AS EST ON USA.bAutorizado = EST.idEstatus
                                    INNER JOIN NP_UNIDAD AS UN ON ESC.KPUNIDAD = UN.IDUNIDAD
                                    INNER JOIN SP_TIPO_NIVEL AS NIV ON PLA.kpNivel = NIV.idTipoNivel
                                    INNER JOIN SP_CICLO AS CIC ON USA.kpPeriodo = CIC.idCiclo";
            List<string> F = new List<string>();

            // Filtros generales
            if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                F.Add(" ESC.KPUNIDAD = @selectedUnidad");
            if (!string.IsNullOrEmpty(selectedNivel) && selectedNivel != "0")
                F.Add(" PLA.kpNivel= @selectedNivel");
            if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                F.Add(" USA.bAutorizado = @selectedEstatus");
            if (!string.IsNullOrEmpty(selectedEscuela) && selectedEscuela != "0")
                F.Add(" ALU.kpEscuelasUadeC = @selectedEscuela");
            if (!string.IsNullOrEmpty(selectedPlan) && selectedPlan != "0")
                F.Add(" ALu.kpPlan_estudios = @selectedPlan");
            if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                F.Add(" USA.kpPeriodo = @selectedPeriodo");
            if (!string.IsNullOrEmpty(matricula))
                F.Add(" ALU.sMatricula LIKE @matricula");
            if (!string.IsNullOrEmpty(correo))
                F.Add(" U.sCorreo LIKE @correo");
            if (!string.IsNullOrEmpty(nombre))
                F.Add(" PER.sNombre_completo LIKE @nombre");

            if (tipoUsuario == "3") // USUARIO RESPONSABLE
            {
                string unidadUsuario = filtros.Split('|')[1];
                // Agregar filtro para unidad del usuario responsable
                F.Add(" ESC.kpUnidad = @unidadUsuario");
            }
            if (tipoUsuario == "4") // USUARIO ENCARGADO DE ESCUELA
            {
                string Escuela = filtros.Split('|')[2];
                // Agregar filtro para unidad del usuario responsable
                F.Add(" USA.kpEscuela = @Escuela");
            }

            // Construcción de la cláusula WHERE final
            string filtroQuery = F.Count > 0 ? " AND " + string.Join(" AND ", F) : "";

            string query = $@"SELECT USA.ID, CIC.sDescripcion AS Periodo,
                               CONVERT(varchar, U.dFechaRegistro, 103) AS dFechaRegistro, 
                               U.sCorreo AS Correo, 
                               ALU.sMatricula AS Matricula, 
                               PER.sNombre_completo AS Alumno, 
                               PLA.sClave + ' - ' + PLA.sDescripcion AS PlanEstudio,
                               ESC.sClave + ' - ' + ESC.sDescripcion AS Escuela,  
                               UN.sCiudad AS UNIDAD, 
                               EST.sDescripcion AS EstadoAutorizacion,
                               EST.idEstatus
                              {baseQuery} {filtroQuery}
                              ORDER BY USA.bAutorizado DESC
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

                    if (!string.IsNullOrEmpty(correo))
                    {
                        cmd.Parameters.AddWithValue("@correo", $"%{correo}%");
                        countCmd.Parameters.AddWithValue("@correo", $"%{correo}%");
                    }
                    if (!string.IsNullOrEmpty(nombre))
                    {
                        cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
                        countCmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
                    }


                    if (tipoUsuario == "3") // Agregar parámetro para Responsable
                    {
                        
                        string unidadUsuario = filtros.Split('|')[1];
                        cmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                        countCmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                    }

                    if (tipoUsuario == "4") // Agregar parámetro para Encargado de Escuela
                    {
                        
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
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            if (CurrentPage > 0)
            {
                CurrentPage -= 1;
                CargarDatos(CurrentPage, matricula, nombre, correo);
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage += 1;
                CargarDatos(CurrentPage, matricula, nombre, correo);
            }
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
                    string matricula = txtMatricula.Text.Trim();
                    string correo = txtCorreo.Text.Trim();
                    string nombre = txtNombre.Text.Trim();

                    // Recargar los datos con los filtros actuales
                    CargarDatos(CurrentPage, matricula, nombre, correo);
                }
            }
        }
        private void CargarEstatus()
        {
            string query = "SELECT idEstatus,sClave, sDescripcion  FROM NP_ESTATUS WHERE sClave IN ('11','23','2') ORDER BY sDescripcion"; // Ajusta la condición según tu criterio
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
                newRow["sCiudad"] = "Seleccione la Unidad...";
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
                newRow["Descripcion"] = "Seleccione el Plan de Estudios...";
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
                newRow["Descripcion"] = "Seleccione la Escuela...";
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
                newRow["sDescripcion"] = "Seleccione el Periodo Escolar...";
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
        protected void cambiarEstatus(string id, string cambio)
        {
            string idUser = "";
            if (Session["idUser"] != null)
                idUser = Session["idUser"].ToString();


            string connectionString = GlobalConstants.SQL;
            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_CambiarEstatusAlumno_ss", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@bAutorizado", cambio);
                    command.Parameters.AddWithValue("@kmAutorizo", idUser);

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
        #endregion
        #region Botones
        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            // Limpiar los TextBox
            txtMatricula.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;

            // Restablecer los DropDownList al primer valor (si tienen un "Seleccione...")
            ddlEstatus.ClearSelection();
            if (ddlEstatus.Items.Count > 0) ddlEstatus.SelectedIndex = 0;

            DDLUnidad.ClearSelection();
            if (DDLUnidad.Items.Count > 0) DDLUnidad.SelectedIndex = 0;

            ddlNivel.ClearSelection();
            if (ddlNivel.Items.Count > 0) ddlNivel.SelectedIndex = 0;

            ddlPlan.ClearSelection();
            if (ddlPlan.Items.Count > 0) ddlPlan.SelectedIndex = 0;

            ddlEscuela.ClearSelection();
            if (ddlEscuela.Items.Count > 0) ddlEscuela.SelectedIndex = 0;

            ddlPeriodo.ClearSelection();
            if (ddlPeriodo.Items.Count > 0) ddlPeriodo.SelectedIndex = 0;
        }
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;
            LinkButton lnkUpdate = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string ID = valores[0];
            string sCorreo = valores[1];

            string cambio;
            string tipo_correo;
            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            cambio = "11";
            tipo_correo = "5";
            cambiarEstatus(ID, cambio);
            enviarCorreo(sCorreo, tipo_correo);
            mensajeScript("Registrado Autorizado con éxito");

            //string searchTerm = txtBusqueda.Text.Trim();
            int page = CurrentPage;
            string matricula = txtMatricula.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            // Recargar los datos con los filtros actuales
            CargarDatos(CurrentPage, matricula, nombre, correo);

            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, "");
            //}
            //else
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, searchTerm);
            //}
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {

            LinkButton lnkUpdate = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string ID = valores[0];
            string sCorreo = valores[1];

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string cambio = "2";
            string tipo_correo = "6";

            cambiarEstatus(ID, cambio);
            enviarCorreo(sCorreo, tipo_correo);
            mensajeScript("Registrado NO Autorizado con éxito");

            string matricula = txtMatricula.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            // Recargar los datos con los filtros actuales
            CargarDatos(CurrentPage, matricula, nombre, correo);
            //string searchTerm = txtBusqueda.Text.Trim();
            //int page = CurrentPage;
            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, "");
            //}
            //else
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, searchTerm);
            //}
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            LinkButton lnkEliminar = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)lnkEliminar.NamingContainer;
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string ID = valores[0];
            string sCorreo = valores[1];

            string cambio = "99";
            //string tipo_correo = "5";
            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            cambiarEstatus(ID, cambio);
            //enviarCorreo(sCorreo, tipo_correo);
            mensajeScript("Alumno eliminado con éxito");

            string matricula = txtMatricula.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            // Recargar los datos con los filtros actuales
            CargarDatos(CurrentPage, matricula, nombre, correo);
            //string searchTerm = txtBusqueda.Text.Trim();
            //int page = CurrentPage;
            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, "");
            //}
            //else
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, searchTerm);
            //}
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
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
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportToExcelAlumnos();
        }
        private void ExportToExcelAlumnos()
        {
            // 🔥 Obtén los datos de Alumnos Registrados
            DataTable dt = ObtenerDatosExportacionAlumnos(
                txtMatricula.Text.Trim(),
                txtNombre.Text.Trim(),
                txtCorreo.Text.Trim(),
                ddlEstatus.SelectedValue,
                DDLUnidad.SelectedValue,
                ddlNivel.SelectedValue,
                ddlPlan.SelectedValue,
                ddlEscuela.SelectedValue,
                ddlPeriodo.SelectedValue
            );

            if (dt.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AlumnosRegistrados.xls");
                Response.Charset = "utf-8";
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = Encoding.UTF8;

                StringBuilder sb = new StringBuilder();
                sb.Append("<table border='1'>");
                sb.Append("<tr>");
                sb.Append("<th>Periodo</th>"); // 👈 NUEVO encabezado
                sb.Append("<th>Fecha de Registro</th>");
                sb.Append("<th>Matrícula</th>");
                sb.Append("<th>Alumno</th>");
                sb.Append("<th>Correo</th>");
                sb.Append("<th>Plan de Estudios</th>");
                sb.Append("<th>Escuela</th>");
                sb.Append("<th>Unidad</th>");
                sb.Append("<th>Estatus</th>");
                sb.Append("</tr>");

                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", row["Periodo"]); // 👈 NUEVO dato
                    sb.AppendFormat("<td>{0}</td>", row["dFechaRegistro"]);
                    sb.AppendFormat("<td>{0}</td>", row["Matricula"]);
                    sb.AppendFormat("<td>{0}</td>", row["Alumno"]);
                    sb.AppendFormat("<td>{0}</td>", row["Correo"]);
                    sb.AppendFormat("<td>{0}</td>", row["PlanEstudio"]);
                    sb.AppendFormat("<td>{0}</td>", row["Escuela"]);
                    sb.AppendFormat("<td>{0}</td>", row["UNIDAD"]);
                    sb.AppendFormat("<td>{0}</td>", row["EstadoAutorizacion"]);
                    sb.Append("</tr>");
                }

                sb.Append("</table>");
                Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
        }
        private DataTable ObtenerDatosExportacionAlumnos(string matricula, string nombre, string correo, string estatus, string unidad, string nivel, string plan, string escuela, string periodo)
        {
            int totalRecords;
            return ObtenerDatos(0, 100000, matricula, nombre, correo, estatus, unidad, nivel, escuela, plan, periodo, out totalRecords);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //// Obtén el término de búsqueda del cuadro de texto
            //string searchTerm = txtBusqueda.Text.Trim();

            //// Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            //// Carga los datos con el término de búsqueda y la página actual
            //CargarDatos(CurrentPage, searchTerm);

            string matricula = txtMatricula.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            // Recargar los datos con los filtros actuales
            CargarDatos(CurrentPage, matricula, nombre, correo);
        }
        protected void btnRegresar_Click(object sender, EventArgs e)
        {

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            if (tipoUsuario == "1")
            {

                Response.Redirect("PanelAdministrador.aspx");
            }
            else if (tipoUsuario == "3")
            {
                Response.Redirect("PanelResponsables.aspx");
            }
        }


        protected void lnkPrev_Click(object sender, EventArgs e)
        {
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
        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }
        


        #endregion

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
                LinkButton btnRechazar = (LinkButton)e.Item.FindControl("btnRechazar");
                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
                LinkButton btnLiberar = (LinkButton)e.Item.FindControl("btnLiberar");
                string estatus = row["idEstatus"].ToString().Trim();
                string filtros = Session["filtros"].ToString();
                string tipoUsuario = filtros.Split('|')[0];

                if (estatus == "20707" || estatus == "1")
                {
                    btnAutorizar.Visible = true;
                    btnRechazar.Visible = false;
                }
                else if (estatus == "11")
                {
                    btnAutorizar.Visible = false;
                    btnRechazar.Visible = true;
                }
                else if (estatus == "2")
                {
                    btnAutorizar.Visible = true;
                    btnRechazar.Visible = false;
                }

                if(tipoUsuario != "1")
                {
                    btnEliminar.Visible = false;
                }
            }
        }


    }
}
