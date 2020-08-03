using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auditoria.CapaEntidades
{
    public class enAuditoriaDetalle
    {
        public Int64 CodigoAuditoria { get; set; }
        public int Correlativo { get; set; }
        public string Nombre { get; set; }
        public string DetalleAntes { get; set; }
        public string DetalleDespues { get; set; }
    }
}