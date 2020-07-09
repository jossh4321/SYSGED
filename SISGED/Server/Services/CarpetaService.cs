using MongoDB.Driver;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class CarpetaService
    {
        private readonly IMongoCollection<Carpeta> _carpeta;

        public CarpetaService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _carpeta = database.GetCollection<Carpeta>("carpetas");
        }

        public List<Carpeta> Get()
        {
            return _carpeta.Find(carpeta => true).ToList();
        }
    }
}
