using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class Dependencias : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["idUser"] == null)
            //    lkbLogout.Visible = false;
            //else
            //    lkbLogout.Visible = true;
        }

        protected void lkbLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Dependencias.aspx");
        }
    }
}