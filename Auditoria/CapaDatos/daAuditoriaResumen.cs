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
    public class daAuditoriaResumen
    {
        private DatabaseProviderFactory construir = new DatabaseProviderFactory();
        private Database db = null;
        public void New()
        {
            db = construir.Create("cadenaConexion");
        }
        public Int64 FN_DaRegistrarAuditoriaResumen(enAuditoriaResumen oEnAuditoriaResumen)
        {
            DbCommand comand = db.GetSqlStringCommand("RegistrarAuditoriaResumen");
            try
            {
                db.AddOutParameter(comand, "@iCodigoProceso", DbType.Int64, 0);

                db.AddInParameter(comand, "@sCodigoUsuario", DbType.String, oEnAuditoriaResumen.CodigoUsuario);
                db.AddInParameter(comand, "@sCodigoOpcion", DbType.String, oEnAuditoriaResumen.CodigoOpcion);
                db.AddInParameter(comand, "@tFechaProceso", DbType.DateTime, oEnAuditoriaResumen.FechaProceso);
                db.AddInParameter(comand, "@tHoraProceso", DbType.DateTime, oEnAuditoriaResumen.HoraProceso);
                db.AddInParameter(comand, "@sCodigoMetodo", DbType.String, oEnAuditoriaResumen.CodigoMetodo);

                db.ExecuteNonQuery(comand);

                Int64 vpiCodigoProceso;
                vpiCodigoProceso = Convert.ToInt64(db.GetParameterValue(comand, "@iCodigoProceso"));

                return vpiCodigoProceso;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                comand.Dispose();
            }
        }
    }
}