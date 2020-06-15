using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Permiso
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("nombre")]
        public String nombre { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("label")]
        public string label { get; set; }
        [BsonElement("icono")]
        public string icono { get; set; }
        [BsonElement("url")]
        public string url { get; set; }
    }
}
