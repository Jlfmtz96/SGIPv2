using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGIPv2.Proyectos
{
    public partial class CRUD : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public static string aCve = "-1";
        public static string aOpc = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener el id
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    aCve = Request.QueryString["id"].ToString();
                    CargarDatos();
                }

                if (Request.QueryString["op"] != null)
                {
                    aOpc = Request.QueryString["op"].ToString();

                    switch (aOpc)
                    {
                        case "C":
                            this.lbltitulo.Text = "Agregar nuevo proyecto";
                            this.BtnCreate.Visible = true;
                            this.tbclave.Enabled = true;
                            break;
                        case "R":
                            this.lbltitulo.Text = "Consulta de proyecto";
                            break;
                        case "U":
                            this.lbltitulo.Text = "Modificar proyecto";
                            this.BtnUpdate.Visible = true;
                            break;
                        case "D":
                            this.lbltitulo.Text = "Dar de baja proyecto";
                            this.BtnCreate.Visible = true;
                            break;
                    }
                }
            }
        }
        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Proyecto WHERE cve_proyecto = @cve_proyecto", con);
            da.SelectCommand.Parameters.Add("@cve_proyecto", SqlDbType.VarChar).Value = aCve;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbclave.Text = row["cve_proyecto"].ToString();
            tbtitulo.Text = row["titulo_proyecto"].ToString();
            tbprotocolo.Text = row["protocolo"].ToString();
            tbalcance.Text = row["alcance"].ToString();
            tbarea.Text = row["area"].ToString();
            tbfinicio.Text = row["fecha_inicio"].ToString();
            tbffin.Text = row["fecha_fin"].ToString();
            tbregetica.Text = row["reg_etica"].ToString();
            tblugar.Text = row["lugar_registro"].ToString();
            tbca.Text = row["CA"].ToString();
            tbfin.Text = row["financiamiento"].ToString();
            tbgradpost.Text = row["grado_posgrado"].ToString();
            tbcomentarios.Text = row["comentarios"].ToString();


            CargarProfesores();
            CargarAlumnos();

            con.Close();
        }

        void CargarProfesores()
        {
            SqlCommand cmd = new SqlCommand("SELECT cve_inv, nombre_investigador FROM Investigador", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Asignar los datos al dropdown de profesores
            tbprofesores.DataSource = dt;
            tbprofesores.DataTextField = "nombre_investigador";
            tbprofesores.DataValueField = "cve_inv";
            tbprofesores.DataBind();
        }

        void CargarAlumnos()
        {
            SqlCommand cmd = new SqlCommand("SELECT cve_alumno, nombre_alumno FROM Alumno", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Asignar los datos al dropdown de alumnos
            tbalumnos.DataSource = dt;
            tbalumnos.DataTextField = "nombre_alumno";
            tbalumnos.DataValueField = "cve_alumno";
            tbalumnos.DataBind();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string titulo = tbtitulo.Text;
            string protocolo = tbprotocolo.Text;
            string alcance = tbalcance.Text;
            string area = tbarea.Text;
            string fechaInicio = tbfinicio.Text;
            string fechaFin = tbffin.Text;
            string regEtica = tbregetica.Text;
            string lugarRegistro = tblugar.Text;
            string ca = tbca.Text;
            string financiamiento = tbfin.Text;
            string gradoPosgrado = tbgradpost.Text;
            string comentarios = tbcomentarios.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(protocolo) || string.IsNullOrWhiteSpace(alcance) || string.IsNullOrWhiteSpace(area) || string.IsNullOrWhiteSpace(fechaInicio) || string.IsNullOrWhiteSpace(fechaFin) || string.IsNullOrWhiteSpace(regEtica) || string.IsNullOrWhiteSpace(lugarRegistro) || string.IsNullOrWhiteSpace(ca) || string.IsNullOrWhiteSpace(financiamiento) || string.IsNullOrWhiteSpace(gradoPosgrado) || string.IsNullOrWhiteSpace(comentarios))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            string query = "INSERT INTO Proyecto (cve_proyecto, titulo_proyecto, protocolo, alcance, area, fecha_inicio, fecha_fin, reg_etica, lugar_registro, CA, financiamiento, grado_posgrado, comentarios) VALUES (@Cve_proyecto, @Titulo, @Protocolo, @Alcance, @Area, @FechaInicio, @FechaFin, @RegEtica, @LugarRegistro, @CA, @Financiamiento, @GradoPosgrado, @Comentarios)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@Cve_proyecto", SqlDbType.VarChar).Value = clave;
                cmd.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = titulo;
                cmd.Parameters.Add("@Protocolo", SqlDbType.VarChar).Value = protocolo;
                cmd.Parameters.Add("@Alcance", SqlDbType.VarChar).Value = alcance;
                cmd.Parameters.Add("@Area", SqlDbType.VarChar).Value = area;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = Convert.ToDateTime(fechaInicio).Date;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = Convert.ToDateTime(fechaFin).Date;
                cmd.Parameters.Add("@RegEtica", SqlDbType.VarChar).Value = regEtica;
                cmd.Parameters.Add("@LugarRegistro", SqlDbType.VarChar).Value = lugarRegistro;
                cmd.Parameters.Add("@CA", SqlDbType.VarChar).Value = ca;
                cmd.Parameters.Add("@Financiamiento", SqlDbType.VarChar).Value = financiamiento;
                cmd.Parameters.Add("@GradoPosgrado", SqlDbType.VarChar).Value = gradoPosgrado;
                cmd.Parameters.Add("@Comentarios", SqlDbType.VarChar).Value = comentarios;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblSuccessMessage.Text = "Proyecto agregado satisfactoriamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                    LimpiarCampos();
                    //Response.Redirect("Index.aspx");
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = "Error al agregar proyecto: " + ex.Message;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                }
                finally
                {
                    con.Close();
                }
            }
        }


        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string titulo = tbtitulo.Text;
            string protocolo = tbprotocolo.Text;
            string alcance = tbalcance.Text;
            string area = tbarea.Text;
            string fechaInicio = tbfinicio.Text;
            string fechaFin = tbffin.Text;
            string regEtica = tbregetica.Text;
            string lugarRegistro = tblugar.Text;
            string ca = tbca.Text;
            string financiamiento = tbfin.Text;
            string gradoPosgrado = tbgradpost.Text;
            string comentarios = tbcomentarios.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(protocolo) || string.IsNullOrWhiteSpace(alcance) || string.IsNullOrWhiteSpace(area) || string.IsNullOrWhiteSpace(fechaInicio) || string.IsNullOrWhiteSpace(fechaFin) || string.IsNullOrWhiteSpace(regEtica) || string.IsNullOrWhiteSpace(lugarRegistro) || string.IsNullOrWhiteSpace(ca) || string.IsNullOrWhiteSpace(financiamiento) || string.IsNullOrWhiteSpace(gradoPosgrado) || string.IsNullOrWhiteSpace(comentarios))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Proyecto SET titulo_proyecto = @Titulo, protocolo = @Protocolo, alcance = @Alcance, area = @Area, fecha_inicio = @FechaInicio, fecha_fin = @FechaFin, reg_etica = @RegEtica, lugar_registro = @LugarRegistro, CA = @CA, financiamiento = @Financiamiento, grado_posgrado = @GradoPosgrado, comentarios = @Comentarios WHERE cve_proyecto = @Cve_proyecto", con);
                con.Open();
                cmd.Parameters.Add("@Cve_proyecto", SqlDbType.VarChar).Value = clave;
                cmd.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = titulo;
                cmd.Parameters.Add("@Protocolo", SqlDbType.VarChar).Value = protocolo;
                cmd.Parameters.Add("@Alcance", SqlDbType.VarChar).Value = alcance;
                cmd.Parameters.Add("@Area", SqlDbType.VarChar).Value = area;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = Convert.ToDateTime(fechaInicio).Date;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = Convert.ToDateTime(fechaFin).Date;
                cmd.Parameters.Add("@RegEtica", SqlDbType.VarChar).Value = regEtica;
                cmd.Parameters.Add("@LugarRegistro", SqlDbType.VarChar).Value = lugarRegistro;
                cmd.Parameters.Add("@CA", SqlDbType.VarChar).Value = ca;
                cmd.Parameters.Add("@Financiamiento", SqlDbType.VarChar).Value = financiamiento;
                cmd.Parameters.Add("@GradoPosgrado", SqlDbType.VarChar).Value = gradoPosgrado;
                cmd.Parameters.Add("@Comentarios", SqlDbType.VarChar).Value = comentarios;

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                lblSuccessMessage.Text = "Proyecto actualizado satisfactoriamente.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessDiv", "showSuccessDiv();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "RedirectAfterDelay", "setTimeout(function() { window.location.href = 'Index.aspx'; }, 2000);", true);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error al actualizar proyecto: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        private bool ClaveExiste(string clave)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Proyecto WHERE cve_proyecto = @Cve_proyecto", con);
            cmd.Parameters.Add("@Cve_proyecto", SqlDbType.VarChar).Value = clave;
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }

        private void LimpiarCampos()
        {
            tbclave.Text = "";
            tbtitulo.Text = "";
            tbprotocolo.Text = "";
            tbalcance.Text = "";
            tbarea.Text = "";
            tbfinicio.Text = "";
            tbffin.Text = "";
            tbregetica.Text = "";
            tblugar.Text = "";
            tbca.Text = "";
            tbfin.Text = "";
            tbgradpost.Text = "";
            tbcomentarios.Text = "";
        }


    }
}