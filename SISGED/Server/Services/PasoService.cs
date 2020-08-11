using MongoDB.Driver;
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
    }
}
