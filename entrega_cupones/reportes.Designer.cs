namespace entrega_cupones
{
    partial class reportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.impresion_comprobanteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DS_cupones = new entrega_cupones.DS_cupones();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.imprimir_cuponBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.imprimir_cuponTableAdapter = new entrega_cupones.DS_cuponesTableAdapters.imprimir_cuponTableAdapter();
            this.impresion_comprobanteTableAdapter = new entrega_cupones.DS_cuponesTableAdapters.impresion_comprobanteTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.impresion_comprobanteBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DS_cupones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imprimir_cuponBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // impresion_comprobanteBindingSource
            // 
            this.impresion_comprobanteBindingSource.DataMember = "impresion_comprobante";
            this.impresion_comprobanteBindingSource.DataSource = this.DS_cupones;
            // 
            // DS_cupones
            // 
            this.DS_cupones.DataSetName = "DS_cupones";
            this.DS_cupones.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.impresion_comprobanteBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "entrega_cupones.planilla_partidos.rdlc";
            this.reportViewer1.LocalReport.ReportPath = "";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(770, 399);
            this.reportViewer1.TabIndex = 0;
            // 
            // imprimir_cuponBindingSource
            // 
            this.imprimir_cuponBindingSource.DataMember = "imprimir_cupon";
            this.imprimir_cuponBindingSource.DataSource = this.DS_cupones;
            // 
            // imprimir_cuponTableAdapter
            // 
            this.imprimir_cuponTableAdapter.ClearBeforeFill = true;
            // 
            // impresion_comprobanteTableAdapter
            // 
            this.impresion_comprobanteTableAdapter.ClearBeforeFill = true;
            // 
            // reportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 399);
            this.Controls.Add(this.reportViewer1);
            this.Name = "reportes";
            this.Text = "reportes";
            this.Load += new System.EventHandler(this.reportes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.impresion_comprobanteBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DS_cupones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imprimir_cuponBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource imprimir_cuponBindingSource;
        private DS_cupones DS_cupones;
        private DS_cuponesTableAdapters.imprimir_cuponTableAdapter imprimir_cuponTableAdapter;
        private System.Windows.Forms.BindingSource impresion_comprobanteBindingSource;
        private DS_cuponesTableAdapters.impresion_comprobanteTableAdapter impresion_comprobanteTableAdapter;
        public Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}