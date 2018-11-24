using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using entrega_cupones.Reportes;
using entrega_cupones.Clases;




namespace entrega_cupones
{
    public partial class frm_actas : Form
    {
        lts_sindicatoDataContext db_sindicato = new lts_sindicatoDataContext();

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

        public frm_actas()
        {
            InitializeComponent();
        }

        private void frm_actas_Load(object sender, EventArgs e)
        {
            cargar_cbx_actas_inspectores();
            cargar_seguimiento();
            dgv_cobros.AutoGenerateColumns = false;
        }

        private void cargar_cbx_actas_inspectores()

        {
            var inspector = (from inspec in db_sindicato.inspectores select inspec).OrderBy(x => x.APELLIDO);

            cbx_actas_inspectores.DisplayMember = "apellido";
            cbx_actas_inspectores.ValueMember = "id_inspector";
            cbx_actas_inspectores.DataSource = inspector.ToList();
        }

        private void btn_cerrar_actas_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_actas_buscar_empresa_Click(object sender, EventArgs e)
        {
            frm_buscar_empresa f_busc_emp = new frm_buscar_empresa();
            f_busc_emp.DatosPasados += new frm_buscar_empresa.PasarDatos(ejecutar);
            f_busc_emp.ShowDialog();
            // txt_actas_empresa.Text = empresa;
        }

        public void ejecutar(string empresa, string domicilio, string cuit, string estudio, string telefono, string localidad)
        {
            txt_actas_empresa.Text = empresa;
            txt_actas_domicilio.Text = domicilio;
            txt_actas_cuit.Text = cuit;
            txt_actas_estudio.Text = estudio;
            txt_actas_telefono.Text = telefono;
            txt_actas_localidad.Text = localidad;
        }

        private void btn_verificar_periodo_Click(object sender, EventArgs e)
        {
            if (txt_actas_cuit.Text != "")
            {
                DateTime desde = Convert.ToDateTime("01/" + txt_actas_desde.Text);
                DateTime hasta = Convert.ToDateTime("01/" + txt_actas_hasta.Text);
                if (desde <= hasta)
                {
                    // Vaciar dgv_actas_dj_empresas
                    while (dgv_actas_dj_empresa.Rows.Count != 0)
                    {
                        dgv_actas_dj_empresa.Rows.RemoveAt(0);
                    }


                    var dj_empresa = (from a in db_sindicato.ddjjt
                                      where (a.periodo >= desde && a.periodo <= hasta) && (a.cuit == Convert.ToInt64(txt_actas_cuit.Text))
                                      select new
                                      {
                                          periodo = a.periodo,
                                          rectifi = a.rect,
                                          aporteley = a.titem1,
                                          aportesocio = a.titem2,
                                          fpago = a.fpago,
                                          importe_depositado = a.impban1 + a.impban2,
                                          impban1 = a.impban1,
                                          total = a.impban1,
                                          capital = a.titem1 + a.titem2
                                      }).ToList().OrderBy(x => x.periodo);
                    //cargar el DGV con los periodos encontrados
                    foreach (var item in dj_empresa)
                    {
                        dgv_actas_dj_empresa.Rows.Add();
                        int fila = dgv_actas_dj_empresa.Rows.Count - 1;
                        dgv_actas_dj_empresa.Rows[fila].Cells["periodo"].Value = item.periodo;//.ToString().Substring(3,7);
                        dgv_actas_dj_empresa.Rows[fila].Cells["rectificacion"].Value = item.rectifi;
                        dgv_actas_dj_empresa.Rows[fila].Cells["aporte_ley"].Value = item.aporteley;
                        dgv_actas_dj_empresa.Rows[fila].Cells["aporte_socio"].Value = item.aportesocio;
                        dgv_actas_dj_empresa.Rows[fila].Cells["fecha_pago"].Value = (item.fpago == null) ? string.Empty : item.fpago.ToString().Substring(0, 10);
                        dgv_actas_dj_empresa.Rows[fila].Cells["importe_depositado"].Value = item.impban1;
                        dgv_actas_dj_empresa.Rows[fila].Cells["capital"].Value = (item.capital == 0 && item.importe_depositado != 0) ? item.impban1 : item.capital;
                        dgv_actas_dj_empresa.Rows[fila].Cells["total"].Value = (item.fpago != null) ? calcular_coeficiente_A(Convert.ToDateTime(item.periodo), Convert.ToDateTime(item.fpago), Convert.ToDouble(item.capital), Convert.ToDouble(item.total))
                                                                            : calcular_coeficiente_B(Convert.ToDateTime(item.periodo), Convert.ToDouble(item.capital), Convert.ToDouble(item.total));
                        //variable para el interes
                        double interes = Convert.ToDouble(dgv_actas_dj_empresa.Rows[fila].Cells["total"].Value) - Convert.ToDouble(dgv_actas_dj_empresa.Rows[fila].Cells["capital"].Value);
                        dgv_actas_dj_empresa.Rows[fila].Cells["interes"].Value = (interes < 0) ? (dgv_actas_dj_empresa.Rows[fila].Cells["total"].Value) : (interes);
                    }

                    //txt_actas_total_con_interes.Text = sumar_totales().ToString("c");
                    generar_periodos_faltantes();
                    calcular_cantidad_empleados();
                    totales_de_consulta();
                    mostrar_actas_involucradas();

                }
                else
                {
                    MessageBox.Show("El periodo [ DESDE ] debe ser menor o iguan que el periodo [ HASTA ] ");
                }
            }
            else
            {
                MessageBox.Show("NO hay empresas para verificar los periodos", "ATENCION");
            }
        }

