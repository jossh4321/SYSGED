using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace SISGED.Server.Services
{
    public class AsistenteService
    {
        private readonly IMongoCollection<Asistente> _asistentes;
        private readonly PasoService pasoService;
        public AsistenteService(ISysgedDatabaseSettings settings, PasoService pasoService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _asistentes = database.GetCollection<Asistente>("asistente");
            this.pasoService = pasoService;
        }
        public async Task<Asistente> GetAsistente(String idexpediente)
        {
            Asistente asistente = await _asistentes.Find(x => x.idexpediente == idexpediente).FirstAsync();
            return asistente;
        }

        public async Task<Asistente> Create(Asistente asistente)
        {
            Pasos pasos = await pasoService.GetPasoByNombreExpediente(asistente.pasos.nombreexpediente);

            asistente.pasos = new PasoAsistente {documentos = pasos.documentos, nombreexpediente = pasos.nombreexpediente };
            asistente.paso = 0;
            asistente.subpaso = 0;
            asistente.tipodocumento = "SolicitudInicial";

            await _asistentes.InsertOneAsync(asistente);

            return asistente;
        }

        public async Task<Asistente> UpdateSolicitudInicial(Asistente asistente, String nombreexpediente)
        {
            Pasos pasos = await pasoService.GetPasoByNombreExpediente(nombreexpediente);

            pasos.documentos.Find(x => x.tipo == asistente.tipodocumento)
                .pasos.Find(x => x.indice == asistente.paso - 1).fechainicio = asistente.pasos.documentos.ElementAt(0).pasos.ElementAt(0).fechainicio;

            pasos.documentos.Find(x => x.tipo == asistente.tipodocumento)
                .pasos.Find(x => x.indice == asistente.paso - 1).fechafin = asistente.pasos.documentos.ElementAt(0).pasos.ElementAt(0).fechafin;

                pasos.documentos.Find(x => x.tipo == asistente.tipodocumento)
                .pasos.Find(x => x.indice == asistente.paso - 1).fechalimite = asistente.pasos.documentos.ElementAt(0).pasos.ElementAt(0).fechalimite;

            FilterDefinition<Asistente> queryUpdate = Builders<Asistente>.Filter.Eq("idexpediente", asistente.idexpediente);

            UpdateDefinition<Asistente> updateAsistente = Builders<Asistente>.Update
                                                                                .Set("paso", asistente.paso)
                                                                                .Set("subpaso", asistente.subpaso)
                                                                                .Set("tipodocumento", asistente.tipodocumento)
                                                                                .Set("pasos", new PasoAsistente { documentos = pasos.documentos, nombreexpediente = pasos.nombreexpediente });
                                                                                

            Asistente asistenteActualizado = await _asistentes.FindOneAndUpdateAsync(queryUpdate, updateAsistente, new FindOneAndUpdateOptions<Asistente> 
            {
                ReturnDocument = ReturnDocument.After
            });

            return asistenteActualizado;

        }

        public async Task<Asistente> Update(PasoDTO pasoDTO)
        {
            FilterDefinition<Asistente> queryUpdate = Builders<Asistente>.Filter.Eq("idexpediente", pasoDTO.idexpediente);

            UpdateDefinition<Asistente> updateAsistente = Builders<Asistente>.Update
                                                                                .Set("paso", pasoDTO.paso)
                                                                                .Set("subpaso", pasoDTO.subpaso)
                                                                                .Set("tipodocumento", pasoDTO.tipodocumento)
                                                                                .Set("pasos.documentos.$[doc].pasos.$[pas].fechainicio", pasoDTO.fechainicio)
                                                                                .Set("pasos.documentos.$[doc].pasos.$[pas].fechalimite", pasoDTO.fechalimite);


            var arrayFilter = new List<ArrayFilterDefinition>();

            arrayFilter.Add(new BsonDocumentArrayFilterDefinition<Asistente>(new BsonDocument("doc.tipo", pasoDTO.tipodocumento)));
            arrayFilter.Add(new BsonDocumentArrayFilterDefinition<Asistente>(new BsonDocument("pas.indice", pasoDTO.paso)));

            Asistente asistenteActualizado = await _asistentes.FindOneAndUpdateAsync(queryUpdate, updateAsistente, new FindOneAndUpdateOptions<Asistente>
            {
                ReturnDocument = ReturnDocument.After,
                ArrayFilters = arrayFilter
            });

            return asistenteActualizado;
        }

        public async Task<Asistente> UpdateFinally(PasoDTO pasoDTO)
        {
            FilterDefinition<Asistente> queryUpdate = Builders<Asistente>.Filter.Eq("idexpediente", pasoDTO.idexpediente);

            UpdateDefinition<Asistente> updateAsistente = Builders<Asistente>.Update
                                                                                .Set("paso", pasoDTO.paso)
                                                                                .Set("subpaso", pasoDTO.subpaso)
                                                                                .Set("tipodocumento", pasoDTO.tipodocumento)
                                                                                .Set("pasos.documentos.$[doc].pasos.$[pas].fechafin", pasoDTO.fechafin);
            
            var arrayFilter = new List<ArrayFilterDefinition>();

            arrayFilter.Add(new BsonDocumentArrayFilterDefinition<Asistente>(new BsonDocument("doc.tipo", pasoDTO.tipodocumento)));
            arrayFilter.Add(new BsonDocumentArrayFilterDefinition<Asistente>(new BsonDocument("pas.indice", pasoDTO.pasoantiguo)));

            Asistente asistenteActualizado = await _asistentes.FindOneAndUpdateAsync(queryUpdate, updateAsistente, new FindOneAndUpdateOptions<Asistente>
            {
                ReturnDocument = ReturnDocument.After,
                ArrayFilters = arrayFilter
            });

            return asistenteActualizado;
        }

        public async Task<Asistente> UpdateNormal(PasoDTO pasoDTO)
        {
            FilterDefinition<Asistente> queryUpdate = Builders<Asistente>.Filter.Eq("idexpediente", pasoDTO.idexpediente);

            UpdateDefinition<Asistente> updateAsistente = Builders<Asistente>.Update
                                                                                .Set("paso", pasoDTO.paso)
                                                                                .Set("subpaso", pasoDTO.subpaso)
                                                                                .Set("tipodocumento", pasoDTO.tipodocumento);

            Asistente asistenteActualizado = await _asistentes.FindOneAndUpdateAsync(queryUpdate, updateAsistente, new FindOneAndUpdateOptions<Asistente>
            {
                ReturnDocument = ReturnDocument.After,
            });

            return asistenteActualizado;
        }

    }
}
