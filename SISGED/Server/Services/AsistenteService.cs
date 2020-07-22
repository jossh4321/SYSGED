using MongoDB.Driver;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class AsistenteService
    {
        private readonly IMongoCollection<Asistente> _asistentes;
        public AsistenteService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _asistentes = database.GetCollection<Asistente>("asistente");
        }
        public async Task<List<Asistente>> GetAsistentes()
        {
            List<Asistente> asistentes = await _asistentes.Find(x => true).ToListAsync();
            return asistentes;
        }
    }
}
