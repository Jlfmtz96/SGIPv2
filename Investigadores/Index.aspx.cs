using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


namespace SGIPv2.Investigadores
{
    public partial class Index : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTabla();
            }
        }

        void CargarTabla()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Investigador", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string nombreCompleto = row["nombre_investigador"].ToString() + " " + row["ap_pat"].ToString() + " " + row["ap_mat"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Remove("nombre_investigador");
            dt.Columns.Remove("ap_pat");
            dt.Columns.Remove("ap_mat");


            dt.Columns["cve_inv"].ColumnName = "Clave UASLP";
            dt.Columns["NombreCompleto"].ColumnName = "Nombre";
            dt.Columns["Correo"].ColumnName = "correo";
            dt.Columns["cuerpo_academico"].ColumnName = "Cuerpo Academico";
            dt.Columns["SNI"].ColumnName = "SNI";
            dt.Columns["activo"].ColumnName = "Activo";

            dt.Columns["Clave UASLP"].SetOrdinal(0);
            dt.Columns["Nombre"].SetOrdinal(1);


            gvinv.DataSource = dt;
            gvinv.DataBind();
            con.Close();
        }

        protected void BtnRead_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Investigadores/CRUD.aspx?id=" + id + "&op=R");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim();

            string consulta = "SELECT cve_inv, nombre_investigador, ap_pat, ap_mat, correo, cuerpo_academico, SNI, activo FROM Investigador WHERE cve_inv LIKE @busqueda OR nombre_investigador LIKE @busqueda OR ap_pat LIKE @busqueda OR ap_mat LIKE @busqueda OR correo LIKE @busqueda or SNI LIKE @busqueda or activo LIKE @busqueda";



            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string nombreCompleto = row["nombre_investigador"].ToString() + " " + row["ap_pat"].ToString() + " " + row["ap_mat"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Remove("nombre_investigador");
            dt.Columns.Remove("ap_pat");
            dt.Columns.Remove("ap_mat");

            dt.Columns["cve_inv"].ColumnName = "Clave UASLP";
            dt.Columns["NombreCompleto"].ColumnName = "Nombre";
            dt.Columns["Correo"].ColumnName = "Correo";
            dt.Columns["cuerpo_academico"].ColumnName = "Cuerpo Academico";
            dt.Columns["SNI"].ColumnName = "SNI";
            dt.Columns["activo"].ColumnName = "Activo";

            dt.Columns["Clave UASLP"].SetOrdinal(0);
            dt.Columns["Nombre"].SetOrdinal(1);

            //lblResultsCount.Text = "Mostrando " + dt.Rows.Count + " de " + dt.Rows.Count + " resultados";


            gvinv.DataSource = dt;
            gvinv.DataBind();
            con.Close();
        }


        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Investigadores/CRUD.aspx?id=" + id + "&op=U");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Investigadores/CRUD.aspx?id=" + id + "&op=D");
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Investigadores/CRUD.aspx?op=C");
        }

        protected void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            // Creamos un MemoryStream para almacenar la salida PDF
            using (MemoryStream ms = new MemoryStream())
            {
                // Creamos un documento PDF
                Document document = new Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Convertimos el contenido del control GridView a HTML y lo agregamos al documento PDF
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvinv.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                HTMLWorker htmlparser = new HTMLWorker(document);
                htmlparser.Parse(sr);

                // Cerramos el documento y liberamos los recursos
                document.Close();

                // Escribir el contenido del MemoryStream en la respuesta
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Investigadores.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }

        }

        protected void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            // Creamos un StringWriter para almacenar la salida HTML generada por el control GridView
            using (StringWriter sw = new StringWriter())
            {
                // Creamos un formulario temporal y agregamos el control GridView a ese formulario
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    Page page = new Page();
                    HtmlForm form = new HtmlForm();
                    gvinv.EnableViewState = false; // Asegúrate de deshabilitar la vista de estado si no es necesario

                    page.EnableEventValidation = false; // Deshabilitar la validación de eventos para evitar excepciones

                    // Agregar el formulario al conjunto de controles de la página
                    page.Controls.Add(form);
                    form.Controls.Add(gvinv);

                    // Realizar el rendering del formulario
                    page.DesignerInitialize();
                    page.RenderControl(hw);

                    // Escribir el contenido del StringWriter en la respuesta
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=Investigadores.xls");
                    Response.Charset = "";
                    Response.Output.Write(sw.ToString());
                    Response.End();
                }
            }
        }
    }
}