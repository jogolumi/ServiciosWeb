using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class InputCobroEn
    {
        public string CODIGO_CLIENTE { get; set; }
        public string CODIGO_USUARIO { get; set; }
        public string ID_POLIZA { get; set; }
        public int NUMERO_CUOTA { get; set; }
        public int IMPORTE { get; set; }
        public int MONEDA { get; set; }
        public string NUMERO_CUENTA { get; set; }
        public int TIPO_CUENTA { get; set; }
        public int SUMA_ASEGURADORA { get; set; }
    }
}
