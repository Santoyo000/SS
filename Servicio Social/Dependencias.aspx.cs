using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using iText.Kernel.Pdf;
using iText.Layout;
using System.IO;
using System.Data;

namespace Servicio_Social
{
    public partial class Dependencias1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBox9.Text = "CALLE TORRE BLANCA #754 COL. LAS TORRES";
                rbtnBuscar.SelectedIndex = 0;
                ddlUnidad.SelectedIndex = 2;
                ddlOrganismo.SelectedIndex=1;
                ddlUnidad2.Items.Insert(0, new ListItem("Seleccionar unidad...", ""));
                ddlUnidad2.Items.Insert(1, new ListItem("SALTILLO", ""));
                ddlUnidad2.Items.Insert(2, new ListItem("NORTE", ""));
                ddlUnidad2.Items.Insert(3, new ListItem("TORREON", ""));

                ddlUnidad.Items.Insert(0, new ListItem("Seleccionar unidad...", ""));
                ddlUnidad.Items.Insert(1, new ListItem("SALTILLO", ""));
                ddlUnidad.Items.Insert(2, new ListItem("NORTE", ""));
                ddlUnidad.Items.Insert(3, new ListItem("TORREON", ""));
                ddlOrganismo.Items.Insert(0, new ListItem("Seleccionar organismo...", ""));
                ddlOrganismo.Items.Insert(1, new ListItem("ESTATAL", ""));
                ddlOrganismo.Items.Insert(2, new ListItem("FEDERAL", ""));
                ddlOrganismo.Items.Insert(3, new ListItem("MUNICIPAL", ""));
                ddlOrganismo.Items.Insert(4, new ListItem("PRIVADO CON CONVENIO", ""));
                ddlOrganismo.Items.Insert(4, new ListItem("SOCIAL", ""));
                ddlPeriodo.Items.Insert(0, new ListItem("Seleccionar periodo...", ""));
                ddlPeriodo.Items.Insert(1, new ListItem("ENERO 2024 - JUNIO 2024", ""));
                ddlPeriodo.Items.Insert(2, new ListItem("AGOSTO 2023 - DICIEMBRE 2023", ""));
                ddlEstatus.Items.Insert(0, new ListItem("Seleccionar estatus...", ""));
                ddlEstatus.Items.Insert(1, new ListItem("REGISTRADO", ""));
                ddlEstatus.Items.Insert(2, new ListItem("NO VALIDADO", ""));
                ddlEstatus.Items.Insert(3, new ListItem("VALIDADO", ""));
                ddlEstatus.Items.Insert(4, new ListItem("AUTORIZADO", ""));
                ddlEstatus.Items.Insert(5, new ListItem("NO AUTORIZADO", ""));

                DropDownList2.Items.Insert(0, new ListItem("Seleccionar organismo...", ""));
                DropDownList2.Items.Insert(1, new ListItem("ESTATAL", ""));
                DropDownList2.Items.Insert(2, new ListItem("FEDERAL", ""));
                DropDownList2.Items.Insert(3, new ListItem("MUNICIPAL", ""));
                DropDownList2.Items.Insert(4, new ListItem("PRIVADO CON CONVENIO", ""));
                DropDownList2.Items.Insert(5, new ListItem("SOCIAL", ""));


                DropDownList3.Items.Insert(0, new ListItem(" ", ""));
                DropDownList3.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList3.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList3.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList3.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList3.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList4.Items.Insert(0, new ListItem(" ", ""));
                DropDownList4.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList4.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList4.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList4.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList4.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList5.Items.Insert(0, new ListItem(" ", ""));
                DropDownList5.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList5.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList5.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList5.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList5.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList6.Items.Insert(0, new ListItem(" ", ""));
                DropDownList6.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList6.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList6.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList6.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList6.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList7.Items.Insert(0, new ListItem(" ", ""));
                DropDownList7.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList7.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList7.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList7.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList7.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList8.Items.Insert(0, new ListItem(" ", ""));
                DropDownList8.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList8.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList8.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList8.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList8.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList9.Items.Insert(0, new ListItem(" ", ""));
                DropDownList9.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList9.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList9.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList9.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList9.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList10.Items.Insert(0, new ListItem(" ", ""));
                DropDownList10.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList10.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList10.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList10.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList10.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList11.Items.Insert(0, new ListItem(" ", ""));
                DropDownList11.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList11.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList11.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList11.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList11.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList12.Items.Insert(0, new ListItem(" ", ""));
                DropDownList12.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList12.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList12.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList12.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList12.Items.Insert(5, new ListItem("EXCELENTE", ""));

