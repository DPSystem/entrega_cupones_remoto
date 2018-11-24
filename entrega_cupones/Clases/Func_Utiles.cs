using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entrega_cupones.Clases
{
    public class Func_Utiles
    {
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
    }
}
