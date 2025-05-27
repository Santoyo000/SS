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
using System.Text;

namespace Servicio_Social
{
    public partial class ProgramasRegistrados : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos(0, "","","","","");
                CargarUnidad();
                CargarEstatus();
                CargarPeriodo();
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
                LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");  // Botón verde
                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");    // Botón rojo
                LinkButton btnEditarCupo = (LinkButton)e.Item.FindControl("btnEditarCupo");
                LinkButton bntEdit = (LinkButton)e.Item.FindControl("bntEdit");
                LinkButton btnDetalle2 = (LinkButton)e.Item.FindControl("btnDetalle2");

                DataRowView row = (DataRowView)e.Item.DataItem;

                if (Session["filtros"] != null)
                {
                    string usuario = Session["filtros"].ToString().Split('|')[0];

                    // Configuración base por tipo de usuario
                    if (usuario == "1") // ADMINISTRADOR
                    {
                        btnEditarCupo.Visible = true;
                        bntEdit.Visible = true;
                        btnDetalle2.Visible = false;

                        // Control según estatus del programa
                        string autorizado = row["kpEstatus_Programa"].ToString().Trim();

                        switch (autorizado)
                        {
                            case "20707": // EN ESPERA
                                btnAutorizar.Visible = true;
                                btnEliminar.Visible = true;
                                break;

                            case "2": // NO AUTORIZADO
                                btnAutorizar.Visible = true;
                                btnEliminar.Visible = false; // Oculta el de NO AUTORIZAR
                                break;

                            case "11": // AUTORIZADO
                                btnAutorizar.Visible = false; // Oculta el de AUTORIZAR
                                btnEliminar.Visible = true;
                                break;

                            default:
                                btnAutorizar.Visible = false;
                                btnEliminar.Visible = false;
                                break;
                        }
                    }
                    else if (usuario == "4") // ENCARGADO ESCUELA
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
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    // Obtén el panel
            //   // Panel pnlAutoriz = (Panel)e.Item.FindControl("pnlAutorizar");
            //    LinkButton btnAutorizar = (LinkButton)e.Item.FindControl("btnAutorizar");
            //    LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
            //    LinkButton btnEditarCupo = (LinkButton)e.Item.FindControl("btnEditarCupo");
            //    LinkButton bntEdit = (LinkButton)e.Item.FindControl("bntEdit");
            //    LinkButton btnDetalle2 = (LinkButton)e.Item.FindControl("btnDetalle2");
            //    DataRowView row = (DataRowView)e.Item.DataItem;

            //    // Obtén el usuario de la sesión
            //    if (Session["filtros"] != null)
            //    {
            //        string usuario = Session["filtros"].ToString().Split('|')[0];
            //        if (usuario == "1") // ADMON
            //        {
            //            btnAutorizar.Visible = true;
            //            btnEliminar.Visible = true;
            //            btnDetalle2.Visible = false;

            //        } 
            //        else if (usuario == "4") // ENCARGADO DE ESCUELA
            //        {
            //            btnAutorizar.Visible = false;
            //            btnEliminar.Visible = false;
            //            btnEditarCupo.Visible = false;
            //            bntEdit.Visible = true;
            //            btnDetalle2.Visible = true;
            //        }
            //        else if (usuario == "3") // DEPENDENCIA
            //        {
            //            btnAutorizar.Visible = false;
            //            btnEliminar.Visible = false;
            //            btnEditarCupo.Visible = false;
            //            bntEdit.Visible = false;
            //        }
            //        else if (usuario == "2") // RESPONSABLE UNIDAD
            //        {
            //            btnAutorizar.Visible = false;
            //            btnEliminar.Visible = false;
            //            btnEditarCupo.Visible = false;
            //            bntEdit.Visible = false;
            //            btnDetalle2.Visible = true;
            //        }
            //    }
            //string autorizado = row["kpEstatus_Programa"].ToString().Trim();

            //switch (autorizado)
            //{
            //    case "1":
            //        btnAutorizar.Visible = true;
            //        btnEliminar.Visible = true;
            //        break;

            //    case "2":
            //        btnAutorizar.Visible = true;
            //        btnEliminar.Visible = false;
            //        break;

            //    case "11":
            //        btnAutorizar.Visible = false;
            //        btnEliminar.Visible = true;
            //        break;

            //    default:
            //        btnAutorizar.Visible = true;
            //        btnEliminar.Visible = true;
            //        break;
            //}
            //}
        }
        protected void CargarDatos(int pageIndex, string NombreDep, string correoDep, string NombrePro, string Responsable,string Fecha)
        {
            int pageSize = 30; // Cantidad de registros por página
            int totalRecords;

            DataTable dt = ObtenerDatos(pageIndex, pageSize, NombreDep, correoDep, NombrePro, Responsable, Fecha, 
                                        ddlEstatus.SelectedValue, ddlPeriodo.SelectedValue, DDLUnidad.SelectedValue,  out totalRecords);

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
                    string Programa = txtPrograma.Text.Trim();
                    string Responsable = txtResponsable.Text.Trim();
                    string Fecha = txtFecha.Text.Trim();

                    // Recargar los datos con los filtros actuales
                    CargarDatos(CurrentPage, NombreDep, CorreoDep, Programa, Responsable, Fecha);
                }
            }
        }
        protected DataTable ObtenerDatos(int pageIndex, int pageSize, string NombreDep, string CorreoDep, string NomprePro, string Responsable, string Fecha, string selectedEstatus ,string selectedPeriodo, string selectedUnidad, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            string filtros = Session["filtros"].ToString();
            string tipoUsuario = filtros.Split('|')[0];
            string unidadUsuario = filtros.Split('|')[1];
            string escuela = tipoUsuario == "4" ? filtros.Split('|')[2] : "";

            List<string> condiciones = new List<string>();

            // Filtros comunes
            if (!string.IsNullOrEmpty(NombreDep))
                condiciones.Add("DS.sDescripcion LIKE @NombreDep");

            if (!string.IsNullOrEmpty(CorreoDep))
                condiciones.Add("USU.sCorreo LIKE @CorreoDep");

            if (!string.IsNullOrEmpty(NomprePro))
                condiciones.Add("PR.sNombre_Programa LIKE @NomprePro");

            if (!string.IsNullOrEmpty(Responsable))
                condiciones.Add("PR.sResponsable LIKE @Responsable");

            if (!string.IsNullOrEmpty(Fecha))
                condiciones.Add("CONVERT(VARCHAR, PR.DFECHAREGISTROP, 103) = @Fecha");

            if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                condiciones.Add("PR.kpEstatus_Programa = @selectedEstatus");

            if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                condiciones.Add("PR.kpPeriodo = @selectedPeriodo");

            if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                condiciones.Add("PR.kpUnidad = @selectedUnidad");

            // Restricciones por tipo de usuario
            if (tipoUsuario == "1")
            {
                condiciones.Add("PR.kpUnidad IN (2, 3, 4)");
            }
            else if (tipoUsuario == "3")
            {
                condiciones.Add("PR.kpUnidad = @unidadUsuario");
            }
            else if (tipoUsuario == "4")
            {
                condiciones.Add("PR.kpUnidad = @unidadUsuario");
                condiciones.Add("DP.kpEscuela = @escuela");
            }

            string joinDetallePrograma = tipoUsuario == "4" ? "INNER JOIN SM_DETALLE_PROGRAMA DP ON PR.idPrograma = DP.kmPrograma" : "";

            string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

            string query = $@"
        SELECT PR.idPrograma, CIC.sDescripcion AS Periodo,
               CONVERT(varchar, PR.DFECHAREGISTROP, 103) AS FechaRegistro,
               DS.sDescripcion AS Dependencia,
               USU.sCorreo AS Correo,
               PR.sNombre_Programa AS NombrePrograma,
               PR.sResponsable AS Responsable,
               PR.kpEstatus_Programa,
               UN.SCIUDAD AS UNIDAD,
               ES.sDescripcion AS Estatus
        FROM SM_PROGRAMA PR
        INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON PR.kpDependencia = DS.idDependenicaServicio
        INNER JOIN NP_ESTATUS ES ON PR.kpEstatus_Programa = ES.idEstatus
        INNER JOIN SM_USUARIO USU ON DS.kmUsuario = USU.idUsuario
        INNER JOIN NP_UNIDAD UN ON PR.KPUNIDAD = UN.IDUNIDAD
        INNER JOIN SP_CICLO CIC ON PR.kpPeriodo = CIC.idCiclo

        {joinDetallePrograma}
        {whereClause}
        ORDER BY PR.kpEstatus_Programa DESC
        OFFSET @rowsToSkip ROWS FETCH NEXT @pageSize ROWS ONLY;
    ";

            string countQuery = $@"
        SELECT COUNT(*)
        FROM SM_PROGRAMA PR
        INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON PR.kpDependencia = DS.idDependenicaServicio
        INNER JOIN NP_ESTATUS ES ON PR.kpEstatus_Programa = ES.idEstatus
        INNER JOIN SM_USUARIO USU ON DS.kmUsuario = USU.idUsuario
        INNER JOIN NP_UNIDAD UN ON PR.KPUNIDAD = UN.IDUNIDAD
        {joinDetallePrograma}
        {whereClause};
    ";

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlCommand countCmd = new SqlCommand(countQuery, con);

                // Parámetros
                if (!string.IsNullOrEmpty(NombreDep))
                {
                    cmd.Parameters.AddWithValue("@NombreDep", $"%{NombreDep}%");
                    countCmd.Parameters.AddWithValue("@NombreDep", $"%{NombreDep}%");
                }

                if (!string.IsNullOrEmpty(CorreoDep))
                {
                    cmd.Parameters.AddWithValue("@CorreoDep", $"%{CorreoDep}%");
                    countCmd.Parameters.AddWithValue("@CorreoDep", $"%{CorreoDep}%");
                }

                if (!string.IsNullOrEmpty(NomprePro))
                {
                    cmd.Parameters.AddWithValue("@NomprePro", $"%{NomprePro}%");
                    countCmd.Parameters.AddWithValue("@NomprePro", $"%{NomprePro}%");
                }

                if (!string.IsNullOrEmpty(Responsable))
                {
                    cmd.Parameters.AddWithValue("@Responsable", $"%{Responsable}%");
                    countCmd.Parameters.AddWithValue("@Responsable", $"%{Responsable}%");
                }

                if (!string.IsNullOrEmpty(Fecha))
                {
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    countCmd.Parameters.AddWithValue("@Fecha", Fecha);
                }

                if (!string.IsNullOrEmpty(selectedEstatus) && selectedEstatus != "0")
                {
                    cmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                    countCmd.Parameters.AddWithValue("@selectedEstatus", selectedEstatus);
                }

                if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                {
                    cmd.Parameters.AddWithValue("@selectedPeriodo", selectedPeriodo);
                    countCmd.Parameters.AddWithValue("@selectedPeriodo", selectedPeriodo);
                }

                if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                {
                    cmd.Parameters.AddWithValue("@selectedUnidad", selectedUnidad);
                    countCmd.Parameters.AddWithValue("@selectedUnidad", selectedUnidad);
                }

                if (tipoUsuario == "3" || tipoUsuario == "4")
                {
                    cmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                    countCmd.Parameters.AddWithValue("@unidadUsuario", unidadUsuario);
                }

                if (tipoUsuario == "4")
                {
                    cmd.Parameters.AddWithValue("@escuela", escuela);
                    countCmd.Parameters.AddWithValue("@escuela", escuela);
                }

                cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                con.Open();
                totalRecords = (int)countCmd.ExecuteScalar();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            return dt;
            //    string filtros = Session["filtros"].ToString();
            //    string tipoUsuario = filtros.Split('|')[0];
            //    string unidadUsuario = filtros.Split('|')[1];
            //    string Escuela = "";
            //    if(tipoUsuario == "4")
            //    { 
            //      Escuela = filtros.Split('|')[2];
            //    }
            //    string filtroquery = "";

            //    string conString = GlobalConstants.SQL;
            //    int rowsToSkip = pageIndex * pageSize;

            //    // Consulta SQL para obtener los datos paginados
            //    string query = @"SELECT PR.idPrograma,CONVERT(varchar, DFECHAREGISTROP, 103) AS FechaRegistro, DS.sDescripcion AS Dependencia, USU.sCorreo AS Correo , PR.sNombre_Programa AS NombrePrograma,
            //                     PR.sResponsable AS Responsable, PR.kpEstatus_Programa ,  UN.SCIUDAD AS UNIDAD,ES.sDescripcion AS Estatus 
            //FROM SM_PROGRAMA AS PR 
            //INNER JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio 
            //                     INNER JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus 
            //                     INNER JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario
            //INNER JOIN NP_UNIDAD AS UN ON PR.KPUNIDAD = UN.IDUNIDAD  ";

            //    if (tipoUsuario == "1")
            //    {
            //        filtroquery = " WHERE PR.kpUnidad IN (2,3,4) ";
            //    }
            //    else if (tipoUsuario == "3")
            //    {
            //        filtroquery = " WHERE PR.kpUnidad = " + unidadUsuario + " ";
            //    }
            //    else if (tipoUsuario == "4")
            //    {
            //        filtroquery = " JOIN SM_DETALLE_PROGRAMA AS DP ON PR.idPrograma= DP.kmPrograma WHERE PR.kpUnidad = " + unidadUsuario + " AND DP.kpEscuela= " + Escuela + " ";
            //    }
            //    query += filtroquery;
            //    // Consulta SQL para contar el total de registros
            //    string countQuery = @"SELECT COUNT(*)
            //                     FROM SM_PROGRAMA AS PR
            //                     INNER JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio
            //                     INNER JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus
            //                     INNER JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario
            //                     INNER JOIN NP_UNIDAD AS UN ON PR.KPUNIDAD = UN.IDUNIDAD ";
            //    countQuery += filtroquery;
            //    if (!string.IsNullOrEmpty(searchTerm))
            //    {
            //        string searchCondition = "AND DS.sDescripcion LIKE @searchTerm OR ES.sDescripcion LIKE @searchTerm " +
            //                                 "OR DS.sResponsable LIKE @searchTerm OR PR.sNombre_Programa LIKE @searchTerm " +
            //                                 "OR USU.sCorreo LIKE @searchTerm ";
            //        query += searchCondition;
            //        countQuery += searchCondition;
            //    }

            //    query += " ORDER BY PR.kpEstatus_Programa DESC " +
            //             " OFFSET @rowsToSkip ROWS " +
            //             " FETCH NEXT @pageSize ROWS ONLY;";

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

        }
        private void CargarPeriodo()
        {
            // Define la conexión SQL y la consulta
            using (SqlConnection con = new SqlConnection(SQL))
            {
                con.Open();
                string queryString = @"SELECT idCiclo, sDescripcion FROM SP_CICLO 
                                        WHERE dFecha_Inicio >='2024-08-05 00:00:00.000' 
                                        AND idCiclo NOT IN (0,34)  ";

                // Crea un DataSet para almacenar los resultados de la consulta
                DataSet ds3 = new DataSet();

                // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                {
                    data.Fill(ds3);
                }
                DataTable dt = ds3.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["sDescripcion"] = "Seleccione el Periodo Escolar...";
                dt.Rows.InsertAt(newRow, 0);

                // Asigna los resultados al DropDownList
                ddlPeriodo.DataSource = ds3;
                ddlPeriodo.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                ddlPeriodo.DataValueField = "idCiclo";
                ddlPeriodo.DataBind();
            }

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
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            string NombreDep = txtDependencia.Text.Trim();
            string CorreoDep = txtCorreo.Text.Trim();
            string Programa = txtPrograma.Text.Trim();
            string Responsable = txtResponsable.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            if (CurrentPage > 0)
            {
                CurrentPage -= 1;
               

                // Recargar los datos con los filtros actuales
                CargarDatos(CurrentPage, NombreDep, CorreoDep, Programa, Responsable, Fecha);
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            string NombreDep = txtDependencia.Text.Trim();
            string CorreoDep = txtCorreo.Text.Trim();
            string Programa = txtPrograma.Text.Trim();
            string Responsable = txtResponsable.Text.Trim();
            string Fecha = txtFecha.Text.Trim();

            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage += 1;
                CargarDatos(CurrentPage, NombreDep, CorreoDep, Programa, Responsable, Fecha);
            }
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
        //protected void btnExportarExcel_Click(object sender, EventArgs e)
        //{
        //    // Verificar si el Repeater contiene elementos
        //    if (Repeater1.Items.Count > 0)
        //    {
        //        // Crear el archivo Excel en memoria
        //        using (ExcelPackage excelPackage = new ExcelPackage())
        //        {
        //            // Agregar una hoja de trabajo al archivo Excel
        //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Programas");

        //            // Definir los encabezados de las columnas
        //            string[] headers = { "Fecha de Registro", "Dependencia", "Correo", "Nombre del Programa", "Responsable", "Estatus" };
        //            // Escribir los encabezados en la primera fila
        //            for (int i = 0; i < headers.Length; i++)
        //            {
        //                worksheet.Cells[1, i + 1].Value = headers[i];
        //            }
        //            int rowIndex = 2; // Comenzar desde la fila 2 (después de los encabezados)

        //            foreach (RepeaterItem item in Repeater1.Items)
        //            {
                       
        //                    TextBox txtFechaRegistro = (TextBox)item.FindControl("txtFechaRegistro");
        //                    TextBox txtDependencia = (TextBox)item.FindControl("txtDependencia");
        //                    TextBox txtCorreo = (TextBox)item.FindControl("txtCorreo");
        //                    TextBox txtNombrePrograma = (TextBox)item.FindControl("txtNombrePrograma");
        //                    TextBox txtResponsable = (TextBox)item.FindControl("txtResponsable");
        //                    Label lblEstatus = (Label)item.FindControl("lblEstatus");

        //                    // Escribir los datos en las celdas correspondientes
        //                    worksheet.Cells[rowIndex, 1].Value = txtFechaRegistro?.Text ?? "";
        //                    worksheet.Cells[rowIndex, 2].Value = txtDependencia?.Text ?? "";
        //                    worksheet.Cells[rowIndex, 3].Value = txtCorreo?.Text ?? "";
        //                    worksheet.Cells[rowIndex, 4].Value = txtNombrePrograma?.Text ?? "";
        //                    worksheet.Cells[rowIndex, 5].Value = txtResponsable?.Text ?? "";
        //                    worksheet.Cells[rowIndex, 6].Value = lblEstatus?.Text ?? "";
                        

        //                rowIndex++; // Mover a la siguiente fila
        //            }

        //            // Configurar el tipo de respuesta y escribir el archivo en la respuesta HTTP
        //            Response.Clear();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=Programas.xlsx");
        //            Response.BinaryWrite(excelPackage.GetAsByteArray());
        //            Response.End();
        //        }
        //    }
        //    else
        //    {
        //        // Si el Repeater está vacío, mostrar un mensaje o realizar alguna acción adicional
        //        // Puedes agregar aquí el código para manejar este caso según tus necesidades
        //    }
        //}
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

            int page = CurrentPage;
            string NombreDep = txtDependencia.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            string CorreoDep = txtCorreo.Text.Trim();
            string Programa = txtPrograma.Text.Trim();
            string Responsable = txtResponsable.Text.Trim();

            CargarDatos(page, NombreDep, CorreoDep, Programa, Responsable, Fecha);
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

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportToExcelProgramas();
        }

        private void ExportToExcelProgramas()
        {
            // 🔥 Obtén los datos de Programas
            DataTable dt = ObtenerDatosExportacionProgramas(
                txtDependencia.Text.Trim(),
                txtCorreo.Text.Trim(),
                txtPrograma.Text.Trim(),
                txtResponsable.Text.Trim(),
                txtFecha.Text.Trim(),
                ddlEstatus.SelectedValue,
                ddlPeriodo.SelectedValue,
                DDLUnidad.SelectedValue
            );

            if (dt.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ProgramasRegistrados.xls");
                Response.Charset = "utf-8"; // ⚡ muy importante
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = Encoding.UTF8;

                StringBuilder sb = new StringBuilder();
                sb.Append("<table border='1'>");
                sb.Append("<tr>");
                sb.Append("<th>Periodo</th>"); // 👈 NUEVO encabezado
                sb.Append("<th>Fecha Registro</th>");
                sb.Append("<th>Dependencia</th>");
                sb.Append("<th>Correo</th>");
                sb.Append("<th>Nombre del Programa</th>");
                sb.Append("<th>Responsable</th>");
                sb.Append("<th>Unidad</th>");
                sb.Append("<th>Estatus</th>");
                sb.Append("</tr>");

                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", row["Periodo"]); // 👈 NUEVO dato
                    sb.AppendFormat("<td>{0}</td>", row["FechaRegistro"]);
                    sb.AppendFormat("<td>{0}</td>", row["Dependencia"]);
                    sb.AppendFormat("<td>{0}</td>", row["Correo"]);
                    sb.AppendFormat("<td>{0}</td>", row["NombrePrograma"]);
                    sb.AppendFormat("<td>{0}</td>", row["Responsable"]);
                    sb.AppendFormat("<td>{0}</td>", row["UNIDAD"]);
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
        private DataTable ObtenerDatosExportacionProgramas(string nombreDep, string correoDep, string nombrePro, string responsable, string fecha, string estatus, string periodo, string unidad)
        {
            int totalRecords;
            return ObtenerDatos(0, 100000, nombreDep, correoDep, nombrePro, responsable, fecha, estatus, periodo, unidad, out totalRecords);
        }
        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            // Limpiar los TextBox
            txtDependencia.Text = string.Empty;
            txtFecha.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtPrograma.Text = string.Empty;
            txtResponsable.Text = string.Empty;


            ddlEstatus.ClearSelection();
            if (ddlEstatus.Items.Count > 0) ddlEstatus.SelectedIndex = 0;

            DDLUnidad.ClearSelection();
            if (DDLUnidad.Items.Count > 0) DDLUnidad.SelectedIndex = 0;

            ddlPeriodo.ClearSelection();
            if (ddlPeriodo.Items.Count > 0) ddlPeriodo.SelectedIndex = 0;


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

            int page = CurrentPage;
            string NombreDep = txtDependencia.Text.Trim();
            string Fecha= txtFecha.Text.Trim();
            string CorreoDep = txtCorreo.Text.Trim();
            string Programa = txtPrograma.Text.Trim();
            string Responsable= txtResponsable.Text.Trim();

            CargarDatos(page, NombreDep, CorreoDep, Programa, Responsable, Fecha);
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
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtén el término de búsqueda del cuadro de texto
            //string searchTerm = txtBusqueda.Text.Trim();

            string NombreDep = txtDependencia.Text.Trim();
            string Fecha = txtFecha.Text.Trim();
            string CorreoDep = txtCorreo.Text.Trim();
            string Programa = txtPrograma.Text.Trim();
            string Responsable = txtResponsable.Text.Trim();
            // Establece la página actual en cero para volver a la primera página después de la búsqueda
            CurrentPage = 0;

            //// Carga los datos con el término de búsqueda y la página actual
           CargarDatos(CurrentPage, NombreDep, CorreoDep, Programa, Responsable, Fecha);
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