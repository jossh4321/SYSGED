using MongoDB.Bson;
using MongoDB.Driver;
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
        public OficioDesignacionNotario registrarOficioDesignacionNotario(OficioDesignacionNotario documentoODN)
        {
            
            _documentos.InsertOne(documentoODN);
            DocumentoExpediente documentoExpediente = new DocumentoExpediente();
            documentoExpediente.indice = 7;
            documentoExpediente.iddocumento = documentoODN.id;
            documentoExpediente.tipo = "OficioDesignacionNotario";
            documentoExpediente.fechacreacion = DateTime.Now;
            documentoExpediente.fechaexceso = DateTime.Now.AddDays(5);
            documentoExpediente.fechademora = null;
            UpdateDefinition<Expediente> updateExpediente = Builders<Expediente>.Update.Push("documentos", documentoExpediente);
            Expediente expediente = _expedientes.FindOneAndUpdate(exp => exp.cliente.numerodocumento == "12345678" &&
            exp.cliente.tipodocumento == "dni", updateExpediente);
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
    }
}
