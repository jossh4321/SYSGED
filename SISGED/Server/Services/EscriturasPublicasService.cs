using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using SISGED.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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

        public async Task<List<EscrituraPublicaRDTO>> filtradoEspecial(ParametrosBusquedaEscrituraPublica parametrosbusqueda)
        {
            BsonArray subpipeline = new BsonArray();

            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$notariox") }
                        )
                    ))
                );
            var lookup = new BsonDocument("$lookup",
             new BsonDocument("from", "notarios")
               .Add("let", new BsonDocument("notariox", "$idnotario"))
               .Add("pipeline", subpipeline)
               .Add("as", "notario"));

            //Creación del BsonDocument del filtros de busqueda
            var filtroDocumento = new BsonDocument();
            if (parametrosbusqueda.direccionoficionotarial != null & parametrosbusqueda.direccionoficionotarial != "")
            {
                filtroDocumento.Add("direccionoficio",
                                   new BsonDocument("$regex", parametrosbusqueda.direccionoficionotarial + ".*")
                                   .Add("$options", "i"));
            }
            if (parametrosbusqueda.nombrenotario != null & parametrosbusqueda.nombrenotario != null)
            {
                filtroDocumento.Add("notario",
                                    new BsonDocument("$regex", parametrosbusqueda.nombrenotario + ".*")
                                    .Add("$options", "i"));
            }
            if (parametrosbusqueda.actojuridico != null & parametrosbusqueda.actojuridico != "")
            {
                filtroDocumento.Add("actosjuridicos.titulo",
                                    new BsonDocument("$regex", parametrosbusqueda.actojuridico + ".*")
                                    .Add("$options", "i"));
            }
            if (parametrosbusqueda.nombreotorgantes != null)
            {
                if (parametrosbusqueda.nombreotorgantes.Count != 0)
                {
                    var listaOtorgantesRegex = parametrosbusqueda.nombreotorgantes.Select(o => new Regex(o + ".*")).ToList();
                    filtroDocumento.Add("actosjuridicos.otorgantes.nombre",
                                        new BsonDocument("$in", new BsonArray().AddRange(listaOtorgantesRegex)));
                }

            }


            List<EscrituraPublicaRDTO> escrituraPublicas = new List<EscrituraPublicaRDTO>();

            escrituraPublicas = await _escriturapublicas.Aggregate()
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
                    notario = ep.notario.nombre + " " + ep.notario.apellido,
                    titulo = ep.titulo
                })
                .Match(filtroDocumento)
                .ToListAsync();

            return escrituraPublicas;
        }
        public EscrituraPublica GetById(string id)
        {
            EscrituraPublica escrituraPublica = new EscrituraPublica();
            escrituraPublica = _escriturapublicas.Find(escritura => escritura.id == id).FirstOrDefault();
            return escrituraPublica;
        }
    }
}
