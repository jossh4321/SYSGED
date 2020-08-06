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
        private readonly ExpedienteService _expedienteservice;
        public StatisticsController(DocumentoService documentoservice, ExpedienteService expedienteservice)
        {
            _documentoservice = documentoservice;
            _expedienteservice = expedienteservice;
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
        [HttpGet("doccaducadosxmes")]
        public async Task<List<StatisticsDTO3_group>> estadisticaDocCaducadosXMes([FromQuery]int mes)
        {
            List<StatisticsDTO3_group> estadisticas = new List<StatisticsDTO3_group>();
            estadisticas = await _documentoservice.estadisticasDocumentosCaducadosXMes(mes);
            return estadisticas;
        }

        [HttpGet("ganttexpedientes")]
        public async Task<List<ExpedienteDTO_group1>> ganttexpedientes([FromQuery]string dni)
        {
            List<ExpedienteDTO_group1> estadisticas = new List<ExpedienteDTO_group1>();
            estadisticas = await _expedienteservice.estadisticaGantt(dni);
            return estadisticas;
        }
        ///////Nuevas Estadisticas/////////////
        //[documentos caducado, procesado y pendiente] => para los 12 tipos de documentos
        [HttpGet("estadistica1")]
        public async Task<List<estadistica1_group>> obtenecionEstadistica1([FromQuery]int mes)
        {
            List<estadistica1_group> estadisticas = new List<estadistica1_group>();
            estadisticas = await _documentoservice.obtenecionEstadistica1(mes);
            return estadisticas;
        }
        //[documentos caducado, procesado y pendiente] => para los 12 tipos de documentos por areas
        [HttpGet("estadistica2")]
        public async Task<List<estadistica1_group>> obtenecionEstadistica2([FromQuery]int mes, [FromQuery]string area)
        {
            List<estadistica1_group> estadisticas = new List<estadistica1_group>();
            estadisticas = await _documentoservice.obtenecionEstadistica2(mes,area);
            return estadisticas;
        }
        //[documentos caducado, procesado y pendiente] => para los 12 tipos de documentos por  usuario espeficio
        [HttpGet("estadistica3")]
        public async Task<List<estadistica1_group>> obtenecionEstadistica3([FromQuery]int mes, [FromQuery]string usuario)
        {
            List<estadistica1_group> estadisticas = new List<estadistica1_group>();
            estadisticas = await _documentoservice.obtenecionEstadistica3(mes, usuario);
            return estadisticas;
        }

        //API PARA EL DIAGRAMA DE GANTT 2
        [HttpGet("ganttexpedientes2")]
        public async Task<List<expediente_project>> ganttestadistica([FromQuery]string dni)
        {
            List<expediente_project> estadisticas = new List<expediente_project>();
            estadisticas = await _expedienteservice.agregacioGantt2(dni);
            return estadisticas;
        }

    }
}
