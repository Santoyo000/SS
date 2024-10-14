using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class SS : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["filtros"] != null || Session["tipoUsuario"] != null)
            {
                string _tipo = "";
                if (Session["filtros"] != null)
                {
                    _tipo = Session["filtros"].ToString().Split('|')[0];
                }
                else
                {
                    _tipo = Session["tipoUsuario"].ToString().Split('|')[0];
                }


                switch (_tipo)
                {
                    case "2": //Usuario tipo Dependencia
                        li_dependenciaPerfil.Visible = true;
                        li_dependenciaProgramas.Visible = true;
                        li_dependenciaListadoProgramas.Visible = true;
                        li_dependenciaAlumnos.Visible = true;
                        li_dependenciahome.Visible = true;
                        break;
                    case "1": //Usuario tipo Administrador
                        li_administradorHome.Visible = true;
                        li_administradorDependencias.Visible = true;
                        li_1istadoP.Visible = true;
                        li_administradorReportes.Visible = true;
                        li_administradorUsuarios.Visible = true;
                        li_alumnosregisitradosadmin.Visible = true;
                        li_programasAlumnoadmin.Visible = true;
                        break;
                    case "3": //Usuario tipo Responsable Unidad
                        li_RespHome.Visible = false;
                        li1DependenciasRegistradas.Visible = true;
                        li_ProgramasRegistrados.Visible = true;
                        li_AlumnosRegistrados.Visible = true;
                        li_RegistroEncargadoEscuela.Visible = true;
                        li_AlumnosPostulados.Visible = true;
                        break;
                    case "5": //Usuario Estudiante
                        li_estudianteHome.Visible = true;
                        li_estudianteReglamento.Visible = true;
                        li_estudianteProgramas.Visible = true;
                        li_estudiantesProgSel.Visible = true;
                        li_estudiantePerfil.Visible = true;
                        break;
                    case "4": // usuario encargado
                        li_EncargHome.Visible = true;
                        li_EncargEscPrograma.Visible = true;
                        li_EncarEscPerfil.Visible = true;
                        li1_EncargEscAlumnos.Visible = true;
                        li_EncargAlumnosPost.Visible = true;
                        break;
                }
            }
            else
            {
                Response.Redirect("Home.aspx");

            }
        }

        protected void lbtCambiarCarrera_Click(object sender, EventArgs e)
        {
            string temp = Session["tipoUsuario"].ToString();
            string temp2 = temp.Split('|')[0] + '|' + temp.Split('|')[1];
            Session["tipoUsuario"] = temp2;
            Session["plan"] = "";
            Response.Redirect("SeleccionPlan.aspx");
        }
    }
}