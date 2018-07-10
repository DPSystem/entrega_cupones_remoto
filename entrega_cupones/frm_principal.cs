using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LinqToExcel;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
//root@localhost:3306  jdbc:mysql://localhost:3306/?user=root


namespace entrega_cupones
{
    public partial class Frm_principal : Form
    {
        lts_sindicatoDataContext db_socios = new lts_sindicatoDataContext();
        //lts_sindicatoDataContext db_socios_1 = new lts_sindicatoDataContext();
        
        public Frm_principal()
        {
            InitializeComponent();
        }

        private void Btn_entrega_cupones_Click(object sender, EventArgs e)
        {
            
            Frm_socios f_socios = new Frm_socios();
            f_socios.ShowDialog();
        }

        private void Btn_mochilas_Click(object sender, EventArgs e)
        {
            
        }

        private void Btn_menu_Click(object sender, EventArgs e)
        {
            if (pnl_menu.Width == 45)
            {
                pnl_menu.Visible =false;
                pnl_menu.Width = 138;
                Btn_menu.Location = new Point(106, 3);
                pnl_menu_transition.ShowSync(pnl_menu);

            }
            else
            {
                pnl_menu.Visible = false;
                pnl_menu.Width = 45;
                Btn_menu.Location = new Point(12, 3);
                pnl_menu_transition.ShowSync(pnl_menu);

            }
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void btn_mochila_Click(object sender, EventArgs e)
        {
            frm_mochilas f_mochilas = new frm_mochilas();
            f_mochilas.Show();
        }

        private void lbl_1_Click(object sender, EventArgs e)
        {

        }

        private void btn_quinchos_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.ShowDialog();
            //if (ofd.FileName.Equals("") == false)
            //{
            //    picbox_socio.Load(ofd.FileName);
            //}
            frm_quinchos f_quinchos = new frm_quinchos();
            f_quinchos.lbl_cuil.Text = lbl_cuil.Text;
            f_quinchos.lbl_socio.Text = lbl_nombre.Text;

            var var_buscar_foto = db_socios.fotos.Where(x => x.FOTOS_CUIL == Convert.ToDouble(lbl_cuil.Text) && x.FOTOS_CODFLIAR == 0);
            if (var_buscar_foto.Count() > 0)
            {
                f_quinchos.picbox_socio_referente.Image = ByteArrayToImage(var_buscar_foto.Single().FOTOS_FOTO.ToArray());
                f_quinchos.btn_sin_foto.Visible = false;
            }
            else
            {
                f_quinchos.picbox_socio_referente.Image = null;
                f_quinchos.btn_sin_foto.Visible = true;
            }
            f_quinchos.Show();
            
        }

        private void btn_pedicuro_Click(object sender, EventArgs e)
        {
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
           
            //picbox_socio.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //fotos1 fts = new fotos1();
            //fts.foto = ms.GetBuffer();
            //db_socios.fotos1.InsertOnSubmit(fts);
            //db_socios.SubmitChanges();
        }

