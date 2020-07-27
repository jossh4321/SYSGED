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
    public class AsistenteController: ControllerBase
    {
        private readonly AsistenteService asistenteService;

        public AsistenteController(AsistenteService asistenteService)
        {
            this.asistenteService = asistenteService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Asistente>> Create()
        {
            Asistente asistente = new Asistente();
            asistente.idexpediente = "gdfh45345436";
            asistente.pasos = new PasoAsistente();
            asistente.pasos.nombreexpediente = "Solicitud";

            return await asistenteService.Create(asistente);

        }

        [HttpPut("updateinicial")]
        public async Task<ActionResult<Asistente>> UpdateSolicitudInicial()
        {
            Asistente asistente = new Asistente();
            asistente.idexpediente = "gdfh45345436";
            asistente.pasos = new PasoAsistente();
            asistente.pasos.nombreexpediente = "Solicitud";

            List<Paso> lstpasos = new List<Paso>();
            lstpasos.Add(new Paso { fechainicio = DateTime.Now.ToString(), 
                fechafin = DateTime.Now.AddDays(1).ToString(),
                fechalimite = DateTime.Now.AddDays(2).ToString() });

            asistente.pasos.documentos = new List<DocumentoPaso>();
            asistente.pasos.documentos.Add(new DocumentoPaso { tipo = "Ejemplo", pasos= lstpasos});
            asistente.paso = 0;
            asistente.subpaso = 2;
            asistente.tipodocumento = "SolicitudDenuncia";

            return await asistenteService.UpdateSolicitudInicial(asistente, "Denuncia");
                
        }

        [HttpPut("update")]
        public async Task<ActionResult<Asistente>> Update()
        {
            PasoDTO paso = new PasoDTO();

            paso.paso = 1;
            paso.subpaso = 1;
            paso.tipodocumento = "SolicitudDenuncia";
            paso.idexpediente = "gdfh45345436";
            paso.fechainicio = DateTime.Now.ToString();
            paso.fechafin = DateTime.Now.AddDays(1).ToString();
            paso.fechalimite = DateTime.Now.AddDays(2).ToString();

            return await asistenteService.Update(paso);

        }
    }

}
