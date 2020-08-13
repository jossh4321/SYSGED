using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class PasoService
    {
        private readonly IMongoCollection<Pasos> _pasos;
        public PasoService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _pasos = database.GetCollection<Pasos>("pasos");
        }

        public async Task<List<Pasos>> GetPasos()
        {
            List<Pasos> pasos = await _pasos.Find(x => true).ToListAsync();
            return pasos;
        }

        public async Task<Pasos> GetPasoByNombreExpediente(String nombreexpediente)
        {
            Pasos paso = await _pasos.Find(x => x.nombreexpediente == nombreexpediente).FirstAsync();
            return paso;
        }

        public async Task<Pasos> GetPasosById(String idpaso)
        {
            Pasos paso = await _pasos.Find(x => x.id == idpaso).FirstAsync();
            return paso;
        }
        public async Task<List<PasosDTO2>> GetPasosDTO2()
        {
            List<Pasos> pasos = await _pasos.Find(x => true).ToListAsync();
            List<PasosDTO2> pasosDTO2s = new List<PasosDTO2>();
            pasosDTO2s = (List<PasosDTO2>)pasos.Select(x => new PasosDTO2()
            {
                id= x.id,
                nombreexpediente = x.nombreexpediente,
                documentos = (List<DocumentoPasoDTO2>)x.documentos.Select((a,b) => new DocumentoPasoDTO2() { 
                    uid = generateUID(),
                    indice = b,
                    tipo = a.tipo,
                    pasos = (List<PasoDocDTO>)a.pasos.Select((c,d) => new PasoDocDTO()
                    {
                        uid = generateUID(),
                        indice=c.indice,
                        nombre=c.nombre,
                        descripcion=c.descripcion,
                        dias=c.dias,
                        subpaso = (List<SubPaso>)c.subpaso.Select((e,f) => new SubPaso()
                        {
                            indice = e.indice,
                            descripcion=e.descripcion
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList();
            return pasosDTO2s;
        }

        public async Task<PasosDTO2> registrarPaso(PasosDTO2 pasosdto2)
        {
            Pasos pasos = new Pasos()
            {
                nombreexpediente = pasosdto2.nombreexpediente,
                documentos = pasosdto2.documentos.Select(x => new DocumentoPaso()
                {
                    tipo = x.tipo,
                    pasos = (List<Paso>)x.pasos.Select((a,b) => new Paso()
                    {
                        indice=b,
                        nombre = a.nombre,
                        descripcion = a.descripcion,
                        dias = a.dias,
                        subpaso = (List<SubPaso>)a.subpaso.Select((c,d) => new SubPaso()
                        {
                            indice = d,
                            descripcion = c.descripcion
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            _pasos.InsertOne(pasos);
            pasosdto2.id = pasos.id;
            return pasosdto2;
        }
        public async Task<PasosDTO2> modificarpaso(PasosDTO2 pasosdto2)
        {
            Pasos pasos = new Pasos()
            {
                id= pasosdto2.id,
                nombreexpediente = pasosdto2.nombreexpediente,
                documentos = pasosdto2.documentos.Select(x => new DocumentoPaso()
                {
                    tipo = x.tipo,
                    pasos = (List<Paso>)x.pasos.Select((a, b) => new Paso()
                    {
                        indice = b,
                        nombre = a.nombre,
                        descripcion = a.descripcion,
                        dias = a.dias,
                        subpaso = (List<SubPaso>)a.subpaso.Select((c, d) => new SubPaso()
                        {
                            indice = d,
                            descripcion = c.descripcion
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            var filter = Builders<Pasos>.Filter.Eq("id", pasos.id);
            var update = Builders<Pasos>.Update
                .Set("nombreexpediente", pasos.nombreexpediente)
                .Set("documentos", pasos.documentos);  
             _pasos.FindOneAndUpdate<Pasos>(filter, update);
            return pasosdto2;
        }
        public string generateUID()
        {
            return Guid.NewGuid().ToString("N");
        }

    }
}
