using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class OutputCobroEn
    {
        public int MONTO { get; set; }
        public int MONEDA { get; set; }
        public string CODIGO_TRANSACCION_ALIADO { get; set; }
        public int FECHA { get; set; }
        public int HORA { get; set; }
        public int TIPO_CAMBIO { get; set; }
        public int FECHA_TIPO_CAMBIO { get; set; }
        public string CODIGO_RETORNO { get; set; }
        public string DESCRIPCION_COD_RETORNO { get; set; }
    }
}
