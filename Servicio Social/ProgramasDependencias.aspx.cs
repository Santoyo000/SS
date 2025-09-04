using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Servicio_Social
{
    public partial class ProgramasDependencias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CurrentPage = 0;
                CargarDatos(CurrentPage, "");
            }
        }
        private int CurrentPage
        {
            get {return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 0;}
            set {ViewState["CurrentPage"] = value;}
        }

        private int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0;}
            set { ViewState["TotalPages"] = value;}
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages - 1) CurrentPage++;
            CargarDatos(CurrentPage, "");
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0) CurrentPage--;
            CargarDatos(CurrentPage, "");
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
            // Valida la sesión por si acaso
            if (Session["idDependencia"] == null)
                throw new InvalidOperationException("La sesión no contiene idDependencia.");

            string idDependencia = Session["idDependencia"].ToString().Split('|')[1];

            string conString = GlobalConstants.SQL;
            int rowsToSkip = pageIndex * pageSize;

            string query = @"
        SELECT PR.idPrograma,
               CONVERT(varchar, DFECHAREGISTROP, 103) AS FechaRegistro,
               DS.sDescripcion AS Dependencia,
               USU.sCorreo AS Correo,
               PR.sNombre_Programa AS NombrePrograma,
               DS.sResponsable AS Responsable,
               PR.kpEstatus_Programa,
               ES.sDescripcion AS Estatus
        FROM SM_PROGRAMA AS PR
        JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio
        JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus
        JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario
        WHERE DS.idDependenicaServicio = @idDependencia";

            string countQuery = @"
        SELECT COUNT(*)
        FROM SM_PROGRAMA AS PR
        JOIN SM_DEPENDENCIA_SERVICIO AS DS ON PR.kpDependencia = DS.idDependenicaServicio
        JOIN NP_ESTATUS AS ES ON PR.kpEstatus_Programa = ES.idEstatus
        JOIN SM_USUARIO AS USU ON DS.kmUsuario = USU.idUsuario
        WHERE DS.idDependenicaServicio = @idDependencia";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string searchCondition = @"
            AND (
                DS.sDescripcion      LIKE @searchTerm OR
                ES.sDescripcion      LIKE @searchTerm OR
                DS.sResponsable      LIKE @searchTerm OR
                PR.sNombre_Programa  LIKE @searchTerm OR
                USU.sCorreo          LIKE @searchTerm
            )";
                query += searchCondition;
                countQuery += searchCondition;
            }

            query += @"
        ORDER BY DFECHAREGISTROP DESC
        OFFSET @rowsToSkip ROWS
        FETCH NEXT @pageSize ROWS ONLY;";

            DataTable dt = new DataTable();
            totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlCommand countCmd = new SqlCommand(countQuery, con))
            {
                // SIEMPRE idDependencia
                cmd.Parameters.AddWithValue("@idDependencia", idDependencia);
                countCmd.Parameters.AddWithValue("@idDependencia", idDependencia);

                // Paginación
                cmd.Parameters.AddWithValue("@rowsToSkip", rowsToSkip);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                // Solo si hay término de búsqueda
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    string like = "%" + searchTerm + "%";
                    cmd.Parameters.AddWithValue("@searchTerm", like);
                    countCmd.Parameters.AddWithValue("@searchTerm", like);
                }

                con.Open();

                totalRecords = (int)countCmd.ExecuteScalar();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            return dt;
        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string idPrograma = btn.CommandArgument;

            string encodedIdPrograma = SimpleEncryptionHelper.Encode(idPrograma);

            Response.Redirect("VerProgramas.aspx?idPrograma=" + HttpUtility.UrlEncode(encodedIdPrograma));
        }
        public class SimpleEncryptionHelper
        {
            private static readonly string key = "P@ssw0rd1234!Key#"; // Tu clave secreta para agregar ofuscación

            public static string Encode(string plainText)
            {
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                string base64String = Convert.ToBase64String(plainTextBytes);

                // Agregar una capa adicional de ofuscación (opcional)
                string obfuscatedString = Obfuscate(base64String);
                return obfuscatedString;
            }

            public static string Decode(string encodedText)
            {
                // Deshacer la ofuscación (opcional)
                string deobfuscatedString = Deobfuscate(encodedText);

                byte[] base64Bytes = Convert.FromBase64String(deobfuscatedString);
                string plainText = System.Text.Encoding.UTF8.GetString(base64Bytes);
                return plainText;
            }

            private static string Obfuscate(string input)
            {
                char[] keyChars = key.ToCharArray();
                char[] inputChars = input.ToCharArray();

                for (int i = 0; i < inputChars.Length; i++)
                {
                    inputChars[i] = (char)(inputChars[i] ^ keyChars[i % keyChars.Length]);
                }

                return new string(inputChars);
            }

            private static string Deobfuscate(string input)
            {
                return Obfuscate(input); // El mismo método puede ser usado para desofuscar
            }
        }

    }

}