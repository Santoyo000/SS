using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Servicio_Social.Dependencias1;
using System.Reflection.Emit;
using System.Web.UI.WebControls.WebParts;
using static Servicio_Social.RegistroEstudiante1;


namespace Servicio_Social
{
    public partial class UsuariosRegistrados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUser"] == null)
                Response.Redirect("LoginAdministrador.aspx");
            else
            {
                if (Session["filtros"].ToString().Split('|')[0] == "3")
                {
                    lbnResponsable.Visible = false;
                    pnlDependencias.Visible = false;
                    pnlResponsables.Visible = false;
                    PanelEncargadosEscuela.Visible = false;
                    pnlRegistrarAdmon.Visible = false;
                    pnlRegistrarEncargado.Visible = false;
                    pnlAlumnosIncorp.Visible = true;
                    
                }
                   
            } 

            if (!IsPostBack)
            {
                CargarDatosResponsable(0, "");
                CargarDatosDependencia(0, "");
                CargarDatosEncargadosEscuela(0, "");
                cargarUnidades();
                CargarUsuarios();
                CargarDatosAlumInc(0, "");
                
               
            }

        }
     
        protected void lbnUsuarios_Click(object sender, EventArgs e)
        {
            pnlResponsables.Visible = false;
            pnlDependencias.Visible = false;
            PanelEncargadosEscuela.Visible = false;
            pnlUsuarios.Visible = true;
            pnlAlumnosIncorp.Visible = false;
            pnlRegistrarResponsable.Visible = false;
            txtExpediente.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlUnidad.Text = "";
            pnlRegistrarAdmon.Visible = false;
            pnlRegistrarResponsable.Visible=false;
            pnlRegistrarEncargado.Visible = false;
            pnlRegistrarDependencias.Visible=false;
            pnlRegistroAlumnos.Visible=false;
        }
        protected void lbnEncargadoEsc_Click(object sender, EventArgs e)
        {
            pnlAlumnosIncorp.Visible = false;
            pnlResponsables.Visible = false;
            PanelEncargadosEscuela.Visible = true;
            ddlUser.SelectedIndex = -1;
            txtExpediente.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlUnidad.Text = "";
            pnlDependencias.Visible = false;
            pnlRegistrarResponsable.Visible = false;
            pnlUsuarios.Visible = false;
            pnlRegistroAlumnos.Visible = false;
        }
        protected void lbnResponsable_Click(object sender, EventArgs e)
        {
            pnlAlumnosIncorp.Visible = false;
            pnlResponsables.Visible = true;
            PanelEncargadosEscuela.Visible = false;
            ddlUser.SelectedIndex = -1;
            txtExpediente.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlUnidad.Text = "";
            pnlDependencias.Visible = false;
            pnlUsuarios.Visible = false;
            pnlRegistroAlumnos.Visible = false;
        }

        protected void lbnDependencias_Click(object sender, EventArgs e)
        {
            pnlAlumnosIncorp.Visible = false;
            pnlResponsables.Visible = false;
            PanelEncargadosEscuela.Visible = false;
            pnlDependencias.Visible = true;
            ddlUser.SelectedIndex = -1;
            txtExpediente.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlUnidad.Text = "";
            pnlUsuarios.Visible = false;
            pnlRegistrarResponsable.Visible = false;
        }
        protected void lbnAlumnosIncorp_Click(object sender, EventArgs e)
        {
            pnlAlumnosIncorp.Visible = true;
            pnlResponsables.Visible = false;
            PanelEncargadosEscuela.Visible = false;
            pnlDependencias.Visible = false;
            ddlUser.SelectedIndex = -1;
            txtExpediente.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlUnidad.Text = "";
            pnlUsuarios.Visible = false;
            pnlRegistrarResponsable.Visible = false;
        }

        private int CurrentPage
        {
            get{ return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 0;}
            set{ ViewState["CurrentPage"] = value;}
        }

        private int TotalPages
        {
            get{return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0;}
            set{ ViewState["TotalPages"] = value;}
        }
        protected void CargarDatosEncargadosEscuela(int pageIndex, string searchTerm)
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;
            DataTable dt = ObtenerDatosEncarg(pageIndex, pageSize, searchTerm, out totalRecords);

            RepeaterEncarg.DataSource = dt;
            RepeaterEncarg.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPreviousEncarg.Enabled = pageIndex > 0;
            btnNextEncarg.Enabled = pageIndex < TotalPages - 1;

            // Actualiza la etiqueta de número de página
            lblPageNumberEncarg.Text = $"Página {pageIndex + 1} de {TotalPages}";
        }
        protected void CargarDatosResponsable(int pageIndex, string searchTerm)
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatosResponsable(pageIndex, pageSize, searchTerm, out totalRecords);

            repeaterResp.DataSource = dt;
            repeaterResp.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPreviousResp.Enabled = pageIndex > 0;
            btnNextResp.Enabled = pageIndex < TotalPages - 1;

            // Actualiza la etiqueta de número de página
            lblPageNumberResp.Text = $"Página {pageIndex + 1} de {TotalPages}";
        }

        protected void CargarDatosDependencia(int pageIndex, string searchTerm)
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatosDependencia(pageIndex, pageSize, searchTerm, out totalRecords);

            RepeaterDep.DataSource = dt;
            RepeaterDep.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPrevDep.Enabled = pageIndex > 0;
            btnNextDep.Enabled = pageIndex < TotalPages - 1;

            // Actualiza la etiqueta de número de página
            lblPageDep.Text = $"Página {pageIndex + 1} de {TotalPages}";
        }
        protected void CargarDatosAlumInc(int pageIndex, string searchTerm)
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatosAlumInc(pageIndex, pageSize, searchTerm, out totalRecords);

            RepeaterAlumnosIncorp.DataSource = dt;
            RepeaterAlumnosIncorp.DataBind();

            // Calcula el número total de páginas
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Configura el estado de los botones
            btnPrevAluInc.Enabled = pageIndex > 0;
            btnNextAluInc.Enabled = pageIndex < TotalPages - 1;

            // Actualiza la etiqueta de número de página
            lblPageAluInc.Text = $"Página {pageIndex + 1} de {TotalPages}";
        }
        protected DataTable ObtenerDatosAlumInc(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            // Consulta SQL para obtener los datos paginados
            string query = @"SELECT  
                                US.idUsuario AS ID, 
                                CONVERT(varchar, US.dFechaRegistro, 103) AS FechaRegistro, 
                                ALU.sMatricula AS Matricula,
                                PER.sApellido_Paterno + ' ' + PER.sApellido_Materno + ' ' + PER.sNombres AS sNombreCompleto,   
                                US.sCorreo, 
                                US.sPassword,
                                PLA.SCLAVE + ' - ' + PLA.SDESCRIPCION AS PlanEst,
                                ESC.SCLAVE + ' - ' + ESC.SDESCRIPCION AS Escuela
                                
                            FROM 
                                SM_USUARIOS_ALUMNOS AS USA
                            INNER JOIN  
                                SM_USUARIO AS US ON USA.kmUsuario = US.idUsuario
                            INNER JOIN  
                                SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno
                            INNER JOIN  
                                SP_PLAN_ESTUDIO AS PLA ON ALU.KPPLAN_ESTUDIOS = PLA.IDPLANESTUDIO
                            INNER JOIN  
                                SP_ESCUELA_UAC AS ESC ON ALU.KPESCUELASUADEC = ESC.IDEsCUELAUAC
                            INNER JOIN  
                                NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona
                            WHERE 
                                USA.bAutorizado = 11 AND sPassword IS NOT NULL";

            // Consulta SQL para contar el total de registros
            string countQuery =

                @"SELECT COUNT(*)     FROM 
                                SM_USUARIOS_ALUMNOS AS USA
                            INNER JOIN  
                                SM_USUARIO AS US ON USA.kmUsuario = US.idUsuario
                            INNER JOIN  
                                SM_ALUMNO AS ALU ON USA.kmAlumno = ALU.idAlumno
                            INNER JOIN  
                                SP_PLAN_ESTUDIO AS PLA ON ALU.KPPLAN_ESTUDIOS = PLA.IDPLANESTUDIO
                            INNER JOIN  
                                SP_ESCUELA_UAC AS ESC ON ALU.KPESCUELASUADEC = ESC.IDEsCUELAUAC
                            INNER JOIN  
                                NM_PERSONA AS PER ON ALU.kmPersona = PER.idPersona
                            WHERE 
                                USA.bAutorizado = 11 AND sPassword IS NOT NULL ";


            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition =
                    " AND (US.dFechaRegistro LIKE @searchTerm OR ALU.sMatricula LIKE @searchTerm OR US.sCorreo LIKE @searchTerm " +
                                         " OR PER.sApellido_Paterno + ' ' + PER.sApellido_Materno + ' ' + PER.sNombres LIKE @searchTerm " +
                                         "OR PLA.SDESCRIPCION LIKE @searchTerm OR ESC.SDESCRIPCION LIKE @searchTerm) ";

                query += searchCondition;
                countQuery += searchCondition;
            }

            query += " ORDER BY US.dFechaRegistro DESC " +
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
        protected DataTable ObtenerDatosEncarg(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            // Consulta SQL para obtener los datos paginados
            string query = "SELECT CONVERT(varchar, U.dFechaRegistro, 103) AS FechaRegistro, U.idUsuario,  U.sApellido_Pat + ' ' + U.sApellido_Mat + ' ' + U.sNombreUsuario AS Nombre, " +
               "U.sCorreo, TU.sDescripcion AS TipoUsuario,ESC.SCLAVE + ' - ' + ESC.SDESCRIPCION AS Escuela, UN.sCiudad AS Unidad, " + 
                "CASE U.bAutorizado WHEN '1' THEN  'AUTORIZADO' WHEN '0' THEN 'NO AUTORIZADO' ELSE '' END AS Estatus " +
                "FROM SM_USUARIO U "+
                "INNER JOIN SP_TIPO_USUARIO TU ON TU.idTipoUsuario = U.kpTipoUsuario " +
                "INNER JOIN NP_UNIDAD UN ON UN.idUnidad = U.kpUnidad "+
                "INNER JOIN SP_ESCUELA_UAC AS ESC ON U.kpEscuela = ESC.idEscuelauac "+
                "WHERE U.kpTipoUsuario = 4 ";

            // Consulta SQL para contar el total de registros
            string countQuery =

                "SELECT COUNT(*) " +
                "FROM SM_USUARIO U " +
                "INNER JOIN SP_TIPO_USUARIO TU ON TU.idTipoUsuario = U.kpTipoUsuario " +
                "INNER JOIN NP_UNIDAD UN ON UN.idUnidad = U.kpUnidad " +
                "INNER JOIN SP_ESCUELA_UAC AS ESC ON U.kpEscuela = ESC.idEscuelauac " +
                "WHERE U.kpTipoUsuario = 4 ";

            //"SELECT COUNT(*) " +
            //"FROM SM_USUARIO U " +
            //"INNER JOIN NM_EMPLEADO E ON E.idEmpleado = U.kmIdentificador " +
            //"INNER JOIN NM_PERSONA P ON P.idPersona = E.kmPersona " +
            //"INNER JOIN SP_TIPO_USUARIO TU ON TU.idTipoUsuario = U.kpTipoUsuario " +
            //"INNER JOIN NP_UNIDAD UN ON UN.idUnidad = U.kpUnidad " +
            //"INNER JOIN SP_ESCUELA_UAC AS ESC ON U.kpEscuela = ESC.idEscuelauac " +
            //"WHERE U.kpTipoUsuario = 4 ";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition =
                    "AND (U.sCorreo LIKE @searchTerm " +
                                         " OR U.sApellido_Pat + ' ' + U.sApellido_Mat + ' ' + U.sNombreUsuario LIKE @searchTerm " +
                                         "OR UN.sCiudad LIKE @searchTerm) ";
                //"AND (U.sCorreo LIKE @searchTerm " +
                //                     "OR E.sExpediente LIKE @searchTerm OR P.sApellido_paterno + ' ' + P.sApellido_materno + ' ' + P.sNombres LIKE @searchTerm " +
                //                     "OR UN.sCiudad LIKE @searchTerm) ";
                query += searchCondition;
                countQuery += searchCondition;
            }

            query += "ORDER BY 3 " +
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
        protected DataTable ObtenerDatosResponsable(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            // Consulta SQL para obtener los datos paginados
            string query = "SELECT U.idUsuario, E.sExpediente, P.sApellido_paterno + ' ' + P.sApellido_materno + ' ' + P.sNombres AS Nombre, " +
                "U.sCorreo, TU.sDescripcion AS TipoUsuario, UN.sCiudad AS Unidad, " +
                "CASE U.bAutorizado WHEN '1' THEN  'Autorizado' WHEN '0' THEN 'No Autorizado' ELSE '' END AS Estatus " +
                "FROM SM_USUARIO U " +
                "INNER JOIN NM_EMPLEADO E ON E.idEmpleado = U.kmIdentificador " +
                "INNER JOIN NM_PERSONA P ON P.idPersona = E.kmPersona " +
                "INNER JOIN SP_TIPO_USUARIO TU ON TU.idTipoUsuario = U.kpTipoUsuario " +
                "INNER JOIN NP_UNIDAD UN ON UN.idUnidad = U.kpUnidad " +
                "WHERE U.kpTipoUsuario = 3 ";

            // Consulta SQL para contar el total de registros
            string countQuery = "SELECT COUNT(*) " +
                "FROM SM_USUARIO U " +
                "INNER JOIN NM_EMPLEADO E ON E.idEmpleado = U.kmIdentificador " +
                "INNER JOIN NM_PERSONA P ON P.idPersona = E.kmPersona " +
                "INNER JOIN SP_TIPO_USUARIO TU ON TU.idTipoUsuario = U.kpTipoUsuario " +
                "INNER JOIN NP_UNIDAD UN ON UN.idUnidad = U.kpUnidad " +
                "WHERE U.kpTipoUsuario = 3 ";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition = "AND (U.sCorreo LIKE @searchTerm " +
                                         "OR E.sExpediente LIKE @searchTerm OR P.sApellido_paterno + ' ' + P.sApellido_materno + ' ' + P.sNombres LIKE @searchTerm " +
                                         "OR UN.sCiudad LIKE @searchTerm) ";
                query += searchCondition;
                countQuery += searchCondition;
            }

            query += "ORDER BY 3 " +
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

        protected DataTable ObtenerDatosDependencia(int pageIndex, int pageSize, string searchTerm, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string unidadUsuario = filtros.Split('|')[1];
            string filtroquery = "";

            // Consulta SQL para obtener los datos paginados
            string query = @"SELECT U.idUsuario, CONVERT(varchar, U.dFechaRegistro, 103) AS FechaRegistro,DS.sDescripcion, U.sCorreo, U.sPassword 
                            FROM SM_USUARIO U 
                            INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.kmUsuario = U.idUsuario
                            WHERE kpTipoUsuario = 2 ";

            if (tipoUsuario == "1")
            {
                filtroquery = "AND DS.kpUnidad IN (2,3,4) ";
            }
            else if (tipoUsuario == "3")
            {
                filtroquery = "AND DS.kpUnidad = " + unidadUsuario + " ";
            }
            query += filtroquery;

            // Consulta SQL para contar el total de registros
            string countQuery = "SELECT COUNT(*) " +
                "FROM SM_USUARIO U " +
               "INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON DS.kmUsuario = U.idUsuario " +
                "WHERE kpTipoUsuario = 2 ";
            countQuery += filtroquery;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchCondition = "AND (  U.sCorreo  LIKE @searchTerm) ";
                query += searchCondition;
                countQuery += searchCondition;
            }

            query += "ORDER BY  U.dFechaRegistro DESC " +
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
        protected void RepeaterAlumnosIncorp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                TextBox txtPassword = (TextBox)e.Item.FindControl("txtPassword");
                string s = txtPassword.Text;
                txtPassword.Text = SeguridadUtils.Desencriptar(s);
            }
        }
        protected void RepeaterAlumnosIncorp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Edit")
            {
                // Mostrar el modo de edición para la fila seleccionada
                Panel pnlViewMode = (Panel)RepeaterAlumnosIncorp.Items[index].FindControl("pnlViewModeAlumInc");
                Panel pnlEditMode = (Panel)RepeaterAlumnosIncorp.Items[index].FindControl("pnlEditModeAlumInc");

                pnlViewMode.Visible = false;
                pnlEditMode.Visible = true;

                ViewState["ActiveTab"] = "#tab2";
            }
            else if (e.CommandName == "Update")
            {
                // Lógica para actualizar los datos en la base de datos
                //Label txtDescripcion = (Label)RepeaterDep.Items[index].FindControl("lblDescripcion");
                TextBox txtCorreo = (TextBox)RepeaterAlumnosIncorp.Items[index].FindControl("txtCorreo");
                TextBox txtPassword = (TextBox)RepeaterAlumnosIncorp.Items[index].FindControl("txtPassword");

                int rowIndex = e.Item.ItemIndex;
                HiddenField hdnID = (HiddenField)RepeaterAlumnosIncorp.Items[rowIndex].FindControl("hdnID");
                int id = Convert.ToInt32(hdnID.Value);
                //string descripcion = txtDescripcion.Text;
                string correo = txtCorreo.Text;
                string password = SeguridadUtils.Encriptar(txtPassword.Text);
                // Actualiza los datos en la base de datos
                UpdateDataInDatabase(id, correo, password);

                // Vuelve al modo de visualización
                Panel pnlViewMode = (Panel)RepeaterAlumnosIncorp.Items[index].FindControl("pnlViewModeAlumInc");
                Panel pnlEditMode = (Panel)RepeaterAlumnosIncorp.Items[index].FindControl("pnlEditModeAlumInc");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;

                if (pnlEditMode.Visible)
                    pnlEditMode.CssClass = "edit-mode";

                string searchTerm = txtBuscarAlumInc.Text.Trim();
                int page = CurrentPage;
                if (string.IsNullOrEmpty(searchTerm))
                {
                    // Vuelve a enlazar los datos al Repeater
                    CargarDatosAlumInc(page, "");
                }
                else
                {
                    // Vuelve a enlazar los datos al Repeater
                    CargarDatosAlumInc(page, searchTerm);
                }

            }
            else if (e.CommandName == "Cancel")
            {
                // Vuelve al modo de visualización sin hacer cambios
                Panel pnlViewMode = (Panel)RepeaterAlumnosIncorp.Items[index].FindControl("pnlViewModeAlumInc");
                Panel pnlEditMode = (Panel)RepeaterAlumnosIncorp.Items[index].FindControl("pnlEditModeAlumInc");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;
                //UpdatePanel1.Update();
            }

            if (e.CommandName == "Page")
            {
                int pageIndex = int.Parse(e.CommandArgument.ToString());
                CargarDatosAlumInc(pageIndex, "");
                //UpdatePanel1.Update();
            }
        }
        protected void RepeaterDep_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                TextBox txtPassword = (TextBox)e.Item.FindControl("txtPassword");
                string s = txtPassword.Text;
                txtPassword.Text = SeguridadUtils.Desencriptar(s);
            }
        }

        protected void RepeaterDep_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Edit")
            {
                // Mostrar el modo de edición para la fila seleccionada
                Panel pnlViewMode = (Panel)RepeaterDep.Items[index].FindControl("pnlViewModeDep");
                Panel pnlEditMode = (Panel)RepeaterDep.Items[index].FindControl("pnlEditModeDep");

                pnlViewMode.Visible = false;
                pnlEditMode.Visible = true;

                ViewState["ActiveTab"] = "#tab2";
            }
            else if (e.CommandName == "Update")
            {
                // Lógica para actualizar los datos en la base de datos
                //Label txtDescripcion = (Label)RepeaterDep.Items[index].FindControl("lblDescripcion");
                TextBox txtCorreo = (TextBox)RepeaterDep.Items[index].FindControl("txtCorreo");
                TextBox txtPassword = (TextBox)RepeaterDep.Items[index].FindControl("txtPassword");

                int rowIndex = e.Item.ItemIndex;
                HiddenField hdnID = (HiddenField)RepeaterDep.Items[rowIndex].FindControl("hdnID");
                int id = Convert.ToInt32(hdnID.Value);
                //string descripcion = txtDescripcion.Text;
                string correo = txtCorreo.Text;
                string password = SeguridadUtils.Encriptar(txtPassword.Text);
                // Actualiza los datos en la base de datos
                UpdateDataInDatabase(id, correo, password);

                // Vuelve al modo de visualización
                Panel pnlViewMode = (Panel)RepeaterDep.Items[index].FindControl("pnlViewModeDep");
                Panel pnlEditMode = (Panel)RepeaterDep.Items[index].FindControl("pnlEditModeDep");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;

                if (pnlEditMode.Visible)
                    pnlEditMode.CssClass = "edit-mode";

                string searchTerm = txtBusquedaDependencias.Text.Trim();
                int page = CurrentPage;
                if (string.IsNullOrEmpty(searchTerm))
                {
                    // Vuelve a enlazar los datos al Repeater
                    CargarDatosDependencia(page, "");
                }
                else
                {
                    // Vuelve a enlazar los datos al Repeater
                    CargarDatosDependencia(page, searchTerm);
                }
               
            }
            else if (e.CommandName == "Cancel")
            {
                // Vuelve al modo de visualización sin hacer cambios
                Panel pnlViewMode = (Panel)RepeaterDep.Items[index].FindControl("pnlViewModeDep");
                Panel pnlEditMode = (Panel)RepeaterDep.Items[index].FindControl("pnlEditModeDep");

                pnlViewMode.Visible = true;
                pnlEditMode.Visible = false;
                //UpdatePanel1.Update();
            }

            if (e.CommandName == "Page")
            {
                int pageIndex = int.Parse(e.CommandArgument.ToString());
                CargarDatosDependencia(pageIndex, "");
                //UpdatePanel1.Update();
            }
        }
        protected void btnPrevAluInc_Click(object sender, EventArgs e)
        {

            string searchTerm = txtBuscarAlumInc.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatosAlumInc(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatosAlumInc(CurrentPage, searchTerm);
                }
            }

        }

        protected void btnNextAluIn_Click(object sender, EventArgs e)
        {
            string searchTerm = txtBuscarAlumInc.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatosAlumInc(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatosAlumInc(CurrentPage, searchTerm);
                }
            }

        }
        protected void btnPrevDep_Click(object sender, EventArgs e)
        {

            string searchTerm = txtBusquedaDependencias.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatosDependencia(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatosDependencia(CurrentPage, searchTerm);
                }
            }

        }

        protected void btnNextDep_Click(object sender, EventArgs e)
        {
            string searchTerm = txtBusquedaDependencias.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatosDependencia(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatosDependencia(CurrentPage, searchTerm);
                }
            }

        }

        protected void btnPrevEncarg_Click(object sender, EventArgs e)
        {

            string searchTerm = txtBuscarEncargado.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatosEncargadosEscuela(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage > 0)
                {
                    CurrentPage -= 1;
                    CargarDatosEncargadosEscuela(CurrentPage, searchTerm);
                }
            }

        }

        protected void btnNextEncarg_Click(object sender, EventArgs e)
        {
            string searchTerm = txtBuscarEncargado.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatosEncargadosEscuela(CurrentPage, "");
                }
            }
            else
            {
                if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
                {
                    CurrentPage += 1;
                    CargarDatosEncargadosEscuela(CurrentPage, searchTerm);
                }
            }

        }


        protected void btnBuscarDependencia_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtBusquedaDependencias.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatosDependencia(CurrentPage, searchTerm);
        }

        protected void btnBuscarEncargado_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtBuscarEncargado.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatosEncargadosEscuela(CurrentPage, searchTerm);
        }
        protected void btnBuscarAlumInc_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtBuscarAlumInc.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatosAlumInc(CurrentPage, searchTerm);
        }

        private void UpdateDataInDatabase(int id, string correo, string password)
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
                    using (SqlCommand cmd1 = new SqlCommand("UPDATE SM_USUARIO SET sCorreo = @sCorreo, sPassword = @sPassword " +
                        "WHERE idUsuario = @id", connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@sCorreo", correo);
                        cmd1.Parameters.AddWithValue("@sPassword", password);

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

        protected void btnPreviousResp_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage -= 1;
                CargarDatosResponsable(CurrentPage, "");
            }
        }

        protected void btnNextResp_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages - 1) // Asegúrate de no exceder el número total de páginas
            {
                CurrentPage += 1;
                CargarDatosResponsable(CurrentPage, "");
            }
        }

        protected void btnBuscarResponsable_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            string searchTerm = txtBusquedaResponsables.Text.Trim();

            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            // Carga los datos con el término de búsqueda y la página actual
            CargarDatosResponsable(CurrentPage, searchTerm);
        }
        

        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            LinkButton lnkUpdate = (LinkButton)sender;
            string id = lnkUpdate.CommandArgument;
            string cambio = "1";
            cambiarEstatus(id, cambio);
            mensajeScript("Registrado Autorizado con éxito");
            CargarDatosResponsable(0, "");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {

            LinkButton lnkUpdate = (LinkButton)sender;
            string id = lnkUpdate.CommandArgument;
            string cambio = "0";
            cambiarEstatus(id, cambio);
            mensajeScript("Registrado NO Autorizado con éxito");
            CargarDatosResponsable(0, "");
        }

        protected void cambiarEstatus(string id, string cambio)
        {
            string connectionString = GlobalConstants.SQL;
            string updateQuery = "UPDATE SM_USUARIO SET bAutorizado = @bAutorizado WHERE idUsuario = @idUsuario";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Parámetros
                    command.Parameters.AddWithValue("@bAutorizado", cambio);
                    command.Parameters.AddWithValue("@idUsuario", id);

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

        public void mensajeScript(string mensaje)
        {
            string scriptText = "alert('" + mensaje + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }

        //protected void repeaterResp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        DataRowView row = (DataRowView)e.Item.DataItem;
        //        LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
        //        LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");

        //        string autorizado = row["Estatus"].ToString().Trim();

        //        switch (autorizado)
        //        {
        //            case "Autorizado":
        //                btnAutorizar.Visible = false;
        //                btnEliminar.Visible = true;
        //                break;

        //            case "No Autorizado":
        //                btnAutorizar.Visible = true;
        //                btnEliminar.Visible = false;
        //                break;

        //            default:
        //                btnAutorizar.Visible = true;
        //                btnEliminar.Visible = true;
        //                break;
        //        }
        //    }

        //}

        // REGISTRO DE RESPONSABLES
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string _idExpediente = getIdEmpleado(txtExpediente.Text);
                if (!string.IsNullOrEmpty(_idExpediente))
                {
                    string connectionString = GlobalConstants.SQL;
                    string _correo = txtEmail.Text.Trim();
                    string _telefono = txtTelefono.Text.Trim();
                    string _unidad = ddlUnidad.SelectedValue;
                    string _tipo = "3";
                    string _nombre = txtNombre.Text.Trim();
                    string _apepat = txtApePat.Text.Trim();
                    string _apemat = txtApeMat.Text.Trim();

                    // Depurar valores
                    //System.Diagnostics.Debug.WriteLine("Nombre: " + _nombre);
                    //System.Diagnostics.Debug.WriteLine("Apellido Paterno: " + _apepat);
                    //System.Diagnostics.Debug.WriteLine("Apellido Materno: " + _apemat);

                    string Concatenado = _idExpediente + " - " + _nombre + " " + _apepat + " " + _apemat;

                    if (!verificarExistente(_correo))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                insertUser(_correo, _telefono, _tipo, _unidad, _idExpediente, Concatenado, connection, transaction);

                                transaction.Commit();
                                pnlRegistrarResponsable.Visible = false;
                                ddlUser.SelectedIndex = -1;
                                mensajeScriptExito("El usuario ha sido creado con éxito.");
                                LimpiarDatos();
                            }
                            catch (Exception ex)
                            {
                                // Si ocurre algún error, realizar un rollback de la transacción
                                transaction.Rollback();

                                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                Response.Write("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        lblMensajeError.Text = "El correo ingresado ya se encuentra registrado.";
                    }
                }
                else
                {
                    lblMensajeError.Text = "Ha ocurrido un error al intentar guardar el registro, contacte al administrador";
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "Ha ocurrido un error: " + ex.Message;
            }

        }
        protected void btnRegistrarAdmon_Click(object sender, EventArgs e)
        {
            try
            {
                string _idExpediente = getIdEmpleado(txt1.Text);
                if (_idExpediente != null || _idExpediente != "")
                {
                    string connectionString = GlobalConstants.SQL;
                    string _correo = txt2.Text.Trim();
                    string _telefono = txt6.Text.Trim();
                    string _unidad = DDL1.SelectedValue;
                    string _tipo = "1";
                    string _nombre = txt3.Text.Trim();
                    string _apepat = txt4.Text.Trim();
                    string _apemat = txt5.Text.Trim();
                    string Concatenado = _idExpediente + " - " + _nombre + " " + _apepat + " " + _apemat;
                    if (verificarExistente(_correo) == false)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                insertUser(_correo, _telefono, _tipo, _unidad, _idExpediente, Concatenado, connection, transaction);

                                transaction.Commit();
                                pnlRegistrarAdmon.Visible = false;
                                ddlUser.SelectedIndex = -1;
                                mensajeScriptExito("El usuario ha sido creado con éxito.");
                                LimpiarDatos();
                            }
                            catch (Exception ex)
                            {
                                // Si ocurre algún error, realizar un rollback de la transacción
                                transaction.Rollback();

                                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                Response.Write("Error: " + ex.Message);

                            }
                        }
                    }
                    else
                    {
                        Label2.Text = "El correo ingresado ya se encuentra registrado.";
                    }
                }
                else
                {
                    Label2.Text = "Ha ocurrido un error al intentar guardar el registro, contecte al administrador";
                }

            }
            catch
            {

            }

        }
        private void insertUser(string correo, string telefono, string tipo, string unidad, string expediente, string Concatenado ,SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO SM_USUARIO (sCorreo, sTelefono, kpTipoUsuario, kpUnidad, kmIdentificador, sNombreUsuario, bAutorizado) " +
                "VALUES (@sCorreo, @sTelefono, @kpTipoUsuario, @kpUnidad, @kmIdentificador, @sNombreUsuario, 1);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sCorreo", correo);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", tipo);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kmIdentificador", expediente);
                cmd.Parameters.AddWithValue("@sNombreUsuario", Concatenado);
                cmd.ExecuteNonQuery();
            }
        }
        [WebMethod]
        public static List<string> buscarCorreo(string correo, string exp)
        {
            List<string> results = new List<string>();
            string connectionString = GlobalConstants.ORA;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string sqlQuery = "SELECT TRIM(GE.NOMBRE) NOMBRE, TRIM(GE.APE_PAT) APE_PAT, TRIM(GE.APE_MAT) APE_MAT, EE.UADEC_EMAIL AS EMAIL " +
                    "FROM MAILEDU.EMPLEADO_EMAIL EE " +
                    "INNER JOIN OM.GEMPLEADO GE ON GE.EXPEDIENTE = EE.EXPEDIENTE " +
                    "WHERE EE.UADEC_EMAIL LIKE :email AND GE.EXPEDIENTE = :exp " +
                    "UNION ALL " +
                    "SELECT TRIM(NOMBRE) NOMBRE, TRIM(APE_PAT) APE_PAT, TRIM(APE_MAT) APE_MAT, EMAIL " +
                    "FROM PLANTADOCENTE.DOCENTEINCORPORADO " +
                    "WHERE EMAIL LIKE :email AND EXPEDIENTE = :exp";
                using (OracleCommand cmd = new OracleCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("email", correo));
                    cmd.Parameters.Add(new OracleParameter("exp", exp));
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader["NOMBRE"].ToString());
                            results.Add(reader["APE_PAT"].ToString());
                            results.Add(reader["APE_MAT"].ToString());
                        }
                    }
                }
            }
            return results;
        }

        public static List<string> buscarCorreo2(string correo, string exp)
        {
            List<string> results = new List<string>();
            string connectionString = GlobalConstants.ORA;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string sqlQuery = "SELECT TRIM(GE.NOMBRE) NOMBRE, TRIM(GE.APE_PAT) APE_PAT, TRIM(GE.APE_MAT) APE_MAT, EE.UADEC_EMAIL AS EMAIL " +
                    "FROM MAILEDU.EMPLEADO_EMAIL EE " +
                    "INNER JOIN OM.GEMPLEADO GE ON GE.EXPEDIENTE = EE.EXPEDIENTE " +
                    "WHERE EE.UADEC_EMAIL LIKE :email AND GE.EXPEDIENTE = :exp " +
                    "UNION ALL " +
                    "SELECT TRIM(NOMBRE) NOMBRE, TRIM(APE_PAT) APE_PAT, TRIM(APE_MAT) APE_MAT, EMAIL " +
                    "FROM PLANTADOCENTE.DOCENTEINCORPORADO " +
                    "WHERE EMAIL LIKE :email AND EXPEDIENTE = :exp";
                using (OracleCommand cmd = new OracleCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("email", correo));
                    cmd.Parameters.Add(new OracleParameter("exp", exp));
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader["NOMBRE"].ToString());
                            results.Add(reader["APE_PAT"].ToString());
                            results.Add(reader["APE_MAT"].ToString());
                        }
                    }
                }
            }
            return results;
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

        public void cargarUnidades()
        {
            string conString = GlobalConstants.SQL;
            string query = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1);";

            // Agregar una opción predeterminada al DropDownList
            ddlUnidad.Items.Add(new ListItem("Selecciona una opción", ""));
            DDL1.Items.Add(new ListItem("Selecciona una opción", ""));

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                // Ejecutar la consulta y leer los resultados
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Añadir cada nombre a un elemento ListItem y agregarlo al DropDownList
                    ddlUnidad.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                    DDL1.Items.Add(new ListItem(reader["sCiudad"].ToString(), reader["idUnidad"].ToString()));
                }

                // Cerrar la conexión
                con.Close();
            }
        }

        public string getIdEmpleado(string expediente)
        {
            string idExpediente = "";
            string conString = GlobalConstants.SQL;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                string query = "SELECT idEmpleado FROM NM_EMPLEADO WHERE sExpediente = @Expediente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Expediente", expediente);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        idExpediente = result.ToString();
                    }
                    else
                        idExpediente = null;
                }
            }

            return idExpediente;
        }

        public bool verificarExistente(string correo)
        {
            string idUser = ddlUser.SelectedValue;
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real

            // Query para verificar si el correo existe en la base de datos
            string query = "SELECT COUNT(*) FROM SM_USUARIO WHERE sCorreo = @Email and kpTipoUsuario =" + idUser;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", correo);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }

        }
        //public bool verificarExistenteAdmon(string correo)
        //{
        //    string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real
            
        //    // Query para verificar si el correo existe en la base de datos
        //    string query = "SELECT COUNT(*) FROM SM_USUARIO WHERE sCorreo = @Email and kpTipoUsuario = 1";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Email", correo);

        //            connection.Open();
        //            int count = (int)command.ExecuteScalar();

        //            return count > 0;
        //        }
        //    }

        //}
        public void CargarUsuarios()
        {
            string conString = GlobalConstants.SQL;
            
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string queryString = "SELECT sDescripcion, idTipoUsuario FROM SP_TIPO_USUARIO ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione el Usuario...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlUser.DataSource = ds3;
                ddlUser.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlUser.DataValueField = "idTipoUsuario";
                ddlUser.DataBind();
            }
        }
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idUser = ddlUser.SelectedValue;
            // Verificar si el valor seleccionado en DDLNIVEL1 es 1
            //RESPONSABLE UNIDAD
            if (idUser == "3")
            {
                LimpiarDatos();
                pnlRegistrarResponsable.Visible = true;
                pnlRegistrarAdmon.Visible = false;
                pnlRegistrarEncargado.Visible = false;
                pnlRegistrarDependencias.Visible= false;
                pnlRegistroAlumnos.Visible = false;
               
                CargarDatosResponsable(0, "");
                repeaterResp.DataBind();
            }
            //ADMINISTRADOR
            else if (idUser == "1")
            {
                LimpiarDatos();
                pnlRegistrarAdmon.Visible = true;
                pnlRegistrarResponsable.Visible = false;
                pnlRegistrarEncargado.Visible = false;
                pnlRegistrarDependencias.Visible = false;
                pnlRegistroAlumnos.Visible = false;
               
            }
            //ENCARGADO ESCUELA
            else if (idUser == "4")
            {
                LimpiarDatos();
                pnlRegistrarEncargado.Visible = true;
                pnlRegistrarAdmon.Visible = false;
                pnlRegistrarResponsable.Visible = false;
                pnlRegistrarDependencias.Visible = false;
                pnlRegistroAlumnos.Visible = false;
                CargarUnidadEnc();
                CargarNivelEnc();
                CargaEscuelaEnc();
            }
            //DEPENDENCIAS
            else if (idUser == "2")
            {
                LimpiarDatos();
                pnlRegistrarDependencias.Visible = true;
                pnlRegistrarEncargado.Visible = false;
                pnlRegistrarAdmon.Visible = false;
                pnlRegistrarResponsable.Visible = false;
                pnlRegistroAlumnos.Visible = false;
                CargarUnidadDep();
                CargarOrganismoDep();


            }
            //ESTUDIANTES
            else if (idUser == "5")
            {
                LimpiarDatos();
                pnlRegistrarDependencias.Visible = false;
                pnlRegistrarEncargado.Visible = false;
                pnlRegistrarAdmon.Visible = false;
                pnlRegistrarResponsable.Visible = false;
                pnlRegistroAlumnos.Visible = true;



            }
        }
       public void LimpiarDatos ()
        {
            Label4.Text = "";
            lblMensajeError.Text = "";
            lblMensajeErrorDep.Text = "";
            lblMensajeErrorEnc.Text = "";
            //ADMINISTRADOR
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            txt4.Text = "";
            txt5.Text = "";
            txt6.Text = "";
            Label1.Text = "";
            Label2.Text = "";
            DDL1.SelectedIndex = -1;
           //RESPONSABLE
            txtExpediente.Text = "";
            txtEmail.Text = "";
            txtNombre.Text = "";
            txtApePat.Text = "";
            txtApeMat.Text = "";
            txtTelefono.Text = "";
            lblMensajeEmail.Text = "";
            lblMensajeExpediente.Text = "";
            ddlUnidad.SelectedIndex = -1;
            //ENCARGADO
            txtExpedienteEnc.Text = "";
            txtEmailEnc.Text = "";
            txtNombreEnc.Text = "";
            txtApePatEnc.Text = "";
            txtApeMatEnc.Text = "";
            txtTelefonoEnc.Text = "";
            //DEPENDENCIAS
            txtDependenciaDep.Text = "";
            txtContraDep.Text = "";
            txtReContraDep.Text = "";
            ddlUnidadDep.SelectedIndex = -1;
            ddlOrganismoDep.SelectedIndex = -1;
            txtResponsableDep.Text = "";
            txtAreaDep.Text = "";
            txtTelefonoDep.Text = "";
            txtDomicilioDep.Text = "";




        }
        public void mensajeScriptExito(string mensaje)
        {
            string scriptText = "$('.alert').remove(); $('body').prepend('<div class=\"alert alert-success alert-dismissible fade show\" role=\"alert\" style=\"background-color: #d4edda; color: #155724;\"><strong>" + mensaje + "</strong><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button></div>'); setTimeout(function() { $('.alert').alert('close'); }, 6000);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
        }
        #region ESTUDIANTES
        protected void ddlTipoEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _tipo = ddlTipoEscuela.SelectedValue;
            switch (_tipo)
            {
                case "1":
                    lblCorreoAl.Text = "Correo Institucional:";
                    pnlPasswordAl.Visible = false;
                    txtMatriculaAl.Text = "";
                    txtCorreoAl.Text = "";
                    break;
                case "2":
                    lblCorreoAl.Text = "Correo Personal:";
                    pnlPasswordAl.Visible = true;
                    txtMatriculaAl.Text = "";
                    break;
                default:
                    lblCorreoAl.Text = "Correo:";
                    pnlPasswordAl.Visible = false;
                    txtMatriculaAl.Text = "";
                    break;
            }
        }
        public string getidAlumno(string escuela, string plan, string matricula)
        {
            string idAlumno = "";
            string conString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                string query = "SELECT idAlumno FROM SM_ALUMNO WHERE sMatricula = @sMatricula AND kpPlan_estudios = @kpPlan_estudios AND kpEscuelasUadeC = @kpEscuelasUadeC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sMatricula", matricula);
                    command.Parameters.AddWithValue("@kpPlan_estudios", plan);
                    command.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        idAlumno = result.ToString();
                    }
                    else
                    {
                        // Si no se encuentra el alumno, se inserta
                        string insertQuery = "EXEC Oracle_Importar_Alumno_WS @sMatricula, @kpEscuelasUadeC, @kpPlan_estudios;";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@sMatricula", matricula);
                            insertCommand.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);
                            insertCommand.Parameters.AddWithValue("@kpPlan_estudios", plan);
                            insertCommand.ExecuteNonQuery();
                        }

                        // Intentamos obtener nuevamente el idAlumno
                        using (SqlCommand retryCommand = new SqlCommand(query, connection))
                        {
                            retryCommand.Parameters.AddWithValue("@sMatricula", matricula);
                            retryCommand.Parameters.AddWithValue("@kpPlan_estudios", plan);
                            retryCommand.Parameters.AddWithValue("@kpEscuelasUadeC", escuela);

                            object retryResult = retryCommand.ExecuteScalar();
                            if (retryResult != null)
                            {
                                idAlumno = retryResult.ToString();
                            }
                            else
                            {
                                idAlumno = null; // Si por alguna razón no se inserta correctamente
                            }
                        }
                    }
                }
            }

            return idAlumno;
        }
        public bool isIncorporada(string escuela)
        {
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real
            string result = "";
            // Query para verificar si el correo existe en la base de datos
            string query = @"SELECT (CASE WHEN sclave LIKE '1%' THEN 2 ELSE 1 END) INC 
                            FROM SP_ESCUELA_UAC
                            WHERE idEscuelaUAC =@idEscuelaUAC ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idEscuelaUAC", escuela);

                    connection.Open();
                    result = command.ExecuteScalar().ToString();
                }
            }

            //1 - Oficial ; 2 - Inc
            if (result == "1")
                return false;
            else return true;
        }
        public bool verificarCorreoExistente(string correo, string matricula)
        {
            string connectionString = GlobalConstants.ORA;

            string _correoLogin = correo.Split('@')[0];
            string _correoDominio = "@" + correo.Split('@')[1].Trim();
            string result = "";

            if (_correoDominio.Contains("@uadec.edu.mx"))
            {
                // Query para verificar si el correo existe en la base de datos
                string query = @"SELECT LOGIN 
                                 FROM MAILEDU.V_ALUMNOS_CON_CORREO 
                                 WHERE MATRICULA = :matricula";

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("matricula", matricula));

                        connection.Open();
                        try
                        {
                            result = command.ExecuteScalar().ToString();
                            if (result == _correoLogin)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            else
                return false;
        }
        public bool VerificarCorreo(string correo, string escuela, string plan, string matricula)
        {
            string connectionString = GlobalConstants.SQL; // Reemplaza esto con tu cadena de conexión real
            string idAlumno = getidAlumno(escuela, plan, matricula);

            // Query para verificar si el correo existe en la base de datos
            string query =  @"SELECT COUNT(*) 
                            FROM SM_USUARIO U 
                            INNER JOIN SM_USUARIOS_ALUMNOS UA ON UA.kmUsuario = U.idUsuario 
                            WHERE U.sCorreo = @Email AND UA.kpEscuela = @kpEscuela AND UA.kpPlan = @kpPlan AND U.kpTipoUsuario = 5 AND UA.bAutorizado <> 99";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", correo);
                    command.Parameters.AddWithValue("@kpEscuela", escuela);
                    command.Parameters.AddWithValue("@kpPlan", plan);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
        private int InsertarUsuario(string _correo, string _idAlumno, string _password, string _tipo, SqlConnection connection, SqlTransaction transaction)
        {
            string password = SeguridadUtils.Encriptar(_password);
            if (_tipo == "1")
            {
                string query = "INSERT INTO SM_USUARIO (sCorreo, kpTipoUsuario, kmIdentificador) VALUES (@sCorreo, @kpTipoUsuario, @kmIdentificador); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@sCorreo", _correo);
                    cmd.Parameters.AddWithValue("@kpTipoUsuario", 5);
                    cmd.Parameters.AddWithValue("@kmIdentificador", _idAlumno);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            else
            {
                string query = "INSERT INTO SM_USUARIO (sCorreo, kpTipoUsuario, sPassword, kmIdentificador) VALUES (@sCorreo, @kpTipoUsuario, @sPassword, @kmIdentificador); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@sCorreo", _correo);
                    cmd.Parameters.AddWithValue("@kpTipoUsuario", 5);
                    cmd.Parameters.AddWithValue("@sPassword", password);
                    cmd.Parameters.AddWithValue("@kmIdentificador", _idAlumno);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        private void InsertarUsuarioAlumno(int _idUsuario, string _idalumno, string _PlanEstudio, string _Escuela, string _sMatricula, string _semestre,
           SqlConnection connection, SqlTransaction transaction)
        {
            // Primero, insertamos los datos proporcionados al método
            InsertarSiNoExiste(_idUsuario, _idalumno, _PlanEstudio, _Escuela, _semestre, connection, transaction);

            // Buscar registros en SM_ALUMNO con diferente kpEscuela y kpPlan
            string searchQuery = "SELECT idAlumno, kpPlan_estudios, kpEscuelasUadeC " +
                "FROM SM_ALUMNO " +
                "WHERE sMatricula = @sMatricula AND (kpEscuelasUadeC <> @kpEscuela OR kpPlan_estudios <> @kpPlan) AND kpPlan_estudios <> 584 AND kpEstatus_Alumno IN (1,2,6); ";

            List<(string idAlumno, string kpPlan, string kpEscuela)> registros = new List<(string idAlumno, string kpPlan, string kpEscuela)>();


            using (SqlCommand searchCmd = new SqlCommand(searchQuery, connection, transaction))
            {
                searchCmd.Parameters.AddWithValue("@sMatricula", _sMatricula);
                searchCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);
                searchCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);

                using (SqlDataReader reader = searchCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Obtener los valores de kpPlan y kpEscuela encontrados
                        string foundIdAlumno = reader["idAlumno"].ToString();
                        string foundPlan = reader["kpPlan_estudios"].ToString();
                        string foundEscuela = reader["kpEscuelasUadeC"].ToString();

                        registros.Add((foundIdAlumno, foundPlan, foundEscuela));
                    }
                }
            }
            // Insertar los registros encontrados
            foreach (var (foundIdAlumno, foundPlan, foundEscuela) in registros)
            {
                InsertarSiNoExiste(_idUsuario, foundIdAlumno, foundPlan, foundEscuela, _semestre, connection, transaction);
            }
        }
        private void InsertarSiNoExiste(int _idUsuario, string _idalumno, string _PlanEstudio, string _Escuela, string _semestre, SqlConnection connection, SqlTransaction transaction)
        {
            // Verificar si el registro ya existe en SM_USUARIOS_ALUMNOS
            string checkQuery = "SELECT COUNT(*) FROM SM_USUARIOS_ALUMNOS WHERE kmAlumno = @kmAlumno AND kpPlan = @kpPlan AND kpEscuela = @kpEscuela AND bAutorizado <> 99 ";

            using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@kmAlumno", _idalumno);
                checkCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);
                checkCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);

                int count = (int)checkCmd.ExecuteScalar();

                // Si no existe, insertar el nuevo registro
                if (count == 0)
                {
                    // Obtener el valor de idCiclo desde SP_CICLO
                    string cicloQuery = "SELECT idCiclo FROM SP_CICLO WHERE bServicioSocial = 1";
                    int? kpPeriodo = null;

                    using (SqlCommand cicloCmd = new SqlCommand(cicloQuery, connection, transaction))
                    {
                        object result = cicloCmd.ExecuteScalar();
                        if (result != null)
                        {
                            kpPeriodo = Convert.ToInt32(result);
                        }
                    }

                    // Insertar el nuevo registro en SM_USUARIOS_ALUMNOS
                    string insertQuery = "INSERT INTO SM_USUARIOS_ALUMNOS (kmUsuario, kmAlumno, kpPlan, kpEscuela, kpPeriodo,iSemestre, bAutorizado) " +
                                         "VALUES (@kmUsuario, @kmAlumno, @kpPlan, @kpEscuela, @kpPeriodo, @iSemestre,@bAutorizado);";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@kmUsuario", _idUsuario);
                        insertCmd.Parameters.AddWithValue("@kmAlumno", _idalumno);
                        insertCmd.Parameters.AddWithValue("@kpPlan", _PlanEstudio);
                        insertCmd.Parameters.AddWithValue("@kpEscuela", _Escuela);
                        insertCmd.Parameters.AddWithValue("@iSemestre", _semestre);


                        if (kpPeriodo.HasValue)
                        {
                            insertCmd.Parameters.AddWithValue("@kpPeriodo", kpPeriodo.Value);
                        }
                        else
                        {
                            insertCmd.Parameters.AddWithValue("@kpPeriodo", DBNull.Value);
                        }

                        insertCmd.Parameters.AddWithValue("@bAutorizado", 20707);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        [WebMethod]
        public static AlumnoDto GetAlumnoInfo(string buscar)
        {
            string oracleConnectionString = GlobalConstants.ORA;
            string sqlConnectionString = GlobalConstants.SQL;

            AlumnoDto alumno = null;
            List<EscuelaDto> escuelas = new List<EscuelaDto>();
            List<PlanEstudioDto> planesEstudio = new List<PlanEstudioDto>();

            string claveEscuela = "";
            string nombreEscuela = "";
            string clavePlan = "";
            string nombrePlan = "";

            using (OracleConnection connection = new OracleConnection(oracleConnectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(@"
                    SELECT A.MATRICULA, A.NOM_COMP AS NOMBRE_ALUMNO, A.APE_PAT, A.APE_MAT, 
                           A.CURP, CASE WHEN A.SEXO = 1 THEN 'FEMENINO' ELSE 'MASCULINO' END AS SEXO, 
                           P.CLAVE, P.NOMBRE AS PLAN_ESTUDIO, U.UNI_ORG, U.NOM_UNI_OR, A.EMAIL
                    FROM AA.GALU_ESC E
                    JOIN AA.GALUMNOS A ON A.MATRICULA = E.MATRICULA
                    JOIN PLANESTUDIO.PLAN P ON P.CLAVE = E.CVE_PLAN
                    JOIN AA.GUNI_ORG U ON U.UNI_ORG = E.UNI_ORG
                    WHERE A.MATRICULA = :smatricula AND E.ESTATUS IN('AR', 'AI', 'NI')", connection)) /**/
                {
                    command.Parameters.Add(new OracleParameter("smatricula", buscar ?? string.Empty));
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            alumno = new AlumnoDto
                            {
                                Nombre = reader["NOMBRE_ALUMNO"].ToString(),
                                ApellidoPaterno = reader["APE_PAT"].ToString(),
                                ApellidoMaterno = reader["APE_MAT"].ToString()
                            };
                            claveEscuela = reader["UNI_ORG"].ToString();
                            nombreEscuela = reader["NOM_UNI_OR"].ToString();
                            clavePlan = reader["CLAVE"].ToString();
                            nombrePlan = reader["PLAN_ESTUDIO"].ToString();
                        }
                    }
                }
            }

            if (alumno != null)
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();

                    // Obtener las escuelas
                    using (SqlCommand sqlCommand = new SqlCommand(@"
                SELECT idEscuelaUAC, sClave, sDescripcion 
                FROM SP_ESCUELA_UAC 
                WHERE sClave = @claveEscuela ", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@claveEscuela", claveEscuela);
                        sqlCommand.Parameters.AddWithValue("@nombreEscuela", nombreEscuela);
                        using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                escuelas.Add(new EscuelaDto
                                {
                                    Id = sqlReader["idEscuelaUAC"].ToString(),
                                    Nombre = sqlReader["sDescripcion"].ToString()
                                });
                            }
                        }
                    }

                    // Obtener los planes de estudio
                    using (SqlCommand sqlCommand = new SqlCommand(@"
                SELECT idPlanEstudio, sClave, sDescripcion 
                FROM SP_PLAN_ESTUDIO 
                WHERE sClave = @clavePlan", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@clavePlan", clavePlan);
                        sqlCommand.Parameters.AddWithValue("@nombrePlan", nombrePlan);
                        using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                planesEstudio.Add(new PlanEstudioDto
                                {
                                    Id = sqlReader["idPlanEstudio"].ToString(),
                                    Nombre = sqlReader["sDescripcion"].ToString()
                                });
                            }
                        }
                    }
                }

                alumno.Escuelas = escuelas;
                alumno.PlanesEstudio = planesEstudio;
            }

            return alumno;
        }
        protected void btnRegistrarAl_Click(object sender, EventArgs e)
        {
            string tipoEscuela = ddlTipoEscuela.SelectedValue;
            string escuela = Request.Form[ddlEscuelaAl.UniqueID];
            string plan = Request.Form[ddlPlanEstudioAl.UniqueID];
            string matricula = txtMatriculaAl.Text;
            string correo = txtCorreoAl.Text.Trim();
            string idAlumno = getidAlumno(escuela, plan, matricula);
            string password = txtPasswordConfirmAl.Text.Trim();
            string connectionString = GlobalConstants.SQL;
            string semestre = txtSemestre.Text.Trim();

            if ((!isIncorporada(escuela) && tipoEscuela == "1") || (isIncorporada(escuela) && tipoEscuela == "2"))
            {
                if (tipoEscuela == "1")
                {
                    if (verificarCorreoExistente(correo, matricula))
                    {
                        if (VerificarCorreo(correo, escuela, plan, matricula))
                        {
                            lblErrorAl.Text = "Los datos ingresados ya se encuentran registrados";
                            txtMatriculaAl.Text = "";
                            txtCorreoAl.Text = "";
                            ddlTipoEscuela.SelectedValue = "";
                            ddlTipoEscuela.Focus();
                        }
                        else
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                SqlTransaction transaction = connection.BeginTransaction();

                                try
                                {
                                    // Primero, insertar en la tabla SM_USUARIO
                                    int idUsuarioInsertado = InsertarUsuario(correo, idAlumno, "", tipoEscuela, connection, transaction);

                                    // Luego, insertar en la tabla SM_DEPENDENCIA_SERVICIO usando el ID del usuario insertado
                                    InsertarUsuarioAlumno(idUsuarioInsertado, idAlumno, plan, escuela, matricula, semestre, connection, transaction);

                                    // Commit de la transacción si todo fue exitoso
                                    transaction.Commit();

                                    mensajeScriptExito("El usuario ha sido creado con éxito.");
                                    //pnlRegistro.Visible = false;
                                    //pnlRegistroExitoso.Visible = true;
                                    //string cambio = "1";
                                    //enviarCorreo(_email, cambio);
                                    ////Response.Redirect("Dependencias.aspx", false);
                                }
                                catch
                                {
                                    // Si ocurre algún error, realizar un rollback de la transacción
                                    transaction.Rollback();

                                    // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                    //Response.Write("Error: " + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        lblErrorAl.Text = "La matrícula ingresada no cuenta con correo institucional o el correo ingresado no corresponde a la matrícula.";
                        txtMatriculaAl.Text = "";
                        ddlTipoEscuela.Focus();
                    }
                }
                else
                {
                    if (VerificarCorreo(correo, escuela, plan, matricula))
                    {
                        lblErrorAl.Text = "Los datos ingresados ya se encuentran registrados";
                        txtMatriculaAl.Text = "";
                        txtCorreoAl.Text = "";
                        ddlTipoEscuela.SelectedValue = "";
                        ddlTipoEscuela.Focus();
                    }
                    else
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                // Primero, insertar en la tabla SM_USUARIO
                                int idUsuarioInsertado = InsertarUsuario(correo, idAlumno, password, tipoEscuela, connection, transaction);

                                // Luego, insertar en la tabla SM_DEPENDENCIA_SERVICIO usando el ID del usuario insertado
                                InsertarUsuarioAlumno(idUsuarioInsertado, idAlumno, plan, escuela, matricula, semestre, connection, transaction);

                                // Commit de la transacción si todo fue exitoso
                                transaction.Commit();


                                //pnlRegistro.Visible = false;
                                //pnlRegistroExitoso.Visible = true;
                                mensajeScriptExito("El usuario ha sido creado con éxito.");
                            }
                            catch
                            {
                                // Si ocurre algún error, realizar un rollback de la transacción
                                transaction.Rollback();

                                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                //Response.Write("Error: " + ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                lblErrorAl.Text = "La escuela seleccionada no corresponde al tipo de escuela.";
                txtMatriculaAl.Text = "";
                txtCorreoAl.Text = "";
                ddlTipoEscuela.SelectedValue = "";
                ddlTipoEscuela.Focus();
            }
        }
        [System.Web.Services.WebMethod]
        public static string GetPlanesEstudio(string escuelaId, string matricula)
        {
            List<PlanEstudioDto> planesEstudio = new List<PlanEstudioDto>();

            string connectionString = GlobalConstants.SQL;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_obtenerPlanesAlumno_ss", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@matricula", matricula);
                    command.Parameters.AddWithValue("@idEscuelaUAC", escuelaId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            planesEstudio.Add(new PlanEstudioDto
                            {
                                Id = reader["idPlanEstudio"].ToString(),
                                Nombre = reader["PlanEstudio"].ToString()
                            });
                        }
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(planesEstudio);

        }
        #endregion
        #region ENCARGADO
        //Filtro  UNIDAD NIVEL ESCUELA
        private void CargarUnidadEnc()
        {
            // Define la conexión SQL y la consulta
            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string queryString = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1) ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sCiudad"] = "Seleccione la Unidad...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlUnidadEnc.DataSource = ds3;
                ddlUnidadEnc.DataTextField = "sCiudad"; // Utiliza el alias "Descripcion" como texto visible
                ddlUnidadEnc.DataValueField = "idUnidad";
                ddlUnidadEnc.DataBind();
            }

        }

        private void CargarNivelEnc()
        {
            // Define la conexión SQL y la consulta
            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
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
                ddlNivelEnc.DataSource = ds3;
                ddlNivelEnc.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlNivelEnc.DataValueField = "idTipoNivel";
                ddlNivelEnc.DataBind();
            }

        }


        private void CargaEscuelaEnc()
        {
            ddlEscuelEnc.Items.Clear();
            string idUnidadSeleccionado = ddlUnidadEnc.SelectedValue;
            string idNivelSeleccionado = ddlNivelEnc.SelectedValue;

            // CARGAR ESCUELAS CON ENFOQUE DIFERENTE PBG
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT DISTINCT ESC.sClave + ' - ' + ESC.sDescripcion Descripcion, ");
            sb.AppendLine(@"       PLA.kpEscuela_UAdeC,PE.kpNivel, kpUnidad, ESC.idEscuelaUAC ");
            sb.AppendLine(@"FROM SM_PLAN_EST_ESCUELA AS PLA ");
            sb.AppendLine(@"INNER JOIN SP_ESCUELA_UAC AS ESC ON PLA.kpEscuela_UAdeC = ESC.idEscuelaUAC ");
            sb.AppendLine(@"INNER JOIN SP_PLAN_ESTUDIO AS PE ON PE.idPlanEstudio = PLA.kpPlan_Estudio ");
            sb.AppendLine(@"WHERE  PE.kpNivel=@idNivel AND ESC.kpUnidad = @idUnidad ");
            sb.AppendLine(@"AND    PE.bVigente=1 AND PE.bActivo=1 ");
            sb.AppendLine(@"ORDER BY ESC.sClave + ' - ' + ESC.sDescripcion");

            string queryString = sb.ToString();

            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(queryString, con);
                cmd.Parameters.AddWithValue("@idNivel", idNivelSeleccionado);
                cmd.Parameters.AddWithValue("@idUnidad", idUnidadSeleccionado);
                SqlDataReader reader = cmd.ExecuteReader();

                // Crear un nuevo DataTable que incluya el primer elemento "Seleccione la Escuela..."
                DataTable dt = new DataTable();
                dt.Columns.Add("Descripcion", typeof(string));
                dt.Columns.Add("kpEscuela_UAdeC", typeof(string));
                dt.Rows.Add("Seleccione la Escuela...", ""); // Agregar el primer elemento

                // Llenar el DataTable con los resultados de la consulta
                while (reader.Read())
                {
                    dt.Rows.Add(reader["Descripcion"], reader["kpEscuela_UAdeC"]);
                }

                // Llena DDLEscuela con los datos del DataTable
                ddlEscuelEnc.DataSource = dt;
                ddlEscuelEnc.DataTextField = "Descripcion";
                ddlEscuelEnc.DataValueField = "kpEscuela_UAdeC";
                ddlEscuelEnc.DataBind();
            }

        }


        protected void ddlNivelEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaEscuelaEnc();
        }
        protected void ddlUnidadEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivelEnc.SelectedIndex = -1;
        }

     
        protected void btnEncargado_Click(object sender, EventArgs e)
        {
            try
            {
                string _idExpediente = getIdEmpleado(txtExpedienteEnc.Text);
                if (_idExpediente != null || _idExpediente != "")
                {
                    string connectionString = GlobalConstants.SQL;
                    string _correo = txtEmailEnc.Text.Trim();
                    string _telefono = txtTelefonoEnc.Text.Trim();
                    string _unidad = ddlUnidadEnc.SelectedValue;

                    string _tipo = ddlUser.SelectedValue;
                    string _escuela = ddlEscuelEnc.SelectedValue;

                    string _nombre = txtNombreEnc.Text.Trim() + txtApePatEnc.Text.Trim() + txtApeMatEnc.Text.Trim();
                    if (verificarExistente(_correo) == false)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                insertEncargado(_correo, _telefono, _tipo, _unidad, _idExpediente, _escuela, connection, transaction);

                                transaction.Commit();
                                pnlRegistrarEncargado.Visible = false;
                                mensajeScriptExito("El usuario ha sido creado con éxito.");
                                ddlUser.SelectedIndex = -1;
                                LimpiarDatos();
                            }
                            catch (Exception ex)
                            {
                                // Si ocurre algún error, realizar un rollback de la transacción
                                transaction.Rollback();

                                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                Response.Write("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        lblMensajeErrorEnc.Text = "El correo ingresado ya se encuentra registrado.";
                    }
                }
                else
                {
                    lblMensajeErrorEnc.Text = "Ha ocurrido un error al intentar guardar el registro, contecte al administrador";
                }

            }
            catch (Exception ex)
            {
                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                Response.Write("Error: " + ex.Message);
            }
        }

        private void insertEncargado(string correo, string telefono, string tipo, string unidad, string expediente, string escuela, SqlConnection connection, SqlTransaction transaction)
        {
            ////////////////////////////////////////
            string query = "INSERT INTO SM_USUARIO (sCorreo, sTelefono, kpTipoUsuario, kpUnidad, kmIdentificador, bAutorizado, kpEscuela) " +
                "VALUES (@sCorreo, @sTelefono, @kpTipoUsuario, @kpUnidad, @kpEmpleado, 1, @kpEscuela);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sCorreo", correo);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", tipo);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kpEmpleado", expediente);
                cmd.Parameters.AddWithValue("@kpEscuela", escuela);
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region DEPENDENCIAS
        //Filtro  UNIDAD NIVEL ESCUELA
        private void CargarUnidadDep()
        {
            // Define la conexión SQL y la consulta
            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string queryString = "SELECT idUnidad, sCiudad FROM NP_UNIDAD WHERE idUnidad NOT IN (1) ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sCiudad"] = "Seleccione la Unidad...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlUnidadDep.DataSource = ds3;
                ddlUnidadDep.DataTextField = "sCiudad"; // Utiliza el alias "Descripcion" como texto visible
                ddlUnidadDep.DataValueField = "idUnidad";
                ddlUnidadDep.DataBind();
            }

        }

        private void CargarOrganismoDep()
        {
            // Define la conexión SQL y la consulta
            string conString = GlobalConstants.SQL;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string queryString = "select sClave, sDescripcion from SP_ORGANISMO";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione el Organismo...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlOrganismoDep.DataSource = ds3;
                ddlOrganismoDep.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlOrganismoDep.DataValueField = "sClave";
                ddlOrganismoDep.DataBind();
            }

        }


        protected void btnDependencia_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlUnidadDep.SelectedValue == "" || ddlOrganismoDep.SelectedValue == "")
                {
                    lblMensajeErrorDep.Text = "Debe seleccionar Unidad y Organismo.";
                    return;
                }

                string _idExpediente = getIdEmpleado(txtExpedienteEnc.Text);
                if (_idExpediente != null || _idExpediente != "")
                {
                    string connectionString = GlobalConstants.SQL;

                    string _password = SeguridadUtils.Encriptar(txtReContraDep.Text);
                    string _correo = txtCorreoDep.Text.Trim();
                    string _telefono = txtTelefonoDep.Text.Trim();
                    string _unidad = ddlUnidadDep.SelectedValue;

                    string _resp = txtResponsableDep.Text.Trim();
                    string _respArea = txtAreaDep.Text.Trim();
                    string _descrip = txtDependenciaDep.Text.Trim();
                    string _domicilio = txtDomicilioDep.Text.Trim();

                    string _tipo = ddlUser.SelectedValue;

                    if (verificarExistente(_correo) == false)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlTransaction transaction = connection.BeginTransaction();

                            try
                            {
                                insertEnUsuarioDep(_password, _correo, _telefono, _tipo, _unidad, null, null, null, connection, transaction, _resp, _respArea, _descrip, _domicilio);

                                transaction.Commit();
                                pnlRegistrarEncargado.Visible = false;
                                mensajeScriptExito("El usuario ha sido creado con éxito.");
                                LimpiarDatos();
                            }
                            catch (Exception ex)
                            {
                                // Si ocurre algún error, realizar un rollback de la transacción
                                transaction.Rollback();

                                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                                Response.Write("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        lblMensajeErrorDep.Text = "El correo ingresado ya se encuentra registrado.";
                    }
                }
                else
                {
                    lblMensajeErrorDep.Text = "Ha ocurrido un error al intentar guardar el registro, contecte al administrador";
                }

            }
            catch (Exception ex)
            {
                // Manejar el error (por ejemplo, mostrar un mensaje de error en la página)
                Response.Write("Error: " + ex.Message);
            }
        }

        private void insertEnUsuarioDep(string password, string correo, string telefono, string tipo, string unidad, string expediente, string plan, string escuela, SqlConnection connection, SqlTransaction transaction, string responsable, string respArea, string descripcion, string domicilio)
        {
            ////////////////////////////////////////
            string query = "INSERT INTO SM_USUARIO (sPassword, sCorreo, sTelefono, kpTipoUsuario, kpUnidad, kmIdentificador, bAutorizado, kpPlanEstudio, kpEscuela) " +
                "VALUES (@sPassword, @sCorreo, @sTelefono, @kpTipoUsuario, @kpUnidad, @kpEmpleado, 0, @kpPlanEstudio, @kpEscuela);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sPassword", password);
                cmd.Parameters.AddWithValue("@sCorreo", correo);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpTipoUsuario", tipo);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kpEmpleado", expediente);

                cmd.Parameters.AddWithValue("@kpPlanEstudio", plan);
                cmd.Parameters.AddWithValue("@kpEscuela", escuela);
                cmd.ExecuteNonQuery();
            }

            ////////////////////////////////////////
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"INSERT INTO SM_DEPENDENCIA_SERVICIO (idDependenicaServicio, kmUsuario, sAreaResponsable, sResponsable, sTelefono, kpUnidad, kpOrganismo, ");
            sb.AppendLine(@"             sDescripcion, sDomicilio, bAutorizado, dFechaRegistroDep, bVigente, kmValido, kmAutorizo)");
            sb.AppendLine(@"VALUES (scope_identity(), @sAreaResponsable, @sResponsable, @sTelefono, @kpUnidad, @kpOrganismo, ");
            sb.AppendLine(@"             @sDescripcion, @sDomicilio, NULL, @dFechaRegistroDep, 1)");

            string query2 = sb.ToString();

            using (SqlCommand cmd = new SqlCommand(query2, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sAreaResponsable", respArea);
                cmd.Parameters.AddWithValue("@sResponsable", responsable);
                cmd.Parameters.AddWithValue("@sTelefono", telefono);
                cmd.Parameters.AddWithValue("@kpUnidad", unidad);
                cmd.Parameters.AddWithValue("@kpOrganismo", expediente);
                cmd.Parameters.AddWithValue("@sDescripcion", descripcion);
                cmd.Parameters.AddWithValue("@sDomicilio", plan);
                cmd.Parameters.AddWithValue("@dFechaRegistroDep", DateTime.Now);
                cmd.ExecuteNonQuery();
            }


        }
        #endregion
    }
}