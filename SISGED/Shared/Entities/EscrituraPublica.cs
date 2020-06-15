using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISGED.Shared.Entities
{
    [BsonDiscriminator(RootClass = true)]
    public class EscrituraPublica
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("direccionoficio")]
        public string direccionoficio { get; set; }
        [BsonElement("titulo")]
        public string titulo { get; set; }
        [BsonElement("idnotario")]
        public string idnotario { get; set; }
        [BsonElement("actosjuridicos")]
        public List<ActoJuridico> actosjuridicos { get; set; }
        [BsonElement("fechaescriturapublica")]
        public DateTime fechaescriturapublica { get; set; }
        [BsonElement("url")]
        public string url { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }
    }
}