        private void btn_masajista_Click(object sender, EventArgs e)
        {
            //var benef = from a in db_socios.fotos1
            //            where a.id_foto >= 4
            //            select new
            //            {
            //                //image.GetThumbnailImage(20/*width*/, 40/*height*/, null, IntPtr.Zero))
            //                //var img = ByteArrayToImage(a.foto.ToArray())
            //                //using(var thumbnail = image.GetThumbnailImage(20/*width*/, 40/*height*/, null, IntPtr.Zero))
            //                foto =  (ByteArrayToImage(a.foto.ToArray())).GetThumbnailImage(70,70,null,IntPtr.Zero)
            //                //foto = ByteArrayToImage(a.foto.ToArray()) 

            //            };
            //dgv_familiar_a_cargo.DataSource = benef;
            //var benf = db_socios.fotos1.Where(x => x.id_foto == 4).Single();

            //picbox_socio.Image = ByteArrayToImage(benf.foto.ToArray()); //Image.FromFile(benf.foto);

            ////ByteArrayToImage(img.alumno_foto.ToArray());
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void Frm_principal_Load(object sender, EventArgs e)
        {

 
            cbx_buscar_por.SelectedIndex = 0;
            cbx_filtrar.SelectedIndex = 1;
            dgv_mostrar_socios.AutoGenerateColumns = false;
            dgv_mostrar_socios.Sort(dgv_mostrar_socios.Columns[0], ListSortDirection.Ascending);
        }

        private void btn_asesoria_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.FileName.Equals("") == false)
            {

                //ruta del archivo
                //string ruta_excel = "E:\\SEC\\Diego\\Socios.xlsx";// Application.StartupPath + "\\Socios.xlsx";
                                                                  // creamos libro a partir de la ruta
                var libro = new ExcelQueryFactory(ofd.FileName);
                // Consulta con linq
                var res = from row in libro.Worksheet("inicio")
                          select new
                          {
                              cuil = row[0].Value,
                              apenom = row[1].Value,
                              nro_afil = row[2].Value
                              //provincia = row[4].Value
                          };
                dgv_excell.DataSource = res.ToList();
            }
        }

        private void btn_colonia_Click(object sender, EventArgs e)
        {
            ////lts_sociosDataContext empleados_sql = new lts_sociosDataContext();
            //baserDataSet baser = new baserDataSet();
            //var empleados_access = baser.empleados;
            //foreach (var item in empleados_access)
            //{
            //    socios empleados_sql = db_socios.socios.Where(x => x.CUIL.Equals(item.cuil)).Single();
            //    if ( empleados_sql != null)
            //    {

            //    }

            //}

            //db_socios.limpiar_tabla_socios();


        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btn_actualizar_Click(object sender, EventArgs e)
        {
            //Limpio la tabla de Socios
            db_socios.limpiar_tabla_socios();
            //string conexion = "Provider=Microsoft.ACE.OleDB.12.0;Data Source = E:\\SEC\\importacion de BD\\nueva carpeta\\socios.xlsx;Extended Properties=Excel 12.0 Xml; HDR=Yes";
            string conexion = "Provider=Microsoft.ACE.OleDB.12.0;Data Source = E:\\SEC\\importacion de BD\\nueva carpeta\\socios.xlsx;Extended Properties=\"Excel 12.0; HDR=Yes\"";
            OleDbConnection origen = default(OleDbConnection);
            origen = new OleDbConnection(conexion);
            origen.Open();

            OleDbCommand seleccion = default(OleDbCommand);
            seleccion = new OleDbCommand("Select * From [socios$]", origen);

            OleDbDataAdapter adaptador = new OleDbDataAdapter();
            adaptador.SelectCommand = seleccion;

            DataSet ds = new DataSet();

            adaptador.Fill(ds);
            dgv_excell.DataSource = ds.Tables[0];

            origen.Close();

            SqlConnection conexion_destino = new SqlConnection();
            conexion_destino.ConnectionString = "Data Source = DROP\\DPS_17; Initial Catalog = sindicato; Integrated Security = True";
            //"                                       Data Source = sindicato; Initial Catalog=sindicato; Integrated Security = True";

            conexion_destino.Open();

            SqlBulkCopy importar = default(SqlBulkCopy);

            importar = new SqlBulkCopy(conexion_destino);
            importar.DestinationTableName = "socios";
            importar.WriteToServer(ds.Tables[0]);
            conexion_destino.Close();
            lbl_total_socios.Text = lbl_total_socios.Text + " " + Convert.ToString(ds.Tables[0].Rows.Count);
            
        }

        private void Dgv_socios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Txt_dni_OnValueChanged(object sender, EventArgs e)
        {
            //buscar_socios(Txt_dni.Text.Trim());
        }

        private void buscar_socios(string dato)
        {

            if (dato == "") // si el parametro "dato" a buscar no contiene nada entonces se muestran todos los socios de acuerdo al filtro
            {
                buscar_en_blanco();
            }
            else
            {
                if (cbx_buscar_por.SelectedItem.ToString() == "D.N.I.") // si esta chequeado buscar por DNI
                {
                    // llamo a la funciona buscar_por_dni y le paso el parametro dato para que aplique el filtro
                    buscar_por_dni(dato);
                }
                if (cbx_buscar_por.SelectedItem.ToString() == "Apellido y Nombre") // si esta chequeado buscar por apellido y nombre
                {
                    // llamo a la funcion buscar_por_apeynom y le paso el parametro dato para que aplique el filtro
                    buscar_por_apeynom(dato);
                }
                if (cbx_buscar_por.SelectedItem.ToString() == "Empresa") // si esta chequeado buscar por empresa
                {
                    // llamo a la funcion buscar_por_empresa y le paso el parametro dato para que aplique el filtro
                    buscar_por_empresa(dato);
                }
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            buscar_socios(Txt_dni.Text);
        }

        private void Txt_dni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buscar_socios(Txt_dni.Text);
            }
        }

