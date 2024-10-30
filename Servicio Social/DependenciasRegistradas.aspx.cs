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

namespace Servicio_Social
{
    public partial class DependenciasRegistradas1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Declarar la variable

            //if (Session["idUser"] == null)
            //{ 
            //    Response.Redirect("Home.aspx");
            //}

            if (!IsPostBack)
            {
                CargarDatos(0, "");
                llenarUnidadModal();
                llenarOrganismoModal();
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

        #region Operaciones 
        protected void lbnDependencias_Click(object sender, EventArgs e)
        {
            pnlDependencias.Visible = true;
            CargarDatos(0, "");
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
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Dependencias");

                    // Definir los encabezados de las columnas
                    string[] headers = { "Dependencia", "Responsable", "Área Responsable", "Unidad", "Organismo", "Telefono", "Correo", "Domicilio", "Estatus" };
                    // Escribir los encabezados en la primera fila
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }
                    int rowIndex = 2; // Comenzar desde la fila 2 (después de los encabezados)

                    foreach (RepeaterItem item in Repeater1.Items)
                    {
                        // Verificar si el Repeater está en modo de edición o de visualización
                        if (item.FindControl("pnlViewMode").Visible)
                        {


                            TextBox txtDependencia = (TextBox)item.FindControl("txtDescripcion");
                            TextBox txtResponsable = (TextBox)item.FindControl("txtResponsable");
                            TextBox txtAreaResponsable = (TextBox)item.FindControl("txtAreaResponsable");
                            DropDownList ddlUnidad = (DropDownList)item.FindControl("ddlUnidad");
                            DropDownList ddlOrganismo = (DropDownList)item.FindControl("ddlOrganismo");
                            TextBox txtTelefono = (TextBox)item.FindControl("txtTelefono");
                            Label lblCorreo = (Label)item.FindControl("sCorreo");
                            TextBox txtDomicilio = (TextBox)item.FindControl("txtDomicilio");
                            Label lblEstatus = (Label)item.FindControl("Estatus");

                            // Escribir los datos en las celdas correspondientes
                            worksheet.Cells[rowIndex, 1].Value = txtDependencia?.Text ?? "";
                            worksheet.Cells[rowIndex, 2].Value = txtResponsable?.Text ?? "";
                            worksheet.Cells[rowIndex, 3].Value = txtAreaResponsable?.Text ?? "";
                            worksheet.Cells[rowIndex, 4].Value = ddlUnidad?.SelectedItem?.Text ?? "";
                            worksheet.Cells[rowIndex, 5].Value = ddlOrganismo?.SelectedItem?.Text ?? "";
                            worksheet.Cells[rowIndex, 6].Value = txtTelefono?.Text ?? "";
                            worksheet.Cells[rowIndex, 7].Value = lblCorreo?.Text ?? "";
                            worksheet.Cells[rowIndex, 8].Value = txtDomicilio?.Text ?? "";
                            worksheet.Cells[rowIndex, 9].Value = lblEstatus?.Text ?? "";
                        }
                        else // Si está en modo de visualización
                        {
                            Label lblDependencia = (Label)item.FindControl("sDescripcion");
                            Label lblResponsable = (Label)item.FindControl("sResponsable");
                            Label lblAreaResponsable = (Label)item.FindControl("sAreaResponsable");
                            Label lblUnidad = (Label)item.FindControl("sUnidad");
                            Label lblOrganismo = (Label)item.FindControl("sOrganismo");
                            Label lblTelefono = (Label)item.FindControl("sTelefono");
                            Label lblCorreo = (Label)item.FindControl("sCorreo");
                            Label lblDomicilio = (Label)item.FindControl("sDomicilio");
                            Label lblEstatus = (Label)item.FindControl("Estatus");

                            // Escribir los datos en las celdas correspondientes
                            worksheet.Cells[rowIndex, 1].Value = lblDependencia?.Text ?? "";
                            worksheet.Cells[rowIndex, 2].Value = lblResponsable?.Text ?? "";
                            worksheet.Cells[rowIndex, 3].Value = lblAreaResponsable?.Text ?? "";
                            worksheet.Cells[rowIndex, 4].Value = lblUnidad?.Text ?? "";
                            worksheet.Cells[rowIndex, 5].Value = lblOrganismo?.Text ?? "";
                            worksheet.Cells[rowIndex, 6].Value = lblTelefono?.Text ?? "";
                            worksheet.Cells[rowIndex, 7].Value = lblCorreo?.Text ?? "";
                            worksheet.Cells[rowIndex, 8].Value = lblDomicilio?.Text ?? "";
                            worksheet.Cells[rowIndex, 9].Value = lblEstatus?.Text ?? "";
                        }

                        rowIndex++; // Mover a la siguiente fila
                    }



                    // Configurar el tipo de respuesta y escribir el archivo en la respuesta HTTP
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Dependencias.xlsx");
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
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string unidadUsuario = filtros.Split('|')[1];
            string filtroquery = "";

            // Consulta SQL para obtener los datos paginados
            string query = "SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep, DS.sResponsable, DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad, " +
                "DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado, E.sDescripcion AS Estatus, ORG.idOrganismo, ORG.sDescripcion AS sOrganismo " +
                "FROM SM_DEPENDENCIA_SERVICIO DS " +
                "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
                "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
                "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
                "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario ";

            if (tipoUsuario == "1")
            {
                filtroquery = "WHERE DS.kpUnidad IN (2,3,4) ";
            }
            else if (tipoUsuario == "3")
            {
                filtroquery = "WHERE DS.kpUnidad = " + unidadUsuario + " ";
            }
            query += filtroquery;

            // Consulta SQL para contar el total de registros
            string countQuery = "SELECT COUNT(*) FROM SM_DEPENDENCIA_SERVICIO DS " +
                "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
                "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
                "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
                "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario ";
            countQuery += filtroquery;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition = "AND (DS.sDescripcion LIKE @searchTerm OR DS.sResponsable LIKE @searchTerm " +
                                         "OR U.sCiudad LIKE @searchTerm " +
                                         "OR USU.sCorreo LIKE @searchTerm " +
                                         "OR E.sDescripcion LIKE @searchTerm OR CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) LIKE @searchTerm) ";
                query += searchCondition;
                countQuery += searchCondition;
            }

