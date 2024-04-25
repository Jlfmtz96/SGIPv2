using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Producto_investigacion", con);
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
            string busqueda = txtBusqueda.Text.Trim();

            string consulta = "SELECT * FROM Producto_investigacion WHERE ID_producto LIKE @busqueda OR titulo_producto LIKE @busqueda OR fecha_publicacion LIKE @busqueda OR tipo_pi LIKE @busqueda OR lugar_publicacion LIKE @busqueda";

            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

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


        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Publicaciones/CRUD.aspx?op=C");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Publicaciones/CRUD.aspx?id=" + id + "&op=U");
        }

        protected void BtnRead_Click(object sender, EventArgs e)
        {

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

                // Agregamos el título del reporte
                Paragraph title = new Paragraph("Reporte de Publicaciones");
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Agregamos espacio adicional
                Paragraph space = new Paragraph("\n");
                document.Add(space);

                // Agregamos la fecha actual
                DateTime currentDate = DateTime.Now;
                Paragraph date = new Paragraph("Fecha: " + currentDate.ToString("dd/MM/yyyy"));
                date.Alignment = Element.ALIGN_RIGHT;
                document.Add(date);

                document.Add(space);

                // Verificamos que el GridView tiene al menos dos columnas
                int columnCount = gvpublicaciones.Columns.Count + 5;
                if (columnCount <= 1)
                {
                    throw new InvalidOperationException("El GridView debe tener al menos dos columnas para crear el PDF.");
                }

                // Creamos una tabla para almacenar los datos del GridView
                PdfPTable table = new PdfPTable(columnCount - 1); // Restamos una columna para omitir la primera

                // Agregamos las cabeceras de las columnas al PDF, omitiendo la primera
                for (int i = 1; i < columnCount; i++) // Comenzamos desde la segunda columna
                {
                    TableCell headerCell = gvpublicaciones.HeaderRow.Cells[i];
                    PdfPCell cell = new PdfPCell(new Phrase(headerCell.Text));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(240, 240, 240);
                    table.AddCell(cell);
                }

                // Agregamos los datos del GridView, omitiendo la primera columna
                for (int i = 0; i < gvpublicaciones.Rows.Count; i++)
                {
                    for (int j = 1; j < columnCount; j++) // Comenzamos desde la segunda columna
                    {
                        TableCell cell = gvpublicaciones.Rows[i].Cells[j];
                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Text));
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
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
                Response.AddHeader("content-disposition", "attachment;filename=Publicaciones " + currentDate.ToString("dd-MM-yyyy") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }











        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET
            // server control at run time.
        }

        protected void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Publicaciones.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            gvpublicaciones.AllowPaging = false;
            gvpublicaciones.DataBind();

            // Hide the buttons column(s) before rendering
            gvpublicaciones.Columns[0].Visible = false;

            gvpublicaciones.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in gvpublicaciones.HeaderRow.Cells)
            {
                cell.BackColor = gvpublicaciones.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gvpublicaciones.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gvpublicaciones.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gvpublicaciones.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gvpublicaciones.RenderControl(hw);

            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }
}