                DropDownList13.Items.Insert(0, new ListItem(" ", ""));
                DropDownList13.Items.Insert(1, new ListItem("PESIMO", ""));
                DropDownList13.Items.Insert(2, new ListItem("DEFICIENTE", ""));
                DropDownList13.Items.Insert(3, new ListItem("SUFICIENTE", ""));
                DropDownList13.Items.Insert(4, new ListItem("ADECUADO", ""));
                DropDownList13.Items.Insert(5, new ListItem("EXCELENTE", ""));

               
                // Simulando una lista de personas
                List<Persona> personas = new List<Persona>
                {
                    new Persona { Programa = "AUTOMATICACION DE ACTIVIDADES DOCENTES", Matrícula = 21343236, Prestador = "ROCÍO MEDINA HURTADO", Estatus ="Asignado", Fecha_Estatus= "13/01/2009"  },
                    new Persona { Programa = "AUTOMATICACION DE ACTIVIDADES DOCENTES", Matrícula = 21654376, Prestador = "JENNIFER MORALES GARCÍA", Estatus ="Evaluado", Fecha_Estatus= "13/01/2009"  },

                };
                // Enlazar la lista de personas al GridView
                GridViewEstudiantes.DataSource = personas;
                GridViewEstudiantes.DataBind();

                

                //// Ruta al archivo PDF que quieres mostrar
                //string pdfFilePath = Server.MapPath("~/ruta/a/tu/archivo.pdf");

                //// Lee el contenido del archivo PDF
                //byte[] pdfBytes = File.ReadAllBytes(pdfFilePath);

                //// Convierte el contenido del archivo PDF a una cadena Base64
                //string pdfBase64 = Convert.ToBase64String(pdfBytes);

                //// Construye el marcado HTML para mostrar el PDF
                //string pdfHtml = $"<embed src='data:application/pdf;base64,{pdfBase64}' type='application/pdf' width='100%' height='100%' />";

