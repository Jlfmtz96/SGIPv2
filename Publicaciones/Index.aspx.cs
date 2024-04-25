using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2.Publicaciones
{
    public partial class Index : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }

        void CargarTabla()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Publicacion", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns["ID_producto"].ColumnName = "Id";
            dt.Columns["titulo_producto"].ColumnName = "Título";
            dt.Columns["fecha_publicacion"].ColumnName = "Fecha de publicación";
            dt.Columns["tipo_pi"].ColumnName = "Tipo";
            dt.Columns["lugar_publicacion"].ColumnName = "Lugar";

            dt.Columns["Id"].SetOrdinal(0);
            dt.Columns["Título"].SetOrdinal(1);

            gvpublicaciones.DataSource = dt;
            gvpublicaciones.DataBind();
            con.Close();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Publicaciones/CRUD.aspx?op=C");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRead_Click(object sender, EventArgs e)
        {

        }
    }
}