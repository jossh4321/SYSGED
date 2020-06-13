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
        public DocumentoService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _documentos = database.GetCollection<Documento>("documentos");
        }
        public List<Documento> obtenerDocumentos()
        {
            return _documentos.Find(documento => true).ToList();
        }
        public OficioDesignacionNotario registrarOficioDesignacionNotario(OficioDesignacionNotario documentoODN)
        {
            _documentos.InsertOne(documentoODN);
            return documentoODN;
        }
        public OficioBPN registrarSolicitudBPN(OficioBPN documentoOficioBPN)
        {
            _documentos.InsertOne(documentoOficioBPN);
            return documentoOficioBPN;
        }
    }
}
