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
        private readonly ExpedienteService _expedienteservice;
        private readonly EscriturasPublicasService _escrituraspublicasservice;
        private readonly IFileStorage _almacenadorDeDocs;
        public DocumentosController(DocumentoService documentoservice, IFileStorage almacenadorDeDocs, ExpedienteService expedienteservice, EscriturasPublicasService escrituraspublicasservice)
        {
            _documentoservice = documentoservice;
            _almacenadorDeDocs = almacenadorDeDocs;
            _expedienteservice = expedienteservice;
            _escrituraspublicasservice = escrituraspublicasservice;
        }
        [HttpGet]
        public ActionResult<List<Documento>> Get()
        {
             return _documentoservice.obtenerDocumentos();
        }
        //Completo
        [HttpPost("documentoodn")]
        public ActionResult<OficioDesignacionNotario> RegistrarDocumentoODN(ExpedienteWrapper expediente)
        {
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario();
             documentoODN = _documentoservice.registrarOficioDesignacionNotario(expediente);
            return documentoODN;
        }

        //Completo
        [HttpPost("documentosbpn")]
        public ActionResult<OficioBPN> RegistrarDocumentoOficioBPN(ExpedienteWrapper expediente)
        {
            OficioBPN documentoOficioBPN = new OficioBPN();
            documentoOficioBPN = _documentoservice.registrarOficioBPNE(expediente);
            return documentoOficioBPN;
        }

        //Con el expediente agregado falta wrapper
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

            Cliente cliente = new Cliente()
            {
                nombre = documento.nombrecliente,
                tipodocumento = documento.tipodocumento,
                numerodocumento = documento.numerodocumento
            };
            Expediente expediente = new Expediente();
            expediente.tipo = "Denuncia";
            expediente.cliente = cliente;
            expediente.fechainicio = DateTime.Now;
            expediente.fechafin = null;
            expediente.documentos = new List<DocumentoExpediente>()
            {
                new DocumentoExpediente(){
                    indice = 1,
                    iddocumento = solicitudDenuncia.id,
                    tipo="SolicitudDenuncia",
                    fechacreacion = solicitudDenuncia.contenido.fechaentrega,
                    fechaexceso=solicitudDenuncia.contenido.fechaentrega.AddDays(10),
                    fechademora = null
                }
            };
            expediente.derivaciones = new List<Derivacion>();
            expediente.estado = "solicitado";
            expediente = _expedienteservice.saveExpediente(expediente);

            _documentoservice.updateBandejaSalida(expediente.id, solicitudDenuncia.id, "5ee2e87bd12fcd05d6b6b13e");
            return solicitudDenuncia;
        }

        //Con el expediente agregado falta wrapper
        [HttpPost("documentosef")]
        public async Task<ActionResult<SolicitudExpedicionFirma>> RegistrarDocumentoSEF(SolicitudExpedicionFirmaDTO documento)
        {
            //por defecto
            string urlData = "www.google.com";
            if (!string.IsNullOrWhiteSpace(documento.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(documento.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicitudexpedicionfirma");
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

            documentoSEF = _documentoservice.registrarSolicitudExpedicionFirma(documentoSEF);

            Cliente cliente = new Cliente()
            {
                nombre = documento.nombrecliente,
                tipodocumento = documento.tipodocumento,
                numerodocumento = documento.numerodocumento
            };

            Expediente expediente = new Expediente();
            expediente.tipo = "Expedicion de Firmas";
            expediente.cliente = cliente;
            expediente.fechainicio = DateTime.Now;
            expediente.fechafin = null;
            expediente.documentos = new List<DocumentoExpediente>()
            {
                new DocumentoExpediente(){
                    indice = 1,
                    iddocumento = documentoSEF.id,
                    tipo="ExpedicionFirmas",
                    fechacreacion = documentoSEF.contenido.fecharealizacion,
                    fechaexceso = documentoSEF.contenido.fecharealizacion.AddDays(10),
                    fechademora = null
                }
            };
            expediente.derivaciones = new List<Derivacion>();
            expediente.estado = "solicitado";
            expediente = _expedienteservice.saveExpediente(expediente);

            _documentoservice.updateBandejaSalida(expediente.id, documentoSEF.id, "5ee2e87bd12fcd05d6b6b13e");
            return documentoSEF;
        }

        //Completo
        [HttpPost("documentocf")]
        public ActionResult<ConclusionFirma> RegistrarDocumentoCF(ExpedienteWrapper expediente)
        {
            ConclusionFirma documentoCF = new ConclusionFirma();
            documentoCF = _documentoservice.registrarConclusionFirmaE(expediente);
            return documentoCF;
        }
        /*Esto funciona por si algo sale mal
         * [HttpPost("documentosbpn")]
        public ActionResult<OficioBPN> RegistrarDocumentoOficioBPN(OficioBPNDTO documento)
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
            documentoODN = _documentoservice.registrarOficioBPN(documentoODN);
            return documentoODN;
        }

        [HttpPost("documentocf")]
        public async Task<ActionResult<ConclusionFirma>> RegistrarDocumentoCF(ConclusionFirmaDTO documento)
        {
            ContenidoConclusionFirma contenidoCF = new ContenidoConclusionFirma()
            {
                idescriturapublica = documento.contenidoDTO.idescriturapublica.id
            };
            ConclusionFirma documentoDF = new ConclusionFirma()
            {
                tipo = "ConclusionFirma",
                contenido = contenidoCF,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };

            documentoDF = _documentoservice.registrarConclusionFirma(documentoDF);
            _escrituraspublicasservice.updateEscrituraPublicaporConclusionFirma(documento.contenidoDTO.idescriturapublica);
            return documentoDF;
        }*/

        /*[HttpPut("estado")]
        public ActionResult<Documento> modificarEstado(Documento documento)
        {
            documento = _documentoservice.modificarEstado(documento);
            return documento;
        }*/
    }
}
