using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using static Servicio_Social.Dependencias1;
using System.Net.NetworkInformation;
using DocumentFormat.OpenXml.Office.Y2022.FeaturePropertyBag;
using System.Web.Services;
namespace Servicio_Social
{
    public partial class AlumnosPostulados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUser"] == null)
            {
                Response.Redirect("Home.aspx");
            }

            if (!IsPostBack)
            {
                CargarDatos(0, "");
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

            DataTable dt = ObtenerDatos(pageIndex, pageSize, searchTerm, out totalRecords);

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
                LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
                string estatus = row["idEstatus"].ToString().Trim();

                HashSet<string> EstatusValidos = new HashSet<string> { "20707", "21522", "21523" };

                string usuario = Session["tipoUsuario"].ToString();
                if ((usuario == "1")) //(usuario == "1" || usuario == "3")
                {
                    if (estatus == "7")
                    {
                        btnEliminar.Visible = false;
                        btnAutorizar.Visible = true;
                    }                        
                    else if (estatus == "21522")
                    {
                        btnEliminar.Visible = true;
                        btnAutorizar.Visible = false;
                    }
                    else if (estatus == "22114")
                    {
                        btnEliminar.Visible = false;
                        btnAutorizar.Visible = false;
                    }
                    else if (estatus == "22113")
                    {
                        btnEliminar.Visible = false;
                        btnAutorizar.Visible = true;
                    }


                }
                else if ((usuario == "4") || (usuario == "2"))
                {

                    if (estatus == "21522")
                    {
                        btnEliminar.Visible = true;
                        btnAutorizar.Visible = false;
                    }
                    else if (estatus == "22113")
                    {
                        btnEliminar.Visible = false;
                        btnAutorizar.Visible = true;
                    }
                }
                else if (usuario == "3")
                {
                    btnEliminar.Visible = false;
                    btnAutorizar.Visible = false;
                }
            }
        }
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
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
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
                
                filtroquery = " WHERE  P.kpUnidad IN (2,3,4)";

            }
            else if (tipoUsuario == "3") // USUARIO RESPONSABLE
            {
                string filtros = Session["filtros"].ToString();
                string unidadUsuario = filtros.Split('|')[1];

                filtroquery = " WHERE  P.kpUnidad =" + unidadUsuario + " ";

            }
            else if (tipoUsuario == "4") //USUARIO ENCARGADO DE ESCUELA
            {
                string Escuela = "";
                string filtros = Session["filtros"].ToString();
                Escuela = filtros.Split('|')[2];
                    filtroquery = " WHERE UA.kpEscuela = " + Escuela + " AND PA.KPESTATUS IN (21522,21523,7,20707) ";
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

                SqlCommand countCmd = new SqlCommand(countQuery, con);
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
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
        
        #endregion

        #region Botones

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
#endregion
    }
}

