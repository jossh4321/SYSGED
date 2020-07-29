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
    public class StatisticsController : ControllerBase
    {
        private readonly DocumentoService _documentoservice;
        public StatisticsController(DocumentoService documentoservice)
        {
            _documentoservice = documentoservice;
        }
        //diagrama de documentos por area en un mes especifico
        [HttpGet("docxmesxarea")]
        public async Task<List<StatisticsDTOR>> estadisticaDocXMesXArea([FromQuery]int mes, [FromQuery]string area)
        {
            List<StatisticsDTOR> estadisticas = new List<StatisticsDTOR>();
            estadisticas = await _documentoservice.estadisticasDocXMesXArea(mes, area);
            return estadisticas;
        }
        //diagrama de documentos procesados por mes
        [HttpGet("docxmes")]
        public async Task<List<StatisticsDTOR>> estadisticaDocXMes([FromQuery]int mes)
        {
            List<StatisticsDTOR> estadisticas = new List<StatisticsDTOR>();
            estadisticas = await _documentoservice.estadisticasDocXMes(mes);
            return estadisticas;
        }
        [HttpGet("docevaluadosxmes")]
        public async Task<List<StatisticsDTO4R>> estadisticaDocAprobadosXMes([FromQuery]int mes)
        {
            List<StatisticsDTO4R> estadisticas = new List<StatisticsDTO4R>();
            estadisticas = await _documentoservice.statisticsDTO4EvaluadosJuntaDirectiva(mes);
            return estadisticas;
        }
        [HttpGet("doccaducadosxmes/id")]
        public async Task<List<StatisticsDTO3_group>> estadisticaDocCaducadosXMes([FromQuery]int mes)
        {
            List<StatisticsDTO3_group> estadisticas = new List<StatisticsDTO3_group>();
            estadisticas = await _documentoservice.estadisticasDocumentosCaducadosXMes(mes);
            return estadisticas;
        }

    }
}
