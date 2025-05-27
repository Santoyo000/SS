using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class DetallePrograma : System.Web.UI.Page
    {
        string SQL = GlobalConstants.SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos(0, "", "", "","");
                CargarUnidad();
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
        protected void CargarDatos(int pageIndex, string Programa, string Responsable, string Dependencia, string Correo)
        {
            int pageSize = 20;
            int totalRecords;

            DataTable dt = ObtenerDatos(pageIndex, pageSize, ddlPeriodo.SelectedValue, Programa, Responsable, Dependencia, Correo, DDLUnidad.SelectedValue
                                        , out totalRecords);

            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

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
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            string Programa = txtPrograma.Text.Trim();
            string Dependencia = txtDependencia.Text.Trim();
            string Responsable = txtResponsable.Text.Trim();
            string Correo = txtCorreo.Text.Trim();

            // Recargar los datos con los filtros actuales
           
            if (CurrentPage > 0)
            {
                CurrentPage -= 1;
                CargarDatos(CurrentPage, Programa, Dependencia, Responsable, Correo);
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            string Programa = txtPrograma.Text.Trim();
            string Dependencia = txtDependencia.Text.Trim();
            string Responsable = txtResponsable.Text.Trim();
            string Correo = txtCorreo.Text.Trim();
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage += 1;
                CargarDatos(CurrentPage, Programa, Dependencia, Responsable, Correo);
            }
        }
            protected DataTable ObtenerDatos(int pageIndex, int pageSize, string selectedPeriodo, string programa, string responsable, string dependencia, string correo, string selectedUnidad, out int totalRecords)
        {
            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;
            string tipoUsuario = Session["tipoUsuario"].ToString();
            List<string> filtros = new List<string>();

            if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                filtros.Add("PR.kpPeriodo = @selectedPeriodo");
            if (!string.IsNullOrEmpty(programa))
                filtros.Add("PR.sNombre_Programa LIKE @programa");
            if (!string.IsNullOrEmpty(responsable))
                filtros.Add("PR.sResponsable LIKE @responsable");
            if (!string.IsNullOrEmpty(dependencia))
                filtros.Add("DS.sDescripcion LIKE @dependencia");
            if (!string.IsNullOrEmpty(correo))
                filtros.Add("USU.sCorreo LIKE @correo");
            if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                filtros.Add("PR.KPUNIDAD = @selectedUnidad");

            if (tipoUsuario == "2")
            {
                string idDependencia = Session["idDependencia"].ToString().Split('|')[1];
                filtros.Add("DS.IDDEPENDENICASERVICIO = @idDependencia");
            }

            if (tipoUsuario == "3")
            {
                string unidadUsuario = Session["filtros"].ToString().Split('|')[1];
                filtros.Add("PR.KPUNIDAD = @unidadUsuario");
            }

            string where = filtros.Count > 0 ? " AND " + string.Join(" AND ", filtros) : "";

            string baseQuery = $@"
        FROM SM_PROGRAMA PR
        INNER JOIN SM_DEPENDENCIA_SERVICIO DS ON PR.kpDependencia = DS.idDependenicaServicio
        INNER JOIN NP_ESTATUS ES ON PR.kpEstatus_Programa = ES.idEstatus
        INNER JOIN SM_USUARIO USU ON DS.kmUsuario = USU.idUsuario
        INNER JOIN NP_UNIDAD UN ON PR.KPUNIDAD = UN.IDUNIDAD
        INNER JOIN SP_CICLO CIC ON PR.kpPeriodo = CIC.idCiclo
        INNER JOIN SM_DETALLE_PROGRAMA DP ON DP.kmPrograma = PR.idPrograma
        INNER JOIN SM_PROGRAMA_ALUMNO PA ON PA.kmDetallePrograma = DP.idDetallePrograma
        AND PA.kpEstatus NOT IN (7, 2, 21522, 21523) 
        WHERE PR.kpEstatus_Programa = 11 {where}
    ";

            string query = $@"
        SELECT 
            PR.idPrograma,
            CIC.sDescripcion AS Periodo,
            CONVERT(varchar, PR.DFECHAREGISTROP, 103) AS FechaRegistro,
            DS.sDescripcion AS Dependencia,
            USU.sCorreo AS Correo,
            PR.sNombre_Programa AS NombrePrograma,
            PR.sResponsable AS Responsable,
            PR.kpEstatus_Programa,
            UN.SCIUDAD AS UNIDAD,
            ES.sDescripcion AS Estatus,
            DP.ICUPO AS Cupo,
            COUNT(PA.idProgramaAlumno) AS TotalAlumnosInscritos 
        {baseQuery}
        GROUP BY 
            PR.idPrograma, CIC.sDescripcion, PR.DFECHAREGISTROP, DS.sDescripcion, 
            USU.sCorreo, PR.sNombre_Programa, PR.sResponsable, PR.kpEstatus_Programa,
            UN.SCIUDAD, DP.ICUPO, ES.sDescripcion
        ORDER BY PR.DFECHAREGISTROP ASC
        OFFSET @rowsToSkip ROWS FETCH NEXT @pageSize ROWS ONLY;";

            string countQuery = $"SELECT COUNT(DISTINCT PR.idPrograma) {baseQuery}";

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlCommand countCmd = new SqlCommand(countQuery, con))
                {
                    void AddParameter(string name, object value)
                    {
                        cmd.Parameters.AddWithValue(name, value);
                        countCmd.Parameters.AddWithValue(name, value);
                    }

                    if (!string.IsNullOrEmpty(selectedPeriodo) && selectedPeriodo != "0")
                        AddParameter("@selectedPeriodo", selectedPeriodo);
                    if (!string.IsNullOrEmpty(programa))
                        AddParameter("@programa", $"%{programa}%");
                    if (!string.IsNullOrEmpty(responsable))
                        AddParameter("@responsable", $"%{responsable}%");
                    if (!string.IsNullOrEmpty(dependencia))
                        AddParameter("@dependencia", $"%{dependencia}%");
                    if (!string.IsNullOrEmpty(correo))
                        AddParameter("@correo", $"%{correo}%");
                    if (!string.IsNullOrEmpty(selectedUnidad) && selectedUnidad != "0")
                        AddParameter("@selectedUnidad", selectedUnidad);

                    if (tipoUsuario == "2")
                    {
                        string idDependencia = Session["idDependencia"].ToString().Split('|')[1];
                        AddParameter("@idDependencia", idDependencia);
                    }

                    if (tipoUsuario == "3")
                    {
                        string unidadUsuario = Session["filtros"].ToString().Split('|')[1];
                        AddParameter("@unidadUsuario", unidadUsuario);
                    }

                    cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    con.Open();
                    totalRecords = (int)countCmd.ExecuteScalar();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
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
                    string Programa = txtPrograma.Text.Trim();
                    string Dependencia = txtDependencia.Text.Trim();
                    string Responsable = txtResponsable.Text.Trim();
                    string Correo = txtCorreo.Text.Trim();
                   
                    // Recargar los datos con los filtros actuales
                    CargarDatos(CurrentPage, Programa, Dependencia, Responsable, Correo);
                }
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
    }
}