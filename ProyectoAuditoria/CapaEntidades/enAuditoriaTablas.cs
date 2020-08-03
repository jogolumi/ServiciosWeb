using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auditoria.CapaEntidades
{
    public class enAuditoriaTablas
    {
        public Int64 CodigoAuditoria { get; set; }
        public int Correlativo { get; set; }
        public string CodigoTabla { get; set; }
        public string CodigoPosicion { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
    }
}