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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Producto_investigacion WHERE activo = 1", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Remove("activo");

            dt.Columns["titulo_producto"].ColumnName = "Título";
            dt.Columns["fecha_publicacion"].ColumnName = "Fecha de publicación";
            dt.Columns["tipo_pi"].ColumnName = "Tipo";
            dt.Columns["lugar_publicacion"].ColumnName = "Lugar";

            gvpublicaciones.DataSource = dt;
            gvpublicaciones.DataBind();

            con.Close();
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Esconder la celda de la columna ID_producto
                e.Row.Cells[1].Visible = false;
            }
        }

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Establecer el índice de la página seleccionada
            gvpublicaciones.PageIndex = e.NewPageIndex;

            // Volver a cargar los datos en la GridView
            CargarTabla();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim();

            string consulta = "SELECT * FROM Producto_investigacion WHERE (titulo_producto LIKE @busqueda " +
                "COLLATE SQL_Latin1_General_CP1_CI_AI OR fecha_publicacion LIKE @busqueda OR tipo_pi LIKE @busqueda " +
                "COLLATE SQL_Latin1_General_CP1_CI_AI OR lugar_publicacion LIKE @busqueda COLLATE SQL_Latin1_General_CP1_CI_AI) " +
                "AND activo = 1";

            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Remove("activo");

            dt.Columns["titulo_producto"].ColumnName = "Título";
            dt.Columns["fecha_publicacion"].ColumnName = "Fecha de publicación";
            dt.Columns["tipo_pi"].ColumnName = "Tipo";
            dt.Columns["lugar_publicacion"].ColumnName = "Lugar";

            gvpublicaciones.DataSource = dt;
            con.Close();

            if (dt.Rows.Count == 0) // Verificar si no hay resultados
            {
                lblMensaje.Text = "No existen coincidencias con tu búsqueda";
                gvpublicaciones.DataSource = null; // Limpiar los datos en caso de haber algún resultado previo
            }
            else
            {
                lblMensaje.Text = ""; // Limpiar el mensaje en caso de haber resultados anteriores
                gvpublicaciones.DataSource = dt;
            }

            gvpublicaciones.DataBind();
        }


        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Publicaciones/CRUD.aspx?op=C");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            string id = gvpublicaciones.DataKeys[selectedrow.RowIndex].Value.ToString();

            // Actualizar el campo "activo" a 0 en lugar de eliminar físicamente el registro
            using (con)
            {
                string query = "UPDATE Producto_Investigacion SET activo = 0 WHERE ID_producto = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                // Redirigir a la página principal u otra página después de "eliminar" el registro
                Response.Redirect("~/Publicaciones/Index.aspx");
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedRow = (GridViewRow)BtnConsultar.NamingContainer;
            string id = gvpublicaciones.DataKeys[selectedRow.RowIndex].Value.ToString();
            Response.Redirect("~/Publicaciones/CRUD.aspx?id=" + id + "&op=U");
        }

        protected void BtnRead_Click(object sender, EventArgs e)
        {
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedRow = (GridViewRow)BtnConsultar.NamingContainer;
            string id = gvpublicaciones.DataKeys[selectedRow.RowIndex].Value.ToString();
            Response.Redirect("~/Publicaciones/CRUD.aspx?id=" + id + "&op=R");
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