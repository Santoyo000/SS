using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class Reglamento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipoUsuario"] == null)
            {
                Response.Redirect("LoginEstudiante.aspx");
            }
            else
            {
                try
                {
                    string id = Session["tipoUsuario"].ToString().Split('|')[2];
                }
                catch
                {
                    Response.Redirect("SeleccionPlan.aspx");
                }
            }
        }
    }
}