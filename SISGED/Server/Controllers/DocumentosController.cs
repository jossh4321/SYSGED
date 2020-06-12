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
        [HttpPost("documentoodn")]
        public ActionResult<OficioDesignacionNotario> RegistrarDocumentoODN(OficioDesignacionNotarioDTO documento)
        {
            ContenidoOficioDesignacionNotario contenidoODN = new ContenidoOficioDesignacionNotario()
            {
                titulo = documento.contenidoDTO.titulo,
                descripcion = documento.contenidoDTO.descripcion,
                fecharealizacion = new DateTime(),
                lugaroficionotarial = documento.contenidoDTO.lugaroficionotarial,
                idusuario = documento.contenidoDTO.idusuario,//nota
                idnotario = documento.contenidoDTO.idnotario.id,
            };
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario()
            {
                tipo = "OficioDesignacionNotario",
                contenido = contenidoODN,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            documentoODN = _documentoservice.registrarOficioDesignacionNotario(documentoODN);
            return documentoODN;
        }
        [HttpPost("documentosbpn")]
        public ActionResult<OficioBPN> RegistrarDocumentoSolicitudBPN(OficioBPNDTO documento)
        {
            ContenidoOficioBPN contenidoSolicitudBPN = new ContenidoOficioBPN()
            {
                titulo = documento.contenidoDTO.titulo,
                descripcion = documento.contenidoDTO.descripcion,
                observacion = documento.contenidoDTO.observacion,
                idcliente = documento.contenidoDTO.idcliente.id,
                direccionoficio = documento.contenidoDTO.direccionoficio,
                idnotario = documento.contenidoDTO.idnotario.id,
                actojuridico = documento.contenidoDTO.actojuridico,
                tipoprotocolo = documento.contenidoDTO.tipoprotocolo,
                otorgantes = documento.contenidoDTO.otorgantes,
                fecharealizacion = DateTime.Now,
                url="ninguna"
            };
            OficioBPN documentoODN = new OficioBPN()
            {
                tipo = "OficioBPN",
                contenido = contenidoSolicitudBPN,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            documentoODN = _documentoservice.registrarSolicitudBPN(documentoODN);
            return documentoODN;
        }
    }
}
