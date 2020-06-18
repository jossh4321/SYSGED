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
    public class BandejasController: ControllerBase
    {
        private readonly BandejaService _bandejaService;

        public BandejasController(BandejaService bandejaService)
        {
            _bandejaService = bandejaService;
        }

        [HttpGet("getbandeja/{usuario}")]
        public async Task<ActionResult<BandejaESDTOR>> Get(string usuario)
        {
            return await _bandejaService.ObtenerBandeja(usuario);
        }

        [HttpGet("bandejaentrada/{usuario}")]
        public async Task<ActionResult<List<BandejaEntradaDTOR>>> GetBE(string usuario)
        {
            return await _bandejaService.ObtenerBandejaEntrada(usuario);
        }
    }
}
