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
    public class BandejasController: ControllerBase
    {
        private readonly BandejaService _bandejaService;

        public BandejasController(BandejaService bandejaService)
        {
            _bandejaService = bandejaService;
        }

        [HttpGet("getBandeja")]
        public async Task<ActionResult<Bandeja>> Get(string usuario)
        {
            return await _bandejaService.ObtenerBandejaDocumento(usuario);
        }
    }
}
