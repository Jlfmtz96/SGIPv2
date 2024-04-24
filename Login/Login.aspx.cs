using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace SGIPv2
{
    public partial class Login : Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the cerrar parameter is present in the URL and is set to true
            string cerrarParameter = Request.QueryString["cerrar"];
            if (!string.IsNullOrEmpty(cerrarParameter) && cerrarParameter.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                // Clear session variables and abandon the session
                Session.Remove("usuariologueado");
                Session["loggedIn"] = false;
                Session.Abandon();

                // Redirect the user to the login page without the cerrar parameter
                Response.Redirect("~/Login/Login.aspx");
            }
        }



        string patron = "InfoToolsSV";
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            string conectar = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            SqlConnection sqlConectar = new SqlConnection(conectar);
            SqlCommand cmd = new SqlCommand("SP_ValidarUsuario", sqlConectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = tbUsuario.Text;
            cmd.Parameters.Add("@Contrasenia", SqlDbType.VarChar, 50).Value = tbPassword.Text;
            cmd.Parameters.Add("@Patron", SqlDbType.VarChar, 50).Value = patron;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //Agregamos una sesion de usuario
                Session["usuariologueado"] = tbUsuario.Text;
                Session["loggedIn"] = true;
                Response.Redirect("../Main/Main.aspx");
            }
            else
            {
                lblError.Text = "Error de Usuario o Contraseña";

            }

            cmd.Connection.Close();
        }
    }
}
