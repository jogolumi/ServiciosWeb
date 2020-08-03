using Auditoria.CapaDatos;
using Auditoria.CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auditoria.CapaNegocio
{
    public class neAuditoriaResumen : enAuditoriaResumen
    {
        public Int64 FN_NeRegistrarProcesoResumen(enAuditoriaResumen oEnAuditoriaResumen)
        {
            Int64 iCodigoAuditoria;
            daAuditoriaResumen oDaAuditoriaResumen = new daAuditoriaResumen();
            iCodigoAuditoria = oDaAuditoriaResumen.FN_DaRegistrarAuditoriaResumen(oEnAuditoriaResumen);

            return iCodigoAuditoria;
        }
    }
}