                //// Muestra el PDF en el control de usuario ASP.NET
                //litPdfViewer.Text = pdfHtml;

            }


        }
        public class Persona
        {
            public string Programa { get; set; }
            public int Matrícula { get; set; }
            public string Prestador { get; set; }
            public string Estatus { get; set; }
            public string Fecha_Estatus { get; set; }
            //public string Operacion { get; set; }


        }
        protected void rbtnBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnBuscar.SelectedValue == "A") // Si la opción seleccionada es "Estudiantes Registrados"
            {
                Panel1.Visible = true; // Ocultar Panel1
                pnlRegistrarDependencia.Visible = false; // Ocultar pnlRegistrarDependencia
                pnlEstudiantesRegistrados.Visible = false;
                PnlEjemploModPerfil.Visible = false;
                pnlPDF.Visible = false;

            }
            if (rbtnBuscar.SelectedValue == "B") // Si la opción seleccionada es "Estudiantes Registrados"
            {
                Panel1.Visible = false; // Ocultar Panel1
                pnlRegistrarDependencia.Visible = true; // Ocultar pnlRegistrarDependencia
                pnlEstudiantesRegistrados.Visible = false;
                PnlEjemploModPerfil.Visible = false;
                pnlPDF.Visible = false;
            }

        }
        protected void btnIngresar_Click(object sender, EventArgs e)
        {

            // Establecer el SelectedIndex en -1
            rbtnBuscar.SelectedIndex = -1;
            rbtnIngreso.SelectedIndex = 0;

            // Verificar si el SelectedIndex es -1 y ocultar los paneles correspondientes
            if (rbtnBuscar.SelectedIndex == -1)
            {
                pnlIngreso.Visible = true;
                pnlBuscar.Visible = false;
                Panel1.Visible = false;
            }
            if (rbtnIngreso.SelectedIndex == 0)
            {
                pnlIngreso.Visible = true;
                pnlBuscar.Visible = false;
                Panel1.Visible = false;
                PnlEjemploModPerfil.Visible = true;
            }

        }
        protected void rbtnIngreso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnIngreso.SelectedValue == "A") // Si la opción seleccionada es "Estudiantes Registrados"
            {
                pnlEstudiantesRegistrados.Visible = false;
                Panel1.Visible = false; // Ocultar Panel1
                pnlRegistrarDependencia.Visible = false; // Ocultar pnlRegistrarDependencia
                PnlEjemploModPerfil.Visible = true;
                pnlPDF.Visible = false;

            }
            if (rbtnIngreso.SelectedValue == "B") // Si la opción seleccionada es "Estudiantes Registrados"
            {
                pnlEstudiantesRegistrados.Visible = true;
                Panel1.Visible = false; // Ocultar Panel1
                pnlRegistrarDependencia.Visible = false; // Ocultar pnlRegistrarDependencia
                PnlEjemploModPerfil.Visible = false;
                pnlPDF.Visible = false;

            }


        }
        protected void GridViewEstudiantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Cambiar el color de fondo de la fila de encabezados
                e.Row.BackColor = System.Drawing.Color.LightYellow;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el índice de la fila actual
                int rowIndex = e.Row.RowIndex;

                // Encontrar los botones en la fila
                Button btnEvaluar = (Button)e.Row.FindControl("btnOperacion");
                Button btnLiberar = (Button)e.Row.FindControl("Button3");

                // Determinar qué botón mostrar basado en el índice de la fila
                if (rowIndex % 2 == 0) // Mostrar "Evaluar" en filas pares
                {
                    btnEvaluar.Visible = true;
                    btnLiberar.Visible = false;
                }
                else // Mostrar "Liberar Prestador" en filas impares
                {
                    btnEvaluar.Visible = false;
                    btnLiberar.Visible = true;
                }
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Obtener los controles de etiqueta en la fila actual
            //    Label lblPrograma = (Label)e.Row.FindControl("lblPrograma");
            //    Label lblMatricula = (Label)e.Row.FindControl("lblMatricula");
            //    Label lblPrestador = (Label)e.Row.FindControl("lblPrestador");
            //    Label lblEstatus = (Label)e.Row.FindControl("lblEstatus");
            //    Label lblFechaEstatus = (Label)e.Row.FindControl("lblFechaEstatus");

            //    // Llenar los datos en las etiquetas
            //    lblPrograma.Text = "AUTOMATICACION DE ACTIVIDADES DOCENTES";
            //    lblMatricula.Text = "21343236";
            //    lblPrestador.Text = "ROCIO MEDINA HURTADO";
            //    lblEstatus.Text = "En Espera";
            //    lblFechaEstatus.Text = "13/01/2009";
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Crear el botón
            //    Button btnOperacion = new Button();
            //    btnOperacion.ID = "btnOperacion";
            //    btnOperacion.Text = "Operación";
            //    btnOperacion.CommandName = "Operacion";
            //    btnOperacion.CommandArgument = e.Row.RowIndex.ToString(); // Obtener el índice de la fila
            //    btnOperacion.CssClass = "btn btn-primary";

            //    // Asignar el evento al botón
            //    btnOperacion.Click += new EventHandler(btnOperacion_Click);

            //    // Agregar el botón a la celda
            //    e.Row.Cells[5].Controls.Add(btnOperacion); // Agregarlo a la última celda

            //    // Asegúrate de que el índice sea correcto según la estructura de tus columnas
            //    // Si la estructura cambia, ajusta el índice correspondientemente
            //}
        }
    
        protected void btnEvaluar_Click(object sender, EventArgs e)
        {
            pnlEvaluarEstudiante.Visible = true;
            pnlEstudiantesRegistrados.Visible = false;
            pnlPDF.Visible = false;
            //// Aquí maneja la lógica cuando se hace clic en el botón
            //Button btn = (Button)sender;
            //int rowIndex = int.Parse(btn.CommandArgument);

            // Puedes usar el índice de la fila para acceder a los datos de esa fila si es necesario
            // Por ejemplo, puedes usar GridViewEstudiantes.Rows[rowIndex] para obtener la fila correspondiente
        }
        protected void btnLiberar_Click(object sender, EventArgs e)
        {
            pnlEvaluarEstudiante.Visible = false;
            pnlPDF.Visible = true;
            pnlEstudiantesRegistrados.Visible=false;
            //// Aquí maneja la lógica cuando se hace clic en el botón
            //Button btn = (Button)sender;
            //int rowIndex = int.Parse(btn.CommandArgument);

            // Puedes usar el índice de la fila para acceder a los datos de esa fila si es necesario
            // Por ejemplo, puedes usar GridViewEstudiantes.Rows[rowIndex] para obtener la fila correspondiente
        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            // Obtener la fecha seleccionada y mostrarla en una etiqueta
            //lblSelectedDate.Text = "Fecha seleccionada: " + Calendar1.SelectedDate.ToShortDateString();
        }
        protected void btnMostrar_Click(object sender, EventArgs e)
        {

        }
    }
}