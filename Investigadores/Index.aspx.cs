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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Investigador WHERE registro_activo = 1", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string nombreCompleto = row["nombre_investigador"].ToString() + " " + row["ap_pat"].ToString() + " " + 
                    row["ap_mat"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Add("SNI_Vigencia", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string sni_vigencia = row["SNI"].ToString() + " / " + row["sni_vigencia"].ToString();
                row["SNI_Vigencia"] = sni_vigencia;
            }

            dt.Columns.Add("PRODEP_Vigencia", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string prodep_vigencia = row["perfil_prodep"].ToString() + " / " + row["prodep_vigencia"].ToString();
                row["PRODEP_Vigencia"] = prodep_vigencia;
            }

            dt.Columns.Remove("nombre_investigador");
            dt.Columns.Remove("ap_pat");
            dt.Columns.Remove("ap_mat");
            dt.Columns.Remove("SNI");
            dt.Columns.Remove("sni_vigencia");
            dt.Columns.Remove("perfil_prodep");
            dt.Columns.Remove("prodep_vigencia");
            dt.Columns.Remove("registro_activo");

            dt.Columns["cve_inv"].ColumnName = "Clave UASLP";
            dt.Columns["NombreCompleto"].ColumnName = "Nombre";
            dt.Columns["Correo"].ColumnName = "Correo";
            dt.Columns["cuerpo_academico"].ColumnName = "Cuerpo Academico";
            dt.Columns["lider_CA"].ColumnName = "Líder del CA";
            dt.Columns["SNI_Vigencia"].ColumnName = "SNI / Vigencia";
            dt.Columns["activo"].ColumnName = "Activo";
            dt.Columns["PRODEP_Vigencia"].ColumnName = "PRODEP / Vigencia";

            dt.Columns["Clave UASLP"].SetOrdinal(0);
            dt.Columns["Nombre"].SetOrdinal(1);


            gvinv.DataSource = dt;
            gvinv.DataBind();
            con.Close();
        }

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Establecer el índice de la página seleccionada
            gvinv.PageIndex = e.NewPageIndex;

            // Volver a cargar los datos en la GridView
            CargarTabla();
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

            string consulta = "SELECT * FROM Investigador " +
                "WHERE (cve_inv LIKE @busqueda OR nombre_investigador LIKE @busqueda COLLATE SQL_Latin1_General_CP1_CI_AI OR ap_pat LIKE " +
                "@busqueda COLLATE SQL_Latin1_General_CP1_CI_AI OR ap_mat LIKE @busqueda COLLATE SQL_Latin1_General_CP1_CI_AI " +
                "OR correo LIKE @busqueda COLLATE SQL_Latin1_General_CP1_CI_AI OR cuerpo_academico LIKE @busqueda " +
                "COLLATE SQL_Latin1_General_CP1_CI_AI OR SNI LIKE @busqueda OR sni_vigencia LIKE @busqueda OR perfil_prodep LIKE " +
                "@busqueda OR prodep_vigencia LIKE @busqueda) AND (@activo = -1 OR activo = @activo) AND registro_activo = 1";

            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");

            cmd.Parameters.AddWithValue("@activo", ddlActivo.SelectedValue); // Valor seleccionado en el DropDownList

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string nombreCompleto = row["nombre_investigador"].ToString() + " " + row["ap_pat"].ToString() + " " + 
                    row["ap_mat"].ToString();
                row["NombreCompleto"] = nombreCompleto;
            }

            dt.Columns.Add("SNI_Vigencia", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string sni_vigencia = row["SNI"].ToString() + " / " + row["sni_vigencia"].ToString();
                row["SNI_Vigencia"] = sni_vigencia;
            }

            dt.Columns.Add("PRODEP_Vigencia", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string prodep_vigencia = row["perfil_prodep"].ToString() + " / " + row["prodep_vigencia"].ToString();
                row["PRODEP_Vigencia"] = prodep_vigencia;
            }

            dt.Columns.Remove("nombre_investigador");
            dt.Columns.Remove("ap_pat");
            dt.Columns.Remove("ap_mat");
            dt.Columns.Remove("SNI");
            dt.Columns.Remove("sni_vigencia");
            dt.Columns.Remove("perfil_prodep");
            dt.Columns.Remove("prodep_vigencia");
            dt.Columns.Remove("registro_activo");

            dt.Columns["cve_inv"].ColumnName = "Clave UASLP";
            dt.Columns["NombreCompleto"].ColumnName = "Nombre";
            dt.Columns["Correo"].ColumnName = "Correo";
            dt.Columns["cuerpo_academico"].ColumnName = "Cuerpo Academico";
            dt.Columns["lider_CA"].ColumnName = "Líder del CA";
            dt.Columns["SNI_Vigencia"].ColumnName = "SNI / Vigencia";
            dt.Columns["activo"].ColumnName = "Activo";
            dt.Columns["PRODEP_Vigencia"].ColumnName = "PRODEP / Vigencia";

            dt.Columns["Clave UASLP"].SetOrdinal(0);
            dt.Columns["Nombre"].SetOrdinal(1);

            gvinv.DataSource = dt;
            //gvinv.DataBind();
            con.Close();

            if (dt.Rows.Count == 0) // Verificar si no hay resultados
            {
                lblMensaje.Text = "No existen coincidencias con tu búsqueda";
                gvinv.DataSource = null; // Limpiar los datos en caso de haber algún resultado previo
            }
            else
            {
                lblMensaje.Text = ""; // Limpiar el mensaje en caso de haber resultados anteriores
                gvinv.DataSource = dt;
            }

            gvinv.DataBind();
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

            // Actualizar el campo "activo" a 0 en lugar de eliminar físicamente el registro
            using (con)
            {
                string query = "UPDATE Investigador SET registro_activo = 0 WHERE cve_inv = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                // Redirigir a la página principal u otra página después de "eliminar" el registro
                Response.Redirect("~/Investigadores/Index.aspx");
            }
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