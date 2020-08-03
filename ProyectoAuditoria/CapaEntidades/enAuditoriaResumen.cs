using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auditoria.CapaEntidades
{
    public class enAuditoriaResumen
    {
        public Int64 CodigoAuditoria { get; set; }
        public string CodigoUsuario { get; set; }
        public string CodigoOpcion { get; set; }
        public DateTime FechaProceso { get; set; }
        public DateTime HoraProceso { get; set; }
        public string CodigoMetodo { get; set; }
    }
}