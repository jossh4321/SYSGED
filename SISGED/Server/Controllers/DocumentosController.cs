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
    public class DocumentosController: ControllerBase
    {
        private readonly DocumentoService _documentoservice;
        public DocumentosController(DocumentoService documentoservice)
        {
            _documentoservice = documentoservice;
        }
        [HttpGet]
        public ActionResult<List<Documento>> Get()
        {
             return _documentoservice.obtenerDocumentos();
        }
    }
}
