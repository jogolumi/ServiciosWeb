using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
using System.Data;
using Auditoria.CapaEntidades;

namespace Auditoria.CapaDatos
{
    public class daAuditoriaTablas
    {
        private DatabaseProviderFactory construir = new DatabaseProviderFactory();
        private Database db = null;
        public daAuditoriaTablas()
        {
            db = construir.Create("ecoamparaEntities");
        }
        public bool FN_DaRegistrarAuditoriaTablas(enAuditoriaTablas oEnAuditoriaTablas)
        {
            DbCommand comand = db.GetStoredProcCommand("RegistrarAuditoriaTablas");
            try
            {
                db.AddInParameter(comand, "@iCodigoAuditoria", DbType.Int64, oEnAuditoriaTablas.CodigoAuditoria);
                db.AddInParameter(comand, "@iCorrelativo", DbType.Int16, oEnAuditoriaTablas.Correlativo);

                db.AddInParameter(comand, "@sCodigoTabla", DbType.String, oEnAuditoriaTablas.CodigoTabla);
                db.AddInParameter(comand, "@sCodigoPosicion", DbType.String, oEnAuditoriaTablas.CodigoPosicion);
                db.AddInParameter(comand, "@sNombre", DbType.String, oEnAuditoriaTablas.Nombre);
                db.AddInParameter(comand, "@sDetalle", DbType.String, oEnAuditoriaTablas.Detalle);

                db.ExecuteNonQuery(comand);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                comand.Dispose();
            }
        }
    }
}