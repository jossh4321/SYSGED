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

        #region Registros de Documentos
        [HttpPost("documentoodn")]
        public ActionResult<OficioDesignacionNotario> RegistrarDocumentoODN(ExpedienteWrapper expediente)
        {
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario();
             documentoODN = _documentoservice.registrarOficioDesignacionNotario(expediente);
            return documentoODN;
        }

        [HttpPost("documentosbpn")]
        public ActionResult<OficioBPN> RegistrarDocumentoOficioBPN(ExpedienteWrapper expediente)
        {
            OficioBPN documentoOficioBPN = new OficioBPN();
            documentoOficioBPN = _documentoservice.registrarOficioBPNE(expediente);
            return documentoOficioBPN;
        }

        [HttpPost("documentosolicbpn")]
        public ActionResult<SolicitudBPN> RegistrarDocumentoSolicitudBPN(ExpedienteWrapper expedienteWrapper)
        {
            /*
            SolicitudBPN documentoSolicBPN = new SolicitudBPN();
            documentoSolicBPN = _documentoservice.registrarSolicitudBPN(expediente);
            return documentoSolicBPN;*/


            //Obtenemos los datos del expedientewrapper
            SolicitudBPNDTO documento = new SolicitudBPNDTO();
            ContenidoSolicitudBPNDTO listaotor = new ContenidoSolicitudBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            documento = JsonConvert.DeserializeObject<SolicitudBPNDTO>(json);

            //Solo para registrar nombre de otorgantes
            List<String> listadeotorgantes = new List<string>();
            foreach (Otorgantelista obs in documento.contenidoDTO.otorganteslista)
            {
                listadeotorgantes.Add(obs.nombre);
            }

            //Creacionde Obj ContenidoSolicitudBPN y almacenamiento en la coleccion documento
            ContenidoSolicitudBPN contenidoSolicitudBPN = new ContenidoSolicitudBPN()
            {
                
                idcliente = documento.contenidoDTO.idcliente.id,
                direccionoficio = documento.contenidoDTO.direccionoficio,
                idnotario = documento.contenidoDTO.idnotario.id,
                actojuridico = documento.contenidoDTO.actojuridico,
                tipoprotocolo = documento.contenidoDTO.tipoprotocolo,
                otorgantes = listadeotorgantes,
                //Lista de objetos
                //otorganteslista = documento.contenidoDTO.otorganteslista,
                fecharealizacion = DateTime.Now,
                //url = "ninguna"
            };

            

            SolicitudBPN solicitudBPN = new SolicitudBPN()
            {
                tipo = "SolicitudBPN",
                contenido = contenidoSolicitudBPN,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            solicitudBPN = _documentoservice.registrarSolicitudBPN(solicitudBPN);
            //_documentos.InsertOne(solicitudBPN);
            //Pegar aqui lo que se cortó

            //Creacionde del Obj. Expediente de Denuncia y registro en coleccion de expedientes
            Cliente cliente = new Cliente()
            {
                nombre = documento.nombrecliente,
                tipodocumento = documento.tipodocumento,
                numerodocumento = documento.numerodocumento
            };
            Expediente expediente = new Expediente();
            expediente.tipo = "SolicitudBPN";
            expediente.cliente = cliente;
            expediente.fechainicio = DateTime.Now;
            expediente.fechafin = null;
            expediente.documentos = new List<DocumentoExpediente>()
            {
                new DocumentoExpediente(){
                    indice = 1,
                    iddocumento = solicitudBPN.id,
                    tipo="SolicitudBPN",
                    fechacreacion = solicitudBPN.contenido.fecharealizacion,
                    fechaexceso=solicitudBPN.contenido.fecharealizacion.AddDays(10),
                    fechademora = null
                }
            };
            expediente.derivaciones = new List<Derivacion>();
            expediente.estado = "solicitado";
            expediente = _expedienteservice.saveExpediente(expediente);

            //actualizacion de bandeja de salida del usuario
            _documentoservice.updateBandejaSalida(expediente.id, solicitudBPN.id, expedienteWrapper.idusuarioactual);

            //Actualizacion de bandeja de salida de usuario
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandeja);

            //Actualizacion de bandeja de entrada de usuario
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);*/
            return solicitudBPN;

        }

        [HttpPost("documentosd")]
        //public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSolicitudDenuncia(ExpedienteWrapper expedientewrapper)
        public async Task<ActionResult<SolicitudDenunciaDTO>> RegistrarDocumentoSolicitudDenuncia(ExpedienteWrapper expedientewrapper)
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

            //Creacionde Obj ContenidoSolicitudDenuncia y almacenamiento en la coleccion documento
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
                historialproceso = new List<Proceso>(),

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
            /*DocumentoDTO doc = new DocumentoDTO();
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

            return bandejaexpdto;*/
            return documento;
        }

        [HttpPost("documentosef")]
        //public async Task<ActionResult<ExpedienteBandejaDTO>> RegistrarDocumentoSEF(ExpedienteWrapper expedientewrapper)
        public async Task<ActionResult<SolicitudExpedicionFirmaDTO>> RegistrarDocumentoSEF(ExpedienteWrapper expedientewrapper)
        {
            //Conversion de Obj a tipo SolicitudExpedicionFirmaDTO
            SolicitudExpedicionFirmaDTO solicitudExpedicionFirmasDTO = new SolicitudExpedicionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            solicitudExpedicionFirmasDTO = JsonConvert.DeserializeObject<SolicitudExpedicionFirmaDTO>(json);

            //Almacenamiento de archivo en repositorio y obtnecion de url
            string urlData = "";
            if (!string.IsNullOrWhiteSpace(solicitudExpedicionFirmasDTO.contenidoDTO.data))
            {
                var solicitudBytes = Convert.FromBase64String(solicitudExpedicionFirmasDTO.contenidoDTO.data);
                urlData = await _almacenadorDeDocs.saveDoc(solicitudBytes, "pdf", "solicitudexpedicionfirma");
            }

            //Registro de objeto ContenidoSolicitudExpedicionFirma y registro en coleccion documentos
            ContenidoSolicitudExpedicionFirma contenidoSEF = new ContenidoSolicitudExpedicionFirma()
            {
                titulo = solicitudExpedicionFirmasDTO.contenidoDTO.titulo,
                descripcion = solicitudExpedicionFirmasDTO.contenidoDTO.descripcion,
                fecharealizacion = DateTime.Now,
                cliente = solicitudExpedicionFirmasDTO.contenidoDTO.cliente,
                codigo = solicitudExpedicionFirmasDTO.contenidoDTO.codigo,
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
                nombre = solicitudExpedicionFirmasDTO.nombrecliente,
                tipodocumento = solicitudExpedicionFirmasDTO.tipodocumento,
                numerodocumento = solicitudExpedicionFirmasDTO.numerodocumento
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
            ///_documentoservice.updateBandejaSalida(expediente.id, documentoSEF.id, expedientewrapper.idusuarioactual);

            //Creacion del objeto ExpedienteBandejaDTO y retorno
            /*DocumentoDTO doc = new DocumentoDTO();
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
            
            return bandejaexpdto;*/
            return solicitudExpedicionFirmasDTO;
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
            ConclusionFirma documentoCF = new ConclusionFirma();
            documentoCF = _documentoservice.registrarConclusionFirmaE(expediente, url2);
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
            return _documentoservice.registrarResolucion(resolucionDTO, urlData, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
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

            return _documentoservice.registrarResultadoBPN(resultadoBPNDTO, url2, expedientewrapper.idusuarioactual, expedientewrapper.idexpediente, expedientewrapper.documentoentrada);
        }
        #endregion

        [HttpPut("cambiarestado")]
        public ActionResult<Documento> ModificarEstado(DocumentoEvaluadoDTO documento)
        {
            return _documentoservice.modificarEstado(documento);
        }
        [HttpPut("generardocumento")]
        public ActionResult<Documento> GenerarEstado(DocumentoGenerarDTO documento)
        {
            return _documentoservice.generarDocumento(documento);
        }

        [HttpPut("cambiarestadodocumento")]
        public ActionResult<Documento> ModificarEstado(DocumentoDTO documento)
        {
            return _documentoservice.modificarEstadoDocumento(documento);
        }

        //obteniendo documentos

        [HttpGet("documentosolicitudes")]
        public async Task<List<Documento>> obtenerSolicitudes()
        {
            return _documentoservice.obtenerSolicitudes();
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

        //Actualizaciones
        [HttpPut("actualizarDocumentoODN")]
        public void modificarDocumentoODN(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoODN(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoAPE")]
        public void modificarDocumentoApelacion(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoApelacion(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoAD")]
        public void modificarDocumentoAperturamientoDisciplinario(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoAperturamientoDisciplinario(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoCF")]
        public void modificarDocumentoConclusionFirma(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoConclusionFirma(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoD")]
        public void modificarDocumentoDictamen(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoDictamen(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoOficioBPN")]
        public void modificarDocumentoOficioBPN(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoOficioBPN(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoR")]
        public void modificarDocumentoResolucion(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoResolucion(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoSEN")]
        public void modificarDocumentoSEN(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoSEN(expedienteWrapper);
        }
        [HttpPut("actualizarDocumentoResultadoBPN")]
        public void modificarrDocumentoResultadoBPN(ExpedienteWrapper expedienteWrapper)
        {
            _documentoservice.actualizarDocumentoResultadoBPN(expedienteWrapper);
        }
    }
}
