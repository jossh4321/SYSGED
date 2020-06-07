using MongoDB.Driver;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class NotarioService
    {
        private readonly IMongoCollection<Notario> _notarios;

        public NotarioService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _notarios = database.GetCollection<Notario>("notarios");
        }

        /*public List<Notario> Get()
        {
            return _personas.Find(persona => true).ToList();
        }*/

    }
}
