using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
    public class EscriturasPublicasController : ControllerBase
    {
        private readonly EscriturasPublicasService escriturasPublicasService;

        public EscriturasPublicasController(EscriturasPublicasService escriturasPublicasService)
        {
            this.escriturasPublicasService = escriturasPublicasService;
        }

        [HttpGet]
        public async Task<List<EscrituraPublicaRDTO>> Get()
        {
            List<EscrituraPublicaRDTO> listaEscriturasPublicas =await escriturasPublicasService.obtenerEscriturasPublicas();
            return listaEscriturasPublicas;
        }
    }
}
