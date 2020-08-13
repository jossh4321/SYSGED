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
        [HttpGet("get/pasosdto2id")]
        public async Task<ActionResult<List<PasosDTO2>>> GetPasosDTOById()
        {
            List<PasosDTO2> pasos;
            pasos = await pasoService.GetPasosDTO2();
            return pasos;
        }
        [HttpPost()]
        public async Task<ActionResult<PasosDTO2>> registrarPasos(PasosDTO2 pasosdto2)
        {
            pasosdto2 = await pasoService.registrarPaso(pasosdto2);
            return pasosdto2;
        }

        [HttpPut()]
        public async Task<ActionResult<PasosDTO2>> actualizarPaso(PasosDTO2 pasodto2)
        {
            pasodto2 = await pasoService.modificarpaso(pasodto2);
            return pasodto2;
        }
    }
}
