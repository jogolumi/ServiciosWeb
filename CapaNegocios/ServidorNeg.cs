using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class ServidorNeg
    {
        public DateTime DevolverFechaHoraServidor()
        {
            DateTime dFechaServidor=DateTime.Now;
            ServidorDa DAServidor = new ServidorDa();
            using(IDataReader dr = DAServidor.DevolverDatosServidor())
            {
                dr.Read();
                dFechaServidor = Convert.ToDateTime(dr["FechaHoraServidor"]);
            }
            return dFechaServidor;
        }
    }
}
