namespace entrega_cupones
{
    partial class reportes_CR
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
            this.CRV = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.actas_involucradas1 = new entrega_cupones.actas_involucradas();
            //this.rpt_cobros_actas1 = new entrega_cupones.rpt_cobros_actas();
            this.SuspendLayout();
            // 
            // CRV
            // 
            this.CRV.ActiveViewIndex = 0;
            this.CRV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRV.Cursor = System.Windows.Forms.Cursors.Default;
            this.CRV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CRV.Location = new System.Drawing.Point(0, 0);
            this.CRV.Name = "CRV";
            //this.CRV.ReportSource = this.rpt_cobros_actas1;
            this.CRV.Size = new System.Drawing.Size(800, 450);
            this.CRV.TabIndex = 0;
            this.CRV.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // reportes_CR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CRV);
            this.Name = "reportes_CR";
            this.Text = "reportes_CR";
            this.Load += new System.EventHandler(this.reportes_CR_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CRV;
        private actas_involucradas actas_involucradas1;
        //private rpt_cobros_actas rpt_cobros_actas1;
        //private rpt_empleados rpt_empleados1;
        //private rpt_edades rpt_edades1;
        //private Actas_Invo Actas_Invo1;
        //private actas_involucradas actas_involucradas1;
    }
}