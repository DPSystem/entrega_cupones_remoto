using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace entrega_cupones
{
    public partial class frm_futbol : Form
    {
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }

        lts_sindicatoDataContext db_sindicato = new lts_sindicatoDataContext();


        public frm_futbol()
        {
            InitializeComponent();
        }

        private void frm_futbol_Load(object sender, EventArgs e)
        {
            cargar_cbx_categoria();
            dgv_equiposInscriptos.AutoGenerateColumns = false;
            //  Dim diasQueFaltanParaElDomingo As Integer = 7 + CInt(DayOfWeek.Monday) - CInt(fechaInicial.DayOfWeek)
            string año = "";
            string mes = "";
            string dia = "";
            int domingo = 0;
            domingo = 7 + Convert.ToInt32((DayOfWeek.Monday)) - Convert.ToInt32(dtp_fecha_partidos.Value.DayOfWeek);
            if (domingo > 7) domingo = domingo - 7;
            if (domingo < 7) domingo = domingo - 1;
            if (domingo == 7) domingo = 0;

            año = DateTime.Now.Year.ToString();
            mes = DateTime.Now.Month.ToString();
            // dia = (DateTime.Now.Day + domingo).ToString();
            dia = (DateTime.Now.AddDays(domingo)).ToString();

            //String f = Convert.ToString(año) + "/" + Convert.ToString(mes) + "/" + dia;

            dtp_fecha_partidos.Value = Convert.ToDateTime(dia);
        }

        private void cargar_cbx_categoria()
        {
            var cat = from a in db_sindicato.categorias select a; // delcaro variable con categorias
            // Cargo el combo  cbx_categoria de la inscripcion de equipos
            cbx_categoria.DisplayMember = "catnombre";
            cbx_categoria.ValueMember = "catid";
            
            cbx_categoria_equipos_inscriptos.DisplayMember = "catnombre";
            cbx_categoria_equipos_inscriptos.ValueMember = "catid";

            cbx_categoria_incripcion_jugadores.DisplayMember = "catnombre";
            cbx_categoria_incripcion_jugadores.ValueMember = "catid";

            cbx_categoria_jugadores_inscriptos.DisplayMember = "catnombre";
            cbx_categoria_jugadores_inscriptos.ValueMember = "catid";

            cbx_categoria_partidos.DisplayMember = "catnombre";
            cbx_categoria_partidos.ValueMember = "catid";

            cbx_categoria.DataSource = cat.ToList();
            cbx_categoria_equipos_inscriptos.DataSource = cat.ToList();
            cbx_categoria_incripcion_jugadores.DataSource = cat.ToList();
            cbx_categoria_jugadores_inscriptos.DataSource = cat.ToList();
            cbx_categoria_partidos.DataSource = cat.ToList();

            // Cargo el combo  cbx_liga
            var liga = from lig in db_sindicato.ligas select lig;
            cbx_liga_inscripcion_equipos.DisplayMember = "liganombre";
            cbx_liga_inscripcion_equipos.ValueMember = "ligaid";

            cbx_liga_partidos.DisplayMember = "liganombre";
            cbx_liga_partidos.ValueMember = "ligaid";
            
            cbx_liga_inscripcion_equipos.DataSource = liga.ToList();
            cbx_liga_partidos.DataSource = liga.ToList();

            // Cargo el combo  campeonato
            var campeonato = from camp in db_sindicato.campeonatos select camp;
            cbx_campeonato_inscripcion_equipos.DisplayMember = "campnombre";
            cbx_campeonato_inscripcion_equipos.ValueMember = "campid";

            cbx_campeonato_partidos.DisplayMember = "campnombre";
            cbx_campeonato_partidos.ValueMember = "campid";

            cbx_campeonato_inscripcion_equipos.DataSource = campeonato.ToList();
            cbx_campeonato_partidos.DataSource = campeonato.ToList();

            // Cargo el combo fases
            var fases = from f in db_sindicato.fases select f;

            cbx_fase_partidos.DisplayMember = "fasenombre";
            cbx_fase_partidos.ValueMember = "fase_id";
            cbx_fase_partidos.DataSource = fases.ToList();

            // Cargo cbx canchas

            var cancha = from can in db_sindicato.canchas select can;

            cbx_cancha_partidos.DisplayMember = "canchanombre";
            cbx_cancha_partidos.ValueMember = "canchaid";
            cbx_cancha_partidos.DataSource = cancha.ToList();

            // Cargar cbx_cambio de equipo
            var cbioEquipo = db_sindicato.equipos.OrderBy(x => x.EQUIPONOMBRE);
            cbx_cambio_equipo.DisplayMember = "equiponombre";
            cbx_cambio_equipo.ValueMember = "equipoid";
            cbx_cambio_equipo.DataSource = cbioEquipo.ToList();
        }

        private void btn_ligas_Click(object sender, EventArgs e)
        {
            bunifuTransition1.ShowSync(picbox_formacion, false, null);
            picbox_formacion.Visible = true;
        }

        private void btn_cerrar_futbol_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_formacion_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageFormacion;
            bunifuTransition1.ShowSync(picbox_formacion, false, null);
            picbox_formacion.Visible = true;
            picbox_posiciones.Visible = false;
            pnl_equiposInscriptos.Visible = false;
            pnl_jugadoresInscriptos.Visible = false;
        }

        private void btn_posiciones_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPagePosiciones;
            bunifuTransition1.ShowSync(picbox_posiciones, false, null);
            pnl_inscripcionEquipos.Visible = false;
            picbox_posiciones.Visible = true;
            picbox_formacion.Visible = false;
            pnl_equiposInscriptos.Visible = false;
            pnl_jugadoresInscriptos.Visible = false;
        }

        private void btn_equipos_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageEquipos;
            bunifuTransition1.ShowSync(pnl_inscripcionEquipos, false, null);
            pnl_inscripcionEquipos.Visible = true;
            picbox_posiciones.Visible = false;
            picbox_formacion.Visible = false;
            transitionHD.ShowSync(pnl_equiposInscriptos, false, null);
            pnl_equiposInscriptos.Visible = true;
            pnl_jugadoresInscriptos.Visible = false;
        }

        private void btn_jugadores_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageJugadores;
            bunifuTransition1.ShowSync(pnl_jugadoresInscripcion, false, null);
            pnl_jugadoresInscripcion.Visible = true;
            picbox_posiciones.Visible = false;
            picbox_formacion.Visible = false;
            pnl_equiposInscriptos.Visible = false;
            transitionHD.ShowSync(pnl_jugadoresInscriptos, false, null);
            pnl_jugadoresInscriptos.Visible = true;
        }

        private void btn_inscribir_equipo_Click(object sender, EventArgs e)
        {
            if (txt_equipo.Text != "")
            {
                try
                {
                    equipos varEquipos = new equipos();
                    varEquipos.EQUIPONOMBRE = txt_equipo.Text.Trim();
                    varEquipos.EQUIPO_CATID = Convert.ToInt32(cbx_categoria.SelectedValue);
                    db_sindicato.equipos.InsertOnSubmit(varEquipos);
                    db_sindicato.SubmitChanges();


                    campequipo campe = new campequipo();
                    campe.CAMP_CAMPID = Convert.ToInt32(cbx_campeonato_inscripcion_equipos.SelectedValue);
                    campe.CAMP_EQUIPOID = db_sindicato.equipos.Max(x => x.EQUIPOID);
                    db_sindicato.campequipo.InsertOnSubmit(campe);
                    db_sindicato.SubmitChanges();

                    
                    MessageBox.Show("El equipo " + txt_equipo.Text.Trim() + " fue cargado exitosamente.");
                    txt_equipo.Text = "";
                    listar_equipos_inscriptos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.ToString());
                    throw;
                }
            }
        }
        private void listar_equipos_inscriptos()
        {
            var equipos_inscriptos = (from equipo in db_sindicato.equipos.Where(x => x.EQUIPO_CATID == Convert.ToInt32(cbx_categoria_equipos_inscriptos.SelectedValue))
                                      select equipo).OrderBy(x=>x.EQUIPONOMBRE);
            dgv_equiposInscriptos.DataSource = equipos_inscriptos.ToList();
            if (equipos_inscriptos.Count() > 0) lbl_total_equipos_inscriptos.Text = "Total inscriptos: " + equipos_inscriptos.Count().ToString();
            if (equipos_inscriptos.Count() == 0) lbl_total_equipos_inscriptos.Text = "Total inscriptos: 0";
        }

        private void cbx_categoria_equipos_inscriptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            listar_equipos_inscriptos();
        }

        private void txt_dni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_dni.Text.Length > 8)
                {
                    txt_dni.Text = txt_dni.Text.Substring(2, 8);
                    //text = text.Substring(2, 8);
                    buscar_socio();
                }
                else
                {
                    if (txt_dni.Text.Length == 8) buscar_socio();
                    if (txt_dni.Text.Length < 8) MessageBox.Show("La cantidad de digitos no es la correcta. Deben ser 8 digitos","ATENCON");
                }
            }
        }

        private void buscar_socio() {


            var no_existe = from soc in db_sindicato.maesoc
                            where soc.MAESOC_NRODOC == txt_dni.Text
                            select soc;

            if (no_existe.Count() > 0) // si es mayor que cero entonces existe luego hay que ver si es socio. Si es menor que cero entonces NO EXISTE
            {
                // si existe
                mostrar_foto(no_existe.First().MAESOC_CUIL,1);
                mostrar_datos_socio(no_existe.First().MAESOC_CUIL);
                //voy a preguntar si es socio 
                var socio = from soc in db_sindicato.maesoc
                            where soc.MAESOC_NRODOC == txt_dni.Text
                            join soce in db_sindicato.soccen on soc.MAESOC_CUIL equals soce.SOCCEN_CUIL
                            where soce.SOCCEN_ESTADO == 1
                            select soc;
                if (socio.Count() > 0) // si es > 0 entonces es socio sino NO ES SOCIO
                {
                    if (socio.Single().MAESOC_NROAFIL.Trim() == "") // si el nro de afiliado es null o espacio en blanco entonces es socio pasivo
                    {
                        txt_nroSocio.Text = "0";
                    }
                    else
                    {
                        txt_nroSocio.Text = socio.Single().MAESOC_NROAFIL.ToString();
                    }
                    if (txt_nroSocio.Text == "0")// si nro de afiliafo  es null o espacio en blanco entonces es socio pasivo
                    {
                        txt_estado.Text = " SOCIO PASIVO (''No posee Nº de Afiliado'')";
                        btn_inscribirJugador.Enabled = false;
                    }
                    else
                    {
                        txt_estado.Text = "SOCIO ACTIVO";
                    btn_inscribirJugador.Enabled = true;
                    }
                }
                else
                {
                    txt_estado.Text = "NO ES SOCIO";
                    btn_inscribirJugador.Enabled = false;
                }
            }
            else
            {
                txt_estado.Text = "NO EXISTE";
                picbox_inscripcion_jugadores.Image = Properties.Resources.contorno_usuario;
                limpiar_controles();
                btn_inscribirJugador.Enabled = false;
            }
        }

        private void mostrar_foto(double cuil, int inscripcion) // inscripcion = 1 muestro foto para picbox_inscripcion
        {
            var foto = db_sindicato.fotos.Where(x => x.FOTOS_CUIL == cuil && x.FOTOS_CODFLIAR == 0); ///Convert.ToDouble(dgv_reservados.CurrentRow.Cells[6].Value.ToString()
            if (foto.Count() > 0)
            {
                if (inscripcion == 1) picbox_inscripcion_jugadores.Image = ByteArrayToImage(foto.Single().FOTOS_FOTO.ToArray());
                if (inscripcion == 2) picbox_jugadores_inscriptos.Image = ByteArrayToImage(foto.Single().FOTOS_FOTO.ToArray());

            }
            else
            {
                if (inscripcion == 1) picbox_inscripcion_jugadores.Image = Properties.Resources.contorno_usuario;
                if (inscripcion == 2) picbox_jugadores_inscriptos.Image = Properties.Resources.contorno_usuario;
                
            }
        }

        private void mostrar_datos_socio(double cuil)
        {
            var datos_socios = (from ds in db_sindicato.maesoc where ds.MAESOC_CUIL == cuil select ds).Single();

            txt_CUIL.Text = datos_socios.MAESOC_CUIL.ToString();
            txt_apellido.Text = datos_socios.MAESOC_APELLIDO.Trim();
            txt_nombre.Text = datos_socios.MAESOC_NOMBRE.Trim();
            String domicilio ="",calle ="",nrocalle="",barrio = "";
            if (datos_socios.MAESOC_CALLE != null) calle = datos_socios.MAESOC_CALLE.Trim();
            if (datos_socios.MAESOC_NROCALLE != null) nrocalle = datos_socios.MAESOC_NROCALLE.Trim();
            if (datos_socios.MAESOC_BARRIO != null) barrio = datos_socios.MAESOC_BARRIO.Trim();
            domicilio = calle + " " + nrocalle + " " + barrio;
            txt_domicilio.Text = domicilio; //datos_socios.MAESOC_CALLE.Trim() + " " + datos_socios.MAESOC_NROCALLE.Trim() + " " + datos_socios.MAESOC_BARRIO.Trim();
            txt_fechaNac.Text = Convert.ToString( datos_socios.MAESOC_FECHANAC.Date);
            txt_edad.Text = calcular_edad(datos_socios.MAESOC_FECHANAC).ToString();
            var empr = db_sindicato.maeemp.Where(t => t.MAEEMP_CUIT == db_sindicato.socemp.Where(x => x.SOCEMP_CUIL == cuil && x.SOCEMP_ULT_EMPRESA == 'S').Single().SOCEMP_CUITE);//.Single().razons.ToString();
            if (empr.Count() > 0)
            {
                txt_empresa.Text = empr.Single().MAEEMP_RAZSOC;//db_sindicato.empresas.Where(t => t.cuit == db_sindicato.socemp.Where(x => x.SOCEMP_CUIL == cuil && x.SOCEMP_ULT_EMPRESA == 'S').Single().SOCEMP_CUITE).Single().razons.ToString();
            }
            else
            {
                txt_empresa.Text = "SIN EMPRESA";
            }
            
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
        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (System.IO.MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void limpiar_controles()
        {
            txt_CUIL.Text = "";
            txt_nroSocio.Text = "";
            txt_apellido.Text = "";
            txt_nombre.Text = "";
            txt_domicilio.Text = "";
            txt_fechaNac.Text = "";
            txt_edad.Text = "";
            txt_empresa.Text = "";
            txt_edad.Text = "";
            txt_empresa.Text = "";
        }

        private void cbx_categoria_incripcion_jugadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            var var_equipos1 = (from equi1 in db_sindicato.equipos
                                where equi1.EQUIPO_CATID == Convert.ToInt32(cbx_categoria_incripcion_jugadores.SelectedValue)
                                select equi1).OrderBy(x => x.EQUIPONOMBRE);

            cbx_equipo_incripcion_jugadores.DisplayMember = "equiponombre";
            cbx_equipo_incripcion_jugadores.ValueMember = "equipoid";
            cbx_equipo_incripcion_jugadores.DataSource = var_equipos1.ToList();

        }

        private void cbx_categoria_jugadores_inscriptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var var_equipos1 = (from equi1 in db_sindicato.equipos
                                where equi1.EQUIPO_CATID == Convert.ToInt32(cbx_categoria_jugadores_inscriptos.SelectedValue)
                                select equi1).OrderBy(x => x.EQUIPONOMBRE);

            cbx_equipo_jugadores_inscriptos.DisplayMember = "equiponombre";
            cbx_equipo_jugadores_inscriptos.ValueMember = "equipoid";
            cbx_equipo_jugadores_inscriptos.DataSource = var_equipos1.ToList();
        }

        private equipos mostrar_equipos_por_categoria(int categoriaID)
        {
            var var_equipos1 = (from equi1 in db_sindicato.equipos
                                where equi1.EQUIPO_CATID == categoriaID //Convert.ToInt32(cbx_categoria_jugadores_inscriptos.SelectedValue)
                                select equi1).OrderBy(x => x.EQUIPONOMBRE);
            return  (equipos) var_equipos1;
        }

        private void txt_nroSocio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_nroSocio.Text != "" )
                {
                    var cuil = from cl in db_sindicato.maesoc.Where(x => x.MAESOC_NROAFIL == txt_nroSocio.Text) select cl;
                    if (cuil.Count() > 0)
                    {
                        txt_dni.Text = cuil.Single().MAESOC_NRODOC;
                        buscar_socio();
                    }
                    else
                    {
                        limpiar_controles();
                        txt_dni.Text = "";
                        txt_estado.Text = "NO EXISTE";
                        btn_inscribirJugador.Enabled = false;
                        mostrar_foto(1, 1);
                    }
                }
                else
                {
                    limpiar_controles();
                    txt_dni.Text = "";
                    txt_estado.Text = "NO EXISTE";
                    btn_inscribirJugador.Enabled = false;
                    mostrar_foto(1, 1);
                }
            }
        }

        private void btn_inscribirJugador_Click(object sender, EventArgs e)
        {
            var ya_inscripto = from a in db_sindicato.jugadores.Where(x => x.JUG_SOCCENCUIL == Convert.ToDouble( txt_CUIL.Text)) select a;

            if (ya_inscripto.Count() > 0)
            {
                MessageBox.Show("Ya Esta Inscripto en el equipo " + db_sindicato.equipos.Where(x => x.EQUIPOID == ya_inscripto.Single().JUG_EQUIPOID).Single().EQUIPONOMBRE);
            }
            else
            {
                jugadores jug = new jugadores();
                jug.JUG_EQUIPOID = Convert.ToInt32(cbx_equipo_incripcion_jugadores.SelectedValue);
                jug.JUG_MAESOC_NROAFIL = txt_nroSocio.Text;
                jug.JUG_SOCCENCUIL = Convert.ToDouble(txt_CUIL.Text);
                db_sindicato.jugadores.InsertOnSubmit(jug);
                db_sindicato.SubmitChanges();
                mostrar_jugadores_inscriptos();
            }
            
        }

        private void mostrar_jugadores_inscriptos()
        {
            var jugador_inscripto = (from ji in db_sindicato.jugadores
                                    where ji.JUG_EQUIPOID == Convert.ToInt32(cbx_equipo_jugadores_inscriptos.SelectedValue)
                                    join msc in db_sindicato.maesoc on ji.JUG_SOCCENCUIL equals msc.MAESOC_CUIL
                                    select new
                                    {
                                        jugid = ji.JUGID,
                                        jugnrosocio = msc.MAESOC_NROAFIL,
                                        jugapenom = msc.MAESOC_APELLIDO.Trim() + " " + msc.MAESOC_NOMBRE,
                                        jugcuil = msc.MAESOC_CUIL
                                    }).OrderBy(x=>x.jugapenom);
            dgv_jugadores_inscriptos.DataSource = jugador_inscripto.ToList();
            txt_total_jugadores.Text = jugador_inscripto.Count().ToString();
        }

        private void cbx_equipo_jugadores_inscriptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            mostrar_jugadores_inscriptos();
        }

        private void dgv_jugadores_inscriptos_SelectionChanged(object sender, EventArgs e)
        {
            mostrar_foto(Convert.ToDouble(dgv_jugadores_inscriptos.CurrentRow.Cells[3].Value), 2);
            mostrar_sanciones();
        }

        private void mostrar_sanciones()
        {

            while (dgv_sanciones_aplicadas.Rows.Count != 0)
            {
                dgv_sanciones_aplicadas.Rows.RemoveAt(0);
            }

            var sancion = from a in db_sindicato.sanciones where  a.ID_JUG == Convert.ToUInt32(dgv_jugadores_inscriptos.CurrentRow.Cells["nroafil"].Value)
                          select new
                          {
                              nro_fecha = a.NRO_FECHA,
                              de = "de",
                              cant_fechas = a.CANTIDAD_FECHAS,
                              fecha_partido = a.FECHA_PARTIDO.Date,
                              id_jugador = a.ID_JUG,
                              id_sancion = a.ID_SANCION
                              
                          };
            if (sancion.Count() > 0)
            {
               
               
                int fila = 0;
                foreach (var item in sancion.ToList())
                {
                    dgv_sanciones_aplicadas.Rows.Add();
                    fila = dgv_sanciones_aplicadas.Rows.Count - 1;
                    dgv_sanciones_aplicadas.Rows[fila].Cells["jugador"].Value = item.id_jugador;
                    dgv_sanciones_aplicadas.Rows[fila].Cells["nro_fecha"].Value = item.nro_fecha;
                    dgv_sanciones_aplicadas.Rows[fila].Cells["de"].Value = item.de;
                    dgv_sanciones_aplicadas.Rows[fila].Cells["cant_fechas"].Value = item.cant_fechas;
                    dgv_sanciones_aplicadas.Rows[fila].Cells["fecha_partido"].Value = item.fecha_partido.Date;
                    //dgv_sanciones_aplicadas.Rows[fila].Cells["id_sancion"].Value = item.id_sancion;
                }

//                dgv_sanciones_aplicadas.DataSource = sancion.ToList();
            }

        }

        private void btn_partidos_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPagePartidos;
           // bunifuTransition1.ShowSync(pnl_inscripcionEquipos, false, null);

            pnl_inscripcionEquipos.Visible = false;
            picbox_posiciones.Visible = false;
            picbox_formacion.Visible = false;
            transitionHD.ShowSync(pnl_equiposInscriptos, false, null);
            pnl_equiposInscriptos.Visible = true;
            pnl_jugadoresInscriptos.Visible = false;
        }

        private void cbx_liga_inscripcion_equipos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbx_categoria_partidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var equi1 = (from e1 in db_sindicato.equipos
                        where e1.EQUIPO_CATID == Convert.ToInt32 (cbx_categoria_partidos.SelectedValue)
                        select e1).OrderBy(x => x.EQUIPONOMBRE);

            cbx_equipo1_partidos.DisplayMember = "equiponombre";
            cbx_equipo1_partidos.ValueMember = "equipoid";
            cbx_equipo1_partidos.DataSource = equi1.ToList();
        }

        private void cbx_equipo1_partidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var equi2 = (from e2 in db_sindicato.equipos
                        where e2.EQUIPO_CATID == (Convert.ToInt32(cbx_categoria_partidos.SelectedValue)) && (e2.EQUIPOID != Convert.ToInt32(cbx_equipo1_partidos.SelectedValue))
                        select e2).OrderBy(x=>x.EQUIPONOMBRE);

            cbx_equipo2_partidos.DisplayMember = "equiponombre";
            cbx_equipo2_partidos.ValueMember = "equipoid";
            cbx_equipo2_partidos.DataSource = equi2.ToList();
        }

        private void btn_guardar_partido_Click(object sender, EventArgs e)
        {
            partidos part = new partidos();
            part.PARTIDO_LIGAID = Convert.ToInt32(cbx_liga_partidos.SelectedValue);
            part.PARTIDO_CAMPID = Convert.ToInt32(cbx_campeonato_partidos.SelectedValue);
            part.PARTIDO_FASEID = Convert.ToInt32(cbx_fase_partidos.SelectedValue);
            part.PARTIDOFECHA = dtp_fecha_partidos.Value;
            part.PARTIDO_CANCHAID = Convert.ToInt32(cbx_cancha_partidos.SelectedValue);
            part.PARTIDO_CATID = Convert.ToInt32(cbx_categoria_partidos.SelectedValue);
            part.PARTIDOEQUIPO1 = Convert.ToInt32(cbx_equipo1_partidos.SelectedValue);
            part.PARTIDOEQUIPO2 = Convert.ToInt32(cbx_equipo2_partidos.SelectedValue);
            part.PARTIDOHORA = dtp_horario_partidos.Value.TimeOfDay;
            part.PARTIDONROFECHA = Convert.ToInt32(cbx_nrofecha.SelectedItem);
            db_sindicato.partidos.InsertOnSubmit(part);
            db_sindicato.SubmitChanges();
            mostrar_partidos_cancha1();
            mostrar_partidos_cancha2();
        }

        private void mostrar_partidos_cancha1()
        {
            var partidos_cancha_1 = (from pc1 in db_sindicato.partidos
                                    where pc1.PARTIDO_CANCHAID == 1 && pc1.PARTIDOFECHA == dtp_fecha_partidos.Value
                                    select new
                                    {
                                        hora = pc1.PARTIDOHORA,
                                        equipo1 = db_sindicato.equipos.Where(x => x.EQUIPOID == pc1.PARTIDOEQUIPO1).Single().EQUIPONOMBRE,
                                        vs = "VS",
                                        equipo2 = db_sindicato.equipos.Where(x => x.EQUIPOID == pc1.PARTIDOEQUIPO2).Single().EQUIPONOMBRE,
                                        categoria = db_sindicato.categorias.Where(x => x.CATID == pc1.PARTIDO_CATID).Single().CATNOMBRE,
                                        partidoid = pc1.PARTIDOID
                                    }).OrderBy(x=>x.hora);
            dgv_partidos_cancha1.DataSource = partidos_cancha_1.ToList();
            lbl_partidos_cancha1.Text = Convert.ToString(partidos_cancha_1.Count()) + " Partidos en Cancha 1";

        }

        private void mostrar_partidos_cancha2()
        {
            var partidos_cancha_2 = (from pc2 in db_sindicato.partidos
                                    where pc2.PARTIDO_CANCHAID == 2 && pc2.PARTIDOFECHA == dtp_fecha_partidos.Value
                                    select new
                                    {
                                        hora = pc2.PARTIDOHORA,
                                        equipo1 = db_sindicato.equipos.Where(x => x.EQUIPOID == pc2.PARTIDOEQUIPO1).Single().EQUIPONOMBRE,
                                        vs = "VS",
                                        equipo2 = db_sindicato.equipos.Where(x => x.EQUIPOID == pc2.PARTIDOEQUIPO2).Single().EQUIPONOMBRE,
                                        categoria = db_sindicato.categorias.Where(x => x.CATID == pc2.PARTIDO_CATID).Single().CATNOMBRE,
                                        partidoid = pc2.PARTIDOID
                                    }).OrderBy(x => x.hora);
            dgv_partidos_cancha2.DataSource = partidos_cancha_2.ToList();
            lbl_partidos_cancha2.Text = Convert.ToString(partidos_cancha_2.Count()) + " Partidos en Cancha 2";
        }

        private void dtp_fecha_partidos_ValueChanged(object sender, EventArgs e)
        {
            mostrar_partidos_cancha1();
            mostrar_partidos_cancha2();
        }

        private void btn_imprmir_equipos_Click(object sender, EventArgs e)
        {

        }

        private void btn_imprimir_planilla_partidos_Click(object sender, EventArgs e)
        {
            //limpio la tabla de impresion
            var im = from a in db_sindicato.impresion_comprobante select a;//DB_socios.impresion_comprobante where a.nro_entrega >= 0 select a;
            foreach (var item in im)
            {
                db_sindicato.impresion_comprobante.DeleteOnSubmit(item);
                db_sindicato.SubmitChanges();
            }
           
            // para el dgv_partidos_cancha_1
            foreach (DataGridViewRow fila in dgv_partidos_cancha1.Rows)
            {
                //impresion_comprobante imp = new impresion_comprobante();
                var partidos = from a in db_sindicato.partidos where a.PARTIDOID == Convert.ToInt32(fila.Cells[5].Value) select a;
                var jugadores_equipo_1 = (from a in db_sindicato.jugadores
                                          where a.JUG_EQUIPOID == partidos.Single().PARTIDOEQUIPO1
                                          join ju in db_sindicato.maesoc on a.JUG_SOCCENCUIL equals ju.MAESOC_CUIL
                                          join fo in db_sindicato.fotos on a.JUG_SOCCENCUIL equals fo.FOTOS_CUIL
                                          where fo.FOTOS_CODFLIAR == 0
                                          select new
                                          {
                                              campeonato = db_sindicato.campeonatos.Where(x => x.CAMPID == partidos.Single().PARTIDO_CAMPID).Single().CAMPNOMBRE,
                                              equipo = fila.Cells[1].Value.ToString(),
                                              fecha = partidos.Single().PARTIDOFECHA,
                                              hora = partidos.Single().PARTIDOHORA,
                                              fase = db_sindicato.fases.Where(x => x.FASE_ID == partidos.Single().PARTIDO_FASEID).Single().FASENOMBRE,
                                              categoria = fila.Cells[4].Value.ToString(),
                                              cancha = db_sindicato.canchas.Where(x => x.CANCHAID == partidos.Single().PARTIDO_CANCHAID).Single().CANCHANOMBRE,
                                              partido = partidos.Single().PARTIDOID,
                                              nrosocio = ju.MAESOC_NROAFIL,
                                              apellido = ju.MAESOC_APELLIDO.Trim(),
                                              nombre = ju.MAESOC_NOMBRE.Trim(),
                                              dni = ju.MAESOC_NRODOC,
                                              foto =  db_sindicato.fotos.Where(x=>x.FOTOS_CUIL == ju.MAESOC_CUIL && x.FOTOS_CODFLIAR == 0).Count() > 0 ? fo.FOTOS_FOTO:null,
                                              nrofecha = partidos.Single().PARTIDONROFECHA
                                              //sancion_x_de = db_sindicato.sanciones.Where(x=>x.FECHA_PARTIDO == partidos.Single().PARTIDOFECHA).Single().NRO_FECHA.ToString(),
                                              //sancion_numero = db_sindicato.sanciones.Where(x => x.FECHA_PARTIDO == partidos.Single().PARTIDOFECHA).Single().CANTIDAD_FECHAS.ToString()
                                          }).OrderBy(x => x.apellido);

                var jugadores_equipo_2 = (from a in db_sindicato.jugadores
                                          where a.JUG_EQUIPOID == partidos.Single().PARTIDOEQUIPO2
                                          join ju in db_sindicato.maesoc on a.JUG_SOCCENCUIL equals ju.MAESOC_CUIL
                                          join fo in db_sindicato.fotos on a.JUG_SOCCENCUIL equals fo.FOTOS_CUIL
                                          where fo.FOTOS_CODFLIAR == 0
                                          select new
                                          {
                                              campeonato = db_sindicato.campeonatos.Where(x => x.CAMPID == partidos.Single().PARTIDO_CAMPID).Single().CAMPNOMBRE,
                                              equipo = fila.Cells[3].Value.ToString(),
                                              fecha = partidos.Single().PARTIDOFECHA,
                                              hora = partidos.Single().PARTIDOHORA,
                                              fase = db_sindicato.fases.Where(x => x.FASE_ID == partidos.Single().PARTIDO_FASEID).Single().FASENOMBRE,
                                              categoria = fila.Cells[4].Value.ToString(),
                                              cancha = db_sindicato.canchas.Where(x => x.CANCHAID == partidos.Single().PARTIDO_CANCHAID).Single().CANCHANOMBRE,
                                              partido = partidos.Single().PARTIDOID,
                                              nrosocio = ju.MAESOC_NROAFIL,
                                              apellido = ju.MAESOC_APELLIDO.Trim(),
                                              nombre = ju.MAESOC_NOMBRE.Trim(),
                                              dni = ju.MAESOC_NRODOC,
                                              foto = fo.FOTOS_FOTO,
                                              nrofecha = partidos.Single().PARTIDONROFECHA
                                          }).OrderBy(x => x.apellido);



                foreach (var item in jugadores_equipo_1)
                {
                    impresion_comprobante imp = new impresion_comprobante();
                    imp.CAMPEONATO = item.campeonato;
                    imp.EQUIPO = item.equipo;
                    imp.FECHA = item.fecha;
                    imp.HORA = item.hora.ToString();
                    imp.FASE = "-"; // item.fase;
                    imp.CATEGORIA = item.categoria;
                    imp.CANCHA = item.cancha;
                    imp.PARTIDOID = Convert.ToString(item.partido);
                    imp.COL1NROSOCIO = item.nrosocio;
                    imp.COL1NOMBRE = item.apellido + " " + item.nombre;
                    imp.COL1DNI = item.dni;
                    imp.COL1FOTO = item.foto;
                    imp.NROFECHA = "SEMIFINALES";//item.nrofecha.ToString();
                    imp.sancion_x_de = nro_sancion(item.fecha, item.nrosocio);
                    imp.sancion_cantidad = cantidad_fechas_sancion(item.fecha, item.nrosocio);
                    db_sindicato.impresion_comprobante.InsertOnSubmit(imp);
                    db_sindicato.SubmitChanges();
                }


                foreach (var item in jugadores_equipo_2)
                {
                    impresion_comprobante imp = new impresion_comprobante();
                    imp.CAMPEONATO = item.campeonato;
                    imp.EQUIPO = item.equipo;
                    imp.FECHA = item.fecha;
                    imp.HORA = item.hora.ToString();
                    imp.FASE = "-"; //item.fase;
                    imp.CATEGORIA = item.categoria;
                    imp.CANCHA = item.cancha;
                    imp.PARTIDOID = Convert.ToString(item.partido);
                    imp.COL1NROSOCIO = item.nrosocio;
                    imp.COL1NOMBRE = item.apellido + " " + item.nombre;
                    imp.COL1DNI = item.dni;
                    imp.COL1FOTO = item.foto;
                    imp.NROFECHA = "SEMIFINALES"; //item.nrofecha.ToString();
                    imp.sancion_x_de = nro_sancion(item.fecha, item.nrosocio);
                    imp.sancion_cantidad = cantidad_fechas_sancion(item.fecha, item.nrosocio);
                    db_sindicato.impresion_comprobante.InsertOnSubmit(imp);
                    db_sindicato.SubmitChanges();
                }
            }

            // Para el dgv_partidos_chancha_2
            foreach (DataGridViewRow fila in dgv_partidos_cancha2.Rows)
            {
                //impresion_comprobante imp = new impresion_comprobante();
                var partidos = from a in db_sindicato.partidos where a.PARTIDOID == Convert.ToInt32(fila.Cells[5].Value) select a;
                var jugadores_equipo_1 = (from a in db_sindicato.jugadores
                                          where a.JUG_EQUIPOID == partidos.Single().PARTIDOEQUIPO1
                                          join ju in db_sindicato.maesoc on a.JUG_SOCCENCUIL equals ju.MAESOC_CUIL
                                          join fo in db_sindicato.fotos on a.JUG_SOCCENCUIL equals fo.FOTOS_CUIL 
                                          where fo.FOTOS_CODFLIAR == 0
                                          select new
                                          {
                                              campeonato = db_sindicato.campeonatos.Where(x => x.CAMPID == partidos.Single().PARTIDO_CAMPID).Single().CAMPNOMBRE,
                                              equipo = fila.Cells[1].Value.ToString(),
                                              fecha = partidos.Single().PARTIDOFECHA,
                                              hora = partidos.Single().PARTIDOHORA,
                                              fase = db_sindicato.fases.Where(x => x.FASE_ID == partidos.Single().PARTIDO_FASEID).Single().FASENOMBRE,
                                              categoria = fila.Cells[4].Value.ToString(),
                                              cancha = db_sindicato.canchas.Where(x => x.CANCHAID == partidos.Single().PARTIDO_CANCHAID).Single().CANCHANOMBRE,
                                              partido = partidos.Single().PARTIDOID,
                                              nrosocio = ju.MAESOC_NROAFIL,
                                              apellido = ju.MAESOC_APELLIDO.Trim(),
                                              nombre = ju.MAESOC_NOMBRE.Trim(),
                                              dni = ju.MAESOC_NRODOC,
                                              foto = fo.FOTOS_FOTO,
                                              nrofecha = partidos.Single().PARTIDONROFECHA
                                          }).OrderBy(x => x.apellido);

                var jugadores_equipo_2 = (from a in db_sindicato.jugadores
                                          where a.JUG_EQUIPOID == partidos.Single().PARTIDOEQUIPO2
                                          join ju in db_sindicato.maesoc on a.JUG_SOCCENCUIL equals ju.MAESOC_CUIL
                                          join fo in db_sindicato.fotos on ju.MAESOC_CUIL equals fo.FOTOS_CUIL 
                                          where fo.FOTOS_CODFLIAR == 0
                                          select new
                                          {
                                              campeonato = db_sindicato.campeonatos.Where(x => x.CAMPID == partidos.Single().PARTIDO_CAMPID).Single().CAMPNOMBRE,
                                              equipo = fila.Cells[3].Value.ToString(),
                                              fecha = partidos.Single().PARTIDOFECHA,
                                              hora = partidos.Single().PARTIDOHORA,
                                              fase = db_sindicato.fases.Where(x => x.FASE_ID == partidos.Single().PARTIDO_FASEID).Single().FASENOMBRE,
                                              categoria = fila.Cells[4].Value.ToString(),
                                              cancha = db_sindicato.canchas.Where(x => x.CANCHAID == partidos.Single().PARTIDO_CANCHAID).Single().CANCHANOMBRE,
                                              partido = partidos.Single().PARTIDOID,
                                              nrosocio = ju.MAESOC_NROAFIL,
                                              apellido = ju.MAESOC_APELLIDO.Trim(),
                                              nombre = ju.MAESOC_NOMBRE.Trim(),
                                              dni = ju.MAESOC_NRODOC,
                                              foto = fo.FOTOS_FOTO,
                                              nrofecha = partidos.Single().PARTIDONROFECHA
                                          }).OrderBy(x => x.apellido);



                foreach (var item in jugadores_equipo_1)
                {
                    impresion_comprobante imp = new impresion_comprobante();
                    imp.CAMPEONATO = item.campeonato;
                    imp.EQUIPO = item.equipo;
                    imp.FECHA = item.fecha;
                    imp.HORA = item.hora.ToString();
                    imp.FASE = "-";//item.fase;
                    imp.CATEGORIA = item.categoria;
                    imp.CANCHA = item.cancha;
                    imp.PARTIDOID = Convert.ToString(item.partido);
                    imp.COL1NROSOCIO = item.nrosocio;
                    imp.COL1NOMBRE = item.apellido + " " + item.nombre;
                    imp.COL1DNI = item.dni;
                    imp.COL1FOTO = item.foto;
                    imp.NROFECHA = "SEMIFINALES";//item.nrofecha.ToString();
                    imp.sancion_x_de = nro_sancion(item.fecha, item.nrosocio);
                    imp.sancion_cantidad = cantidad_fechas_sancion(item.fecha, item.nrosocio);
                    db_sindicato.impresion_comprobante.InsertOnSubmit(imp);
                    db_sindicato.SubmitChanges();
                }


                foreach (var item in jugadores_equipo_2)
                {
                    impresion_comprobante imp = new impresion_comprobante();
                    imp.CAMPEONATO = item.campeonato;
                    imp.EQUIPO = item.equipo;
                    imp.FECHA = item.fecha;
                    imp.HORA = item.hora.ToString();
                    imp.FASE = "-";//item.fase;
                    imp.CATEGORIA = item.categoria;
                    imp.CANCHA = item.cancha;
                    imp.PARTIDOID = Convert.ToString(item.partido);
                    imp.COL1NROSOCIO = item.nrosocio;
                    imp.COL1NOMBRE = item.apellido + " " + item.nombre;
                    imp.COL1DNI = item.dni;
                    imp.COL1FOTO = item.foto;
                    imp.NROFECHA = "SEMIFINALES"; // item.nrofecha.ToString();
                    imp.sancion_x_de = nro_sancion(item.fecha, item.nrosocio);
                    imp.sancion_cantidad = cantidad_fechas_sancion(item.fecha, item.nrosocio);
                    db_sindicato.impresion_comprobante.InsertOnSubmit(imp);
                    db_sindicato.SubmitChanges();
                }
            }

            reportes frm_reportes = new reportes();
            frm_reportes.nombreReporte = "planilla_partidos";
            frm_reportes.Show();
        }

        private string nro_sancion (DateTime fecha_partido, string id_jugador)
        {
            var nrosanc = from a in db_sindicato.sanciones where a.FECHA_PARTIDO.Date == fecha_partido.Date && a.ID_JUG == Convert.ToInt32(id_jugador.Trim()) select a;
            if (nrosanc.Count() > 0)
            {

                return nrosanc.Single().NRO_FECHA.ToString();
            }
            else
            {
                return "";
            }
        }


        private string cantidad_fechas_sancion(DateTime fecha_partido, string id_jugador)
        {
            var nrosanc = from a in db_sindicato.sanciones where a.FECHA_PARTIDO.Date == fecha_partido.Date && a.ID_JUG == Convert.ToInt32(id_jugador) select a;
            if (nrosanc.Count() > 0)
            {

                return nrosanc.Single().CANTIDAD_FECHAS.ToString();
            }
            else
            {
                return "";
            }
        }

        private fotos buscar_foto(float cuil )
        {
            var f = db_sindicato.fotos.Where(x => x.FOTOS_CUIL == cuil && x.FOTOS_CODFLIAR == 0);
            if (f.Count() > 0 )  return (fotos) f;
            else
            {
                return (fotos)f;
            }
        }

        private void btn_planilla_informe_arbitral_Click(object sender, EventArgs e)
        {
            reportes f_report = new reportes();
            f_report.nombreReporte = "planilla_arbitro";
            f_report.Show();
        }

        private void pnl_jugadoresInscriptos_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_guardar_cambio_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de cambiar de equipo al jugador ''" + dgv_jugadores_inscriptos.CurrentRow.Cells[2].Value.ToString() + "''","¡¡¡ ATENCION !!!",MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                var jug = db_sindicato.jugadores.Where(x => x.JUGID == Convert.ToInt32(dgv_jugadores_inscriptos.CurrentRow.Cells[0].Value) && x.JUG_EQUIPOID == Convert.ToInt32(cbx_equipo_jugadores_inscriptos.SelectedValue)).Single();
                jug.JUG_EQUIPOID = Convert.ToInt32(cbx_cambio_equipo.SelectedValue);
                db_sindicato.SubmitChanges();
                mostrar_jugadores_inscriptos();
            }
            
        }

        private void btn_quitar_jugador_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de quitar del equipo " + cbx_cambio_equipo.Text + " al jugador ''" + dgv_jugadores_inscriptos.CurrentRow.Cells[2].Value.ToString() + "''", "¡¡¡ ATENCION !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var jug = db_sindicato.jugadores.Where(x => x.JUGID == Convert.ToInt32(dgv_jugadores_inscriptos.CurrentRow.Cells[0].Value) && x.JUG_EQUIPOID == Convert.ToInt32(cbx_equipo_jugadores_inscriptos.SelectedValue)).Single();
                jug.JUG_EQUIPOID = 0;
                db_sindicato.SubmitChanges();
                mostrar_jugadores_inscriptos();
                //db_sindicato.ExecuteCommand("delete from jugadores where jugid = " + jug.JUGID);
            }
        }

        private void btn_crear_fixture_Click(object sender, EventArgs e)
        {
            var equipos_para_sorteo = from a in db_sindicato.campequipo where a.CAMP_CAMPID == Convert.ToInt32(cbx_campeonato_partidos.SelectedValue) select a;
        }

        private void btn_aplicar_sancion_Click(object sender, EventArgs e)
        {

        }

        private void btn_aplicar_sancion_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de aplicar Sancion al jugador ''" + dgv_jugadores_inscriptos.CurrentRow.Cells[2].Value.ToString() + "''", "¡¡¡ ATENCION !!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // for que va desde 0 hasta la cantidad de fechas. 
                DateTime f = dtp_sancion_desde.Value;
                for (int i = 0; i < Convert.ToInt32(cbx_cantidad_fechas.SelectedItem.ToString()); i++)
                {
                    
                    sanciones sanci = new sanciones();
                    sanci.ID_JUG = Convert.ToInt32(dgv_jugadores_inscriptos.CurrentRow.Cells["nroafil"].Value);
                    sanci.FECHA_DESDE = dtp_sancion_desde.Value;
                    sanci.FECHA_HASTA = dtp_sancion_hasta.Value;
                    sanci.CANTIDAD_FECHAS = Convert.ToInt32(cbx_cantidad_fechas.SelectedItem.ToString());
                    sanci.NRO_FECHA = i + 1;
                    sanci.FECHA_PARTIDO = f;
                    f = f.AddDays(7);
                    db_sindicato.sanciones.InsertOnSubmit(sanci);
                    db_sindicato.SubmitChanges();
                }
            }
        }
    }
}
