using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Servicio_Social.ProgramasDependencias;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Servicio_Social
{
    public partial class VerProgramas : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        private const string NPE_VIEWSTATE_KEY = "NPE";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string encodedIdPrograma = Request.QueryString["idPrograma"];
                if (!string.IsNullOrEmpty(encodedIdPrograma))
                {
                    try
                    {
                        string idPrograma = SimpleEncryptionHelper.Decode(HttpUtility.UrlDecode(encodedIdPrograma));
                        // Usa el valor de idPrograma como necesites
                        Editar(idPrograma);
                    }
                    catch (Exception ex)
                    {
                        // Manejar errores de desencriptación
                        // Por ejemplo, redirigir a una página de error o mostrar un mensaje
                        ///Response.Redirect("Error.aspx");
                    }
                }
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
            string Periodo = "";
            string Modalidad = "";
            string Enfoque = "";
            string Unidad = "";
            string OtrM = "";
            string OtrE = "";
            int idModalidad = 0;
            int idEnfoque = 0;
            //int Unidad = 0;

            // Establece la cadena de conexión a tu base de datos
            string connectionString = GlobalConstants.SQL;

            // Define la consulta SQL para recuperar la descripción de la dependencia basada en el ID de usuario
            string query = "SELECT P.sNombre_Programa, DEP.sDescripcion ,P.sDomicilio_Serv, P.sObjetivos,P.sActividades_desarollo," +
                            "P.sHorario, P.slinkMaps, P.kpModalidad,P.kpEnfoque, P.sAreaResponsable, P.sLugarAdscripcion, P.sOtraModalidad, P.sOtroEnfoque," +
                            " P.sResponsable, P.sCargoResp, P.kpUnidad , PER.sDescripcion AS Periodo , UN.sCiudad AS Unidad, " +
                            "  TS.sDescripcion  AS Modalidad, ENF.sDescripcion AS Enfoque, P.sOtraModalidad AS OtrM, P.sOtroEnfoque AS OtrE  " +
                            " FROM SM_PROGRAMA AS P JOIN SM_DEPENDENCIA_SERVICIO AS DEP ON P.kpDependencia = dep.idDependenicaServicio" +
                            " JOIN SP_CICLO AS PER ON P.kpPeriodo = PER.idCiclo " +
                            " JOIN NP_UNIDAD AS UN ON P.kpUnidad= UN.idUnidad "+
                            " JOIN SP_TIPO_SERVICIO AS TS ON P.kpModalidad = TS.idTipoServicio   " +
                            " JOIN SP_ENFOQUE AS ENF ON P.kpEnfoque = ENF.idEnfoque " +
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
                            Periodo = reader["Periodo"].ToString();
                            NombreP = reader["sNombre_Programa"].ToString();
                            Dependencia = reader["sDescripcion"].ToString();
                            Domicilio = reader["sDomicilio_Serv"].ToString();
                            Objetivos = reader["sObjetivos"].ToString();
                            Actividades = reader["sActividades_desarollo"].ToString();
                            Horario = reader["sHorario"].ToString();
                            Link = reader["slinkMaps"].ToString();
                            Area = reader["sAreaResponsable"].ToString();
                            Lugar = reader["sLugarAdscripcion"].ToString();
                            Unidad = reader["Unidad"].ToString();
                            Modalidad = reader["Modalidad"].ToString();
                            Enfoque = reader["Enfoque"].ToString();
                            idModalidad = Convert.ToInt32(reader["kpModalidad"]);
                            idEnfoque = Convert.ToInt32(reader["kpEnfoque"]);
                            OtrM = reader["OtrM"].ToString();
                            OtrE = reader["OtrE"].ToString();

                            //Modalidad = Convert.ToInt32(reader["kpModalidad"]);
                            //Enfoque = Convert.ToInt32(reader["kpEnfoque"]);
                            //Periodo = Convert.ToInt32(reader["kpPeriodo"]);
                            Responsable = reader["sResponsable"].ToString();
                            Cargo = reader["sCargoResp"].ToString();
                            //Unidad = Convert.ToInt32(reader["kpUnidad"]);

                        }
                    }
                    if (idModalidad == 9){ pnlOModalidad.Visible = true;} else { pnlOModalidad.Visible = false; }
                    if (idEnfoque == 13) { pnlOEnfoque.Visible = true; } else { pnlOEnfoque.Visible = false; }
                    txtUnidad.Text = Unidad;
                    txtModalidad.Text = Modalidad;
                    txtEnfoque.Text = Enfoque;
                    txtPeriodo.Text = Periodo;
                    txtPrograma.Text = NombreP;
                    txtDependencia.Text = Dependencia;
                    TxtDomicilio.Text = Domicilio;
                    txtObjetivos.Text = Objetivos;
                    txtActividades.Text = Actividades;
                    txtHorario.Text = Horario;
                    txtLinkGoogle.Text = Link;
                    txtAreaResp.Text = Area;
                    txtLugar.Text = Lugar;
                    txtOtM.Text = OtrM;
                    txtOE.Text = OtrE;
                    //DDLModalidad.SelectedValue = Modalidad.ToString();
                    //DDLEnfoque.SelectedValue = Enfoque.ToString();
                    //DDLPeriodo.SelectedValue = Periodo.ToString();
                    txtResponsable.Text = Responsable;
                    txtCargo.Text = Cargo;
                    //DDLUnidad.SelectedValue = Unidad.ToString();

                }

                // Dentro de tu método Editar(string idPrograma)
                string queryDetallePrograma = "SELECT DP.idDetallePrograma, N.sDescripcion AS Nivel, DP.kpNivel, PE.sClave +'-'+ PE.sDescripcion AS PlanE, DP.kpPlanEstudio, " +
                                              "ES.sClave +'-' + ES.sDescripcion Escuela,  DP.kpEscuela,iCupo " +
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

       

        public void mensajeScript(string mensaje)
        {
            string scriptText = "$('.alert').remove(); $('body').prepend('<div class=\"alert alert-success alert-dismissible fade show\" role=\"alert\" style=\"background-color: #d4edda; color: #155724;\"><strong>" + mensaje + "</strong><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button></div>');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
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
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[6].Visible = false;


                }
            }
            catch { }
        }


    }
}
