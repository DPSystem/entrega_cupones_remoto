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
    public partial class Frm_socios : Form
    {
        lts_sindicatoDataContext db_socios = new lts_sindicatoDataContext();
        public Frm_socios()
        {
            InitializeComponent();
        }
 
        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            separador_1.Visible = true;
            separador_2.Visible = false;
            lbl_1.Text = "Ingrese los Datos";

        }

        private void btn_modificar_Click(object sender, EventArgs e)
        {
            separador_1.Visible = false;
            separador_2.Visible = true;
            lbl_1.Text = "Modifique los Datos";
        }

        private void frm_clientes_Load(object sender, EventArgs e)
        {
            separador_1.Visible = true;
            separador_2.Visible = false;
            Actualizar_cupones_entregados();
            Rbtn_dni_cupon_entregado.Checked = true;

          }

        private void Actualizar_cupones_entregados() { 
            /*var cupones_entregados = (from a in db_socios.socios.Where(x=>x.fiesta_fin_año != null)
                         select new
                         {
                             numero_socio = a.socio,
                             dni_socio = a.DNI,
                             apeynom = a.ApellidoyNombre,
                             empresa = a.Empresa,
                             num_cupon = a.numero_cupon,
                             fecha_entrega = a.fiesta_fin_año_fecha_entrega
                         }).OrderBy(x => x.apeynom);
            Dgv_cupones_entregados.DataSource = cupones_entregados.ToList();
            Lbl_cupones_entregados.Text = "Cupones Entregados: " + Convert.ToString( cupones_entregados.Count());
            */
        }

        private void Buscar_cupones_entregados(string dato)
        {
            /*if (dato == "") // si el parametro "dato" a buscar no contiene nada entonces se muestran todos los socios
            {
                var cupones = (from a in db_socios.socios.Where(x => x.fiesta_fin_año != null )
                               select new
                                {
                                    numero_socio = a.socio,
                                    dni_socio = a.DNI,
                                    apeynom = a.ApellidoyNombre,
                                    empresa = a.Empresa,
                                    num_cupon = a.numero_cupon,
                                    fecha_entrega = a.fiesta_fin_año_fecha_entrega
                                }).OrderBy(x => x.apeynom);
                Dgv_cupones_entregados.DataSource = cupones.ToList();
                Lbl_cupones_entregados.Text = "Cupones Entregados: " + Convert.ToString(cupones.Count());
            }
            else
            {
                if (Rbtn_dni_cupon_entregado.Checked) // si esta chequeado buscar por DNI
                {
                    var cupones = (from a in db_socios.socios
                                    where (a.DNI.Contains(dato)) && (a.fiesta_fin_año  != null)
                                    select new
                                    {
                                        numero_socio = a.socio,
                                        dni_socio = a.DNI,
                                        apeynom = a.ApellidoyNombre,
                                        empresa = a.Empresa,
                                        num_cupon = a.numero_cupon,
                                        fecha_entrega = a.fiesta_fin_año_fecha_entrega
                                    }).OrderBy(x => x.apeynom);
                    Dgv_cupones_entregados.DataSource = cupones.ToList();
                    Lbl_cupones_entregados.Text = "Cupones Entregados: " + Convert.ToString(cupones.Count());
                }
                if (Rbtn_apenom_cupon_entregado.Checked) // si esta chequeado buscar por apellido y nombre
                {
                    var cupones = (from a in db_socios.socios
                                    where (a.ApellidoyNombre.Contains(dato)) && (a.fiesta_fin_año != null)
                                   select new
                                    {
                                        numero_socio = a.socio,
                                        dni_socio = a.DNI,
                                        apeynom = a.ApellidoyNombre,
                                        empresa = a.Empresa,
                                        num_cupon = a.numero_cupon,
                                        fecha_entrega = a.fiesta_fin_año_fecha_entrega
                                    }).OrderBy(x => x.apeynom);
                    Dgv_cupones_entregados.DataSource = cupones.ToList();
                    Lbl_cupones_entregados.Text = "Cupones Entregados: " + Convert.ToString(cupones.Count());
                }
            }*/
        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_buscar_Click(object sender, EventArgs e)
        {
            // grande 1210; 554
            // chico 592; 554
            int ancho = 0;
            if (Btn_mostrar_cupones.ButtonText == "Mostrar Cupones Entregados >>>")
            {
                ancho = 1320;
                Btn_mostrar_cupones.ButtonText = "<<< Ocultar Cupones Entregados";
                for (int i = 586; i < ancho; i+=5)
                {
                    this.Width = i;
                    this.CenterToScreen();
                }
            }
            else
            {
                ancho = 586;
                Btn_mostrar_cupones.ButtonText = "Mostrar Cupones Entregados >>>";
                for (int i = 1320; i > ancho; i -= 5)
                {
                    this.Width = i;
                    this.CenterToScreen();
                }
            }
        }

        private void Txt_dni_OnValueChanged(object sender, EventArgs e)
        {
            Buscar_socio(Txt_dni.Text);
            
        }

        private void Rbtn_dni_CheckedChanged(object sender, EventArgs e)
        {
            if (Rbtn_dni.Checked == true)
            {
                Txt_dni.Enabled = true;
                Txt_dni.Focus();
                Txt_apellido.Enabled = false;
                Txt_empresa.Enabled = false;
            }
        }

        private void Rbtn_apenom_CheckedChanged(object sender, EventArgs e)
        {
            if (Rbtn_apenom.Checked)
            {
                Txt_apellido.Enabled = true;
                Txt_apellido.Focus();
                Txt_dni.Enabled = false;
                Txt_empresa.Enabled = false;
            }
        }

        private void Rbtn_empresa_CheckedChanged(object sender, EventArgs e)
        {
            if (Rbtn_empresa.Checked)
            {
                Txt_empresa.Enabled = true;
                Txt_empresa.Focus();
                Txt_apellido.Enabled = false;
                Txt_dni.Enabled = false;
            }
        }

        private void Txt_apellido_OnValueChanged(object sender, EventArgs e)
        {
            Buscar_socio(Txt_apellido.Text);
        }

        private void Buscar_socio(string dato)
        {
            /*if (dato == "") // si el parametro "dato" a buscar no contiene nada entonces se muestran todos los socios
            {
                var socio = (from a in db_socios.socios
                            select new
                            {
                                numero_socio = a.socio,
                                dni_socio = a.DNI,
                                apeynom = a.ApellidoyNombre,
                                empresa = a.Empresa
                            }).OrderBy(x =>x.apeynom);
                Dgv_socios.DataSource = socio.ToList();
                lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
            }
            else
            {
                if (Rbtn_dni.Checked ) // si esta chequeado buscar por DNI
                {
                    var socio = (from a in db_socios.socios
                                where a.DNI.Contains(dato)
                                select new
                                {
                                    numero_socio = a.socio,
                                    dni_socio = a.DNI,
                                    apeynom = a.ApellidoyNombre,
                                    empresa = a.Empresa
                                }).OrderBy(x => x.apeynom);
                     Dgv_socios.DataSource = socio.ToList();
                    lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
                }
                if (Rbtn_apenom.Checked) // si esta chequeado buscar por apellido y nombre
                {
                    var socio = (from a in db_socios.socios
                                where a.ApellidoyNombre.Contains(dato)
                                select new
                                {
                                    numero_socio = a.socio,
                                    dni_socio = a.DNI,
                                    apeynom = a.ApellidoyNombre,
                                    empresa = a.Empresa
                                }).OrderBy(x => x.apeynom);
                    Dgv_socios.DataSource = socio.ToList();
                    lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
                }
                if (Rbtn_empresa.Checked) // si esta chequeado buscar por empresa
                {
                    var socio = (from a in db_socios.socios
                                where a.Empresa.Contains(dato)
                                select new
                                {
                                    numero_socio = a.socio,
                                    dni_socio = a.DNI,
                                    apeynom = a.ApellidoyNombre,
                                    empresa = a.Empresa
                                }).OrderBy(x => x.apeynom);
                    Dgv_socios.DataSource = socio.ToList();
                    lbl_total_socios.Text = "Cantidad de Socios: " + Convert.ToString(socio.Count());
                }
            }*/
        }

        private void Txt_empresa_OnValueChanged(object sender, EventArgs e)
        {
            Buscar_socio(Txt_empresa.Text);
        }

        private void Btn_entregar_Click(object sender, EventArgs e)
        {
            /*if (Dgv_socios.RowCount != 0)
            {
                string dni = Convert.ToString(Dgv_socios.CurrentRow.Cells[1].Value);
                try
                {
                    var nro_cupon = db_socios.socios.Max(x => x.numero_cupon);

                    socios soc = db_socios.socios.Where(x => x.DNI.Equals(dni)).Single();

                    if (MessageBox.Show("Esta seguro de entregar el cupon al Socio " + soc.ApellidoyNombre + " ????", "ATENCION", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        soc.fiesta_fin_año = 1;
                        soc.fiesta_fin_año_fecha_entrega = DateTime.Now;
                        soc.numero_cupon = nro_cupon + 1;
                        db_socios.SubmitChanges();
                        Actualizar_cupones_entregados();
                        Verificar_entrega();
                        var nro_cupon_ = db_socios.socios.Max(x => x.numero_cupon);
                        //muestro el cupon
                        reportes frm_reportes = new reportes();
                        frm_reportes.Nro_cupon = Convert.ToInt32( nro_cupon_);
                        frm_reportes.ShowDialog();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                    throw;
                }
            }
            */
        }

        private void Dgv_socios_SelectionChanged(object sender, EventArgs e)
        {
            Verificar_entrega();
        }

        private void Verificar_entrega()
        {
            /*string dni = Convert.ToString(Dgv_socios.CurrentRow.Cells[1].Value);
            var soc = db_socios.socios.Where(x => x.DNI.Equals(dni)).Single();
            if (soc.fiesta_fin_año == null)
            {
                Lbl_cupon_entregado.Text = "";
                Btn_entregar.Visible = true;
            }
            else
            {
                Lbl_cupon_entregado.Text = "Cupon Entregado el " + soc.fiesta_fin_año_fecha_entrega;
                Btn_entregar.Visible = false;
            }
            */
        }

        private void Txt_dni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Dgv_socios.Focus();
            }
        }

        private void Txt_apellido_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Dgv_socios.Focus();
            }
        }

        private void Txt_empresa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Dgv_socios.Focus();
            }
        }

        private void Txt_dni_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Txt_dni_2_OnValueChanged(object sender, EventArgs e)
        {
            Buscar_cupones_entregados(Txt_dni_2.Text);
        }

        private void Ttxt_apenom_OnValueChanged(object sender, EventArgs e)
        {
            Buscar_cupones_entregados(Txt_apenom_cupon_entregado.Text);
        }

        private void Rbtn_dni_cupon_entregado_CheckedChanged(object sender, EventArgs e)
        {
            if (Rbtn_dni_cupon_entregado.Checked)
            {
                Txt_dni_2.Enabled = true;
                Txt_dni_2.Focus();
                Txt_apenom_cupon_entregado.Enabled = false;
            }
        }

        private void Rbtn_apenom_cupon_entregado_CheckedChanged(object sender, EventArgs e)
        {
            if (Rbtn_apenom_cupon_entregado.Checked)
            {
                Txt_apenom_cupon_entregado.Enabled = true;
                Txt_apenom_cupon_entregado.Focus();
                Txt_dni_2.Enabled = false;
            }
        }

    }
}
