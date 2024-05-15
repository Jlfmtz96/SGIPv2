using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2.Investigadores
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
                            this.lbltitulo.Text = "Agregar nuevo investigador";
                            this.BtnCreate.Visible = true;
                            this.tbclave.Enabled=true;
                            break;
                        case "R":
                            this.lbltitulo.Text = "Consulta de investigador";
                            DeshabilitarCampos();
                            break;
                        case "U":
                            this.lbltitulo.Text = "Modificar investigador";
                            this.BtnUpdate.Visible = true;
                            break;
                        case "D":
                            this.lbltitulo.Text = "Dar de baja investigador";
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
            tbnombre.Enabled = false;
            tbappat.Enabled = false;
            tbapmat.Enabled = false;
            tbcorreo.Enabled = false;
            ddlCuerpoAcademico.Enabled = false;
            ddlSNI.Enabled = false;
            tbSniVigencia.Enabled = false;
            ddlPerfil.Enabled = false;
            tbProdepVigencia.Enabled = false;
            CheckBoxActivo.Enabled = false;
            lblDatos.Visible = false;
        }

        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Investigador WHERE cve_inv = @cve_inv", con);
            da.SelectCommand.Parameters.Add("@cve_inv", SqlDbType.VarChar).Value = aCve;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbclave.Text = row["cve_inv"].ToString();
            tbnombre.Text = row["nombre_investigador"].ToString();
            tbappat.Text = row["ap_pat"].ToString();
            tbapmat.Text = row["ap_mat"].ToString();
            tbcorreo.Text = row["correo"].ToString();
            ddlCuerpoAcademico.Text = row["cuerpo_academico"].ToString();
            ddlSNI.Text = row["SNI"].ToString();
            // Obtener el estado del investigador
            bool activo = Convert.ToBoolean(row["activo"]);

            // Establecer el estado del checkbox
            CheckBoxActivo.Checked = activo;

            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string nombre = tbnombre.Text;
            string appat = tbappat.Text;
            string apmat = tbapmat.Text;
            string correo = tbcorreo.Text;
            string CA = ddlCuerpoAcademico.SelectedValue;
            string liderCA = tbLiderCA.Text;
            string SNI = ddlSNI.SelectedValue;
            string vigenciaSNI = tbSniVigencia.Text;
            string perfil = ddlPerfil.SelectedValue;
            string vigenciaPerfil = tbProdepVigencia.Text;
            bool activo = CheckBoxActivo.Checked;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(CA) || string.IsNullOrWhiteSpace(liderCA) || string.IsNullOrWhiteSpace(SNI) ||
                string.IsNullOrWhiteSpace(vigenciaSNI) || string.IsNullOrWhiteSpace(perfil) || string.IsNullOrWhiteSpace(vigenciaPerfil))
            {
                lblErrorMessage.Text = "Complete los campos obligatorios";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            if (ClaveExiste(clave))
            {
                lblErrorMessage.Text = "Esta clave ya se encuentra registrada";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO Investigador (cve_inv, nombre_investigador, ap_pat, ap_mat, correo, " +
                "cuerpo_academico, SNI, activo, perfil_prodep, sni_vigencia, prodep_vigencia, lider_CA) VALUES (@Cve_inv, @Nombre, " +
                "@ApellidoPaterno, @ApellidoMaterno, @Correo, @CuerpoAcademico, @SNI, @Activo, @PerfilProdep, @SniVigencia, " +
                "@ProdepVigencia, @LiderCA)", con);

            using (cmd)
            {
                cmd.Parameters.AddWithValue("@Cve_inv", clave);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@ApellidoPaterno", appat);
                cmd.Parameters.AddWithValue("@ApellidoMaterno", apmat);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@CuerpoAcademico", CA);
                cmd.Parameters.AddWithValue("@SNI", SNI);
                cmd.Parameters.AddWithValue("@Activo", activo);
                cmd.Parameters.AddWithValue("@PerfilProdep", perfil);
                cmd.Parameters.AddWithValue("@SniVigencia", vigenciaSNI);
                cmd.Parameters.AddWithValue("@ProdepVigencia", vigenciaPerfil);
                cmd.Parameters.AddWithValue("@LiderCA", liderCA);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                    // Mostrar el modal de éxito
                    lblSuccessMessage.Text = "Alumno agregado satisfactoriamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                    LimpiarCampos();
                    //Response.Redirect("Index.aspx");
                    ScriptManager.RegisterStartupScript(this, GetType(), "RedirectAfterDelay", "setTimeout(function() { " +
                        "window.location.href = 'Index.aspx'; }, 2000);", true);
                }
                catch (Exception ex)
                {
                    // Mostrar el modal de error con el mensaje de error
                    lblErrorMessage.Text = "Error al agregar alumno: " + ex.Message;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                }
            }

            con.Close();
        }

        private void LimpiarCampos()
        {
            tbclave.Text = "";
            tbnombre.Text = "";
            tbappat.Text = "";
            tbapmat.Text = "";
            tbcorreo.Text = "";
            ddlCuerpoAcademico.Text = "";
            ddlSNI.Text = "";
            tbSniVigencia.Text = "";
            ddlPerfil.Text = "";
            tbProdepVigencia.Text = "";
            CheckBoxActivo.Text = "";
        }

        private bool ClaveExiste(string clave)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Investigador WHERE cve_inv = @Cve_inv", con);
            cmd.Parameters.Add("@Cve_inv", SqlDbType.VarChar).Value = clave;
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string nombre = tbnombre.Text;
            string appat = tbappat.Text;
            string apmat = tbapmat.Text;
            string correo = tbcorreo.Text;
            string CA = ddlCuerpoAcademico.SelectedValue;
            string liderCA = tbLiderCA.Text;
            string SNI = ddlSNI.SelectedValue;
            string vigenciaSNI = tbSniVigencia.Text;
            string perfil = ddlPerfil.SelectedValue;
            string vigenciaPerfil = tbProdepVigencia.Text;
            bool activo = CheckBoxActivo.Checked;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(CA) || string.IsNullOrWhiteSpace(liderCA) || string.IsNullOrWhiteSpace(SNI) ||
                string.IsNullOrWhiteSpace(vigenciaSNI) || string.IsNullOrWhiteSpace(perfil) || string.IsNullOrWhiteSpace(vigenciaPerfil))
            {
                lblErrorMessage.Text = "Complete los campos obligatorios";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Investigador SET nombre_investigador = @Nombre, ap_pat = @ApellidoPaterno, " +
                    "ap_mat = @ApellidoMaterno, correo = @Correo, cuerpo_academico = @CuerpoAcademico, SNI = @SNI, activo = @Activo, " +
                    "perfil_prodep = @PerfilProdep, sni_vigencia = @SniVigencia, prodep_vigencia = @ProdepVigencia, lider_CA = @LiderCA " +
                    "WHERE cve_inv = @Cve_inv", con);
                con.Open();

                cmd.Parameters.AddWithValue("@Cve_inv", clave);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@ApellidoPaterno", appat);
                cmd.Parameters.AddWithValue("@ApellidoMaterno", apmat);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@CuerpoAcademico", CA);
                cmd.Parameters.AddWithValue("@SNI", SNI);
                cmd.Parameters.AddWithValue("@Activo", activo);
                cmd.Parameters.AddWithValue("@PerfilProdep", perfil);
                cmd.Parameters.AddWithValue("@SniVigencia", vigenciaSNI);
                cmd.Parameters.AddWithValue("@ProdepVigencia", vigenciaPerfil);
                cmd.Parameters.AddWithValue("@LiderCA", liderCA);

                cmd.ExecuteNonQuery();
                con.Close();

                // Mostrar mensaje de éxito
                lblSuccessMessage.Text = "Alumno actualizado satisfactoriamente.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                //System.Threading.Thread.Sleep(3000);
                //Response.Redirect("Index.aspx");
                ScriptManager.RegisterStartupScript(this, GetType(), "RedirectAfterDelay", "setTimeout(function() { " +
                    "window.location.href = 'Index.aspx'; }, 2000);", true);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error al actualizar alumno: " + ex.Message;
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
    }
}