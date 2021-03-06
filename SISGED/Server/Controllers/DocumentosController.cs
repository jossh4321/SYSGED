﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
    public class DocumentosController : ControllerBase
    {
        private readonly DocumentoService _documentoservice;
        private readonly ExpedienteService _expedienteservice;
        private readonly BandejaService _bandejaService;
        private readonly EscriturasPublicasService _escrituraspublicasservice;
        private readonly IFileStorage _almacenadorDeDocs;
        private readonly AsistenteService asistenteService;

        public DocumentosController(DocumentoService documentoservice, IFileStorage almacenadorDeDocs,
            ExpedienteService expedienteservice, EscriturasPublicasService escrituraspublicasservice, BandejaService bandejaService,
            AsistenteService asistenteService)
        {
            _documentoservice = documentoservice;
            _almacenadorDeDocs = almacenadorDeDocs;
            _expedienteservice = expedienteservice;
            _escrituraspublicasservice = escrituraspublicasservice;
            _bandejaService = bandejaService;
            this.asistenteService = asistenteService;
        }
        [HttpGet("tododoc")]
        public ActionResult<List<Documento>> Get()
        {
            return _documentoservice.obtenerDocumentos();
        }

        #region Registros de Documentos
        [HttpPost("documentoodn")]
        public async Task<ActionResult<OficioDesignacionNotario>> RegistrarDocumentoODN(ExpedienteWrapper expediente)
        {
            OficioDesignacionNotarioDTO oficioDesignacionNotarioDTO = new OficioDesignacionNotarioDTO();
            var json = JsonConvert.SerializeObject(expediente.documento);
            oficioDesignacionNotarioDTO = JsonConvert.DeserializeObject<OficioDesignacionNotarioDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in oficioDesignacionNotarioDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "oficiodesignacionnotario");
                    url2.Add(urlData2);
                }
            }
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario();
            documentoODN = _documentoservice.registrarOficioDesignacionNotario(expediente, url2);
            return documentoODN;
        }

        [HttpPost("documentosbpn")]
        public async Task<ActionResult<OficioBPN>> RegistrarDocumentoOficioBPN(ExpedienteWrapper expediente)
        {
            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            var json = JsonConvert.SerializeObject(expediente.documento);
            oficioBPNDTO = JsonConvert.DeserializeObject<OficioBPNDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in oficioBPNDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "oficiobpn");
                    url2.Add(urlData2);
                }
            }
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(oficioBPNDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(oficioBPNDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "oficiobpn");
            }
            OficioBPN documentoOficioBPN = new OficioBPN();
            documentoOficioBPN = _documentoservice.registrarOficioBPNE(expediente, url2, urlData);

            return documentoOficioBPN;
        }

        [HttpPost("documentosolicbpn")]
        public async Task<ActionResult<SolicitudBPN>> RegistrarDocumentoSolicitudBPN(ExpedienteWrapper expedienteWrapper)
        {
            //Obtenemos los datos del expedientewrapper
            SolicitudBPNDTO documento = new SolicitudBPNDTO();
            ContenidoSolicitudBPNDTO listaotor = new ContenidoSolicitudBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            documento = JsonConvert.DeserializeObject<SolicitudBPNDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";

            foreach (string u in documento.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "solicitudbpn");
                    url2.Add(urlData2);
                }
            }

            SolicitudBPN solicitudBPN = new SolicitudBPN();
            solicitudBPN = _documentoservice.registrarSolicitudBPN(expedienteWrapper, url2);

            return solicitudBPN;
        }

        [HttpPost("documentosd")]
        //public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSolicitudDenuncia(ExpedienteWrapper expedientewrapper)
        public async Task<ActionResult<SolicitudDenuncia>> RegistrarDocumentoSolicitudDenuncia(ExpedienteWrapper expedientewrapper)
        {

            //conversion de Object a Tipo especifico
            SolicitudDenunciaDTO documento = new SolicitudDenunciaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            documento = JsonConvert.DeserializeObject<SolicitudDenunciaDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in documento.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "solicituddenuncia");
                    url2.Add(urlData2);
                }
            }
            //subida de archivo a repositorio y retorno de url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(documento.contenidoDTO.urldata))
            {
                var solicitudBytes = Convert.FromBase64String(documento.contenidoDTO.urldata);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicituddenuncia");
            }

            SolicitudDenuncia solicitudDenuncia = new SolicitudDenuncia();
            solicitudDenuncia = _documentoservice.registrarSolicitudDenuncia(expedientewrapper, url2, urlData);

            return solicitudDenuncia;
        }

        [HttpPost("documentosef")]
        //public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSEF(ExpedienteWrapper expedientewrapper)
        public async Task<ActionResult<SolicitudExpedicionFirma>> RegistrarDocumentoSEF(ExpedienteWrapper expedientewrapper)
        {
            //Conversion de Obj a tipo SolicitudExpedicionFirmaDTO
            SolicitudExpedicionFirmaDTO solicitudExpedicionFirmasDTO = new SolicitudExpedicionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            solicitudExpedicionFirmasDTO = JsonConvert.DeserializeObject<SolicitudExpedicionFirmaDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in solicitudExpedicionFirmasDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "solicitudexpedicionfirma");
                    url2.Add(urlData2);
                }
            }
            //Almacenamiento de archivo en repositorio y obtnecion de url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(solicitudExpedicionFirmasDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(solicitudExpedicionFirmasDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicitudexpedicionfirma");
            }

            SolicitudExpedicionFirma documentoSEF = new SolicitudExpedicionFirma();
            documentoSEF = _documentoservice.registrarSolicitudExpedicionFirma(expedientewrapper, url2, urlData);

            return documentoSEF;
        }

        [HttpPost("registrarsolicitudinicial")]
        public async Task<ActionResult<ExpedienteDocumentoSIDTO>> RegistrarDocumentoSolicitudInicial(ExpedienteWrapper expedienteWrapper)
        {
            //Obtenemos los datos del expedientewrapper
            SolicitudInicialDTO doc = new SolicitudInicialDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            doc = JsonConvert.DeserializeObject<SolicitudInicialDTO>(json);

            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in doc.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "solicitudesiniciales");
                    url2.Add(urlData2);
                }
            }

            //Creacionde Obj y almacenamiento en la coleccion documento
            ContenidoSolicitudInicial contenidoDTOInicial = new ContenidoSolicitudInicial()
            {
                titulo = doc.contenidoDTO.titulo,
                descripcion = doc.contenidoDTO.descripcion,
            };

            SolicitudInicial soliInicial = new SolicitudInicial()
            {
                tipo = "SolicitudInicial",
                contenido = contenidoDTOInicial,
                estado = "pendiente",
                urlanexo = url2,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };

            soliInicial = _documentoservice.registrarSolicitudInicial(soliInicial);

            //Creacionde del Obj. Expediente de Denuncia y registro en coleccion de expedientes
            Cliente cliente = new Cliente()
            {
                nombre = doc.nombrecliente,
                tipodocumento = doc.tipodocumento,
                numerodocumento = doc.numerodocumento
            };
            Expediente expediente = new Expediente();
            expediente.tipo = "Solicitud";
            expediente.cliente = cliente;
            expediente.fechainicio = DateTime.UtcNow.AddHours(-5);
            expediente.fechafin = null;
            expediente.documentos = new List<DocumentoExpediente>()
            {
                new DocumentoExpediente(){
                    indice = 1,
                    iddocumento = soliInicial.id,
                    tipo  = "SolicitudInicial",
                    fechacreacion = DateTime.UtcNow.AddHours(-5),
                    fechaexceso= DateTime.UtcNow.AddHours(-5).AddDays(10),
                    fechademora = null
                }
            };
            expediente.derivaciones = new List<Derivacion>();
            expediente.estado = "solicitado";
            expediente = _expedienteservice.saveExpediente(expediente);

            _bandejaService.InsertarBandejaEntradaUsuario(expediente.id, soliInicial.id, "josue");

            Asistente asistente = new Asistente();
            asistente.idexpediente = expediente.id;
            asistente.pasos = new PasoAsistente();
            asistente.pasos.nombreexpediente = "Solicitud";

            await asistenteService.Create(asistente);

            ExpedienteDocumentoSIDTO expedienteDocumentoSIDTO = new ExpedienteDocumentoSIDTO();
            expedienteDocumentoSIDTO.expediente = expediente;
            expedienteDocumentoSIDTO.solicitudI = soliInicial;

            return expedienteDocumentoSIDTO;
        }


        [HttpPost("documentocf")]
        public async Task<ActionResult<ConclusionFirma>> RegistrarDocumentoCF(ExpedienteWrapper expediente)
        {
            ConclusionFirmaDTO conclusionfirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expediente.documento);
            conclusionfirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in conclusionfirmaDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "conclusionfirma");
                    url2.Add(urlData2);
                }
            }

            ExpedienteDTO expedientePorConsultar = _expedienteservice.getById(expediente.idexpediente);
            DocumentoExpediente documentosolicitud = expedientePorConsultar.documentos.Find(x => x.tipo == "SolicitudInicial");

            ConclusionFirma documentoCF = new ConclusionFirma();
            documentoCF = _documentoservice.registrarConclusionFirmaE(expediente, url2, documentosolicitud.iddocumento);
            _escrituraspublicasservice.updateEscrituraPublicaporConclusionFirma(conclusionfirmaDTO.contenidoDTO.idescriturapublica);
            return documentoCF;
        }


        [HttpPost("documentoad")]
        public async Task<ActionResult<AperturamientoDisciplinario>> RegistrarDocumentoAperturamientoDisciplinario(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo AperturamientoDisciplinario
            AperturamientoDisciplinarioDTO aperturamientoDisciplinarioDTO = new AperturamientoDisciplinarioDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            aperturamientoDisciplinarioDTO = JsonConvert.DeserializeObject<AperturamientoDisciplinarioDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in aperturamientoDisciplinarioDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "aperturamientodiciplinario");
                    url2.Add(urlData2);
                }
            }
            //Almacenando el pdf en el servidor de archivos y obtencion de la url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(aperturamientoDisciplinarioDTO.contenidoDTO.url))
            {
                var solicitudBytes = Convert.FromBase64String(aperturamientoDisciplinarioDTO.contenidoDTO.url);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "aperturamientodisciplinario");
            }

            return _documentoservice.registrarAperturamientoDisciplinario(aperturamientoDisciplinarioDTO, urlData, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }


        [HttpPost("documentoAPE")]
        public async Task<ActionResult<Apelacion>> registrarDocumentoApelacion(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo Apelacion
            ApelacionDTO apelacionDTO = new ApelacionDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            apelacionDTO = JsonConvert.DeserializeObject<ApelacionDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in apelacionDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "apelaciones");
                    url2.Add(urlData2);
                }
            }
            //Almacenando el pdf en el servidor de archivos y obtencion de la url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(apelacionDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(apelacionDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "apelaciones");
            }

            return _documentoservice.registrarApelacion(apelacionDTO, urlData, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }


        [HttpPost("documentoSEN")]
        public async Task<ActionResult<SolicitudExpedienteNotario>> registrarSolicitudExpedienteNotario(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo Apelacion
            SolicitudExpedienteNotarioDTO solicitudExpedienteNotarioDTO = new SolicitudExpedienteNotarioDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            solicitudExpedienteNotarioDTO = JsonConvert.DeserializeObject<SolicitudExpedienteNotarioDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in solicitudExpedienteNotarioDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "solicitudexpedientenotario");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.registrarSolicitudExpedienteNotario(solicitudExpedienteNotarioDTO, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }


        [HttpPost("documentod")]
        public async Task<ActionResult<Dictamen>> RegistrarDocumentoDictamen(ExpedienteWrapper expedientewrapper)
        {
            DictamenDTO dictamenDTO = new DictamenDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            dictamenDTO = JsonConvert.DeserializeObject<DictamenDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in dictamenDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "dictamen");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.RegistrarDictamen(dictamenDTO, expedientewrapper, url2);
        }

        [HttpPost("documentor")]
        public async Task<ActionResult<Resolucion>> RegistrarDocumentoResolucion(ExpedienteWrapper expedientewrapper)
        {
            ResolucionDTO resolucionDTO = new ResolucionDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            resolucionDTO = JsonConvert.DeserializeObject<ResolucionDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in resolucionDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "resolucion");
                    url2.Add(urlData2);
                }
            }
            //Almacenando el pdf en el servidor de archivos y obtencion de la url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(resolucionDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(resolucionDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "resolucion");
            }

            ExpedienteDTO expedientePorConsultar = _expedienteservice.getById(expedientewrapper.idexpediente);
            DocumentoExpediente documentosolicitud = expedientePorConsultar.documentos.Find(x => x.tipo == "SolicitudInicial");

            return _documentoservice.registrarResolucion(resolucionDTO, urlData, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada, documentosolicitud.iddocumento);
        }

        [HttpPost("documentoResultadoBPN")]
        public async Task<ActionResult<ResultadoBPN>> RegistrarDocumentoResultadoBPN(ExpedienteWrapper expedientewrapper)
        {
            ResultadoBPNDTO resultadoBPNDTO = new ResultadoBPNDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            resultadoBPNDTO = JsonConvert.DeserializeObject<ResultadoBPNDTO>(json);

            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in resultadoBPNDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "resultadobpn");
                    url2.Add(urlData2);
                }
            }

            ExpedienteDTO expedientePorConsultar = _expedienteservice.getById(expedientewrapper.idexpediente);
            DocumentoExpediente documentosolicitud = expedientePorConsultar.documentos.Find(x => x.tipo == "SolicitudInicial");

            return _documentoservice.registrarResultadoBPN(resultadoBPNDTO, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada, documentosolicitud.iddocumento);
        }

        [HttpPost("documentoEntregaExpedienteNotario")]
        public async Task<ActionResult<EntregaExpedienteNotario>> RegistrarDocumentoEntregaExpedienteNotario(ExpedienteWrapper expedientewrapper)
        {
            //Deserealizacion de objeto de tipo C
            EntregaExpedienteNotarioDTO entregaExpedienteNotarioDTO = new EntregaExpedienteNotarioDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            entregaExpedienteNotarioDTO = JsonConvert.DeserializeObject<EntregaExpedienteNotarioDTO>(json);

            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in entregaExpedienteNotarioDTO.contenidoDTO.urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "entregaexpedientenotario");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.registrarEntregaExpedienteNotario(entregaExpedienteNotarioDTO, expedientewrapper, url2);
        }
        #endregion

        [HttpPut("cambiarestado")]
        public ActionResult<Documento> ModificarEstado(Evaluacion documento, [FromQuery] string docId)
        {
            return _documentoservice.modificarEstado(documento, docId);
        }
        [HttpPut("generardocumento")]
        public async Task<ActionResult<Documento>> GenerarEstado(DocumentoGenerarDTO documento)
        {
            if (!string.IsNullOrWhiteSpace(documento.firma))
            {
                var solicitudBytes2 = Convert.FromBase64String(documento.firma);
                documento.firma = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "png", "resultadobpn");
            }
            if (!string.IsNullOrWhiteSpace(documento.urlDeGenerado))
            {
                var solicitudBytes2 = Convert.FromBase64String(documento.urlDeGenerado);
                documento.urlDeGenerado = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "resultadobpn");
            }

            return _documentoservice.generarDocumento(documento);
        }

        [HttpPut("cambiarestadodocumento")]
        public ActionResult<Documento> ModificarEstado(DocumentoDTO documento)
        {
            return _documentoservice.modificarEstadoDocumento(documento);
        }

        //obteniendo documentos

        [HttpGet("documentosolicitudes/{numerodocumento}")]
        public async Task<List<DocumentoADTO2>> obtenerSolicitudes(string numerodocumento)
        {
            return await _documentoservice.ObtenerSolicitudesUsuario(numerodocumento);
        }
        [HttpGet("documentosolicitudes2/{numerodocumento}")]
        public async Task<List<DocumentoADTO2>> obtenerSolicitudes2(string numerodocumento)
        {
            return await _documentoservice.ObtenerSolicitudesUsuario2(numerodocumento);
        }

        [HttpGet("documentosolicitudes3/{numerodocumento}")]
        public async Task<List<DocumentoADTO2>> obtenerSolicitudes3(string numerodocumento)
        {
            return await _documentoservice.ObtenerSolicitudesUsuario3(numerodocumento);
        }


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
        [HttpGet("documentosolbpn")]
        public async Task<ActionResult<SolicitudBPNDTO>> obtenerSolicitudBpn([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerSolicitudBusquedaProtocoloNotarial(iddoc);
        }
        /*
        [HttpGet("documentosold")]
        public async Task<ActionResult<OficioBPNDTO>> obtenerSolicitudDenuncia([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerOficioBusquedaProtocoloNotarial(iddoc);
        }
        [HttpGet("documentosolef")]
        public async Task<ActionResult<OficioBPNDTO>> obtenerSolicitudExpedicionFirmas([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerOficioBusquedaProtocoloNotarial(iddoc);
        }*/
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
        [HttpGet("documentorbpn")]
        public async Task<ActionResult<ResultadoBPNDTO>> obtenerResultadoBPN([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerResultadoBPNDTO(iddoc);
        }
        [HttpGet("documentoape")]
        public async Task<ActionResult<ApelacionDTO>> obtenerApelacionDTO([FromQuery] string iddoc)
        {
            return _documentoservice.ObtenerDocumentoApelacion(iddoc);
        }
        [HttpGet("documentocf")]
        public async Task<ActionResult<ConclusionFirmaDTO>> obtenerConclusionFirma([FromQuery] string iddoc)
        {
            return _documentoservice.ObtenerDocumentoConclusionFirma(iddoc);
        }

        [HttpGet("documentoad")]
        public async Task<ActionResult<AperturamientoDisciplinarioDTO>> obtenerAperturamientoDisciplinario([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerDocumentoAperturamientoDisciplinario(iddoc);
        }
        [HttpGet("documentosen")]
        public async Task<ActionResult<SolicitudExpedienteNotarioDTO>> obtenerSolicitudExpedienteNot([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerSolicitudExpedienteNotario(iddoc);

        }
        [HttpGet("documentodto")]
        public async Task<ActionResult<DocumentoDTO>> obtenerDocumento([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerDocumentoDTO(iddoc);
        }
        [HttpGet("obtenersolicitudinicial")]
        public async Task<ActionResult<SolicitudInicialDTO>> obtenerSolicitudInicial([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerSolicitudInicial(iddoc);
        }
        [HttpGet("obtenerdocumentoEEN")]
        public async Task<ActionResult<EntregaExpedienteNotarioDTO>> obtenerEntregaExpedienteNotario([FromQuery] string iddoc)
        {
            return _documentoservice.obtenerEntregaExpedienteNotarioDTO(iddoc);
        }

        //Actualizaciones
        [HttpPut("actualizarDocumentoODN")]
        public async Task<ActionResult<OficioDesignacionNotario>> modificarDocumentoODN(ExpedienteWrapper expedienteWrapper)
        {
            OficioDesignacionNotarioDTO oficioDesignacionNotarioDTO = new OficioDesignacionNotarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioDesignacionNotarioDTO = JsonConvert.DeserializeObject<OficioDesignacionNotarioDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in oficioDesignacionNotarioDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "oficiodesignacionnotario");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.actualizarDocumentoODN(expedienteWrapper, url2);
        }
        [HttpPut("actualizarDocumentoAPE")]
        public async Task<ActionResult<Apelacion>> modificarDocumentoApelacion(ExpedienteWrapper expedienteWrapper)
        {
            ApelacionDTO apelacionDTO = new ApelacionDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            apelacionDTO = JsonConvert.DeserializeObject<ApelacionDTO>(json);
            string urlData = "";
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in apelacionDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "apelaciones");
                    url2.Add(urlData2);
                }
            }
            if (!string.IsNullOrWhiteSpace(apelacionDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(apelacionDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "apelaciones");
            }
            return _documentoservice.actualizarDocumentoApelacion(expedienteWrapper, urlData, url2);
        }
        [HttpPut("actualizarDocumentoAD")]
        public async Task<ActionResult<AperturamientoDisciplinario>> modificarDocumentoAperturamientoDisciplinario(ExpedienteWrapper expedienteWrapper)
        {
            AperturamientoDisciplinarioDTO aperturamientoDisciplinarioDTO = new AperturamientoDisciplinarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            aperturamientoDisciplinarioDTO = JsonConvert.DeserializeObject<AperturamientoDisciplinarioDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in aperturamientoDisciplinarioDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "aperturamientodiciplinario");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.actualizarDocumentoAperturamientoDisciplinario(expedienteWrapper, url2);
        }
        [HttpPut("actualizarDocumentoCF")]
        public async Task<ActionResult<ConclusionFirma>> modificarDocumentoConclusionFirma(ExpedienteWrapper expedienteWrapper)
        {
            ConclusionFirmaDTO conclusionFirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            conclusionFirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in conclusionFirmaDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "dictamen");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.actualizarDocumentoConclusionFirma(expedienteWrapper, url2);
        }
        [HttpPut("actualizarDocumentoD")]
        public async Task<ActionResult<Dictamen>> modificarDocumentoDictamen(ExpedienteWrapper expedienteWrapper)
        {
            DictamenDTO dictamenDTO = new DictamenDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            dictamenDTO = JsonConvert.DeserializeObject<DictamenDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in dictamenDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "dictamen");
                    url2.Add(urlData2);
                }
            }
            
            return _documentoservice.actualizarDocumentoDictamen(expedienteWrapper, url2);
        }
        [HttpPut("actualizarDocumentoOficioBPN")]
        public async Task<ActionResult<OficioBPN>> modificarDocumentoOficioBPN(ExpedienteWrapper expedienteWrapper)
        {
            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioBPNDTO = JsonConvert.DeserializeObject<OficioBPNDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in oficioBPNDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "oficiobpn");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.actualizarDocumentoOficioBPN(expedienteWrapper, url2);
        }
        [HttpPut("actualizarDocumentoR")]
        public async Task<ActionResult<Resolucion>> modificarDocumentoResolucion(ExpedienteWrapper expedienteWrapper)
        {
            ResolucionDTO resolucionDTO = new ResolucionDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            resolucionDTO = JsonConvert.DeserializeObject<ResolucionDTO>(json);
            string urlData = "";
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in resolucionDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "resolucion");
                    url2.Add(urlData2);
                }
            }
            if (!string.IsNullOrWhiteSpace(resolucionDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(resolucionDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "resolucion");
            }
            return _documentoservice.actualizarDocumentoResolucion(expedienteWrapper, urlData, url2);
        }
        

        [HttpPut("actualizarDocumentoSEN")]
        public void modificarDocumentoSEN(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoSEN(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoResultadoBPN")]
        public async Task<ActionResult<ResultadoBPN>> modificarrDocumentoResultadoBPN(ExpedienteWrapper expedienteWrapper)
        {
            ResultadoBPNDTO resultadoBPNDTO = new ResultadoBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            resultadoBPNDTO = JsonConvert.DeserializeObject<ResultadoBPNDTO>(json);
            List<string> url2 = new List<string>();
            string urlData2 = "";
            foreach (string u in resultadoBPNDTO.contenidoDTO.Urlanexo)
            {
                if (!string.IsNullOrWhiteSpace(u))
                {
                    var solicitudBytes2 = Convert.FromBase64String(u);
                    urlData2 = await _almacenadorDeDocs.saveDoc(solicitudBytes2, "pdf", "resultadobpn");
                    url2.Add(urlData2);
                }
            }
            return _documentoservice.actualizarDocumentoResultadoBPN(expedienteWrapper, url2);
        }
        [HttpPut("actualizarDocumentoSolicitudInicial")]
        public void modificarDocumentoSolicitudInicial(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoSolicitudInicial(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoEEN")]
        public void modificarDocumentoEEN(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoEEN(expedienteWrapper);
        }

        [HttpPut("estadosolicitud")]
        public void modificarEstadoSolicitudInicial(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.modifyState(expedienteWrapper);
        }
        [HttpGet("ganttexpediente")]
        public async Task<List<Expediente_group>> listaexpedientesgantt([FromQuery] string dni)
        {
            List<Expediente_group> estadisticas = new List<Expediente_group>();
            estadisticas = await _expedienteservice.listaexpedientegantt(dni);
            return estadisticas;
        }
    }
}
