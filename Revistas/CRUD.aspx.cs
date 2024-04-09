using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2.Revistas
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public static string aCve = "-1";
        public static string aOpc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener el id
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    aCve = Request.QueryString["id"].ToString();
                    CargarDatos();
                }

                if (Request.QueryString["op"] != null)
                {
                    aOpc = Request.QueryString["op"].ToString();

                    switch (aOpc)
                    {
                        case "C":
                            this.lbltitulo.Text = "Agregar nueva revista";
                            this.BtnCreate.Visible = true;
                            this.tbclave.Enabled = true;
                            break;
                        case "R":
                            this.lbltitulo.Text = "Consulta de revista";
                            break;
                        case "U":
                            this.lbltitulo.Text = "Modificar revista";
                            this.BtnUpdate.Visible = true;
                            break;
                        case "D":
                            this.lbltitulo.Text = "Dar de baja revista";
                            this.BtnCreate.Visible = true;
                            break;
                    }
                }
            }
        }

        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Revista WHERE ID_revista = @ID_revista", con);
            da.SelectCommand.Parameters.Add("@ID_revista", SqlDbType.VarChar).Value = aCve;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbclave.Text = row["id_revista"].ToString(); // Establecer la clave del alumno en el textbox
            tbnombre.Text = row["nombre_revista"].ToString();
            tbtipo.Text = row["tipo_revista"].ToString();
            tbpais.Text = row["pais"].ToString();
            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string nombre = tbnombre.Text;
            string tipo = tbtipo.Text;
            string pais = tbpais.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(pais))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            if (ClaveExiste(clave))
            {
                lblErrorMessage.Text = "Esta clave ya se encuentra registrada";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            string query = "INSERT INTO Revista (ID_revista, nombre_revista, tipo_revista, pais) VALUES (@ID_revista, @nombre_revista, @tipo_revista, @pais)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@ID_revista", SqlDbType.VarChar).Value = tbclave.Text;
                cmd.Parameters.Add("@nombre_revista", SqlDbType.VarChar).Value = tbnombre.Text;
                cmd.Parameters.Add("@tipo_revista", SqlDbType.VarChar).Value = tbtipo.Text;
                cmd.Parameters.Add("@pais", SqlDbType.VarChar).Value = tbpais.Text;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    // Mostrar el modal de éxito
                    lblSuccessMessage.Text = "Revista agregada satisfactoriamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                    LimpiarCampos();
                    //Response.Redirect("Index.aspx");
                }
                catch (Exception ex)
                {
                    // Mostrar el modal de error con el mensaje de error
                    lblErrorMessage.Text = "Error al agregar la revista: " + ex.Message;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                }
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string nombre = tbnombre.Text;
            string tipo = tbtipo.Text;
            string pais = tbpais.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(pais))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Revista SET nombre_revista = @nombre_revista, tipo_revista = @tipo_revista, pais = @pais WHERE ID_revista = @ID_revista", con);
                con.Open();
                cmd.Parameters.Add("@ID_revista", SqlDbType.VarChar).Value = tbclave.Text;
                cmd.Parameters.Add("@nombre_revista", SqlDbType.VarChar).Value = tbnombre.Text;
                cmd.Parameters.Add("@tipo_revista", SqlDbType.VarChar).Value = tbtipo.Text;
                cmd.Parameters.Add("@pais", SqlDbType.VarChar).Value = tbpais.Text;

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                // Mostrar mensaje de éxito
                lblSuccessMessage.Text = "Revista actualizada satisfactoriamente.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                //System.Threading.Thread.Sleep(3000);
                //Response.Redirect("Index.aspx");
                ScriptManager.RegisterStartupScript(this, GetType(), "RedirectAfterDelay", "setTimeout(function() { window.location.href = 'Index.aspx'; }, 2000);", true);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error al actualizar la revista: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        private bool ClaveExiste(string clave)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Revista WHERE ID_revista = @ID_revista", con);
            cmd.Parameters.Add("@ID_revista", SqlDbType.VarChar).Value = clave;
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }

        private void LimpiarCampos()
        {
            tbclave.Text = "";
            tbnombre.Text = "";
            tbtipo.Text = "";
            tbpais.Text = "";
        }
    }
}