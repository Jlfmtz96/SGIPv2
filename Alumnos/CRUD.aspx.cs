using System;
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
                            break;
                        case "R":
                            this.lbltitulo.Text = "Consulta de alumno";
                            break;
                        case "U":
                            this.lbltitulo.Text = "Modificar alumno";
                            this.BtnUpdate.Visible = true;
                            tbclave.ReadOnly = true;
                            break;
                        case "D":
                            this.lbltitulo.Text = "Dar de baja alumno";
                            this.BtnCreate.Visible = true;
                            break;
                    }
                }
            }
        }

        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("alumno_read", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@cve_alumno", SqlDbType.VarChar).Value=aCve;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbclave.Text = row["cve_alumno"].ToString(); // Establecer la clave del alumno en el textbox
            tbnombre.Text = row["nombre_alumno"].ToString();
            tbappat.Text = row["ap_pat"].ToString();
            tbapmat.Text = row["ap_mat"].ToString();
            tblicenciatura.Text = row["licenciatura"].ToString();
            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("alumnos_create", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Cve_alumno", SqlDbType.VarChar).Value = tbclave.Text;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = tbnombre.Text;
            cmd.Parameters.Add("@Ap_pat", SqlDbType.VarChar).Value = tbappat.Text;
            cmd.Parameters.Add("@Ap_mat", SqlDbType.VarChar).Value = tbapmat.Text;
            cmd.Parameters.Add("@Licenciatura", SqlDbType.VarChar).Value = tblicenciatura.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Alumno SET nombre_alumno = @Nombre, ap_pat = @ApellidoPaterno, ap_mat = @ApellidoMaterno, licenciatura = @Licenciatura WHERE cve_alumno = @Cve_alumno", con);
                con.Open();
                cmd.Parameters.Add("@Cve_alumno", SqlDbType.VarChar).Value = tbclave.Text;
                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = tbnombre.Text;
                cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar).Value = tbappat.Text;
                cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar).Value = tbapmat.Text;
                cmd.Parameters.Add("@Licenciatura", SqlDbType.VarChar).Value = tblicenciatura.Text;

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                Session["UpdateSuccess"] = true;

                Response.Redirect("Index.aspx");
            }
            catch (Exception ex)
            {
                string errorScript = @"<script>
                                document.addEventListener('DOMContentLoaded', function () {
                                    var modalBody = document.getElementById('modalBody');
                                    var message = 'Error al actualizar los datos.';
                                    modalBody.textContent = message;
                                    var myModal = new bootstrap.Modal(document.getElementById('messageModal'), {
                                        backdrop: 'static',
                                        keyboard: false
                                    });
                                    myModal.show();
                                });
                            </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", errorScript);
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }
    }
}