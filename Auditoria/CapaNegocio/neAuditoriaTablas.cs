using Auditoria.CapaDatos;
using Auditoria.CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auditoria.CapaNegocio
{
    public class neAuditoriaTablas : enAuditoriaTablas
    {
        public bool FN_NeRegistrarAuditoriaTablas(enAuditoriaTablas oEnAuditoriaTablas)
        {
            daAuditoriaTablas oDaAuditoriaTablas = new daAuditoriaTablas();
            return oDaAuditoriaTablas.FN_DaRegistrarAuditoriaTablas(oEnAuditoriaTablas);
        }
    }
}