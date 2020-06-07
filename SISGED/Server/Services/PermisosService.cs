using MongoDB.Driver;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class PermisosService
    {
        private readonly IMongoCollection<Permiso> _permisos;

        public PermisosService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _permisos = database.GetCollection<Permiso>("permisos");
        }

        public Permiso GetById(string id)
        {
            Permiso permiso = new Permiso();
            permiso = _permisos.Find(perm => perm.id == id).FirstOrDefault();
            return permiso;
        }
    }
}
