namespace entrega_cupones
{
    partial class frm_cargar_actas_cmbt
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
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.bunifuCustomLabel1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_cerrar_actas = new Bunifu.Framework.UI.BunifuImageButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cerrar_actas)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Century Gothic", 18.75F, System.Drawing.FontStyle.Bold);
            this.bunifuCustomLabel1.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(9, 11);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(510, 29);
            this.bunifuCustomLabel1.TabIndex = 149;
            this.bunifuCustomLabel1.Text = "Carga de Actas - Ingreso Comprobantes";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Tomato;
            this.panel2.Controls.Add(this.bunifuCustomLabel1);
            this.panel2.Controls.Add(this.btn_cerrar_actas);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(695, 51);
            this.panel2.TabIndex = 4;
            // 
            // btn_cerrar_actas
            // 
            this.btn_cerrar_actas.BackColor = System.Drawing.Color.Transparent;
            this.btn_cerrar_actas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_cerrar_actas.Image = global::entrega_cupones.Properties.Resources.cross_close_or_delete_circular_interface_button_symbol;
            this.btn_cerrar_actas.ImageActive = null;
            this.btn_cerrar_actas.Location = new System.Drawing.Point(655, 10);
            this.btn_cerrar_actas.Name = "btn_cerrar_actas";
            this.btn_cerrar_actas.Size = new System.Drawing.Size(31, 30);
            this.btn_cerrar_actas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btn_cerrar_actas.TabIndex = 148;
            this.btn_cerrar_actas.TabStop = false;
            this.btn_cerrar_actas.Zoom = 10;
            // 
            // frm_cargar_actas_cmbt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 450);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_cargar_actas_cmbt";
            this.Text = "frm_cargar_actas_cmbt";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cerrar_actas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel1;
        private Bunifu.Framework.UI.BunifuImageButton btn_cerrar_actas;
    }
}