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
            SqlCommand cmd = new SqlCommand("SELECT cve_proyecto, titulo_proyecto, nivel, grado_posgrado, fecha_inicio, fecha_fin FROM Proyecto", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

          

            dt.Columns["cve_proyecto"].ColumnName = "Clave";
            dt.Columns["titulo_proyecto"].ColumnName = "Titulo";
            dt.Columns["nivel"].ColumnName = "Nivel del posgrado";
            dt.Columns["grado_posgrado"].ColumnName = "Grado del posgrado";
            dt.Columns["fecha_inicio"].ColumnName = "Fecha Inicio";
            dt.Columns["fecha_fin"].ColumnName = "Fecha Fin";

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
                // Inicializamos un documento PDF
                Document document = new Document();

                // Inicializamos PdfWriter con el MemoryStream
                PdfWriter.GetInstance(document, ms);

                // Abrimos el documento
                document.Open();

                // Agregamos el título del reporte
                Paragraph title = new Paragraph("Reporte de Proyectos");
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
                int columnCount = gvinv.Columns.Count + 13;
                if (columnCount <= 1)
                {
                    throw new InvalidOperationException("El GridView debe tener al menos dos columnas para crear el PDF.");
                }

                // Creamos una tabla para almacenar los datos del GridView
                PdfPTable table = new PdfPTable(columnCount - 1); // Restamos una columna para omitir la primera

                // Agregamos las cabeceras de las columnas al PDF, omitiendo la primera
                for (int i = 1; i < columnCount; i++) // Comenzamos desde la segunda columna
                {
                    TableCell headerCell = gvinv.HeaderRow.Cells[i];
                    PdfPCell cell = new PdfPCell(new Phrase(headerCell.Text));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(240, 240, 240);
                    table.AddCell(cell);
                }

                // Agregamos los datos del GridView, omitiendo la primera columna
                for (int i = 0; i < gvinv.Rows.Count; i++)
                {
                    for (int j = 1; j < columnCount; j++) // Comenzamos desde la segunda columna
                    {
                        TableCell cell = gvinv.Rows[i].Cells[j];
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
                Response.AddHeader("content-disposition", "attachment;filename=Proyectos " + currentDate.ToString("dd-MM-yyyy") + ".pdf");
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
                    DateTime currentDate = DateTime.Now;
                    Page page = new Page();
                    HtmlForm form = new HtmlForm();
                    gvinv.EnableViewState = false; // Asegúrate de deshabilitar la vista de estado si no es necesario

                    page.EnableEventValidation = false; // Deshabilitar la validación de eventos para evitar excepciones

                    // Agregar el formulario al conjunto de controles de la página
                    page.Controls.Add(form);
                    form.Controls.Add(gvinv);

                    // Obtener los índices de las columnas que deben mostrarse
                    List<int> visibleColumnIndexes = GetVisibleColumnIndexes();

                    // Ocultar las columnas que no deben mostrarse
                    foreach (DataControlField column in gvinv.Columns)
                    {
                        // Verificar si la columna debe estar visible
                        if (!visibleColumnIndexes.Contains(gvinv.Columns.IndexOf(column)))
                        {
                            // Ocultar la columna
                            column.Visible = false;
                        }
                    }

                    // Realizar el rendering del formulario
                    page.DesignerInitialize();
                    page.RenderControl(hw);

                    // Escribir el contenido del StringWriter en la respuesta
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=Proyectos" + currentDate + ".xls");
                    Response.Charset = "";
                    Response.Output.Write(sw.ToString());
                    Response.End();
                }
            }
        }

        // Método para obtener los índices de las columnas visibles basado en el estado de las casillas de verificación
        private List<int> GetVisibleColumnIndexes()
        {
            List<int> visibleColumnIndexes = new List<int>();
            for (int i = 0; i < gvinv.Columns.Count; i++)
            {
                // Verificar si la casilla de verificación correspondiente a la columna está marcada
                CheckBox chkBox = FindCheckBoxByHeaderText(gvinv.Columns[i].HeaderText);
                if (chkBox != null && chkBox.Checked)
                {
                    visibleColumnIndexes.Add(i);
                }
            }
            return visibleColumnIndexes;
        }

        // Método auxiliar para encontrar la casilla de verificación según el texto de encabezado de columna
        private CheckBox FindCheckBoxByHeaderText(string headerText)
        {
            foreach (Control control in Page.Controls)
            {
                if (control is CheckBox && ((CheckBox)control).Text == headerText)
                {
                    return (CheckBox)control;
                }
            }
            return null;
        }

    }
}