using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Net;
using System.Web.Http.Results;
using iText.Kernel.Geom;
using OfficeOpenXml;
using System.IO;
using System.Web.Services;
using static System.Data.Entity.Infrastructure.Design.Executor;
using AjaxControlToolkit;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using WebListItem = System.Web.UI.WebControls.ListItem;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Servicio_Social
{
    public partial class DependenciasRegistradas1 : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Declarar la variable

            //if (Session["idUser"] == null || Session["idDependencia"] == null)
            //{
            //    Response.Redirect("Home.aspx");
            //}

            if (!IsPostBack)
            {
                CargarDatos(0, "","","");
                llenarUnidadModal();
                llenarOrganismoModal();
                CargarUnidad();
                //CargarPeriodo();
                CargarEstatus();
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
        #region Operaciones 
        protected void lbnDependencias_Click(object sender, EventArgs e)
        {
            pnlDependencias.Visible = true;
            CargarDatos(0, "", "", "");
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

        private void CargarEstatus()
        {
            string query = @"SELECT idEstatus,sClave, sDescripcion  FROM NP_ESTATUS WHERE sClave IN ('11','23','2') ORDER BY sDescripcion"; // Ajusta la condición según tu criterio
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
       
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string NombreDep, string Correo, string Fecha, string selectedEstatus, string selectedUnidad,  out int totalRecords)
        {
                    string conString = GlobalConstants.SQL;
                    int rowsToSkip = pageIndex * pageSize;

                    string filtros = Session["filtros"].ToString();
                    string tipoUsuario = filtros.Split('|')[0];
                    string unidadUsuario = filtros.Split('|')[1];

                    List<string> condiciones = new List<string>();

                    if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                        condiciones.Add("DS.kpUnidad = @selectedUnidad");

                    if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                        condiciones.Add("DS.bAutorizado = @selectedEstatus");

                    if (!string.IsNullOrEmpty(NombreDep))
                        condiciones.Add("DS.sDescripcion LIKE @NombreDep");

                    if (!string.IsNullOrEmpty(Correo))
                        condiciones.Add("USU.sCorreo LIKE @Correo");

                    if (!string.IsNullOrEmpty(Fecha))
                        condiciones.Add("CONVERT(VARCHAR, DS.DFECHAREGISTRODEP, 103) = @Fecha");

                    //if (!string.IsNullOrEmpty(selectedPeriodo))
                    //    condiciones.Add("DS.kpPeriodo = @selectedPeriodo"); 

                    // Restricciones por tipo de usuario
                    if (tipoUsuario == "3" || tipoUsuario == "4")
                        condiciones.Add("DS.kpUnidad = @unidadUsuario");

                    string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

                    string query = $@"
                SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep,
                       DS.sResponsable, DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad,
                       DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado,
                       E.sDescripcion AS Estatus, ORG.idOrganismo, ORG.sDescripcion AS sOrganismo
                FROM SM_DEPENDENCIA_SERVICIO DS
                INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad
                INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo
                INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado
                INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario
                {whereClause}
                ORDER BY DS.bAutorizado DESC
                OFFSET @rowsToSkip ROWS FETCH NEXT @pageSize ROWS ONLY;
            ";

                    string countQuery = $@"
                SELECT COUNT(*)
                FROM SM_DEPENDENCIA_SERVICIO DS
                INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad
                INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo
                INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado
                INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario
                {whereClause};
            ";

                    DataTable dt = new DataTable();
                    totalRecords = 0;

                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlCommand countCmd = new SqlCommand(countQuery, con);

                        // Parámetros
                        if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                        {
                            cmd.Parameters.AddWithValue("@selectedUnidad", selectedUnidad);
                            countCmd.Parameters.AddWithValue("@selectedUnidad", selectedUnidad);
                        }

                        if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                        {
                            cmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                            countCmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                        }

                        if (!string.IsNullOrEmpty(NombreDep))
                        {
                            cmd.Parameters.AddWithValue("@NombreDep", $"%{NombreDep}%");
                            countCmd.Parameters.AddWithValue("@NombreDep", $"%{NombreDep}%");
                        }

                        if (!string.IsNullOrEmpty(Correo))
                        {
                            cmd.Parameters.AddWithValue("@Correo", $"%{Correo}%");
                            countCmd.Parameters.AddWithValue("@Correo", $"%{Correo}%");
                        }

                        if (!string.IsNullOrEmpty(Fecha))
                        {
                            cmd.Parameters.AddWithValue("@Fecha", Fecha);
                            countCmd.Parameters.AddWithValue("@Fecha", Fecha);
                        }

                        if (tipoUsuario == "3" || tipoUsuario == "4")
                        {
                            cmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                            countCmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                        }

                        cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                        cmd.Parameters.AddWithValue("@pageSize", pageSize);

                        con.Open();
                        totalRecords = (int)countCmd.ExecuteScalar();

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }

                    return dt;
        }
        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            // 🔥 Obtén los datos
            DataTable dt = ObtenerDatosExportacion(
                txtDependencia.Text.Trim(),
                txtCorreo.Text.Trim(),
                txtFecha.Text.Trim(),
                ddlEstatus.SelectedValue,
                DDLUnidad.SelectedValue
            );

            if (dt.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DependenciasRegistradas.xls");
                Response.Charset = "utf-8"; // ⚡ muy importante
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = Encoding.UTF8;

                StringBuilder sb = new StringBuilder();
                sb.Append("<table border='1'>");
                sb.Append("<tr>");
                sb.Append("<th>Fecha Registro</th>");
                sb.Append("<th>Dependencia</th>");
                sb.Append("<th>Unidad</th>");
                sb.Append("<th>Telefono</th>");
                sb.Append("<th>Correo</th>");
                sb.Append("<th>Estatus</th>");
                sb.Append("</tr>");

                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", row["dFechaRegistroDep"]);
                    sb.AppendFormat("<td>{0}</td>", row["sDescripcion"]);
                    sb.AppendFormat("<td>{0}</td>", row["sUnidad"]);
                    sb.AppendFormat("<td>{0}</td>", row["sTelefono"]);
                    sb.AppendFormat("<td>{0}</td>", row["sCorreo"]);
                    sb.AppendFormat("<td>{0}</td>", row["Estatus"]);
                    sb.Append("</tr>");
                }

                sb.Append("</table>");
                Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
        }
        private DataTable ObtenerDatosExportacion(string nombreDep, string correo, string fecha, string estatus, string unidad)
        {
            // pageIndex = 0, pageSize MUY GRANDE para traer todo
            int totalRecords;
            return ObtenerDatos(0, 100000, nombreDep, correo, fecha, estatus, unidad, out totalRecords);
        }
       
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            string NombreDep = txtDependencia.Text.Trim();
            string Correo = txtCorreo.Text.Trim();
            string Fecha = txtFecha.Text.Trim();

            if (CurrentPage > 0)
            {
                CurrentPage -= 1;
                CargarDatos(CurrentPage, NombreDep, Correo, Fecha);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string NombreDep = txtDependencia.Text.Trim();
            string Correo = txtCorreo.Text.Trim();
            string Fecha = txtFecha.Text.Trim();

            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage += 1;
                CargarDatos(CurrentPage, NombreDep, Correo, Fecha);
            }
        }
        //protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords) RESPALDO DE METODO ANTERIOR
        //{
        //    string conString = GlobalConstants.SQL;
        //    int rowsToSkip = pageIndex * pageSize;

        //    string filtros = Session["filtros"].ToString();
        //    string tipoUsuario = filtros.Split('|')[0];
        //    string unidadUsuario = filtros.Split('|')[1];
        //    string filtroquery = "";

        //    // Consulta SQL para obtener los datos paginados
        //    string query = "SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep, DS.sResponsable, DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad, " +
        //        "DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado, E.sDescripcion AS Estatus, ORG.idOrganismo, ORG.sDescripcion AS sOrganismo " +
        //        "FROM SM_DEPENDENCIA_SERVICIO DS " +
        //        "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
        //        "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
        //        "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
        //        "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario ";

        //    if (tipoUsuario == "1")
        //    {
        //        filtroquery = "WHERE DS.kpUnidad IN (2,3,4) ";
        //    }
        //    else if (tipoUsuario == "3")
        //    {
        //        filtroquery = "WHERE DS.kpUnidad = " + unidadUsuario + " ";
        //    }
        //    query += filtroquery;

        //    // Consulta SQL para contar el total de registros
        //    string countQuery = "SELECT COUNT(*) FROM SM_DEPENDENCIA_SERVICIO DS " +
        //        "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
        //        "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
        //        "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
        //        "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario ";
        //    countQuery += filtroquery;

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        string searchCondition = "AND (DS.sDescripcion LIKE @searchTerm OR DS.sResponsable LIKE @searchTerm " +
        //                                 "OR U.sCiudad LIKE @searchTerm " +
        //                                 "OR USU.sCorreo LIKE @searchTerm " +
        //                                 "OR E.sDescripcion LIKE @searchTerm OR CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) LIKE @searchTerm) ";
        //        query += searchCondition;
        //        countQuery += searchCondition;
        //    }

        //    query += "ORDER BY DS.bAutorizado DESC " +
        //             "OFFSET @rowsToSkip ROWS " +
        //             "FETCH NEXT @pageSize ROWS ONLY;";

        //    DataTable dt = new DataTable();
        //    totalRecords = 0;

        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
        //        cmd.Parameters.AddWithValue("@pageSize", pageSize);

        //        SqlCommand countCmd = new SqlCommand(countQuery, con);
        //        if (!string.IsNullOrEmpty(searchTerm))
        //        {
        //            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
        //            countCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

        //        }

        //        con.Open();

        //        // Obtener el número total de registros
        //        totalRecords = (int)countCmd.ExecuteScalar();

        //        // Obtener los datos paginados
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dt);
        //    }

        //    return dt;

        //}

        //protected DataTable ObtenerDatos2(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        //{
        //    string conString = GlobalConstants.SQL;
        //    int rowsToSkip = pageIndex * pageSize;

        //    string filtros = Session["filtros"].ToString();
        //    string tipoUsuario = filtros.Split('|')[0];
        //    string unidadUsuario = filtros.Split('|')[1];
        //    string filtroquery = "";

        //    // Consulta SQL para obtener los datos paginados
        //    string query = "SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep, DS.sResponsable, DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad, " +
        //        "DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado, E.sDescripcion AS Estatus, ORG.idOrganismo, ORG.sDescripcion AS sOrganismo " +
        //        "FROM SM_DEPENDENCIA_SERVICIO DS " +
        //        "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
        //        "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
        //        "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
        //        "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario WHERE DS.kmValido is not null AND DS.bAutorizado = 9";

        //    if (tipoUsuario == "1")
        //    {
        //        filtroquery = "WHERE DS.kpUnidad IN (2,3,4) ";
        //    }
        //    else if (tipoUsuario == "3")
        //    {
        //        filtroquery = "WHERE DS.kpUnidad = " + unidadUsuario + " ";
        //    }
        //    query += filtroquery;

        //    // Consulta SQL para contar el total de registros
        //    string countQuery = "SELECT COUNT(*) FROM SM_DEPENDENCIA_SERVICIO DS " +
        //        "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
        //        "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
        //        "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
        //        "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario ";
        //    countQuery += filtroquery;

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        string searchCondition = "AND (DS.sDescripcion LIKE @searchTerm OR DS.sResponsable LIKE @searchTerm " +
        //                                 "OR U.sCiudad LIKE @searchTerm " +
        //                                 "OR USU.sCorreo LIKE @searchTerm " +
        //                                 "OR E.sDescripcion LIKE @searchTerm OR CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) LIKE @searchTerm) ";
        //        query += searchCondition;
        //        countQuery += searchCondition;
        //    }

        //    query += "ORDER BY DS.bAutorizado " +
        //             "OFFSET @rowsToSkip ROWS " +
        //             "FETCH NEXT @pageSize ROWS ONLY;";

        //    DataTable dt = new DataTable();
        //    totalRecords = 0;

        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
        //        cmd.Parameters.AddWithValue("@pageSize", pageSize);

        //        SqlCommand countCmd = new SqlCommand(countQuery, con);
        //        if (!string.IsNullOrEmpty(searchTerm))
        //        {
        //            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
        //            countCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

        //        }

        //        con.Open();

        //        // Obtener el número total de registros
        //        totalRecords = (int)countCmd.ExecuteScalar();

        //        // Obtener los datos paginados
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dt);
        //    }

        //    return dt;

        //}

        protected void cambiarEstatus(string id, string cambio)
        {
            string idUser = "";
            if (Session["idUser"] != null)
                idUser = Session["idUser"].ToString();


            string connectionString = GlobalConstants.SQL;
            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            string updateQuery;
            string parametroKm;


            if (tipoUsuario == "1")
            {
                updateQuery = "UPDATE SM_DEPENDENCIA_SERVICIO SET bAutorizado = @bAutorizado, kmAutorizo = @kmAutorizo WHERE idDependenicaServicio = @idDependenicaServicio";
                parametroKm = "@kmAutorizo";
            }
            else if (tipoUsuario == "3")
            {
                updateQuery = "UPDATE SM_DEPENDENCIA_SERVICIO SET bAutorizado = @bAutorizado, kmValido = @kmValido WHERE idDependenicaServicio = @idDependenicaServicio";
                parametroKm = "@kmValido";
            }
            else { return; }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@bAutorizado", cambio);
                    command.Parameters.AddWithValue("@idDependenicaServicio", id);
                    command.Parameters.AddWithValue(parametroKm, idUser);

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

        private void cargarUnidades(DropDownList ddl)
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1);";

            ddl.Items.Clear();
            ddl.Items.Add(new WebListItem("Selecciona una opción", ""));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ddl.Items.Add(new WebListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                }

                con.Close();
            }
        }



        private void cargarOrganismo(DropDownList ddl)
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idOrganismo, sDescripcion FROM SP_ORGANISMO;";

            ddl.Items.Clear();
            ddl.Items.Add(new WebListItem("Selecciona una opción", ""));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);    
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ddl.Items.Add(new WebListItem(reader["sDescripcion"].ToString(), reader["idOrganismo"].ToString()));
                }

                con.Close();
            }
        }

        protected void CargarDatos(int pageIndex, string NombreDep, string CorreoDep, string Fecha)
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatos(pageIndex, pageSize, NombreDep, CorreoDep, Fecha, 
                                        ddlEstatus.SelectedValue, DDLUnidad.SelectedValue, out totalRecords);

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
                    string NombreDep = txtDependencia.Text.Trim();
                    string CorreoDep = txtCorreo.Text.Trim();
                    string Fecha = txtFecha.Text.Trim();

                    // Recargar los datos con los filtros actuales
                    CargarDatos(CurrentPage, NombreDep, CorreoDep, Fecha);
                }
            }
        }
        //protected void CargarDatos2(int pageIndex, string searchTerm)
        //{
        //    int pageSize = 10; // Cantidad de registros por página
        //    int totalRecords;

        //    DataTable dt = ObtenerDatos2(pageIndex, pageSize, searchTerm, out totalRecords);

        //    Repeater1.DataSource = dt;
        //    Repeater1.DataBind();

        //    // Calcula el número total de páginas
        //    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        //    // Configura el estado de los botones
        //    btnPrevious.Enabled = pageIndex > 0;
        //    btnNext.Enabled = pageIndex < TotalPages - 1;

        //    // Actualiza la etiqueta de número de página
        //    lblPageNumber.Text = $"Página {pageIndex + 1} de {TotalPages}";
        //}

        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }

        private void UpdateDataInDatabase(int id, string dependencia, string responsable, string unidad, string telefono, string domicilio, string area, string organismo)
        {
            //UpdateDataInDatabase(id, descripcion, responsable, unidad,telefono,domicilio);
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Inicia una transacción
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Update SM_DEPENDENCIA_SERVICIO
                    using (SqlCommand cmd1 = new SqlCommand("UPDATE SM_DEPENDENCIA_SERVICIO SET sDescripcion = @sDescripcion, sResponsable = @sResponsable, " +
                        "kpUnidad = @kpUnidad, sTelefono = @sTelefono, sDomicilio = @sDomicilio, sAreaResponsable = @sAreaResponsable, kpOrganismo = @kpOrganismo " +
                        "WHERE idDependenicaServicio = @id", connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@sDescripcion", dependencia);
                        cmd1.Parameters.AddWithValue("@sResponsable", responsable);
                        cmd1.Parameters.AddWithValue("@kpUnidad", unidad);
                        cmd1.Parameters.AddWithValue("@sTelefono", telefono);
                        cmd1.Parameters.AddWithValue("@sDomicilio", domicilio);
                        cmd1.Parameters.AddWithValue("@sAreaResponsable", area);
                        cmd1.Parameters.AddWithValue("@kpOrganismo", organismo);

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
        #endregion

        #region Eventos Items
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                DropDownList ddlUnidad = (DropDownList)e.Item.FindControl("ddlUnidad");
                DropDownList ddlOrganismo = (DropDownList)e.Item.FindControl("ddlOrganismo");
                LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");


                if (ddlUnidad != null)
                {
                    cargarUnidades(ddlUnidad);

                    //DataRowView row = (DataRowView)e.Item.DataItem;
                    string unidadValue = row["sUnidad"].ToString().Trim();

                    WebListItem itemToSelect = ddlUnidad.Items.FindByText(unidadValue);
                    if (itemToSelect != null)
                    {
                        ddlUnidad.SelectedValue = itemToSelect.Value;
                        System.Diagnostics.Debug.WriteLine("Selected Value: " + itemToSelect.Value);
                    }
                }


                if (ddlOrganismo != null)
                {
                    cargarOrganismo(ddlOrganismo);

                    //DataRowView row = (DataRowView)e.Item.DataItem;
                    string OrganismoValue = row["sOrganismo"].ToString().Trim();

                    WebListItem itemToSelect = ddlOrganismo.Items.FindByText(OrganismoValue);
                    if (itemToSelect != null)
                    {
                        ddlOrganismo.SelectedValue = itemToSelect.Value;
                        System.Diagnostics.Debug.WriteLine("Selected Value: " + itemToSelect.Value);
                    }
                }


                string autorizado = row["bAutorizado"].ToString().Trim();

                switch (autorizado)
                {
                    case "20707":
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = true;
                        break;

                    case "2":
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = false;
                        break;

                    case "11":
                        btnAutorizar.Visible = false;
                        btnEliminar.Visible = true;
                        break;

                    default:
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = true;
                        break;
                }
            }
        }

        protected void gvDependencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[7].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[7].Visible = false;

                LinkButton btnAutorizar = e.Row.FindControl("btnAutorizar") as LinkButton;
                LinkButton btnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
                string id = (e.Row.DataItem as DataRowView)["idDependenicaServicio"].ToString();
                string correo = (e.Row.DataItem as DataRowView)["sCorreo"].ToString();
                string concatenar = id + "|" + correo;
                btnAutorizar.CommandArgument = concatenar;
                btnEliminar.CommandArgument = concatenar;
                string estatus = (e.Row.DataItem as DataRowView)["bAutorizado"].ToString();
                string Fecha = (e.Row.DataItem as DataRowView)["dFechaRegistroDep"].ToString();
                switch (estatus)
                {
                    case "1":
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = true;
                        break;

                    case "2":
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = false;
                        break;

                    case "11":
                        btnAutorizar.Visible = false;
                        btnEliminar.Visible = true;
                        break;

                    default:
                        btnAutorizar.Visible = true;
                        btnEliminar.Visible = true;
                        break;
                }
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Edit")
            {
                // Mostrar el modo de edición para la fila seleccionada
                Panel pnlViewMode = (Panel)Repeater1.Items[index].FindControl("pnlViewMode");
                Panel pnlEditMode = (Panel)Repeater1.Items[index].FindControl("pnlEditMode");

                pnlViewMode.Visible = false;
                pnlEditMode.Visible = true;
            }
            else if (e.CommandName == "Update")
            {
                // Lógica para actualizar los datos en la base de datos
                TextBox txtDescripcion = (TextBox)Repeater1.Items[index].FindControl("txtDescripcion");
                TextBox txtResponsable = (TextBox)Repeater1.Items[index].FindControl("txtResponsable");
                TextBox txtAreaResponsable = (TextBox)Repeater1.Items[index].FindControl("txtAreaResponsable");
                DropDownList dUnidad = (DropDownList)Repeater1.Items[index].FindControl("ddlUnidad");
                DropDownList dOrganismo = (DropDownList)Repeater1.Items[index].FindControl("ddlOrganismo");
                TextBox txtTelefono = (TextBox)Repeater1.Items[index].FindControl("txtTelefono");
                TextBox txtDomicilio = (TextBox)Repeater1.Items[index].FindControl("txtDomicilio");

                int rowIndex = e.Item.ItemIndex;
                HiddenField hdnID = (HiddenField)Repeater1.Items[rowIndex].FindControl("hdnID");
                int id = Convert.ToInt32(hdnID.Value.Split('|')[0]);
                string descripcion = txtDescripcion.Text;
                string responsable = txtResponsable.Text;
                string area = txtAreaResponsable.Text;
                string unidad = dUnidad.SelectedValue;
                string organismo = dOrganismo.SelectedValue;
                string telefono = txtTelefono.Text;
                string domicilio = txtDomicilio.Text;

                // Actualiza los datos en la base de datos
                UpdateDataInDatabase(id, descripcion, responsable, unidad, telefono, domicilio, area, organismo);

                // Vuelve al modo de visualización
                Panel pnlViewMode = (Panel)Repeater1.Items[index].FindControl("pnlViewMode");
                Panel pnlEditMode = (Panel)Repeater1.Items[index].FindControl("pnlEditMode");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;

                if (pnlEditMode.Visible)
                    pnlEditMode.CssClass = "edit-mode";

                // Volver a cargar con filtros activos
                string NombreDep = txtDependencia.Text.Trim();
                string Correo = txtCorreo.Text.Trim();
                string Fecha = txtFecha.Text.Trim();
                int page = CurrentPage;

                if (string.IsNullOrEmpty(NombreDep) || string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Fecha))
                {
                    // Vuelve a enlazar los datos al Repeater
                    CargarDatos(page, "", "", "");
                }
                else
                {
                    // Vuelve a enlazar los datos al Repeater
                    CargarDatos(page, NombreDep, Correo, Fecha);
                }
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
            else if (e.CommandName == "Cancel")
            {
                // Vuelve al modo de visualización sin hacer cambios
                Panel pnlViewMode = (Panel)Repeater1.Items[index].FindControl("pnlViewMode");
                Panel pnlEditMode = (Panel)Repeater1.Items[index].FindControl("pnlEditMode");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;
            }

            if (e.CommandName == "Page")
            {
                int pageIndex = int.Parse(e.CommandArgument.ToString());
                CargarDatos(pageIndex, "","","");
            }
        }
        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            // Limpiar los TextBox
            txtDependencia.Text = string.Empty;
            txtFecha.Text = string.Empty;
            txtCorreo.Text = string.Empty;

            ddlEstatus.ClearSelection();
            if (ddlEstatus.Items.Count > 0) ddlEstatus.SelectedIndex = 0;

            DDLUnidad.ClearSelection();
            if (DDLUnidad.Items.Count > 0) DDLUnidad.SelectedIndex = 0;


        }
        #endregion

        #region Botones
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {

            LinkButton lnkUpdate = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            string[] valores = hdnID.Value.Split('|');
            string idDependenicaServicio = valores[0];
            string sCorreo = valores[1];

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            string cambio = tipoUsuario == "1" ? "11" : "9"; // Admin: 11, Responsable: 9

            cambiarEstatus(idDependenicaServicio, cambio);

            if (tipoUsuario == "1")
            {
                enviarCorreo(sCorreo, cambio);
                mensajeScript("Registrado Autorizado con éxito");
            }

            // Volver a cargar con filtros activos
            string NombreDep = txtDependencia.Text.Trim();
            string Correo = txtCorreo.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            int page = CurrentPage;

            //if (string.IsNullOrEmpty(NombreDep) || string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Fecha))
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, "", "", "");
            //}
            //else
            //{
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, NombreDep, Correo, Fecha);
           // }

            //LinkButton lnkUpdate = (LinkButton)sender;

            //// Encuentra el RepeaterItem asociado al LinkButton
            //RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;

            //// Encuentra el HiddenField dentro del RepeaterItem
            //HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            //// Obtén el valor del HiddenField
            //string[] valores = hdnID.Value.Split('|');
            //string idDependenicaServicio = valores[0];
            //string sCorreo = valores[1];

            ////LinkButton button = sender as LinkButton;
            ////string id = button.CommandArgument.ToString();
            ////string correo = button.CommandArgument.ToString().Split('|')[1];
            //string cambio;
            //string filtros = Session["filtros"].ToString();
            //string tipoUsuario = filtros.Split('|')[0];

            //if (tipoUsuario == "1")
            //{
            //    cambio = "11";
            //    cambiarEstatus(idDependenicaServicio, cambio);
            //    enviarCorreo(sCorreo, cambio);
            //    mensajeScript("Registrado Autorizado con éxito");
            //}
            //else if (tipoUsuario == "3")
            //{
            //    cambio = "9";
            //    cambiarEstatus(idDependenicaServicio, cambio);
            //}
            //else
            //{
            //    // Tipo de usuario no reconocido, manejar el error o la lógica correspondiente
            //    return;
            //}


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

            LinkButton lnkUpdate = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            string[] valores = hdnID.Value.Split('|');
            string idDependenicaServicio = valores[0];
            string sCorreo = valores[1];

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string cambio = "2"; // No Autorizado

            cambiarEstatus(idDependenicaServicio, cambio);

            if (tipoUsuario == "1")
            {
                // enviarCorreo(sCorreo, cambio); // Solo si deseas enviar correo también
                mensajeScript("Registrado NO Autorizado con éxito");
            }

            // Volver a cargar con filtros activos
            string NombreDep = txtDependencia.Text.Trim();
            string Correo = txtCorreo.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            int page = CurrentPage;

            //if (string.IsNullOrEmpty(NombreDep) || string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Fecha))
            //{
            //    // Vuelve a enlazar los datos al Repeater
            //    CargarDatos(page, "", "", "");
            //}
            //else
            //{
                // Vuelve a enlazar los datos al Repeater
                CargarDatos(page, NombreDep, Correo, Fecha);
           // }

            //LinkButton lnkUpdate = (LinkButton)sender;

            //// Encuentra el RepeaterItem asociado al LinkButton
            //RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;

            //// Encuentra el HiddenField dentro del RepeaterItem
            //HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            //// Obtén el valor del HiddenField
            //string[] valores = hdnID.Value.Split('|');
            //string idDependenicaServicio = valores[0];
            //string sCorreo = valores[1];

            //string filtros = Session["filtros"].ToString();
            //string tipoUsuario = filtros.Split('|')[0];
            //string cambio = "2";
            //if (tipoUsuario == "1")
            //{

            //    cambiarEstatus(idDependenicaServicio, cambio);
            ////    enviarCorreo(sCorreo, cambio);
            //    mensajeScript("Registrado NO Autorizado con éxito");
            //}
            //else if (tipoUsuario == "3")
            //{
            //    cambiarEstatus(idDependenicaServicio, cambio);
            //}
            //else
            //{
            //    // Tipo de usuario no reconocido, manejar el error o la lógica correspondiente
            //    return;
            //}


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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            //string searchTerm = txtBusqueda.Text.Trim();

            string NombreDep = txtDependencia.Text.Trim();
            string CorreoDep = txtCorreo.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            //CargarDatos(CurrentPage, searchTerm);

            CargarDatos(CurrentPage, NombreDep, CorreoDep, Fecha);



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

        #endregion

        [WebMethod]
        public static string llenarDatosModal(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string query =
                @"SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep, DS.sResponsable, 
                DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad, DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado, E.sDescripcion AS Estatus, 
                ORG.idOrganismo, ORG.sDescripcion AS sOrganismo, PE_valido.sNombre_completo AS PersonaValido, PE_autorizo.sNombre_completo AS PersonaAutorizo 
                FROM SM_DEPENDENCIA_SERVICIO DS
                INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad
                INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo 
                INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado
                INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario
                LEFT JOIN SM_USUARIO USU_VALIDO ON USU_VALIDO.idUsuario = DS.kmValido
                LEFT JOIN NM_PERSONA PE_valido ON PE_valido.idPersona = USU_VALIDO.kmIdentificador
                 LEFT JOIN SM_USUARIO USU_AUTORIZO ON USU_AUTORIZO.idUsuario = DS.kmAutorizo
                 LEFT JOIN NM_PERSONA PE_autorizo ON PE_autorizo.idPersona = USU_AUTORIZO.kmIdentificador
                WHERE DS.idDependenicaServicio = @Id";

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

                        // Primer par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Fecha Registro:</strong> {reader["dFechaRegistroDep"]}</td>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Dependencia:</strong> {reader["sDescripcion"]}</td>";
                        htmlResult += "</tr>";

                        // Segundo par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Responsable:</strong> {reader["sResponsable"]}</td>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Área Responsable:</strong> {reader["sAreaResponsable"]}</td>";
                        htmlResult += "</tr>";

                        // Tercer par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Correo:</strong> {reader["sCorreo"]}</td>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Organismo:</strong> {reader["sOrganismo"]}</td>";
                        htmlResult += "</tr>";

                        // Cuarto par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Teléfono:</strong> {reader["sTelefono"]}</td>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Domicilio:</strong> {reader["sDomicilio"]}</td>";
                        htmlResult += "</tr>";

                        // Quinto par de datos
                        htmlResult += "<tr>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Validó:</strong> {reader["PersonaValido"]}</td>";
                        htmlResult += $"<td style='text-align: left; width: 50%;'><strong>Autorizó:</strong> {reader["PersonaAutorizo"]}</td>";
                        htmlResult += "</tr>";

                        htmlResult += "</table>";
                    }

                    con.Close();
                }
            }

            return htmlResult;
        }


        private void llenarUnidadModal()
        {
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1);", con))
                {
                    con.Open();
                    ddlUnidadModal.DataSource = cmd.ExecuteReader();
                    ddlUnidadModal.DataTextField = "sCiudad";
                    ddlUnidadModal.DataValueField = "idUnidad";
                    ddlUnidadModal.DataBind();
                }
            }
        }

        private void llenarOrganismoModal()
        {
            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT idOrganismo, sDescripcion FROM SP_ORGANISMO", con))
                {
                    con.Open();
                    ddlOrganismoModal.DataSource = cmd.ExecuteReader();
                    ddlOrganismoModal.DataTextField = "sDescripcion";
                    ddlOrganismoModal.DataValueField = "idOrganismo";
                    ddlOrganismoModal.DataBind();
                }
            }
        }
        //protected void btnVisualizar_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    int dependenciaId = Convert.ToInt32(btn.CommandArgument);

        //    string connectionString = GlobalConstants.SQL;
        //}


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int dependenciaId = Convert.ToInt32(btn.CommandArgument);

            string connectionString = GlobalConstants.SQL;
            string query = "SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep, DS.sResponsable, DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad, " +
                "DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado, E.sDescripcion AS Estatus, ORG.idOrganismo, ORG.sDescripcion AS sOrganismo " +
                "FROM SM_DEPENDENCIA_SERVICIO DS " +
                "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
                "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
                "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
                "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario " +
                "WHERE DS.idDependenicaServicio = @IdDependencia";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdDependencia", dependenciaId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        hfDependenciaId.Value = reader["idDependenicaServicio"].ToString();
                        txtNombreDependencia.Text = reader["sDescripcion"].ToString();
                        txtResponsable.Text = reader["sResponsable"].ToString();
                        txtAreaResponsable.Text = reader["sAreaResponsable"].ToString();
                        ddlUnidadModal.SelectedValue = reader["idUnidad"].ToString();
                        ddlOrganismoModal.SelectedValue = reader["idOrganismo"].ToString();
                        txtTelefono.Text = reader["sTelefono"].ToString();
                        txtDomicilio.Text = reader["sDomicilio"].ToString();
                    }
                }
            }

            //ScriptManager.RegisterStartupScript(this, GetType(), "editModalScript", "$('#editModal').modal('show');", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "abrirEditModal();", true);
            string scriptKey = Guid.NewGuid().ToString();
            //ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), scriptKey, "setTimeout(function(){ abrirEditModal(); }, 200);", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "var modal = new bootstrap.Modal(document.getElementById('editModal')); modal.show();", true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int dependenciaId = Convert.ToInt32(hfDependenciaId.Value);
            string nombreDependencia = txtNombreDependencia.Text;
            string responsable = txtResponsable.Text;
            string areaResponsable = txtAreaResponsable.Text;
            string unidad = ddlUnidadModal.SelectedValue;
            string organismo = ddlOrganismoModal.SelectedValue;
            string telefono = txtTelefono.Text;
            string domicilio = txtDomicilio.Text;
            string query = "UPDATE SM_DEPENDENCIA_SERVICIO SET sDescripcion = @NombreDependencia, sResponsable = @responsable, sAreaResponsable = @arearesponsable, kpUnidad = @unidad, " +
                "kpOrganismo = @organismo, sTelefono = @telefono, sDomicilio = @domicilio WHERE idDependenicaServicio = @IdDependencia";

            string connectionString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NombreDependencia", nombreDependencia);
                    cmd.Parameters.AddWithValue("@responsable", responsable);
                    cmd.Parameters.AddWithValue("@arearesponsable", areaResponsable);
                    cmd.Parameters.AddWithValue("@unidad", unidad);
                    cmd.Parameters.AddWithValue("@organismo", organismo);
                    cmd.Parameters.AddWithValue("@telefono", telefono);
                    cmd.Parameters.AddWithValue("@domicilio", domicilio);
                    cmd.Parameters.AddWithValue("@IdDependencia", dependenciaId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }


            string NombreDep = txtDependencia.Text.Trim();
            string Correo = txtCorreo.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            if (string.IsNullOrEmpty(NombreDep))
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatos(CurrentPage, "", "", "");
                }
            }
            else
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatos(CurrentPage, NombreDep, Correo, Fecha);
                }
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "editModalScript", "var modal = bootstrap.Modal.getInstance(document.getElementById('editModal')); if(modal) modal.hide();", true);
        }
    }
}