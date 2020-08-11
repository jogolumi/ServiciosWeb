using CapaEntidad;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_SeguroAsistencias_LBC.Services
{
    public class ServicioCobro
    {
        public OutputCobroEn ServiciosCobro(string CODIGO_CLIENTE, string CODIGO_USUARIO, string ID_POLIZA, int NUMERO_CUOTA,
                                         int IMPORTE, int MONEDA, string NUMERO_CUENTA, int TIPO_CUENTA, int SUMA_ASEGURADORA)
        {
            try
            {
                

                return null;
            }
            catch (Exception ex)
            {
                //agregar el .log
                throw new Exception();
            }
        }
    }
}