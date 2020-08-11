using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WS_SeguroAsistencias_LBC.Services;

namespace WS_SeguroAsistencias_LBC.Controllers
{
    public class CobroController : ApiController
    {
        [Route("api/cobro")]
        [HttpPost]
        // POST api/cobro
        public IHttpActionResult Cobros(string CODIGO_CLIENTE, string CODIGO_USUARIO, string ID_POLIZA, int NUMERO_CUOTA,
                                         int IMPORTE, int MONEDA, string NUMERO_CUENTA, int TIPO_CUENTA, int SUMA_ASEGURADORA)
        {
            if (CODIGO_CLIENTE == null)
            {
                return BadRequest();
            }
            else if (CODIGO_USUARIO == null)
            {
                return BadRequest();
            }
            else if (ID_POLIZA == null)
            {
                return BadRequest();
            }
            else if (NUMERO_CUOTA == 0)
            {
                return BadRequest();
            }
            else if (IMPORTE == 0)
            {
                return BadRequest();
            }
            else if (MONEDA == 0)
            {
                return BadRequest();
            }
            else if (NUMERO_CUENTA == null)
            {
                return BadRequest();
            }
            else if (TIPO_CUENTA == 0)
            {
                return BadRequest();
            }
            else if (SUMA_ASEGURADORA == 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OutputCobroEn oCobro = new OutputCobroEn();
                ServicioCobro servicioCobro= new ServicioCobro();

                oCobro = servicioCobro.ServiciosCobro(CODIGO_CLIENTE, CODIGO_USUARIO, ID_POLIZA, NUMERO_CUOTA,
                                         IMPORTE, MONEDA, NUMERO_CUENTA, TIPO_CUENTA, SUMA_ASEGURADORA);

                return Ok(oCobro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}