﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSolicitudDenuncia(ExpedienteWrapper expedientewrapper)
        {
            SolicitudDenunciaDTO documento = (SolicitudDenunciaDTO)expedientewrapper.documento;
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

            _documentoservice.updateBandejaSalida(expediente.id, solicitudDenuncia.id, expedientewrapper.idusuarioactual);


            DocumentoDTO doc = new DocumentoDTO();
            doc.id = solicitudDenuncia.id;
            doc.id = solicitudDenuncia.tipo;
            doc.historialcontenido = new List<ContenidoVersion>();
            doc.historialproceso = new List<Proceso>();
            doc.contenido = solicitudDenuncia.contenido;
            doc.estado = solicitudDenuncia.estado;

            ExpedienteBandejaDTO bandejaexpdto = new ExpedienteBandejaDTO();
            bandejaexpdto.idexpediente = expediente.id;
            bandejaexpdto.cliente = expediente.cliente;
            bandejaexpdto.documento = doc;
            bandejaexpdto.documentosobj = new List<DocumentoDTO>() { doc };
            bandejaexpdto.tipo = expediente.tipo;




            return bandejaexpdto;
        }

        //Con el expediente agregado falta wrapper
        [HttpPost("documentosef")]
        public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSEF(ExpedienteWrapper expedientewrapper)
        {
            SolicitudExpedicionFirmaDTO conclusionfirmaDTO = new SolicitudExpedicionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            conclusionfirmaDTO = JsonConvert.DeserializeObject<SolicitudExpedicionFirmaDTO>(json);

            string urlData = "";
            if (!string.IsNullOrWhiteSpace(conclusionfirmaDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(conclusionfirmaDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicitudexpedicionfirma");
            }
            ContenidoSolicitudExpedicionFirma contenidoSEF = new ContenidoSolicitudExpedicionFirma()
            {
                titulo = conclusionfirmaDTO.contenidoDTO.titulo,
                descripcion = conclusionfirmaDTO.contenidoDTO.descripcion,
                fecharealizacion = DateTime.Now,
                cliente = conclusionfirmaDTO.contenidoDTO.cliente,
                codigo = conclusionfirmaDTO.contenidoDTO.codigo,
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
                nombre = conclusionfirmaDTO.nombrecliente,
                tipodocumento = conclusionfirmaDTO.tipodocumento,
                numerodocumento = conclusionfirmaDTO.numerodocumento
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

            _documentoservice.updateBandejaSalida(expediente.id, documentoSEF.id, expedientewrapper.idusuarioactual);

            DocumentoDTO doc = new DocumentoDTO();
            doc.id = documentoSEF.id;
            doc.id = documentoSEF.tipo;
            doc.historialcontenido = new List<ContenidoVersion>();
            doc.historialproceso = new List<Proceso>();
            doc.contenido = documentoSEF.contenido;
            doc.estado = documentoSEF.estado;

            ExpedienteBandejaDTO bandejaexpdto = new ExpedienteBandejaDTO();
            bandejaexpdto.idexpediente = expediente.id;
            bandejaexpdto.cliente = expediente.cliente;
            bandejaexpdto.documento = doc;
            bandejaexpdto.documentosobj = new List<DocumentoDTO>() { doc };
            bandejaexpdto.tipo = expediente.tipo;


            return bandejaexpdto;
        }

        //Completo
        [HttpPost("documentocf")]
        public ActionResult<ConclusionFirma> RegistrarDocumentoCF(ExpedienteWrapper expediente)
        {
            ConclusionFirmaDTO conclusionfirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expediente.documento);
            conclusionfirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);

            ConclusionFirma documentoCF = new ConclusionFirma();
            documentoCF = _documentoservice.registrarConclusionFirmaE(expediente);
            _escrituraspublicasservice.updateEscrituraPublicaporConclusionFirma(conclusionfirmaDTO.contenidoDTO.idescriturapublica);
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

        [HttpPut("cambiarestado")]
        public ActionResult<Documento> ModificarEstado(DocumentoEvaluadoDTO documento)
        {
            return _documentoservice.modificarEstado(documento);
        }
    }
}
