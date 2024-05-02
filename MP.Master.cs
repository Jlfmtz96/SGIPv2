using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2
{
    public partial class MP : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Request.Url.AbsolutePath.ToLower().Contains("/login/login.aspx"))
            {
                // Check if the session has expired and the current page is not the login page
                if (Session["loggedIn"] == null || !(bool)Session["loggedIn"])
                {
                    // Redirect the user to the login page
                    Response.Redirect("~/Login/Login.aspx");
                }
            }

            if (!IsPostBack)
            {
                // Check if the session is logged in or not
                if (Session["loggedIn"] != null && (bool)Session["loggedIn"])
                {
                    // If logged in, set the text of the label to the username
                    lblUsername.Text = Session["usuariologueado"].ToString();
                    navbarsesion.Visible = true; // Show the logout button
                }
                else
                {
                    // If not logged in, set the text of the label to "Iniciar Sesión"
                    lblUsername.Text = "Iniciar Sesión";
                    navbarsesion.Visible = false; // Hide the logout button
                    mobilemenu.Visible = false;
                }
            }

            // Obtener la ruta de la página actual desde la URL de la solicitud
            string currentPage = Request.Url.AbsolutePath.ToLower();

            // Determinar la sección activa basada en la URL
            string activeSection = "inicio"; // Sección activa por defecto

            if (currentPage.Contains("/alumnos/"))
            {
                activeSection = "alumnos";
            }
            else if (currentPage.Contains("/investigadores/"))
            {
                activeSection = "investigadores";
            }
            else if (currentPage.Contains("/proyectos/"))
            {
                activeSection = "proyectos";
            }
            // Agrega más condiciones según tus necesidades para otras secciones

            // Establecer los estilos de los enlaces según la sección activa
            switch (activeSection)
            {
                case "alumnos":
                    AlumnosLink.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    AlumnosLinkM.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    break;
                case "investigadores":
                    InvestigadoresLink.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    InvestigadoresLinkM.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    break;
                case "proyectos":
                    ProyectosLink.CssClass = "bg-[#004a98] text-white rounded-md px-3 py-2 text-sm font-medium";
                    break;
                // Agrega más casos según tus necesidades para otras secciones
                default:
                    // Enlace por defecto (puedes cambiarlo según tu diseño)
                    AlumnosLink.CssClass = "hover:bg-[#00b2e3] hover:text-white rounded-md px-3 py-2 text-sm font-medium";
                    InvestigadoresLink.CssClass = "hover:bg-[#00b2e3] hover:text-white rounded-md px-3 py-2 text-sm font-medium";
                    ProyectosLink.CssClass = "hover:bg-[#00b2e3] hover:text-white rounded-md px-3 py-2 text-sm font-medium";
                    // Establecer el estilo por defecto también para otros enlaces
                    break;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Remove session variables
            Session.Remove("usuariologueado");
            Session["loggedIn"] = false;

            // Redirect to login page
            Response.Redirect("~/Login/Login.aspx");
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Check if the session is logged in or not
            if (Session["loggedIn"] == null || !(bool)Session["loggedIn"])
            {
                // If not logged in, hide the div
                navbarsesion.Style["display"] = "none";

            }


        }
    }
}
