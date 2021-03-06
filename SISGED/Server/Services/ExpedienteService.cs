﻿using MongoDB.Bson;
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
        public ExpedienteBandejaDTO registrarDerivacion(Expediente expediente, string userId)
        {

            Derivacion derivacion = new Derivacion();
            derivacion = expediente.derivaciones.FirstOrDefault();
            derivacion.usuarioreceptor = userId;

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
            proceso.fecharecepcion = DateTime.Now;
            proceso.idemisor = derivacion.usuarioemisor;
            proceso.idreceptor = userId;
	
            UpdateDefinition<Documento> updateDocumento = Builders<Documento>.Update.Push("historialproceso", proceso);
            _documentos.UpdateOne(doc => doc.id == bandejaDocumento.iddocumento, updateDocumento);

            ExpedienteBandejaDTO ex = new ExpedienteBandejaDTO();
            ex = obtenerExpedienteBandeja(expediente.id);



            return ex;
        }


        public ExpedienteBandejaDTO obtenerExpedienteBandeja(string idexpediente)
        {
            BsonArray embebedpipeline = new BsonArray();
            var match = new BsonDocument("$match",
                        new BsonDocument("_id",
                        new ObjectId(idexpediente)));

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
                Add("as", "documentosobj"));


            var group = new BsonDocument("$group",
                        new BsonDocument
                            {
                                { "_id", "$_id" },
                                { "tipo",
                        new BsonDocument("$first", "$tipo") },
                                { "cliente",
                        new BsonDocument("$first", "$cliente") },
                                { "documentosobj",
                        new BsonDocument("$push", "$documentosobj") }
                            });

            var project = new BsonDocument("$project",
                            new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "idexpediente", "$_id" },
                                    { "tipo", "$tipo" },
                                    { "cliente", "$cliente" },
                                    { "documentosobj", "$documentosobj" },
                                    { "documento",
                            new BsonDocument("$arrayElemAt",
                            new BsonArray
                                        {
                                            "$documentosobj",
                                            -1
                                        }) }
                                });


            ExpedienteBandejaDTO listaexpedientesdto = new ExpedienteBandejaDTO();
            listaexpedientesdto = _expedientes.Aggregate()
                .AppendStage<Expediente>(match)
                .Unwind<Expediente, Expedientedoc>(x => x.documentos)
                .AppendStage<Expedientedoc_lookup>(lookup)
                .Unwind<Expedientedoc_lookup, Expedientedoc_lookup_ur>(x => x.documentosobj)
                .AppendStage<Expedientedoc_group>(group)
                .AppendStage<ExpedienteBandejaDTO>(project).SingleOrDefault();
            
            return listaexpedientesdto;



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

        public Expediente updateExpedientBySolicitudInitial(Expediente expediente)
        {

            var updateExpediente = Builders<Expediente>.Update
                                                            .Set("tipo", expediente.tipo)
                                                            .Push("documentos", expediente.documentos.ElementAt(0));

            var queryExpediente = Builders<Expediente>.Filter.Eq("id", expediente.id);

            return _expedientes.FindOneAndUpdate(queryExpediente, updateExpediente, new FindOneAndUpdateOptions<Expediente> 
            {
                ReturnDocument = ReturnDocument.After
            });
        }

        public async Task<List<Expediente_group>> listaexpedientegantt(string dnicliente)
        {
            var match1 = new BsonDocument("$match",
             new BsonDocument("cliente.numerodocumento", dnicliente));

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

            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id", "$_id" },
                                    { "tipo",
                            new BsonDocument("$first", "$tipo") },
                                    { "cliente",
                            new BsonDocument("$first", "$cliente") },
                                    { "fechainicio",
                            new BsonDocument("$first", "$fechainicio") },
                                    { "fechafin",
                            new BsonDocument("$first", "$fechafin") },
                                    { "documentos",
                            new BsonDocument("$push", "$documentos") },
                                    { "derivaciones",
                            new BsonDocument("$first", "$derivaciones") },
                                    { "estado",
                            new BsonDocument("$first", "$estado") },
                                    { "documentoobj",
                            new BsonDocument("$push", "$documentoobj") }
                                });


            List<Expediente_group> estadisticas = new List<Expediente_group>();
            estadisticas = await _expedientes.Aggregate()
                .AppendStage<Expediente>(match1)
                .Unwind<Expediente, Expediente_unwind1>(x => x.documentos)
                .AppendStage<Expediente_lookup>(lookup)
                .Unwind<Expediente_lookup, Expediente_unwind2>(x => x.documentoobj)
                .AppendStage<Expediente_group>(group)
                .ToListAsync();
            return estadisticas;
        }

        public async Task<List<ExpedienteDTO_group1>> estadisticaGantt(string dni)
        {
            var match1 = new BsonDocument("$match",
                                new BsonDocument("cliente.numerodocumento", dni));
            var lookup = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "documentos" },
                                        { "let",
                                new BsonDocument("iddoc", "$documentos.iddocumento") },
                                        { "pipeline",
                                new BsonArray
                                        {
                                            new BsonDocument("$match",
                                            new BsonDocument("$expr",
                                            new BsonDocument("$eq",
                                            new BsonArray
                                                        {
                                                            "$_id",
                                                            new BsonDocument("$toObjectId", "$$iddoc")
                                                        })))
                                        } },
                                        { "as", "documentosobj" }
                                    });

            var group = new BsonDocument("$group",
                            new BsonDocument
                                {
                                    { "_id", "$_id" },
                                    { "tipo",
                            new BsonDocument("$first", "$tipo") },
                                    { "cliente",
                            new BsonDocument("$first", "$cliente") },
                                    { "fechainicio",
                            new BsonDocument("$first", "$fechainicio") },
                                    { "fechafin",
                            new BsonDocument("$first", "$fechafin") },
                                    { "documentos",
                            new BsonDocument("$push", "$documentos") },
                                    { "derivaciones",
                            new BsonDocument("$first", "$derivaciones") },
                                    { "estado",
                            new BsonDocument("$first", "$estado") },
                                    { "documentosobj",
                            new BsonDocument("$push", "$documentosobj") }
                                });

            List<ExpedienteDTO_group1> listaexpediente = new List<ExpedienteDTO_group1>();
            listaexpediente = _expedientes.Aggregate()
                                    .AppendStage<Expediente>(match1)
                                    .Unwind<Expediente, ExpedineteDTO_unwind1>(x => x.documentos)
                                    .AppendStage<ExpedienteDTO_lookup>(lookup)
                                    .Unwind<ExpedienteDTO_lookup, ExpedienteDTO_unwind2>(x => x.documentosobj)
                                    .AppendStage<ExpedienteDTO_group1>(group).ToList();
            return listaexpediente;
        }

        //agregacion gannt 2
        public async Task<List<expediente_project>> agregacioGantt2( string dni)
        {
            var match1 = new BsonDocument("$match",
                                new BsonDocument("cliente.numerodocumento", dni));

            var project = new BsonDocument("$project",
    new BsonDocument
        {
            { "_id", "$_id" },
            { "tipoexpediente", "$tipo" },
            { "iddocumento", "$documentos.iddocumento" },
            { "tipodocumento", "$documentos.tipo" },
            { "cliente", "$cliente" },
            { "fechacreacion", "$documentos.fechacreacion" },
            { "fechademora",
    new BsonDocument("$cond",
    new BsonArray
                {
                    new BsonDocument("$eq",
                    new BsonArray
                        {
                            "$documentos.fechademora",
                            BsonNull.Value
                        }),
                    "$documentos.fechaexceso",
                    "$documentos.fechademora"
                }) },
            { "fechaexceso", "$documentos.fechaexceso" },
            { "estado",
    new BsonDocument("$switch",
    new BsonDocument("branches",
    new BsonArray
                    {
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "pendiente"
                                }) },
                            { "then", "pendiente" }
                        },
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "creado"
                                }) },
                            { "then", "pendiente" }
                        },
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "modificado"
                                }) },
                            { "then", "pendiente" }
                        },
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "procesado"
                                }) },
                            { "then", "procesado" }
                        },
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "finalizado"
                                }) },
                            { "then", "procesado" }
                        },
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "revisado"
                                }) },
                            { "then", "procesado" }
                        },
                        new BsonDocument
                        {
                            { "case",
                        new BsonDocument("$eq",
                        new BsonArray
                                {
                                    "$documentoobj.estado",
                                    "caducado"
                                }) },
                            { "then", "caducado" }
                        }
                    })) }
        });

            var lookup = new BsonDocument("$lookup",
                    new BsonDocument
                        {
                            { "from", "documentos" },
                            { "let",
                    new BsonDocument("iddoc", "$documentos.iddocumento") },
                            { "pipeline",
                    new BsonArray
                            {
                                new BsonDocument("$match",
                                new BsonDocument("$expr",
                                new BsonDocument("$eq",
                                new BsonArray
                                            {
                                                "$_id",
                                                new BsonDocument("$toObjectId", "$$iddoc")
                                            })))
                            } },
                            { "as", "documentoobj" }
                        });


            List<expediente_project> listaexpediente = new List<expediente_project>();
            listaexpediente = await  _expedientes.Aggregate()
                                   .AppendStage<Expediente>(match1)
                                   .Unwind<Expediente, ExpedineteDTO_unwind1>(x => x.documentos)
                                   .AppendStage<ExpedineteDTO_lookup1g>(lookup)
                                   .Unwind<ExpedineteDTO_lookup1g, ExpedineteDTO_unwind2g>(x=> x.documentoobj)
                                   .AppendStage<expediente_project>(project).ToListAsync();
            return listaexpediente;
        }

        public Expediente GetById(string id)
        {
            Expediente expe = new Expediente();
            expe = _expedientes.Find(exped => exped.id == id).FirstOrDefault();
            return expe;
        }

    }
}
