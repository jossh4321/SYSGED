using MongoDB.Bson;
using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using SISGED.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class ExpedienteService
    {
        private readonly IMongoCollection<Expediente> _expedientes;
        private readonly IMongoCollection<Bandeja> _bandejas;
        private readonly IMongoCollection<Documento> _documentos;
        public ExpedienteService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _expedientes = database.GetCollection<Expediente>("expedientes");
            _bandejas = database.GetCollection<Bandeja>("bandejas");
            _documentos = database.GetCollection<Documento>("documentos");

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
        public Expediente registrarDerivacion(Expediente expediente, string userId)
        {

            Derivacion derivacion = new Derivacion();
            derivacion = expediente.derivaciones.FirstOrDefault();

            var filter = Builders<Expediente>.Filter.Eq(exp => exp.id, expediente.id);
            var update = Builders<Expediente>.Update.Push("derivaciones", derivacion);
            _expedientes.UpdateOne(filter, update);

            BandejaDocumento bandejaDocumento = new BandejaDocumento();
            bandejaDocumento.idexpediente = expediente.id;
            bandejaDocumento.iddocumento = expediente.documentos.Last().iddocumento;

            UpdateDefinition<Bandeja> updateBandeja = Builders<Bandeja>.Update.Push("bandejaentrada", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == userId, updateBandeja);

            UpdateDefinition<Bandeja> updateBandejaS = Builders<Bandeja>.Update.Pull("bandejasalida", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == derivacion.usuarioemisor, updateBandejaS);

            UpdateDefinition<Bandeja> updateBandejaE = Builders<Bandeja>.Update.Pull("bandejaentrada", bandejaDocumento);
            _bandejas.UpdateOne(band => band.usuario == derivacion.usuarioemisor, updateBandejaE);

            Proceso proceso = new Proceso();
            proceso.area = derivacion.areaprocedencia;
            proceso.fechaemision = DateTime.Now;
            proceso.idusuario = userId;
	
            UpdateDefinition<Documento> updateDocumento = Builders<Documento>.Update.Push("historialproceso", proceso);
            _documentos.UpdateOne(doc => doc.id == bandejaDocumento.iddocumento, updateDocumento);

            return expediente;
        }

        public ExpedienteDTO getbynestediddoc(string iddoc)
        {
            Expediente expediente = _expedientes.Find(exp => exp.documentos.Any(doc=>doc.iddocumento == iddoc)).FirstOrDefault();
            ExpedienteDTO dTO = new ExpedienteDTO
            {
                cliente = expediente.cliente,
                derivaciones = expediente.derivaciones,
                documentos = expediente.documentos,
                estado = expediente.estado,
                fechafin = expediente.fechafin,
                fechainicio = expediente.fechainicio,
                id = expediente.id,
                tipo = expediente.tipo
            };
            return dTO;
        }

        public ExpedienteDTO getById(string iddoc)
        {
            Expediente expediente = _expedientes.Find(exp => exp.id == iddoc).FirstOrDefault();
            ExpedienteDTO dTO = new ExpedienteDTO
            {
                cliente = expediente.cliente,
                derivaciones = expediente.derivaciones,
                documentos = expediente.documentos,
                estado = expediente.estado,
                fechafin = expediente.fechafin,
                fechainicio = expediente.fechainicio,
                id = expediente.id,
                tipo = expediente.tipo
            };
            return dTO;
        }

        public async Task<List<ExpedienteDTO>> filtrado(ParametrosBusquedaExpediente parametrosbusqueda)
        {
            BsonArray embebedpipeline = new BsonArray();
            embebedpipeline.Add(
                    new BsonDocument("$match", new BsonDocument(
                        "$expr", new BsonDocument(
                            "$eq", new BsonArray{ "$_id", new BsonDocument(
                                "$toObjectId", "$$iddoc")}
                            ))));
            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "documentos").
                Add("let", new BsonDocument("iddoc", "$documentos.iddocumento")).
                Add("pipeline", embebedpipeline).
                Add("as", "documentoobj"));

            var filtroDocumento = new BsonDocument();
            if (parametrosbusqueda.estado != null & parametrosbusqueda.estado != "")
            {
                filtroDocumento.Add("estado",
                                   new BsonDocument("$regex", parametrosbusqueda.estado + ".*")
                                   .Add("$options", "i"));
            }

            if (parametrosbusqueda.tipo != null & parametrosbusqueda.tipo != "")
            {
                filtroDocumento.Add("tipo",
                                   new BsonDocument("$regex", parametrosbusqueda.tipo + ".*")
                                   .Add("$options", "i"));
            }

            if (parametrosbusqueda.nombrecliente != null & parametrosbusqueda.nombrecliente != "")
            {
                filtroDocumento.Add("cliente.nombre",
                                    new BsonDocument("$regex", parametrosbusqueda.nombrecliente + ".*")
                                    .Add("$options", "i"));
            }


            List<ExpedienteDTO> expedientes = new List<ExpedienteDTO>();
            expedientes = await _expedientes.Aggregate()
                .Unwind<Expediente, ExpedienteDTO_ur1>(e => e.documentos)
                .AppendStage<ExpedienteDTO_look_up>(lookup)
                .Unwind<ExpedienteDTO_look_up, ExpedienteDTO_ur2>(p => p.documentoobj)
                .Group<ExpedienteDTO>(new BsonDocument
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
                })
                .Match(filtroDocumento)
                .ToListAsync();
            return expedientes;            
        }

    }
}
