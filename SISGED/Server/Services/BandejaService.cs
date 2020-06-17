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
    public class BandejaService
    {
        private readonly IMongoCollection<Bandeja> _bandejas;

        public BandejaService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _bandejas = database.GetCollection<Bandeja>("bandejas");
        }

        public async Task<Bandeja> ObtenerBandejaDocumento(string usuario)
        {
            Bandeja bandeja = await _bandejas.FindAsync(band => band.usuario == usuario).Result.FirstAsync();
            return bandeja;
        }


        public async Task<List<BandejaDTOR>> ObtenerBandeja(string usuario)
        {
            BsonArray subpipeline = new BsonArray();

            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$expedienteId") })
                    )
                ));

            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "expedientes")
                    .Add("let", new BsonDocument("expedienteId", "$bandejasalida.idexpediente"))
                    .Add("pipeline", subpipeline)
                    .Add("as", "bandejadocumento")
                );

            var condicionalFiltro = new BsonDocument("$eq", new BsonArray { "$$item.iddocumento", "$bandejasalida.iddocumento" });

            var filtro = new BsonDocument("$filter", new BsonDocument("input", "$bandejadocumento.documentos")
                                                     .Add("as", "item")
                                                     .Add("cond", condicionalFiltro));

            var declararVariable = new BsonDocument("$let", new BsonDocument("vars", new BsonDocument("documento", filtro))
                                                            .Add("in", new BsonDocument("$arrayElemAt", new BsonArray { "$$documento", 0 })));

            var project = new BsonDocument("$project",
                                  new BsonDocument("documento", declararVariable)
                                  .Add("bandejasalida", 1)
                                  .Add("tipoexpediente", "$bandejadocumento.tipo")
                                  .Add("cliente","$bandejadocumento.cliente"));
            List<BandejaDTOR> listabandejas = new List<BandejaDTOR>();
            var filtroUsuario = Builders<Bandeja>.Filter.Eq("usuario", usuario);
            listabandejas = await _bandejas.Aggregate()
                                        .Match(filtroUsuario)
                                        .Unwind<Bandeja, BandejaDTO>(b => b.bandejasalida)
                                        .AppendStage<BandejaDTODocumento>(lookup)
                                        .Unwind<BandejaDTODocumento, BandejaDTODocumentoExpediente>(b => b.bandejadocumento)
                                        .AppendStage<BandejaDTOR>(project)
                                        .ToListAsync();
            return listabandejas;
        }



        public async Task<List<BandejaEntradaDTOR>> ObtenerBandejaEntrada(string usuario)
        {
            BsonArray subpipeline = new BsonArray();

            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$expedienteId") })
                    )
                ));

            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "expedientes")
                    .Add("let", new BsonDocument("expedienteId", "$bandejaentrada.idexpediente"))
                    .Add("pipeline", subpipeline)
                    .Add("as", "bandejadocumento")
                );

            var condicionalFiltro = new BsonDocument("$eq", new BsonArray { "$$item.iddocumento", "$bandejaentrada.iddocumento" });

            var filtro = new BsonDocument("$filter", new BsonDocument("input", "$bandejadocumento.documentos")
                                                     .Add("as", "item")
                                                     .Add("cond", condicionalFiltro));

            var declararVariable = new BsonDocument("$let", new BsonDocument("vars", new BsonDocument("documento", filtro))
                                                            .Add("in", new BsonDocument("$arrayElemAt", new BsonArray { "$$documento", 0 })));

            var project = new BsonDocument("$project",
                                  new BsonDocument("documento", declararVariable)
                                  .Add("bandejaentrada", 1)
                                  .Add("tipoexpediente", "$bandejadocumento.tipo")
                                  .Add("cliente", "$bandejadocumento.cliente"));
            List<BandejaEntradaDTOR> listabandejas = new List<BandejaEntradaDTOR>();
            var filtroUsuario = Builders<Bandeja>.Filter.Eq("usuario", usuario);
            listabandejas = await _bandejas.Aggregate()
                                        .Match(filtroUsuario)
                                        .Unwind<Bandeja, BandejaEntradaDTO>(b => b.bandejaentrada)
                                        .AppendStage<BandejaEntradaDTODocumento>(lookup)
                                        .Unwind<BandejaEntradaDTODocumento, BandejaEntradaDTODocumentoExpediente>(b => b.bandejadocumento)
                                        .AppendStage<BandejaEntradaDTOR>(project)
                                        .ToListAsync();
            return listabandejas;
        }
    }
}
