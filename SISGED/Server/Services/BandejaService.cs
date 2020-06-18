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


        public async Task<BandejaESDTOR> ObtenerBandeja(string usuario)
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

            /* LookUp para los documentos */
            BsonArray subpipeline2 = new BsonArray();

            subpipeline2.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                         "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$documentoId") })
                        )
                    ));

            var lookup2 = new BsonDocument("$lookup",
                new BsonDocument("from", "documentos")
                    .Add("let", new BsonDocument("documentoId", "$bandejadocumento.documentos.iddocumento"))
                    .Add("pipeline", subpipeline2)
                    .Add("as", "documentosobj")
                    );

            /* Group para agrupar los documentos en un array */

            var group = new BsonDocument("_id", "$bandejadocumento._id")
                .Add("cliente", new BsonDocument("$first", "$bandejadocumento.cliente"))
                .Add("tipo", new BsonDocument("$first", "$bandejadocumento.tipo"))
                .Add("documentosobj", new BsonDocument("$push", "$documentosobj"))
                .Add("bandejasalida", new BsonDocument("$first", "$bandejasalida"))
                .Add("bandejaentrada", new BsonDocument("$first", "$bandejaentrada"))
                .Add("idbandeja", new BsonDocument("$first", "$_id"));


            /* Proyección para devolver los datos que se necesitan */

            var condicionalFiltro = new BsonDocument("$eq", new BsonArray { "$$item._id", new BsonDocument("$toObjectId", "$bandejasalida.iddocumento") });

            var filtro = new BsonDocument("$filter", new BsonDocument("input", "$documentosobj")
                                                     .Add("as", "item")
                                                     .Add("cond", condicionalFiltro));

            var declararVariable = new BsonDocument("$let", new BsonDocument("vars", new BsonDocument("documento", filtro))
                                                            .Add("in", new BsonDocument("$arrayElemAt", new BsonArray { "$$documento", 0 })));




            var project = new BsonDocument("$project",
                                  new BsonDocument("documento", declararVariable)
                                  .Add("documentosobj", "$documentosobj")
                                  .Add("tipo", "$tipo")
                                  .Add("_id", "$_id")
                                  .Add("cliente", "$cliente")
                                  .Add("bandejaentrada", 1)
                                  .Add("idbandeja", 1));



            /* Proyección para crear el atributo de expediente salida*/
            var project1 = new BsonDocument("$project",
                                    new BsonDocument("_id", 0)
                                    .Add("idbandeja",1)
                                    .Add("bandejaentrada",1)
                                    .Add("expedientesalida", 
                                            new BsonDocument("idexpediente","$_id")
                                            .Add("cliente","$cliente")
                                            .Add("tipo","$tipo")
                                            .Add("documentosobj","$documentosobj")
                                            .Add("documento","$documento")));

            /* Group para unificar todos los expedientes de la bandeja de salida*/
            var group1 = new BsonDocument("_id", "$idbandeja")
                .Add("bandejaentrada", new BsonDocument("$first", "$bandejaentrada"))
                .Add("expedientesalida", new BsonDocument("$push", "$expedientesalida"));


            /* Lookup para los expedientes de la bandeja de entrada*/
            BsonArray subpipelineE = new BsonArray();

            subpipelineE.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$expedienteId") })
                    )
                ));

            var lookupe = new BsonDocument("$lookup",
                new BsonDocument("from", "expedientes")
                    .Add("let", new BsonDocument("expedienteId", "$bandejaentrada.idexpediente"))
                    .Add("pipeline", subpipelineE)
                    .Add("as", "bandejadocumento")
                );


            /* Group para la bandeja de entrada y agrupos los elementos en un array*/
            var groupe = new BsonDocument("_id", "$bandejadocumento._id")
                .Add("cliente", new BsonDocument("$first", "$bandejadocumento.cliente"))
                .Add("tipo", new BsonDocument("$first", "$bandejadocumento.tipo"))
                .Add("documentosobj", new BsonDocument("$push", "$documentosobj"))
                .Add("expedientesalida", new BsonDocument("$first", "$expedientesalida"))
                .Add("bandejaentrada", new BsonDocument("$first", "$bandejaentrada"))
                .Add("idbandeja", new BsonDocument("$first", "$_id"));


            /* Proyección para encontrar el documento de entrada */

            var condicionalFiltroE = new BsonDocument("$eq", new BsonArray { "$$item._id", new BsonDocument("$toObjectId", "$bandejaentrada.iddocumento") });

            var filtroE = new BsonDocument("$filter", new BsonDocument("input", "$documentosobj")
                                                     .Add("as", "item")
                                                     .Add("cond", condicionalFiltroE));

            var declararVariableE = new BsonDocument("$let", new BsonDocument("vars", new BsonDocument("documento", filtroE))
                                                            .Add("in", new BsonDocument("$arrayElemAt", new BsonArray { "$$documento", 0 })));




            var projecte = new BsonDocument("$project",
                                  new BsonDocument("documento", declararVariableE)
                                  .Add("documentosobj", "$documentosobj")
                                  .Add("tipo", "$tipo")
                                  .Add("_id", "$_id")
                                  .Add("cliente", "$cliente")
                                  .Add("bandejaentrada", 1)
                                  .Add("idbandeja", 1)
                                  .Add("expedientesalida",1));



            /* Proyección para crear el expedienteentrada*/
            var project1e = new BsonDocument("$project",
                                   new BsonDocument("_id", 0)
                                   .Add("idbandeja", 1)
                                   .Add("expedientesalida", 1)
                                   .Add("expedienteentrada",
                                           new BsonDocument("idexpediente", "$_id")
                                           .Add("cliente", "$cliente")
                                           .Add("tipo", "$tipo")
                                           .Add("documentosobj", "$documentosobj")
                                           .Add("documento", "$documento")));


            /* Group final para la unión de los expedientes */
            var groupfinal = new BsonDocument("_id", "$idbandeja")
               .Add("expedienteentrada", new BsonDocument("$push", "$expedienteentrada"))
               .Add("expedientesalida", new BsonDocument("$first", "$expedientesalida"));

            var filtroUsuario = Builders<Bandeja>.Filter.Eq("usuario", usuario);

            BandejaESDTOR listabandejas = new BandejaESDTOR();
            listabandejas = await _bandejas.Aggregate()
                                        .Match(filtroUsuario)
                                        .Unwind<Bandeja, BandejaDTO>(b => b.bandejasalida)
                                        .AppendStage<BandejaDTODocumento>(lookup)
                                        .Unwind<BandejaDTODocumento, BandejaDTODocumentoExpediente>(b => b.bandejadocumento)
                                        .Unwind<BandejaDTODocumentoExpediente, BandejaExpedienteDTO>(be => be.bandejadocumento.documentos)
                                        .AppendStage<BandejaExpDocDTO>(lookup2)
                                        .Unwind<BandejaExpDocDTO,BandejaExpDocUndDTO>(bed => bed.documentosobj)
                                        .Group<BandejaExpDocGroupDTO>(group)
                                        .AppendStage<BandejaDTOR>(project)
                                        .AppendStage<BandejaRPDTO>(project1)
                                        .Group<BandejaRV1DTO>(group1)
                                        .Unwind<BandejaRV1DTO,BandejaDTOE>(be=>be.bandejaentrada)
                                        .AppendStage<BandejaDTOEDocumento>(lookupe)
                                        .Unwind<BandejaDTOEDocumento,BandejaDTOEDocumentoExpediente>(b => b.bandejadocumento)
                                        .Unwind<BandejaDTOEDocumentoExpediente, BandejaExpedienteDTOE>(bee => bee.bandejadocumento.documentos)
                                        .AppendStage<BandejaExpDocDTOE>(lookup2)
                                        .Unwind<BandejaExpDocDTOE, BandejaExpDocUndDTOE>(bed => bed.documentosobj)
                                        .Group<BandejaExpDocGroupDTOE>(groupe)
                                        .AppendStage<BandejaESDTO>(projecte)
                                        .AppendStage<BandejaESDTOP>(project1e)
                                        .Group<BandejaESDTOR>(groupfinal)
                                        .FirstAsync();
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
