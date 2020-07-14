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
    public class NotificacionService
    {
        private readonly IMongoCollection<Notificacion> _notificaciones;
        public NotificacionService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _notificaciones = database.GetCollection<Notificacion>("notificaciones");

        }
        public List<Notificacion> Get()
        {
            return _notificaciones.Find<Notificacion>(notifiacion => true).ToList();
        }

        public List<NotificacionDTO> obtenernotificacion()
        {

            var match = new BsonDocument("$match",
                    new BsonDocument("estado", "novisto"));
            //Agregacion 1
            BsonArray subpipeline1 = new BsonArray();
            subpipeline1.Add(
                new BsonDocument("$match", new BsonDocument(
                        "$expr", new BsonDocument(
                            "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idusu") }
                            )
                    ))
                );
            var aggregation1 = new BsonDocument("$lookup",
                               new BsonDocument("from", "usuarios")
                               .Add("let", new BsonDocument("idusu", "$idreceptor"))
                               .Add("pipeline", subpipeline1)
                               .Add("as", "receptor"));

            //Agregacion 2
            BsonArray subpipeline2 = new BsonArray();
            subpipeline2.Add(
                new BsonDocument("$match", new BsonDocument(
                        "$expr", new BsonDocument(
                            "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idusu") }
                            )
                    ))
                );
            var aggregation2 = new BsonDocument("$lookup",
                               new BsonDocument("from", "usuarios")
                               .Add("let", new BsonDocument("idusu", "$idemisor"))
                               .Add("pipeline", subpipeline2)
                               .Add("as", "emisor"));
            //Agregacion 3
            BsonArray subpipeline3 = new BsonArray();
            subpipeline3.Add(
                new BsonDocument("$match", new BsonDocument(
                        "$expr", new BsonDocument(
                            "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$iddoc") }
                            )
                    ))
                );
            var aggregation3 = new BsonDocument("$lookup",
                               new BsonDocument("from", "documentos")
                               .Add("let", new BsonDocument("iddoc", "$iddocumento"))
                               .Add("pipeline", subpipeline3)
                               .Add("as", "documento"));

            //projeccion 1
            var project = new BsonDocument("$project",
                new BsonDocument {
                    { "_id","$_id" },
                    { "titulo","$titulo"},
                    { "cuerpo","$cuerpo"},
                    { "estado","$estado"},
                    { "fechaemision","$fechaemision"},
                    { "idemisor", new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$emisor", 0 }) },
                    { "idreceptor", new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$receptor", 0 }) },
                    { "iddocumento", new BsonDocument("$arrayElemAt",
                                new BsonArray{ "$documento", 0 }) }
    });


            List<NotificacionDTO> notificacion = new List<NotificacionDTO>();
            notificacion = _notificaciones.Aggregate()
                .AppendStage<Notificacion>(match)
                .AppendStage<Notificacion_lookup1>(aggregation1)
                .AppendStage<Notificacion_lookup2>(aggregation2)
                .AppendStage<Notificacion_lookup3>(aggregation3)
                .AppendStage< NotificacionDTO>(project).ToList();

            return notificacion;
        }

           }
}
