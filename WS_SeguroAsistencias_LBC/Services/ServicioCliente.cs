using Auditoria.CapaEntidades;
using Auditoria.CapaNegocio;
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
        
        public OutputClienteEn ServicioTraerCliente(string tipoDoc, string numeroDoc)
        {
            try
            {
                ClienteNeg neCliente = new ClienteNeg();
                InputClienteEn enCliente = new InputClienteEn();
                OutputClienteEn enClienteRetorno = new OutputClienteEn();

                enCliente.TipoDocumento = tipoDoc;
                enCliente.Documento = numeroDoc;
                enCliente.CodigoUsuario = "aticona";
                enClienteRetorno = neCliente.DatosCliente(enCliente);

                return enClienteRetorno;
            }
            catch (Exception ex)
            {
                //agregar el .log
                throw new Exception();
            }

        }
        
    }
}