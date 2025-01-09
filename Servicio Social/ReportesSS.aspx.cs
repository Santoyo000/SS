using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using Table = CrystalDecisions.CrystalReports.Engine.Table;
using System.Data;

namespace Servicio_Social
{
    public partial class ReportesSS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CargarPeriodo();
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {

            string selectedPeriodo = ddlPeriodo.SelectedValue;
            GenerarReporte(selectedPeriodo);

        }
        private void CargarPeriodo()
        {
            string query = @"SELECT idCiclo ,sDescripcion FROM SP_CICLO WHERE dFecha_Inicio > '2024-08-04 00:00:00.000' AND 
            idCiclo  NOT IN (0,34)";
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlPeriodo.DataSource = reader;
                ddlPeriodo.DataTextField = "sDescripcion";
                ddlPeriodo.DataValueField = "idCiclo";
                ddlPeriodo.DataBind();
                ddlPeriodo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione un Periodo......", "")); // Agrega una opción por defecto
            }
        }
        private void GenerarReporte(string selectedPeriodo)
        {
            try
            {
                // Ruta del archivo .rpt de Crystal Reports
                string reportPath = Server.MapPath("~/Reportes/TotalAlumnosRegistrados.rpt");

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
                    reporte.SetParameterValue("@kmPeriodo", selectedPeriodo);

                    // Exportar el reporte a un Stream en formato PDF
                    using (Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat))
                    {
                        // Convierte el Stream en un array de bytes
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);

                        // Convierte los bytes a una cadena Base64
                        string base64String = Convert.ToBase64String(bytes);

                        // Establece el contenido del PDF en el iframe de la página
                        string iframeSrc = $"data:application/pdf;base64,{base64String}";
                        pdfIframe.Attributes["src"] = iframeSrc;
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
    }
    }