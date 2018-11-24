using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using entrega_cupones.Clases;

namespace entrega_cupones
{
    public partial class frm_buscar_empresa : Form
    {
        #region codigo para efecto shadow

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

        #endregion

        public delegate void PasarDatos(string empresa, string domicilio, string cuit, string estudio, string telefono, string localidad);
        public event PasarDatos DatosPasados;


        public frm_buscar_empresa()
        {
            InitializeComponent();
        }
        lts_sindicatoDataContext DB_Sindicato = new lts_sindicatoDataContext();

        private void btn_cerrar_busqueda_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_buscar_empresa_Load(object sender, EventArgs e)
        {
            txt_buscar_empresa.Focus();
            mostrar_todos();

        }

        private void mostrar_todos()
        {

            var empresas = (from a in DB_Sindicato.maeemp
                            select new
                            {
                                _CUIT = a.MAEEMP_CUIT,
                                _razonsocial = a.MAEEMP_RAZSOC,
                                _nombre_fantasia = a.MAEEMP_NOMFAN,
                                //_deuda_aproximada =  calcular_coeficientes()
                            }).OrderBy(x => x._razonsocial);
            dgv_buscar_empresas.DataSource = empresas.ToList();
            lbl_cantidad_empresas_encontradas.Text = "Total Empresas: " + empresas.Count().ToString();

        }

        private void txt_buscar_empresa_TextChanged(object sender, EventArgs e)
        {
            var empresas = (from a in DB_Sindicato.maeemp
                                // where a.cuitstr.Contains(txt_buscar_empresa.Text.Trim()) ||
                            where Convert.ToString(a.MEEMP_CUIT_STR).Contains(txt_buscar_empresa.Text.Trim()) ||
                            a.MAEEMP_RAZSOC.Contains(txt_buscar_empresa.Text) ||
                            a.MAEEMP_NOMFAN.Contains(txt_buscar_empresa.Text)//.Where( x => Convert.ToString(x.cuit).Any(x.cuit.ToString().Contains(txt_buscar_empresa.Text)))// .Contains(txt_buscar_empresa.Text) //  .Hierarchy.Any(x.cuit.ToString().Contains(txt_buscar_empresa.Text)) // .Contains(txt_buscar_empresa.Text))
                            select new
                            {
                                _CUIT = a.MAEEMP_CUIT,
                                _razonsocial = a.MAEEMP_RAZSOC,
                                _nombre_fantasia = a.MAEEMP_NOMFAN
                            }

                           ).OrderBy(x => x._razonsocial);
            dgv_buscar_empresas.DataSource = empresas.ToList();
            lbl_cantidad_empresas_encontradas.Text = "Total Empresas: " + empresas.Count().ToString();
        }

        private void dgv_buscar_empresas_SelectionChanged(object sender, EventArgs e)
        {
            var mostar_datos = (from a in DB_Sindicato.maeemp.Where(x => x.MAEEMP_CUIT == Convert.ToDouble(dgv_buscar_empresas.CurrentRow.Cells["cuit"].Value))

                                select new
                                {
                                    _domicilio = a.MAEEMP_CALLE.Trim() + " Nº: " + a.MAEEMP_NRO.Trim(),
                                    _telefono = a.MAEEMP_TEL.Trim(),
                                    _localidad = DB_Sindicato.localidad.Where(x => x.MAELOC_CODLOC == a.MAEEMP_CODLOC).First().MAELOC_NOMBRE,
                                    _estudio = a.MAEEMP_ESTUDIO_CONTACTO.Trim() + " " + a.MAEEMP_ESTUDIO_TEL.Trim()
                                }).FirstOrDefault();
            txt_buscar_empresa_domicilio.Text = mostar_datos._domicilio;
            txt_buscar_empresa_telefono.Text = mostar_datos._telefono;
            txt_localidad.Text = mostar_datos._localidad;
            txt_buscar_empresa_estudio.Text = mostar_datos._estudio;
        }

        private void btn_estado_empresa_Click(object sender, EventArgs e)
        {
            if (dgv_buscar_empresas.Rows.Count > 0)
            {
                pasar_datos();
                this.Close();
            }
        }

        private void txt_buscar_empresa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dgv_buscar_empresas.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (dgv_buscar_empresas.Rows.Count > 0)
                {
                    pasar_datos();
                    this.Close();
                }
            }
        }


        private void pasar_datos()
        {
            DatosPasados(
                dgv_buscar_empresas.CurrentRow.Cells["razonsocial"].Value.ToString(),
                txt_buscar_empresa_domicilio.Text,
                dgv_buscar_empresas.CurrentRow.Cells["cuit"].Value.ToString(),
                txt_buscar_empresa_estudio.Text,
                txt_buscar_empresa_telefono.Text,
                txt_localidad.Text
            );
        }

        private void dgv_buscar_empresas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pasar_datos();
                this.Close();
            }
        }

        private void dgv_buscar_empresas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pasar_datos();
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
