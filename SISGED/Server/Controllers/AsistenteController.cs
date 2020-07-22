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
    public class AsistenteController: ControllerBase
    {
        private readonly AsistenteService asistenteService;

        public AsistenteController(AsistenteService asistenteService)
        {
            this.asistenteService = asistenteService;
        }

        [HttpGet("getasistentes")]
        public async Task<ActionResult<List<Asistente>>> GetAsistentes()
        {
            List<Asistente> asistentes = new List<Asistente>();
            asistentes =await asistenteService.GetAsistentes();
            return asistentes;
        }
    }
}
