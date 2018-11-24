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

namespace entrega_cupones
{
    
    public partial class frm_edades : Form
    {
        lts_sindicatoDataContext db_socios = new lts_sindicatoDataContext();
        public frm_edades()
        {
            InitializeComponent();
        }

        private void btn_imprimir_Click(object sender, EventArgs e)
        {
            var edades = (from a in db_socios.soccen
                          join sf in db_socios.socflia on a.SOCCEN_CUIL equals sf.SOCFLIA_CUIL
                          join flia in db_socios.maeflia on sf.SOCFLIA_CODFLIAR equals flia.MAEFLIA_CODFLIAR
                          where a.SOCCEN_ESTADO == 1 
                          select new
                          {
                              sexo = flia.MAEFLIA_SEXO,
                              edad = calcular_edad(flia.MAEFLIA_FECNAC)
                          }).ToList();
            //dgv_edades.DataSource = edades.Where(x => x.edad <= 19).ToList();

            dgv_edades.Rows.Add(5);
            dgv_edades.Rows[3].Cells["edad"].Value = "12 a 18";
            dgv_edades.Rows[3].Cells["F"].Value = edades.Where(x => x.edad >= 12 && x.edad <= 18 && x.sexo == 'F').Count();
            dgv_edades.Rows[3].Cells["M"].Value = edades.Where(x => x.edad >= 12 && x.edad <= 18 && x.sexo == 'M').Count();
            dgv_edades.Rows[3].Cells["cantidad"].Value = edades.Where(x => x.edad >= 12 && x.edad <= 18 && x.sexo != ' ' ).Count();

            dgv_edades.Rows[2].Cells["edad"].Value = "8 a 11";
            dgv_edades.Rows[2].Cells["F"].Value = edades.Where(x => x.edad >= 8 && x.edad <= 11 && x.sexo == 'F').Count();
            dgv_edades.Rows[2].Cells["M"].Value = edades.Where(x => x.edad >= 8 && x.edad <= 11 && x.sexo == 'M').Count();
            dgv_edades.Rows[2].Cells["cantidad"].Value = edades.Where(x => x.edad >= 8 && x.edad <= 11 && x.sexo != ' ').Count();

            dgv_edades.Rows[1].Cells["edad"].Value = "6 a 7";
            dgv_edades.Rows[1].Cells["F"].Value = edades.Where(x => x.edad >= 6 && x.edad <= 7 && x.sexo == 'F').Count();
            dgv_edades.Rows[1].Cells["M"].Value = edades.Where(x => x.edad >= 6 && x.edad <= 7 && x.sexo == 'M').Count();
            dgv_edades.Rows[1].Cells["cantidad"].Value = edades.Where(x => x.edad >= 6 && x.edad <= 7 && x.sexo != ' ').Count();

            dgv_edades.Rows[0].Cells["edad"].Value = "3 a 5";
            dgv_edades.Rows[0].Cells["F"].Value = edades.Where(x => x.edad >= 3 && x.edad <= 5 && x.sexo == 'F').Count();
            dgv_edades.Rows[0].Cells["M"].Value = edades.Where(x => x.edad >= 3 && x.edad <= 5 && x.sexo == 'M').Count();
            dgv_edades.Rows[0].Cells["cantidad"].Value = edades.Where(x => x.edad >= 3 && x.edad <= 5 && x.sexo != ' ').Count();

            dgv_edades.Rows[4].Cells["edad"].Value = "sin sexo";
            dgv_edades.Rows[4].Cells["cantidad"].Value = edades.Where(x => x.edad >= 3 && x.edad <= 18 && x.sexo == ' ').Count();

            lbl_total_edades.Text = edades.Where(x => x.edad >= 3 && x.edad <= 18).Count().ToString();
            dgv_linq();
        }

        private int calcular_edad(DateTime fecha_nac)
        {
            int edad = 0;
            DateTime fecha_actual = DateTime.Today;
            edad = fecha_actual.Year - fecha_nac.Year;
            if ((fecha_actual.Month < fecha_nac.Month) || (fecha_actual.Month == fecha_nac.Month && fecha_actual.Day < fecha_nac.Day))
            {
                edad--;
            }
            return edad;
        }

        private void dgv_linq()
        {
            DS_cupones.crystalDataTable DT = new DS_cupones.crystalDataTable();

            DT.AddcrystalRow("1","2","3","4");
            rpt_edades ed = new rpt_edades();
            //ed.SetDataSource(DT.ToList());
            reportes_CR rpt = new reportes_CR("SS");
            rpt.Show();
          
        }
    }
}
