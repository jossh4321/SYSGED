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
                fecharealizacion = DateTime.Now,
                lugaroficionotarial = oficioDesignacionNotarioDTO.contenidoDTO.lugaroficionotarial,
                idusuario = oficioDesignacionNotarioDTO.contenidoDTO.idusuario,
                idnotario = oficioDesignacionNotarioDTO.contenidoDTO.idnotario.id,
            };
            OficioDesignacionNotario documentoODN = new OficioDesignacionNotario()
            {
                tipo = "OficioDesignacionNotario",
                contenido = contenidoODN,
                evaluacion = new Evaluacion()
                {
                    status = "pendiente",
                    observacion = null
                },
                estado = "creado",
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

        public SolicitudBPN registrarSolicitudBPN(ExpedienteWrapper expedienteWrapper)
        {
            
            //Obtenemos los datos del expedientewrapper
            SolicitudBPNDTO documento = new SolicitudBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            documento = JsonConvert.DeserializeObject<SolicitudBPNDTO>(json);
            //Listas de participantes a string
            List<String> listaotorgantes = new List<string>();
            foreach (Otorgantelista oto in documento.contenidoDTO.otorganteslista)
            {
                listaotorgantes.Add(oto.nombre);
                listaotorgantes.Add(oto.apellido);
                listaotorgantes.Add(oto.dni);
            }


            //Creacionde Obj ContenidoSolicitudBPN y almacenamiento en la coleccion documento
            ContenidoSolicitudBPN contenidoSolicitudBPN = new ContenidoSolicitudBPN()
            {
                idcliente = documento.contenidoDTO.idcliente.id,
                direccionoficio = documento.contenidoDTO.direccionoficio,
                idnotario = documento.contenidoDTO.idnotario.id,
                actojuridico = documento.contenidoDTO.actojuridico,
                tipoprotocolo = documento.contenidoDTO.tipoprotocolo,
                otorgantes = listaotorgantes,
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
            _documentos.InsertOne(solicitudBPN);
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
                evaluacion = new Evaluacion()
                {
                    status = "pendiente",
                    observacion = "Ninguna",
                },
                estado = "Creado",
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
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == "5eeadf0b8ca4ff53a0b791e3", updateExpediente);

            //Actualizacion de bandeja de salida de usuario
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandeja);*/

            //Actualizacion de bandeja de entrada de usuario
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);*/

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedienteWrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
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
                idescriturapublica = conclusionfirmaDTO.contenidoDTO.idescriturapublica.id,
                idnotario = conclusionfirmaDTO.contenidoDTO.idnotario.id,
                idcliente = conclusionfirmaDTO.contenidoDTO.idcliente.id,
                cantidadfoja = conclusionfirmaDTO.contenidoDTO.cantidadfoja,
                precio = (conclusionfirmaDTO.contenidoDTO.cantidadfoja * 30)
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
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == expedienteWrapper.idexpediente, updateExpediente);

            //actualizando Bandeja salida
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandeja);*/

            //actualizando Bandeja Entrada
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", expedienteWrapper.documentoentrada));
            _bandejas.UpdateOne(band => band.usuario == expedienteWrapper.idusuarioactual, updateBandejaEntrada);*/

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", expedienteWrapper.documentoentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
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
                .Set("evaluacion", documento.evaluacion);
            //BandejaDocumento bandejaDocumento = new BandejaDocumento();
            //bandejaDocumento.idexpediente = documento.idexpediente;
            //bandejaDocumento.iddocumento = documento.id;

            //UpdateDefinition<Bandeja> updateBandejaD = Builders<Bandeja>.Update.Pull("bandejaentrada", bandejaDocumento);
            //_bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaD);

            //UpdateDefinition<Bandeja> updateBandejaI = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            //_bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaI);

            return _documentos.FindOneAndUpdate<Documento>(filter, update);
        }
        public Documento generarDocumento(DocumentoGenerarDTO documento)
        {
            Documento doc = new Documento();
            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = documento.idexpediente;
            bandejaDocumento.iddocumento = documento.iddocumento;

            UpdateDefinition<Bandeja> updateBandejaD = Builders<Bandeja>.Update.Pull("bandejaentrada", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaD);

            UpdateDefinition<Bandeja> updateBandejaI = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == documento.idusuario, updateBandejaI);

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
            string urldata, string idusuario, string idexpediente, string iddocentrada)
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
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);*/

            //actualizando Bandeja Entrada
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);*/

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
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
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
            string urldata, List<string> url2, string idusuario, string idexpediente, string iddocentrada)
        {
            //Creacionde le objeto de AperturamientoDisciplinario y registro en la coleccion documentos
            ContenidoResolucion contenidoResolucion = new ContenidoResolucion()
            {
                titulo= resolucionDTO.contenidoDTO.titulo,
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
                urlanexo = url2,
                evaluacion = new Evaluacion()
                {
                    status = "pendiente",
                    observacion = ""
                },estado = "creado"
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
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);*/

            //actualizando Bandeja Entrada
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);*/

            //Actulizar el documento anterior a revisado
            var filter = Builders<Documento>.Filter.Eq("id", iddocentrada);
            var update = Builders<Documento>.Update
                .Set("estado", "revisado");
            _documentos.UpdateOne(filter, update);
            return resolucion;
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
                urlanexo = url2,
                evaluacion = new Evaluacion()
                {
                    status = "pendiente",
                    observacion = "Ninguna",
                },
                estado = "creado"
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
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);*/

            //actualizando Bandeja Entrada
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);*/

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
                titulo = solicitudExpedienteNotarioDTO.contenidoDTO.titulo,
                descripcion = solicitudExpedienteNotarioDTO.contenidoDTO.descripcion,
                idnotario = solicitudExpedienteNotarioDTO.contenidoDTO.idnotario.id,
                fechaemision = DateTime.Now
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
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            expediente = actualizarExpediente(documentoExpediente, idexpediente);

            //actualizando Bandeja salida
            /*BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandeja);*/

            //actualizando Bandeja Entrada
            /*UpdateDefinition<Bandeja> updateBandejaEntrada =
               Builders<Bandeja>.Update.PullFilter("bandejaentrada",
                 Builders<BandejaDocumento>.Filter.Eq("iddocumento", iddocentrada));
            _bandejas.UpdateOne(band => band.usuario == idusuario, updateBandejaEntrada);*/

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
                participantes = docResolucion.contenido.participantes.Select((x, y) => new Participante() { nombre = x, index = y }).ToList(),
                sancion = docResolucion.contenido.sancion,
                data = docResolucion.contenido.url
            };
            return resolucionDTO;
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
        //actualizarDocumentoODN
        public void actualizarDocumentoODN(ExpedienteWrapper expedienteWrapper)
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

            var filter = Builders<Documento>.Filter.Eq("id", oficioDesignacionNotarioDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoODN.titulo)
                .Set("contenido.descripcion", contenidoODN.descripcion)
                .Set("contenido.lugaroficionotarial", contenidoODN.lugaroficionotarial)
                .Set("contenido.idusuario", contenidoODN.idusuario)
                .Set("contenido.idnotario", contenidoODN.idnotario);
            _documentos.UpdateOne(filter, update);
        }

        public void actualizarDocumentoApelacion(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            ApelacionDTO apelacionDTO = new ApelacionDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            apelacionDTO = JsonConvert.DeserializeObject<ApelacionDTO>(json);

            //Creacion de Obj Apelacion y registro en coleccion de documentos 
            ContenidoApelacion contenidoAPE = new ContenidoApelacion()
            {
                titulo = apelacionDTO.contenidoDTO.titulo,
                descripcion = apelacionDTO.contenidoDTO.descripcion
            };

            var filter = Builders<Documento>.Filter.Eq("id", apelacionDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoAPE.titulo)
                .Set("contenido.descripcion", contenidoAPE.descripcion);
            _documentos.UpdateOne(filter, update);
        }

        public void actualizarDocumentoAperturamientoDisciplinario(ExpedienteWrapper expedienteWrapper)
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
                .Set("contenido.fechafinaudiencia", contenidoAD.fechafinaudiencia);
            _documentos.UpdateOne(filter, update);
        }
        
        public void actualizarDocumentoConclusionFirma(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            ConclusionFirmaDTO conclusionFirmaDTO = new ConclusionFirmaDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            conclusionFirmaDTO = JsonConvert.DeserializeObject<ConclusionFirmaDTO>(json);
            ConclusionFirma documentocf = new ConclusionFirma();

            //Creacion de Obj y registro en coleccion de documentos 
            ContenidoConclusionFirma contenidoCF = new ContenidoConclusionFirma()
            {
                idescriturapublica = conclusionFirmaDTO.contenidoDTO.idescriturapublica.id
            };

            var filter = Builders<Documento>.Filter.Eq("id", conclusionFirmaDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.idescriturapublica", contenidoCF.idescriturapublica);
             _documentos.UpdateOne(filter, update);
        }


        public void actualizarDocumentoDictamen(ExpedienteWrapper expedienteWrapper)
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

            var filter = Builders<Documento>.Filter.Eq("id", dictamenDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoD.titulo)
                .Set("contenido.descripcion", contenidoD.descripcion)
                .Set("contenido.nombredenunciante", contenidoD.nombredenunciante)
                .Set("contenido.conclusion", contenidoD.conclusion)
                .Set("contenido.observaciones", contenidoD.observaciones)
                .Set("contenido.recomendaciones", contenidoD.recomendaciones);
            _documentos.UpdateOne(filter, update);
        }
        
        public void actualizarDocumentoOficioBPN(ExpedienteWrapper expedienteWrapper)
        {
            //Deserealizacion de Obcject a tipo DTO
            OficioBPNDTO oficioBPNDTO = new OficioBPNDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioBPNDTO = JsonConvert.DeserializeObject<OficioBPNDTO>(json);

            /*List<String> listaOtorgantes = new List<string>();
            foreach (Otorgante obs in oficioBPNDTO.contenidoDTO.otorgantes)
            {
                listaOtorgantes.Add(obs.nombre + " " + obs.apellido);
            }*/

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
                otorgantes = oficioBPNDTO.contenidoDTO.otorgantes,
            };

            var filter = Builders<Documento>.Filter.Eq("id", oficioBPNDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoOficioBPN.titulo)
                .Set("contenido.descripcion", contenidoOficioBPN.descripcion)
                .Set("contenido.idcliente", contenidoOficioBPN.idcliente)
                .Set("contenido.direccionoficio", contenidoOficioBPN.direccionoficio)
                .Set("contenido.idnotario", contenidoOficioBPN.idnotario)
                .Set("contenido.actojuridico", contenidoOficioBPN.actojuridico)
                .Set("contenido.tipoprotocolo", contenidoOficioBPN.tipoprotocolo)
                .Set("contenido.otorgantes", contenidoOficioBPN.otorgantes);
            _documentos.UpdateOne(filter, update);
        }

        public void actualizarDocumentoResolucion(ExpedienteWrapper expedienteWrapper)
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
                participantes = listaParticipantes
            };

            var filter = Builders<Documento>.Filter.Eq("id", resolucionDTO.id);
            var update = Builders<Documento>.Update
                .Set("contenido.titulo", contenidoResolucion.titulo)
                .Set("contenido.descripcion", contenidoResolucion.descripcion)
                .Set("contenido.sancion", contenidoResolucion.sancion)
                .Set("contenido.fechainicioaudiencia", contenidoResolucion.fechainicioaudiencia)
                .Set("contenido.fechafinaudiencia", contenidoResolucion.fechafinaudiencia)
                .Set("contenido.participantes", contenidoResolucion.participantes);
            _documentos.UpdateOne(filter, update);
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
    }
}
