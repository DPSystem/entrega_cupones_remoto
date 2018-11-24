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
using Microsoft.Reporting.WinForms;

namespace entrega_cupones
{
    public partial class reportes : Form
    {
        

        public reportes()
        {
            InitializeComponent();
        }
        public int Nro_cupon { get; set; }
        public string nombreReporte { get; set; }

        private void reportes_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DS_cupones.impresion_comprobante' Puede moverla o quitarla según sea necesario.
            this.impresion_comprobanteTableAdapter.Fill(this.DS_cupones.impresion_comprobante);
            //this.impresion_actasTableAdapter1.Fill(DS_cupones.impresion_actas);
            // TODO: esta línea de código carga datos en la tabla 'DS_cupones.imprimir_cupon' Puede moverla o quitarla según sea necesario.
            //this.imprimir_cuponTableAdapter.Fill(this.DS_cupones.imprimir_cupon,12);
            this.reportViewer1.RefreshReport();
            cambiar_reporte(nombreReporte);
        }

        private void cambiar_reporte (string nombre_reporte)
        {
            ReportDataSource nueva_fuente_de_datos = new ReportDataSource();
            reportViewer1.LocalReport.DataSources.Clear();
            if (nombre_reporte == "planilla_partidos")
            {
                reportViewer1.LocalReport.ReportEmbeddedResource = "entrega_cupones.planilla_partidos.rdlc";
                //reportViewer1.LocalReport.ReportPath = Path.Combine(entrega_cupones.reportes, "planilla_partidos.rdlc");
                nueva_fuente_de_datos.Name = "DataSet1";
                nueva_fuente_de_datos.Value = impresion_comprobanteBindingSource;
                reportViewer1.LocalReport.DataSources.Add(nueva_fuente_de_datos);
            }
            if (nombre_reporte == "planilla_arbitro")
            {
                reportViewer1.LocalReport.ReportEmbeddedResource = "entrega_cupones.informe_arbitro.rdlc";
                nueva_fuente_de_datos.Name = "DataSet1";
                nueva_fuente_de_datos.Value = impresion_comprobanteBindingSource;
                reportViewer1.LocalReport.DataSources.Add(nueva_fuente_de_datos);
            }

            if (nombre_reporte == "simulacion_actas")
            {
                reportViewer1.LocalReport.ReportEmbeddedResource = "entrega_cupones.simulacion_actas.rdlc";// "entrega_cupones.simulacion_actas.rdlc"; // 
               //   "D:\Proyectos\entrega_cupones\entrega_cupones\Reportes\simulacion_actas.rdlc"
               //  "\\D:\\Proyectos\\entrega_cupones\\entrega_cupones\\Reportes\\simulacion_actas.rdlc"
                nueva_fuente_de_datos.Name = "DataSet1";
                nueva_fuente_de_datos.Value = impresion_comprobanteBindingSource;
                reportViewer1.LocalReport.DataSources.Add(nueva_fuente_de_datos);

                //nueva_fuente_de_datos.Name = "DataSet2";
                //nueva_fuente_de_datos.Value = impresion_actasBindingsource;
                //reportViewer1.LocalReport.DataSources.Add(nueva_fuente_de_datos);
            }
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
