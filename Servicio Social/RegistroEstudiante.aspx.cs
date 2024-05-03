using System;
using Novell.Directory.Ldap;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class RegistroEstudiante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["matricula"] = null;
            Session["nombre"] = null;
            pnlRegistro.Visible = false;
        }

        #region Metodos
        #region Validar con ActiveDirectory
        public Boolean auntenticar(string usuario, string password)
        {
            Boolean response = false;
            string scriptErrorUserPass = "alert('Usuario o contraseña incorrectos.'); window.location='" + Request.ApplicationPath + "RegistroEstudiante.aspx'";

            string ldapHost = "148.212.9.25";
            int ldapPort = 389;
            string searchBase = "DC=uadec,DC=edu,DC=mx";
            string searchFilter = "mail=" + usuario;
            string[] requiredAttributes = { "mail", "name", "description", "SAMaccountname" };

            try
            {
                string matricula = "";
                string nombre = "";
                LdapConnection conn = new LdapConnection();
                conn.Connect(ldapHost, ldapPort);
                conn.Bind(usuario,password);

                LdapSearchResults lsc = conn.Search(searchBase, LdapConnection.SCOPE_SUB, searchFilter, requiredAttributes, false);
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = lsc.next();
                    string accountName = nextEntry.getAttribute("SAMACCOUNTNAME").StringValue;
                    matricula = nextEntry.getAttribute("description").StringValue;
                    nombre = nextEntry.getAttribute("name").StringValue;

                    //test.Text = matricula + "<br>" + nombre;

                    Session["matricula"] = matricula;
                    Session["nombre"] = nombre;
                    response = true;
                }
                catch
                (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptErrorUserPass, true);
                    response = false ;
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptErrorUserPass, true);
                response = false;
            }

            return response;
        }
        #endregion
        #endregion

        #region Botones

        #region Boton Inicio Sesion
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario, password;
            usuario = txtCorreo.Text + "@uadec.edu.mx";
            password = txtPassword.Text;
            Session["_USUARIOACTIVO"] = txtCorreo.Text;
            if(auntenticar(usuario, password))
                Response.Redirect("PanelEstudiante.aspx");
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            pnlInicioSesion.Visible = false;
            pnlRegistro.Visible = true;
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            pnlInicioSesion.Visible = true;
            pnlRegistro.Visible = false;
        }
        #endregion
        #endregion
    }
}