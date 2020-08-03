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
    public class daAuditoriaDetalle
    {
        private DatabaseProviderFactory construir = new DatabaseProviderFactory();
        private Database db = null;
        public void New()
        {
            db = construir.Create("cadenaConexion");
        }
        public bool FN_DaRegistrarAuditoriaDetalle(enAuditoriaDetalle oEnAuditoriaDetalle)
        {
            DbCommand comand = db.GetSqlStringCommand("RegistrarAuditoriaDetalle");
            try
            {
                db.AddInParameter(comand, "@iCodigoAuditoria", DbType.Int64, oEnAuditoriaDetalle.CodigoAuditoria);
                db.AddInParameter(comand, "@iCorrelativo", DbType.Int32, oEnAuditoriaDetalle.Correlativo);

                db.AddInParameter(comand, "@sNombre", DbType.String, oEnAuditoriaDetalle.Nombre);
                db.AddInParameter(comand, "@sDetalleAntes", DbType.String, oEnAuditoriaDetalle.DetalleAntes);
                db.AddInParameter(comand, "@sDetalleDespues", DbType.String, oEnAuditoriaDetalle.DetalleDespues);

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