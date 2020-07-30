using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Services;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsistenteController: ControllerBase
    {
        private readonly AsistenteService asistenteService;

        public AsistenteController(AsistenteService asistenteService)
        {
            this.asistenteService = asistenteService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Asistente>> Create()
        {
            Asistente asistente = new Asistente();
            asistente.idexpediente = "gdfh45345436";
            asistente.pasos = new PasoAsistente();
            asistente.pasos.nombreexpediente = "Solicitud";

            return await asistenteService.Create(asistente);

        }

        [HttpPut("updateinicial")]
        public async Task<ActionResult<Asistente>> UpdateSolicitudInicial(Asistente asistente,[FromQuery] String nombreExpediente)
        {
            return await asistenteService.UpdateSolicitudInicial(asistente, nombreExpediente);
                
        }

        [HttpPut("update")]
        public async Task<ActionResult<Asistente>> Update(PasoDTO paso)
        {
            return await asistenteService.Update(paso);
        }

        [HttpGet("getbyexpedientID")]
        public async Task<ActionResult<Asistente>> GetByExpedientID([FromQuery] String expedientID)
        {
            Asistente asistente = new Asistente();
            asistente = await asistenteService.GetAsistente(expedientID);

            return asistente;
        }
        [HttpPut("updateNormal")]
        public async Task<ActionResult<Asistente>> UpdateNormal(PasoDTO paso)
        {
            return await asistenteService.UpdateNormal(paso);
        }
        [HttpPut("updateFinally")]
        public async Task<ActionResult<Asistente>> UpdateFinally(PasoDTO paso)
        {
            return await asistenteService.UpdateFinally(paso);
        }
    }

}
