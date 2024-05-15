using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2.Publicaciones
{
    public partial class CRUD : System.Web.UI.Page
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
                            this.lbltitulo.Text = "Agregar nuevo producto de investigación";
                            this.BtnCreate.Visible = true;
                            this.tbclave.Enabled = true;
                            break;
                        case "R":
                            this.lbltitulo.Text = "Consulta producto de investigación";
                            DeshabilitarCampos();
                            break;
                        case "U":
                            this.lbltitulo.Text = "Modificar producto de investigación";
                            this.BtnUpdate.Visible = true;
                            break;
                        case "D":
                            this.lbltitulo.Text = "Dar de baja producto de investigación";
                            this.BtnCreate.Visible = true;
                            break;
                    }
                }
            }
        }

        private void DeshabilitarCampos()
        {
            // Desactivar la edición de los campos
            tbclave.Enabled = false;
            tbtitulo.Enabled = false;
            tbfpub.Enabled = false;
            tbtipo.Enabled = false;
            tblugar.Enabled = false;
        }

        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Producto_investigacion WHERE ID_producto = @ID_producto", con);
            da.SelectCommand.Parameters.Add("@ID_producto", SqlDbType.VarChar).Value = aCve;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbclave.Text = row["id_producto"].ToString();
            tbtitulo.Text = row["titulo_producto"].ToString();
            tbfpub.Text = row["fecha_publicacion"].ToString();
            tbtipo.Text = row["tipo_PI"].ToString();
            tblugar.Text = row["lugar_publicacion"].ToString();
            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string titulo = tbtitulo.Text;
            string fechaPub = tbfpub.Text;
            string tipo = tbtipo.Text;
            string lugar = tblugar.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(fechaPub) ||
                string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(lugar))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            if (ClaveExiste(clave))
            {
                lblErrorMessage.Text = "Este Id ya se encuentra registrada";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            DateTime fechaPublicacion;
            if (!DateTime.TryParseExact(fechaPub, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaPublicacion))
            {
                lblErrorMessage.Text = "La fecha de publicación '" + fechaPub + "' no tiene un formato válido.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            string query = "INSERT INTO Producto_investigacion (ID_producto, titulo_producto, fecha_publicacion, tipo_PI, " +
                "lugar_publicacion) VALUES (@ID_producto, @titulo_producto, @fecha_publicacion, @tipo_pi, @lugar_publicacion)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID_producto", clave);
                cmd.Parameters.AddWithValue("@titulo_producto", titulo);
                cmd.Parameters.AddWithValue("@fecha_publicacion", fechaPub);
                cmd.Parameters.AddWithValue("@tipo_pi", tipo);
                cmd.Parameters.AddWithValue("@lugar_publicacion", lugar);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    // Mostrar el modal de éxito
                    lblSuccessMessage.Text = "Producto agregado satisfactoriamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                    LimpiarCampos();
                    //Response.Redirect("Index.aspx");
                    ScriptManager.RegisterStartupScript(this, GetType(), "RedirectAfterDelay", "setTimeout(function() { " +
                        "window.location.href = 'Index.aspx'; }, 2000);", true);
                }
                catch (Exception ex)
                {
                    // Mostrar el modal de error con el mensaje de error
                    lblErrorMessage.Text = "Error al agregar el producto: " + ex.Message;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                }
            }

            con.Close();
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string titulo = tbtitulo.Text;
            string fecha = tbfpub.Text;
            string tipo = tbtipo.Text;
            string lugar = tblugar.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(fecha) ||
                string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(lugar))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Producto_investigacion SET titulo_producto = @titulo_producto, " +
                    "fecha_publicacion = @fecha_publicacion, tipo_PI = @tipo_pi, lugar_publicacion = @lugar_publicacion " +
                    "WHERE ID_producto = @ID_producto", con);
                con.Open();

                cmd.Parameters.AddWithValue("@ID_producto", clave);
                cmd.Parameters.AddWithValue("@titulo_producto", titulo);
                cmd.Parameters.AddWithValue("@fecha_publicacion", fecha);
                cmd.Parameters.AddWithValue("@tipo_pi", tipo);
                cmd.Parameters.AddWithValue("@lugar_publicacion", lugar);

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                // Mostrar mensaje de éxito
                lblSuccessMessage.Text = "Publicación actualizada satisfactoriamente.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                //System.Threading.Thread.Sleep(3000);
                //Response.Redirect("Index.aspx");
                ScriptManager.RegisterStartupScript(this, GetType(), "RedirectAfterDelay", "setTimeout(function() { " +
                    "window.location.href = 'Index.aspx'; }, 2000);", true);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error al actualizar la publicación: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            //
        }

        private bool ClaveExiste(string clave)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Producto_investigacion WHERE ID_producto = @ID_producto", con);
            cmd.Parameters.Add("@ID_producto", SqlDbType.VarChar).Value = clave;
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }

        private void LimpiarCampos()
        {
            tbclave.Text = "";
            tbtitulo.Text = "";
            tbfpub.Text = "";
            tbtipo.Text = "";
            tblugar.Text = "";
        }
    }
}