        private double calcular_coeficiente_A(DateTime periodo, DateTime fpago, double tot_periodo, double pagado)
        {
            if (tot_periodo == 0 && pagado != 0)
            {
                tot_periodo = pagado;
            }

            double tot_interes_mora_de_pago = 0;
            double tot_intereses_a_la_fecha = 0;
            double coef_A = 0;
            double coef_B = 0;
            DateTime fecha_vencimiento = periodo.AddMonths(1);
            fecha_vencimiento = fecha_vencimiento.AddDays(14);

            if (fpago > fecha_vencimiento)
            {
                coef_A = Math.Round(((fpago - fecha_vencimiento).TotalDays * 0.001), 5);
                tot_interes_mora_de_pago = tot_periodo - pagado + (pagado * coef_A);
                coef_B = ((Convert.ToDateTime(txt_actas_venc.Text) - fecha_vencimiento).TotalDays * 0.001) + 1;
                tot_intereses_a_la_fecha = ((tot_periodo - pagado) * coef_B) + ((pagado * coef_A) * (coef_B - coef_A));
            }
            return tot_intereses_a_la_fecha; //tot_interes_mora_de_pago;
        }

        private double calcular_coeficiente_B(DateTime periodo, double tot_periodo, double pagado)
        {
            if (tot_periodo == 0 && pagado != 0)
            {
                tot_periodo = pagado;
            }
            double tot_intereses_a_la_fecha = 0;
            double coef_A = 0;
            double coef_B = 0;
            DateTime fecha_vencimiento = periodo.AddMonths(1);
            fecha_vencimiento = fecha_vencimiento.AddDays(14);
            coef_B = ((Convert.ToDateTime(txt_actas_venc.Text) - fecha_vencimiento).TotalDays * 0.001) + 1;
            tot_intereses_a_la_fecha = ((tot_periodo - pagado) * coef_B) + ((pagado * coef_A) * (coef_B - coef_A));

            return tot_intereses_a_la_fecha; //tot_interes_mora_de_pago;
        }

