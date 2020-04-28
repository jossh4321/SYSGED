using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Rol
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("listamenu")]
        public List<Permiso> listamenu { get; set; } = new List<Permiso>();
        [BsonElement("descripcion")]
        public string descripcion { get; set; }
    }
}
