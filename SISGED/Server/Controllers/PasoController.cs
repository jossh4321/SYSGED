using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Services;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasoController: ControllerBase
    {
        private readonly PasoService pasoService;

        public PasoController(PasoService pasoService)
        {
            this.pasoService = pasoService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<List<Pasos>>> GetPasos()
        {
            List<Pasos> pasos;
            pasos = await pasoService.GetPasos();
            return pasos;
        }

        [HttpGet("get/id")]
        public async Task<ActionResult<Pasos>> GetPasosById([FromQuery] string id)
        {
            Pasos pasos = new Pasos(); ;
            pasos = await pasoService.GetPasosById(id);
            return pasos;
        }
    }
}
