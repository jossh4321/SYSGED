using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using SISGED.Server.Helpers;
using SISGED.Server.Services;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using SISGED.Shared.Models;
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

        [HttpGet("filter")]
        public ActionResult<List<EscrituraPublica>> autocompletefilter([FromQuery] string term)
        {
            List<EscrituraPublica> listaescritura = new List<EscrituraPublica>();
            listaescritura = escriturasPublicasService.filter(term);
            return listaescritura;
        }

        [HttpGet("filterespecial")]
        public async Task<List<EscrituraPublicaRDTO>> autocompleteFilterCompleto([FromQuery] ParametrosBusquedaEscrituraPublica parametrosbusqueda)
        {
            List<EscrituraPublicaRDTO> listaescriturasPublicasFiltrado = await escriturasPublicasService.filtradoEspecial(parametrosbusqueda);
            await HttpContext.InsertPagedParameterOnResponse(listaescriturasPublicasFiltrado.AsQueryable(),parametrosbusqueda.cantidadregistros);
            List<EscrituraPublicaRDTO> listaescriturasPublicasFiltradoPaginado =
                listaescriturasPublicasFiltrado.AsQueryable().Paginate(parametrosbusqueda.Paginacion).ToList();
            return listaescriturasPublicasFiltradoPaginado;
        }
    }
}
