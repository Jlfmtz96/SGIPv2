﻿using iTextSharp.text.pdf;
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

namespace SGIPv2.Revistas
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTabla();
                CheckAllCheckboxes();
            }
        }

        void CheckAllCheckboxes()
        {
            chkColumn1.Checked = true;
            chkColumn2.Checked = true;
            chkColumn3.Checked = true;
            chkColumn4.Checked = true;
        }

        void CargarTabla()
        {
            SqlCommand cmd = new SqlCommand("SELECT ID_revista AS 'Clave ISBN', nombre_revista AS 'Nombre', tipo_revista AS 'Tipo', " +
                "pais AS 'País', doi AS 'DOI' FROM Revista WHERE activo = 1", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvrevistas.DataSource = dt;
            gvrevistas.DataBind();
            con.Close();
        }

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Establecer el índice de la página seleccionada
            gvrevistas.PageIndex = e.NewPageIndex;

            // Volver a cargar los datos en la GridView
            CargarTabla();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text.Trim();

            string consulta = "SELECT ID_revista AS 'Clave ISBN', nombre_revista AS 'Nombre', tipo_revista AS 'Tipo', pais AS 'País', " +
                "doi AS 'DOI' FROM Revista WHERE (ID_revista LIKE @busqueda OR nombre_revista LIKE @busqueda " +
                "COLLATE SQL_Latin1_General_CP1_CI_AI OR tipo_revista LIKE @busqueda COLLATE SQL_Latin1_General_CP1_CI_AI OR pais LIKE " +
                "@busqueda COLLATE SQL_Latin1_General_CP1_CI_AI) AND activo = 1";

            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvrevistas.DataSource = dt;
            con.Close();

            if (dt.Rows.Count == 0) // Verificar si no hay resultados
            {
                lblMensaje.Text = "No existen coincidencias con tu búsqueda";
                gvrevistas.DataSource = null; // Limpiar los datos en caso de haber algún resultado previo
            }
            else
            {
                lblMensaje.Text = ""; // Limpiar el mensaje en caso de haber resultados anteriores
                gvrevistas.DataSource = dt;
            }

            gvrevistas.DataBind();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Revistas/CRUD.aspx?op=C");
        }

        protected void BtnRead_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Revistas/CRUD.aspx?id=" + id + "&op=R");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Revistas/CRUD.aspx?id=" + id + "&op=U");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string id;
            Button BtnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)BtnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;

            // Actualizar el campo "activo" a 0 en lugar de eliminar físicamente el registro
            using (con)
            {
                string query = "UPDATE Revista SET activo = 0 WHERE ID_revista = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                // Redirigir a la página principal u otra página después de "eliminar" el registro
                Response.Redirect("~/Revistas/Index.aspx");
            }
        }

        /*protected void btnGenerarPDF_Click(object sender, EventArgs e)
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
                Paragraph title = new Paragraph("Reporte de Revistas");
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                Paragraph space = new Paragraph("\n"); // Add as many newline characters as you need
                document.Add(space);
                DateTime currentDate = DateTime.Now;
                Paragraph date = new Paragraph("Fecha: " + currentDate.ToString("dd/MM/yyyy")); // Adjust date format as needed
                date.Alignment = Element.ALIGN_RIGHT; // Align the date to the right
                document.Add(date);
                document.Add(space);

                // Creamos una tabla para almacenar los datos del GridView
                PdfPTable table = new PdfPTable(gvrevistas.Columns.Count +4); // +1 for header column

                // Agregamos las cabeceras de las columnas al PDF (atributos de los alumnos)
                foreach (TableCell headerCell in gvrevistas.HeaderRow.Cells)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(headerCell.Text));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(240, 240, 240);
                    table.AddCell(cell);
                }

                // Transponemos los datos y los agregamos a la tabla del PDF
                for (int i = 0; i < gvrevistas.Rows.Count; i++)
                {
                    //table.AddCell(new Phrase(gvrevistas.Rows[i].Cells[1].Text)); // Student name

                    // Datos de los estudiantes (los demás atributos)
                    foreach (TableCell cell in gvrevistas.Rows[i].Cells)
                    {
                        if (cell == gvrevistas.Rows[i].Cells[0]) // Skip first cell which contains the name
                            continue;

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
                Response.AddHeader("content-disposition", "attachment;filename=Revistas " + currentDate + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }*/

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
                Paragraph title = new Paragraph("Reporte de Revistas");
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
                int columnCount = gvrevistas.Columns.Count+4;
                if (columnCount <= 1)
                {
                    throw new InvalidOperationException("El GridView debe tener al menos dos columnas para crear el PDF.");
                }

                // Creamos una tabla para almacenar los datos del GridView
                PdfPTable table = new PdfPTable(columnCount -1); // Restamos una columna para omitir la primera

                // Agregamos las cabeceras de las columnas al PDF, omitiendo la primera
                for (int i = 1; i < columnCount; i++) // Comenzamos desde la segunda columna
                {
                    TableCell headerCell = gvrevistas.HeaderRow.Cells[i];
                    PdfPCell cell = new PdfPCell(new Phrase(headerCell.Text));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(240, 240, 240);
                    table.AddCell(cell);
                }

                // Agregamos los datos del GridView, omitiendo la primera columna
                for (int i = 0; i < gvrevistas.Rows.Count; i++)
                {
                    for (int j = 1; j < columnCount; j++) // Comenzamos desde la segunda columna
                    {
                        TableCell cell = gvrevistas.Rows[i].Cells[j];
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
                Response.AddHeader("content-disposition", "attachment;filename=Revistas " + currentDate.ToString("dd-MM-yyyy") + ".pdf");
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
                    gvrevistas.EnableViewState = false; // Asegúrate de deshabilitar la vista de estado si no es necesario

                    page.EnableEventValidation = false; // Deshabilitar la validación de eventos para evitar excepciones

                    // Agregar el formulario al conjunto de controles de la página
                    page.Controls.Add(form);
                    form.Controls.Add(gvrevistas);

                    // Obtener los índices de las columnas que deben mostrarse
                    List<int> visibleColumnIndexes = GetVisibleColumnIndexes();

                    // Ocultar las columnas que no deben mostrarse
                    foreach (DataControlField column in gvrevistas.Columns)
                    {
                        // Verificar si la columna debe estar visible
                        if (!visibleColumnIndexes.Contains(gvrevistas.Columns.IndexOf(column)))
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
                    Response.AddHeader("content-disposition", "attachment;filename=Revistas" + currentDate + ".xls");
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
            for (int i = 0; i < gvrevistas.Columns.Count; i++)
            {
                // Verificar si la casilla de verificación correspondiente a la columna está marcada
                CheckBox chkBox = FindCheckBoxByHeaderText(gvrevistas.Columns[i].HeaderText);
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