            query += "ORDER BY DS.bAutorizado DESC " +
                     "OFFSET @rowsToSkip ROWS " +
                     "FETCH NEXT @pageSize ROWS ONLY;";

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
            ddl.Items.Add(new ListItem("Selecciona una opción", ""));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ddl.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                }

                con.Close();
            }
        }

        private void cargarOrganismo(DropDownList ddl)
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idOrganismo, sDescripcion FROM SP_ORGANISMO;";

            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Selecciona una opción", ""));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ddl.Items.Add(new ListItem(reader["sDescripcion"].ToString(), reader["idOrganismo"].ToString()));
                }

                con.Close();
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

                    ListItem itemToSelect = ddlUnidad.Items.FindByText(unidadValue);
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

                    ListItem itemToSelect = ddlOrganismo.Items.FindByText(OrganismoValue);
                    if (itemToSelect != null)
                    {
                        ddlOrganismo.SelectedValue = itemToSelect.Value;
                        System.Diagnostics.Debug.WriteLine("Selected Value: " + itemToSelect.Value);
                    }
                }


                string autorizado = row["bAutorizado"].ToString().Trim();

                switch (autorizado)
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
                CargarDatos(pageIndex, "");
            }
        }
        #endregion

        #region Botones
        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdate = (LinkButton)sender;

            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;

            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string idDependenicaServicio = valores[0];
            string sCorreo = valores[1];

            //LinkButton button = sender as LinkButton;
            //string id = button.CommandArgument.ToString();
            //string correo = button.CommandArgument.ToString().Split('|')[1];
            string cambio;
            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];

            if (tipoUsuario == "1")
            {
                cambio = "11";
                cambiarEstatus(idDependenicaServicio, cambio);
                enviarCorreo(sCorreo, cambio);
                mensajeScript("Registrado Autorizado con éxito");
            }
            else if (tipoUsuario == "3")
            {
                cambio = "9";
                cambiarEstatus(idDependenicaServicio, cambio);
            }
            else
            {
                // Tipo de usuario no reconocido, manejar el error o la lógica correspondiente
                return;
            }


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

            LinkButton lnkUpdate = (LinkButton)sender;

            // Encuentra el RepeaterItem asociado al LinkButton
            RepeaterItem item = (RepeaterItem)lnkUpdate.NamingContainer;

            // Encuentra el HiddenField dentro del RepeaterItem
            HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

            // Obtén el valor del HiddenField
            string[] valores = hdnID.Value.Split('|');
            string idDependenicaServicio = valores[0];
            string sCorreo = valores[1];

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string cambio = "2";
            if (tipoUsuario == "1")
            {

                cambiarEstatus(idDependenicaServicio, cambio);
            //    enviarCorreo(sCorreo, cambio);
                mensajeScript("Registrado NO Autorizado con éxito");
            }
            else if (tipoUsuario == "3")
            {
                cambiarEstatus(idDependenicaServicio, cambio);
            }
            else
            {
                // Tipo de usuario no reconocido, manejar el error o la lógica correspondiente
                return;
            }


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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtBusqueda.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatos(CurrentPage, searchTerm);
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

        [WebMethod]
        public static string llenarDatosModal(int id)
        {
            string connectionString = GlobalConstants.SQL;
            string query =
                "SELECT DS.idDependenicaServicio, DS.sDescripcion, CONVERT(varchar, DS.DFECHAREGISTRODEP, 103) AS dFechaRegistroDep, DS.sResponsable, " +
                "DS.sAreaResponsable, U.idUnidad, U.sCiudad AS sUnidad, DS.sTelefono, USU.sCorreo, DS.sDomicilio, DS.bAutorizado, E.sDescripcion AS Estatus, " +
                "ORG.idOrganismo, ORG.sDescripcion AS sOrganismo, PE_valido.sNombre_completo AS PersonaValido, PE_autorizo.sNombre_completo AS PersonaAutorizo " +
                "FROM SM_DEPENDENCIA_SERVICIO DS " +
                "INNER JOIN NP_UNIDAD U ON U.idUnidad = DS.kpUnidad " +
                "INNER JOIN SP_ORGANISMO ORG ON ORG.idOrganismo = DS.kpOrganismo " +
                "INNER JOIN NP_ESTATUS E ON E.idEstatus = DS.bAutorizado " +
                "INNER JOIN SM_USUARIO USU ON USU.idUsuario = DS.kmUsuario " +
                "LEFT JOIN NM_PERSONA PE_valido ON DS.kmValido = PE_valido.idPersona " +
                "LEFT JOIN NM_PERSONA PE_autorizo ON DS.kmAutorizo = PE_autorizo.idPersona " +
                "WHERE DS.idDependenicaServicio = @Id";

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

            ScriptManager.RegisterStartupScript(this, GetType(), "editModalScript", "$('#editModal').modal('show');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "editModalScript", "$('#editModal').modal('hide');", true);
        }
    }
}