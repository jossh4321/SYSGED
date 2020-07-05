using MongoDB.Driver;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class NotificacionService
    {
        private readonly IMongoCollection<Notificacion> _notificaciones;
        public NotificacionService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _notificaciones = database.GetCollection<Notificacion>("notificaciones");

        }
        public List<Notificacion> Get()
        {
            return _notificaciones.Find<Notificacion>(notifiacion => true).ToList();
        }
    }
}
