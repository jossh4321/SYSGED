using MongoDB.Bson;
using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class ExpedienteService
    {
        private readonly IMongoCollection<Expediente> _expedientes;
        public ExpedienteService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _expedientes = database.GetCollection<Expediente>("expedientes");
        }
        public  Expediente saveExpediente(Expediente expediente)
        {
            _expedientes.InsertOne(expediente);
            return expediente;
        }

        public async Task<List<ExpedienteDTO2>> getAllExpedienteDTO()
        {

            BsonArray embebedpipeline = new BsonArray();
            embebedpipeline.Add(
                    new BsonDocument("$match",new BsonDocument(
                        "$expr",new BsonDocument(
                            "$eq",new BsonArray{ "$_id", new BsonDocument(
                                "$toObjectId", "$$iddoc")}
                            ))));
            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "documentos").
                Add("let", new BsonDocument("iddoc", "$documentos.iddocumento")).
                Add("pipeline",embebedpipeline).
                Add("as","documentoobj"));
          List<ExpedienteDTO2> listaexpedientesdto = new List<ExpedienteDTO2>();
            listaexpedientesdto = await _expedientes.Aggregate()
                .Unwind<Expediente, ExpedienteDTO_ur1>(e => e.documentos)
                .AppendStage<ExpedienteDTO_look_up>(lookup)
                .Unwind<ExpedienteDTO_look_up, ExpedienteDTO_ur2>(p => p.documentoobj)
                .Group<ExpedienteDTO2>(new BsonDocument
                {
                    { "_id", "$_id"},
                    {
                        "tipo", new BsonDocument
                        {
                            {"$first", "$tipo"}
                        }
                    },
                    { "cliente", new BsonDocument{ { "$first", "$cliente" } } },
                    { "fechainicio", new BsonDocument{ { "$first", "$fechainicio" } } },
                    { "fechafin", new BsonDocument{ { "$first", "$fechafin" } } },
                    {"documentos", new BsonDocument{ { "$push", "$documentos" } } },
                    {"documentosobj", new BsonDocument{ { "$push", "$documentoobj" } } },
                    { "derivaciones", new BsonDocument{ { "$first", "$derivaciones" } } },
                    { "estado", new BsonDocument{ { "$first", "$estado" } } }
                }).ToListAsync();
            return listaexpedientesdto;
        }

        public List<Expediente> getAllExpediente()
        {
            List<Expediente> expedientes = new List<Expediente>();
            expedientes = _expedientes.Find(expediente => true).ToList();
            return expedientes;
        }
    }
}
