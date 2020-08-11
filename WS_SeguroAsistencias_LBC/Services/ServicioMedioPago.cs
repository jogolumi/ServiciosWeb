using CapaEntidad;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_SeguroAsistencias_LBC.Services
{
    public class ServicioMedioPago
    {
        public OutputMedioPagoEn ServicioTraerMedioPago(string codigoCliente, string codigoUsuario)
        {
            try
            {
                MedioPagoNeg neMedioPago = new MedioPagoNeg();
                InputMedioPagoEn enMedioPago = new InputMedioPagoEn();
                OutputMedioPagoEn enMedioPagoRetorno = new OutputMedioPagoEn();

                enMedioPago.CodigoCliente = codigoCliente;
                enMedioPago.CodigoUsuario = codigoUsuario;
                enMedioPagoRetorno = neMedioPago.DatosMedioPago(enMedioPago);

                return enMedioPagoRetorno;
            }
            catch (Exception ex)
            {
                //agregar el .log
                throw new Exception();
            }
        }
    }
}