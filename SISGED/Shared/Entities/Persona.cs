using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISGED.Shared.Entities
{
     public class Persona
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("apellido")]
        public string apellido { get; set; }
        [BsonElement("edad")]
        public Int32 edad { get; set; }
        [BsonElement("tipodocumento")]
        public string tipodocumento { get; set; }
        [BsonElement("numerodocumento")]
        public string numerodocumento { get; set; }
    }
}
