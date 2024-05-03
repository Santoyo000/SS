using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class PanelEstudiante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["matricula"] is null)
                Response.Redirect("RegistroEstudiante.aspx");
            else
                test.Text = "Bienvenido mafaqa <br>" + Session["matricula"].ToString() + "<br>" + Session["nombre"].ToString();
        }
    }
}