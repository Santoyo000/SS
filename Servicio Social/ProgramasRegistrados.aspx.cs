using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Web.Services;
using System.Web.Http.Results;

namespace Servicio_Social
{
    public partial class ProgramasRegistrados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

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
        public class Programa
        {
            public DateTime FechaRegistro { get; set; }
            public string Dependencia { get; set; }
            public string Correo { get; set; }
            public string NombrePrograma { get; set; }
            public string Responsable { get; set; }
            public string Estatus { get; set; }
          
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Obtén el panel
               // Panel pnlAutoriz = (Panel)e.Item.FindControl("pnlAutorizar");
                LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
                LinkButton btnEditarCupo = (LinkButton)e.Item.FindControl("btnEditarCupo");
                LinkButton bntEdit = (LinkButton)e.Item.FindControl("bntEdit");
                LinkButton btnDetalle2 = (LinkButton)e.Item.FindControl("btnDetalle2");

                // Obtén el usuario de la sesión
                if (Session["filtros"] != null)
                {
                    string usuario = Session["filtros"].ToString().Split('|')[0];
                    if (usuario == "1") // ADMON
                    {
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = true;
                        btnDetalle2.Visible = false;

                    } 
                    else if (usuario == "4") // ENCARGADO DE ESCUELA
                    {
                        btnAutorizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnEditarCupo.Visible = false;
                        bntEdit.Visible = true;
                        btnDetalle2.Visible = true;
                    }
                    else if (usuario == "3") // DEPENDENCIA
                    {
                        btnAutorizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnEditarCupo.Visible = false;
                        bntEdit.Visible = false;
                    }
                    else if (usuario == "2") // RESPONSABLE UNIDAD
                    {
                        btnAutorizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnEditarCupo.Visible = false;
                        bntEdit.Visible = false;
                        btnDetalle2.Visible = true;
                    }
                }
            }
        }
        protected void CargarDatos(int pageIndex, string searchTerm)
        {
            int pageSize = 30; // Cantidad de registros por página
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
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            
            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string unidadUsuario = filtros.Split('|')[1];
            string Escuela = "";
            if(tipoUsuario == "4")
            { 
              Escuela = filtros.Split('|')[2];
            }
            string filtroquery = "";

            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            // Consulta SQL para obtener los datos paginados
            string query = @"SELECT PR.idPrograma,CONVERT(varchar, DFECHAREGISTROP, 103) AS FechaRegistro, DS.sDescripcion AS Dependencia, USU.sCorreo AS Correo , PR.sNombre_Programa AS NombrePrograma,
                             PR.sResponsable AS Responsable, PR.kpEstatus_Programa ,  UN.SCIUDAD AS UNIDAD,ES.sDescripcion AS Estatus 
							 FROM SM_PROGRAMA AS PR 
							 INNER JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio 
                             INNER JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus 
                             INNER JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario
							 INNER JOIN NP_UNIDAD AS UN ON PR.KPUNIDAD = UN.IDUNIDAD  ";
            
            if (tipoUsuario == "1")
            {
                filtroquery = " WHERE PR.kpUnidad IN (2,3,4) ";
            }
            else if (tipoUsuario == "3")
            {
                filtroquery = " WHERE PR.kpUnidad = " + unidadUsuario + " ";
            }
            else if (tipoUsuario == "4")
            {
                filtroquery = " JOIN SM_DETALLE_PROGRAMA AS DP ON PR.idPrograma= DP.kmPrograma WHERE PR.kpUnidad = " + unidadUsuario + " AND DP.kpEscuela= " + Escuela + " ";
            }
            query += filtroquery;
            // Consulta SQL para contar el total de registros
            string countQuery = @"SELECT COUNT(*)
                             FROM SM_PROGRAMA AS PR
                             INNER JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio
                             INNER JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus
                             INNER JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario
                             INNER JOIN NP_UNIDAD AS UN ON PR.KPUNIDAD = UN.IDUNIDAD ";
            countQuery += filtroquery;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition = "AND DS.sDescripcion LIKE @searchTerm OR ES.sDescripcion LIKE @searchTerm " +
                                         "OR DS.sResponsable LIKE @searchTerm OR PR.sNombre_Programa LIKE @searchTerm " +
                                         "OR USU.sCorreo LIKE @searchTerm ";
                query += searchCondition;
                countQuery += searchCondition;
            }

            query += " ORDER BY PR.kpEstatus_Programa DESC " +
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

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string idPrograma = btn.CommandArgument;
            Response.Redirect("EditarPrograma.aspx?idPrograma=" + idPrograma);
        }
        protected void bntEditarDetallePrograma(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string idPrograma = btn.CommandArgument;
            Response.Redirect("EditarDetallePrograma.aspx?idPrograma=" + idPrograma);
        }
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
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
            else if (tipoUsuario == "4")
            {
                Response.Redirect("PanelEncargadoEscuelaspx.aspx");
            }

        }
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            // Verificar si el Repeater contiene elementos
            if (Repeater1.Items.Count > 0)
            {
                // Crear el archivo Excel en memoria
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregar una hoja de trabajo al archivo Excel
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Programas");

                    // Definir los encabezados de las columnas
                    string[] headers = { "Fecha de Registro", "Dependencia", "Correo", "Nombre del Programa", "Responsable", "Estatus" };
                    // Escribir los encabezados en la primera fila
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }
                    int rowIndex = 2; // Comenzar desde la fila 2 (después de los encabezados)

                    foreach (RepeaterItem item in Repeater1.Items)
                    {
                       
                            TextBox txtFechaRegistro = (TextBox)item.FindControl("txtFechaRegistro");
                            TextBox txtDependencia = (TextBox)item.FindControl("txtDependencia");
                            TextBox txtCorreo = (TextBox)item.FindControl("txtCorreo");
                            TextBox txtNombrePrograma = (TextBox)item.FindControl("txtNombrePrograma");
                            TextBox txtResponsable = (TextBox)item.FindControl("txtResponsable");
                            Label lblEstatus = (Label)item.FindControl("lblEstatus");

                            // Escribir los datos en las celdas correspondientes
                            worksheet.Cells[rowIndex, 1].Value = txtFechaRegistro?.Text ?? "";
                            worksheet.Cells[rowIndex, 2].Value = txtDependencia?.Text ?? "";
                            worksheet.Cells[rowIndex, 3].Value = txtCorreo?.Text ?? "";
                            worksheet.Cells[rowIndex, 4].Value = txtNombrePrograma?.Text ?? "";
                            worksheet.Cells[rowIndex, 5].Value = txtResponsable?.Text ?? "";
                            worksheet.Cells[rowIndex, 6].Value = lblEstatus?.Text ?? "";
                        

                        rowIndex++; // Mover a la siguiente fila
                    }

                    // Configurar el tipo de respuesta y escribir el archivo en la respuesta HTTP
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Programas.xlsx");
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.End();
                }
            }
            else
            {
                // Si el Repeater está vacío, mostrar un mensaje o realizar alguna acción adicional
                // Puedes agregar aquí el código para manejar este caso según tus necesidades
            }
        }
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdate = (LinkButton)sender;

            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;

            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string idPrograma = valores[0];
            string sCorreo = valores[1];

            string correo = "3";
            string estatus = "11";
            cambiarEstatus(idPrograma, estatus);
            enviarCorreo(sCorreo, correo);
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
        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
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
        protected void cambiarEstatus(string id, string cambio)
        {
            string connectionString = GlobalConstants.SQL;
            string updateQuery = "UPDATE SM_PROGRAMA SET kpEstatus_Programa = @Estatus WHERE idPrograma = @idPrograma";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@Estatus", cambio);
                    command.Parameters.AddWithValue("@idPrograma", id);

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
        protected void btnEliminar_Click(object sender, EventArgs e)
        {

            LinkButton lnkUpdate = (LinkButton)sender;

            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;

            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string idPrograma = valores[0];
            string sCorreo = valores[1];
            string estatus = "2";
            string correo = "4";
            cambiarEstatus(idPrograma, estatus);
            enviarCorreo(sCorreo, correo);
            mensajeScript("Registrado NO Autorizado con éxito");
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
        [WebMethod]
        public static string llenarDatosModal(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string query =
               "SELECT sHorario,PR.sObjetivos, PR.sActividades_desarollo, PR.sDomicilio_Serv, TS.sDescripcion AS MODALIDAD, PR.sOtraModalidad AS OTRA_MOD ,EN.sDescripcion AS ENFOQUE , PR.sOtroEnfoque AS OTRO_ENF, PR.slinkMaps " +
                            "  FROM SM_PROGRAMA AS PR JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio " +
                            " JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus " +
                            " JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario "+
                            " JOIN SP_TIPO_SERVICIO AS TS ON PR.kpModalidad = TS.idTipoServicio" +
                            " JOIN SP_ENFOQUE AS EN ON PR.kpEnfoque = EN.idEnfoque"+
                            " WHERE PR.idPrograma = @Id";

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
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Horario:</strong> {reader["sHorario"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Modalidad:</strong> {reader["MODALIDAD"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Otra Modalidad:</strong> {reader["OTRA_MOD"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Enfoque:</strong> {reader["ENFOQUE"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Otro Enfoque:</strong> {reader["OTRO_ENF"]}</td>";
                        htmlResult += "</tr>";

                        // Primer par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Objetivos:</strong> {reader["sObjetivos"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";

                        htmlResult += "</tr>";

                        // Segundo par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Actividades de desarrollo:</strong> {reader["sActividades_desarollo"]}</td>";
                        htmlResult += "</tr>";

                        // Segundo par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Domicilio Servicio:</strong> {reader["sDomicilio_Serv"]}</td>";
                        htmlResult += "</tr>";

                        
                        htmlResult += "<tr>";
                        string mapaUrl = reader["slinkMaps"].ToString();
                        htmlResult += $"<td class='small-text'><strong>Mapa:</strong> <a href='{mapaUrl}' target='_blank'>Ver en Google Maps</a></td>";
                        htmlResult += "</tr>";
                      
                        

                        htmlResult += "</table>";
                    }

                    con.Close();
                }
            }

            return htmlResult;
        }

        [WebMethod]
        public static string llenarDatosModalDet(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string query =
                "SELECT NIVEL.sDescripcion AS NIVEL, PLA.sClave + ' - ' + PLA.sDescripcion AS PLANEST, ESC.sClave + ' - ' + ESC.sDescripcion AS ESCUELA, DETP.iCupo " +
                "FROM SM_DETALLE_PROGRAMA AS DETP " +
                "JOIN SP_PLAN_ESTUDIO AS PLA ON DETP.kpPlanEstudio = PLA.idPlanEstudio " +
                "JOIN SP_ESCUELA_UAC AS ESC ON DETP.kpEscuela = ESC.idEscuelaUAC " +
                "JOIN SP_TIPO_NIVEL AS NIVEL ON DETP.kpNivel = NIVEL.idTipoNivel " +
                "WHERE DETP.kmPrograma = @Id";

            string htmlResult = "<table style='width: 100%; table-layout: fixed;'>";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Nivel:</strong> {reader["NIVEL"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Plan de Estudio:</strong> {reader["PLANEST"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Escuela:</strong> {reader["ESCUELA"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 100%;'><strong>Cupo:</strong> {reader["iCupo"]}</td>";
                        htmlResult += "</tr>";
                    }

                    con.Close();
                }
            }

            htmlResult += "</table>";
            return htmlResult;
        }

    }
}