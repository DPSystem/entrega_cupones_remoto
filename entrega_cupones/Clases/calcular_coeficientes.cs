using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entrega_cupones.Clases
{
    public class calcular_coeficientes
    {
        public double calcular_coeficiente_A(DateTime periodo, DateTime fpago, double tot_periodo, double pagado, DateTime fecha_venc_acta)

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
                coef_B = ((fecha_venc_acta - fecha_vencimiento).TotalDays * 0.001) + 1;
                tot_intereses_a_la_fecha = ((tot_periodo - pagado) * coef_B) + ((pagado * coef_A) * (coef_B - coef_A));
            }
            return tot_intereses_a_la_fecha; //tot_interes_mora_de_pago;

        }

        public double calcular_coeficiente_B(DateTime periodo, double tot_periodo, double pagado, DateTime fecha_venc_acta)
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
            coef_B = ((fecha_venc_acta - fecha_vencimiento).TotalDays * 0.001) + 1;
            tot_intereses_a_la_fecha = ((tot_periodo - pagado) * coef_B) + ((pagado * coef_A) * (coef_B - coef_A));

            return tot_intereses_a_la_fecha; //tot_interes_mora_de_pago;
        }
    }
}

