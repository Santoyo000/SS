using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Servicio_Social
{
    public partial class PanelDependencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idDependencia"] == null)
                Response.Redirect("LoginDependencia.aspx");
        }
    }
}