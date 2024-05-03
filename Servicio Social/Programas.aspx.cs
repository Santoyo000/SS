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
    public partial class Programas1 : System.Web.UI.Page
    {

        string SQL = GlobalConstants.SQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Asegúrate de que la página no se está cargando debido a un postback
            {
                
                // Define la conexión SQL y la consulta
                using (SqlConnection con = new SqlConnection(SQL))
                {
                    con.Open();
                    string queryString = "SELECT sClave + ' - ' + sDescripcion [Descripcion] FROM SP_ESCUELA_UAC WHERE  sDescripcion IS NOT NULL ORDER BY sClave";

                    // Crea un DataSet para almacenar los resultados de la consulta
                    DataSet ds = new DataSet();

                    // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                    using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                    {
                        data.Fill(ds);
                    }

                    // Asigna los resultados al DropDownList
                    DDLEscuela.DataSource = ds;
                    DDLEscuela.DataTextField = "Descripcion"; // Utiliza el alias "Descripcion" como texto visible
                    DDLEscuela.DataBind();
                }
                // Define la conexión SQL y la consulta
                using (SqlConnection con = new SqlConnection(SQL))
                {
                    con.Open();
                    string queryString = "SELECT sClave + ' - ' + sDescripcion [Descripcion] FROM SP_PLAN_ESTUDIO WHERE SCLAVE IS NOT NULL ORDER BY sClave";

                    // Crea un DataSet para almacenar los resultados de la consulta
                    DataSet ds2 = new DataSet();

                    // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                    using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                    {
                        data.Fill(ds2);
                    }

                    // Asigna los resultados al DropDownList
                    DDLPlan.DataSource = ds2;
                    DDLPlan.DataTextField = "Descripcion"; // Utiliza el alias "Descripcion" como texto visible
                    DDLPlan.DataBind();
                }
                // Define la conexión SQL y la consulta
                using (SqlConnection con = new SqlConnection(SQL))
                {
                    con.Open();
                    string queryString = "SELECT sDescripcion FROM SP_CICLO  WHERE bActivo= 1";

                    // Crea un DataSet para almacenar los resultados de la consulta
                    DataSet ds3 = new DataSet();

                    // Utiliza un SqlDataAdapter para ejecutar la consulta y llenar el DataSet
                    using (SqlDataAdapter data = new SqlDataAdapter(queryString, con))
                    {
                        data.Fill(ds3);
                    }

                    // Asigna los resultados al DropDownList
                    DDLPeriodo.DataSource = ds3;
                    DDLPeriodo.DataTextField = "sDescripcion"; // Utiliza el alias "Descripcion" como texto visible
                    // DDLPeriodo.DataValueField = "sDescripcion";
                    DDLPeriodo.DataBind();
                }
            }
        }
    }
}