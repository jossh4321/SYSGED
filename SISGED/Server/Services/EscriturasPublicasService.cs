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

        public async Task<List<EscrituraPublicaRDTO>> obtenerEscriturasPublicas()
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

            // Primero filtro para la direccion oficio notarial
            var regexOficioNotarial = parametrosbusqueda.direccionoficionotarial + ".*";
            var filterRegexOficio = Builders<EscrituraPublicaRDTO>.Filter.Regex("direccionoficio", new BsonRegularExpression(regexOficioNotarial, "i"));

            // Segundo filtro del nombre del notario
            var regexnombreNotario = parametrosbusqueda.nombrenotario + ".*";
            var filterRegexNotario = Builders<EscrituraPublicaRDTO>.Filter.Regex("notario", new BsonRegularExpression(regexnombreNotario, "i"));

            // Tercer filtro de los actos jurídicos
            var regexactojuridico = parametrosbusqueda.actojuridico + ".*";
            var filterRegexActo = Builders<EscrituraPublicaRDTO>.Filter.Regex("actosjuridicos.titulo", new BsonRegularExpression(regexactojuridico, "i"));

            //Filtro final de los otorgantes
            var listaOtorgantesRegex = parametrosbusqueda.nombreotorgantes.Select(o => new Regex(o + ".*")).ToList();
            var matchInRegexOtorgante = new BsonDocument("actosjuridicos.otorgantes.nombre",
                                                    new BsonDocument("$in", new BsonArray().AddRange(listaOtorgantesRegex)));
            
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
                .Match(filterRegexOficio)
                .Match(filterRegexNotario)
                .Match(filterRegexActo)
                .Match(matchInRegexOtorgante)
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
