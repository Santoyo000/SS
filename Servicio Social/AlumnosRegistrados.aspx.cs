using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class AlumnosRegistrados : System.Web.UI.Page
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
            public string Correo { get; set; }
            public string PlanEstudios { get; set; }
            public string Escuela { get; set; }
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
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string unidadUsuario = filtros.Split('|')[1];
            string Escuela = tipoUsuario == "4" ? filtros.Split('|')[2] : null;
            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Llamada al SP para contar el total de registros
                using (SqlCommand countCmd = new SqlCommand("sp_ContarAlumnosRegistrados_ss", con))
                {
                    countCmd.CommandType = CommandType.StoredProcedure;
                    countCmd.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);
                    countCmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                    countCmd.Parameters.AddWithValue("@Escuela", Escuela ?? (object)DBNull.Value);
                    countCmd.Parameters.AddWithValue("@searchTerm", !string.IsNullOrEmpty(searchTerm) ? $"%{searchTerm}%" : (object)DBNull.Value);

                    totalRecords = (int)countCmd.ExecuteScalar();
                }

                // Llamada al SP para obtener los datos paginados
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerAlumnosRegistrados_ss", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);
                    cmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                    cmd.Parameters.AddWithValue("@Escuela", Escuela ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@searchTerm", !string.IsNullOrEmpty(searchTerm) ? $"%{searchTerm}%" : (object)DBNull.Value);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;

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
        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }
        protected void btnLiberar_Click(object sender, EventArgs e)
        {

            //// Obtén el ID del registro desde el CommandArgument
            //LinkButton btn = (LinkButton)sender;
            //string id = btn.CommandArgument;

            //// Consulta SQL para obtener los datos necesarios
            //string connectionString = GlobalConstants.SQL;
            //string query = " SELECT USA.ID ,US.dFechaRegistro, US.sCorreo AS Correo, ALU.sMatricula AS Matricula, PER.sNombre_completo AS Alumno, PLA.sClave + ' - ' + PLA.sDescripcion AS PlanEstudio, " +
            //" ESC.sClave + ' - ' + ESC.sDescripcion AS Escuela, EST.sDescripcion AS EstadoAutorizacion " +
            //" FROM SM_USUARIO AS US JOIN SM_USUARIOS_ALUMNOS AS USA ON US.idUsuario = USA.kmUsuario JOIN SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno " +
            //" JOIN NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona JOIN SP_ESCUELA_UAC AS ESC ON USA.kpEscuela = ESC.idEscuelaUAC " +
            //" JOIN SP_PLAN_ESTUDIO AS PLA ON USA.kpPlan = PLA.idPlanEstudio JOIN NP_ESTATUS AS EST ON USA.bAutorizado = EST.idEstatus WHERE USA.ID = @ID";

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@ID", id);
            //    conn.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        string director = "Nombre del Director"; // Deberías obtenerlo también
            //        string escuela = reader["Escuela"].ToString();
            //        string alumno = reader["Alumno"].ToString();
            //        string matricula = reader["Matricula"].ToString();
            //        string planEstudios = reader["PlanEstudio"].ToString();
            //        string fechaInicio = "Fecha de Inicio"; // Deberías obtenerlo también
            //        string fechaFin = "Fecha de Fin"; // Deberías obtenerlo también
            //        string numHoras = "Número de Horas"; // Deberías obtenerlo también
            //        string actividades = "Actividades"; // Deberías obtenerlo también

            //        // Generar el documento Word
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true))
            //            {
            //                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

            //                // Crear las propiedades de sección
            //                SectionProperties sectionProps = new SectionProperties(
            //                    new PageMargin()
            //                    {
            //                        Top = 1440,     // 1 pulgada (en twips)
            //                        Right = 1440,   // 1 pulgada
            //                        Bottom = 1440,  // 1 pulgada
            //                        Left = 1440,    // 1 pulgada
            //                        Header = 720,   // 0.5 pulgada
            //                        Footer = 720,   // 0.5 pulgada
            //                        Gutter = 0      // Sin espacio adicional
            //                    }
            //                    , new PageBorders(
            //                        new TopBorder { Val = BorderValues.Single, Size = 4 },
            //                        new BottomBorder { Val = BorderValues.Single, Size = 4 },
            //                        new LeftBorder { Val = BorderValues.Single, Size = 4 },
            //                        new RightBorder { Val = BorderValues.Single, Size = 4 }

            //                        )
            //                );

            //                // Crear el documento con el cuerpo y agregar las propiedades de sección
            //                mainPart.Document = new Document(new Body(
            //                sectionProps, // Agregar las propiedades de sección al cuerpo del documento

            //                new Paragraph(new ParagraphProperties(
            //                    new Justification() { Val = JustificationValues.Both }
            //                ),
            //                new Run(new RunProperties(
            //                    new Bold() // Aplica negritas
            //                ),
            //                new Text(director))),



            //                new Paragraph(new ParagraphProperties(
            //                    new Justification() { Val = JustificationValues.Both }
            //                ),
            //                new Run(new Text($"Director de {escuela}"))),

            //                new Paragraph(new ParagraphProperties(
            //                    new Justification() { Val = JustificationValues.Both }
            //                ),
            //                new Run(new Text("P r e s e n t e .-"))),

            //                new Paragraph(new ParagraphProperties(
            //                    new Justification() { Val = JustificationValues.Both }
            //                ),
            //                new Run(new Text($"Por medio de la presente HACE CONSTAR que el/la C. {alumno} con matrícula {matricula} estudiante de {planEstudios} de la UNIVERSIDAD AUTÓNOMA DE COAHUILA, realizó su servicio social en [Institución_Servicio], durante el periodo comprendido del {fechaInicio} al {fechaFin} cubriendo un total de {numHoras} horas efectivas, desarrollando como actividades {actividades}."))),

            //                new Paragraph(new ParagraphProperties(
            //                    new Justification() { Val = JustificationValues.Both }
            //                ),
            //                new Run(new Text("Sin mas por el momento, y agradeciendo de antemano sus intenciones, me despido."))),

            //                new Paragraph(new ParagraphProperties(
            //                    new Justification() { Val = JustificationValues.Both }
            //                ),
            //                new Run(new Text("Atentamente")))
            //            ));
            //            }
            //            // Enviar el archivo al cliente
            //            Response.Clear();
            //            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            //            Response.AddHeader("content-disposition", "attachment;filename=DocumentoGenerado.docx");
            //            Response.BinaryWrite(ms.ToArray());
            //            Response.End();
            //        }
            //    }
            //}
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
