using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ServidorDa
    {
        private DatabaseProviderFactory construir = new DatabaseProviderFactory();
        private Database db = null;
        public ServidorDa()
        {
            db = construir.Create("ecoamparaEntities");
        }
        public IDataReader DevolverDatosServidor()
        {
            DbCommand comand = db.GetStoredProcCommand("ConsultarFechaHoraServidor");
            IDataReader dr = db.ExecuteReader(comand);
            comand.Dispose();

            return dr;
        }
    }
}
