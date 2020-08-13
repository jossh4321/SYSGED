using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class DocumentoService
    {
        private readonly IMongoCollection<Documento> _documentos;
        private readonly IMongoCollection<Expediente> _expedientes;
        private readonly IMongoCollection<Bandeja> _bandejas;
        private readonly ExpedienteService _expedienteservice;
        private readonly DocumentoService _documentoservice;
        public DocumentoService(ISysgedDatabaseSettings settings, ExpedienteService expedienteService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _documentos = database.GetCollection<Documento>("documentos");
            _expedientes = database.GetCollection<Expediente>("expedientes");
            _bandejas = database.GetCollection<Bandeja>("bandejas");
            _expedienteservice = expedienteService;
        }
        public List<Documento> obtenerDocumentos()
        {
            return _documentos.Find(documento => true).ToList();
        }
        public OficioDesignacionNotario registrarOficioDesignacionNotario(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo OficioDesignacionNotarioDTO
            OficioDesignacionNotarioDTO oficioDesignacionNotarioDTO = new OficioDesignacionNotarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioDesignacionNotarioDTO = JsonConvert.DeserializeObject<OficioDesignacionNotarioDTO>(json);

            //Creacion de Obj OficioDesignacionNotario y registro en coleccion de documentos 
            ContenidoOficioDesignacionNotario contenidoODN = new ContenidoOficioDesignacionNotario()
            {
                codigo = "",
                titulo = oficioDesignacionNotarioDTO.contenidoDTO.titulo,
                descripcion = oficioDesignacionNotarioDTO.contenidoDTO.descripcion,
                fecharealizacion = DateTime.UtcNow.AddHours(-5),
                lugaroficionotarial = oficioDesignacionNotarioDTO.contenidoDTO.lugaroficionotarial,
                idusuario = oficioDesignacionNotarioDTO.contenidoDTO.idusuario,
                idnotario = oficioDesignacionNotarioDTO.contenidoDTO.idnotario.id,
                firma = ""
            };
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario()
            {
                tipo = "OficioDesignacionNotario",
                contenido = contenidoODN,
                evaluacion = new Evaluacion()
                {
                    resultado = "pendiente",
                    evaluaciones = new List<EvaluacionIndividual>()
                },
                estado = "creado",
                urlanexo= url2,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            _documentos.InsertOne(documentoODN);

            //Actualizacion del expediente
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 2;
            documentoExpediente.iddocumento = documentoODN.id;
            documentoExpediente.tipo = "OficioDesignacionNotario";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == expedienteWrapper.idexpediente, updateExpediente);


            //actualizacion bandeja salida del usuario
            /*BandejaDocumento bandejaSalidaDocumento = new BandejaDocumento();
            bandejaSalidaDocumento.idexpediente = expediente.id;
            bandejaSalidaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandejaSalida = Builders<Bandeja>.Update.Push("bandejasalida", bandejaSalidaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaSalida);*/

            //actualizacion bandeja de entrada del usuario
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
                Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                  Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);*/

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedienteWrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return documentoODN;
        }

        public SolicitudBPN registrarSolicitudBPN(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
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
                codigo = "",
                idcliente = documento.contenidoDTO.idcliente.id,
                direccionoficio = documento.contenidoDTO.direccionoficio,
                idnotario = documento.contenidoDTO.idnotario.id,
                actojuridico = documento.contenidoDTO.actojuridico,
                tipoprotocolo = documento.contenidoDTO.tipoprotocolo,
                otorgantes = listadeotorgantes,
                fecharealizacion = DateTime.UtcNow.AddHours(-5),
                firma = ""
            };

            SolicitudBPN solicitudBPN = new SolicitudBPN()
            {
                tipo = "SolicitudBPN",
                contenido = contenidoSolicitudBPN,
                estado = "pendiente",
                urlanexo = url2,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };

            Expediente expediente = new Expediente();
            expediente.id = expedienteWrapper.idexpediente;
            expediente.tipo = "Busqueda Protocolo Notarial";
            

            _documentos.InsertOne(solicitudBPN);

            expediente.documentos = new List<DocumentoExpediente>()
            {
                new DocumentoExpediente(){
                    indice = 2,
                    iddocumento = solicitudBPN.id,
                    tipo="SolicitudBPN",
                    fechacreacion = solicitudBPN.contenido.fecharealizacion,
                    fechaexceso=solicitudBPN.contenido.fecharealizacion.AddDays(10),
                    fechademora = null
                }
            };

            _expedienteservice.updateExpedientBySolicitudInitial(expediente);

            var filter = Builders<Documento>.Filter.Eq("id", expedienteWrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            return solicitudBPN;
        }
        public OficioBPN registrarOficioBPNE(ExpedienteWrapper expedienteWrapper, List<string> url2, string url)
        {
            //Obtenemos los datos del expedientewrapper
            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioBPNDTO = JsonConvert.DeserializeObject<OficioBPNDTO>(json);

            List<String> otor = new List<string>();
            foreach (OtorganteDTO ot in oficioBPNDTO.contenidoDTO.otorgantes)
            {
                otor.Add(ot.nombre);
            }

            //Insertando el oficio normal
            ContenidoOficioBPN contenidoSolicitudBPN = new ContenidoOficioBPN()
            {
                codigo = "",
                titulo = oficioBPNDTO.contenidoDTO.titulo,
                descripcion = oficioBPNDTO.contenidoDTO.descripcion,
                idcliente = oficioBPNDTO.contenidoDTO.idcliente.id,
                direccionoficio = oficioBPNDTO.contenidoDTO.direccionoficio,
                idnotario = oficioBPNDTO.contenidoDTO.idnotario.id,
                actojuridico = oficioBPNDTO.contenidoDTO.actojuridico,
                tipoprotocolo = oficioBPNDTO.contenidoDTO.tipoprotocolo,
                otorgantes = otor,
                fecharealizacion = DateTime.Now,
                url = url,
                firma = ""
            };
            OficioBPN documentoBPN = new OficioBPN()
            {
                tipo = "OficioBPN",
                contenido = contenidoSolicitudBPN,
                //estado = "pendiente",
                evaluacion = new Evaluacion()
                {
                    resultado = "pendiente",
                    evaluaciones = new List<EvaluacionIndividual>(),
                },
                estado = "Creado",
                historialcontenido = new List<ContenidoVersion>(),
                urlanexo=url2,
                historialproceso = new List<Proceso>()
            };
            _documentos.InsertOne(documentoBPN);

            //Actualizamos el expediente y agregamos el documento a sus documentos contenidos
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 2;
            documentoExpediente.iddocumento = documentoBPN.id;
            documentoExpediente.tipo = "OficioBPN";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;

            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == expedienteWrapper.idexpediente, updateExpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedienteWrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return documentoBPN;
        }
        public SolicitudExpedicionFirma registrarSolicitudExpedicionFirma(ExpedienteWrapper expedientewrapper, List<string> url2, string urlData)
        {
            //Conversion de Obj a tipo SolicitudExpedicionFirmaDTO
            SolicitudExpedicionFirmaDTO solicitudExpedicionFirmasDTO = new SolicitudExpedicionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            solicitudExpedicionFirmasDTO = JsonConvert.DeserializeObject<SolicitudExpedicionFirmaDTO>(json);

            //Registro de objeto ContenidoSolicitudExpedicionFirma y registro en coleccion documentos
            ContenidoSolicitudExpedicionFirma contenidoSEF = new ContenidoSolicitudExpedicionFirma()
            {
                titulo = solicitudExpedicionFirmasDTO.contenidoDTO.titulo,
                descripcion = solicitudExpedicionFirmasDTO.contenidoDTO.descripcion,
                fecharealizacion = DateTime.UtcNow.AddHours(-5),
                cliente = solicitudExpedicionFirmasDTO.nombrecliente,
                codigo = solicitudExpedicionFirmasDTO.contenidoDTO.codigo,
                url = urlData
            };
            SolicitudExpedicionFirma documentoSEF = new SolicitudExpedicionFirma()
            {
                tipo = "SolicitudExpedicionFirma",
                contenido = contenidoSEF,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                urlanexo = url2,
                historialproceso = new List<Proceso>()
            };

            //Creacion del objeto Expediente y registro en la coleccion Expedientes
            Expediente expediente = new Expediente();
            expediente.id = expedientewrapper.idexpediente;
            expediente.tipo = "Expedicion de Firmas";
            
            expediente.derivaciones = new List<Derivacion>();
            expediente.estado = "solicitado";

            _documentos.InsertOne(documentoSEF);

            expediente.documentos = new List<DocumentoExpediente>()
            {
                new DocumentoExpediente(){
                    indice = 1,
                    iddocumento = documentoSEF.id,
                    tipo="SolicitudExpedicionFirma",
                    fechacreacion = documentoSEF.contenido.fecharealizacion,
                    fechaexceso = documentoSEF.contenido.fecharealizacion.AddDays(10),
                    fechademora = null
                }
            };

            _expedienteservice.updateExpedientBySolicitudInitial(expediente);

            var filter = Builders<Documento>.Filter.Eq("id", expedientewrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            return documentoSEF;
        }

        public void updateBandejaSalida(string idexp, string iddoc, string idusuario)
        {
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = idexp;
            bandejaDocumento.iddocumento = iddoc;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);
        }

        public SolicitudDenuncia registrarSolicitudDenuncia(ExpedienteWrapper expedientewrapper, List<string> url2, string urlData)
        {
            //conversion de Object a Tipo especifico
            SolicitudDenunciaDTO documento = new SolicitudDenunciaDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            documento = JsonConvert.DeserializeObject<SolicitudDenunciaDTO>(json);

            //Creacionde Obj ContenidoSolicitudDenuncia y almacenamiento en la coleccion documento
            ContenidoSolicitudDenuncia contenidoSolicitudDenuncia = new ContenidoSolicitudDenuncia()
            {
                codigo = documento.contenidoDTO.codigo,
                titulo = documento.contenidoDTO.titulo,
                descripcion = documento.contenidoDTO.descripcion,
                nombrecliente = documento.nombrecliente,
                fechaentrega = DateTime.UtcNow.AddHours(-5),
                url = urlData
            };

            SolicitudDenuncia solicitudDenuncia = new SolicitudDenuncia()
            {
                tipo = "SolicitudDenuncia",
                contenido = contenidoSolicitudDenuncia,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                urlanexo = url2,
                historialproceso = new List<Proceso>(),
            };

            Expediente expediente = new Expediente();
            expediente.id = expedientewrapper.idexpediente;
            expediente.tipo = "Denuncia";
           
            expediente.derivaciones = new List<Derivacion>();
            expediente.estado = "solicitado";

            _documentos.InsertOne(solicitudDenuncia);

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

            _expedienteservice.updateExpedientBySolicitudInitial(expediente);

            var filter = Builders<Documento>.Filter.Eq("id", expedientewrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            return solicitudDenuncia;
        }

        public SolicitudInicial registrarSolicitudInicial(SolicitudInicial documentoSI)
        {
            _documentos.InsertOne(documentoSI);
            return documentoSI;
        }
        public ConclusionFirma registrarConclusionFirmaE(ExpedienteWrapper expedienteWrapper, List<string> url2, string iddocumentoSolicitud)
        {
            //Obtenemos los datos del expedientewrapper
            ConclusionFirmaDTO conclusionfirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            conclusionfirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);

            //Insertando la conclusion normal
            ContenidoConclusionFirma contenidoCF = new ContenidoConclusionFirma()
            {
                codigo = "",
                idescriturapublica = conclusionfirmaDTO.contenidoDTO.idescriturapublica.id,
                idnotario = conclusionfirmaDTO.contenidoDTO.idnotario.id,
                idcliente = conclusionfirmaDTO.contenidoDTO.idcliente.id,
                cantidadfoja = conclusionfirmaDTO.contenidoDTO.cantidadfoja,
                precio = (conclusionfirmaDTO.contenidoDTO.cantidadfoja * 30),
                firma = ""
            };

            ConclusionFirma documentoDF = new ConclusionFirma()
            {
                tipo = "ConclusionFirma", 
                contenido = contenidoCF,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                urlanexo = url2,
                historialproceso = new List<Proceso>()
            };
            _documentos.InsertOne(documentoDF);

            //Actualizamos el expediente y agregamos el documento a sus documentos contenidos
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = documentoDF.id;
            documentoExpediente.tipo = "ConclusionFirma";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;

            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == expedienteWrapper.idexpediente, updateExpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedienteWrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            //Actualizar el documento de solicitud a emitido, obtener el id de la solicitudBPN
            if (!string.IsNullOrEmpty(iddocumentoSolicitud))
            {
                var filterS = Builders<Documento>.Filter.Eq("id", iddocumentoSolicitud);
                var updateS = Builders<Documento>.Update
                    .Set("estado", "emitido");
                _documentos.UpdateOne(filterS, updateS);
            }
            return documentoDF;
        }

        /*Esto funciona
         * public ConclusionFirma registrarConclusionFirma(ConclusionFirma documentoCF)
        {
            _documentos.InsertOne(documentoCF);
            return documentoCF;
        }*/

        public Documento modificarEstado(Evaluacion documento, string docId)
        {
            var filter = Builders<Documento>.Filter.Eq("id", docId);
            var update = Builders<Documento>.Update
                .Set("evaluacion.resultado", documento.resultado)
                .Set("evaluacion.evaluaciones", documento.evaluaciones);
            return _documentos.FindOneAndUpdate<Documento>(filter, update);
            //BandejaDocumento bandejaDocumento = new BandejaDocumento();
            //bandejaDocumento.idexpediente = documento.idexpediente;
            //bandejaDocumento.iddocumento = documento.id;

            //UpdateDefinition<Bandeja> updateBandejaD = Builders<Bandeja>.Update.Pull("bandejaentrada", bandejaDocumento);
            //_bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaD);

            //UpdateDefinition<Bandeja> updateBandejaI = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            //_bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaI);
        }
        public Documento generarDocumento(DocumentoGenerarDTO documento)
        {
            Documento doc = new Documento();
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = documento.idexpediente;
            //bandejaDocumento.iddocumento = documento.iddocumento;
            bandejaDocumento.iddocumento = documento.iddocumentoAnterior;

            UpdateDefinition<Bandeja> updateBandejaD = Builders<Bandeja>.Update.Pull("bandejaentrada", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaD);

            bandejaDocumento.iddocumento = documento.iddocumento;

            UpdateDefinition<Bandeja> updateBandejaI = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaI);

            ContenidoVersion contenidoVersion = new ContenidoVersion();

            contenidoVersion.version = 1;
            contenidoVersion.url = DateTime.UtcNow.AddHours(-5).ToString();

            var UpdateFilter = Builders<Documento>.Update
                                                       .Set("contenido.codigo", documento.codigo)
                                                       .Set("contenido.firma", documento.firma)
                                                       .Set("contenido.urlGenerado", documento.urlDeGenerado)
                                                       .Push("historialcontenido", contenidoVersion);

            var UpdateQuery = Builders<Documento>.Filter.Eq("id", documento.iddocumento);

            _documentos.UpdateOne(UpdateQuery, UpdateFilter);

            return doc;
        }

        public Documento modificarEstadoDocumento(DocumentoDTO documento)
        {
            var filter = Builders<Documento>.Filter.Eq("id", documento.id);
            var update = Builders<Documento>.Update
                .Set("estado", documento.estado);

            return _documentos.FindOneAndUpdate<Documento>(filter, update);
        }

        public AperturamientoDisciplinario registrarAperturamientoDisciplinario(AperturamientoDisciplinarioDTO aperturamientoDisciplinarioDTO,
            string urldata, List<string> url2, string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacionde le objeto de AperturamientoDisciplinario y registro en la coleccion documentos
            ContenidoAperturamientoDisciplinario contenidoAD = new ContenidoAperturamientoDisciplinario()
            {
                codigo = "",
                idnotario = aperturamientoDisciplinarioDTO.contenidoDTO.idnotario.id,
                idfiscal = aperturamientoDisciplinarioDTO.contenidoDTO.idfiscal,
                nombredenunciante = aperturamientoDisciplinarioDTO.contenidoDTO.nombredenunciante,
                titulo = aperturamientoDisciplinarioDTO.contenidoDTO.titulo,
                descripcion = aperturamientoDisciplinarioDTO.contenidoDTO.descripcion,
                fechainicioaudiencia = aperturamientoDisciplinarioDTO.contenidoDTO.fechainicioaudiencia,
                fechafinaudiencia = aperturamientoDisciplinarioDTO.contenidoDTO.fechafinaudiencia,
                participantes = aperturamientoDisciplinarioDTO.contenidoDTO.participantes.Select(x => x.nombre).ToList(),
                lugaraudiencia = aperturamientoDisciplinarioDTO.contenidoDTO.lugaraudiencia,
                hechosimputados = aperturamientoDisciplinarioDTO.contenidoDTO.hechosimputados.Select(x => x.descripcion).ToList(),
                url = urldata,
                firma = ""
            };
            AperturamientoDisciplinario aperturamientodisciplinario = new AperturamientoDisciplinario()
            {
                tipo = "AperturamientoDisciplinario",
                contenido = contenidoAD,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                urlanexo= url2,
                estado = "creado"
            };
            _documentos.InsertOne(aperturamientodisciplinario);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = aperturamientodisciplinario.id;
            documentoExpediente.tipo = "AperturamientoDisciplinario";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", iddocentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return aperturamientodisciplinario;
        }

        public Dictamen RegistrarDictamen(DictamenDTO dictamenDTO, ExpedienteWrapper expedientewrapper, List<string> url2)
        {
            //Obtenemos los datos del expedientewrapper
            
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            dictamenDTO = JsonConvert.DeserializeObject<DictamenDTO>(json);

            //creacion del Objeto de tipo Dictamen y el registro en la coleccion Dictamen
            ContenidoDictamen contenidodictamen = new ContenidoDictamen()
            {
                codigo = "",
                titulo = dictamenDTO.contenidoDTO.titulo,
                descripcion = dictamenDTO.contenidoDTO.descripcion,
                nombredenunciante = dictamenDTO.contenidoDTO.nombredenunciante,
                conclusion = dictamenDTO.contenidoDTO.conclusion,
                observaciones = dictamenDTO.contenidoDTO.observaciones.Select(x => x.descripcion).ToList(),
                recomendaciones = dictamenDTO.contenidoDTO.recomendaciones.Select(x => x.descripcion).ToList(),
                firma = ""
                //fecha           
            };
            Dictamen dictamen = new Dictamen()
            {
                tipo = "Dictamen",
                contenido = contenidodictamen,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = "creado",
                urlanexo = url2
            };
            _documentos.InsertOne(dictamen);

            //actualizacion de expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = dictamen.id;
            documentoExpediente.tipo = "Dictamen";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, expedientewrapper.idexpediente);

            //actualizando Bandeja salida
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedientewrapper.idusuarioactual, updateBandeja);*/

            //actualizando Bandeja Entrada
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedientewrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedientewrapper.idusuarioactual, updateBandejaEntrada);*/

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedientewrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return dictamen;
        }

        public Resolucion registrarResolucion(ResolucionDTO resolucionDTO,
            string urldata, List<string> url2, string idusuario, string idexpediente, string iddocentrada, string iddocumentoSolicitud)
        {
            //Creacionde le objeto de AperturamientoDisciplinario y registro en la coleccion documentos
            ContenidoResolucion contenidoResolucion = new ContenidoResolucion()
            {
                codigo = "",
                titulo= resolucionDTO.contenidoDTO.titulo,
                descripcion = resolucionDTO.contenidoDTO.descripcion,
                fechainicioaudiencia = resolucionDTO.contenidoDTO.fechainicioaudiencia,
                fechafinaudiencia = resolucionDTO.contenidoDTO.fechafinaudiencia,
                participantes = resolucionDTO.contenidoDTO.participantes.Select(x => x.nombre).ToList(),
                sancion = resolucionDTO.contenidoDTO.sancion,
                url = urldata,
                firma = ""
            };
            Resolucion resolucion = new Resolucion()
            {
                tipo = "Resolucion",
                contenido = contenidoResolucion,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                urlanexo = url2,
                evaluacion = new Evaluacion()
                {
                    resultado = "pendiente",
                    evaluaciones = new List<EvaluacionIndividual>()
                },estado = "creado"
            };
            _documentos.InsertOne(resolucion);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = resolucion.id;
            documentoExpediente.tipo = "Resolucion";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", iddocentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            //Actualizar el documento de solicitud inicial a finalizado
            if(!String.IsNullOrEmpty(iddocumentoSolicitud))
            {
                var filterS = Builders<Documento>.Filter.Eq("id", iddocumentoSolicitud);
                var updateS = Builders<Documento>.Update
                       .Set("estado", "finalizado");

                _documentos.UpdateOne(filter, update);
            }
            return resolucion;
        }

        public ResultadoBPN registrarResultadoBPN(ResultadoBPNDTO resultadoBPNDTO, List<string> url2,
            string idusuario, string idexpediente, string iddocentrada, string iddocumentoSolicitud)
        {
            //Creacionde le objeto y registro en la coleccion documentos
            ContenidoResultadoBPN contenidoResultadoBPN = new ContenidoResultadoBPN()
            {
                codigo = "",
                cantidadfoja = resultadoBPNDTO.contenidoDTO.cantidadfoja,
                costo = resultadoBPNDTO.contenidoDTO.costo,
                idescriturapublica = resultadoBPNDTO.contenidoDTO.idescriturapublica.id,
                estado = "pendiente",
                firma = ""
            };
            ResultadoBPN resultadoBPN = new ResultadoBPN()
            {
                tipo = "ResultadoBPN",
                contenido = contenidoResultadoBPN,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                urlanexo = url2,
                estado = "creado"
            };
            _documentos.InsertOne(resultadoBPN);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = resultadoBPN.id;
            documentoExpediente.tipo = "ResultadoBPN";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", iddocentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            //Actualizar el documento de solicitud a emitido, obtener el id de la solicitudBPN
            if(!string.IsNullOrEmpty(iddocumentoSolicitud))
            {
                var filterS = Builders<Documento>.Filter.Eq("id", iddocumentoSolicitud);
                var updateS = Builders<Documento>.Update
                    .Set("estado", "emitido");
                _documentos.UpdateOne(filterS, updateS);
            }

            return resultadoBPN;
        }

        public EntregaExpedienteNotario registrarEntregaExpedienteNotario(EntregaExpedienteNotarioDTO entregaExpedienteNotarioDTO, ExpedienteWrapper expedientewrapper, List<string> url2)
        {
            //Obtenemos los datos del expedientewrapper
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            entregaExpedienteNotarioDTO = JsonConvert.DeserializeObject<EntregaExpedienteNotarioDTO>(json);

            //creacion del Objeto
            ContenidoEntregaExpedienteNotario contenidoEntregaExpedienteNotario = new ContenidoEntregaExpedienteNotario()
            {
                titulo = entregaExpedienteNotarioDTO.contenidoDTO.titulo,
                descripcion = entregaExpedienteNotarioDTO.contenidoDTO.descripcion,
                idnotario = entregaExpedienteNotarioDTO.contenidoDTO.idnotario.id
            };

            EntregaExpedienteNotario entregaExpedienteNotario = new EntregaExpedienteNotario()
            {
                tipo = "EntregaExpedienteNotario",
                contenido = contenidoEntregaExpedienteNotario,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = "creado",
                urlanexo = url2
            };

            _documentos.InsertOne(entregaExpedienteNotario);

            //actualizacion de expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 7;
            documentoExpediente.iddocumento = entregaExpedienteNotario.id;
            documentoExpediente.tipo = "EntregaExpedienteNotario";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, expedientewrapper.idexpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedientewrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);

            return entregaExpedienteNotario;
        }

        public Expediente actualizarExpediente(DocumentoExpediente documentoExpediente, string idexpediente)
        {
            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == idexpediente, updateExpediente);
            return expediente;
        }

        public Apelacion registrarApelacion(ApelacionDTO apelacionDTO,
            string urldata, List<string> url2, string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacion de la Apelacion y registro en la coleccion documentos
            ContenidoApelacion contenidoApe = new ContenidoApelacion()
            {
                codigo = "",
                titulo = apelacionDTO.contenidoDTO.titulo,
                descripcion = apelacionDTO.contenidoDTO.descripcion,
                fechaapelacion = DateTime.UtcNow.AddHours(-5),
                url = urldata,
                firma = ""
            };
            Apelacion apelacion = new Apelacion()
            {
                tipo = "Apelacion",
                contenido = contenidoApe,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                urlanexo = url2,
                evaluacion = new Evaluacion()
                {
                    resultado = "pendiente",
                    evaluaciones = new List<EvaluacionIndividual>(),
                },
                estado = "creado"
            };
            _documentos.InsertOne(apelacion);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 9;
            documentoExpediente.iddocumento = apelacion.id;
            documentoExpediente.tipo = "Apelacion";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", iddocentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return apelacion;
        }

        //registrarSolicitudExpedienteNotario
        public SolicitudExpedienteNotario registrarSolicitudExpedienteNotario(SolicitudExpedienteNotarioDTO solicitudExpedienteNotarioDTO, List<string> url2,
            string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacion de la Apelacion y registro en la coleccion documentos
            ContenidoSolicitudExpedienteNotario contenidoSEN = new ContenidoSolicitudExpedienteNotario()
            {
                codigo = "",
                titulo = solicitudExpedienteNotarioDTO.contenidoDTO.titulo,
                descripcion = solicitudExpedienteNotarioDTO.contenidoDTO.descripcion,
                idnotario = solicitudExpedienteNotarioDTO.contenidoDTO.idnotario.id,
                fechaemision = DateTime.UtcNow.AddHours(-5),
                firma = ""
            };
            SolicitudExpedienteNotario solicitudExpedienteNotarioAct = new SolicitudExpedienteNotario()
            {
                tipo = "SolicitudExpedienteNotario",
                contenido = contenidoSEN,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                urlanexo = url2,
                estado = "pendiente"
            };
            _documentos.InsertOne(solicitudExpedienteNotarioAct);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 10;
            documentoExpediente.iddocumento = solicitudExpedienteNotarioAct.id;
            documentoExpediente.tipo = "SolicitudExpedienteNotario";
            documentoExpediente.fechacreacion = DateTime.UtcNow.AddHours(-5);
            documentoExpediente.fechaexceso = DateTime.UtcNow.AddHours(-5).AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", iddocentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return solicitudExpedienteNotarioAct;
        }

        public OficioDesignacionNotarioDTO obtenerOficioDesignacionNotario(string id)
        {
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(id)));

            BsonArray subpipeline = new BsonArray();
            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idnot") }
                        )
                    ))
                );

            var aggregation = new BsonDocument("$lookup",
                                new BsonDocument("from", "notarios")
                                .Add("let", new BsonDocument("idnot", "$contenido.idnotario"))
                                .Add("pipeline", subpipeline)
                                .Add("as", "notario"));


            OficioDesignacionNotario_ur documentoODN = new OficioDesignacionNotario_ur();
            documentoODN = _documentos.Aggregate()
                .AppendStage<OficioDesignacionNotario>(match)
                .AppendStage<OficioDesignacionNotario_lookup>(aggregation)
                .Unwind<OficioDesignacionNotario_lookup, OficioDesignacionNotario_ur>(x => x.notario)
                .First();

            OficioDesignacionNotarioDTO oficioDesignacionNotario = new OficioDesignacionNotarioDTO();
            oficioDesignacionNotario.id = documentoODN.id;
            oficioDesignacionNotario.tipo = documentoODN.tipo;
            oficioDesignacionNotario.contenidoDTO = new ContenidoOficioDesignacionNotarioDTO()
            {
                titulo = documentoODN.contenido.titulo,
                descripcion = documentoODN.contenido.descripcion,
                fecharealizacion = documentoODN.contenido.fecharealizacion,
                lugaroficionotarial = documentoODN.contenido.lugaroficionotarial,
                idusuario = documentoODN.contenido.idusuario,
                idnotario = documentoODN.notario
            };
            oficioDesignacionNotario.historialcontenido = documentoODN.historialcontenido;
            oficioDesignacionNotario.historialproceso = documentoODN.historialproceso;
            oficioDesignacionNotario.evaluacion = documentoODN.evaluacion;
            return oficioDesignacionNotario;

        }

        public List<Documento> obtenerSolicitudes()
        {
            IEnumerable<string> tipos = new[] { "SolicitudExpedicionFirma", "SolicitudDenuncia", "SolicitudBPN" };
            var filter = Builders<Documento>.Filter.In(x => x.tipo , tipos);

            List<Documento> values = _documentos.Find(filter).ToList();
            return values;
        }
        

        public SolicitudExpedicionFirmaDTO obtenerSolicitudExpedicionFirmas(string id)
        {
            SolicitudExpedicionFirma docSolicitudExpedicionFirma = new SolicitudExpedicionFirma();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docSolicitudExpedicionFirma = _documentos.Aggregate().
              AppendStage<SolicitudExpedicionFirma>(match).First();

            SolicitudExpedicionFirmaDTO solicitudEFDTO = new SolicitudExpedicionFirmaDTO();
            solicitudEFDTO.id = docSolicitudExpedicionFirma.id;
            solicitudEFDTO.tipo = docSolicitudExpedicionFirma.tipo;
            //solicitudbpnDTO.historialcontenido = docSolicitudBPN.historialcontenido;
            //solicitudbpnDTO.historialproceso = docSolicitudBPN.historialproceso;
            solicitudEFDTO.estado = docSolicitudExpedicionFirma.estado;
            solicitudEFDTO.contenidoDTO = new ContenidoSolicitudExpedicionFirmaDTO()
            {

                titulo = docSolicitudExpedicionFirma.contenido.titulo,
                descripcion = docSolicitudExpedicionFirma.contenido.descripcion,
                fecharealizacion = docSolicitudExpedicionFirma.contenido.fecharealizacion,
                


            };
            return solicitudEFDTO;
        }
        public SolicitudDenunciaDTO obtenerSolicitudDenuncias(string id)
        {
            SolicitudDenuncia docSolicitudDenuncia = new SolicitudDenuncia();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docSolicitudDenuncia = _documentos.Aggregate().
              AppendStage<SolicitudDenuncia>(match).First();

            SolicitudDenunciaDTO solicitudDenuncia = new SolicitudDenunciaDTO();
            solicitudDenuncia.id = docSolicitudDenuncia.id;
            solicitudDenuncia.tipo = docSolicitudDenuncia.tipo;
            //solicitudbpnDTO.historialcontenido = docSolicitudBPN.historialcontenido;
            //solicitudbpnDTO.historialproceso = docSolicitudBPN.historialproceso;
            solicitudDenuncia.estado = docSolicitudDenuncia.estado;
            solicitudDenuncia.contenidoDTO = new ContenidoSolicitudDenunciaDTO()
            {
                
                titulo = docSolicitudDenuncia.contenido.titulo,
                descripcion = docSolicitudDenuncia.contenido.descripcion,
                fechaentrega = docSolicitudDenuncia.contenido.fechaentrega,
                


            };
            return solicitudDenuncia;
        }
        public SolicitudBPNDTO obtenerSolicitudBusquedaProtocoloNotarial(string id)
        {
            SolicitudBPN docSolicitudBPN = new SolicitudBPN();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docSolicitudBPN = _documentos.Aggregate().
              AppendStage<SolicitudBPN>(match).First();

            SolicitudBPNDTO solicitudbpnDTO = new SolicitudBPNDTO();
            solicitudbpnDTO.id = docSolicitudBPN.id;
            solicitudbpnDTO.tipo = docSolicitudBPN.tipo;
            //solicitudbpnDTO.historialcontenido = docSolicitudBPN.historialcontenido;
            //solicitudbpnDTO.historialproceso = docSolicitudBPN.historialproceso;
            solicitudbpnDTO.estado = docSolicitudBPN.estado;
            solicitudbpnDTO.contenidoDTO = new ContenidoSolicitudBPNDTO()
            {
                
                actojuridico = docSolicitudBPN.contenido.actojuridico,
                tipoprotocolo = docSolicitudBPN.contenido.tipoprotocolo,

                fecharealizacion = docSolicitudBPN.contenido.fecharealizacion,
                //Poner lista de objetos
                // otorgantes = docSolicitudBPN.contenido.otorgantes.Select((x, y) => new Otorgantelista() { nombre = x, index = y }).ToList(),


            };
            return solicitudbpnDTO;
        }



        public OficioBPNDTO  obtenerOficioBusquedaProtocoloNotarial(string id)
        {
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            OficioBPNDTO oficioBPN = new OficioBPNDTO();
            BsonArray subpipeline = new BsonArray();
            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idnot") }
                        )
                    ))
                );
            var aggregation = new BsonDocument("$lookup",
                               new BsonDocument("from", "notarios")
                               .Add("let", new BsonDocument("idnot", "$contenido.idnotario"))
                               .Add("pipeline", subpipeline)
                               .Add("as", "notario"));

            BsonArray subpipeline2 = new BsonArray();
            subpipeline2.Add(
              new BsonDocument("$match", new BsonDocument(
                  "$expr", new BsonDocument(
                      "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idcli") }
                      )
                  ))
              );
            var aggregation2 = new BsonDocument("$lookup",
                               new BsonDocument("from", "usuarios")
                               .Add("let", new BsonDocument("idcli", "$contenido.idcliente"))
                               .Add("pipeline", subpipeline2)
                               .Add("as", "cliente"));

            var project = new BsonDocument("$project",
                    new BsonDocument {
                        { "_id","$_id"},
                        { "tipo","$tipo"},
                        { "estado","$estado"},
                        { "historialcontenido","$historialcontenido"},
                        { "historialproceso","$historialproceso"},
                        { "contenidoDTO", new BsonDocument{
                            { "titulo","$contenido.titulo"},
                            { "descripcion","$contenido.descripcion"},
                            { "idcliente",new BsonDocument("$arrayElemAt", new BsonArray{ "$cliente",0})},
                            { "idnotario","$notario"},
                            { "actojuridico","$contenido.actojuridico"},
                            { "tipoprotocolo","$contenido.tipoprotocolo"},
                            { "otorgantes","$contenido.otorgantes"},
                            { "fecharealizacion","$contenido.fecharealizacion"},
                            { "url","$url"}
                        }},
                    });


            oficioBPN = _documentos.Aggregate().
                AppendStage<OficioBPN>(match)
                .AppendStage<OficioBPNDTO_lookup>(aggregation)
                .Unwind<OficioBPNDTO_lookup, OficioBPNDTO_ur>(x => x.notario)
                .AppendStage<OficioBPNDTO_lookup2>(aggregation2)
                .AppendStage<OficioBPNDTO>(project).First();

            /*OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            oficioBPNDTO.id = oficioBPN.id;
            oficioBPNDTO.tipo = oficioBPN.tipo;
            oficioBPNDTO.historialcontenido = oficioBPN.historialcontenido;
            oficioBPNDTO.historialproceso = oficioBPN.historialproceso;
            oficioBPNDTO.contenidoDTO = new ContenidoOficioBPNDTO()
            {
                titulo = oficioBPN.contenido.titulo,
                descripcion = oficioBPN.contenido.descripcion,
                idcliente = new Usuario() { id = oficioBPN.contenido.idcliente },
                direccionoficio = oficioBPN.contenido.direccionoficio,
                idnotario = oficioBPN.notario,
                actojuridico = oficioBPN.contenido.actojuridico,
                tipoprotocolo = oficioBPN.contenido.tipoprotocolo,
                otorgantes = oficioBPN.contenido.otorgantes,
                fecharealizacion = oficioBPN.contenido.fecharealizacion,
                url = oficioBPN.contenido.url
            };
            oficioBPNDTO.estado = oficioBPN.estado;*/
            return oficioBPN;
        }

        public DictamenDTO obtenerDictamenDTO(string id)
        {
            Dictamen docDictamen = new Dictamen();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docDictamen = _documentos.Aggregate().
              AppendStage<Dictamen>(match).First();

            DictamenDTO dictamenDto = new DictamenDTO();
            dictamenDto.id = docDictamen.id;
            dictamenDto.historialcontenido = docDictamen.historialcontenido;
            dictamenDto.historialcontenido = docDictamen.historialcontenido;
            dictamenDto.estado = docDictamen.estado;
            dictamenDto.tipo = docDictamen.tipo;
            dictamenDto.contenidoDTO = new ContenidoDictamenDTO()
            {
                Urlanexo = docDictamen.urlanexo.ToList(),
                descripcion = docDictamen.contenido.descripcion,
                nombredenunciante = docDictamen.contenido.nombredenunciante,
                titulo = docDictamen.contenido.titulo,
                observaciones = docDictamen.contenido.observaciones.Select((x, index) => new Observaciones() { descripcion = x, index = index }).ToList(),
                conclusion = docDictamen.contenido.conclusion,
                recomendaciones = docDictamen.contenido.recomendaciones.Select((x, index) => new Recomendaciones() { descripcion = x, index = index }).ToList()

            };
            return dictamenDto;
        }

        public ResolucionDTO obtenerResolucionDTO(string id)
        {
            Resolucion docResolucion = new Resolucion();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docResolucion = _documentos.Aggregate().
              AppendStage<Resolucion>(match).First();

            ResolucionDTO resolucionDTO = new ResolucionDTO();
            resolucionDTO.id = docResolucion.id;
            resolucionDTO.tipo = docResolucion.tipo;
            resolucionDTO.historialcontenido = docResolucion.historialcontenido;
            resolucionDTO.historialproceso = docResolucion.historialproceso;
            resolucionDTO.estado = docResolucion.estado;
            resolucionDTO.contenidoDTO = new ContenidoResolucionDTO()
            {
                Urlanexo = docResolucion.urlanexo.ToList(),
                descripcion = docResolucion.contenido.descripcion,
                titulo = docResolucion.contenido.titulo,
                fechainicioaudiencia = docResolucion.contenido.fechainicioaudiencia,
                fechafinaudiencia = docResolucion.contenido.fechafinaudiencia,
                participantes = docResolucion.contenido.participantes.Select((x, y) => new Participante() { nombre = x, index = y }).ToList(),
                sancion = docResolucion.contenido.sancion,
                data = docResolucion.contenido.url
            };
            return resolucionDTO;
        }

        public ResultadoBPNDTO obtenerResultadoBPNDTO(string id)
        {
            BsonArray pipeline = new BsonArray();
            pipeline.Add(new BsonDocument("$match",
                new BsonDocument("$expr",
                    new BsonDocument("$eq",
                        new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idescritura") }))));
            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "escrituraspublicas")
                .Add("let", new BsonDocument("idescritura", "$contenido.idescriturapublica"))
                .Add("pipeline", pipeline)
                .Add("as", "escriturapublica"));

            var project = new BsonDocument("$project",
                new BsonDocument {
                    { "_id","$_id" },
                    { "tipo","$tipo"},
                    { "contenidoDTO",new BsonDocument{
                        {
                            "idescriturapublica",
                            new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$escriturapublica",0 })
                        },
                        {"cantidadfoja","$contenido.cantidadfoja" },
                        {"costo","$contenido.costo" },
                        {"estado","$estado"}
                    }
                    },
                    { "estado","$estado"},
                    { "historialcontenido", "$historialcontenido" },
                    { "historialproceso", "$historialproceso" }
                });

            ResultadoBPNDTO docResultadoBPN = new ResultadoBPNDTO();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docResultadoBPN = _documentos.Aggregate().
              AppendStage<ResultadoBPN>(match)
              .AppendStage<ResultadoBPN_lookup>(lookup)
              .AppendStage<ResultadoBPNDTO>(project).First();


            return docResultadoBPN;
        }

        public ApelacionDTO ObtenerDocumentoApelacion(string id)
        {
            Apelacion docApelacion = new Apelacion();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docApelacion = _documentos.Aggregate().
              AppendStage<Apelacion>(match).First();

            ApelacionDTO apelacionDTO = new ApelacionDTO();
            apelacionDTO.id = docApelacion.id;
            apelacionDTO.tipo = docApelacion.tipo;
            apelacionDTO.historialcontenido = docApelacion.historialcontenido;
            apelacionDTO.historialproceso = docApelacion.historialproceso;
            apelacionDTO.estado = docApelacion.estado;
            apelacionDTO.contenidoDTO = new ContenidoApelacionDTO()
            {
                Urlanexo = docApelacion.urlanexo.ToList(),
                titulo = docApelacion.contenido.titulo,
                descripcion = docApelacion.contenido.descripcion,
                fechaapelacion = docApelacion.contenido.fechaapelacion,
                data = docApelacion.contenido.url,
            };
            return apelacionDTO;

        }
        public ConclusionFirmaDTO ObtenerDocumentoConclusionFirma(string id)
        {
            BsonArray pipeline = new BsonArray();
            pipeline.Add(new BsonDocument("$match",
                new BsonDocument("$expr",
                    new BsonDocument("$eq",
                        new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idescritura") }))));
            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "escrituraspublicas")
                .Add("let", new BsonDocument("idescritura", "$contenido.idescriturapublica"))
                .Add("pipeline", pipeline)
                .Add("as", "escriturapublica"));



            BsonArray pipeline2 = new BsonArray();
            pipeline2.Add(new BsonDocument("$match",
               new BsonDocument("$expr",
                   new BsonDocument("$eq",
                       new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idnotario") }))));
            var lookup2 = new BsonDocument("$lookup",
                new BsonDocument("from", "notarios")
                .Add("let", new BsonDocument("idnotario", "$contenido.idnotario"))
                .Add("pipeline", pipeline2)
                .Add("as", "notario"));


            BsonArray pipeline3 = new BsonArray();
            pipeline3.Add(new BsonDocument("$match",
               new BsonDocument("$expr",
                   new BsonDocument("$eq",
                       new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idcliente") }))));
            var lookup3 = new BsonDocument("$lookup",
                new BsonDocument("from", "usuarios")
                .Add("let", new BsonDocument("idcliente", "$contenido.idcliente"))
                .Add("pipeline", pipeline3)
                .Add("as", "cliente"));


            var project = new BsonDocument("$project",
                new BsonDocument {
                    { "_id","$_id" },
                    { "tipo","$tipo"},
                    { "contenidoDTO",new BsonDocument{
                        {
                            "idescriturapublica",
                            new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$escriturapublica",0 })
                        },
                        { "idnotario",new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$notario", 0 })},
                        { "idcliente",new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$cliente", 0 })},
                        {"cantidadfoja","$contenido.cantidadfoja" },
                        {"precio","$contenido.precio" }
                    }
                    },
                    { "estado","$estado"},
                    { "historialcontenido", "$historialcontenido" },
                    { "historialproceso", "$historialproceso" }
                });

            ConclusionFirmaDTO docConclusionFirma = new ConclusionFirmaDTO();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docConclusionFirma = _documentos.Aggregate().
              AppendStage<ConclusionFirma>(match)
              .AppendStage<ConclusionFirma_lookup>(lookup)
              .AppendStage<ConclusionFirma_lookup2>(lookup2)
              .AppendStage<ConclusionFirma_lookup3>(lookup3)
              .AppendStage<ConclusionFirmaDTO>(project).First();


            return docConclusionFirma;
        }

        public AperturamientoDisciplinarioDTO obtenerDocumentoAperturamientoDisciplinario(string id)
        {

            var match = new BsonDocument("$match",
                new BsonDocument("_id", new ObjectId(id)));
            BsonArray pipeline = new BsonArray {
                new BsonDocument("$match",
                    new BsonDocument("$expr",
                        new BsonDocument("$eq",new BsonArray{"$_id", new BsonDocument("$toObjectId", "$$idnot") })))
            };


            var lookup = new BsonDocument("$lookup", new BsonDocument(
                            "from", "notarios")
                .Add("let", new BsonDocument("idnot", "$contenido.idnotario"))
                .Add("pipeline", pipeline)
                .Add("as", "notario"));


            var project = new BsonDocument("$project",
               new BsonDocument {{ "_id", "$_id" },
                        { "estado", "$estado" },
                        { "tipo", "$tipo" },
                        { "contenido",  new BsonDocument{
                          { "idnotario", new BsonDocument("$arrayElemAt",
                                            new BsonArray
                                {
                                    "$notario",
                                    0
                                }) },
                            { "idfiscal", "$contenido.idfiscal" },
                            { "nombredenunciante", "$contenido.nombredenunciante" },
                            { "titulo", "$contenido.titulo" },
                            { "descripcion", "$contenido.descripcion" },
                            { "fechainicioaudiencia", "$contenido.fechainicioaudiencia" },
                            { "fechafinaudiencia", "$contenido.fechafinaudiencia" },
                            { "participantes", "$contenido.participantes" },
                            { "lugaraudiencia", "$contenido.lugaraudiencia" },
                            { "hechosimputados", "$contenido.hechosimputados" },
                            { "url", "$contenido.url" }
            } },
            { "historialproceso", "$historialproceso" },
            { "historialcontenido", "$historialcontenido" }
       });



            AperturamientoD_project documentoAperturamiento = new AperturamientoD_project();
            documentoAperturamiento = _documentos.Aggregate()
                .AppendStage<AperturamientoDisciplinario>(match)
                .AppendStage<AperturamientoD_lookup_notario>(lookup)
                .AppendStage<AperturamientoD_project>(project).First();

            AperturamientoDisciplinarioDTO aperturamientodto = new AperturamientoDisciplinarioDTO();
            aperturamientodto.id = documentoAperturamiento.id;
            aperturamientodto.tipo = documentoAperturamiento.tipo;
            aperturamientodto.historialcontenido = documentoAperturamiento.historialcontenido;
            aperturamientodto.historialproceso = documentoAperturamiento.historialproceso;
            aperturamientodto.contenidoDTO = new ContenidoAperturamientoDisciplinarioDTO()
            {
                idnotario = documentoAperturamiento.contenido.idnotario,
                idfiscal = documentoAperturamiento.contenido.idfiscal,
                nombredenunciante = documentoAperturamiento.contenido.nombredenunciante,
                titulo = documentoAperturamiento.contenido.titulo,
                descripcion = documentoAperturamiento.contenido.descripcion,
                fechainicioaudiencia = documentoAperturamiento.contenido.fechainicioaudiencia,
                fechafinaudiencia = documentoAperturamiento.contenido.fechafinaudiencia,
                participantes = documentoAperturamiento.contenido.participantes.Select((x, y) => new Participante() { nombre = x, index = y }).ToList(),
                lugaraudiencia = documentoAperturamiento.contenido.lugaraudiencia,
                hechosimputados = documentoAperturamiento.contenido.hechosimputados.Select((x, y) => new Hecho() { descripcion = x, index = y }).ToList(),
                url = documentoAperturamiento.contenido.url
            };
            return aperturamientodto;
        }

        public SolicitudExpedienteNotarioDTO obtenerSolicitudExpedienteNotario(string iddoc)
        {
            var match = new BsonDocument("$match",
                new BsonDocument("_id", new ObjectId(iddoc)));

            var pipeline = new BsonArray {
                new BsonDocument("$match",
                    new BsonDocument("$expr",
                        new BsonDocument("$eq", new BsonArray{ "$_id",
                            new BsonDocument("$toObjectId", "$$idnot") })))
            };

            var lookup = new BsonDocument("$lookup",
                new BsonDocument {
                    { "from","notarios"},
                    { "let",new BsonDocument("idnot","$contenido.idnotario")},
                    { "pipeline",pipeline},
                    { "as","notario"}
                });

            var project = new BsonDocument("$project",
                new BsonDocument {
                    { "_id", "$_id" },
                    { "estado", "$estado" },
                    { "tipo", "$tipo" },
                    { "contenidoDTO",new BsonDocument{
                        { "titulo","$contenido.titulo"},
                        { "descripcion","$contenido.descripcion"},
                        { "fechaemision","$contenido.fechaemision"},
                        { "idnotario", new BsonDocument("$arrayElemAt",
                                            new BsonArray{ "$notario", 0 })}
                    }},
                    { "historialproceso", "$historialproceso" },
                    { "historialcontenido", "$historialcontenido" }
                });

            SolicitudExpedienteNotarioDTO solicitudExpNotario = new SolicitudExpedienteNotarioDTO();
            solicitudExpNotario = _documentos.Aggregate()
                .AppendStage<SolicitudExpedienteNotario>(match)
                .AppendStage<SolicitudExpedienteNotario_lookup>(lookup)
                .AppendStage<SolicitudExpedienteNotarioDTO>(project).First();

            return solicitudExpNotario;
        }

        public DocumentoDTO obtenerDocumentoDTO(string id)
        {
            DocumentoDTO docDocumento = new DocumentoDTO();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            docDocumento = _documentos.Aggregate().
              AppendStage<DocumentoDTO>(match).First();

            //DocumentoDTO documentoDto = new DocumentoDTO
            //{
            //    id = docDocumento.id,
            //    historialcontenido = docDocumento.historialcontenido,
            //    historialproceso = docDocumento.historialproceso,
            //    tipo = docDocumento.tipo,
            //    urlanexo = docDocumento.urlanexo
            //};
            return docDocumento;
        }


        public SolicitudInicialDTO obtenerSolicitudInicial(string id)
        {
            SolicitudInicial doc = new SolicitudInicial();
            var match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            doc = _documentos.Aggregate().
              AppendStage<SolicitudInicial>(match).First();

            SolicitudInicialDTO SIDTO = new SolicitudInicialDTO();
            SIDTO.id = doc.id;
            SIDTO.tipo = doc.tipo;
            SIDTO.historialcontenido = doc.historialcontenido;
            SIDTO.historialproceso = doc.historialproceso;
            SIDTO.estado = doc.estado;
            SIDTO.contenidoDTO = new ContenidoSolicitudInicialDTO()
            {
                titulo = doc.contenido.titulo,
                descripcion = doc.contenido.descripcion,
                fechacreacion = doc.fechacreacion
            };
            return SIDTO;
        }
        //
        public EntregaExpedienteNotarioDTO obtenerEntregaExpedienteNotarioDTO(string id)
        {
            var match = new BsonDocument("$match",
                new BsonDocument("_id", new ObjectId(id)));

            var pipeline = new BsonArray {
                new BsonDocument("$match",
                    new BsonDocument("$expr",
                        new BsonDocument("$eq", new BsonArray{ "$_id",
                            new BsonDocument("$toObjectId", "$$idnot") })))
            };

            var lookup = new BsonDocument("$lookup",
                new BsonDocument {
                    { "from","notarios"},
                    { "let",new BsonDocument("idnot","$contenido.idnotario")},
                    { "pipeline",pipeline},
                    { "as","notario"}
                });

            var project = new BsonDocument("$project",
                new BsonDocument {
                    { "_id", "$_id" },
                    { "estado", "$estado" },
                    { "tipo", "$tipo" },
                    { "contenidoDTO",new BsonDocument{
                        { "titulo","$contenido.titulo"},
                        { "descripcion","$contenido.descripcion"},
                        { "idnotario", new BsonDocument("$arrayElemAt",
                                            new BsonArray{ "$notario", 0 })}
                    }},
                    { "historialproceso", "$historialproceso" },
                    { "historialcontenido", "$historialcontenido" },
                    { "urlanexo", "$urlanexo" }
                });

            EntregaExpedienteNotarioDTO entregaExpedienteNotario = new EntregaExpedienteNotarioDTO();
            entregaExpedienteNotario = _documentos.Aggregate()
                .AppendStage<EntregaExpedienteNotario>(match)
                .AppendStage<EntregaExpedienteNotario_lookup>(lookup)
                .AppendStage<EntregaExpedienteNotarioDTO>(project).First();

            return entregaExpedienteNotario;
        }
        //

        public OficioDesignacionNotario actualizarDocumentoODN(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo OficioDesignacionNotarioDTO
            OficioDesignacionNotarioDTO oficioDesignacionNotarioDTO = new OficioDesignacionNotarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioDesignacionNotarioDTO = JsonConvert.DeserializeObject<OficioDesignacionNotarioDTO>(json);

            //Creacion de Obj OficioDesignacionNotario y registro en coleccion de documentos 
            ContenidoOficioDesignacionNotario contenidoODN = new ContenidoOficioDesignacionNotario()
            {
                titulo = oficioDesignacionNotarioDTO.contenidoDTO.titulo,
                descripcion = oficioDesignacionNotarioDTO.contenidoDTO.descripcion,
                lugaroficionotarial = oficioDesignacionNotarioDTO.contenidoDTO.lugaroficionotarial,
                idusuario = oficioDesignacionNotarioDTO.contenidoDTO.idusuario,
                idnotario = oficioDesignacionNotarioDTO.contenidoDTO.idnotario.id,
            };
            OficioDesignacionNotario oficioDesignacionNotario = new OficioDesignacionNotario();
            var filter = Builders<Documento>.Filter.Eq("id", oficioDesignacionNotarioDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoODN.titulo)
                .Set("contenido.descripcion", contenidoODN.descripcion)
                .Set("contenido.lugaroficionotarial", contenidoODN.lugaroficionotarial)
                .Set("contenido.idusuario", contenidoODN.idusuario)
                .Set("contenido.idnotario", contenidoODN.idnotario)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return oficioDesignacionNotario;
        }

        public Apelacion actualizarDocumentoApelacion(ExpedienteWrapper expedienteWrapper, string urlData, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            ApelacionDTO apelacionDTO = new ApelacionDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            apelacionDTO = JsonConvert.DeserializeObject<ApelacionDTO>(json);

            //Creacion de Obj Apelacion y registro en coleccion de documentos 
            ContenidoApelacion contenidoAPE = new ContenidoApelacion()
            {
                titulo = apelacionDTO.contenidoDTO.titulo,
                descripcion = apelacionDTO.contenidoDTO.descripcion,
                url = urlData,
            };
            Apelacion apelacion = new Apelacion();

            var filter = Builders<Documento>.Filter.Eq("id", apelacionDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoAPE.titulo)
                .Set("contenido.descripcion", contenidoAPE.descripcion)
                .Set("contenido.url", contenidoAPE.url)
                .Set("urlanexo", url2);

            _documentos.UpdateOne(filter, update);
            return apelacion;
        }

        public AperturamientoDisciplinario actualizarDocumentoAperturamientoDisciplinario(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            AperturamientoDisciplinarioDTO aperturamientoDisciplinarioDTO = new AperturamientoDisciplinarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            aperturamientoDisciplinarioDTO = JsonConvert.DeserializeObject<AperturamientoDisciplinarioDTO>(json);

            //Listas de participantes a string
            List<String> listaParticipantes = new List<string>();
            foreach (Participante part in aperturamientoDisciplinarioDTO.contenidoDTO.participantes)
            {
                listaParticipantes.Add(part.nombre);
            }

            //Listas de hechos a string
            List<String> listaHechosImputados = new List<string>();
            foreach (Hecho part in aperturamientoDisciplinarioDTO.contenidoDTO.hechosimputados)
            {
                listaHechosImputados.Add(part.descripcion);
            }

            //Creacion de Obj y registro en coleccion de documentos
            ContenidoAperturamientoDisciplinario contenidoAD = new ContenidoAperturamientoDisciplinario()
            {
                titulo = aperturamientoDisciplinarioDTO.contenidoDTO.titulo,
                descripcion = aperturamientoDisciplinarioDTO.contenidoDTO.descripcion,
                nombredenunciante = aperturamientoDisciplinarioDTO.contenidoDTO.nombredenunciante,
                lugaraudiencia = aperturamientoDisciplinarioDTO.contenidoDTO.lugaraudiencia,
                idnotario = aperturamientoDisciplinarioDTO.contenidoDTO.idnotario.id,
                idfiscal = aperturamientoDisciplinarioDTO.contenidoDTO.idfiscal,
                participantes = listaParticipantes,
                hechosimputados = listaHechosImputados,
                fechainicioaudiencia = aperturamientoDisciplinarioDTO.contenidoDTO.fechainicioaudiencia,
                fechafinaudiencia = aperturamientoDisciplinarioDTO.contenidoDTO.fechafinaudiencia
            };
            AperturamientoDisciplinario aperturamientoDisciplinario = new AperturamientoDisciplinario();
            var filter = Builders<Documento>.Filter.Eq("id", aperturamientoDisciplinarioDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoAD.titulo)
                .Set("contenido.descripcion", contenidoAD.descripcion)
                .Set("contenido.nombredenunciante", contenidoAD.nombredenunciante)
                .Set("contenido.lugaraudiencia", contenidoAD.lugaraudiencia)
                .Set("contenido.idnotario", contenidoAD.idnotario)
                .Set("contenido.idfiscal", contenidoAD.idfiscal)
                .Set("contenido.participantes", contenidoAD.participantes)
                .Set("contenido.hechosimputados", contenidoAD.hechosimputados)
                .Set("contenido.fechainicioaudiencia", contenidoAD.fechainicioaudiencia)
                .Set("contenido.fechafinaudiencia", contenidoAD.fechafinaudiencia)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return aperturamientoDisciplinario;
        }
        
        public ConclusionFirma actualizarDocumentoConclusionFirma(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            ConclusionFirmaDTO conclusionFirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            conclusionFirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);
            ConclusionFirma documentocf = new ConclusionFirma();

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoConclusionFirma contenidoCF = new ContenidoConclusionFirma()
            {
                idescriturapublica = conclusionFirmaDTO.contenidoDTO.idescriturapublica.id,
                 idnotario = conclusionFirmaDTO.contenidoDTO.idnotario.id,
                  idcliente = conclusionFirmaDTO.contenidoDTO.idcliente.id,
                  cantidadfoja = conclusionFirmaDTO.contenidoDTO.cantidadfoja,
                  precio = conclusionFirmaDTO.contenidoDTO.cantidadfoja*30
            };
            ConclusionFirma conclusionFirma = new ConclusionFirma();
            var filter = Builders<Documento>.Filter.Eq("id", conclusionFirmaDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.idescriturapublica", contenidoCF.idescriturapublica)
                .Set("contenido.idnotario", contenidoCF.idnotario)
                .Set("contenido.idcliente", contenidoCF.idcliente)
                .Set("contenido.cantidadfoja",contenidoCF.cantidadfoja)
                .Set("contenido.precio", contenidoCF.precio)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return conclusionFirma;
        }


        public Dictamen actualizarDocumentoDictamen(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            DictamenDTO dictamenDTO = new DictamenDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            dictamenDTO = JsonConvert.DeserializeObject<DictamenDTO>(json);

            //Listas de participantes a string
            List<String> listaObservaciones = new List<string>();
            foreach (Observaciones obs in dictamenDTO.contenidoDTO.observaciones)
            {
                listaObservaciones.Add(obs.descripcion);
            }

            //Listas de hechos a string
            List<String> listaRecomendaciones = new List<string>();
            foreach (Recomendaciones rec in dictamenDTO.contenidoDTO.recomendaciones)
            {
                listaRecomendaciones.Add(rec.descripcion);
            }

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoDictamen contenidoD = new ContenidoDictamen()
            {
                nombredenunciante = dictamenDTO.contenidoDTO.nombredenunciante,
                titulo = dictamenDTO.contenidoDTO.titulo,
                descripcion = dictamenDTO.contenidoDTO.descripcion,
                conclusion = dictamenDTO.contenidoDTO.conclusion,
                observaciones = listaObservaciones,
                recomendaciones = listaRecomendaciones
            };
            Dictamen dictamen = new Dictamen();

            var filter = Builders<Documento>.Filter.Eq("id", dictamenDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoD.titulo)
                .Set("contenido.descripcion", contenidoD.descripcion)
                .Set("contenido.nombredenunciante", contenidoD.nombredenunciante)
                .Set("contenido.conclusion", contenidoD.conclusion)
                .Set("contenido.observaciones", contenidoD.observaciones)
                .Set("contenido.recomendaciones", contenidoD.recomendaciones)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return dictamen;

        }

        public OficioBPN actualizarDocumentoOficioBPN(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioBPNDTO = JsonConvert.DeserializeObject<OficioBPNDTO>(json);

            List<String> listaOtorgantes = new List<string>();
            foreach (OtorganteDTO obs in oficioBPNDTO.contenidoDTO.otorgantes)
            {
                listaOtorgantes.Add(obs.nombre);
            }

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoOficioBPN contenidoOficioBPN = new ContenidoOficioBPN()
            {
                titulo = oficioBPNDTO.contenidoDTO.titulo,
                descripcion = oficioBPNDTO.contenidoDTO.descripcion,
                idcliente = oficioBPNDTO.contenidoDTO.idcliente.id,
                direccionoficio = oficioBPNDTO.contenidoDTO.direccionoficio,
                idnotario = oficioBPNDTO.contenidoDTO.idnotario.id,
                actojuridico = oficioBPNDTO.contenidoDTO.actojuridico,
                tipoprotocolo = oficioBPNDTO.contenidoDTO.tipoprotocolo,
                otorgantes = listaOtorgantes
            };
            OficioBPN oficioBPN = new OficioBPN();
            var filter = Builders<Documento>.Filter.Eq("id", oficioBPNDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoOficioBPN.titulo)
                .Set("contenido.descripcion", contenidoOficioBPN.descripcion)
                .Set("contenido.idcliente", contenidoOficioBPN.idcliente)
                .Set("contenido.direccionoficio", contenidoOficioBPN.direccionoficio)
                .Set("contenido.idnotario", contenidoOficioBPN.idnotario)
                .Set("contenido.actojuridico", contenidoOficioBPN.actojuridico)
                .Set("contenido.tipoprotocolo", contenidoOficioBPN.tipoprotocolo)
                .Set("contenido.otorgantes", contenidoOficioBPN.otorgantes)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return oficioBPN;
        }

        public Resolucion actualizarDocumentoResolucion(ExpedienteWrapper expedienteWrapper, string urlData, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            ResolucionDTO resolucionDTO = new ResolucionDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            resolucionDTO = JsonConvert.DeserializeObject<ResolucionDTO>(json);

            //Listas de participantes a string
            List<String> listaParticipantes = new List<string>();
            foreach (Participante part in resolucionDTO.contenidoDTO.participantes)
            {
                listaParticipantes.Add(part.nombre);
            }

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoResolucion contenidoResolucion = new ContenidoResolucion()
            {
                titulo = resolucionDTO.contenidoDTO.titulo,
                descripcion = resolucionDTO.contenidoDTO.descripcion,
                sancion = resolucionDTO.contenidoDTO.sancion,
                fechainicioaudiencia = resolucionDTO.contenidoDTO.fechainicioaudiencia,
                fechafinaudiencia = resolucionDTO.contenidoDTO.fechafinaudiencia,
                url = urlData,
                participantes = listaParticipantes
                
            };
            Resolucion resolucion = new Resolucion();
            var filter = Builders<Documento>.Filter.Eq("id", resolucionDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoResolucion.titulo)
                .Set("contenido.descripcion", contenidoResolucion.descripcion)
                .Set("contenido.sancion", contenidoResolucion.sancion)
                .Set("contenido.fechainicioaudiencia", contenidoResolucion.fechainicioaudiencia)
                .Set("contenido.fechafinaudiencia", contenidoResolucion.fechafinaudiencia)
                .Set("contenido.url", contenidoResolucion.url)
                .Set("contenido.participantes", contenidoResolucion.participantes)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return resolucion;
        }

        public void actualizarDocumentoSEN(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            SolicitudExpedienteNotarioDTO solicitudExpedienteNotarioDTO = new SolicitudExpedienteNotarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            solicitudExpedienteNotarioDTO = JsonConvert.DeserializeObject<SolicitudExpedienteNotarioDTO>(json);

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoSolicitudExpedienteNotario contenidoSolicitudExpedienteNotario = new ContenidoSolicitudExpedienteNotario()
            {
                titulo = solicitudExpedienteNotarioDTO.contenidoDTO.titulo,
                descripcion = solicitudExpedienteNotarioDTO.contenidoDTO.descripcion,
                idnotario = solicitudExpedienteNotarioDTO.contenidoDTO.idnotario.id
            };

            var filter = Builders<Documento>.Filter.Eq("id", solicitudExpedienteNotarioDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoSolicitudExpedienteNotario.titulo)
                .Set("contenido.descripcion", contenidoSolicitudExpedienteNotario.descripcion)
                .Set("contenido.idnotario", contenidoSolicitudExpedienteNotario.idnotario);
            _documentos.UpdateOne(filter, update);
        }

        public ResultadoBPN actualizarDocumentoResultadoBPN(ExpedienteWrapper expedienteWrapper, List<string> url2)
        {
            //Deserealizacion de Obcject a tipo DTO
            ResultadoBPNDTO resultadoBPNDTO = new ResultadoBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            resultadoBPNDTO = JsonConvert.DeserializeObject<ResultadoBPNDTO>(json);

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoResultadoBPN contenidoResultadoBPN = new ContenidoResultadoBPN()
            {
                cantidadfoja = resultadoBPNDTO.contenidoDTO.cantidadfoja,
                costo = resultadoBPNDTO.contenidoDTO.costo,
                idescriturapublica = resultadoBPNDTO.contenidoDTO.idescriturapublica.id
            };
            ResultadoBPN resultadoBPN = new ResultadoBPN();

            var filter = Builders<Documento>.Filter.Eq("id", resultadoBPNDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.cantidadfoja", contenidoResultadoBPN.cantidadfoja)
                .Set("contenido.costo", contenidoResultadoBPN.costo)
                .Set("contenido.idescriturapublica", contenidoResultadoBPN.idescriturapublica)
                .Set("urlanexo", url2);
            _documentos.UpdateOne(filter, update);
            return resultadoBPN;
        }
        public async Task<List<DocumentoADTO2>> ObtenerSolicitudesUsuario(string numerodocumento)
        {

            var filtroNumeroDocumento = Builders<Expediente>.Filter.Eq("cliente.numerodocumento",numerodocumento);

            var proyeccionInicial = new BsonDocument("$project",
                                               new BsonDocument("_id", 0)
                                                    .Add("documento", 
                                                    new BsonDocument("$arrayElemAt",
                                                        new BsonArray { "$documentos", 0 })));

            BsonArray subpipeline = new BsonArray();
            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$iddoc")}
                        )
                    )
                ));

            var lookupe = new BsonDocument("$lookup",
                new BsonDocument("from", "documentos")
                    .Add("let", new BsonDocument("iddoc", "$documento.iddocumento"))
                    .Add("pipeline", subpipeline)
                    .Add("as", "documentoOriginal")
                );

            var proyeccionFinal = new BsonDocument("$project",
                                               new BsonDocument("_id", "$documentoOriginal._id")
                                                    .Add("tipo", "$documentoOriginal.tipo")
                                                    .Add("estado", "$documentoOriginal.estado")
                                                    .Add("contenido","$documentoOriginal.contenido")
                                                    .Add("urlanexo" ,"$documentoOriginal.urlanexo")
                                                    .Add("historialcontenido","$documentoOriginal.historialcontenido")
                                                    .Add("historialproceso","$documentoOriginal.historialproceso"));

         
            List<DocumentoADTO2> expedientes = await _expedientes.Aggregate()
                                        .Match(filtroNumeroDocumento)
                                        .AppendStage<DocumentoUsuarioDTO>(proyeccionInicial)
                                        .AppendStage<DocumentoUsuarioLUDTO>(lookupe)
                                        .Unwind<DocumentoUsuarioLUDTO, DocumentoUsuarioUDTO>(t => t.documentoOriginal)
                                        .AppendStage<DocumentoADTO2>(proyeccionFinal)
                                        .ToListAsync();

            return expedientes;
        }

        public async Task<List<DocumentoADTO2>> ObtenerSolicitudesUsuario2(string numerodocumento)
        {

            var filtroNumeroDocumento = Builders<Expediente>.Filter.Eq("cliente.numerodocumento", numerodocumento);

            var proyeccionInicial = new BsonDocument("$project",
                                        new BsonDocument
                                            {
                                                { "_id", "$_id" },
                                                { "tipo", "$tipo" },
                                                { "documento",
                                        new BsonDocument("$arrayElemAt",
                                        new BsonArray
                                                    {
                                                        "$documentos",
                                                        0
                                                    }) }
                                            });

            BsonArray subpipeline = new BsonArray();
            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$iddoc") }
                        )
                    )
                ));

            var lookupe = new BsonDocument("$lookup",
                new BsonDocument("from", "documentos")
                    .Add("let", new BsonDocument("iddoc", "$documento.iddocumento"))
                    .Add("pipeline", subpipeline)
                    .Add("as", "documentoOriginal")
                );

            var proyeccionFinal = new BsonDocument("$project",
                                        new BsonDocument
                                            {
                                                { "_id", "$documentoOriginal._id" },
                                                { "tipo",
                                        new BsonDocument("$cond",
                                        new BsonArray
                                                    {
                                                        new BsonDocument("$eq",
                                                        new BsonArray
                                                            {
                                                                "$tipo",
                                                                "Solicitud"
                                                            }),
                                                        "$documento.tipo",
                                                        "$tipo"
                                                    }) },
                                                { "estado", "$documentoOriginal.estado" },
                                                { "contenido", "$documentoOriginal.contenido" },
                                                { "urlanexo", "$documentoOriginal.urlanexo" },
                                                { "historialcontenido", "$documentoOriginal.historialcontenido" },
                                                { "historialproceso", "$documentoOriginal.historialproceso" }
                                            });


            List<DocumentoADTO2> expedientes = await _expedientes.Aggregate()
                                        .Match(filtroNumeroDocumento)
                                        .AppendStage<DocumentoUsuarioDTO>(proyeccionInicial)
                                        .AppendStage<DocumentoUsuarioLUDTO>(lookupe)
                                        .Unwind<DocumentoUsuarioLUDTO, DocumentoUsuarioUDTO>(t => t.documentoOriginal)
                                        .AppendStage<DocumentoADTO2>(proyeccionFinal)
                                        .ToListAsync();

            return expedientes;
        }

        public async Task<List<DocumentoADTO2>> ObtenerSolicitudesUsuario3(string numerodocumento)
        {

            var filtroNumeroDocumento = Builders<Expediente>.Filter.Eq("cliente.numerodocumento", numerodocumento);

            var proyeccionInicial = new BsonDocument("$project",
                                        new BsonDocument
                                            {
                                                { "_id", "$_id" },
                                                { "tipo", "$tipo" },
                                                { "documentoinicial",
                                        new BsonDocument("$arrayElemAt",
                                        new BsonArray
                                                    {
                                                        "$documentos",
                                                        0
                                                    }) },
                                                { "documentofinal",
                                        new BsonDocument("$arrayElemAt",
                                        new BsonArray
                                                    {
                                                        "$documentos",
                                                        -1
                                                    }) }
                                            });

            var lookup1 = new BsonDocument("$lookup",
                new BsonDocument
                    {
                        { "from", "documentos" },
                        { "let",
                new BsonDocument("iddoc", "$documentoinicial.iddocumento") },
                        { "pipeline",
                new BsonArray
                        {
                            new BsonDocument("$match",
                            new BsonDocument("$expr",
                            new BsonDocument("$eq",
                            new BsonArray
                                        {
                                            "$_id",
                                            new BsonDocument("$toObjectId", "$$iddoc")
                                        })))
                        } },
                        { "as", "documentoInicialOriginal" }
                    });

            var lookup2 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "documentos" },
                                    { "let",
                            new BsonDocument("iddoc", "$documentofinal.iddocumento") },
                                    { "pipeline",
                            new BsonArray
                                    {
                                        new BsonDocument("$match",
                                        new BsonDocument("$expr",
                                        new BsonDocument("$eq",
                                        new BsonArray
                                                    {
                                                        "$_id",
                                                        new BsonDocument("$toObjectId", "$$iddoc")
                                                    })))
                                    } },
                                    { "as", "documentoFinalOriginal" }
                                });

            var lookup3 = new BsonDocument("$lookup",
                            new BsonDocument
                                {
                                    { "from", "escrituraspublicas" },
                                    { "let",
                            new BsonDocument("idescpub", "$documentoFinalOriginal.contenido.idescriturapublica") },
                                    { "pipeline",
                            new BsonArray
                                    {
                                        new BsonDocument("$match",
                                        new BsonDocument("$expr",
                                        new BsonDocument("$eq",
                                        new BsonArray
                                                    {
                                                        "$_id",
                                                        new BsonDocument("$toObjectId", "$$idescpub")
                                                    })))
                                    } },
                                    { "as", "escriturapublica" }
                                });

            var proyeccionFinal = new BsonDocument("$project",
                                    new BsonDocument
                                        {
                                            { "_id", "$documentoInicialOriginal._id" },
                                            { "tipo",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                                {
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                        {
                                                            "$tipo",
                                                            "Solicitud"
                                                        }),
                                                    "Solicitud Inicial",
                                                    "$tipo"
                                                }) },
                                            { "estado", "$documentoInicialOriginal.estado" },
                                            { "contenido", "$documentoInicialOriginal.contenido" },
                                            { "urlanexo", "$documentoInicialOriginal.urlanexo" },
                                            { "historialcontenido", "$documentoInicialOriginal.historialcontenido" },
                                            { "historialproceso", "$documentoInicialOriginal.historialproceso" },
                                            { "urlexpediente",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                                {
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$size", "$escriturapublica"),
                                                            0
                                                        }),
                                                    "ninguno",
                                                    new BsonDocument("$arrayElemAt",
                                                    new BsonArray
                                                        {
                                                            "$escriturapublica.url",
                                                            0
                                                        })
                                                }) }
                                        });


            List<DocumentoADTO2> expedientes = await _expedientes.Aggregate()
                                        .Match(filtroNumeroDocumento)
                                        .AppendStage<DocumentoUsuarioDTO2>(proyeccionInicial)
                                        .AppendStage<DocumentoUsuarioDTO2_lk1>(lookup1)
                                        .Unwind<DocumentoUsuarioDTO2_lk1, DocumentoUsuarioDTO2_uw1>(t => t.documentoInicialOriginal)
                                        .AppendStage<DocumentoUsuarioDTO2_lk2>(lookup2)
                                         .Unwind<DocumentoUsuarioDTO2_lk2, DocumentoUsuarioDTO2_uw2>(t => t.documentoFinalOriginal)
                                        .AppendStage<DocumentoUsuarioDTO2_lk3>(lookup3)
                                        .AppendStage<DocumentoADTO2>(proyeccionFinal)
                                        .ToListAsync();

            return expedientes;
        }


        public async Task<List<StatisticsDTOR>> estadisticasDocXMesXArea(int mes, string area)
        {
           var match1= new BsonDocument("$match",
                new BsonDocument("historialproceso.area", area));
           var project = new BsonDocument("$project",
                    new BsonDocument
                        {
                            { "_id", "$_id" },
                            { "tipo", "$tipo" },
                            { "mes",  new BsonDocument("$month", "$fechacreacion") }
                        });
            var match2 = new BsonDocument("$match",
                            new BsonDocument("mes", mes));

            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id", "$tipo" },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });

            List<StatisticsDTOR> estadisticas = new List<StatisticsDTOR>();
            estadisticas = await _documentos.Aggregate()
                .AppendStage<Documento>(match1)
                .AppendStage<StatisticsDTO>(project)
                .AppendStage<StatisticsDTO>(match2)
                .AppendStage<StatisticsDTOR>(group)
                .ToListAsync();
            return estadisticas;
        }

        public async Task<List<StatisticsDTOR>> estadisticasDocXMes(int mes)
        {
            var project = new BsonDocument("$project",
                     new BsonDocument
                         {
                            { "_id", "$_id" },
                            { "tipo", "$tipo" },
                            { "mes",  new BsonDocument("$month", "$fechacreacion") }
                         });
            var match2 = new BsonDocument("$match",
                            new BsonDocument("mes", mes));

            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id", "$tipo" },
                                    { "cantidad",
                            new BsonDocument("$sum", 1) }
                                });

            List<StatisticsDTOR> estadisticas = new List<StatisticsDTOR>();
            estadisticas = await _documentos.Aggregate()
                .AppendStage<StatisticsDTO>(project)
                .AppendStage<StatisticsDTO>(match2)
                .AppendStage<StatisticsDTOR>(group)
                .ToListAsync();
            return estadisticas;
        }

        public async Task<List<StatisticsDTO4R>> statisticsDTO4EvaluadosJuntaDirectiva(int mes)
        {
            var match1 = new BsonDocument("$match",
                            new BsonDocument("tipo",
                            new BsonDocument("$in",
                            new BsonArray
                                        {
                                            "OficioDesignacionNotario",
                                            "Resolucion",
                                            "Apelacion",
                                            "OficioBPN"
                                        })));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", "$_id" },
                                    { "tipo", "$tipo" },
                                    { "evaluacion", "$evaluacion" },
                                    { "mes", new BsonDocument("$month", "$fechacreacion") }
                                });

            var group = new BsonDocument("$group",
                                new BsonDocument
                                    {
                                        { "_id", "$tipo" },
                                        { "aprobados",
                                new BsonDocument("$sum",
                                new BsonDocument("$cond",
                                new BsonArray{
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                        {
                                                            "$evaluacion.resultado",
                                                            "Aprobado"
                                                        }),
                                                    1,
                                                    0
                                                })) },
                                        { "rechazados",
                                new BsonDocument("$sum",
                                new BsonDocument("$cond",
                                new BsonArray
                                                {
                                                    new BsonDocument("$eq",
                                                    new BsonArray
                                                        {
                                                            "$evaluacion.resultado",
                                                            "Desaprobado"
                                                        }), 1, 0
                                                })) }
                                    });

            var match2 = new BsonDocument("$match",
                            new BsonDocument("mes",mes));
            List<StatisticsDTO4R> estadisticas = new List<StatisticsDTO4R>();
            estadisticas = await _documentos.Aggregate()
                .AppendStage<Documento>(match1)
                .AppendStage<StatisticsDTO4_project1>(project)
                .AppendStage<StatisticsDTO4_project1>(match2)
                .AppendStage<StatisticsDTO4R>(group)
                .ToListAsync();
            return estadisticas;
        }
        public async Task<List<StatisticsDTO3_group>> estadisticasDocumentosCaducadosXMes(int mes)
        {
            var match1 = new BsonDocument("$match",
                new BsonDocument("estado", "caducado"));
            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", "$_id" },
                                    { "tipo", "$tipo" },
                                    { "mes",
                            new BsonDocument("$month", "$fechacreacion") }
                                });
            var match2 = new BsonDocument("$match",
                             new BsonDocument("mes", mes));
            var group = new BsonDocument("$group",
                                new BsonDocument
                                    {
                                        { "_id", "$tipo" },
                                        { "caducados",
                                new BsonDocument("$sum", 1) }
                                    });
            List<StatisticsDTO3_group> estadisticas = new List<StatisticsDTO3_group>();
            estadisticas = await _documentos.Aggregate()
                .AppendStage<Documento>(match1)
                .AppendStage<StatisticsDTO3_project>(project)
                .AppendStage<StatisticsDTO3_project>(match2)
                .AppendStage<StatisticsDTO3_group>(group)
                .ToListAsync();
            return estadisticas;
        }

        public void actualizarDocumentoSolicitudInicial(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            SolicitudInicialDTO SIDTO = new SolicitudInicialDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            SIDTO = JsonConvert.DeserializeObject<SolicitudInicialDTO>(json);
            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoSolicitudInicial contenidoSolicitudInicial = new ContenidoSolicitudInicial()
            {
               
                titulo = SIDTO.contenidoDTO.titulo,
                descripcion = SIDTO.contenidoDTO.descripcion
            };

            var filter = Builders<Documento>.Filter.Eq("id", SIDTO.id);
            var update = Builders<Documento>.Update
                .Set("estado", "modificado")
                .Set("contenido.titulo", SIDTO.contenidoDTO.titulo)
                .Set("contenido.descripcion", SIDTO.contenidoDTO.descripcion);
            _documentos.UpdateOne(filter, update);
        }

        public void actualizarDocumentoEEN(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            EntregaExpedienteNotarioDTO EENDTO = new EntregaExpedienteNotarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            EENDTO = JsonConvert.DeserializeObject<EntregaExpedienteNotarioDTO>(json);

            var filter = Builders<Documento>.Filter.Eq("id", EENDTO.id);
            var update = Builders<Documento>.Update
                .Set("estado", "modificado")
                .Set("contenido.titulo", EENDTO.contenidoDTO.titulo)
                .Set("contenido.descripcion", EENDTO.contenidoDTO.descripcion)
                .Set("contenido.idnotario", EENDTO.contenidoDTO.idnotario.id);

            _documentos.UpdateOne(filter, update);
        }
        public void modifyState(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            SolicitudInicialDTO SIDTO = new SolicitudInicialDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            SIDTO = JsonConvert.DeserializeObject<SolicitudInicialDTO>(json);

            SolicitudInicial solicitud = new SolicitudInicial()
            {
                estado = SIDTO.estado
            };

            var filter = Builders<Documento>.Filter.Eq("id", SIDTO.id);
            var update = Builders<Documento>.Update
                .Set("estado", SIDTO.estado);
            _documentos.UpdateOne(filter, update);
        }

        //Nuevas estadisticas recomendadas por el profesor Linarez
        //docs caducados, procesados y pendientes para los 12 tipos de docs por mes
        public async Task<List<estadistica1_group>> obtenecionEstadistica1(int mes)
        {
            var match1 = new BsonDocument("$match",
                new BsonDocument
                    {
                        { "estado",
                new BsonDocument("$in",
                new BsonArray
                            {"caducado", "procesado","pendiente"}) },
                        { "tipo",
                            new BsonDocument("$nin",
                            new BsonArray
                                        {
                                            "SolicitudInicial"
                                        }) }
                    });

           var project1 =  new BsonDocument("$project",
                    new BsonDocument
                        {
                                { "mes",
                        new BsonDocument("$month", "$fechacreacion") },
                                { "estado", "$estado" },
                                { "tipo", "$tipo" }
                        });

            var match2 = new BsonDocument("$match",
                                new BsonDocument("mes", mes));

            var group = new BsonDocument("$group",
    new BsonDocument
        {
            { "_id", "$tipo" },
            { "caducados",
                    new BsonDocument("$sum",
                    new BsonDocument("$cond",
                    new BsonArray{new BsonDocument("$eq",
                                        new BsonArray
                                            { "$estado",
                                                "caducado"
                                            }), 0, 1})) },
                            { "procesados",
                                    new BsonDocument("$sum",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                    {
                                        new BsonDocument("$eq",
                                        new BsonArray
                                            {
                                                "$estado",
                                                "procesado"
                                            }),
                                        0,
                                        1
                                    })) },
                            { "pendientes",
                                    new BsonDocument("$sum",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                                    {
                                                        new BsonDocument("$eq",
                                                        new BsonArray
                                                            {
                                                                "$estado",
                                                                "pendiente"
                                                            }),
                                                        0,
                                                        1
                                                    })) }
                                        });

            List<estadistica1_group> estadistica1 = new List<estadistica1_group>();
            estadistica1 = _documentos.Aggregate()
                                        .AppendStage<Documento>(match1)
                                        .AppendStage<estadistica1_proyect1>(project1)
                                        .AppendStage<estadistica1_proyect1>(match2)
                                        .AppendStage<estadistica1_group>(group)
                                        .ToList();
            return estadistica1;
        }

        //docs caducados, procesados y pendientes para los 12 tipos de docs por mes y por areas
        public async Task<List<estadistica1_group>> obtenecionEstadistica2(int mes, string area)
        {
            var match1 = new BsonDocument("$match",
                                new BsonDocument
                                    {
                                        { "estado",
                                new BsonDocument("$in",
                                new BsonArray
                                            {
                                                "caducado",
                                                "procesado",
                                                "pendiente"
                                            }) },
                                        { "tipo",
                                new BsonDocument("$nin",
                                new BsonArray
                                            {
                                                "SolicitudInicial"
                                            }) },
                                        { "historialproceso.area", area }
                                    });


            var project1 = new BsonDocument("$project",
                     new BsonDocument
                         {
                                { "mes",
                        new BsonDocument("$month", "$fechacreacion") },
                                { "estado", "$estado" },
                                { "tipo", "$tipo" }
                         });

            var match2 = new BsonDocument("$match",
                                new BsonDocument("mes", mes));

            var group = new BsonDocument("$group",
    new BsonDocument
        {
            { "_id", "$tipo" },
            { "caducados",
                    new BsonDocument("$sum",
                    new BsonDocument("$cond",
                    new BsonArray{new BsonDocument("$eq",
                                        new BsonArray
                                            { "$estado",
                                                "caducado"
                                            }), 0, 1})) },
                            { "procesados",
                                    new BsonDocument("$sum",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                    {
                                        new BsonDocument("$eq",
                                        new BsonArray
                                            {
                                                "$estado",
                                                "procesado"
                                            }),
                                        0,
                                        1
                                    })) },
                            { "pendientes",
                                    new BsonDocument("$sum",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                                    {
                                                        new BsonDocument("$eq",
                                                        new BsonArray
                                                            {
                                                                "$estado",
                                                                "pendiente"
                                                            }),
                                                        0,
                                                        1
                                                    })) }
                                        });

            List<estadistica1_group> estadistica1 = new List<estadistica1_group>();
            estadistica1 = _documentos.Aggregate()
                                        .AppendStage<Documento>(match1)
                                        .AppendStage<estadistica1_proyect1>(project1)
                                        .AppendStage<estadistica1_proyect1>(match2)
                                        .AppendStage<estadistica1_group>(group)
                                        .ToList();
            return estadistica1;
        }

        public async Task<List<estadistica1_group>> obtenecionEstadistica3(int mes, string idusuario)
        {
            var match1 = new BsonDocument("$match",
                                new BsonDocument
                                    {
                                        { "estado",
                                            new BsonDocument("$in",
                                            new BsonArray
                                                        {
                                                            "caducado",
                                                            "procesado",
                                                            "pendiente"
                                                        }) },
                                        { "tipo",
                                                new BsonDocument("$nin",
                                                new BsonArray
                                                            {
                                                                "SolicitudInicial"
                                                            }) },
                                                        { "historialproceso.idemisor", idusuario }
                                    });


            var project1 = new BsonDocument("$project",
                     new BsonDocument
                         {
                                { "mes",
                        new BsonDocument("$month", "$fechacreacion") },
                                { "estado", "$estado" },
                                { "tipo", "$tipo" }
                         });

            var match2 = new BsonDocument("$match",
                                new BsonDocument("mes", mes));

            var group = new BsonDocument("$group",
    new BsonDocument
        {
            { "_id", "$tipo" },
            { "caducados",
                    new BsonDocument("$sum",
                    new BsonDocument("$cond",
                    new BsonArray{new BsonDocument("$eq",
                                        new BsonArray
                                            { "$estado",
                                                "caducado"
                                            }), 0, 1})) },
                            { "procesados",
                                    new BsonDocument("$sum",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                    {
                                        new BsonDocument("$eq",
                                        new BsonArray
                                            {
                                                "$estado",
                                                "procesado"
                                            }),
                                        0,
                                        1
                                    })) },
                            { "pendientes",
                                    new BsonDocument("$sum",
                                    new BsonDocument("$cond",
                                    new BsonArray
                                                    {
                                                        new BsonDocument("$eq",
                                                        new BsonArray
                                                            {
                                                                "$estado",
                                                                "pendiente"
                                                            }),
                                                        0,
                                                        1
                                                    })) }
                                        });

            List<estadistica1_group> estadistica1 = new List<estadistica1_group>();
            estadistica1 = _documentos.Aggregate()
                                        .AppendStage<Documento>(match1)
                                        .AppendStage<estadistica1_proyect1>(project1)
                                        .AppendStage<estadistica1_proyect1>(match2)
                                        .AppendStage<estadistica1_group>(group)
                                        .ToList();
            return estadistica1;
        }


       
    }
}
