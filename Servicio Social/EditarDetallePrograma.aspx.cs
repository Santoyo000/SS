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

namespace Servicio_Social
{
    public partial class EditarDetallePrograma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                
                // Este bloque de código se ejecuta solo en la primera carga de la página
               
                CargarDatosDetallePrograma();

            
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
        protected void btnRegresar_Click(object sender, EventArgs e)
        {

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            if (tipoUsuario == "1")
            {

                Response.Redirect("ProgramasRegistrados.aspx");
            }
            else if (tipoUsuario == "3")
            {
                Response.Redirect("ProgramasRegistrados.aspx");
            }
        }
        protected void CargarDatosDetallePrograma()
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = obtenerDatosDetallePrograma();

            RepeaterDetPro.DataSource = dt;
            RepeaterDetPro.DataBind();

        }
        protected DataTable obtenerDatosDetallePrograma()
        {
            string conString = GlobalConstants.SQL;

            string idPrograma = Request.QueryString["idPrograma"];


            // Consulta SQL para obtener los datos paginados
            string query = @" SELECT 
                            DP.idDetallePrograma, 
                            N.sDescripcion AS Nivel, 
                            DP.kpNivel, 
                            PE.sClave + ' - ' + PE.sDescripcion AS PlanE, 
                            DP.kpPlanEstudio,
                            ES.sClave + ' - ' + ES.sDescripcion AS Escuela,  
                            DP.kpEscuela,
                            COUNT(CASE WHEN PA.kpEstatus  IN (21522) THEN 1 END) AS Asignados,
                            DP.iCupo - COUNT(CASE WHEN PA.kpEstatus IN (20707, 21522, 21523, 21526) THEN 1 END) AS Vacantes,
	                         iCupo
                        FROM 
                            SM_DETALLE_PROGRAMA AS DP 
                        JOIN 
                            SP_TIPO_NIVEL AS N ON DP.kpNivel = N.idTipoNivel                                               
                        JOIN 
                            SP_PLAN_ESTUDIO AS PE ON DP.kpPlanEstudio = PE.idPlanEstudio 
                        JOIN 
                            SP_ESCUELA_UAC AS ES ON DP.kpEscuela = ES.idEscuelaUAC
                        LEFT JOIN 
                            SM_PROGRAMA_ALUMNO PA ON PA.kmDetallePrograma = DP.idDetallePrograma WHERE DP.kmprograma =" + idPrograma +

                            @" GROUP BY 
                            DP.idDetallePrograma, 
                            N.sDescripcion, 
                            DP.kpNivel, 
                            PE.sClave,
                            PE.sDescripcion, 
                            DP.kpPlanEstudio,
                            ES.sClave, 
                            ES.sDescripcion,
                            DP.kpEscuela,
                            iCupo;";

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);


                con.Open();


                // Obtener los datos paginados
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            return dt;

        }

        protected void RepeaterDetPro_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Edit")
            {
                // Mostrar el modo de edición para la fila seleccionada
                Panel pnlViewMode = (Panel)RepeaterDetPro.Items[index].FindControl("pnlViewMode");
                Panel pnlEditMode = (Panel)RepeaterDetPro.Items[index].FindControl("pnlEditMode");

                pnlViewMode.Visible = false;
                pnlEditMode.Visible = true;

                ViewState["ActiveTab"] = "#tab2";
            }
            else if (e.CommandName == "Update")
            {
                // Lógica para actualizar los datos en la base de datos
                TextBox txtCupo = (TextBox)RepeaterDetPro.Items[index].FindControl("txtCupo");

                int rowIndex = e.Item.ItemIndex;
                HiddenField hdnID = (HiddenField)RepeaterDetPro.Items[rowIndex].FindControl("hdnID2");
                int id = Convert.ToInt32(hdnID.Value);
                string Cupo = txtCupo.Text;
                //string descripcion = txtDescripcion.Text;
                // Actualiza los datos en la base de datos
                UpdateCupo(id, Cupo);

                // Vuelve al modo de visualización
                Panel pnlViewMode = (Panel)RepeaterDetPro.Items[index].FindControl("pnlViewMode");
                Panel pnlEditMode = (Panel)RepeaterDetPro.Items[index].FindControl("pnlEditMode");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;

                if (pnlEditMode.Visible)
                    pnlEditMode.CssClass = "edit-mode";


                // Vuelve a enlazar los datos al Repeater
                CargarDatosDetallePrograma();


            }
            else if (e.CommandName == "Cancel")
            {
                // Vuelve al modo de visualización sin hacer cambios
                Panel pnlViewMode = (Panel)RepeaterDetPro.Items[index].FindControl("pnlViewMode");
                Panel pnlEditMode = (Panel)RepeaterDetPro.Items[index].FindControl("pnlEditMode");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;
                //UpdatePanel1.Update();
            }

            if (e.CommandName == "Page")
            {
                int pageIndex = int.Parse(e.CommandArgument.ToString());
                CargarDatosDetallePrograma();
                //UpdatePanel1.Update();
            }
        }

        private void UpdateCupo(int id, string Cupo)
        {
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Inicia una transacción
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Update SM_DEPENDENCIA_SERVICIO
                    using (SqlCommand cmd1 = new SqlCommand("UPDATE SM_DETALLE_PROGRAMA SET iCupo = @sCupo" +
                        " WHERE idDetallePrograma = @id", connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@sCupo", Cupo);
                        cmd1.Parameters.AddWithValue("@id", id);
                        cmd1.ExecuteNonQuery();
                    }

                    // Confirma la transacción
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, realiza un rollback
                    transaction.Rollback();
                }
            }
        }
    }
}