using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using iTextSharp.text.pdf;
using iTextSharp.text;

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
            SqlCommand cmd = new SqlCommand("SELECT cve_alumno, nombre_alumno, ap_pat_alumno, ap_mat_alumno, licenciatura FROM Alumno", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows) 
            {
                string nombreCompleto = row["nombre_alumno"].ToString() + " " + row["ap_pat_alumno"].ToString() + " " + row["ap_mat_alumno"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Remove("nombre_alumno");
            dt.Columns.Remove("ap_pat_alumno");
            dt.Columns.Remove("ap_mat_alumno");


            dt.Columns["cve_alumno"].ColumnName = "Clave UASLP";
            dt.Columns["NombreCompleto"].ColumnName = "Nombre";
            dt.Columns["licenciatura"].ColumnName = "Licenciatura";

            dt.Columns["Clave UASLP"].SetOrdinal(0);
            dt.Columns["Nombre"].SetOrdinal(1);

            gvalumnos.DataSource = dt;
            gvalumnos.DataBind();
            con.Close();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim();

            string consulta = "SELECT cve_alumno, nombre_alumno, ap_pat_alumno, ap_mat_alumno, licenciatura FROM Alumno WHERE nombre_alumno LIKE @busqueda OR ap_pat_alumno LIKE @busqueda OR ap_mat_alumno LIKE @busqueda";

            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string nombreCompleto = row["nombre_alumno"].ToString() + " " + row["ap_pat_alumno"].ToString() + " " + row["ap_mat_alumno"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Remove("nombre_alumno");
            dt.Columns.Remove("ap_pat_alumno");
            dt.Columns.Remove("ap_mat_alumno");

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

        protected void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            // Creamos un MemoryStream para almacenar la salida PDF
            using (MemoryStream ms = new MemoryStream())
            {
                // Inicializamos un documento PDF
                Document document = new Document();

                // Inicializamos PdfWriter con el MemoryStream
                PdfWriter.GetInstance(document, ms);

                // Abrimos el documento
                document.Open();

                // Agregamos un título al documento
                Paragraph title = new Paragraph("Reporte de Alumnos");
                title.Font.Size = 16;
                document.Add(title);

                // Creamos una tabla para almacenar los datos del GridView
                PdfPTable table = new PdfPTable(gvalumnos.Columns.Count);
                table.WidthPercentage = 100; // Para que la tabla ocupe el ancho completo de la página

                // Agregamos las cabeceras de las columnas al PDF
                foreach (TableCell headerCell in gvalumnos.HeaderRow.Cells)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(headerCell.Text));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER; // Alineamos el contenido al centro
                    table.AddCell(cell);
                }

                // Agregamos las filas y columnas de datos al PDF
                foreach (GridViewRow row in gvalumnos.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Text));
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER; // Alineamos el contenido al centro
                        table.AddCell(pdfCell);
                    }
                }

                // Agregamos la tabla al documento
                document.Add(table);

                // Cerramos el documento
                document.Close();

                // Escribimos el contenido del MemoryStream en la respuesta
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Alumnos.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
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
                    gvalumnos.EnableViewState = false; // Asegúrate de deshabilitar la vista de estado si no es necesario

                    page.EnableEventValidation = false; // Deshabilitar la validación de eventos para evitar excepciones

                    // Agregar el formulario al conjunto de controles de la página
                    page.Controls.Add(form);
                    form.Controls.Add(gvalumnos);

                    // Ocultar la primera columna del GridView antes de realizar el rendering
                    gvalumnos.Columns[0].Visible = false;

                    // Realizar el rendering del formulario
                    page.DesignerInitialize();
                    page.RenderControl(hw);

                    // Escribir el contenido del StringWriter en la respuesta
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=Alumnos.xls");
                    Response.Charset = "";
                    Response.Output.Write(sw.ToString());
                    Response.End();
                }
            }
        }
    }
}