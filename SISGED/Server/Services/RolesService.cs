using MongoDB.Driver;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class RolesService
    {
        private readonly IMongoCollection<Rol> _roles;

        public RolesService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _roles = database.GetCollection<Rol>("roles");
        }

        public List<Rol> Get()
        {
           return  _roles.Find<Rol>(x => true).ToList();
        }
    }
}
