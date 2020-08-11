using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class OutputMedioPagoEn
    {
        public List<MediosPagoEn> ListaMediosPago { get; set; }
        public string CodigoRetorno { get; set; }
        public string DescripcionRetorno { get; set; }
    }
}
