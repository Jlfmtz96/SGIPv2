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


namespace SGIPv2.Proyectos
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Proyecto", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

          

            dt.Columns["cve_proyecto"].ColumnName = "Clave";
            dt.Columns["titulo_proyecto"].ColumnName = "Titulo";
            dt.Columns["protocolo"].ColumnName = "Protocolo";
            dt.Columns["alcance"].ColumnName = "Alcance";
            dt.Columns["area"].ColumnName = "Area";
            dt.Columns["fecha_inicio"].ColumnName = "Fecha Inicio";
            dt.Columns["fecha_fin"].ColumnName = "Fecha Fin";
            dt.Columns["reg_etica"].ColumnName = "Registro Ética";
            dt.Columns["lugar_registro"].ColumnName = "Lugar de Registro";
            dt.Columns["CA"].ColumnName = "Cuerpo Académico";
            dt.Columns["financiamiento"].ColumnName = "Financiamiento";
            dt.Columns["grado_posgrado"].ColumnName = "Grado del posgrado";
            dt.Columns["comentarios"].ColumnName = "Comentarios";

            dt.Columns["Clave"].SetOrdinal(0);
            dt.Columns["Titulo"].SetOrdinal(1);


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
            Response.Redirect("~/Proyectos/CRUD.aspx?id=" + id + "&op=R");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim();

            string consulta = "SELECT cve_proyecto, titulo_proyecto, protocolo, alcance, area, fecha_inicio, fecha_fin, reg_etica , lugar_registro , CA , financiamiento  , grado_posgrado, comentarios FROM Proyecto WHERE cve_proyecto LIKE @busqueda OR titulo_proyecto LIKE @busqueda OR protocolo LIKE @busqueda OR alcance LIKE @busqueda OR area LIKE @busqueda OR reg_etica LIKE @busqueda OR lugar_registro LIKE @busqueda OR CA LIKE @busqueda OR financiamiento LIKE @busqueda OR grado_posgrado LIKE @busqueda OR comentarios LIKE @busqueda";



            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            dt.Columns["cve_proyecto"].ColumnName = "Clave";
            dt.Columns["titulo_proyecto"].ColumnName = "Título";
            dt.Columns["protocolo"].ColumnName = "Protocolo";
            dt.Columns["alcance"].ColumnName = "Alcance";
            dt.Columns["area"].ColumnName = "Área";
            dt.Columns["fecha_inicio"].ColumnName = "Fecha de inicio";
            dt.Columns["fecha_fin"].ColumnName = "Fecha de fin";
            dt.Columns["reg_etica"].ColumnName = "Registro de ética";
            dt.Columns["lugar_registro"].ColumnName = "Lugar de registro";
            dt.Columns["CA"].ColumnName = "Cuerpo académico";
            dt.Columns["financiamiento"].ColumnName = "Financiamiento";
            dt.Columns["grado_posgrado"].ColumnName = "Grado del posgrado";
            dt.Columns["comentarios"].ColumnName = "Comentarios";

            dt.Columns["Clave"].SetOrdinal(0);
            dt.Columns["Título"].SetOrdinal(1);

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
            Response.Redirect("~/Proyectos/CRUD.aspx?id=" + id + "&op=U");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Proyectos/CRUD.aspx?id=" + id + "&op=D");
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Proyectos/CRUD.aspx?op=C");
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
                Response.AddHeader("content-disposition", "attachment;filename=Proyectos.pdf");
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
                    Response.AddHeader("content-disposition", "attachment;filename=Proyectos.xls");
                    Response.Charset = "";
                    Response.Output.Write(sw.ToString());
                    Response.End();
                }
            }
        }

    }
}