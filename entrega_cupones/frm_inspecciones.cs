using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace entrega_cupones
{
    public partial class frm_inspecciones : Form
    {
        lts_sindicatoDataContext db_socios = new lts_sindicatoDataContext();
        ContextMenuStrip menu_copiar_de = new ContextMenuStrip();

        public frm_inspecciones()
        {
            InitializeComponent();
        }

        private void frm_inspecciones_Load(object sender, EventArgs e)
        {

            var empr = (from emp in db_socios.maeemp
                        select new
                        {
                            cuite = emp.MAEEMP_CUIT,
                            razon_social = emp.MAEEMP_RAZSOC.Trim(),
                            nombre_fantasia = emp.MAEEMP_NOMFAN
                        }).OrderBy(x => x.razon_social);

            cbx_nombre_empresa.DisplayMember = "razon_social";
            cbx_nombre_empresa.ValueMember = "cuite";
            cbx_nombre_empresa.DataSource = empr.ToList();

            cbx_cuit.DisplayMember = "cuite";
            cbx_cuit.ValueMember = "cuite";
            cbx_cuit.DataSource = empr.ToList();

            var localidad = from loc in db_socios.localidades where loc.idprovincias == 14
                            select loc;

            cbx_localidad.DisplayMember = "nombre";
            cbx_localidad.ValueMember = "codigopostal";
            cbx_localidad.DataSource = localidad.ToList(); ;

            var inspector = (from inspec in db_socios.inspectores select inspec).OrderBy(x => x.APELLIDO);

            cbx_inspectores.DisplayMember = "apellido";// + " " + "nombre";
            cbx_inspectores.ValueMember = "id_inspector";
            cbx_inspectores.DataSource = inspector.ToList();

            cbx_seguimiento_inspector.DisplayMember = "apellido";// + " " + "nombre";
            cbx_seguimiento_inspector.ValueMember = "id_inspector";
            cbx_seguimiento_inspector.DataSource = inspector.ToList();
            


            generar_seguimiento();
            
        }

        private void cbx_nombre_empresa_SelectedIndexChanged(object sender, EventArgs e)
        {

            actualizar_consulta();

        }

        private void btn_actualizar_Click(object sender, EventArgs e)
        {
            actualizar_consulta();
        }

        private void actualizar_consulta()
        {
            if (cbx_nombre_empresa.SelectedValue.ToString() != null)
            {
                Int64 num_cuit = Convert.ToInt64(cbx_nombre_empresa.SelectedValue.ToString());
                DateTime desde = Convert.ToDateTime("01/" + cbx_desde.SelectedItem.ToString());
                DateTime hasta = Convert.ToDateTime("01/" + cbx_hasta.SelectedItem.ToString());
                txt_cuit.Text = num_cuit.ToString("##-########-#");
                var datos_empresa = db_socios.maeemp.Where(x => x.MAEEMP_CUIT == num_cuit).Single();
                txt_telefono.Text = datos_empresa.MAEEMP_TEL;

                txt_domicilio.Text = datos_empresa.MAEEMP_CALLE.Trim() + " " + datos_empresa.MAEEMP_NRO;
                txt_estudio_contable.Text = datos_empresa.MAEEMP_ESTUDIO_CONTACTO.Trim();
                //txt_localidad.Text = db_socios.localidades.Where(x => x.codigopostal == datos_empresa.MAEEMP_CODPOS).Single().nombre;

                var ddjj_empresa = (from djt
                                    in db_socios.ddjjt.Where(x => x.cuit.Equals(num_cuit) && (x.periodo >= desde && x.periodo <= hasta))
                                    select new
                                    {
                                        periodo = djt.periodo,
                                        rectificacion = djt.rect,  //db_socios.ddjjt.Where(x=>x.periodo == djt.periodo).Max(y=>y.rect),
                                        presentacion = djt.fpres,
                                        modificacion = djt.fmod,
                                        aporte_2 = djt.titem1,
                                        aporte_socio = djt.titem2,
                                        capital = ((djt.titem1 + djt.titem2) == 0 && djt.impban1 != 0) ? djt.impban1 : (djt.titem1 + djt.titem2),
                                        pago = djt.fpago, //(djt.fpago != null )? calcular_coeficiente_A(Convert.ToDateTime(djt.fpago.ToString())) : djt.fpago ,
                                        procesado = djt.fproc,
                                        importeps = djt.impban1,
                                        transferencia = djt.trans1,
                                        cant_emple = db_socios.ddjj.Where(x => x.periodo == djt.periodo && x.cuite == num_cuit).Select(t => t.item1).Count(),
                                        cant_socios = db_socios.ddjj.Where(x => x.periodo == djt.periodo && x.cuite == num_cuit && x.item2 == true).Select(t => t.item2).Count(),
                                        total_periodo = (djt.fpago != null) ? calcular_coeficiente_A(Convert.ToDateTime(djt.periodo.ToString()), Convert.ToDateTime(djt.fpago.ToString()), Convert.ToDouble(djt.titem1 + djt.titem2), Convert.ToDouble(djt.impban1))
                                                                  : calcular_coeficiente_B(Convert.ToDateTime(djt.periodo.ToString()), Convert.ToDouble(djt.titem1 + djt.titem2), Convert.ToDouble(djt.impban1)),
                                    }).OrderBy(x => x.periodo);//.Distinct().Where(x=>x.pago != null).OrderBy(x => x.periodo);
                //ddjj_empresa.ToList().RemoveAll(x => x.periodo == Convert.ToDateTime(dgv_dj_empresa.CurrentRow.Cells["periodo"].Value) && x.rectificacion == Convert.ToInt16( dgv_dj_empresa.CurrentRow.Cells["rectificacion"].Value));

                dgv_dj_empresa.DataSource = ddjj_empresa.ToList();

                obtener_diferencia_capital_interes();
                //Convert.ToDecimal(empleados_periodo.Sum(x => x.sueldo)).ToString("#,0.00");
                lbl_total_2.Text = Convert.ToDecimal(ddjj_empresa.Sum(x => x.aporte_2)).ToString("#,0.00");
                if (lbl_total_2.Text == "")
                {
                    lbl_total_2.Text = "0.00";
                }
                lbl_total_socio.Text = Convert.ToDecimal(ddjj_empresa.Sum(x => x.aporte_socio)).ToString("#,0.00");
                if (lbl_total_socio.Text == "")
                {
                    lbl_total_socio.Text = "00.0";
                }
                lbl_total_periodo.Text = (Convert.ToDecimal(lbl_total_2.Text) + Convert.ToDecimal(lbl_total_socio.Text)).ToString("#,0.00");
                lbl_total_recaudado.Text = Convert.ToDecimal(ddjj_empresa.Where(x => x.pago != null && x.importeps != 0 && x.transferencia != 0).Sum(t => t.importeps)).ToString("#,0.00");
                if (lbl_total_recaudado.Text == "")
                {
                    lbl_total_recaudado.Text = "0.00";
                }
                lbl_falta_recaudar.Text = ((Convert.ToDecimal(lbl_total_periodo.Text)) - (Convert.ToDecimal(lbl_total_recaudado.Text))).ToString("#,0.00");
                //muestro el periodo seleccionado
                if (dgv_dj_empresa.Rows.Count > 0)
                {
                    lbl_cant_emp_periodo.Text = "[" + dgv_dj_empresa.CurrentRow.Cells["periodo"].Value.ToString().Substring(0, 10) + "]";
                }
                else
                {
                    lbl_cant_emp_periodo.Text = "-";
                }

                //pintar_filas_dgv();

                mostrar_actas_involucradas();


            }
        }

        private void obtener_diferencia_capital_interes()
        {

            foreach (DataGridViewRow fila in dgv_dj_empresa.Rows)
            {
                double interes = Convert.ToDouble(fila.Cells["total"].Value) - Convert.ToDouble(fila.Cells["capital"].Value);
                if (interes < 0)
                {
                    fila.Cells["interes"].Value = fila.Cells["total"].Value;
                }
                else
                {
                    fila.Cells["interes"].Value = interes;
                }

            }
            double total_con_interes = 0;
            foreach (DataGridViewRow fila in dgv_dj_empresa.Rows)
            {
                total_con_interes = total_con_interes + Convert.ToDouble(fila.Cells["total"].Value);
            }

            lbl_total_con_interes.Text = Math.Round(total_con_interes, 2).ToString();
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
                coef_B = ((dtp_venc_acta.Value - fecha_vencimiento).TotalDays * 0.001) + 1;
                tot_intereses_a_la_fecha = ((tot_periodo - pagado) * coef_B) + ((pagado * coef_A) * (coef_B - coef_A)) ;
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
            coef_B = ((dtp_venc_acta.Value - fecha_vencimiento).TotalDays * 0.001) + 1;
            tot_intereses_a_la_fecha = ((tot_periodo - pagado) * coef_B) + ((pagado * coef_A) * (coef_B - coef_A));

            return tot_intereses_a_la_fecha; //tot_interes_mora_de_pago;
        }

        private void mostrar_actas_involucradas()
        {
            var actas_involucradas = from act in db_socios.ACTAS
                                     where act.CUIT == Convert.ToInt64(cbx_nombre_empresa.SelectedValue.ToString())
                                     select new
                                     {
                                         acta = act.ACTA,
                                         desde = act.DESDE,
                                         hasta = act.HASTA,
                                         estado = act.COBRADOTOTALMENTE,
                                         inspector = act.INSPECTOR,
                                         importe_acta = act.DEUDATOTAL
                                     };

            dgv_actas.DataSource = actas_involucradas.ToList();
            if (actas_involucradas.Count() == 0)
            {
                dgv_cobros.DataSource = null;
                dgv_cobros.Refresh();
            }

        }

        private void pintar_filas_dgv()
        {

            foreach (DataGridViewRow fila in dgv_dj_empresa.Rows)
            {
                if (Convert.ToInt16(fila.Cells["acta"].Value) != 0)
                {
                    fila.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void dgv_dj_empresa_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dgv_dj_empresa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_actualizar_actas_Click(object sender, EventArgs e)

        {
            //    //DateTime ff = Convert.ToDateTime("20140218 00:00:00.000");
            //    var actas = from act in db_socios.actas_cruzar  where  act.NRO_ACTA == 2295// act.CUIT == 20281398300 //where act.FECHA > ff //.Where(x=>x.CUIT == 30714070858 && x.NRO_ACTA == 2020)
            //                select new
            //                {
            //                    periodo_desde = act.DESDE,
            //                    periodo_hasta = act.HASTA,
            //                    cuit = act.CUIT,
            //                    acta = act.NRO_ACTA,
            //                    cancelado = act.COBRADO_TOTALMENTE
            //                };

            //    //var dj = from a in db_socios.ddjjt where a.periodo == Convert.ToDateTime("01/01/2014 0:00:00") select a; //a.cuit == item.cuit &&
            //    foreach (var item in actas)
            //    {
            //        var dje = from a in db_socios.ddjjt where a.cuit == item.cuit &&  
            //                  (a.periodo >= Convert.ToDateTime(item.periodo_desde.Value) && 
            //                  a.periodo <= Convert.ToDateTime(item.periodo_hasta.Value)) select a;
            //        //int mes_ini = item.periodo_desde.Value.Month;
            //        //int año_ini = item.periodo_desde.Value.Year;
            //        //int mes_hasta = item.periodo_hasta.Value.Month;
            //        //int año_hasta = item.periodo_hasta.Value.Year;
            //        DateTime desde = item.periodo_desde.Value;
            //        DateTime hasta = item.periodo_hasta.Value;
            //        if (desde <= hasta)
            //        {
            //            int dim = Math.Abs((desde.Month - hasta.Month) + 12 * (desde.Year - hasta.Year));
            //            DateTime[] periodos_array = new DateTime[dim + 1];
            //            int i = 0;
            //            while (desde <= hasta)
            //            {
            //                periodos_array[i] = desde;
            //                i++;
            //                desde = desde.AddMonths(1);
            //            }

            //            foreach (DateTime per in periodos_array)
            //            {
            //                var sub_dje = dje.Where(x => x.periodo == Convert.ToDateTime(per));
            //                if (sub_dje.Count() == 0)
            //                {
            //                    //foreach (var item3 in sub_dje)
            //                    //{
            //                    ddjjt dj_insertar = new ddjjt();
            //                    dj_insertar.cuit = Convert.ToDouble(item.cuit);
            //                    dj_insertar.periodo = per;
            //                    dj_insertar.rect = 0;
            //                    dj_insertar.fpres = DateTime.Today;
            //                    dj_insertar.fmod = DateTime.Today;
            //                    dj_insertar.fpago = DateTime.Today;
            //                    dj_insertar.acta = Convert.ToInt16(item.acta);
            //                    //dj_insertar.cancelado = item.cancelado;
            //                    db_socios.ddjjt.InsertOnSubmit(dj_insertar);
            //                    db_socios.SubmitChanges();
            //                    //}
            //                }
            //                else
            //                {
            //                    foreach (var item3 in sub_dje)
            //                    {
            //                        ddjjt djt = db_socios.ddjjt.Where(x => x.id_ddjjt == item3.id_ddjjt).Single();
            //                        djt.acta = Convert.ToInt16(item.acta);
            //                        djt.cancelado = item.cancelado;
            //                        db_socios.SubmitChanges();
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Fecha de periodo de actas no es correcta.");
            //        }
            //    }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_dj_empresa_SelectionChanged_1(object sender, EventArgs e)
        {
            if (cbx_nombre_empresa.SelectedValue != null)
            {
                if (dgv_dj_empresa.Rows.Count > 0)
                {
                    DateTime periodo = Convert.ToDateTime(dgv_dj_empresa.CurrentRow.Cells["periodo"].Value);

                    Int64 cuit_empresa = Convert.ToInt64(cbx_nombre_empresa.SelectedValue);

                    var empleados_periodo = (from a in db_socios.ddjj.Where(x => x.periodo == periodo && x.cuite == cuit_empresa)
                                             select new
                                             {
                                                 cuil = a.cuil,
                                                 nombre = (from nom in db_socios.maesoc
                                                           where nom.MAESOC_CUIL == (db_socios.socemp.Where(x => x.SOCEMP_CUIL == a.cuil && x.SOCEMP_CUITE == a.cuite).First().SOCEMP_CUIL)
                                                           select new { nombre = nom.MAESOC_APELLIDO.Trim() + " " + nom.MAESOC_NOMBRE.Trim() }).Single().nombre,

                                                 //db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil).First().apellido.Trim() + " " +  //.Single().apellido + " " +
                                                 //db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil).First().nombre.Trim(),// .Single().nombre,
                                                 sueldo = a.impo + a.impoaux,
                                                 aporte_empleado_ley = (a.impo + a.impoaux) * 0.02,
                                                 aporte_empleado_socio = (a.item2.Equals("True")) ? (a.impo + a.impoaux) * 0.02 : 0,//(a.item2 == true)? a.impo * 0.02:0
                                                 licencia = a.lic,

                                                 alta = (from alt in db_socios.soccen where alt.SOCCEN_FEING.Value.Month == periodo.Month && alt.SOCCEN_FEING.Value.Year == periodo.Year && alt.SOCCEN_CUIL == a.cuil select new { fing = alt.SOCCEN_FEING }).Single().fing == null ? "0" : "1",
                                                 //db_socios.soccen.Where(x => x.SOCCEN_FEING.Value.Month == periodo.Month && x.SOCCEN_FEING.Value.Year == periodo.Year && x.SOCCEN_CUIL == a.cuil).Count() > 0 ?

                                                 //(db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).Single().fechaing.Value.Month == periodo.Month &&
                                                 //        db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).Single().fechaing.Value.Year == periodo.Year) ?
                                                 //        db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).Single().fechaing :
                                                 //        null,
                                                 //baja = (db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 1).Single().fechabaja.Value.Month == periodo.Month &&
                                                 //        db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 1).Single().fechabaja.Value.Year == periodo.Year) ?
                                                 //        db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 1).Single().fechabaja :
                                                 //        null,
                                                 acuerdo = a.impoaux
                                                 //categ = db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).First().categ

                                             }).OrderBy(x => x.nombre);

                    dgv_empleados_periodo.DataSource = empleados_periodo.ToList();
                    //muestro los totales de los empleados del periodo seleccionado en el dgv de las ddjj
                    lbl_total_sueldo.Text = Convert.ToDecimal(empleados_periodo.Sum(x => x.sueldo)).ToString("#,0.00"); //Convert.ToString(empleados_periodo.Sum(x => x.sueldo));
                    lbl_total_ley.Text = Convert.ToDecimal(empleados_periodo.Sum(x => x.aporte_empleado_ley)).ToString("#,0.00");
                    lbl_total_socios_periodo.Text = Convert.ToDecimal(empleados_periodo.Sum(x => x.aporte_empleado_socio)).ToString("#,0.00");
                    lbl_total_licencia.Text = Convert.ToString(empleados_periodo.Count(x => x.licencia != "0000"));
                    lbl_total_altas.Text = Convert.ToString(empleados_periodo.Count(x => x.alta != null));
                    //lbl_total_bajas.Text = Convert.ToString(empleados_periodo.Count(x => x.baja != null));
                    lbl_total_acuerdo.Text = Convert.ToDecimal(empleados_periodo.Sum(x => x.acuerdo)).ToString("#,0.00");

                    //var empleados_periodo = (from a in db_socios.ddjj.Where(x => x.periodo == periodo && x.cuite == cuit_empresa)
                    //                         select new
                    //                         {
                    //                             cuil = a.cuil,
                    //                             nombre = db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil ).First().apellido.Trim() + " " +  //.Single().apellido + " " +
                    //                             db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil).First().nombre.Trim(),// .Single().nombre,
                    //                             sueldo = a.impo + a.impoaux,
                    //                             aporte_empleado_ley = (a.impo + a.impoaux) * 0.02,
                    //                             aporte_empleado_socio = (a.item2.Equals("True")) ? (a.impo + a.impoaux) * 0.02 : 0,//(a.item2 == true)? a.impo * 0.02:0
                    //                             licencia = a.lic,
                    //                             alta = (db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).Single().fechaing.Value.Month == periodo.Month &&
                    //                                     db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).Single().fechaing.Value.Year == periodo.Year) ?
                    //                                     db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).Single().fechaing :
                    //                                     null,
                    //                             baja = (db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 1).Single().fechabaja.Value.Month == periodo.Month &&
                    //                                     db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 1).Single().fechabaja.Value.Year == periodo.Year) ?
                    //                                     db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 1).Single().fechabaja :
                    //                                     null,
                    //                             acuerdo = a.impoaux,
                    //                             categ = db_socios.empleados.Where(x => x.cuite == cuit_empresa && x.cuil == a.cuil && x.activo == 0).First().categ

                    //                         });//.OrderBy(x => x.nombre);

                }
            }
        }

        private void dgv_actas_SelectionChanged(object sender, EventArgs e)
        {
            var comprobantes_actas = from comp in db_socios.COBROS
                                     where comp.ACTA == Convert.ToInt32(dgv_actas.CurrentRow.Cells[0].Value)
                                     select new
                                     {
                                         fecha = comp.FECHARECAUDACION,
                                         comprobante = comp.RECIBO,
                                         total = comp.TOTAL

                                     };
            dgv_cobros.DataSource = comprobantes_actas.ToList();
        }

        private void btn_cerrar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        


        private void btn_generar_ranking_Click(object sender, EventArgs e)
        {
            while (dgv_simular_acta.Rows.Count != 0)
            {
                dgv_simular_acta.Rows.RemoveAt(0);
            }
            // genero la lista de empresas segun la localidad seleccionada
            var empresa = from emp in db_socios.maeemp
                          where emp.MAEEMP_CODPOS == cbx_localidad.SelectedValue.ToString()
                          select new
                          {
                              cuit = emp.MAEEMP_CUIT,
                              Nombre = emp.MAEEMP_RAZSOC
                          };

            
            // traigo todas las deudas de todas la empresas
            var ddjj_empresa = from djt in db_socios.ddjjt
                               where (djt.periodo >= Convert.ToDateTime("01/06/2013") && djt.periodo <= DateTime.Now) 
                               select new
                               {
                                   cuit = djt.cuit,
                                   periodo = djt.periodo,
                                   rectificacion = djt.rect,
                                   presentacion = djt.fpres,
                                   modificacion = djt.fmod,
                                   aporte_2 = djt.titem1,
                                   aporte_socio = djt.titem2,
                                   capital = djt.titem1 + djt.titem2,
                                   pago = djt.fpago,
                                   procesado = djt.fproc,
                                   importeps = djt.impban1,
                                   transferencia = djt.trans1,
                                   total_periodo = (djt.fpago != null) ? calcular_coeficiente_A(Convert.ToDateTime(djt.periodo.ToString()), Convert.ToDateTime(djt.fpago.ToString()), Convert.ToDouble(djt.titem1 + djt.titem2), Convert.ToDouble(djt.impban1))
                                                             : calcular_coeficiente_B(Convert.ToDateTime(djt.periodo.ToString()), Convert.ToDouble(djt.titem1 + djt.titem2), Convert.ToDouble(djt.impban1))

                               };

            double total_a_cobrar = 0;
            foreach (var em in empresa)
            {
               
                double deuda_empresa = 0;
                double cuit = Convert.ToDouble(em.cuit);
                //obtengo las actas involucradas y tomo el ultimo periodo para calcular la deuda.
                var actas_involucradas = from act in db_socios.ACTAS
                                         where act.CUIT == cuit
                                         select new
                                         {
                                             acta = act.ACTA,
                                             desde = act.DESDE,
                                             hasta = act.HASTA,
                                             estado = act.COBRADOTOTALMENTE,
                                             inspector = act.INSPECTOR,
                                             importe_acta = act.DEUDATOTAL
                                         };

                // Tomo el mayor valor del campo "hasta" y desde del periodo siguiente calculo la deuda. 
                if (actas_involucradas.Count() > 0)
                {
                    //quiere decir que si hay actas involucradas
                    var deuda = ddjj_empresa.Where(x => x.cuit == cuit && x.periodo > actas_involucradas.Max(y => y.hasta));

                    foreach (var d in deuda)
                    {
                        deuda_empresa += d.total_periodo;
                    }
                }
                else //viene por aqui por que no hay actas involucradas
                {
                    var deuda = ddjj_empresa.Where(x => x.cuit == cuit);

                    foreach (var d in deuda)
                    {
                        deuda_empresa += d.total_periodo;
                    }
                    //deuda_empresa = deuda_empresa; // - Convert.ToDouble(actas_involucradas.Sum(x => x.importe_acta));
                }

                dgv_ranking.Rows.Add();
                dgv_ranking.Rows[dgv_ranking.Rows.Count - 1].Cells["cuit"].Value = em.cuit;
                dgv_ranking.Rows[dgv_ranking.Rows.Count - 1].Cells["empresa"].Value = em.Nombre;
                dgv_ranking.Rows[dgv_ranking.Rows.Count - 1].Cells["deuda"].Value = deuda_empresa;
                total_a_cobrar += deuda_empresa;
            }
            dgv_ranking.Sort(this.dgv_ranking.Columns["deuda"],ListSortDirection.Descending);
            lbl_total_a_cobrar.Text = total_a_cobrar.ToString("C2");
            lbl_cant_empresas.Text = dgv_ranking.Rows.Count.ToString();
        }

        private void cbx_nombre_empresa_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void cbx_nombre_empresa_TextUpdate(object sender, EventArgs e)
        {
            //var empr = (from emp in db_socios.maeemp
            //            where emp.MAEEMP_RAZSOC.Contains(cbx_nombre_empresa.Text)
            //            select new
            //            {
            //                cuite = emp.MAEEMP_CUIT,
            //                razon_social = emp.MAEEMP_RAZSOC,
            //                nombre_fantasia = emp.MAEEMP_NOMFAN
            //            }).OrderBy(x => x.razon_social);

            //cbx_nombre_empresa.DisplayMember = "razon_social";
            //cbx_nombre_empresa.ValueMember = "cuite";
            //cbx_nombre_empresa.DataSource = empr.ToList();
        }

        private void txt_cuit_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_quitar_periodo_Click(object sender, EventArgs e)
        {
            dgv_dj_empresa.Rows.Remove(dgv_dj_empresa.CurrentRow);
        }

        private void btn_simular_acta_Click(object sender, EventArgs e)
        {
            while (dgv_simular_acta.Rows.Count != 0)
            {
                dgv_simular_acta.Rows.RemoveAt(0);  
            }

           

            int f = 0;
            string tot = "";
            string per = "";
            foreach (DataGridViewRow fila in dgv_dj_empresa.Rows)
            {
                dgv_simular_acta.Rows.Add();
                f = dgv_simular_acta.Rows.Count - 1;
                dgv_simular_acta.Rows[f].Cells["simulacion_periodo"].Value = fila.Cells["periodo"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_rectificacion"].Value = fila.Cells["rectificacion"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_aporte_ley"].Value = fila.Cells["aporte_ley"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_aporte_socio"].Value = fila.Cells["aporte_socio"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_fecha_pago"].Value = fila.Cells["fecha_pago"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_importe_ps"].Value = fila.Cells["importe_ps"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_empleados"].Value = fila.Cells["empleados"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_socios"].Value = fila.Cells["socios"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_capital"].Value = fila.Cells["capital"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_interes"].Value = fila.Cells["interes"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_total"].Value = fila.Cells["total"].Value;

                tot = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_total"].Value).ToString("C2") ;
                per = Convert.ToDateTime(dgv_simular_acta.Rows[f].Cells["simulacion_periodo"].Value).Date.ToString("d");
                contextMenuStrip1.Items.Add("Periodo: " + per + " -- Total: [" + tot + "]");

                menu_copiar_de.Items.Add(dgv_simular_acta.Rows[f].Cells["simulacion_periodo"].Value.ToString() + "[" + dgv_simular_acta.Rows[f].Cells["simulacion_total"].Value.ToString() +  "]");
                calcular_totales_simulacion_actas();
                //contextMenuStrip1.Items.Add("Periodo: " + Convert.ToDateTime(dgv_simular_acta.Rows[f].Cells["simulacion_periodo"].Value).Date.ToString() + " -- Total: [" +  dgv_simular_acta.Rows[f].Cells["simulacion_total"].Value + "]");
            }

        }

        private void btn_generar_periodos_Click(object sender, EventArgs e)
        {

            //DateTime periodo = Convert.ToDateTime("01/01/2010");
            //for (int i = 0; i < 180; i++)
            //{
            //    secuencia_periodos sec_per = new secuencia_periodos();
            //    sec_per.periodo = periodo.AddMonths(i);
            //    db_socios.secuencia_periodos.InsertOnSubmit(sec_per);
            //}
            //db_socios.SubmitChanges();
            //var list = (from row in dataGridView1.Rows.Cast<DataGridViewRow>()
            //            from cell in row.Cells.Cast<DataGridViewCell>()
            //            select new
            //            {
            //                //project into your new class from the row and cell vars.
            //            }).ToList();

            List<DateTime> lista1 = new List<DateTime>();
            
            foreach (DataGridViewRow fila in dgv_simular_acta.Rows)
            {
                lista1.Add(Convert.ToDateTime(fila.Cells["simulacion_periodo"].Value));
            }

            List<DateTime> lista2 = new List<DateTime>();

            foreach (var fila in db_socios.secuencia_periodos.Where(x => x.periodo >= Convert.ToDateTime("01/" + cbx_desde.Text) && x.periodo <= Convert.ToDateTime("01/" + cbx_hasta.Text)))
            {
                lista2.Add(Convert.ToDateTime(fila.periodo));
            }


            var periodo_faltante = from p in lista2.Except(lista1) select new { periodo = p.Date };

            int f = 0;
            foreach (var item in periodo_faltante.ToList())
            {
                dgv_simular_acta.Rows.Add();
                f = dgv_simular_acta.Rows.Count - 1;
                dgv_simular_acta.Rows[f].DefaultCellStyle.BackColor = Color.MediumVioletRed;
                dgv_simular_acta.Rows[f].Cells["simulacion_periodo"].Value = item.periodo;
                dgv_simular_acta.Rows[f].Cells["simulacion_rectificacion"].Value = 0; // fila.Cells["rectificacion"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_aporte_ley"].Value = 0; //fila.Cells["aporte_ley"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_aporte_socio"].Value = 0; //fila.Cells["aporte_socio"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_fecha_pago"].Value ="NO Declara"; //  fila.Cells["fecha_pago"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_importe_ps"].Value = 0; //fila.Cells["importe_ps"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_empleados"].Value = 0; //fila.Cells["empleados"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_socios"].Value = 0; //fila.Cells["socios"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_capital"].Value = 0; //fila.Cells["capital"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_interes"].Value = 0; //fila.Cells["interes"].Value;
                dgv_simular_acta.Rows[f].Cells["simulacion_total"].Value = 0; //fila.Cells["total"].Value;
            }
            //dgv_ranking.Sort(this.dgv_ranking.Columns["deuda"],ListSortDirection.Descending);
            dgv_simular_acta.Sort(this.dgv_simular_acta.Columns["simulacion_periodo"], ListSortDirection.Ascending);
            calcular_totales_simulacion_actas();
        }

        private void btn_quitar_periodo_simulacion_Click(object sender, EventArgs e)
        {
            dgv_simular_acta.Rows.RemoveAt(dgv_simular_acta.CurrentRow.Index);
            calcular_totales_simulacion_actas();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void dgv_simular_acta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_simular_acta_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menu_copiar_de.Show();
                MessageBox.Show("boton derecho");
                //contextMenuStrip1.Items.Add
                
            }
        }

        private void calcular_totales_simulacion_actas()
        {
            double deuda_total = 0;
            double total_capital = 0;
            double total_interes = 0;

            foreach (DataGridViewRow f in dgv_simular_acta.Rows)
            {
                total_capital = total_capital  + Convert.ToDouble(f.Cells["simulacion_capital"].Value);
                deuda_total = deuda_total + Convert.ToDouble(f.Cells["simulacion_total"].Value);
                total_interes = total_interes +  Convert.ToDouble(f.Cells["simulacion_interes"].Value);
            }
            lbl_simular_acta_total_deuda.Text = deuda_total.ToString("C2");
            lbl_simular_acta_total_capital.Text = total_capital.ToString("C2");
            lbl_simular_acta_total_interes.Text = total_interes.ToString("C2");
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void chk_filtro_localidad_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_filtro_localidad.Checked )
            {
                cbx_localidad.Enabled = true;
            }
            else
            {
                cbx_localidad.Enabled = false;
            }
        }

        private void btn_imprimir_simulacion_acta_Click(object sender, EventArgs e)
        {
            //limpio la tabla de impresion
            var im = from a in db_socios.impresion_comprobante select a;
            foreach (var item in im)
            {
                db_socios.impresion_comprobante.DeleteOnSubmit(item);
                db_socios.SubmitChanges();
            }
            
            for (int f = 0; f < dgv_simular_acta.Rows.Count; f++)
            {
                impresion_comprobante imp = new impresion_comprobante();
                imp.empresa = cbx_nombre_empresa.Text;
                imp.cuit = txt_cuit.Text;
                imp.localidad = txt_localidad.Text;
                imp.telefono = txt_telefono.Text;
                imp.estudio = txt_estudio_contable.Text;
                imp.domicilio = txt_domicilio.Text;
                imp.desde = cbx_desde.Text;
                imp.hasta = cbx_hasta.Text;
                imp.periodo = Convert.ToDateTime (dgv_simular_acta.Rows[f].Cells["simulacion_periodo"].Value);
                imp.rectificacion = Convert.ToInt16(dgv_simular_acta.Rows[f].Cells["simulacion_rectificacion"].Value);
                imp.aporte_ley = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_aporte_ley"].Value);
                imp.aporte_socio = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_aporte_socio"].Value);
                imp.fechapago = (dgv_simular_acta.Rows[f].Cells["simulacion_fecha_pago"].Value != null) ? dgv_simular_acta.Rows[f].Cells["simulacion_fecha_pago"].Value.ToString() :"" ;
                imp.importe_depositado = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_importe_ps"].Value);
                imp.cantidad_empleados = Convert.ToInt16(dgv_simular_acta.Rows[f].Cells["simulacion_empleados"].Value);
                imp.cantidad_socios = Convert.ToInt16(dgv_simular_acta.Rows[f].Cells["simulacion_socios"].Value);
                imp.capital = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_capital"].Value);
                imp.interes = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_interes"].Value);
                imp.total = Convert.ToDecimal(dgv_simular_acta.Rows[f].Cells["simulacion_total"].Value);
                db_socios.impresion_comprobante.InsertOnSubmit(imp);
                db_socios.SubmitChanges();
            }
            reportes frm_reportes = new reportes();
            frm_reportes.nombreReporte = "simulacion_actas"; // la variable nombreReporte la ponemos como accesso global asi le podamos cambiar el datasource del reporte
            frm_reportes.Show();
        }

        private void btn_asignar_inspector_Click(object sender, EventArgs e)
        {
            bool existe_asignacion = db_socios.inspectores_asignacion_empresa.Any(x => x.CUIT_EMPRESA == Convert.ToInt64(cbx_nombre_empresa.SelectedValue.ToString()) && x.DESDE == Convert.ToDateTime("01/" + cbx_desde.Text) && x.HASTA == Convert.ToDateTime("01/" + cbx_hasta.Text)); //.Where(x => x.CUIT_EMPRESA == Convert.ToInt64(cbx_nombre_empresa.SelectedValue.ToString()) && x.DESDE == Convert.ToDateTime("01/" + cbx_desde.Text) && x.HASTA == Convert.ToDateTime("01/" + cbx_hasta.Text)).First();
            if (!existe_asignacion)
            {


                if (MessageBox.Show("Esta seguro de Asignar la empresa " + cbx_nombre_empresa.Text + " al Inspector " + cbx_inspectores.Text, "ATENCION", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    inspectores_asignacion_empresa ins_asig_emp = new inspectores_asignacion_empresa();
                    ins_asig_emp.CUIT_EMPRESA = Convert.ToInt64(cbx_nombre_empresa.SelectedValue.ToString());
                    ins_asig_emp.ID_INSPECTOR = Convert.ToInt32(cbx_inspectores.SelectedValue.ToString());
                    ins_asig_emp.FECHA_ASIGNACION = DateTime.Now;
                    ins_asig_emp.DESDE = Convert.ToDateTime("01/" + cbx_desde.Text);
                    ins_asig_emp.HASTA = Convert.ToDateTime("01/" + cbx_hasta.Text);
                    ins_asig_emp.DEUDA_APROXIMADA = Convert.ToDecimal(lbl_total_con_interes.Text);
                    ins_asig_emp.ESTADO = 0;
                    db_socios.inspectores_asignacion_empresa.InsertOnSubmit(ins_asig_emp);
                    db_socios.SubmitChanges();
                    MessageBox.Show("EL inspector " + cbx_inspectores.Text + " se asigno exitosamente.", "Asignacion de Inspectores");
                }
                else
                {
                    MessageBox.Show("EL inspector " + cbx_inspectores.Text + " no fue asignado.", "Asignacion de Inspectores");
                }
            }
            else
            {
                MessageBox.Show("La Empresa " + cbx_nombre_empresa.Text + " Ya tiene asignada un inspctor en ese periodo ", "Asignacion de Inspectores");
                
            }
        }

        private void generar_seguimiento()
        {
            double total_deuda = 0;

            if (Convert.ToInt32(cbx_seguimiento_inspector.SelectedValue) != 7) // SELECCIONO TODOS
            {
                var segui = (from s in db_socios.inspectores_asignacion_empresa
                            where s.ID_INSPECTOR == (Convert.ToInt32(cbx_seguimiento_inspector.SelectedValue))
                            join empr in db_socios.maeemp on s.CUIT_EMPRESA equals empr.MAEEMP_CUIT
                            select new
                            {
                                asig_fecha = s.FECHA_ASIGNACION.Date,
                                asig_empresa = db_socios.maeemp.Where(x => x.MAEEMP_CUIT == s.CUIT_EMPRESA).First().MAEEMP_RAZSOC,
                                asig_desde = s.DESDE,
                                asig_hasta = s.HASTA,
                                asig_deuda = s.DEUDA_APROXIMADA,
                                asig_inspector = db_socios.inspectores.Where(x => x.ID_INSPECTOR == s.ID_INSPECTOR).Single().APELLIDO,
                                asig_fecha_generacion_aviso = s.FECHA_GENERACION_AVISO,
                                asig_fecha_entrega_aviso = s.FECHA_ENTREGA_AVISO,
                                asig_nro_aviso = s.NRO_AVISO,
                                asig_acta = s.ACTA_GENERADA,
                                asig_estado = s.ESTADO,
                                asig_dias = (DateTime.Today - s.FECHA_ASIGNACION.Date).Days,
                            }).OrderByDescending(x => x.asig_deuda);
                dgv_seguimiento.DataSource = segui.ToList();
            }
            else
            {
                var segui = (from s in db_socios.inspectores_asignacion_empresa
                         
                           join empr in db_socios.maeemp on s.CUIT_EMPRESA equals empr.MAEEMP_CUIT
                           select new
                           {
                               asig_fecha = s.FECHA_ASIGNACION.Date,
                               asig_empresa = db_socios.maeemp.Where(x => x.MAEEMP_CUIT == s.CUIT_EMPRESA).First().MAEEMP_RAZSOC,
                               asig_desde = s.DESDE,
                               asig_hasta = s.HASTA,
                               asig_deuda = s.DEUDA_APROXIMADA,
                               asig_inspector = db_socios.inspectores.Where(x => x.ID_INSPECTOR == s.ID_INSPECTOR).Single().APELLIDO,
                               asig_fecha_generacion_aviso = s.FECHA_GENERACION_AVISO,
                               asig_fecha_entrega_aviso = s.FECHA_ENTREGA_AVISO,
                               asig_nro_aviso = s.NRO_AVISO,
                               asig_acta = s.ACTA_GENERADA,
                               asig_estado = s.ESTADO,
                               asig_dias = (DateTime.Today - s.FECHA_ASIGNACION.Date).Days,
                           }).OrderByDescending(x=>x.asig_deuda);
                dgv_seguimiento.DataSource = segui.ToList();
            }
            foreach (DataGridViewRow fila in dgv_seguimiento.Rows)
            {
                total_deuda = total_deuda + Convert.ToDouble(fila.Cells["asignacion_importe"].Value);
            }
            lbl_seguimiento_total_deuda.Text = total_deuda.ToString("C2");
            lbl_seguimiento_total_empresas.Text = dgv_seguimiento.Rows.Count.ToString();

        }

        private void btn_aplicar_filtro_Click(object sender, EventArgs e)
        {
            generar_seguimiento();
        }

        private void cbx_seguimiento_inspector_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
