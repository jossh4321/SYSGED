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
    public class ExpedienteController:ControllerBase
    {
        private readonly ExpedienteService _expedienteService;

        public ExpedienteController(ExpedienteService expedienteService)
        {
            _expedienteService = expedienteService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<ExpedienteDTO_ur2>>> getAllExpedianteDTO()
        {
            return await _expedienteService.getAllExpedienteDTO();
        }

        [HttpGet]
        public  ActionResult<List<Expediente>> getExpedientes()
        {
            return _expedienteService.getAllExpediente();
        }
    }
}
