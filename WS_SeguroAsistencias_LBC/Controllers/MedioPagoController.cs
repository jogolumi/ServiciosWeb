using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WS_SeguroAsistencias_LBC.Services;

namespace WS_SeguroAsistencias_LBC.Controllers
{
    public class MedioPagoController : ApiController
    {
        [Route("api/medioPago")]
        [HttpPost]
        // POST api/cliente
        public IHttpActionResult TraerMedioDePagos(string codigoCliente, string codigoUsuario)
        {
            if (codigoCliente == null)
            {
                return BadRequest();
            }
            else if (codigoUsuario == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OutputMedioPagoEn oMedioPago = new OutputMedioPagoEn();
                ServicioMedioPago servidioMedioPago = new ServicioMedioPago();

                oMedioPago = servidioMedioPago.ServicioTraerMedioPago(codigoCliente, codigoUsuario);

                return Ok(oMedioPago);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
