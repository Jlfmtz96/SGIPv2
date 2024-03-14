using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2
{
    public partial class MP : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Obtener la ruta de la página actual desde la URL de la solicitud
            string currentPage = Request.Url.AbsolutePath.ToLower();

            // Determinar la sección activa basada en la URL
            string activeSection = "inicio"; // Sección activa por defecto

            if (currentPage.Contains("/Alumnos/"))
            {
                activeSection = "alumnos";
            }
            else if (currentPage.Contains("/Investigadores/"))
            {
                activeSection = "investigadores";
            }
            // Agrega más condiciones según tus necesidades para otras secciones

            // Establecer los estilos de los enlaces según la sección activa
            switch (activeSection)
            {
                case "alumnos":
                    AlumnosLink.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    break;
                case "investigadores":
                    InvestigadoresLink.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    break;
                // Agrega más casos según tus necesidades para otras secciones
                default:
                    // Enlace por defecto (puedes cambiarlo según tu diseño)
                    AlumnosLink.CssClass = "hover:bg-[#00b2e3] hover:text-white rounded-md px-3 py-2 text-sm font-medium";
                    InvestigadoresLink.CssClass = "hover:bg-[#00b2e3] hover:text-white rounded-md px-3 py-2 text-sm font-medium";
                    // Establecer el estilo por defecto también para otros enlaces
                    break;
            }
        }
    }
}