using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Helpers;
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
        private readonly IFileStorage _almacenadorDeDocs;
        public DocumentosController(DocumentoService documentoservice, IFileStorage almacenadorDeDocs)
        {
            _documentoservice = documentoservice;
            _almacenadorDeDocs = almacenadorDeDocs;
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
            Estado estado = new Estado()//cambiado por mi
            {
                status = "pendiente",
                observacion = "Ninguna",
            };
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario()
            {
                tipo = "OficioDesignacionNotario",
                contenido = contenidoODN,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = estado,
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
        [HttpPost("documentosd")]
        public async Task<ActionResult<SolicitudDenuncia>> RegistrarDocumentoSolicitudDenuncia(SolicitudDenunciaDTO documento)
        {
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(documento.contenidoDTO.urldata))
            {
                var solicitudBytes = Convert.FromBase64String(documento.contenidoDTO.urldata);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicituddenuncia");
            }
            ContenidoSolicitudDenuncia contenidoSolicitudDenuncia = new ContenidoSolicitudDenuncia()
            {
                codigo = documento.contenidoDTO.codigo,
                titulo = documento.contenidoDTO.titulo,
                descripcion = documento.contenidoDTO.descripcion,
                nombrecliente = documento.contenidoDTO.nombrecliente,
                fechaentrega = DateTime.Now,
                url = urlData
            };
            SolicitudDenuncia solicitudDenuncia = new SolicitudDenuncia()
            {
                tipo = "SolicitudDenuncia",
                contenido = contenidoSolicitudDenuncia,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            solicitudDenuncia = _documentoservice.registrarSolicitudDenuncia(solicitudDenuncia);
            return solicitudDenuncia;

        }
        [HttpPost("documentosef")]
        public async Task<ActionResult<SolicitudExpedicionFirma>> RegistrarDocumentoSEF(SolicitudExpedicionFirmaDTO documento)
        {
            string urlData = "www.google.com";
            if (!string.IsNullOrWhiteSpace(documento.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(documento.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveFile(solicitudBytes, "jpg", "solicitudexpedicionfirma");
            }
            ContenidoSolicitudExpedicionFirma contenidoSEF = new ContenidoSolicitudExpedicionFirma()
            {
                titulo = documento.contenidoDTO.titulo,
                descripcion = documento.contenidoDTO.descripcion,
                fecharealizacion = DateTime.Now,
                cliente = documento.contenidoDTO.cliente,
                codigo = documento.contenidoDTO.codigo,
                url = urlData
            };
            SolicitudExpedicionFirma documentoSEF = new SolicitudExpedicionFirma()
            {
                tipo = "SolicitudExpedicionFirma",
                contenido = contenidoSEF,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            return _documentoservice.registrarSolicitudExpedicionFirma(documentoSEF);
        }
        /*[HttpPut("estado")]
        public ActionResult<Documento> modificarEstado(Documento documento)
        {
            documento = _documentoservice.modificarEstado(documento);
            return documento;
        }*/
    }
}
