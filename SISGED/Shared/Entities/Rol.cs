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
        [BsonElement("label")]
        public string label { get; set; }
        [BsonElement("listaherramientas")]
        public List<String> listaherramientas { get; set; } = new List<String>();
        [BsonElement("listainterfaces")]
        public List<String> listainterfaces { get; set; } = new List<String>();
        [BsonElement("descripcion")]
        public string descripcion { get; set; }
    }
}
