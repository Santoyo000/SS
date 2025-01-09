using System;
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


namespace Servicio_Social

{
    public partial class AlumnosPostulados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                CargarDatos(0, "");
                CargarEstatus();
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
        protected void CargarDatos(int pageIndex, string searchTerm)
        {
            int pageSize = 20; // Cantidad de registros por página
            int totalRecords;
            string selectedEstatus = ddlEstatus.SelectedValue;
            DataTable dt = ObtenerDatos(pageIndex, pageSize, searchTerm, selectedEstatus, out totalRecords);
                
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
                }
            },
            // Configuración para DEPENDENCIA (usuario == "2")
            { "2", new Dictionary<string, Action>
                {
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

        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, string selectedEstatus, out int totalRecords)
        {
            //string idDependencia = Session["idDependencia"].ToString().Split('|')[1];
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;
            string tipoUsuario = Session["tipoUsuario"].ToString();
            

            //string tipoUsuario = filtros.Split('|')[0];


            string filtroquery = "";

            // Consulta SQL para obtener los datos paginados
            string query = @" SELECT  
                            DS.IDDEPENDENICASERVICIO, PA.idProgramaAlumno,AL.IDALUMNO,
                            CONVERT(varchar, PA.DFECHAREGISTRO, 103) AS FECHAREGISTRO,
                            AL.SMATRICULA AS MATRICULA ,PER.SNOMBRE_COMPLETO AS NOMBRE_COMPLETO, P.SNOMBRE_PROGRAMA AS PROGRMA,
                            PE.sClave + ' - ' + PE.sDescripcion AS PLANEST, UO.sClave + ' - ' + UO.sDescripcion AS ESCUELA,
                            UN.SCIUDAD AS UNIDAD,
                            DP.ICUPO,
                            NPEST.SDESCRIPCION AS ESTATUS, NPEST.idEstatus
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
                            INNER JOIN NP_ESTATUS NPEST ON PA.KPESTATUS = NPEST.IDESTATUS
                            INNER JOIN NP_UNIDAD UN ON UO.KPUNIDAD = UN.IDUNIDAD";
                            

            if (tipoUsuario == "2") // USUARIO DEPENDENCIA
            {
                string filtros = Session["idDependencia"].ToString();
                string idUsuario = filtros.Split('|')[0];
                string idDependencia = filtros.Split('|')[1];
                filtroquery = " WHERE  DS.IDDEPENDENICASERVICIO = " + idDependencia + " AND PA.KPESTATUS != 7"; 

            }
            else if (tipoUsuario == "1") // USUARIO ADMINISTRADOR
            {
               //string filtros = Session["idUser"].ToString();
                //string idUsuario = filtros.Split('|')[0];
                
                filtroquery = " WHERE  P.kpUnidad IN (2,3,4) AND PA.KPESTATUS != 7";

            }
            else if (tipoUsuario == "3") // USUARIO RESPONSABLE
            {
                string filtros = Session["filtros"].ToString();
                string unidadUsuario = filtros.Split('|')[1];

                filtroquery = " WHERE  P.kpUnidad =" + unidadUsuario + " AND PA.KPESTATUS != 7";

            }
            else if (tipoUsuario == "4") //USUARIO ENCARGADO DE ESCUELA
            {
                string Escuela = "";
                string filtros = Session["filtros"].ToString();
                Escuela = filtros.Split('|')[2];
                    filtroquery = " WHERE UA.kpEscuela = " + Escuela + "  AND PA.KPESTATUS != 7 "; //PA.KPESTATUS IN (22115, 22116, 21522,21523,7,20707 ) "; AUTORIZADO POR DEPENDENCIA, AUTORIZADO POR ENCARGADO ESCUELA, CANCELADO, EN ESPERA, EVALUADO, LIBERADO DEP
            }

            // Agregar filtro de estatus si está seleccionado
            if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
            {
                filtroquery += " AND PA.KPESTATUS = @selectedEstatus";
            }
            query += filtroquery;


            // Consulta SQL para contar el total de registros
            string countQuery = @"SELECT COUNT(*) 
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
                            INNER JOIN NP_ESTATUS NPEST ON PA.KPESTATUS = NPEST.IDESTATUS ";
            
            // Agregar filtro de estatus si está seleccionado
            if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
            {
                countQuery += " AND PA.KPESTATUS = @selectedEstatus";
            }

            countQuery += filtroquery;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition = " AND (DFECHAREGISTRO LIKE @searchTerm OR AL.sMatricula LIKE @searchTerm OR PER.sNombre_completo LIKE @searchTerm " +
                                         " OR P.SNOMBRE_PROGRAMA LIKE @searchTerm  OR PE.sDescripcion LIKE @searchTerm OR UO.sDescripcion LIKE @searchTerm" +
                                         " OR PE.sClave LIKE @searchTerm OR UO.sClave LIKE @searchTerm  OR NPEST.sDescripcion LIKE @searchTerm) ";
                query += searchCondition;
                countQuery += searchCondition;
            }


            query += " ORDER BY NPEST.sClave" +
                     " OFFSET @rowsToSkip ROWS " +
                     " FETCH NEXT @pageSize ROWS ONLY;";

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                {
                    cmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                SqlCommand countCmd = new SqlCommand(countQuery, con);

                if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                {
                    countCmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    countCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                con.Open();

                // Obtener el número total de registros
                totalRecords = (int)countCmd.ExecuteScalar();

                // Obtener los datos paginados
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
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

        private void ExportToExcel()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            DocumentFormat.OpenXml.Spreadsheet.Page page = new DocumentFormat.OpenXml.Spreadsheet.Page();
            HtmlForm form = new HtmlForm();

            RepeaterTemp.DataSource = GetAllData();
            RepeaterTemp.DataBind();
            RepeaterTemp.RenderControl(htw);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Sanciones.xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

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
        #endregion

        #region Botones

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
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
            
            string searchTerm = txtBusqueda.Text.Trim();
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
            string searchTerm = txtBusqueda.Text.Trim();
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
            string searchTerm = txtBusqueda.Text.Trim();
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

            string searchTerm = txtBusqueda.Text.Trim();
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

            string searchTerm = txtBusqueda.Text.Trim();
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
       protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtBusqueda.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatos(CurrentPage, searchTerm);



          
        }
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            string searchTerm = txtBusqueda.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatos(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatos(CurrentPage, searchTerm);
                }
            }
        }
        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            string searchTerm = txtBusqueda.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatos(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatos(CurrentPage, searchTerm);
                }
            }


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

