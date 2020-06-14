using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class EscriturasPublicasService
    {
        private readonly IMongoCollection<EscrituraPublica> _escriturapublicas;
        private readonly IMongoCollection<Notario> _notarios;
        public EscriturasPublicasService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _escriturapublicas = database.GetCollection<EscrituraPublica>("escrituraspublicas");
            _notarios = database.GetCollection<Notario>("notarios");
        }

        public async Task<List<EscrituraPublicaRDTO>> obtenerEscriturasPublicas()
        {

            BsonArray subpipeline = new BsonArray();

            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray {  "$_id",new BsonDocument("$toObjectId","$$notariox") }
                        )
                    ))
                );
            var lookup = new BsonDocument("$lookup",
             new BsonDocument("from", "notarios")
               .Add("let", new BsonDocument("notariox", "$idnotario"))
               .Add("pipeline", subpipeline)
               .Add("as", "notario"));

            List<EscrituraPublicaRDTO> escrituraPublicas = new List<EscrituraPublicaRDTO>();

            escrituraPublicas =await  _escriturapublicas.Aggregate()
                .AppendStage<EscrituraPublicasDTO>(lookup)
                .Unwind<EscrituraPublicasDTO, EscrituraPublicaDTO>(p => p.notario)
                .Project(ep => new EscrituraPublicaRDTO
                {
                    id = ep.id,
                    direccionoficio = ep.direccionoficio,
                    idnotario = ep.idnotario,
                    actosjuridicos = ep.actosjuridicos,
                    fechaescriturapublica = ep.fechaescriturapublica,
                    url = ep.url,
                    estado = ep.estado,
                    notario = ep.notario.nombre + " " + ep.notario.apellido
                })
                .ToListAsync();

            return escrituraPublicas;
        }

        public List<EscrituraPublica> filter(string term)
        {
            string regex = "\\b" + term.ToLower() + ".*";
            var filter = Builders<EscrituraPublica>.Filter.Regex("titulo", new BsonRegularExpression(regex, "i"));
            return _escriturapublicas.Find(filter).ToList();
        }

        public UpdateResult updateEscrituraPublicaporConclusionFirma(EscrituraPublica ep)
        {
            var filter = Builders<EscrituraPublica>.Filter.Eq(escp => escp.id, ep.id);

            var update = Builders<EscrituraPublica>.Update.Set(escp => escp.estado, "concluido");

            var escrituraP = _escriturapublicas.UpdateOne(filter, update);
            return escrituraP;
        }
    }
}
