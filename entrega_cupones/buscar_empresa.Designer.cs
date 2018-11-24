namespace entrega_cupones
{
    partial class frm_buscar_empresa
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_buscar_empresa));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.bunifuCustomLabel1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.btn_cerrar_busqueda = new Bunifu.Framework.UI.BunifuImageButton();
            this.btn_cerrar = new Bunifu.Framework.UI.BunifuImageButton();
            this.dgv_buscar_empresas = new System.Windows.Forms.DataGridView();
            this.cuit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.razonsocial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombrefantasia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_estado_empresa = new Bunifu.Framework.UI.BunifuFlatButton();
            this.txt_buscar_empresa = new System.Windows.Forms.TextBox();
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuCustomLabel13 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lbl_cantidad_empresas_encontradas = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.txt_buscar_empresa_domicilio = new System.Windows.Forms.TextBox();
            this.bunifuCustomLabel2 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bunifuCustomLabel3 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.txt_buscar_empresa_telefono = new System.Windows.Forms.TextBox();
            this.bunifuCustomLabel4 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.txt_localidad = new System.Windows.Forms.TextBox();
            this.bunifuCustomLabel5 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.txt_buscar_empresa_estudio = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cerrar_busqueda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_buscar_empresas)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Tomato;
            this.panel2.Controls.Add(this.bunifuCustomLabel1);
            this.panel2.Controls.Add(this.btn_cerrar_busqueda);
            this.panel2.Controls.Add(this.btn_cerrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(645, 51);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Century Gothic", 18.75F, System.Drawing.FontStyle.Bold);
            this.bunifuCustomLabel1.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(9, 11);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(299, 29);
            this.bunifuCustomLabel1.TabIndex = 149;
            this.bunifuCustomLabel1.Text = "Busqueda de Empresas";
            // 
            // btn_cerrar_busqueda
            // 
            this.btn_cerrar_busqueda.BackColor = System.Drawing.Color.Transparent;
            this.btn_cerrar_busqueda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_cerrar_busqueda.Image = global::entrega_cupones.Properties.Resources.cross_close_or_delete_circular_interface_button_symbol;
            this.btn_cerrar_busqueda.ImageActive = null;
            this.btn_cerrar_busqueda.Location = new System.Drawing.Point(606, 9);
            this.btn_cerrar_busqueda.Name = "btn_cerrar_busqueda";
            this.btn_cerrar_busqueda.Size = new System.Drawing.Size(31, 30);
            this.btn_cerrar_busqueda.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btn_cerrar_busqueda.TabIndex = 148;
            this.btn_cerrar_busqueda.TabStop = false;
            this.btn_cerrar_busqueda.Zoom = 10;
            this.btn_cerrar_busqueda.Click += new System.EventHandler(this.btn_cerrar_busqueda_Click);
            // 
            // btn_cerrar
            // 
            this.btn_cerrar.BackColor = System.Drawing.Color.Transparent;
            this.btn_cerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_cerrar.Image = global::entrega_cupones.Properties.Resources.cross_close_or_delete_circular_interface_button_symbol;
            this.btn_cerrar.ImageActive = null;
            this.btn_cerrar.Location = new System.Drawing.Point(1297, 3);
            this.btn_cerrar.Name = "btn_cerrar";
            this.btn_cerrar.Size = new System.Drawing.Size(31, 30);
            this.btn_cerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btn_cerrar.TabIndex = 147;
            this.btn_cerrar.TabStop = false;
            this.btn_cerrar.Zoom = 10;
            // 
            // dgv_buscar_empresas
            // 
            this.dgv_buscar_empresas.AllowUserToAddRows = false;
            this.dgv_buscar_empresas.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgv_buscar_empresas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_buscar_empresas.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_buscar_empresas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_buscar_empresas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_buscar_empresas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cuit,
            this.razonsocial,
            this.nombrefantasia});
            this.dgv_buscar_empresas.Location = new System.Drawing.Point(6, 116);
            this.dgv_buscar_empresas.Name = "dgv_buscar_empresas";
            this.dgv_buscar_empresas.ReadOnly = true;
            this.dgv_buscar_empresas.RowHeadersVisible = false;
            this.dgv_buscar_empresas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_buscar_empresas.Size = new System.Drawing.Size(631, 186);
            this.dgv_buscar_empresas.TabIndex = 206;
            this.dgv_buscar_empresas.SelectionChanged += new System.EventHandler(this.dgv_buscar_empresas_SelectionChanged);
            this.dgv_buscar_empresas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_buscar_empresas_KeyDown);
            this.dgv_buscar_empresas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgv_buscar_empresas_MouseDoubleClick);
            // 
            // cuit
            // 
            this.cuit.DataPropertyName = "_cuit";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.cuit.DefaultCellStyle = dataGridViewCellStyle3;
            this.cuit.HeaderText = "C.U.I.T.";
            this.cuit.Name = "cuit";
            this.cuit.ReadOnly = true;
            // 
            // razonsocial
            // 
            this.razonsocial.DataPropertyName = "_razonsocial";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.razonsocial.DefaultCellStyle = dataGridViewCellStyle4;
            this.razonsocial.HeaderText = "Razon Social";
            this.razonsocial.Name = "razonsocial";
            this.razonsocial.ReadOnly = true;
            this.razonsocial.Width = 250;
            // 
            // nombrefantasia
            // 
            this.nombrefantasia.DataPropertyName = "_nombre_fantasia";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.nombrefantasia.DefaultCellStyle = dataGridViewCellStyle5;
            this.nombrefantasia.HeaderText = "Nombre Fantasia";
            this.nombrefantasia.Name = "nombrefantasia";
            this.nombrefantasia.ReadOnly = true;
            this.nombrefantasia.Width = 250;
            // 
            // btn_estado_empresa
            // 
            this.btn_estado_empresa.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(174)))), ((int)(((byte)(70)))));
            this.btn_estado_empresa.BackColor = System.Drawing.Color.Tomato;
            this.btn_estado_empresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_estado_empresa.BorderRadius = 5;
            this.btn_estado_empresa.ButtonText = "Aceptar";
            this.btn_estado_empresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_estado_empresa.DisabledColor = System.Drawing.Color.Gray;
            this.btn_estado_empresa.Iconcolor = System.Drawing.Color.Transparent;
            this.btn_estado_empresa.Iconimage = ((System.Drawing.Image)(resources.GetObject("btn_estado_empresa.Iconimage")));
            this.btn_estado_empresa.Iconimage_right = null;
            this.btn_estado_empresa.Iconimage_right_Selected = null;
            this.btn_estado_empresa.Iconimage_Selected = null;
            this.btn_estado_empresa.IconMarginLeft = 0;
            this.btn_estado_empresa.IconMarginRight = 0;
            this.btn_estado_empresa.IconRightVisible = false;
            this.btn_estado_empresa.IconRightZoom = 0D;
            this.btn_estado_empresa.IconVisible = true;
            this.btn_estado_empresa.IconZoom = 50D;
            this.btn_estado_empresa.IsTab = false;
            this.btn_estado_empresa.Location = new System.Drawing.Point(541, 77);
            this.btn_estado_empresa.Name = "btn_estado_empresa";
            this.btn_estado_empresa.Normalcolor = System.Drawing.Color.Tomato;
            this.btn_estado_empresa.OnHovercolor = System.Drawing.Color.Lime;
            this.btn_estado_empresa.OnHoverTextColor = System.Drawing.Color.White;
            this.btn_estado_empresa.selected = false;
            this.btn_estado_empresa.Size = new System.Drawing.Size(96, 33);
            this.btn_estado_empresa.TabIndex = 210;
            this.btn_estado_empresa.Text = "Aceptar";
            this.btn_estado_empresa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_estado_empresa.Textcolor = System.Drawing.Color.White;
            this.btn_estado_empresa.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_estado_empresa.Click += new System.EventHandler(this.btn_estado_empresa_Click);
            // 
            // txt_buscar_empresa
            // 
            this.txt_buscar_empresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_buscar_empresa.Location = new System.Drawing.Point(14, 82);
            this.txt_buscar_empresa.Name = "txt_buscar_empresa";
            this.txt_buscar_empresa.Size = new System.Drawing.Size(460, 23);
            this.txt_buscar_empresa.TabIndex = 0;
            this.txt_buscar_empresa.TextChanged += new System.EventHandler(this.txt_buscar_empresa_TextChanged);
            this.txt_buscar_empresa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_buscar_empresa_KeyDown);
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.panel2;
            this.bunifuDragControl1.Vertical = true;
            // 
            // bunifuCustomLabel13
            // 
            this.bunifuCustomLabel13.AutoSize = true;
            this.bunifuCustomLabel13.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.bunifuCustomLabel13.ForeColor = System.Drawing.Color.Black;
            this.bunifuCustomLabel13.Location = new System.Drawing.Point(12, 61);
            this.bunifuCustomLabel13.Name = "bunifuCustomLabel13";
            this.bunifuCustomLabel13.Size = new System.Drawing.Size(347, 19);
            this.bunifuCustomLabel13.TabIndex = 213;
            this.bunifuCustomLabel13.Text = "Buscar por CUIT / Razon Social / Nombre Fantasia";
            // 
            // lbl_cantidad_empresas_encontradas
            // 
            this.lbl_cantidad_empresas_encontradas.AutoSize = true;
            this.lbl_cantidad_empresas_encontradas.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.lbl_cantidad_empresas_encontradas.ForeColor = System.Drawing.Color.Black;
            this.lbl_cantidad_empresas_encontradas.Location = new System.Drawing.Point(487, 305);
            this.lbl_cantidad_empresas_encontradas.Name = "lbl_cantidad_empresas_encontradas";
            this.lbl_cantidad_empresas_encontradas.Size = new System.Drawing.Size(117, 19);
            this.lbl_cantidad_empresas_encontradas.TabIndex = 215;
            this.lbl_cantidad_empresas_encontradas.Text = "Total Empresas: ";
            this.lbl_cantidad_empresas_encontradas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_buscar_empresa_domicilio
            // 
            this.txt_buscar_empresa_domicilio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_buscar_empresa_domicilio.Location = new System.Drawing.Point(87, 310);
            this.txt_buscar_empresa_domicilio.Name = "txt_buscar_empresa_domicilio";
            this.txt_buscar_empresa_domicilio.Size = new System.Drawing.Size(340, 23);
            this.txt_buscar_empresa_domicilio.TabIndex = 216;
            // 
            // bunifuCustomLabel2
            // 
            this.bunifuCustomLabel2.AutoSize = true;
            this.bunifuCustomLabel2.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.bunifuCustomLabel2.ForeColor = System.Drawing.Color.Black;
            this.bunifuCustomLabel2.Location = new System.Drawing.Point(2, 310);
            this.bunifuCustomLabel2.Name = "bunifuCustomLabel2";
            this.bunifuCustomLabel2.Size = new System.Drawing.Size(75, 19);
            this.bunifuCustomLabel2.TabIndex = 217;
            this.bunifuCustomLabel2.Text = "Domicilio:";
            this.bunifuCustomLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bunifuCustomLabel3
            // 
            this.bunifuCustomLabel3.AutoSize = true;
            this.bunifuCustomLabel3.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.bunifuCustomLabel3.ForeColor = System.Drawing.Color.Black;
            this.bunifuCustomLabel3.Location = new System.Drawing.Point(2, 339);
            this.bunifuCustomLabel3.Name = "bunifuCustomLabel3";
            this.bunifuCustomLabel3.Size = new System.Drawing.Size(70, 19);
            this.bunifuCustomLabel3.TabIndex = 219;
            this.bunifuCustomLabel3.Text = "Telefono:";
            this.bunifuCustomLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_buscar_empresa_telefono
            // 
            this.txt_buscar_empresa_telefono.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_buscar_empresa_telefono.Location = new System.Drawing.Point(87, 339);
            this.txt_buscar_empresa_telefono.Name = "txt_buscar_empresa_telefono";
            this.txt_buscar_empresa_telefono.Size = new System.Drawing.Size(168, 23);
            this.txt_buscar_empresa_telefono.TabIndex = 218;
            // 
            // bunifuCustomLabel4
            // 
            this.bunifuCustomLabel4.AutoSize = true;
            this.bunifuCustomLabel4.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.bunifuCustomLabel4.ForeColor = System.Drawing.Color.Black;
            this.bunifuCustomLabel4.Location = new System.Drawing.Point(264, 339);
            this.bunifuCustomLabel4.Name = "bunifuCustomLabel4";
            this.bunifuCustomLabel4.Size = new System.Drawing.Size(83, 19);
            this.bunifuCustomLabel4.TabIndex = 221;
            this.bunifuCustomLabel4.Text = "Localidad:";
            this.bunifuCustomLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_localidad
            // 
            this.txt_localidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_localidad.Location = new System.Drawing.Point(349, 339);
            this.txt_localidad.Name = "txt_localidad";
            this.txt_localidad.Size = new System.Drawing.Size(190, 23);
            this.txt_localidad.TabIndex = 220;
            // 
            // bunifuCustomLabel5
            // 
            this.bunifuCustomLabel5.AutoSize = true;
            this.bunifuCustomLabel5.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.bunifuCustomLabel5.ForeColor = System.Drawing.Color.Black;
            this.bunifuCustomLabel5.Location = new System.Drawing.Point(2, 368);
            this.bunifuCustomLabel5.Name = "bunifuCustomLabel5";
            this.bunifuCustomLabel5.Size = new System.Drawing.Size(62, 19);
            this.bunifuCustomLabel5.TabIndex = 223;
            this.bunifuCustomLabel5.Text = "Estudio:";
            this.bunifuCustomLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_buscar_empresa_estudio
            // 
            this.txt_buscar_empresa_estudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_buscar_empresa_estudio.Location = new System.Drawing.Point(87, 368);
            this.txt_buscar_empresa_estudio.Name = "txt_buscar_empresa_estudio";
            this.txt_buscar_empresa_estudio.Size = new System.Drawing.Size(546, 23);
            this.txt_buscar_empresa_estudio.TabIndex = 222;
            // 
            // frm_buscar_empresa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(645, 406);
            this.Controls.Add(this.bunifuCustomLabel5);
            this.Controls.Add(this.txt_buscar_empresa_estudio);
            this.Controls.Add(this.bunifuCustomLabel4);
            this.Controls.Add(this.txt_localidad);
            this.Controls.Add(this.bunifuCustomLabel3);
            this.Controls.Add(this.txt_buscar_empresa_telefono);
            this.Controls.Add(this.bunifuCustomLabel2);
            this.Controls.Add(this.txt_buscar_empresa_domicilio);
            this.Controls.Add(this.lbl_cantidad_empresas_encontradas);
            this.Controls.Add(this.txt_buscar_empresa);
            this.Controls.Add(this.btn_estado_empresa);
            this.Controls.Add(this.dgv_buscar_empresas);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.bunifuCustomLabel13);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_buscar_empresa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "buscar_empresa";
            this.Load += new System.EventHandler(this.frm_buscar_empresa_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cerrar_busqueda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_buscar_empresas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.Framework.UI.BunifuImageButton btn_cerrar;
        private Bunifu.Framework.UI.BunifuImageButton btn_cerrar_busqueda;
        private System.Windows.Forms.DataGridView dgv_buscar_empresas;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel1;
        private Bunifu.Framework.UI.BunifuFlatButton btn_estado_empresa;
        private System.Windows.Forms.TextBox txt_buscar_empresa;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel13;
        private Bunifu.Framework.UI.BunifuCustomLabel lbl_cantidad_empresas_encontradas;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel3;
        private System.Windows.Forms.TextBox txt_buscar_empresa_telefono;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel2;
        private System.Windows.Forms.TextBox txt_buscar_empresa_domicilio;
        private System.Windows.Forms.DataGridViewTextBoxColumn cuit;
        private System.Windows.Forms.DataGridViewTextBoxColumn razonsocial;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombrefantasia;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel4;
        private System.Windows.Forms.TextBox txt_localidad;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel5;
        private System.Windows.Forms.TextBox txt_buscar_empresa_estudio;
    }
}