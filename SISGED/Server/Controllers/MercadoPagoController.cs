using Microsoft.AspNetCore.Mvc;
using SISGED.Shared.Models;
using System.Threading.Tasks;
using System.Xml.Linq;
using ws_mercadopago;

namespace SISGED.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        [HttpPost("request")]
        public async Task<MercadoPagoResponse> MercadoPago(MercadoPagoRequest mercadoPago)
        {
            XDocument xml = new XDocument(
                new XElement("paymentmp",
                    new XElement("pago",
                        new XElement("aplicacion", mercadoPago.aplicacion),
                        new XElement("codigoreferencia", mercadoPago.codigoreferencia),
                        new XElement("descripcion", mercadoPago.descripcion),
                        new XElement("monto", mercadoPago.monto)
            )));
            MERCADOPAGOResponse response = await new ws_lolimsaservicesSoapPortClient().MERCADOPAGOAsync(xml.ToString());
            return new MercadoPagoResponse { url = response.Result };
        }
    }
}