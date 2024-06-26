﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SGIPv2.Pages
{
    public partial class CRUD : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public static string aCve = "-1";
        public static string aOpc = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener el id
            if(!Page.IsPostBack)
            {
                if (Request.QueryString["id"]!=null)
                {
                    aCve = Request.QueryString["id"].ToString();
                    CargarDatos();
                }

                if (Request.QueryString["op"]!=null)
                {
                    aOpc = Request.QueryString["op"].ToString();

                    switch (aOpc) 
                    {
                        case "C":
                            this.lbltitulo.Text = "Agregar nuevo alumno";
                            this.BtnCreate.Visible = true;
                            this.tbclave.Enabled= true;
                            break;
                        case "R":
                            this.lbltitulo.Text = "Consulta de alumno";
                            DeshabilitarCampos();
                            break;
                        case "U":
                            this.lbltitulo.Text = "Modificar alumno";
                            this.BtnUpdate.Visible = true;
                            break;
                        case "D":
                            this.lbltitulo.Text = "Dar de baja alumno";
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
            ddlLicenciatura.Enabled = false;
            lblDatos.Visible = false;
        }

        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Alumno WHERE cve_alumno = @cve_alumno", con);
            da.SelectCommand.Parameters.Add("@cve_alumno", SqlDbType.VarChar).Value = aCve;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbclave.Text = row["cve_alumno"].ToString(); // Establecer la clave del alumno en el textbox
            tbnombre.Text = row["nombre_alumno"].ToString();
            tbappat.Text = row["ap_pat_alumno"].ToString();
            tbapmat.Text = row["ap_mat_alumno"].ToString();
            ddlLicenciatura.Text = row["licenciatura"].ToString();

            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string nombre = tbnombre.Text;
            string appat = tbappat.Text;
            string apmat = tbapmat.Text;
            string licenciatura = ddlLicenciatura.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(licenciatura))
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

            string query = "INSERT INTO Alumno (cve_alumno, nombre_alumno, ap_pat_alumno, ap_mat_alumno, licenciatura) VALUES " +
                "(@Cve_alumno, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Licenciatura)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Cve_alumno", clave);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@ApellidoPaterno", appat);
                cmd.Parameters.AddWithValue("@ApellidoMaterno", apmat);
                cmd.Parameters.AddWithValue("@Licenciatura", licenciatura);

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

                con.Close();
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string nombre = tbnombre.Text;
            string appat = tbappat.Text;
            string apmat = tbapmat.Text;
            string licenciatura = ddlLicenciatura.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(licenciatura))
            {
                lblErrorMessage.Text = "Complete los campos obligatorios";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Alumno SET nombre_alumno = @Nombre, ap_pat_alumno = @ApellidoPaterno, " +
                    "ap_mat_alumno = @ApellidoMaterno, licenciatura = @Licenciatura WHERE cve_alumno = @Cve_alumno", con);
                con.Open();
                cmd.Parameters.AddWithValue("@Cve_alumno", clave);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@ApellidoPaterno", appat);
                cmd.Parameters.AddWithValue("@ApellidoMaterno", apmat);
                cmd.Parameters.AddWithValue("@Licenciatura", licenciatura);

                int rowsAffected = cmd.ExecuteNonQuery();
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

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            //
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        private bool ClaveExiste(string clave)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Alumno WHERE cve_alumno = @Cve_alumno", con);
            cmd.Parameters.Add("@Cve_alumno", SqlDbType.VarChar).Value = clave;
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }

        private void LimpiarCampos()
        {
            tbclave.Text = "";
            tbnombre.Text = "";
            tbappat.Text = "";
            tbapmat.Text = "";
            ddlLicenciatura.Text = "";
        }
    }
}