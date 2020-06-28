using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("documentosd")]
        public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSolicitudDenuncia(ExpedienteWrapper expedientewrapper)
        {
            //conversion de Object a Tipo especifico
            SolicitudDenunciaDTO documento = new SolicitudDenunciaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            documento = JsonConvert.DeserializeObject<SolicitudDenunciaDTO>(json);

            //subida de archivo a repositorio y retorno de url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(documento.contenidoDTO.urldata))
            {
                var solicitudBytes = Convert.FromBase64String(documento.contenidoDTO.urldata);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicituddenuncia");
            }
            //Creacionde Obj SolicitudDenuncia y almacenamiento en la coleccion documento
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

            //Creacionde del Obj. Expediente de Denuncia y registro en coleccion de expedientes
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

            //actualizacion de bandeja de salida del usuario
            _documentoservice.updateBandejaSalida(expediente.id, solicitudDenuncia.id, expedientewrapper.idusuarioactual);

            //creacion de obj ExpedienteBandejaDTO
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

        [HttpPost("documentosef")]
        public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSEF(ExpedienteWrapper expedientewrapper)
        {
            //Conversion de Obj a tipo SolicitudExpedicionFirmaDTO
            SolicitudExpedicionFirmaDTO conclusionfirmaDTO = new SolicitudExpedicionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            conclusionfirmaDTO = JsonConvert.DeserializeObject<SolicitudExpedicionFirmaDTO>(json);

            //Almacenamiento de archivo en repositorio y obtnecion de url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(conclusionfirmaDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(conclusionfirmaDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicitudexpedicionfirma");
            }

            //Registro de objeto ContenidoSolicitudExpedicionFirma y registro en coleccion documentos
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

            //Creacion del objeto Expediente y registro en la coleccion Expedientes
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

            //Actualizacion de la bandeja de salida del usuario con el expediente
            _documentoservice.updateBandejaSalida(expediente.id, documentoSEF.id, expedientewrapper.idusuarioactual);

            //Creacion del objeto ExpedienteBandejaDTO y retorno
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
       
        [HttpPut("cambiarestado")]
        public ActionResult<Documento> ModificarEstado(DocumentoEvaluadoDTO documento)
        {
            return _documentoservice.modificarEstado(documento);
        }

        //Completo
        [HttpPost("documentoad")]
        public async Task<ActionResult<AperturamientoDisciplinario>> RegistrarDocumentoAperturamientoDisciplinario(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo AperturamientoDisciplinario
            AperturamientoDisciplinarioDTO aperturamientoDisciplinarioDTO = new AperturamientoDisciplinarioDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            aperturamientoDisciplinarioDTO = JsonConvert.DeserializeObject<AperturamientoDisciplinarioDTO>(json);

            //Almacenando el pdf en el servidor de archivos y obtencion de la url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(aperturamientoDisciplinarioDTO.contenidoDTO.url))
            {
                var solicitudBytes = Convert.FromBase64String(aperturamientoDisciplinarioDTO.contenidoDTO.url);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "aperturamientodisciplinario");
            }

            return _documentoservice.registrarAperturamientoDisciplinario(aperturamientoDisciplinarioDTO, urlData, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }

        //Falta probar
        [HttpPost("documentoAPE")]
        public async Task<ActionResult<Apelacion>> registrarDocumentoApelacion(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo Apelacion
            ApelacionDTO apelacionDTO = new ApelacionDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            apelacionDTO = JsonConvert.DeserializeObject<ApelacionDTO>(json);

            //Almacenando el pdf en el servidor de archivos y obtencion de la url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(apelacionDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(apelacionDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "apelaciones");
            }

            return _documentoservice.registrarApelacion(apelacionDTO, urlData, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }

        //Falta probar
        [HttpPost("documentoSEN")]
        public async Task<ActionResult<SolicitudExpedienteNotario>> registrarSolicitudExpedienteNotario(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo Apelacion
            SolicitudExpedienteNotarioDTO solicitudExpedienteNotarioDTO = new SolicitudExpedienteNotarioDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            solicitudExpedienteNotarioDTO = JsonConvert.DeserializeObject<SolicitudExpedienteNotarioDTO>(json);

            return _documentoservice.registrarSolicitudExpedienteNotario(solicitudExpedienteNotarioDTO, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }


        [HttpPost("documentod")]
        public async Task<ActionResult<Dictamen>> RegistrarDocumentoDictamen(ExpedienteWrapper expedientewrapper)
        {
            return _documentoservice.RegistrarDictamen(expedientewrapper);
        }

        [HttpPost("documentor")]
        public async Task<ActionResult<Resolucion>> RegistrarDocumentoResolucion(ExpedienteWrapper expedientewrapper)
        {
            ResolucionDTO resolucionDTO = new ResolucionDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            resolucionDTO = JsonConvert.DeserializeObject<ResolucionDTO>(json);

            //Almacenando el pdf en el servidor de archivos y obtencion de la url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(resolucionDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(resolucionDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "resolucion");
            }
            return _documentoservice.registrarResolucion(resolucionDTO, urlData, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }
        //obteniendo documentos
        [HttpGet("documentoodn")]
        public async Task<ActionResult<OficioDesignacionNotarioDTO>> obtenerOficioDesignacionNotario([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerOficioDesignacionNotario(iddoc);
        }
        [HttpGet("documentoobpn")]
        public async Task<ActionResult<OficioBPNDTO>> obtenerOficioBpn([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerOficioBusquedaProtocoloNotarial(iddoc);
        }
        [HttpGet("documentod")]
        public async Task<ActionResult<DictamenDTO>> obtenerDictamen([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerDictamenDTO(iddoc);
        }
        [HttpGet("documentord")]
        public async Task<ActionResult<ResolucionDTO>> obtenerResolucion([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerResolucionDTO(iddoc);
        }
    }
}
