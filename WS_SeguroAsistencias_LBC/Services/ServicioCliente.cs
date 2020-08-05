using CapaEntidad;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_SeguroAsistencias_LBC.Services
{
    public class ServicioCliente
    {
        public OutputClienteEn ServicioTraerCliente(string tipoDoc,string numeroDoc)
        {
            ClienteNeg neCliente = new ClienteNeg();
            InputClienteEn enCliente = new InputClienteEn();
            OutputClienteEn enClienteRetorno = new OutputClienteEn();
            //string error = string.Empty;
            enCliente.TipoDocumento = tipoDoc;
            enCliente.Documento = numeroDoc;
            enClienteRetorno = neCliente.DatosCliente(enCliente);
            
            return enClienteRetorno;
        }
    }
}