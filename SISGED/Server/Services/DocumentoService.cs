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

        public DocumentoService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _documentos = database.GetCollection<Documento>("documentos");
            _expedientes = database.GetCollection<Expediente>("expedientes");
            _bandejas = database.GetCollection<Bandeja>("bandejas");
        }
        public List<Documento> obtenerDocumentos()
        {
            return _documentos.Find(documento => true).ToList();
        }
        public OficioDesignacionNotario registrarOficioDesignacionNotario(ExpedienteWrapper expedienteWrapper)
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
                fecharealizacion = new DateTime(),
                lugaroficionotarial = oficioDesignacionNotarioDTO.contenidoDTO.lugaroficionotarial,
                idusuario = oficioDesignacionNotarioDTO.contenidoDTO.idusuario,
                idnotario = oficioDesignacionNotarioDTO.contenidoDTO.idnotario.id,
            };
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario()
            {
                tipo = "OficioDesignacionNotario",
                contenido = contenidoODN,
                estado = new Estado()
                {
                    status = "pendiente",
                    observacion = null
                },
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            _documentos.InsertOne(documentoODN);

            //Actualizacion del expediente
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 2;
            documentoExpediente.iddocumento = documentoODN.id;
            documentoExpediente.tipo = "OficioDesignacionNotario";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == "5ee5f24e7d8f833f68cc88a0", updateExpediente);


            //actualizacion bandeja salida del usuario
            BandejaDocumento bandejaSalidaDocumento = new BandejaDocumento();
            bandejaSalidaDocumento.idexpediente = expediente.id;
            bandejaSalidaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandejaSalida = Builders<Bandeja>.Update.Push("bandejasalida", bandejaSalidaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaSalida);

            //actualizacion bandeja de entrada del usuario
            UpdateDefinition<Bandeja> updateBandejaEntrada = 
                Builders<Bandeja>.Update.PullFilter("bandejaentrada", 
                  Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);
            return documentoODN;
        }
        public OficioBPN registrarOficioBPNE(ExpedienteWrapper expedienteWrapper)
        {
            //Obtenemos los datos del expedientewrapper
            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioBPNDTO = JsonConvert.DeserializeObject<OficioBPNDTO>(json);

            //Insertando el oficio normal
            ContenidoOficioBPN contenidoSolicitudBPN = new ContenidoOficioBPN()
            {
                titulo = oficioBPNDTO.contenidoDTO.titulo,
                descripcion = oficioBPNDTO.contenidoDTO.descripcion,
                idcliente = oficioBPNDTO.contenidoDTO.idcliente.id,
                direccionoficio = oficioBPNDTO.contenidoDTO.direccionoficio,
                idnotario = oficioBPNDTO.contenidoDTO.idnotario.id,
                actojuridico = oficioBPNDTO.contenidoDTO.actojuridico,
                tipoprotocolo = oficioBPNDTO.contenidoDTO.tipoprotocolo,
                otorgantes = oficioBPNDTO.contenidoDTO.otorgantes,
                fecharealizacion = DateTime.Now,
                url = "ninguna"
            };
            OficioBPN documentoBPN = new OficioBPN()
            {
                tipo = "OficioBPN",
                contenido = contenidoSolicitudBPN,
                //estado = "pendiente",
                estado = new Estado()
                {
                    status = "pendiente",
                    observacion = "Ninguna",
                },
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            _documentos.InsertOne(documentoBPN);

            //Actualizamos el expediente y agregamos el documento a sus documentos contenidos
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 2;
            documentoExpediente.iddocumento = documentoBPN.id;
            documentoExpediente.tipo = "OficioBPN";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;

            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == "5ee5f24e7d8f833f68cc88a0", updateExpediente);

            //Actualizacion de bandeja de salida de usuario
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandeja);

            //Actualizacion de bandeja de entrada de usuario
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);
            return documentoBPN;
        }
        public SolicitudExpedicionFirma registrarSolicitudExpedicionFirma(SolicitudExpedicionFirma documentoSEF)
        {
            _documentos.InsertOne(documentoSEF);
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

        public SolicitudDenuncia registrarSolicitudDenuncia(SolicitudDenuncia documentoSD)
        {
            _documentos.InsertOne(documentoSD);
            return documentoSD;
        }
        public ConclusionFirma registrarConclusionFirmaE(ExpedienteWrapper expedienteWrapper)
        {
            //Obtenemos los datos del expedientewrapper
            ConclusionFirmaDTO conclusionfirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            conclusionfirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);

            //Insertando la conclusion normal
            ContenidoConclusionFirma contenidoCF = new ContenidoConclusionFirma()
            {
                idescriturapublica = conclusionfirmaDTO.contenidoDTO.idescriturapublica.id
            };
            ConclusionFirma documentoDF = new ConclusionFirma()
            {
                tipo = "ConclusionFirma",
                contenido = contenidoCF,
                estado = "pendiente",
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>()
            };
            _documentos.InsertOne(documentoDF);

            //Actualizamos el expediente y agregamos el documento a sus documentos contenidos
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = documentoDF.id;
            documentoExpediente.tipo = "ConclusionFirma";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;

            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id ==  expedienteWrapper.idexpediente, updateExpediente);

            //actualizando Bandeja salida
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandeja);

            //actualizando Bandeja Entrada
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);

            return documentoDF;
        }

        /*Esto funciona
         * public ConclusionFirma registrarConclusionFirma(ConclusionFirma documentoCF)
        {
            _documentos.InsertOne(documentoCF);
            return documentoCF;
        }*/

        public Documento modificarEstado(DocumentoEvaluadoDTO documento)
        {
            var filter = Builders<Documento>.Filter.Eq("id", documento.id);
            var update = Builders<Documento>.Update
                .Set("estado", documento.estado);
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = documento.idexpediente;
            bandejaDocumento.iddocumento = documento.id;

            UpdateDefinition<Bandeja> updateBandejaD = Builders<Bandeja>.Update.Pull("bandejaentrada", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaD);

            UpdateDefinition<Bandeja> updateBandejaI = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaI);

            return _documentos.FindOneAndUpdate<Documento>(filter, update);
        }

        public AperturamientoDisciplinario registrarAperturamientoDisciplinario(AperturamientoDisciplinarioDTO aperturamientoDisciplinarioDTO,
            string urldata, string idusuario,string idexpediente , string iddocentrada)
        {
            //Creacionde le objeto de AperturamientoDisciplinario y registro en la coleccion documentos
            ContenidoAperturamientoDisciplinario contenidoAD = new ContenidoAperturamientoDisciplinario()
            {
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
                url = urldata
            };
            AperturamientoDisciplinario aperturamientodisciplinario = new AperturamientoDisciplinario()
            {
                tipo = "AperturamientoDisciplinario",
                contenido = contenidoAD,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = "creado"
            };
            _documentos.InsertOne(aperturamientodisciplinario);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = aperturamientodisciplinario.id;
            documentoExpediente.tipo = "AperturamientoDisciplinario";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //actualizando Bandeja salida
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);

            //actualizando Bandeja Entrada
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);

            return aperturamientodisciplinario;
        }

        public Dictamen RegistrarDictamen(ExpedienteWrapper expedientewrapper)
        {
            //Obtenemos los datos del expedientewrapper
            DictamenDTO dictamenDTO = new DictamenDTO();
            var json = JsonConvert.SerializeObject(expedientewrapper.documento);
            dictamenDTO = JsonConvert.DeserializeObject<DictamenDTO>(json);

            //creacion del Objeto de tipo Dictamen y el registro en la coleccion Dictamen
            ContenidoDictamen contenidodictamen = new ContenidoDictamen()
            {
                titulo = dictamenDTO.contenidoDTO.titulo,
                descripcion = dictamenDTO.contenidoDTO.descripcion,
                nombredenunciante = dictamenDTO.contenidoDTO.nombredenunciante,
                conclusion = dictamenDTO.contenidoDTO.conclusion,
                observaciones = dictamenDTO.contenidoDTO.observaciones.Select(x => x.descripcion).ToList(),
                recomendaciones = dictamenDTO.contenidoDTO.recomendaciones.Select(x => x.descripcion).ToList(),
                //fecha           
            };
            Dictamen dictamen = new Dictamen()
            {
                tipo = "Dictamen",
                contenido = contenidodictamen,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = "creado"
            };
            _documentos.InsertOne(dictamen);

            //actualizacion de expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = dictamen.id;
            documentoExpediente.tipo = "Dictamen";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, expedientewrapper.idexpediente);

            //actualizando Bandeja salida
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedientewrapper.idusuarioactual, updateBandeja);

            //actualizando Bandeja Entrada
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedientewrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedientewrapper.idusuarioactual, updateBandejaEntrada);

            return dictamen;
        }

        public Resolucion registrarResolucion(ResolucionDTO resolucionDTO,
            string urldata, string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacionde le objeto de AperturamientoDisciplinario y registro en la coleccion documentos
            ContenidoResolucion contenidoResolucion = new ContenidoResolucion()
            {
                descripcion = resolucionDTO.contenidoDTO.descripcion,
                fechainicioaudiencia = resolucionDTO.contenidoDTO.fechainicioaudiencia,
                fechafinaudiencia = resolucionDTO.contenidoDTO.fechafinaudiencia,
                participantes = resolucionDTO.contenidoDTO.participantes.Select(x => x.nombre).ToList(),
                sancion = resolucionDTO.contenidoDTO.sancion,
                url = urldata
            };
            Resolucion resolucion = new Resolucion()
            {
                tipo = "Resolucion",
                contenido = contenidoResolucion,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = new Estado()
                {
                    status = "pendiente",
                    observacion = ""
                }
            };
            _documentos.InsertOne(resolucion);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 8;
            documentoExpediente.iddocumento = resolucion.id;
            documentoExpediente.tipo = "Resolucion";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //actualizando Bandeja salida
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);

            //actualizando Bandeja Entrada
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);

            return resolucion;
        }

        public Expediente actualizarExpediente(DocumentoExpediente documentoExpediente, string idexpediente)
        {
            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == idexpediente, updateExpediente);
            return expediente;
        }

        public Apelacion registrarApelacion(ApelacionDTO apelacionDTO,
            string urldata, string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacion de la Apelacion y registro en la coleccion documentos
            ContenidoApelacion contenidoApe = new ContenidoApelacion()
            {
                titulo = apelacionDTO.contenidoDTO.titulo,
                descripcion = apelacionDTO.contenidoDTO.descripcion,
                fechaapelacion = DateTime.Now,
                url = urldata
            };
            Apelacion apelacion = new Apelacion()
            {
                tipo = "Apelacion",
                contenido = contenidoApe,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = new Estado()
                {
                    status = "pendiente",
                    observacion = "Ninguna",
                }
            };
            _documentos.InsertOne(apelacion);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 9;
            documentoExpediente.iddocumento = apelacion.id;
            documentoExpediente.tipo = "AperturamientoDisciplinario";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //actualizando Bandeja salida
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);

            //actualizando Bandeja Entrada
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);

            return apelacion;
        }

        //registrarSolicitudExpedienteNotario
        public SolicitudExpedienteNotario registrarSolicitudExpedienteNotario(SolicitudExpedienteNotarioDTO solicitudExpedienteNotarioDTO,
            string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacion de la Apelacion y registro en la coleccion documentos
            ContenidoSolicitudExpedienteNotario contenidoSEN = new ContenidoSolicitudExpedienteNotario()
            {
                titulo = solicitudExpedienteNotarioDTO.contenidoDTO.titulo,
                descripcion = solicitudExpedienteNotarioDTO.contenidoDTO.descripcion,
                fechaemision = DateTime.Now
            };
            SolicitudExpedienteNotario solicitudExpedienteNotarioAct = new SolicitudExpedienteNotario()
            {
                tipo = "SolicitudExpedienteNotario",
                contenido = contenidoSEN,
                historialcontenido = new List<ContenidoVersion>(),
                historialproceso = new List<Proceso>(),
                estado = "pendiente"
            };
            _documentos.InsertOne(solicitudExpedienteNotarioAct);

            //Actualizacion del expediente
            Expediente expediente = new Expediente();
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 10;
            documentoExpediente.iddocumento = solicitudExpedienteNotarioAct.id;
            documentoExpediente.tipo = "SolicitudExpedienteNotario";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //actualizando Bandeja salida
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);

            //actualizando Bandeja Entrada
            UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);

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
                                new BsonDocument("from","notarios")
                                .Add("let",new BsonDocument("idnot", "$contenido.idnotario"))
                                .Add("pipeline", subpipeline)
                                .Add("as", "notario"));


            OficioDesignacionNotario_ur documentoODN = new OficioDesignacionNotario_ur();
            documentoODN = _documentos.Aggregate()
                .AppendStage<OficioDesignacionNotario>(match)
                .AppendStage<OficioDesignacionNotario_lookup>(aggregation)
                .Unwind<OficioDesignacionNotario_lookup, OficioDesignacionNotario_ur>(x =>x.notario)
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
            oficioDesignacionNotario.estado = documentoODN.estado;

            return oficioDesignacionNotario;

        }
        public OficioBPNDTO obtenerOficioBusquedaProtocoloNotarial(string id)
        {
            var  match = new BsonDocument("$match", new BsonDocument("_id",
                        new ObjectId(id)));
            OficioBPNDTO_ur oficioBPN = new OficioBPNDTO_ur();
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

            oficioBPN = _documentos.Aggregate().
                AppendStage<OficioBPN>(match)
                .AppendStage<OficioBPNDTO_lookup>(aggregation)
                .Unwind<OficioBPNDTO_lookup, OficioBPNDTO_ur>(x => x.notario).First();

            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
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
            oficioBPNDTO.estado = oficioBPN.estado;
            return oficioBPNDTO;
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
                descripcion = docResolucion.contenido.descripcion,
                titulo = docResolucion.contenido.titulo,
                fechainicioaudiencia = docResolucion.contenido.fechainicioaudiencia,
                fechafinaudiencia = docResolucion.contenido.fechafinaudiencia,
                participantes = docResolucion.contenido.participantes.Select((x, y) => new Participante() { nombre=x,index=y}).ToList(),
                sancion = docResolucion.contenido.sancion,
                data = docResolucion.contenido.url         
            };
            return resolucionDTO;
        }
    }
}
