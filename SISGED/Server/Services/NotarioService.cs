using MongoDB.Bson;
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

        public List<Notario> filter(string term)
        {
            string regex = "\\b"+term.ToLower() + ".*";
            var filter = Builders<Notario>.Filter.Regex("nombre", new BsonRegularExpression(regex, "i"));
            return _notarios.Find(filter).ToList();
        }
        public Notario GetById(string id)
        {
            Notario notario = new Notario();
            notario = _notarios.Find(notario => notario.id == id).FirstOrDefault();
            return notario;
        }

    }
}