        private void cbx_filtrar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buscar_por_dni(string dato)
        {
            string estado = verificar_filtro();
            if (estado == "Todos")
            {
                var socio = (from a in db_socios.soccen 
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL where sc.MAESOC_NRODOC == dato
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons,
                                 empresa_cuit = empr.cuit,
                                 //sexo= sc.MAESOC_SEXO,
                                 //domi = sc.MAESOC_BARRIO,
                                 //localidad = sc.MAESOC_CODLOC,
                                 //cp = sc.MAESOC_CODPOS,
                                 //estado_civil = sc.MAESOC_ESTCIV,
                                 //telefono = sc.MAESOC_TEL

                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());

                
            }
            else
            {
                var socio = (from a in db_socios.soccen where a.SOCCEN_ESTADO == Convert.ToByte(estado)
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL where sc.MAESOC_NRODOC == dato
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
        }

        private void buscar_por_apeynom(string dato)
        {

            string estado = verificar_filtro();
            if (estado == "Todos")
            {
                var socio = (from a in db_socios.soccen
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL
                             where sc.MAESOC_APELLIDO.Contains(dato) || sc.MAESOC_NOMBRE.Contains(dato) 
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
            else
            {
                var socio = (from a in db_socios.soccen where a.SOCCEN_ESTADO == Convert.ToByte(estado)
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL
                             where sc.MAESOC_APELLIDO.Contains(dato)
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL
                             where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
        }

        private void buscar_por_empresa(string dato)
        {
            string estado = verificar_filtro();
            if (estado == "Todos")
            {
               
                var socio = (from a in db_socios.soccen
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL
                             where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             where empr.razons.Contains(dato)
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
            else
            {
                
                var socio = (from a in db_socios.soccen
                             where a.SOCCEN_ESTADO == Convert.ToByte(estado)
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL
                             where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             where empr.razons.Contains(dato)
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
        }

        private void buscar_en_blanco()
        {
            string estado = verificar_filtro();
            if (estado == "Todos")
            {
                 var socio = (from a in db_socios.soccen
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL
                             where se.SOCEMP_ULT_EMPRESA == 'S' 
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
            else
            {
                var socio = (from a in db_socios.soccen
                             where a.SOCCEN_ESTADO == Convert.ToInt32(estado)
                             join sc in db_socios.maesoc on a.SOCCEN_CUIL equals sc.MAESOC_CUIL
                             join se in db_socios.socemp on a.SOCCEN_CUIL equals se.SOCEMP_CUIL where se.SOCEMP_ULT_EMPRESA == 'S'
                             join empr in db_socios.empresas on se.SOCEMP_CUITE equals empr.cuit
                             
                             select new
                             {
                                 numero_socio = sc.MAESOC_NROAFIL.Trim(),
                                 dni_socio = sc.MAESOC_NRODOC.Trim(),
                                 apeynom = sc.MAESOC_APELLIDO.Trim() + " " + sc.MAESOC_NOMBRE.Trim(),
                                 empresa = empr.razons
                             }).OrderBy(x => x.apeynom);
                dgv_mostrar_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
              
            }
        }
        
        private string verificar_filtro()
        {
            string estado = "";

            if (cbx_filtrar.SelectedItem.ToString() == "Socios")
            {
                estado = "1";
            }
            if (cbx_filtrar.SelectedItem.ToString() == "NO Socios")
            {
                estado = "0";
            }
            if (cbx_filtrar.SelectedItem.ToString() == "Todos")
            {
                estado = "Todos";
            }

            return estado;
        }

       

        private void mostrar_benef(string v)
        {
            double cuil_titular = db_socios.maesoc.Where(x => x.MAESOC_NRODOC == v).FirstOrDefault().MAESOC_CUIL;
            var var_beneficiario = from sf in db_socios.socflia
                                   where sf.SOCFLIA_CUIL == cuil_titular
                                   join ms in db_socios.maeflia on sf.SOCFLIA_CODFLIAR equals ms.MAEFLIA_CODFLIAR
                                   select new
                                   {
                                        apeynom = ms.MAEFLIA_APELLIDO.Trim() + " " + ms.MAEFLIA_NOMBRE.Trim(),
                                        parentesco = (sf.SOCFLIA_PARENT == 1) ? "CONYUGE" :
                                                        (sf.SOCFLIA_PARENT == 2) ? "HIJO MENOR DE 16" :
                                                        (sf.SOCFLIA_PARENT == 3) ? "HIJO MENOR DE 18" :
                                                        (sf.SOCFLIA_PARENT == 4) ? "HIJO MENOR DE 21" :
                                                        (sf.SOCFLIA_PARENT == 5) ? "HIJO MAYOR DE 21" : "",
                                        codigo_fliar = sf.SOCFLIA_CODFLIAR,
                                   };
            dgv_mostrar_beneficiario.DataSource = var_beneficiario.ToList();
            if (dgv_mostrar_beneficiario.Rows.Count == 0)
            {
                picbox_beneficiario.Image = null;
                btn_sin_imagen_benef.Visible = true;
            }                      
        }

        private void mostrar_benef_datos (long codigo_benef)
        {
            var var_dato_benef = from benef in db_socios.maeflia where benef.MAEFLIA_CODFLIAR == codigo_benef
                                   select new
                                   {
                                       codigo_fliar = benef.MAEFLIA_CODFLIAR,
                                       apellido = benef.MAEFLIA_APELLIDO.Trim(),
                                       nombre = benef.MAEFLIA_NOMBRE.Trim(),
                                       dni = benef.MAEFLIA_NRODOC,
                                       fnac = benef.MAEFLIA_FECNAC,
                                       fingreso = benef.MAEFLIA_FECING,
                                       sexo = benef.MAEFLIA_SEXO,
                                       edad = calcular_edad(benef.MAEFLIA_FECNAC),
                                       estado_civil = benef.MAEFLIA_ESTCIVIL,
                                       //estado
                                       estudia = benef.MAEFLIA_ESTUDIA,
                                       discapacitado = benef.MAEFLIA_DISCAPAC,
                                   };
            if (var_dato_benef.Count() > 0 )
            {
                txt_benef_ape.Text = var_dato_benef.Single().apellido;
                txt_benef_nombre.Text = var_dato_benef.Single().nombre;
                txt_benef_dni.Text = var_dato_benef.Single().dni.ToString();
                // si la fecha de nacimiento no es correcta entocnces muestro null
                if ((((DateTime.MinValue > var_dato_benef.Single().fnac) && (var_dato_benef.Single().fnac < DateTime.MaxValue)) || (var_dato_benef.Single().fnac == null)))
                {
                    dtp_fnac.Value = var_dato_benef.Single().fnac;
                }
               // dtp_fingreso.Value = var_dato_benef.Single().fingreso;
                cbx_benef_sexo.Text = var_dato_benef.Single().sexo.ToString();
                cbx_benef_estado_civil.Text = var_dato_benef.Single().estado_civil.ToString();
                //cbx_benef_parentesco.Text = dgv_mostrar_beneficiario.CurrentRow.Cells[1].Value.ToString();
                // swt_benef_activo.Value = if (var_dato_benef.Single().a true)
                //swt_benef_estudia.Value = var_dato_benef.Single().estudia == 0 ? true : false;  
                if (var_dato_benef.Single().estudia == 0 )
                {
                    swt_benef_estudia.Value = true;
                }
                else
                {
                    swt_benef_estudia.Value = false;
                }
                //swt_benef_activo.Value = db_socios.soccen.Where(x=>x.SOCCEN_CUIL == )
                swt_benef_discapa.Value = (var_dato_benef.Single().discapacitado == 0) ? true : false;

            }
        }

        private void mostrar_historial_servicios(string dni)
        {
            var serv = (from servi in db_socios.recibos.Where(x => x.dni.Equals(dni))
                       select new
                       {
                           serv_fecha =  servi.PERIODO,   //servi.PERIODO.Value.Day + "/"+ servi.PERIODO.Value.Year,
                           serv_descrip = servi.CONCEPTO
                       }).OrderBy(x=>x.serv_fecha);
            dgv_historial_servicios.DataSource = serv.ToList();
            if (dgv_historial_servicios.Rows.Count >= 1) // me posiciono en la ultima fila del dgv_aportes
            {
                dgv_historial_servicios.CurrentCell = dgv_historial_servicios.Rows[dgv_historial_servicios.Rows.Count - 1].Cells[0];
            }
        }

        private void mostrar_datos_socio(string dni)
        {
            var datos_socio = db_socios.maesoc.Where(x => x.MAESOC_NRODOC == dni).First();    //db_socios.socios.Where(x => x.DNI.Equals(dni)).First();

            var var_buscar_foto = db_socios.fotos.Where(x => x.FOTOS_CUIL == datos_socio.MAESOC_CUIL && x.FOTOS_CODFLIAR == 0);
            if (var_buscar_foto.Count() > 0)
            {
                picbox_socio.Image = ByteArrayToImage(var_buscar_foto.Single().FOTOS_FOTO.ToArray());
                btn_sin_imagen.Visible = false;
            }
            else
            {
                picbox_socio.Image = null;
                btn_sin_imagen.Visible = true;
            }

            lbl_nro_socio.Text = datos_socio.MAESOC_NROAFIL.ToString();
            lbl_nombre.Text = datos_socio.MAESOC_APELLIDO.Trim() + " " + datos_socio.MAESOC_NOMBRE.Trim();
            lbl_dni.Text = datos_socio.MAESOC_NRODOC;
            lbl_cuil.Text = datos_socio.MAESOC_CUIL.ToString();
            lbl_sexo.Text = datos_socio.MAESOC_SEXO.ToString();
            lbl_empresa.Text = dgv_mostrar_socios.CurrentRow.Cells[3].Value.ToString();
            lbl_domicilio.Text = datos_socio.MAESOC_BARRIO + " " + datos_socio.MAESOC_CALLE.Trim() + " " + datos_socio.MAESOC_NROCALLE ;
            lbl_localidad.Text = datos_socio.MAESOC_CODLOC.ToString();
            lbl_codigo_postal.Text = datos_socio.MAESOC_CODPOS;
            lbl_edad.Text =  calcular_edad(datos_socio.MAESOC_FECHANAC).ToString();
            lbl_telefono.Text = datos_socio.MAESOC_TELCEL;
            lbl_estado_civil.Text = datos_socio.MAESOC_ESTCIV.ToString();
            var es_socio = db_socios.soccen.Where(x => x.SOCCEN_CUIL == datos_socio.MAESOC_CUIL).Single();
            if (es_socio.SOCCEN_ESTADO == 1)
            {
                if (datos_socio.MAESOC_NROAFIL.Trim() == "")
                {
                    lbl_estado_socio.Text = "PASIVO";
                }
                else
                {
                    lbl_estado_socio.Text = "ACTIVO";
                }
            }
            else
            {
                lbl_estado_socio.Text = "NO SOCIO";
            }
        }

        private void mostrar_ddjj(Int64 cuil, Int64 cuite) // Calculo los aportes del socio segun la declaracion jurada de la empresa
        {

            var ddjj = (from dj in db_socios.ddjj
                        where dj.cuil == cuil
                        select new
                        {
                            periodo = dj.periodo,
                            aporte_ley = (dj.impo + dj.impoaux) * 0.02,
                            aporte_socios = (dj.item2 == true) ? (dj.impo + dj.impoaux) * 0.02 : 0,
                            cuit = dj.cuite
                            //aporte_pagado = (db_socios.ddjjt.Where(x => x.periodo == dj.periodo).Max(x => x.rect))// (djt.fpago.Value == null) ? "NO":"SI"
                       }).OrderBy(x=>x.periodo);
            dgv_mostrar_aportes.DataSource = ddjj.ToList();

             if (dgv_mostrar_aportes.Rows.Count >= 1) // me posiciono en la ultima fila del dgv_aportes
            {
                dgv_mostrar_aportes.CurrentCell = dgv_mostrar_aportes.Rows[dgv_mostrar_aportes.Rows.Count -1].Cells[0];
                dgv_mostrar_aportes.Rows[dgv_mostrar_aportes.Rows.Count - 1].Selected = true;
            }
        }

        private int calcular_edad(DateTime fecha_nac)
        {
            int edad = 0;
            DateTime fecha_actual = DateTime.Today;
            edad = fecha_actual.Year - fecha_nac.Year;
            if ((fecha_actual.Month < fecha_nac.Month  ) || (fecha_actual.Month == fecha_nac.Month && fecha_actual.Day < fecha_nac.Day))
            {
                edad--;
            }
            return edad;
        }

        private void btn_inspectores_Click(object sender, EventArgs e)
        {
            frm_inspecciones f_inspecciones = new frm_inspecciones();
            f_inspecciones.ShowDialog();
        }

        private void mysql_conex()
        {
            MySqlConnectionStringBuilder builder = new  MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "root";
            builder.Database = "fotos_";
            MySqlConnection conn = new MySqlConnection(builder.ToString());
            //MySqlCommand cmd = conn.CreateCommand();

            conn.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            MySqlDataAdapter da = new MySqlDataAdapter();
            string sqlstr = "select fotos.FOTOS_FOTO, concat(maeflia.MAEFLIA_NRODOC, ' ', maeflia.MAEFLIA_APELLIDO) , maeflia.MAEFLIA_NOMBRE,socflia.SOCFLIA_PARENT from maesoc join socflia on socflia.SOCFLIA_CUIL = maesoc.maesoc_cuil join maeflia on maeflia.MAEFLIA_CODFLIAR = socflia.SOCFLIA_CODFLIAR left join fotos on fotos.FOTOS_CODFLIAR = maeflia.MAEFLIA_CODFLIAR where maesoc.maesoc_nrodoc = " +dgv_mostrar_socios.CurrentRow.Cells[1].Value.ToString();// 27269547761";
            da.SelectCommand = new MySqlCommand(sqlstr, conn);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            BindingSource bndg_source = new BindingSource();
            dgv_excell.DataSource = tabla;
            conn.Close();

            //cmd.ExecuteNonQuery();
        }

        private void Btn_mostrar_cupones_Click(object sender, EventArgs e)
        {
            mysql_conex();
        }

        private void btn_odontologo_Click(object sender, EventArgs e)
        {
            Frm_socios f_socios = new Frm_socios();
            f_socios.Show();
        }

        private void dgv_mostrar_socios_SelectionChanged(object sender, EventArgs e)
        {
            mostrar_datos_socio(dgv_mostrar_socios.CurrentRow.Cells[1].Value.ToString());
            mostrar_benef(dgv_mostrar_socios.CurrentRow.Cells[1].Value.ToString());
            mostrar_historial_servicios(dgv_mostrar_socios.CurrentRow.Cells[1].Value.ToString());
            var cuil_s = db_socios.maesoc.Where(x => x.MAESOC_NRODOC == dgv_mostrar_socios.CurrentRow.Cells[1].Value.ToString());// .Equals(dni)).First();
            Int64 cuil_socio = 0;
            if (cuil_s.Count() > 0 )
            {
                cuil_socio = Convert.ToInt64(cuil_s.Where(x => x.MAESOC_NRODOC == dgv_mostrar_socios.CurrentRow.Cells[1].Value.ToString()).FirstOrDefault().MAESOC_CUIL);
            }
            var cuit_s = db_socios.socemp.Where(x => x.SOCEMP_CUIL == cuil_socio && x.SOCEMP_ULT_EMPRESA == 'S');
            Int64 cuit_empresa = 0;
            if (cuit_s.Count() > 0)
            {
                cuit_empresa = Convert.ToInt64(cuit_s.Where(x => x.SOCEMP_CUIL == cuil_socio && x.SOCEMP_ULT_EMPRESA == 'S').FirstOrDefault().SOCEMP_CUITE);
            } 
            mostrar_ddjj(cuil_socio, cuit_empresa);
        }

        private void dgv_mostrar_beneficiario_SelectionChanged(object sender, EventArgs e)
        {
            Int64 cod_f = Convert.ToInt64(dgv_mostrar_beneficiario.CurrentRow.Cells[2].Value.ToString());
            mostrar_foto_benef(cod_f);
            mostrar_benef_datos(cod_f);
        }

        private void mostrar_foto_benef(Int64 cod_fliar)
        {
            var var_buscar_foto = db_socios.fotos.Where(x => x.FOTOS_CODFLIAR == cod_fliar);//x.FOTOS_CUIL == datos_socio.MAESOC_CUIL && x.FOTOS_CODFLIAR == 0);
            if (var_buscar_foto.Count() > 0)
            {
                picbox_beneficiario.Image = ByteArrayToImage(var_buscar_foto.Single().FOTOS_FOTO.ToArray());
                btn_sin_imagen_benef.Visible = false;
            }
            else
            {
                picbox_beneficiario.Image = null;
                btn_sin_imagen_benef.Visible = true;
            }
        }

        private void dgv_mostrar_aportes_SelectionChanged(object sender, EventArgs e)
        {
           // var empresa = db_socios.maeemp.Where(x => x.MAEEMP_CUIT == Convert.ToInt64(dgv_mostrar_aportes.CurrentRow.Cells[3].Value.ToString()));
            //lbl_empresa_aporte.Text = empresa.Single().MAEEMP_RAZSOC.ToString();
            //lbl_cuit_aporte.Text = dgv_mostrar_aportes.CurrentRow.Cells[3].Value.ToString();
        }

        private void swt_discapa_OnValueChange(object sender, EventArgs e)
        {
            if (swt_benef_discapa.Value ==  true)
            {
                lbl_discapa.Text = "SI";
            }
            else
            {
                lbl_discapa.Text = "NO";
            }
        }

        private void swt_estudia_OnValueChange_1(object sender, EventArgs e)
        {
            if (swt_benef_estudia.Value == true)
                {
                    lbl_estudia.Text = "SI";
                }
            else
                {
                    lbl_estudia.Text = "NO";
                }
        }

        private void swt_activo_OnValueChange(object sender, EventArgs e)
        {
            if (swt_benef_activo.Value == true)
            {
                lbl_activo.Text = "SI";
            }
            else
            {
                lbl_activo.Text = "NO";
            }
        }

        private void dgv_mostrar_socios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_futbol_Click(object sender, EventArgs e)
        {
            frm_futbol f_futbol = new frm_futbol();
            f_futbol.Show();
        }
    }
}
