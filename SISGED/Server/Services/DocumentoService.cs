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
            OficioDesignacionNotarioDTO oficioDesignacionNotarioDTO = new OficioDesignacionNotarioDTO();
            var json = JsonConvert.SerializeObject(expedienteWrapper.documento);
            oficioDesignacionNotarioDTO = JsonConvert.DeserializeObject<OficioDesignacionNotarioDTO>(json);

            //oficioDesignacionNotarioDTO = (OficioDesignacionNotarioDTO)expedienteWrapper.documento;
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

            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 7;
            documentoExpediente.iddocumento = documentoODN.id;
            documentoExpediente.tipo = "OficioDesignacionNotario";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;

            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            //var filter = Builders<Expediente>.Filter.Eq("id", ObjectId.Parse("5ee5f24e7d8f833f68cc88a0"));
            Expediente expediente = _expedientes.FindOneAndUpdate(x => x.id == "5ee5f24e7d8f833f68cc88a0", updateExpediente);

            /*UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            var filter = Builders<Expediente>.Filter.Eq("id", ObjectId.Parse("5ee5f24e7d8f833f68cc88a0"));
            Expediente expediente = _expedientes.FindOneAndUpdate(filter, updateExpediente);*/

            /*  UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(exp => exp.id == "12345678" &&
            exp.cliente.tipodocumento == "dni", updateExpediente);*/

            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = documentoExpediente.iddocumento;
            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == "5ee2e87bd12fcd05d6b6b13e", updateBandeja);
            return documentoODN;
        }
        public OficioBPN registrarSolicitudBPN(OficioBPN documentoOficioBPN)
        {
            _documentos.InsertOne(documentoOficioBPN);
            return documentoOficioBPN;
        }
        public SolicitudExpedicionFirma registrarSolicitudExpedicionFirma(SolicitudExpedicionFirma documentoSEF)
        {
            _documentos.InsertOne(documentoSEF);
            return documentoSEF;
        }
        public SolicitudDenuncia registrarSolicitudDenuncia(SolicitudDenuncia documentoSD)
        {
            _documentos.InsertOne(documentoSD);
            return documentoSD;
        }
        /*public Documento modificarEstado(Documento documento)
        {
            var filter = Builders<Documento>.Filter.Eq("id", documento.id);
            var update = Builders<Documento>.Update
                .Set("estado", documento.estado);
            documento = _documentos.FindOneAndUpdate<Documento>(filter, update, new FindOneAndUpdateOptions<Documento>
            {
                ReturnDocument = ReturnDocument.After
            });
            return documento;
        }*/
    }
}