        private void generar_periodos_faltantes()
        {
            // Se genera una lista y se llena con los periodos  del dgv_actas_dj_empresa
            List<DateTime> lista1 = new List<DateTime>();

            foreach (DataGridViewRow fila in dgv_actas_dj_empresa.Rows)
            {
                lista1.Add(Convert.ToDateTime(fila.Cells["periodo"].Value));
            }

            // Se genera una lista y se llena  con los periodos  de la tabla de periodos
            List<DateTime> lista2 = new List<DateTime>();

            foreach (var fila in db_sindicato.secuencia_periodos.Where(x => x.periodo >= Convert.ToDateTime("01/" + txt_actas_desde.Text) && x.periodo <= Convert.ToDateTime("01/" + txt_actas_hasta.Text)))
            {
                lista2.Add(Convert.ToDateTime(fila.periodo));
            }
            // Se genera una variable con los periodos de la lista2 que no esten en la lista1 y obtengo los periodos faltantes
            var periodo_faltante = from p in lista2.Except(lista1) select new { periodo = p.Date };

            int f = 0;
            foreach (var item in periodo_faltante.ToList())
            {
                dgv_actas_dj_empresa.Rows.Add();
                f = dgv_actas_dj_empresa.Rows.Count - 1;
                dgv_actas_dj_empresa.Rows[f].DefaultCellStyle.BackColor = Color.PaleVioletRed;
                dgv_actas_dj_empresa.Rows[f].Cells["periodo"].Value = item.periodo.Date;
                dgv_actas_dj_empresa.Rows[f].Cells["rectificacion"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["aporte_ley"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["aporte_socio"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["fecha_pago"].Value = "NO Declara";
                dgv_actas_dj_empresa.Rows[f].Cells["importe_depositado"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["empleados"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["socios"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["capital"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["interes"].Value = 0;
                dgv_actas_dj_empresa.Rows[f].Cells["total"].Value = 0;
            }

            dgv_actas_dj_empresa.Sort(this.dgv_actas_dj_empresa.Columns["periodo"], ListSortDirection.Ascending);

        }

        private void totales_de_consulta()
        {
            decimal total_periodos = 0, total_NO_declarados = 0, total_pagados = 0, total_NO_pagados = 0;
            decimal total_aporte2 = 0, total_aporte_socios = 0, total_depositado = 0, total_interes = 0, total_con_interes = 0;
            decimal total_capital = 0;

            foreach (DataGridViewRow fila in dgv_actas_dj_empresa.Rows)
            {
                total_periodos = dgv_actas_dj_empresa.Rows.Count;

                if (fila.Cells["fecha_pago"].Value.ToString() == "NO Declara") total_NO_declarados += 1;
                if (fila.Cells["fecha_pago"].Value.ToString() == "") total_NO_pagados += 1;
                if ((fila.Cells["fecha_pago"].Value.ToString() != "") && (fila.Cells["fecha_pago"].Value.ToString() != "NO Declara")) total_pagados += 1;
                total_aporte2 += Convert.ToDecimal(fila.Cells["aporte_ley"].Value);
                total_aporte_socios += Convert.ToDecimal(fila.Cells["aporte_socio"].Value);
                total_depositado += Convert.ToDecimal(fila.Cells["importe_depositado"].Value);
                total_interes += Convert.ToDecimal(fila.Cells["interes"].Value);
                total_capital += Convert.ToDecimal(fila.Cells["capital"].Value);
                total_con_interes += Convert.ToDecimal(fila.Cells["total"].Value);
            }

            txt_actas_total_periodos.Text = total_periodos.ToString();
            txt_actas_no_declarados.Text = total_NO_declarados.ToString();
            txt_actas_pagados.Text = total_pagados.ToString();
            txt_actas_no_pagados.Text = total_NO_pagados.ToString();
            txt_actas_aporte2.Text = total_aporte2.ToString();
            txt_actas_aportes_socio.Text = total_aporte_socios.ToString();
            txt_actas_depositado.Text = total_depositado.ToString("C");
            txt_actas_intereses.Text = total_interes.ToString("C");
            txt_capital.Text = total_capital.ToString("C");
            txt_actas_total_con_interes.Text = total_con_interes.ToString("C");
        }

        private void calcular_cantidad_empleados()
        {
            foreach (DataGridViewRow fila in dgv_actas_dj_empresa.Rows)
            {
                var cant_emp = db_sindicato.ddjj.Where(x => x.cuite == Convert.ToDouble(txt_actas_cuit.Text) && (x.periodo == Convert.ToDateTime(fila.Cells["periodo"].Value)) && x.rect == Convert.ToInt16(fila.Cells["rectificacion"].Value));
                fila.Cells["empleados"].Value = cant_emp.Count(x => x.item1);
                fila.Cells["socios"].Value = cant_emp.Count(x => x.item2);
            }
        }

        private void btn_quitar_periodo_Click(object sender, EventArgs e)
        {
            dgv_actas_dj_empresa.Rows.RemoveAt(dgv_actas_dj_empresa.CurrentRow.Index);
            totales_de_consulta();
        }

        private void mostrar_actas_involucradas()
        {
            var actas_involucradas = from act in db_sindicato.ACTAS
                                     where act.CUIT == Convert.ToInt64(txt_actas_cuit.Text)
                                     select new
                                     {
                                         fecha_asig = act.FECHA_ASIG,
                                         acta = act.ACTA,
                                         desde = act.DESDE,
                                         hasta = act.HASTA,
                                         estado = act.COBRADOTOTALMENTE,
                                         inspector = act.INSPECTOR,
                                         importe_acta = act.DEUDATOTAL
                                     };

            dgv_actas_inv_asig.DataSource = actas_involucradas.ToList();

            
            if (actas_involucradas.Count() == 0)
            {
                dgv_cobros.DataSource = null;
                dgv_cobros.Refresh();
            }
            else
            {
                //reportes_CR f_reportes_CR = new reportes_CR()
                //List < DataGridViewRow > rows = (from item in dgv_actas_inv_asig.Rows.Cast<DataGridViewRow>()
                //                                   let fecha = Convert.ToDateTime(item.Cells["fecha_asig"].Value ?? string.Empty)
                //                                   let desde = Convert.ToString(item.Cells["desde"].Value ?? string.Empty)
                //                                   let descripcion = Convert.ToString(item.Cells["descripcion"].Value ?? string.Empty)
                //                                   where clave.Contains(busqueda) ||
                //                                          modelo.Contains(busqueda) ||
                //                                          descripcion.Contains(busqueda)
                //                                   select item).ToList<DataGridViewRow>();
            }
            

        }

        private void dgv_actas_inv_asig_SelectionChanged(object sender, EventArgs e)
        {
            mostrar_comprobantes();
        }

        private void mostrar_comprobantes()
        {

            var comprobantes_actas = from comp in db_sindicato.COBROS
                                     where comp.ACTA == Convert.ToInt32(dgv_actas_inv_asig.CurrentRow.Cells["num_acta"].Value)
                                     select new
                                     {
                                         cobro_id = comp.Id,
                                         cuota = (comp.CONCEPTO == "2") ? (comp.CUOTAX.ToString() + " de " + comp.CANTIDAD_CUOTAS.ToString()) : ("Anticipo"),
                                         fecha_venc = comp.FECHA_VENC,
                                         fecha = comp.FECHARECAUDACION,
                                         comprobante = comp.RECIBO,
                                         importe = comp.TOTAL
                                     };
            dgv_cobros.DataSource = comprobantes_actas.ToList();
            if (dgv_cobros.Rows.Count > 0)
            {
                foreach (DataGridViewRow fila in dgv_cobros.Rows)
                {
                    double dias = (DateTime.Today.Date - Convert.ToDateTime(fila.Cells["f_venc"].Value).Date).TotalDays;
                    if (fila.Cells["cuota"].Value.ToString() != "Anticipo")
                    {

                        if ((DateTime.Today - Convert.ToDateTime(fila.Cells["f_venc"].Value)).TotalDays > 0)
                        {
                            fila.Cells["dias_atraso"].Value = dias;//(DateTime.Today - Convert.ToDateTime(fila.Cells["f_venc"].Value)).TotalDays;
                            fila.Cells["interes_mora"].Value = (0.01 * dias) * Convert.ToDouble(fila.Cells["monto_pago"].Value);
                        }
                        else
                        {
                            fila.Cells["dias_atraso"].Value = "0";
                        }

                    }
                }
            }
        }

        private void btn_actas_asignar_inspector_Click(object sender, EventArgs e)
        {
            if (txt_actas_empresa.Text != "")
            {
                asignar_acta();
            }

        }

        private bool controlar_periodos_asignados()
        {
            bool ok = true;

            if (dgv_actas_dj_empresa.Rows.Count > 0)
            {
                string mensaje = string.Empty;
                DateTime desde = Convert.ToDateTime("01/" + txt_actas_desde.Text);
                DateTime hasta = Convert.ToDateTime("01/" + txt_actas_hasta.Text);
                foreach (DataGridViewRow fila in dgv_actas_inv_asig.Rows)
                {
                    if (desde <= hasta)
                    {
                        if (
                            (
                            Convert.ToDateTime(fila.Cells["acta_desde"].Value) >= desde && Convert.ToDateTime(fila.Cells["acta_desde"].Value) <= hasta
                           ||
                             Convert.ToDateTime(fila.Cells["acta_hasta"].Value) >= desde && Convert.ToDateTime(fila.Cells["acta_hasta"].Value) <= hasta
                             )
                           ||
                            (
                            desde >= Convert.ToDateTime(fila.Cells["acta_desde"].Value) && desde <= Convert.ToDateTime(fila.Cells["acta_hasta"].Value)
                           ||
                             hasta >= Convert.ToDateTime(fila.Cells["acta_hasta"].Value) && hasta <= Convert.ToDateTime(fila.Cells["acta_hasta"].Value))
                             )
                        {

                            if (fila.Cells["num_acta"].Value != null)
                            {

                                if (fila.Cells["num_acta"].Value.ToString() != "")
                                {
                                    mensaje += "El periodo pertenece al acta generada Nº " + fila.Cells["num_acta"].Value.ToString();
                                    ok = false;
                                }
                                else
                                {
                                    mensaje += "El periodo pertenece al acta asignada para el inspector " + fila.Cells["inspector"].Value.ToString();
                                    ok = false;
                                }
                            }
                            else
                            {
                                mensaje += "El periodo pertenece al acta asignada para el inspector " + fila.Cells["acta_inspector"].Value.ToString();
                                ok = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("El periodo [ DESDE ] debe ser menor o iguan que el periodo [ HASTA ] ");
                    }
                }
                if (mensaje != string.Empty)
                {
                    MessageBox.Show(mensaje, "Imposible asignar el Acta");
                }
            }
            return ok;

        }

        private void asignar_acta()
        {
            if (controlar_periodos_asignados() && buscar_cuit_asignado())
            {
                dgv_actas_asignar.Rows.Add();
                int f = dgv_actas_asignar.Rows.Count - 1;
                dgv_actas_asignar.Rows[f].Cells["asignacion"].Value = DateTime.Now.Date.ToShortDateString(); ;
                dgv_actas_asignar.Rows[f].Cells["cuit"].Value = txt_actas_cuit.Text;
                dgv_actas_asignar.Rows[f].Cells["empresa"].Value = txt_actas_empresa.Text;
                dgv_actas_asignar.Rows[f].Cells["inspector"].Value = cbx_actas_inspectores.Text;
                dgv_actas_asignar.Rows[f].Cells["desde"].Value = txt_actas_desde.Text;
                dgv_actas_asignar.Rows[f].Cells["hasta"].Value = txt_actas_hasta.Text;
                dgv_actas_asignar.Rows[f].Cells["deuda_aproximada"].Value = txt_actas_total_con_interes.Text;
                btn_actas_quitar.Enabled = true;
                btn_actas_aplicar.Enabled = true;
            }
        }

        private bool buscar_cuit_asignado()
        {
            bool ok = true;
            foreach (DataGridViewRow fila in dgv_actas_asignar.Rows)
            {
                if (fila.Cells["cuit"].Value.ToString().Trim() == txt_actas_cuit.Text.Trim())
                {
                    ok = false;
                    MessageBox.Show("La empresa ya se encuentra en la lista de asignacion para el Inspector [ " + fila.Cells["inspector"].Value.ToString() + " ]", "IMPORTANTE");
                }
            }
            return ok;
        }

        private void btn_actas_aplicar_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow fila in dgv_actas_asignar.Rows)
            {
                ACTAS acta = new ACTAS();
                acta.CUIT = Convert.ToInt64(fila.Cells["cuit"].Value.ToString());
                acta.FECHA_ASIG = Convert.ToDateTime(fila.Cells["asignacion"].Value);
                acta.ASIG_DESDE = Convert.ToDateTime("01/" + fila.Cells["desde"].Value);
                acta.ASIG_HASTA = Convert.ToDateTime("01/" + fila.Cells["hasta"].Value);
                acta.DESDE = Convert.ToDateTime("01/" + fila.Cells["desde"].Value);
                acta.HASTA = Convert.ToDateTime("01/" + fila.Cells["hasta"].Value);
                acta.INSPECTOR = fila.Cells["inspector"].Value.ToString();
                db_sindicato.ACTAS.InsertOnSubmit(acta);
                db_sindicato.SubmitChanges();
            }
            while (dgv_actas_asignar.Rows.Count != 0)
            {
                dgv_actas_asignar.Rows.RemoveAt(0);
            }
            cargar_seguimiento();

        }

        private void btn_actas_quitar_Click(object sender, EventArgs e)
        {
            dgv_actas_asignar.Rows.Remove(dgv_actas_asignar.CurrentRow);
            if (dgv_actas_asignar.Rows.Count > 1)
            {
                btn_actas_quitar.Enabled = true;
            }
            else
            {
                btn_actas_quitar.Enabled = false;
                btn_actas_aplicar.Enabled = false;
            }
        }

        private void cargar_seguimiento()
        {
            while (dgv_seguimiento.Rows.Count != 0)
            {
                dgv_seguimiento.Rows.RemoveAt(0);
            }
            var segui = (from a in db_sindicato.ACTAS
                         where a.ACTA.ToString() == null
                         select new
                         {

                             fecha_asignacion = a.FECHA_ASIG,
                             cuit = a.CUIT,
                             empresa = db_sindicato.maeemp.Where(x => x.MAEEMP_CUIT == a.CUIT).Single().MAEEMP_RAZSOC,
                             inspector = a.INSPECTOR,
                             dias = (DateTime.Today - Convert.ToDateTime(a.FECHA_ASIG)).TotalDays,
                             id_acta = a.ID_ACTA

                         }).OrderByDescending(x => x.dias).ToList();
            if (segui.Count > 0)
            {
                foreach (var item in segui)
                {
                    dgv_seguimiento.Rows.Add();
                    int f = dgv_seguimiento.Rows.Count - 1;
                    dgv_seguimiento.Rows[f].Cells["dgv_segui_asignacion"].Value = item.fecha_asignacion;
                    dgv_seguimiento.Rows[f].Cells["dgv_segui_cuit"].Value = item.cuit;
                    dgv_seguimiento.Rows[f].Cells["dgv_segui_empresa"].Value = item.empresa;
                    dgv_seguimiento.Rows[f].Cells["dgv_segui_inspector"].Value = item.inspector;
                    dgv_seguimiento.Rows[f].Cells["dgv_segui_dias"].Value = item.dias.ToString();
                    dgv_seguimiento.Rows[f].Cells["id_acta"].Value = item.id_acta;
                }
            }
        }

        private void btn_cargar_acta_Click(object sender, EventArgs e)
        {
            //control_carga_acta();
            cargar_acta();

        }

        private void control_carga_acta()
        {
            int ok = 0;
            if (txt_acta_nro.Text == "") ok = 1;
            if (txt_acta_desde.Text == "") ok = 2;
            if (txt_acta_hasta.Text == "") ok = 3;
            if (txt_acta_capital.Text == "") ok = 4;
            if (txt_acta_interes.Text == "") ok = 5;
            if (txt_acta_subtotal.Text == "") ok = 6;
            if (txt_acta_total.Text == "") ok = 7;
            if (chk_cargar_financiacion.Checked == true)
            {
                if (txt_acta_anticipo.Text == "") ok = 8;
                if (txt_acta_tasa.Text == "") ok = 9;
                if (txt_acta_interes_financ.Text == "") ok = 10;
                if (txt_acta_coeficiente.Text == "") ok = 11;
                if (txt_acta_cuotas.Text == "") ok = 12;
                if (txt_acta_importe_cuota.Text == "") ok = 13;

            }

            string msj = "Debe Revisar el Ingreso de los siguientes datos: ";
            switch (ok)
            {
                case 1:
                    msj += "Nº de Acta - ";
                    break;
                case 2:
                    msj += "Fecha Desde - ";
                    break;
                case 3:
                    msj += "Fecha Hasta - ";
                    break;
                case 4:
                    msj += "Capital - ";
                    break;
                case 5:
                    msj += "Interes - ";
                    break;
                case 6:
                    msj += "Subtotal - ";
                    break;
                case 7:
                    msj += "Total - ";
                    break;
                case 8:
                    msj += "Anticipo - ";
                    break;
                case 9:
                    msj += "Tasa - ";
                    break;
                case 10:
                    msj += "Interes de Financiacion - ";
                    break;
                case 11:
                    msj += "Coeficiente - ";
                    break;
                case 12:
                    msj += "Importe - ";
                    break;
            }

        }

        private void mostrar_mensajes()
        {

        }

        private void cargar_acta()
        {
            try
            {
                var cargar_acta = (from a in db_sindicato.ACTAS.Where(x => x.ID_ACTA == Convert.ToInt32(dgv_seguimiento.CurrentRow.Cells["id_acta"].Value)) select a).Single();
                cargar_acta.FECHA = dtp_fecha_gen_acta.Value;
                cargar_acta.ACTA = Convert.ToDouble(txt_acta_nro.Text);
                cargar_acta.CUIT = Convert.ToDouble(dgv_seguimiento.CurrentRow.Cells["dgv_segui_cuit"].Value.ToString());
                cargar_acta.DESDE = Convert.ToDateTime("01/" + txt_actas_desde.Text);
                cargar_acta.DEUDAHISTORICA = Convert.ToDouble(txt_acta_capital.Text);
                cargar_acta.INTERESES = Convert.ToDouble(txt_acta_interes.Text);
                cargar_acta.DEUDATOTAL = Convert.ToDouble(txt_acta_total.Text);
                if (chk_cargar_financiacion.Checked)
                {
                    cargar_acta.ANTICIPO = Convert.ToDecimal(txt_acta_anticipo.Text);
                    cargar_acta.TASA = Convert.ToDecimal(txt_acta_tasa.Text);
                    cargar_acta.INTERESFINANC = Convert.ToDouble(txt_acta_interes_financ.Text);
                    cargar_acta.COEFICIENTE = txt_acta_coeficiente.Text;
                    cargar_acta.CANTIDADCUOTAS = Convert.ToDouble(txt_acta_cuotas.Text);
                    cargar_acta.IMPORTE_CUOTA = Convert.ToDecimal(txt_acta_importe_cuota.Text);

                    // Genero el Registro para cobros.
                    if (txt_acta_anticipo.Text != "0.00") // SI hay anticipo, genero el registro para efecuar el cobro
                    {
                        COBROS cobro_ = new COBROS();
                        cobro_.ACTA = Convert.ToDouble(txt_acta_nro.Text);
                        cobro_.CUIT = Convert.ToDouble(dgv_seguimiento.CurrentRow.Cells["dgv_segui_cuit"].Value.ToString());
                        cobro_.CONCEPTO = "1"; // 1 - ANTICIPO
                        cobro_.IMPORTE = Convert.ToDouble(txt_acta_anticipo.Text);
                        cobro_.TOTAL = Convert.ToDouble(txt_acta_anticipo.Text);
                        cobro_.FECHA_VENC = String.Format("{0:d}", dtp_venc_anticipo.Value);
                        db_sindicato.COBROS.InsertOnSubmit(cobro_);
                        db_sindicato.SubmitChanges();
                    }
                    // Genero el registropara cobros de cuotas de actas
                    DateTime fecha_venc = dtp_venc_anticipo.Value; //.Date.AddMonths(1);
                    for (int i = 1; i <= Convert.ToInt16(txt_acta_cuotas.Text); i++)
                    {

                        COBROS cobro = new COBROS();
                        cobro.ACTA = Convert.ToDouble(txt_acta_nro.Text);
                        cobro.CUIT = Convert.ToDouble(dgv_seguimiento.CurrentRow.Cells["dgv_segui_cuit"].Value.ToString());
                        cobro.CONCEPTO = "2"; // 2 - CUOTA DE PLAN DE PAGO DE ACTA
                        cobro.IMPORTE = Convert.ToDouble(txt_acta_importe_cuota.Text);
                        cobro.TOTAL = Convert.ToDouble(txt_acta_importe_cuota.Text);
                        cobro.FECHA_VENC = String.Format("{0:d}", fecha_venc.Date.AddMonths(i));
                        cobro.CUOTAX = i.ToString();
                        cobro.CANTIDAD_CUOTAS = txt_acta_cuotas.Text;
                        db_sindicato.COBROS.InsertOnSubmit(cobro);
                        db_sindicato.SubmitChanges();
                    }

                }

                db_sindicato.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }

        }



        private void chk_Cargar_Acta_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Cargar_Acta.Checked)
            {
                gbx_asig_actas.Enabled = true;
                chk_cargar_financiacion.Checked = false;
                chk_cargar_financiacion.Enabled = true;
            }
            else
            {
                gbx_asig_actas.Enabled = false;
                gbx_financiacion.Enabled = false;
                chk_cargar_financiacion.Enabled = false;

            }

        }

        private void chk_cargar_financiacion_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_cargar_financiacion.Checked)
            {
                gbx_financiacion.Enabled = true;
            }
            else
            {
                gbx_financiacion.Enabled = false;
                //chk_cargar_financiacion.Enabled = false;
            }
        }

        private void btn_imprimir_consulta_Click(object sender, EventArgs e)
        {
            //limpio la tabla de impresion
            var im = from a in db_sindicato.impresion_comprobante select a;
            foreach (var item in im)
            {
                db_sindicato.impresion_comprobante.DeleteOnSubmit(item);
                db_sindicato.SubmitChanges();
            }

            var imprimir_actas = from a in db_sindicato.impresion_actas select a;

            foreach (var item in imprimir_actas)
            {
                db_sindicato.impresion_actas.DeleteOnSubmit(item);
                db_sindicato.SubmitChanges();
            }

            for (int f = 0; f < dgv_actas_dj_empresa.Rows.Count; f++)
            {
                impresion_comprobante imp = new impresion_comprobante();
                imp.empresa = txt_actas_empresa.Text;
                imp.cuit = txt_actas_cuit.Text;
                imp.localidad = txt_actas_localidad.Text;
                imp.telefono = txt_actas_telefono.Text;
                imp.estudio = txt_actas_estudio.Text;
                imp.domicilio = txt_actas_domicilio.Text;
                imp.desde = txt_actas_desde.Text;
                imp.hasta = txt_actas_hasta.Text;
                imp.periodo = Convert.ToDateTime(dgv_actas_dj_empresa.Rows[f].Cells["periodo"].Value);
                imp.rectificacion = Convert.ToInt16(dgv_actas_dj_empresa.Rows[f].Cells["rectificacion"].Value);
                imp.aporte_ley = Convert.ToDecimal(dgv_actas_dj_empresa.Rows[f].Cells["aporte_ley"].Value);
                imp.aporte_socio = Convert.ToDecimal(dgv_actas_dj_empresa.Rows[f].Cells["aporte_socio"].Value);
                imp.fechapago = (dgv_actas_dj_empresa.Rows[f].Cells["fecha_pago"].Value != null) ? dgv_actas_dj_empresa.Rows[f].Cells["fecha_pago"].Value.ToString() : "";
                imp.importe_depositado = Convert.ToDecimal(dgv_actas_dj_empresa.Rows[f].Cells["importe_depositado"].Value);
                imp.cantidad_empleados = Convert.ToInt16(dgv_actas_dj_empresa.Rows[f].Cells["empleados"].Value);
                imp.cantidad_socios = Convert.ToInt16(dgv_actas_dj_empresa.Rows[f].Cells["socios"].Value);
                imp.capital = Convert.ToDecimal(dgv_actas_dj_empresa.Rows[f].Cells["capital"].Value);
                imp.interes = Convert.ToDecimal(dgv_actas_dj_empresa.Rows[f].Cells["interes"].Value);
                imp.total = Convert.ToDecimal(dgv_actas_dj_empresa.Rows[f].Cells["total"].Value);
                db_sindicato.impresion_comprobante.InsertOnSubmit(imp);
                db_sindicato.SubmitChanges();
            }

            var actas_involucradas = (from act in db_sindicato.ACTAS
                                      where act.CUIT == Convert.ToInt64(txt_actas_cuit.Text)
                                      select new
                                      {
                                          fecha_asig = act.FECHA_ASIG,
                                          acta = act.ACTA,
                                          desde = act.DESDE,
                                          hasta = act.HASTA,
                                          estado = act.COBRADOTOTALMENTE,
                                          inspector = act.INSPECTOR,
                                          importe_acta = act.DEUDATOTAL
                                      }).ToList();

            // instancio el formulario contenedor del reporte y le paso los parametros que seran los datos del encabezado del mismo
            double suma_aportes = Convert.ToDouble(txt_actas_aporte2.Text) + Convert.ToDouble(txt_actas_aportes_socio.Text);

            reportes_CR f_reportes_CR = new reportes_CR(txt_actas_empresa.Text, txt_actas_localidad.Text,
                txt_actas_cuit.Text.Substring(0, 2) + "-" + txt_actas_cuit.Text.Substring(2, 8) + "-" + txt_actas_cuit.Text.Substring(10, 1),
                txt_actas_estudio.Text, txt_actas_desde.Text, txt_actas_hasta.Text, txt_actas_domicilio.Text,
                txt_actas_pagados.Text, txt_actas_no_pagados.Text, txt_actas_no_declarados.Text, txt_actas_total_periodos.Text,
                "$ " + txt_actas_aporte2.Text, "$ " + txt_actas_aportes_socio.Text, "$ " + suma_aportes.ToString(), txt_actas_depositado.Text, txt_actas_intereses.Text,
                txt_capital.Text, txt_actas_total_con_interes.Text, Convert.ToDouble(txt_actas_cuit.Text)

                );
            // string localidad, string cuit, string estudio, string desde, string hasta, string domicilio

            f_reportes_CR.Show();

        }

        private void btn_imprimir_empleados_Click(object sender, EventArgs e)
        {
            //limpio la tabla de impresion
            var im = from a in db_sindicato.impresion_comprobante select a;
            foreach (var item in im)
            {
                db_sindicato.impresion_comprobante.DeleteOnSubmit(item);
                db_sindicato.SubmitChanges();
            }

            DateTime desde = Convert.ToDateTime("01/" + txt_actas_desde.Text);
            DateTime hasta = Convert.ToDateTime("01/" + txt_actas_hasta.Text);

            var emp = (from a in db_sindicato.ddjj
                       where a.cuite == Convert.ToDouble(txt_actas_cuit.Text) && a.periodo >= desde && a.periodo <= hasta
                       join datos in db_sindicato.maesoc on a.cuil equals datos.MAESOC_CUIL
                       join cat in db_sindicato.categorias_empleado on datos.MAESOC_CODCAT equals cat.MAECAT_CODCAT
                       join act in db_sindicato.actividad on datos.MAESOC_CODACT equals act.MAEACT_CODACT
                       join soce in db_sindicato.socemp on a.cuil equals soce.SOCEMP_CUIL
                       where soce.SOCEMP_ULT_EMPRESA == 'S'
                       orderby a.periodo
                       select new
                       {
                           periodo = a.periodo,//String.Format("{0:dd/MM/yyyy}", a.periodo),
                           CUIL = String.Format("{0:g}", a.cuil),
                           nombre = (datos.MAESOC_APELLIDO.Trim() + " " + datos.MAESOC_NOMBRE.Trim()),
                           aporte_ley = (a.impo + a.impoaux) * 0.02 , // String.Format("{0:C}", ((a.impo + a.impoaux) * 0.02)),
                           aporte_socio = (a.item2 == true) ? ((a.impo + a.impoaux) * 0.02) : 0, //String.Format("{0:C}", ((a.item2 == true) ? (a.impo + a.impoaux) * 0.02 : 0)),
                           sueldo = a.impo,//String.Format("{0:C}", a.impo),
                           acuerdo = a.impoaux, //String.Format("{0:C}", a.impoaux),
                           licencia = (a.lic != "0000") ? "SI" : "NO",
                           jornada = (a.jorp == true) ? "JP" : "JC",
                           fecha_ing = (soce.SOCEMP_FECHAING == null) ? " " :  soce.SOCEMP_FECHAING.ToString(), //String.Format("{0:dd/MM/yyyy}", soce.SOCEMP_FECHAING),
                           fecha_baja = (soce.SOCEMP_FECHABAJA == null) ? " " : soce.SOCEMP_FECHABAJA.ToString(),
                           categoria = cat.MAECAT_NOMCAT.Trim()

                       }).ToList();//.OrderBy(x=>Convert.ToDateTime( x.periodo));
            //dgv_1.DataSource = emp.ToList();//.ThenBy(x=>x.APENOM).ToList();

            foreach (var item in emp)
            {
                impresion_comprobante imp = new impresion_comprobante();
                imp.fecha1 = item.periodo;
                imp.cuit = item.CUIL;
                imp.socio_apenom = item.nombre;
                imp.aporte_ley = Convert.ToDecimal( item.aporte_ley);
                imp.aporte_socio = Convert.ToDecimal(item.aporte_socio);
                imp.importe_depositado = Convert.ToDecimal(item.sueldo);
                imp.interes = Convert.ToDecimal(item.acuerdo);
                imp.COL2NOMBRE = item.licencia;
                imp.COL1NOMBRE = item.jornada;
                imp.COL2EMPRESA =  item.fecha_ing ;
                imp.COL1EMPRESA = item.fecha_baja; //item.fecha_baja == " " ? " " : item.fecha_baja.ToString().Substring(9,2);// + "/" + item.fecha_baja.Substring(6,2) + "/" + item.fecha_baja.Substring(1,4);
                imp.CATEGORIA = item.categoria;
                db_sindicato.impresion_comprobante.InsertOnSubmit(imp);
                db_sindicato.SubmitChanges();
            }
            reportes_CR f_reportes = new reportes_CR(1);
            
            
            f_reportes.Show();
        }
        string generar_ceros(string valor, int tamaño)
        {
            string ceros = null;
            for (int i = 0; i < tamaño - valor.Length; i++)
            {
                ceros += "0";
            }
            valor = ceros + valor;
            return valor;
        }
    }
}

