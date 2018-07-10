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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace entrega_cupones
{
   
    public partial class frm_mochilas : Form
       
    {
        //lts_scios_mysqlDataContext DB_soc_mysql = new lts_scios_mysqlDataContext();
        lts_sindicatoDataContext DB_soc_mysql = new lts_sindicatoDataContext();
        lts_sindicatoDataContext DB_socios = new lts_sindicatoDataContext();
        public frm_mochilas()
        {
            InitializeComponent();
        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            buscar_mochila_por_dni_linq(Txt_dni.Text);
        }

        private void buscar_mochila_por_dni_linq(string text)
        {
            if ( text.Length > 8)
            {
                text = text.Substring(2, 8);
            }

            var var_socio_1 = DB_soc_mysql.maesoc.Where(x => x.MAESOC_NRODOC == text); // para saber si se encontro  el socio
            
            
            //var var_socio = DB_soc_mysql.maesoc.Where(x => x.MAESOC_NRODOC == text).Single();
            
            if (var_socio_1.Count() > 0) // para saber si se encontro  el socio
            {
                var var_socio = DB_soc_mysql.maesoc.Where(x => x.MAESOC_NRODOC == text).Single(); // Quiere decir que si se encontro
                var var_socio_centro = DB_soc_mysql.soccen.Where(x => x.SOCCEN_CUIL == var_socio.MAESOC_CUIL); // && x.SOCCEN_ESTADO == 1);

                if (var_socio_centro.Count() != 0)
                {
                    txt_ape_nom.Text = var_socio.MAESOC_APELLIDO.Trim() + " " + var_socio.MAESOC_NOMBRE.Trim();
                    txt_nro_socio.Text = var_socio.MAESOC_NROAFIL;
                    txt_dni_socio.Text = var_socio.MAESOC_NRODOC;
                    var cuit = DB_soc_mysql.socemp.Where(x => x.SOCEMP_CUIL == var_socio.MAESOC_CUIL && x.SOCEMP_ULT_EMPRESA.Equals("S")).First();
                    var emp = DB_soc_mysql.maeemp.Where(x => x.MAEEMP_CUIT == cuit.SOCEMP_CUITE).SingleOrDefault();
                    txt_empresa.Text = emp.MAEEMP_RAZSOC;
                    var var_foto_1 = DB_soc_mysql.fotos.Where(x => x.FOTOS_CUIL == var_socio.MAESOC_CUIL && x.FOTOS_CODFLIAR == 0);
                    if (var_foto_1.Count() > 0 )
                    {
                        var var_foto = DB_soc_mysql.fotos.Where(x => x.FOTOS_CUIL == var_socio.MAESOC_CUIL && x.FOTOS_CODFLIAR == 0).Single();
                        picbox_socio.Image = ByteArrayToImage(var_foto.FOTOS_FOTO.ToArray());
                        btn_sin_imagen.Visible = false;
                    }
                    else
                    {
                        picbox_socio.Image = null;
                        btn_sin_imagen.Visible = true;
                    }
                    var var_benef = (from a in DB_soc_mysql.socflia
                                     join f in DB_soc_mysql.fotos on a.SOCFLIA_CODFLIAR equals f.FOTOS_CODFLIAR into fo //aqui se aplica el left join de sql
                                     from f in fo.DefaultIfEmpty()
                                     where a.SOCFLIA_CUIL == var_socio.MAESOC_CUIL
                                     select new
                                     {
                                         foto_benef = (f.FOTOS_FOTO != null) ? (ByteArrayToImage(f.FOTOS_FOTO.ToArray())).GetThumbnailImage(70, 70, null, IntPtr.Zero) : null,
                                         apeynom = (DB_soc_mysql.maeflia.Where(x => x.MAEFLIA_CODFLIAR == a.SOCFLIA_CODFLIAR)).Single().MAEFLIA_APELLIDO.Trim() +
                                         " " + (DB_soc_mysql.maeflia.Where(x => x.MAEFLIA_CODFLIAR == a.SOCFLIA_CODFLIAR)).Single().MAEFLIA_NOMBRE.Trim(),
                                         dni = DB_soc_mysql.maeflia.Where(x => x.MAEFLIA_CODFLIAR == a.SOCFLIA_CODFLIAR).Single().MAEFLIA_NRODOC,
                                         edad = calcular_edad(DB_soc_mysql.maeflia.Where(x => x.MAEFLIA_CODFLIAR == a.SOCFLIA_CODFLIAR).Single().MAEFLIA_FECNAC),
                                         codigo_fliar = a.SOCFLIA_CODFLIAR,
                                     }).OrderBy(x=>x.apeynom);
                    dgv_familiar_a_cargo_mochilas.DataSource = var_benef.ToList();
                    controlar_mochilas_entregadas();
                    //sugerir_mochila();
                }
            }
        }

        private void buscar_mochila_por_dni(string dni)
        {
            
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
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
                //string sqlstr = "select fotos.FOTOS_FOTO, maeflia.MAEFLIA_NRODOC, maeflia.MAEFLIA_APELLIDO,  maeflia.MAEFLIA_NOMBRE,socflia.SOCFLIA_PARENT from maesoc join socflia on socflia.SOCFLIA_CUIL = maesoc.maesoc_cuil join maeflia on maeflia.MAEFLIA_CODFLIAR = socflia.SOCFLIA_CODFLIAR left join fotos on fotos.FOTOS_CODFLIAR = maeflia.MAEFLIA_CODFLIAR where maesoc.maesoc_nrodoc = " + Txt_dni.Text;
                string sqlstr = "select fotos.FOTOS_FOTO, maeflia.MAEFLIA_NRODOC, maeflia.MAEFLIA_APELLIDO,  maeflia.MAEFLIA_NOMBRE,socflia.SOCFLIA_PARENT from maesoc join socflia on socflia.SOCFLIA_CUIL = maesoc.maesoc_cuil join maeflia on maeflia.MAEFLIA_CODFLIAR = socflia.SOCFLIA_CODFLIAR left join fotos on fotos.FOTOS_CODFLIAR = maeflia.MAEFLIA_CODFLIAR where maesoc.maesoc_nrodoc = " + Txt_dni.Text; 
                //"Select * from fotos where fotos_cuil = " + Dgv_socios.CurrentRow.Cells[1].Value.ToString(); //limit 1000 ";
                da.SelectCommand = new MySqlCommand(sqlstr, conn);
                DataTable tabla = new DataTable();
                da.Fill(tabla);
                BindingSource bndg_source = new BindingSource();
                dgv_familiar_a_cargo_mochilas.DataSource = tabla;
                conn.Close();
            
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

        private void Txt_dni_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buscar_mochila_por_dni_linq(Txt_dni.Text);
            }
        }

        private void btn_salir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Txt_dni_OnValueChanged(object sender, EventArgs e)
        {

        }

        private int calcular_edad(DateTime fecha_nac)
        {
            int edad = 0;
            DateTime fecha_actual = DateTime.Today;
            edad = fecha_actual.Year - fecha_nac.Year;
            if ((fecha_actual.Month < fecha_nac.Month) || (fecha_actual.Month == fecha_nac.Month && fecha_actual.Day < fecha_nac.Day))
            {
                edad--;
            }
            return edad;
        }

        private void sugerir_mochila()
        {
           
            foreach (DataGridViewRow fila in dgv_familiar_a_cargo_mochilas.Rows)
            {

                var var_docu_recibida = from doc in DB_socios.entregas_mochilas
                                        where doc.cod_fliar == Convert.ToInt32(fila.Cells["codigo_familiar"].Value.ToString())
                                        select new
                                        {
                                            cod_fliar = doc.cod_fliar,
                                            recibe_doc = doc.recibe_docum,
                                            fecha_recep = doc.fecha_recepcion_docum
                                        };
                
                    // DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt32(fila.Cells["codigo_familiar"].Value.ToString())).First();
                if (var_docu_recibida.Count() > 0)
                {
                        fila.Cells["documentacion"].Value = true;
                        fila.Cells["documentacion"].ReadOnly = true;
                        fila.Cells["doc_recibida"].Value = "SI";
                }               
                if (Convert.ToInt16(fila.Cells["edad"].Value.ToString()) >= 2 && Convert.ToInt32(fila.Cells["edad"].Value.ToString()) <= 5)
                {
                    fila.Cells["mochila"].Value = "1 - Salita de 3 y Jardin";
                }

                if (Convert.ToInt16(fila.Cells["edad"].Value.ToString()) >= 6 && Convert.ToInt16(fila.Cells["edad"].Value.ToString()) <= 7)
                {
                    fila.Cells["mochila"].Value = "2 - Primaria I (1º y 2º Grado)";
                }

                if (Convert.ToInt16(fila.Cells["edad"].Value.ToString()) >= 8 && Convert.ToInt16(fila.Cells["edad"].Value.ToString()) <= 11)
                {
                    fila.Cells["mochila"].Value = "3 - Primaria II (3º y 6º Grado)";
                }

                if (Convert.ToInt16(fila.Cells["edad"].Value.ToString()) >=12 && Convert.ToInt16(fila.Cells["edad"].Value.ToString()) <= 19)
                {
                    fila.Cells["mochila"].Value = "4 - Secundaria (7º en adelante)";
                }

                if (Convert.ToInt16(fila.Cells["edad"].Value.ToString()) >= 20 )
                {
                    fila.Cells["mochila"].Value = "5 - No Corresponde";
                    fila.Cells["documentacion"].ReadOnly = true;
                    fila.Cells["entregar_mochila"].ReadOnly = true;
                }

                //1 - Salita de 3 y Jardin
                //2 - Primaria I(1º y 2º Grado)
                //3 - Primaria II(3º y 6º Grado)
                //4 - Secundaria(7º en adelante)
                //5 - No Corresponde
            }
        }

        private void cargar_documentacion_recibida()
        {
            
        }

        
        private void btn_limpiar_Click(object sender, EventArgs e)
        {
            limpiar();

        }

        private void limpiar()
        {
            txt_ape_nom.Text = "";
            txt_dni_socio.Text = "";
            txt_document_recibida.Text = "0";
            txt_empresa.Text = "";
            txt_mochilas_entregadas.Text = "0";
            txt_nro_socio.Text = "";
            Txt_dni.Text = "";
            txt_legajo.Text = "";
            chk_fdo_desempleo.Checked = false;
            txt_comentario.Text = "";
            Txt_dni.Focus();
            picbox_socio.Image = null;
            btn_sin_imagen.Visible = true;
        }

        private void btn_reasignar_mochila_Click(object sender, EventArgs e)
        {
            dgv_familiar_a_cargo_mochilas.CurrentRow.Cells["mochila"].Value = cbx_mochila.SelectedItem.ToString();
            dgv_familiar_a_cargo_mochilas.CurrentRow.Cells["entregar_mochila"].Value = true;
            dgv_familiar_a_cargo_mochilas.CurrentRow.Cells["entregar_mochila"].ReadOnly = false;
            
        }

        private void btn_recibir_docum_Click(object sender, EventArgs e)
        {
            int b = 0;
            if (contar_document_y_mochilas() == 0)
            {
                MessageBox.Show("debe tildar al menos una casilla de recepcion de documentacion", "¡¡¡¡ Atencion !!!!");
            }
            else
            {
                try
                {
                    foreach (DataGridViewRow fila in dgv_familiar_a_cargo_mochilas.Rows)
                    {
                        if ((fila.Cells["documentacion"].Value != null))
                        {
                            // && (fila.Cells["doc_recibida"].Value.ToString()) != "SI")
                            var var_controlar_repetidos = from a in DB_socios.entregas_mochilas where a.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString()) select a;
                            if (var_controlar_repetidos.Count() == 0)
                            {
                                entregas_mochilas entrega_mochi = new entregas_mochilas();
                                entrega_mochi.cod_fliar = Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString());
                                entrega_mochi.nro_doc_fliar = Convert.ToInt32(fila.Cells["dni_beneficiario"].Value.ToString());
                                entrega_mochi.recibe_docum = true;
                                entrega_mochi.fecha_recepcion_docum = DateTime.Now;
                                entrega_mochi.tipo_mochila = Convert.ToInt16(fila.Cells["mochila"].Value.ToString().Substring(0, 1));
                                entrega_mochi.codigo_usuario_recepcion = 0;
                                entrega_mochi.legajo = txt_legajo.Text;
                                if (chk_fdo_desempleo.Checked)
                                {
                                    entrega_mochi.fondo_desempleo = 1;
                                }
                                entrega_mochi.comentario = txt_comentario.Text;
                                DB_socios.entregas_mochilas.InsertOnSubmit(entrega_mochi);
                                DB_socios.SubmitChanges();
                                b = 1;
                            }
                        }
                    }

                    if (b == 1)
                    {
                        MessageBox.Show("Documentacion recibia EXITOSAMENTE", "¡¡¡¡ ATENCION !!!!");
                        limpiar();
                    }
                    
                    
                }
                catch (Exception)
                {

                    MessageBox.Show(""+ e,"¡¡¡¡ ERROR !!!!");
                    //Console.WriteLine(e));
                }
            }
        }

        private int contar_document_y_mochilas()
        {
            int cant_document = 0;
            int cant_mochilas = 0;

            foreach (DataGridViewRow fila in dgv_familiar_a_cargo_mochilas.Rows)
            {
               
                DataGridViewCheckBoxCell check = (DataGridViewCheckBoxCell) fila.Cells["documentacion"];
                if (check.Value != null)
                {
                    cant_document++;

                }

                if (fila.Cells["entregar_mochila"].Value != null)
                {
                    cant_mochilas++;
                }
            }
            txt_document_recibida.Text = cant_document.ToString();
            txt_mochilas_entregadas.Text = cant_mochilas.ToString();
            return cant_document;
        }

        private void dgv_familiar_a_cargo_mochilas_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_familiar_a_cargo_mochilas.IsCurrentCellDirty)
            {
                dgv_familiar_a_cargo_mochilas.CommitEdit(DataGridViewDataErrorContexts.Commit);
                contar_document_y_mochilas();
            }
        }

        private void btn_entrgar_mochila_Click(object sender, EventArgs e)
        {
            int b = 0;
            int nro_ent = 0;
            nro_ent =  Convert.ToInt32(DB_socios.entregas_mochilas.Max(x => x.numero_entrega).ToString() + 1);
            var im = from a in DB_socios.impresion_comprobante where a.nro_entrega >= 0 select a;
            //limpio la tabla de impresion
            foreach (var item in im)
            {
                DB_socios.impresion_comprobante.DeleteOnSubmit(item);
                DB_socios.SubmitChanges();
            }
            foreach (DataGridViewRow fila in dgv_familiar_a_cargo_mochilas.Rows)
            {
                if ((fila.Cells["entregar_mochila"].Value != null) && ( Convert.ToInt32(fila.Cells["entregada"].Value.ToString()) == 0))
                {
                    // busco en la tabla entregas_mochilas por codfliar y verifico si se encontro
                    var se_recibio_documentacion = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString()));
                    if (se_recibio_documentacion.Count() > 0)
                    {
                        // Ya esta encontrada la mochila de ese beneficiario y verifico no fue entregada
                        var se_recibio_documentacion_1 = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single(); 
                        if (se_recibio_documentacion_1.entrega_mochila is null)
                        {
                            //  se encontro el registro en la tabla de entregas_mochilas por que se recibio documentacion y no fue entragada aun, entonces cargo la entrega solamente
                            entregas_mochilas entrega_mochi_1 = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single();
                            entrega_mochi_1.entrega_mochila = true;
                            entrega_mochi_1.fecha_entrega_mochila = DateTime.Now;
                            DB_socios.SubmitChanges();
                        }
                    }
                    else
                    {
                        // NO se encontro el registro en la tabla de entregas_mochilas entonces cargo los datos necesarios
                        entregas_mochilas entrega_mochi = new entregas_mochilas();
                        entrega_mochi.cod_fliar = Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString());
                        entrega_mochi.nro_doc_fliar = Convert.ToInt32(fila.Cells["dni_beneficiario"].Value.ToString());
                        entrega_mochi.recibe_docum = true;
                        entrega_mochi.fecha_recepcion_docum = DateTime.Now;
                        entrega_mochi.tipo_mochila = Convert.ToInt16(fila.Cells["mochila"].Value.ToString().Substring(0, 1));
                        entrega_mochi.codigo_usuario_recepcion = 0;
                        entrega_mochi.legajo = txt_legajo.Text;
                        if (chk_fdo_desempleo.Checked)
                        {
                            entrega_mochi.fondo_desempleo = 1;
                        }
                        entrega_mochi.comentario = txt_comentario.Text;
                        entrega_mochi.entrega_mochila = true;
                        entrega_mochi.fecha_entrega_mochila = DateTime.Now;
                        DB_socios.entregas_mochilas.InsertOnSubmit(entrega_mochi);
                        DB_socios.SubmitChanges();
                        calcular_cant_entregas();
                    }

                    //grabo en impresiones_comprobantes
                    impresion_comprobante imp_comp = new impresion_comprobante();
                    imp_comp.nro_entrega = nro_ent;
                    imp_comp.nro_socio = Convert.ToInt32( txt_nro_socio.Text);
                    imp_comp.socio_apenom = txt_ape_nom.Text;
                    imp_comp.socio_dni = txt_dni_socio.Text;
                    imp_comp.socio_empresa = txt_empresa.Text;
                    imp_comp.benef_apenom = fila.Cells["ayn"].Value.ToString();
                    imp_comp.benef_dni = fila.Cells["dni_beneficiario"].Value.ToString(); 
                    imp_comp.benef_edad = Convert.ToInt32(fila.Cells["edad"].Value.ToString());
                    imp_comp.benef_tipo_mochila = fila.Cells["mochila"].Value.ToString().Substring(3);
                    imp_comp.benef_sexo = DB_soc_mysql.maeflia.Where(x => x.MAEFLIA_CODFLIAR == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().MAEFLIA_SEXO.ToString() ;
                    imp_comp.benef_legajo = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().legajo;
                    if (DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().fondo_desempleo != null)
                    {
                        imp_comp.benef_fdo_desempleo = "SI";
                    }
                    else
                    {
                        imp_comp.benef_fdo_desempleo = "NO";
                    }
                    imp_comp.comentario = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().comentario;
                    DB_socios.impresion_comprobante.InsertOnSubmit(imp_comp);
                    DB_socios.SubmitChanges();
                    calcular_cant_entregas();
                    b = 1;
                }  
            }
            if (b==1)
            {
                reportes frm_reportes = new reportes();
                frm_reportes.ShowDialog();
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            int nro_ent = 0;
            nro_ent = Convert.ToInt32(DB_socios.entregas_mochilas.Max(x => x.numero_entrega).ToString() + 1);
            var im = from a in DB_socios.impresion_comprobante where a.nro_entrega >= 0 select a;
            //limpio la tabla de impresion
            foreach (var item in im)
            {
                DB_socios.impresion_comprobante.DeleteOnSubmit(item);
                DB_socios.SubmitChanges();
            }
            foreach (DataGridViewRow fila in dgv_familiar_a_cargo_mochilas.Rows)
            {
                if ((fila.Cells["entregar_mochila"].Value != null) && (Convert.ToInt32(fila.Cells["entregada"].Value.ToString()) == 1))
                {
                    //grabo en impresiones_comprobantes
                    impresion_comprobante imp_comp = new impresion_comprobante();
                    imp_comp.nro_entrega = nro_ent;
                    imp_comp.nro_socio = Convert.ToInt32(txt_nro_socio.Text);
                    imp_comp.socio_apenom = txt_ape_nom.Text;
                    imp_comp.socio_dni = txt_dni_socio.Text;
                    imp_comp.socio_empresa = txt_empresa.Text;
                    imp_comp.benef_apenom = fila.Cells["ayn"].Value.ToString();
                    imp_comp.benef_dni = fila.Cells["dni_beneficiario"].Value.ToString();
                    imp_comp.benef_edad = Convert.ToInt32(fila.Cells["edad"].Value.ToString());
                    string mo = fila.Cells["mochila"].Value.ToString();
                    imp_comp.benef_tipo_mochila = mo.Substring(12,mo.Length - 12);
                    imp_comp.benef_sexo = DB_soc_mysql.maeflia.Where(x => x.MAEFLIA_CODFLIAR == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().MAEFLIA_SEXO.ToString();
                    imp_comp.benef_legajo = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().legajo;
                    if (DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().fondo_desempleo != null)
                    {
                        imp_comp.benef_fdo_desempleo = "SI";
                    }
                    else
                    {
                        imp_comp.benef_fdo_desempleo = "NO";
                    }
                    imp_comp.comentario = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single().comentario;
                    DB_socios.impresion_comprobante.InsertOnSubmit(imp_comp);
                    DB_socios.SubmitChanges();
                    calcular_cant_entregas();
                    calcular_totales();
                }
            }
            reportes frm_reportes = new reportes();
            frm_reportes.ShowDialog();
        }

        private void txt_buscar_por_legajo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var buscar_por_leg = DB_socios.entregas_mochilas.Where(x => x.legajo == txt_buscar_por_legajo.Text);
                    
                if (buscar_por_leg.Count() > 0)
                {
                    var buscar_por_leg_1 = DB_socios.entregas_mochilas.Where(x => x.legajo == txt_buscar_por_legajo.Text).First();
                    //var buscar_dni_titu = DB_soc_mysql.socflia.Where(x => x.SOCFLIA_CODFLIAR == buscar_por_leg_1.cod_fliar).Single().SOCFLIA_CUIL;
                    Txt_dni.Text = DB_soc_mysql.socflia.Where(x => x.SOCFLIA_CODFLIAR == buscar_por_leg_1.cod_fliar).Single().SOCFLIA_CUIL.ToString().Substring(2,8);
                }
            }
        }

        private void controlar_mochilas_entregadas()
        {
            foreach (DataGridViewRow fila in dgv_familiar_a_cargo_mochilas.Rows)
            {
                // busco en la tabla entregas_mochilas por codfliar y verifico si se encontro
                var se_recibio_documentacion = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString()));
                if (se_recibio_documentacion.Count() > 0)
                {
                    // Ya esta encontrada la mochila de ese beneficiario y verifico no fue entregada
                    var se_recibio_documentacion_1 = DB_socios.entregas_mochilas.Where(x => x.cod_fliar == Convert.ToInt16(fila.Cells["codigo_familiar"].Value.ToString())).Single(); ;
                    if (se_recibio_documentacion_1.entrega_mochila == true)
                    {
                        fila.DefaultCellStyle.BackColor = Color.Red;
                        fila.Cells["entregar_mochila"].ReadOnly = true;
                        if (se_recibio_documentacion_1.tipo_mochila.Value == 1) fila.Cells["mochila"].Value = "[ENTREGADO] Jardin ";
                        if (se_recibio_documentacion_1.tipo_mochila.Value == 2) fila.Cells["mochila"].Value = "[ENTREGADO] Primaria I  (1º y 2º Grado)";
                        if (se_recibio_documentacion_1.tipo_mochila.Value == 3) fila.Cells["mochila"].Value = "[ENTREGADO] Primaria II (3º y 6º Grado)";
                        if (se_recibio_documentacion_1.tipo_mochila.Value == 4) fila.Cells["mochila"].Value = "[ENTREGADO] Secundaria  (7º en adelante)";
                        fila.Cells["mochila"].ReadOnly = true;
                        fila.Cells["entregar_mochila"].Value = true;
                        fila.Cells["entregada"].Value = 1;
                    }
                    else
                    {
                        fila.Cells["entregada"].Value = 0;
                    }
                }
                else
                {
                    fila.Cells["entregada"].Value = 0;
                }
            }
        }

        private void frm_mochilas_Load(object sender, EventArgs e)
        {
            calcular_cant_entregas();
            calcular_totales();
        }

        private void calcular_cant_entregas()
        {
            var mochi_entregadas = DB_socios.entregas_mochilas.Where(x => x.entrega_mochila == true).Count();
            txt_mochilas_entregadas.Text = mochi_entregadas.ToString();
        }

        private void calcular_totales()
        {
            var total_mujer = from a in DB_socios.entregas_mochilas
                              join g in DB_socios.maeflia on a.cod_fliar equals Convert.ToInt32( g.MAEFLIA_CODFLIAR)  
                              where a.entrega_mochila == true
                              select new
                              {
                                  tipo_mochi = a.tipo_mochila,
                                  sexo = g.MAEFLIA_SEXO,
                              };

            txt_tipomoch1_mujer.Text = total_mujer.Count(x => x.tipo_mochi == 1 && x.sexo == 'F').ToString();
            txt_tipomoch1_varon.Text = total_mujer.Count(x => x.tipo_mochi == 1 && x.sexo == 'M').ToString();

            txt_tipomoch2_mujer.Text = total_mujer.Count(x => x.tipo_mochi == 2 && x.sexo == 'F').ToString();
            txt_tipomoch2_varon.Text = total_mujer.Count(x => x.tipo_mochi == 2 && x.sexo == 'M').ToString();

            txt_tipomoch3_mujer.Text = total_mujer.Count(x => x.tipo_mochi == 3 && x.sexo == 'F').ToString();
            txt_tipomoch3_varon.Text = total_mujer.Count(x => x.tipo_mochi == 3 && x.sexo == 'M').ToString();

            txt_tipomoch4_mujer.Text = total_mujer.Count(x => x.tipo_mochi == 4 && x.sexo == 'F').ToString();
            txt_tipomoch4_varon.Text = total_mujer.Count(x => x.tipo_mochi == 4 && x.sexo == 'M').ToString();

            txt_tot_tipomoch1.Text = (Convert.ToInt32(txt_tipomoch1_mujer.Text) + Convert.ToInt32(txt_tipomoch1_varon.Text)).ToString();
            txt_tot_tipomoch2.Text = (Convert.ToInt32(txt_tipomoch2_mujer.Text) + Convert.ToInt32(txt_tipomoch2_varon.Text)).ToString();
            txt_tot_tipomoch3.Text = (Convert.ToInt32(txt_tipomoch3_mujer.Text) + Convert.ToInt32(txt_tipomoch3_varon.Text)).ToString();
            txt_tot_tipomoch4.Text = (Convert.ToInt32(txt_tipomoch4_mujer.Text) + Convert.ToInt32(txt_tipomoch4_varon.Text)).ToString();

            txt_tot_tipomoch_mujer.Text = (Convert.ToInt32(txt_tipomoch1_mujer.Text) + Convert.ToInt32(txt_tipomoch2_mujer.Text) + Convert.ToInt32(txt_tipomoch3_mujer.Text) + Convert.ToInt32(txt_tipomoch4_mujer.Text)).ToString();
            txt_tot_tipomoch_varon.Text = (Convert.ToInt32(txt_tipomoch1_varon.Text) + Convert.ToInt32(txt_tipomoch2_varon.Text) + Convert.ToInt32(txt_tipomoch3_varon.Text) + Convert.ToInt32(txt_tipomoch4_varon.Text)).ToString();

            txt_total_mochilas.Text = (Convert.ToInt32(txt_tot_tipomoch1.Text) + Convert.ToInt32(txt_tot_tipomoch2.Text) + Convert.ToInt32(txt_tot_tipomoch3.Text) + Convert.ToInt32(txt_tot_tipomoch4.Text)).ToString();

            txt_sin_sexo_cargado.Text = total_mujer.Count(x => x.sexo.Equals("")).ToString();

            txt_total_mochilas_estimada.Text = (Convert.ToInt32(txt_tipomoch1_estimado.Text) + Convert.ToInt32(txt_tipomoch2_estimado.Text) + Convert.ToInt32(txt_tipomoch3_estimado.Text) + Convert.ToInt32(txt_tipomoch4_estimado.Text)).ToString();

            //Calculo las cantidades de mochilas que faltarian entregar
            txt_tipomoch1_falta_ent.Text = (Convert.ToInt32(txt_tipomoch1_estimado.Text) - Convert.ToInt32(txt_tot_tipomoch1.Text)).ToString();
            txt_tipomoch2_falta_ent.Text = (Convert.ToInt32(txt_tipomoch2_estimado.Text) - Convert.ToInt32(txt_tot_tipomoch2.Text)).ToString();
            txt_tipomoch3_falta_ent.Text = (Convert.ToInt32(txt_tipomoch3_estimado.Text) - Convert.ToInt32(txt_tot_tipomoch3.Text)).ToString();
            txt_tipomoch4_falta_ent.Text = (Convert.ToInt32(txt_tipomoch4_estimado.Text) - Convert.ToInt32(txt_tot_tipomoch4.Text)).ToString();
            txt_total_mochilas_falta_ent.Text = (Convert.ToInt32(txt_total_mochilas_estimada.Text) - Convert.ToInt32(txt_total_mochilas.Text)).ToString();
        }
    }
}
