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
    public partial class frm_quinchos : Form
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
    
        
        public frm_quinchos()
        {
            InitializeComponent();
        }

        private void frm_quinchos_Load(object sender, EventArgs e)
        {
            cargar_cbx_eventos();
            mostrar_reservas();
        }

        private void cargar_cbx_eventos()
        {

            var eventos = from eve in db_sindicato.eventos.OrderBy(x => x.eventos_nombre) select new
            {
                eventos_nom = eve.eventos_nombre,
                eventos_id = eve.eventos_id
            };
                        
            cbx_eventos.DisplayMember = "eventos_nom";
            cbx_eventos.ValueMember = "eventos_id";
            cbx_eventos.DataSource = eventos.ToList();
        }

        private void dtp_consulta_ValueChanged(object sender, EventArgs e)
        {
            mostrar_reservas();
        }

        private void mostrar_reservas()
        {
            while (dgv_reservados.Rows.Count > 0)
            {
                dgv_reservados.Rows.RemoveAt(0);
            }
            var quinchos = db_sindicato.quinchos;

            foreach (var item in quinchos)
            {
                var reservas = from res in db_sindicato.reservas_quinchos
                               where res.reservas_fecha == dtp_consulta.Value && res.reservas_quinchos_id == item.quinchos_id
                               //join qui in db_sindicato.quinchos on res.reservas_quinchos_id equals qui.quinchos_id
                               select new
                               {
                                   quincho_id = res.reservas_quinchos_id,
                                   reserva_ID = res.reservas_id,
                                   quincho = item.quinchos_nombre,//  qui.quinchos_nombre,
                                   horario = item.quinchos_turno,//qui.quinchos_turno,
                                   estado = "Reservado",
                                   capacidad = item.quinchos_capacidad, //quinchos_capacidad,
                                   cuil = res.reservas_socios_id //db_sindicato.maesoc.Where(x=>x.MAESOC_CUIL == res.reservas_socios_id).Single().MAESOC_APELLIDO.Trim() + " " + db_sindicato.maesoc.Where(x => x.MAESOC_CUIL == res.reservas_socios_id).Single().MAESOC_NOMBRE.Trim()
                               };

                dgv_reservados.Rows.Add();
                int fila = dgv_reservados.Rows.Count - 1;
                if (reservas.Count() > 0)
                {
                    if (reservas.Count() > 1)
                    {
                        //quiere decir que ese quincho tiene varias reservas para ese dia entonces habilito el datagrid con los socios que alquilaron 
                        dgv_reservados.Rows[fila].Cells[0].Value = reservas.First().quincho;
                        dgv_reservados.Rows[fila].Cells[1].Value = reservas.First().horario;
                        dgv_reservados.Rows[fila].Cells[2].Value = reservas.First().estado;
                        dgv_reservados.Rows[fila].Cells[2].Style.BackColor = Color.Tomato;
                        dgv_reservados.Rows[fila].Cells[3].Value = reservas.First().capacidad;
                        dgv_reservados.Rows[fila].Cells[4].Value = reservas.First().quincho_id;
                        dgv_reservados.Rows[fila].Cells[5].Value = 0;//reservas.First().reserva_ID; // cero para indicar que tiene varias reservas ese dia
                        dgv_reservados.Rows[fila].Cells[6].Value = reservas.First().cuil;
                    }
                    else //igual a 1
                    {
                        //quiere decir que tiene solo una reserva para ese dia
                        dgv_reservados.Rows[fila].Cells[0].Value = reservas.Single().quincho;
                        dgv_reservados.Rows[fila].Cells[1].Value = reservas.Single().horario;
                        dgv_reservados.Rows[fila].Cells[2].Value = reservas.Single().estado;
                        dgv_reservados.Rows[fila].Cells[2].Style.BackColor = Color.Tomato;
                        dgv_reservados.Rows[fila].Cells[3].Value = reservas.Single().capacidad;
                        dgv_reservados.Rows[fila].Cells[4].Value = reservas.Single().quincho_id;
                        dgv_reservados.Rows[fila].Cells[5].Value = reservas.Single().reserva_ID;
                        dgv_reservados.Rows[fila].Cells[6].Value = reservas.Single().cuil;
                    }
                }
                else
                {
                    //no tiene reservas para ese dia
                    dgv_reservados.Rows[fila].Cells[0].Value = item.quinchos_nombre;
                    dgv_reservados.Rows[fila].Cells[1].Value = item.quinchos_turno;
                    dgv_reservados.Rows[fila].Cells[2].Value = "Libre";
                    dgv_reservados.Rows[fila].Cells[2].Style.BackColor = Color.GreenYellow;
                    dgv_reservados.Rows[fila].Cells[3].Value = item.quinchos_capacidad;
                    dgv_reservados.Rows[fila].Cells[4].Value = item.quinchos_id;
                }
            }
        }

        private void dgv_reservados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_reservados.Rows.Count > 0)
            {
                if (Convert.ToString(dgv_reservados.CurrentRow.Cells[2].Value) == "Reservado")
                {
                    if (Convert.ToInt32(dgv_reservados.CurrentRow.Cells[5].Value.ToString()) == 0) // para saber si tiene varias reservas ese mismo dia
                    {
                        dgv_multi_reserva.Enabled = true;
                        while (dgv_multi_reserva.Rows.Count > 0) // limpio el datagrid
                        {
                            dgv_multi_reserva.Rows.RemoveAt(0);
                        }
                        var varias_resevas = from vr in db_sindicato.reservas_quinchos
                                             where vr.reservas_fecha == dtp_consulta.Value && vr.reservas_quinchos_id == Convert.ToInt32(dgv_reservados.CurrentRow.Cells[4].Value)
                                             select new
                                             {
                                                socio_apenom = db_sindicato.maesoc.Where(x=>x.MAESOC_CUIL == vr.reservas_socios_id).Single().MAESOC_APELLIDO.Trim() + " " +
                                                                db_sindicato.maesoc.Where(x => x.MAESOC_CUIL == vr.reservas_socios_id).Single().MAESOC_NOMBRE.Trim(),
                                                vr.reservas_id,
                                                vr.reservas_quinchos_id,
                                                vr.reservas_socios_id
                                             };
                       
                        foreach (var item in varias_resevas) // recorro la variable que tiene toas las reservas
                        {
                            dgv_multi_reserva.Rows.Add();
                            int fila = dgv_multi_reserva.Rows.Count - 1;
                            dgv_multi_reserva.Rows[fila].Cells[0].Value = item.socio_apenom;
                            dgv_multi_reserva.Rows[fila].Cells[1].Value = item.reservas_id;
                            dgv_multi_reserva.Rows[fila].Cells[2].Value = item.reservas_quinchos_id;
                            dgv_multi_reserva.Rows[fila].Cells[3].Value = item.reservas_socios_id;
                        }
                        dgv_multi_reserva.Rows.Add();
                        dgv_multi_reserva.Rows[dgv_multi_reserva.Rows.Count - 1].Cells[0].Value = "LIBRE";
                        dgv_multi_reserva.Rows[dgv_multi_reserva.Rows.Count - 1].Cells[0].Style.BackColor = Color.GreenYellow;
                    }
                    else
                    {
                        mostrar_datos_reserva(Convert.ToDouble(dgv_reservados.CurrentRow.Cells[6].Value.ToString()), Convert.ToInt32(dgv_reservados.CurrentRow.Cells[5].Value));
                    }
                }
                else
                {
                    limpiar_controles();
                }
            }
        }

        public void limpiar_controles ()
        {
            picbox_socio.Image = null;
            btn_sin_imagen.Visible = true;
            lbl_datos_reserva.Text = "Libre";
            cbx_eventos.SelectedValue = 0; // Posiciona en el combobox el evento que corresponde a la reserva 
            txt_invitados.Text = "";
            txt_tenedor.Text = "";
            txt_cuchillo.Text = "";
            txt_vasos.Text = "";
            txt_costo.Text = "";
            txt_observaciones.Text = "";
            txt_socio.Text = "";
            btn_reservar.Enabled = true;
        }

        public void mostrar_datos_reserva(double cuil, int reserva_id)
        {
            // Mostrar foto
            var foto = db_sindicato.fotos.Where(x => x.FOTOS_CUIL == cuil && x.FOTOS_CODFLIAR == 0); ///Convert.ToDouble(dgv_reservados.CurrentRow.Cells[6].Value.ToString()
            if (foto.Count() > 0)
            {
                picbox_socio.Image = ByteArrayToImage(foto.Single().FOTOS_FOTO.ToArray());
                btn_sin_imagen.Visible = false;
            }
            else
            {
                picbox_socio.Image = null;
                btn_sin_imagen.Visible = true;
            }
            lbl_datos_reserva.Text = "Datos de la Reserva";
            var datos_reserva = db_sindicato.reservas_quinchos.Where(x => x.reservas_id == reserva_id).Single();// Convert.ToInt32(dgv_reservados.CurrentRow.Cells[5].Value)).Single();
            cbx_eventos.SelectedValue = datos_reserva.reservas_evento; // Posiciona en el combobox el evento que corresponde a la reserva 
            txt_invitados.Text = datos_reserva.reservas_invitados.ToString();
            txt_tenedor.Text = datos_reserva.reservas_tenedor.ToString();
            txt_cuchillo.Text = datos_reserva.reservas_cuchillos.ToString();
            txt_vasos.Text = datos_reserva.reservas_vasos.ToString();
            txt_costo.Text = datos_reserva.reservas_costo.ToString();
            txt_observaciones.Text = datos_reserva.reservas_comentario;
            var so = db_sindicato.maesoc.Where(x => x.MAESOC_CUIL == datos_reserva.reservas_socios_id).Single();
            txt_socio.Text = so.MAESOC_APELLIDO.Trim() + " " + so.MAESOC_NOMBRE.Trim();
            btn_reservar.Enabled = false;
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void btn_reservar_Click(object sender, EventArgs e)
        {
            try
            {
                reservas_quinchos res_qui = new reservas_quinchos();
                res_qui.reservas_fecha = dtp_consulta.Value;
                res_qui.reservas_quinchos_id = Convert.ToInt32(dgv_reservados.CurrentRow.Cells[4].Value);// Convert.ToInt32(cbx_quinchos.SelectedValue);
                res_qui.reservas_socios_id = Convert.ToDouble(lbl_cuil.Text);
                res_qui.reservas_evento = Convert.ToInt32(cbx_eventos.SelectedValue);
                res_qui.reservas_invitados = Convert.ToInt32(txt_invitados.Text);
                res_qui.reservas_tenedor = Convert.ToInt32(txt_tenedor.Text);
                res_qui.reservas_vasos = Convert.ToInt32(txt_vasos.Text);
                res_qui.reservas_cuchillos = Convert.ToInt32(txt_cuchillo.Text);
                res_qui.reservas_comentario = txt_observaciones.Text;
                res_qui.reservas_costo = Convert.ToDouble(txt_costo.Text);
                db_sindicato.reservas_quinchos.InsertOnSubmit(res_qui);
                db_sindicato.SubmitChanges();
                frm_dialog f_dialog = new frm_dialog();
                f_dialog.lbl_detalle.Text = "La Reserva de '" + dgv_reservados.CurrentRow.Cells[0].Value.ToString() +
                                            "' para el cliente '" + lbl_socio.Text + "' fue reservada con exito " +
                                            " para el dia '" + dtp_consulta.Value.ToString("dd/MM/yyyy") + "'";
                f_dialog.Show();
                mostrar_reservas();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
           
        }

      
        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_invitados_OnValueChanged(object sender, EventArgs e)
        {
            txt_tenedor.Text = txt_invitados.Text;
            txt_cuchillo.Text = txt_invitados.Text;
            txt_vasos.Text = txt_invitados.Text;
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbx_eventos_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_costo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                txt_invitados.Focus();
            }
        }

        private void dgv_multi_reserva_SelectionChanged(object sender, EventArgs e)
        {
        //    if (dgv_multi_reserva.Rows.Count > 0)
        //    {
                            
        //        if (Convert.ToString(dgv_multi_reserva.CurrentRow.Cells[0].Value) == "LIBRE")
        //        {
        //            btn_reservar.Enabled = true;
        //            limpiar_controles();
        //        }
        //        else
        //        {
        //            mostrar_datos_reserva(Convert.ToDouble(dgv_multi_reserva.CurrentRow.Cells[3].Value), Convert.ToInt16(dgv_multi_reserva.CurrentRow.Cells[1].Value));
        //            btn_reservar.Enabled = false;
        //        }
        //    }
        }
    }
}
