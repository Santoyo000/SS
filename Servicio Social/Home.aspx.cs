using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Servicio_Social
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<byte[]> images = GetAllImagesFromDatabase();
                ViewState["Images"] = images;
                LoadCarouselImages(images);
            }
        }
        private List<byte[]> GetAllImagesFromDatabase()
        {
            List<byte[]> images = new List<byte[]>();
            string connectionString = GlobalConstants.SQL;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ImageData FROM SM_ImagesSS ORDER BY IdImage"; // Asegúrate de que 'Id' es el campo autoincremental

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(reader["ImageData"] as byte[]);
                        }
                    }
                }
            }

            return images;
        }
        private void LoadCarouselImages(List<byte[]> images)
        {
            int index = 0; // Contador para los indicadores
            foreach (var imageData in images)
            {
                string base64String = Convert.ToBase64String(imageData, 0, imageData.Length);
                string activeClass = index == 0 ? " active" : ""; // Hacer el primer slide activo
                string imgTag = $"<div class='carousel-item{activeClass}'><img src='data:image/jpeg;base64,{base64String}' class='d-block w-100' alt='Slide'></div>";
                carouselLiteral.Text += imgTag;

                // Agregar el indicador
                carouselIndicators.Text += $"<li data-target='#myCarousel' data-slide-to='{index}' class='{(index == 0 ? "active" : "")}'></li>";
                index++;
            }
        }
    }
}