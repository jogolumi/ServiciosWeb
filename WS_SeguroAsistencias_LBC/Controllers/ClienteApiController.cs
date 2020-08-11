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
    public class ClienteApiController : ApiController
    {
        [Route("api/cliente")]
        [HttpPost]
        // POST api/cliente
        public IHttpActionResult TraerDatosClientes(int tipoDoc, string numeroDoc,string codigoUsuario)
        {
            if (tipoDoc == null)
            {
                return BadRequest();
            }
            else if (numeroDoc == null)
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
                OutputClienteEn oCliente = new OutputClienteEn();
                ServicioCliente servicioCliente = new ServicioCliente();

                oCliente = servicioCliente.ServicioTraerCliente(tipoDoc, numeroDoc, codigoUsuario);

                return Ok(oCliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
