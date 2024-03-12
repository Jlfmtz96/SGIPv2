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
    public partial class Index : System.Web.UI.Page
    {
        readonly SqlConnection con=new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTabla();
            }
        }

        void CargarTabla()
        {
            SqlCommand cmd = new SqlCommand("alumnos_load", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows) 
            {
                string nombreCompleto = row["nombre_alumno"].ToString() + " " + row["ap_pat"].ToString() + " " + row["ap_mat"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Remove("nombre_alumno");
            dt.Columns.Remove("ap_pat");
            dt.Columns.Remove("ap_mat");


            dt.Columns["cve_alumno"].ColumnName = "Clave UASLP";
            dt.Columns["NombreCompleto"].ColumnName = "Nombre";
            dt.Columns["licenciatura"].ColumnName = "Licenciatura";

            dt.Columns["Clave UASLP"].SetOrdinal(0);
            dt.Columns["Nombre"].SetOrdinal(1);

            gvalumnos.DataSource = dt;
            gvalumnos.DataBind();
            con.Close();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Alumnos/CRUD.aspx?op=C");
        }

        protected void BtnRead_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Alumnos/CRUD.aspx?id="+id+"&op=R");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Alumnos/CRUD.aspx?id=" + id + "&op=U");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Alumnos/CRUD.aspx?id=" + id + "&op=D");
        }
    }
}