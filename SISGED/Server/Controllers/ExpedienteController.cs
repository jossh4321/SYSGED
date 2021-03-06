﻿using Microsoft.AspNetCore.Mvc;
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
    public class ExpedienteController:ControllerBase
    {
        private readonly ExpedienteService _expedienteService;
        private readonly EscriturasPublicasService _escriturapublicaService;

        public ExpedienteController(ExpedienteService expedienteService, EscriturasPublicasService escriturapublicaService)
        {
            _expedienteService = expedienteService;
            _escriturapublicaService = escriturapublicaService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<ExpedienteDTO2>>> getAllExpedianteDTO()
        {
            return await _expedienteService.getAllExpedienteDTO();
        }

        [HttpGet]
        public  ActionResult<List<Expediente>> getExpedientes()
        {
            return _expedienteService.getAllExpediente();
        }
        
        [HttpGet("filter")]
        public async Task<List<ExpedienteDTO>> autocompleteFilterCompleto([FromQuery] ParametrosBusquedaExpediente parametrosbusqueda)
        {
            List<ExpedienteDTO> listaExpedientesFiltrado = await _expedienteService.filtrado(parametrosbusqueda);
            await HttpContext.InsertPagedParameterOnResponse(listaExpedientesFiltrado.AsQueryable(), parametrosbusqueda.cantidadregistros);
            List<ExpedienteDTO> listaExpedientesFiltradoPaginado =
                listaExpedientesFiltrado.AsQueryable().Paginate(parametrosbusqueda.Paginacion).ToList();
            return listaExpedientesFiltradoPaginado;
        }

        [HttpGet("getbynested")]
        public async Task<ActionResult<ExpedienteDTO>> getbynestediddoc([FromQuery] string iddoc)
        {
            return _expedienteService.getbynestediddoc(iddoc);
        }

        [HttpPost("derivacion")]
        public ActionResult<ExpedienteBandejaDTO> registrarDerivacion(Expediente expediente, [FromQuery] string userId)
        {

            return _expedienteService.registrarDerivacion(expediente, userId);
        }
        [HttpGet("prueba")]
        public ActionResult<ExpedienteBandejaDTO> obtenerExpediente ([FromQuery] string id)
        {
            return _expedienteService.obtenerExpedienteBandeja(id);
        }

        [HttpGet("id")]
        public ActionResult<Expediente> GetById([FromQuery] string id)
        {
            Expediente expe = new Expediente();
            expe = _expedienteService.GetById(id);
            return expe;
        }
    }
}
