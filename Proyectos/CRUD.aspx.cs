using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace SGIPv2.Proyectos
{
    public partial class CRUD : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public static string aCve = "-1";
        public static string aOpc = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarProfesores();
            CargarAlumnos();
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

            // Agregar el script necesario para manejar la apertura del modal
            /*ClientScript.RegisterStartupScript(this.GetType(), "ModalScript", @"
                document.getElementById('btnSearch').addEventListener('click', function() {
                    var modal = document.getElementById('myModal');
                    modal.classList.remove('hidden');
                    document.body.classList.add('overflow-hidden'); // Agregar clase para bloquear el scroll
                });

                // Manejar el cierre del modal al hacer clic en el botón de cierre
                document.querySelector('.modal .close').addEventListener('click', function() {
                    var modal = document.getElementById('myModal');
                    modal.classList.add('hidden');
                    document.body.classList.remove('overflow-hidden'); // Quitar clase para desbloquear el scroll
                });

                // Manejar la búsqueda al escribir en el campo de búsqueda del modal
                document.getElementById('txtBusquedaModal').addEventListener('input', function() {
                    var busqueda = this.value.trim();
                    BuscarInvestigador(busqueda);
                });
            ", true);*/
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
            //ddlNivel.Value = row["nivel"].ToString();
            //ddlOptions.Value = row["posgrado"].ToString();
            //tbRedInput.Text = row["red"].ToString();
            tbfinicio.Text = row["fecha_inicio"].ToString();
            tbffin.Text = row["fecha_fin"].ToString();
            //tbregetica.Text = row["reg_etica"].ToString();
            //tblugar.Text = row["lugar_registro"].ToString();
            //tbca.Text = row["CA"].ToString();
            comments.Value = row["comentarios"].ToString();


            CargarProfesores();
            CargarAlumnos();

            con.Close();
        }

        void CargarProfesores()
        {
            searchResults.Items.Clear();

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT cve_inv, nombre_investigador, ap_pat, ap_mat FROM Investigador", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string cve_inv = reader["cve_inv"].ToString();
                    string nombre = reader["nombre_investigador"].ToString();
                    string apPat = reader["ap_pat"].ToString();
                    string apMat = reader["ap_mat"].ToString();

                    string nombre_completo = nombre + " " + apPat + " " + apMat;
                    string textoItem = cve_inv + " - " + nombre_completo;

                    ListItem item = new ListItem(textoItem, cve_inv);

                    // Crear un nuevo elemento de lista y agregarlo al div searchResults
                    searchResults.Items.Add(item);
                }
                reader.Close();
            }
            catch (Exception ex) { }
            finally
            {
                con.Close();
            }
        }


        void CargarAlumnos()
        {
            SqlCommand cmd = new SqlCommand("SELECT cve_alumno, nombre_alumno FROM Alumno", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Asignar los datos al dropdown de alumnos
            ddAlumnos.DataSource = dt;
            ddAlumnos.DataTextField = "nombre_alumno";
            ddAlumnos.DataValueField = "cve_alumno";
            ddAlumnos.DataBind();
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string clave = tbclave.Text;
            string titulo = tbtitulo.Text;
            string nivelProyecto = string.Empty;

            if (enfermeria.Checked)
            {
                nivelProyecto = "Enfermería";
            }
            else if (nutricion.Checked)
            {
                nivelProyecto = "Nutrición";
            }
            else if (otro.Checked)
            {
                nivelProyecto = otroInput.Value;
            }

            string nivelPosgrado = string.Empty;

            if (especialidad.Checked)
            {
                nivelPosgrado = "Especialidad en " + especialidadInput.Value;
            }
            else if (master.Checked)
            {
                nivelPosgrado = "Maestría en " + masterInput.Value;
            }
            else if (doctor.Checked)
            {
                nivelPosgrado = "Doctorado en " + doctorInput.Value;
            }
            else if (no.Checked)
            {
                nivelPosgrado = "no";
            }

            string proyectoRed = string.Empty;

            if (redNo.Checked)
            {
                proyectoRed = "no";
            }
            else if (nombreRed.Checked)
            {
                proyectoRed = redInput.Value;
            }

            string fechaInicio = tbfinicio.Text;
            string fechaFin = tbffin.Text;

            string registroCE = string.Empty;
            string numeroCE = string.Empty;
            string instCE = string.Empty;
            string otroCE = string.Empty;

            if (siCE.Checked)
            {
                // Si la opción "Si" está seleccionada, recuperamos los valores de los campos de texto
                registroCE = "si";
                numeroCE = numeroInput.Value; // Número
                instCE = instInput.Value;     // Nombre de la institución que otorga
                otroCE = instOInput.Value;    // Otro
            }
            else if (noCE.Checked)
            {
                registroCE = "no";
            }

            string lugarAplicacion = string.Empty;

            if (interno.Checked)
            {
                lugarAplicacion = "Interno";
            }
            else if (externo.Checked)
            {
                lugarAplicacion = "Externo";
            }

            string lugar = lugarInput.Value; // Obtener el valor del campo de texto "Lugar"
            string fechaAp = tbfechaAp.Text;

            string perteneceInvCA = string.Empty;
            string nombreInvCA = string.Empty;
            if (noInv.Checked)
            {
                perteneceInvCA = "no";
                nombreInvCA = "no";
            }
            else if (siInv.Checked)
            {
                perteneceInvCA = "si";
                nombreInvCA = invCAInput.Value; // Obtener el valor del campo de texto "Nombre" si la opción "Si" está seleccionada
            }

            string perteneceCA = string.Empty;
            string nombreCA = string.Empty;
            if (caNo.Checked)
            {
                perteneceCA = "no";
                nombreCA = "no";
            }
            else if (caSi.Checked)
            {
                nombreCA = caInput.Value; // Obtener el valor del campo de texto "Nombre" si la opción "Nombre" está seleccionada
            }

            string difundiraFueraFacultad = string.Empty;
            string nombreMedio = string.Empty;

            if (mediosNo.Checked)
            {
                difundiraFueraFacultad = "no";
                nombreMedio = "no";
            }
            else if (mediosSi.Checked)
            {
                nombreMedio = medioInput.Value; // Obtener el valor del campo de texto "Medio" si la opción "Nombre de Medio" está seleccionada
            }

            // Crear una lista para almacenar las claves de los investigadores
            List<string> clavesInvestigadores = new List<string>();

            // Recorrer los elementos del ListBox y tomar solo las claves
            foreach (ListItem item in ListBox1.Items)
            {
                // Obtener el texto completo del elemento
                string textoCompleto = item.Text;

                // Dividir el texto en base a algún carácter que separa la clave del nombre
                // Por ejemplo, si el formato es "clave - nombre", podrías dividirlo usando el carácter '-'
                string[] partes = textoCompleto.Split('-');

                // Suponiendo que la clave está en la primera parte después de dividir el texto
                // Ajusta esto según el formato real de tus elementos del ListBox
                string claveInv = partes[0].Trim(); // Obtener la clave y eliminar espacios en blanco

                // Agregar la clave a la lista de claves
                clavesInvestigadores.Add(claveInv);

                // Depuración: Imprimir la clave para verificar si se está obteniendo correctamente
                //Debug.WriteLine("Clave del investigador: " + claveInv);
            }

            string comentarios = comments.Value;
            DateTime fechaActual = DateTime.Today;


            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(titulo) 
                || string.IsNullOrWhiteSpace(nivelProyecto) || string.IsNullOrWhiteSpace(fechaInicio) 
                || string.IsNullOrWhiteSpace(fechaFin) || string.IsNullOrWhiteSpace(lugarAplicacion) 
                || string.IsNullOrWhiteSpace(lugar) || string.IsNullOrWhiteSpace(fechaAp))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            string query = "INSERT INTO Proyecto (cve_proyecto, titulo_proyecto, nivel, grado_posgrado, red, fecha_inicio, fecha_fin, CA, difusion, comentarios, fecha, nombre_ca) VALUES (@Cve_proyecto, @Titulo, @Nivel, @GradoPosgrado, @Red, @FechaInicio, @FechaFin, @CA, @Difusion, @Comentarios, @Fecha, @NombreCA)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@Cve_proyecto", SqlDbType.VarChar).Value = clave;
                cmd.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = titulo;
                cmd.Parameters.Add("@Nivel", SqlDbType.VarChar).Value = nivelProyecto;
                cmd.Parameters.Add("@GradoPosgrado", SqlDbType.VarChar).Value = nivelPosgrado;
                cmd.Parameters.Add("@Red", SqlDbType.VarChar).Value = proyectoRed;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = fechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = fechaFin;
                cmd.Parameters.Add("@CA", SqlDbType.VarChar).Value = nombreCA;
                cmd.Parameters.Add("@Difusion", SqlDbType.VarChar).Value = nombreMedio;
                cmd.Parameters.Add("@Comentarios", SqlDbType.VarChar).Value = comentarios;
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fechaActual;
                cmd.Parameters.Add("@NombreCA", SqlDbType.VarChar).Value = nombreInvCA;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblSuccessMessage.Text = "Proyecto agregado satisfactoriamente.";

                    GuardarInvestigadores(clave, clavesInvestigadores);

                    // Insertar datos en la tabla Comite_Etica
                    string queryComiteE = "INSERT INTO Comite_Etica (numero, nombre, cve_proyecto) VALUES (@Numero, @Nombre, @CveProyecto)";
                    using (SqlCommand cmdComiteE = new SqlCommand(queryComiteE, con))
                    {
                        cmdComiteE.Parameters.Add("@Numero", SqlDbType.Int).Value = Convert.ToInt32(numeroCE);
                        cmdComiteE.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = instCE;
                        cmdComiteE.Parameters.Add("@CveProyecto", SqlDbType.VarChar).Value = clave;

                        cmdComiteE.ExecuteNonQuery();
                    }

                    // Insertar datos en la tabla Lugar_Aplicacion
                    string queryLugar = "INSERT INTO Lugar_Aplicacion (tipo_lugar, lugar, fecha_aplicacion, cve_proyecto) VALUES (@TipoLugar, @Lugar, @FechaAplicacion, @CveProyecto)";
                    using (SqlCommand cmdLugar = new SqlCommand(queryLugar, con))
                    {
                        cmdLugar.Parameters.Add("@TipoLugar", SqlDbType.VarChar).Value = lugarAplicacion;
                        cmdLugar.Parameters.Add("@Lugar", SqlDbType.VarChar).Value = lugar;
                        cmdLugar.Parameters.Add("@FechaAplicacion", SqlDbType.Date).Value = fechaAp;
                        cmdLugar.Parameters.Add("@CveProyecto", SqlDbType.VarChar).Value = clave;

                        cmdLugar.ExecuteNonQuery();
                    }

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
            string nivelProyecto = string.Empty;

            if (enfermeria.Checked)
            {
                nivelProyecto = "Enfermería";
            }
            else if (nutricion.Checked)
            {
                nivelProyecto = "Nutrición";
            }
            else if (otro.Checked)
            {
                nivelProyecto = otroInput.Value;
            }

            string nivelPosgrado = string.Empty;

            if (especialidad.Checked)
            {
                nivelPosgrado = "Especialidad en " + especialidadInput.Value;
            }
            else if (master.Checked)
            {
                nivelPosgrado = "Maestría en " + masterInput.Value;
            }
            else if (doctor.Checked)
            {
                nivelPosgrado = "Doctorado en " + doctorInput.Value;
            }
            else if (no.Checked)
            {
                nivelPosgrado = "no";
            }

            string proyectoRed = string.Empty;

            if (redNo.Checked)
            {
                proyectoRed = "no";
            }
            else if (nombreRed.Checked)
            {
                proyectoRed = redInput.Value;
            }

            string fechaInicio = tbfinicio.Text;
            string fechaFin = tbffin.Text;

            string registroCE = string.Empty;
            string numeroCE = string.Empty;
            string instCE = string.Empty;
            string otroCE = string.Empty;

            if (siCE.Checked)
            {
                // Si la opción "Si" está seleccionada, recuperamos los valores de los campos de texto
                registroCE = "si";
                numeroCE = numeroInput.Value; // Número
                instCE = instInput.Value;     // Nombre de la institución que otorga
                otroCE = instOInput.Value;    // Otro
            }
            else if (noCE.Checked)
            {
                registroCE = "no";
            }

            string lugarAplicacion = string.Empty;

            if (interno.Checked)
            {
                lugarAplicacion = "Interno";
            }
            else if (externo.Checked)
            {
                lugarAplicacion = "Externo";
            }

            string lugar = lugarInput.Value; // Obtener el valor del campo de texto "Lugar"
            string fechaAp = tbfechaAp.Text;

            string perteneceInvCA = string.Empty;
            string nombreInvCA = string.Empty;
            if (noInv.Checked)
            {
                perteneceInvCA = "no";
            }
            else if (siInv.Checked)
            {
                perteneceInvCA = "si";
                nombreInvCA = invCAInput.Value; // Obtener el valor del campo de texto "Nombre" si la opción "Si" está seleccionada
            }

            string perteneceCA = string.Empty;
            string nombreCA = string.Empty;
            if (caNo.Checked)
            {
                perteneceCA = "no";
                nombreCA = "no";
            }
            else if (caSi.Checked)
            {
                nombreCA = caInput.Value; // Obtener el valor del campo de texto "Nombre" si la opción "Nombre" está seleccionada
            }

            string difundiraFueraFacultad = string.Empty;
            string nombreMedio = string.Empty;

            if (mediosNo.Checked)
            {
                difundiraFueraFacultad = null;
                nombreMedio = "no";
            }
            else if (mediosSi.Checked)
            {
                nombreMedio = medioInput.Value; // Obtener el valor del campo de texto "Medio" si la opción "Nombre de Medio" está seleccionada
            }

            string comentarios = comments.Value;
            DateTime fechaActual = DateTime.Today;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(titulo)
                || string.IsNullOrWhiteSpace(nivelProyecto) || string.IsNullOrWhiteSpace(fechaInicio)
                || string.IsNullOrWhiteSpace(fechaFin) || string.IsNullOrWhiteSpace(lugarAplicacion)
                || string.IsNullOrWhiteSpace(lugar) || string.IsNullOrWhiteSpace(fechaAp))
            {
                lblErrorMessage.Text = "Todos los campos son obligatorios.";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorDiv", "showErrorDiv();", true);
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Proyecto SET titulo_proyecto = @Titulo, nivel = @Nivel, grado_posgrado = @GradoPosgrado, red = @Red, fecha_inicio = @FechaInicio, fecha_fin = @FechaFin, CA = @CA, difusion = @Difusion, comentarios = @Comentarios WHERE cve_proyecto = @Cve_proyecto", con);
                con.Open();
                cmd.Parameters.Add("@Cve_proyecto", SqlDbType.VarChar).Value = clave;
                cmd.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = titulo;
                cmd.Parameters.Add("@Nivel", SqlDbType.VarChar).Value = nivelProyecto;
                cmd.Parameters.Add("@GradoPosgrado", SqlDbType.VarChar).Value = nivelPosgrado;
                cmd.Parameters.Add("@Red", SqlDbType.VarChar).Value = redInput;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = Convert.ToDateTime(fechaInicio).Date;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = Convert.ToDateTime(fechaFin).Date;
                cmd.Parameters.Add("@CA", SqlDbType.VarChar).Value = nombreCA;
                cmd.Parameters.Add("@Difusion", SqlDbType.VarChar).Value = nombreMedio;
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

        [WebMethod]
        public static object BuscarInvestigadores(string busqueda)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
            {
                string consulta = "SELECT cve_inv, nombre_investigador, ap_pat, ap_mat FROM Investigador WHERE cve_inv LIKE @busqueda OR nombre_investigador LIKE @busqueda OR ap_pat LIKE @busqueda OR ap_mat LIKE @busqueda";
                using (SqlCommand cmd = new SqlCommand(consulta, con))
                {
                    cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        private void GuardarInvestigadores(string claveProyecto, List<string> clavesInvestigadores)
        {
            string queryInsertPI = "INSERT INTO Proyecto_Investigador (ID_cargo, cve_proyecto, cve_inv, cargo) VALUES (@ID_cargo, @cve_proyecto, @cve_inv, @cargo)";
            using (SqlCommand cmdInsertPI = new SqlCommand(queryInsertPI, con))
            {
                foreach (string claveInvestigador in clavesInvestigadores)
                {
                    string ID_cargo = Guid.NewGuid().ToString();

                    cmdInsertPI.Parameters.Clear();
                    cmdInsertPI.Parameters.AddWithValue("@ID_cargo", ID_cargo);
                    cmdInsertPI.Parameters.AddWithValue("@cve_proyecto", claveProyecto);
                    cmdInsertPI.Parameters.AddWithValue("@cve_inv", claveInvestigador);
                    cmdInsertPI.Parameters.AddWithValue("@cargo", "responsable"); // El cargo es siempre "responsable"

                    cmdInsertPI.ExecuteNonQuery();
                }
            }
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
            //tbprotocolo.Text = "";
            //tbalcance.Text = "";
            //tbarea.Text = "";
            //tbfinicio.Text = "";
            //tbffin.Text = "";
            //tbregetica.Text = "";
            //tblugar.Text = "";
            //tbca.Text = "";
            //tbfin.Text = "";
            //tbgradpost.Text = "";
            //tbcomentarios.Text = "";
        }


    }
}