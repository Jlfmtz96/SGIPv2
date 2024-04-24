using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2.Main
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loggedIn"] != null && (bool)Session["loggedIn"] == true)
            {
                string usuariologueado = Session["usuariologueado"].ToString();
                lblBienvenida.Text = "Bienvenido/a " + usuariologueado;
            }
            else
            {
                Response.Redirect("../Login/Login.aspx");
            }
        }

    }
}