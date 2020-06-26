﻿using MongoDB.Bson;
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
    }
}
