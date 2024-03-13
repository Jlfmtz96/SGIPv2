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
            tbca.Text = row["cuerpo_academico"].ToString();
            tbsni.Text = row["SNI"].ToString();
            // Obtener el estado del investigador
            bool activo = Convert.ToBoolean(row["activo"]);

            // Establecer el estado del checkbox
            CheckBoxActivo.Checked = activo;

            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Investigador (cve_inv, nombre_investigador, ap_pat, ap_mat, correo, cuerpo_academico, SNI, activo) VALUES (@Cve_investigador, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Correo, @CuerpoAcademico, @SNI, @Activo)", con);
            con.Open();
            cmd.Parameters.Add("@Cve_investigador", SqlDbType.VarChar).Value = tbclave.Text;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = tbnombre.Text;
            cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar).Value = tbappat.Text;
            cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar).Value = tbapmat.Text;
            cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = tbcorreo.Text;
            cmd.Parameters.Add("@CuerpoAcademico", SqlDbType.VarChar).Value = tbca.Text;
            cmd.Parameters.Add("@SNI", SqlDbType.VarChar).Value = tbsni.Text;
            // Aquí asumimos que tienes un CheckBox llamado CheckBoxActivo
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = CheckBoxActivo.Checked ? 1 : 0;

            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");

        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Investigador SET nombre_investigador = @Nombre, ap_pat = @ApellidoPaterno, ap_mat = @ApellidoMaterno, correo = @Correo, cuerpo_academico = @CuerpoAcademico, SNI = @SNI, activo = @Activo WHERE cve_inv = @Cve_investigador", con);
            con.Open();
            cmd.Parameters.Add("@Cve_investigador", SqlDbType.VarChar).Value = tbclave.Text;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = tbnombre.Text;
            cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar).Value = tbappat.Text;
            cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar).Value = tbapmat.Text;
            cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = tbcorreo.Text;
            cmd.Parameters.Add("@CuerpoAcademico", SqlDbType.VarChar).Value = tbca.Text;
            cmd.Parameters.Add("@SNI", SqlDbType.VarChar).Value = tbsni.Text;
            // Aquí asumimos que tienes un CheckBox llamado CheckBoxActivo
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = CheckBoxActivo.Checked ? 1 : 0;
            
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}