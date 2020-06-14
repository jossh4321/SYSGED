using MongoDB.Driver;
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

        public async Task<Bandeja> ObtenerBandejaDocumento(Usuario usuario)
        {
            Bandeja bandeja = await _bandejas.FindAsync(band => band.usuario == usuario.id).Result.FirstAsync();
            return bandeja;
        }
    }
}
