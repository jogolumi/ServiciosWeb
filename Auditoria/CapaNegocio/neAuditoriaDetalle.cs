using Auditoria.CapaDatos;
using Auditoria.CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auditoria.CapaNegocio
{
    public class neAuditoriaDetalle : enAuditoriaDetalle
    {
        public bool FN_NeRegistrarProcesoDetalle(enAuditoriaDetalle oEnAuditoriaDetalle)
        {
            daAuditoriaDetalle oDaAuditoriaDetalle= new daAuditoriaDetalle();
            return oDaAuditoriaDetalle.FN_DaRegistrarAuditoriaDetalle(oEnAuditoriaDetalle);
        }
    }
}