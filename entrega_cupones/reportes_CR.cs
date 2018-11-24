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
    public partial class reportes_CR : Form
    {
        lts_sindicatoDataContext db_sindicato = new lts_sindicatoDataContext();

        public reportes_CR(string empresa, string localidad, string cuit, string estudio, string desde, string hasta, string domicilio,
                    string per_pagado, string per_NO_pagado, string per_NO_declarado, string per_total, 
                    string aporte_ley, string aporte_socio, string total_aportes, string depositado, string intereses, 
                    string total_capital, string total_deuda, double cuit2
            
            )
        //txt_actas_pagados.Text, txt_actas_no_pagados.Text, txt_actas_no_declarados.Text,  txt_actas_total_periodos.Text,
        //txt_actas_aporte2.Text, txt_actas_aportes_socio.Text, txt_actas_depositado.Text, txt_actas_intereses.Text, 
        //txt_actas_total_con_interes.Text
        {
            InitializeComponent();
            rpt_consulta_empresa CR = new rpt_consulta_empresa();
            //CR.DataSourceConnections = db_sindicato.impresion_comprobante;
            CR.DataDefinition.FormulaFields["empresa"].Text = "'" +  empresa + "'";
            CR.DataDefinition.FormulaFields["localidad"].Text = "'" + localidad + "'";
            CR.DataDefinition.FormulaFields["cuit"].Text = "'" + cuit + "'";
            CR.DataDefinition.FormulaFields["estudio"].Text = "'" + estudio + "'";
            CR.DataDefinition.FormulaFields["desde"].Text = "'" + desde + "'";
            CR.DataDefinition.FormulaFields["hasta"].Text = "'" + hasta + "'";//"'" + hasta.Substring(0, 2) + "-" + hasta.Substring(3, 8) + "-" + hasta.Substring(9, 1) + "'";
            CR.DataDefinition.FormulaFields["domicilio"].Text = "'" + domicilio + "'";
            CR.DataDefinition.FormulaFields["per_pagado"].Text = "'" + per_pagado + "'";
            CR.DataDefinition.FormulaFields["per_NO_pagado"].Text = "'" + per_NO_pagado + "'";
            CR.DataDefinition.FormulaFields["per_NO_declarado"].Text = "'" + per_NO_declarado + "'";
            CR.DataDefinition.FormulaFields["per_total"].Text = "'" + per_total + "'";
            CR.DataDefinition.FormulaFields["aporte_ley"].Text = "'" + aporte_ley + "'";
            CR.DataDefinition.FormulaFields["aporte_total"].Text = "'" + total_aportes + "'";
            CR.DataDefinition.FormulaFields["aporte_socio"].Text = "'" + aporte_socio + "'";
            CR.DataDefinition.FormulaFields["depositado"].Text = "'" + depositado + "'";
            CR.DataDefinition.FormulaFields["intereses"].Text = "'" + intereses + "'";
            CR.DataDefinition.FormulaFields["capital"].Text = "'" + total_capital + "'";
            CR.DataDefinition.FormulaFields["total_deuda"].Text = "'" + total_deuda + "'";

            var actas_involucradas = (from act in db_sindicato.ACTAS
                                      where act.CUIT == Convert.ToInt64(cuit2)
                                      select new
                                      {
                                          Fecha = String.Format("{0:d}", act.DESDE),// act.FECHA == null ? "" :  act.FECHA.ToString(),
                                          Acta = Convert.ToInt64(act.ACTA).ToString(),// act.ACTA == null ? "" : act.ACTA.ToString(),//.ToString(),   
                                          //cuit = act.CUIT.ToString() ,
                                          Desde = String.Format("{0:MM/yyyy}", act.DESDE) ,// act.DESDE.Value.Month.ToString() + "/" + act.DESDE.Value.Year.ToString() ,
                                          Hasta = String.Format("{0:MM/yyyy}", act.HASTA) ,// act.HASTA.Value.Month.ToString() + "/" + act.HASTA.Value.Year.ToString() ,
                                          Deudatotal = String.Format("{0:C}", act.DEUDATOTAL),
                                          Cobradototalmente = act.COBRADOTOTALMENTE == null ? "NO" : act.COBRADOTOTALMENTE,
                                          Inspector = act.INSPECTOR
                                      }).ToList();

            CR.Subreports[0].SetDataSource(actas_involucradas);

            var act_ = actas_involucradas.ToList();
            var comprobantes_actas = (from comp in db_sindicato.COBROS
                                     where comp.CUIT ==   Convert.ToDouble(cuit2)
                                     select new
                                     {
                                         //cobro_id = comp.Id,
                                         //cuota = (comp.CONCEPTO == "2") ? (comp.CUOTAX.ToString() + " de " + comp.CANTIDAD_CUOTAS.ToString()) : ("Anticipo"),
                                         //fecha_venc = comp.FECHA_VENC,
                                         acta = comp.ACTA,
                                         FECHARECAUDACION = comp.FECHARECAUDACION,
                                         RECIBO = comp.RECIBO,
                                         TOTAL = comp.TOTAL
                                     }).ToList();

            
            foreach (var item in comprobantes_actas)
            {
                impresion_actas comp_act = new impresion_actas();
                comp_act.num3 = Convert.ToDecimal(item.acta);
                comp_act.fecha1 = item.FECHARECAUDACION;
                comp_act.num1 = Convert.ToDecimal(item.RECIBO);
                comp_act.importe = Convert.ToDecimal(item.TOTAL);
                db_sindicato.impresion_actas.InsertOnSubmit(comp_act);
                db_sindicato.SubmitChanges();
            }

            //CR.Subreports[1].SetDataSource(db_sindicato.impresion_actas);
            //CR.Subreports[1].Refresh();
            CRV.ReportSource = CR;
            
            CRV.RefreshReport();
        }

        public  reportes_CR(string ss)
        {
            InitializeComponent();
            rpt_edades CR = new rpt_edades();
            CR.DataDefinition.FormulaFields["edad"].Text = "3";
            //CR.SetDataSource(ss.ToList());
                         
            CRV.Refresh();
            CRV.ReportSource = CR;
            
        }

        public reportes_CR(int emp)
        {
            InitializeComponent();
            rpt_empleados emple = new rpt_empleados();
            CRV.Refresh();
            CRV.RefreshReport();
            CRV.ReportSource = emple;
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

        private void reportes_CR_Load(object sender, EventArgs e)
        {

        }
    }
}
