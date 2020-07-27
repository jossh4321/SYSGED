using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Asistente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String id { get; set; }
        [BsonElement("idexpediente")]
        public String idexpediente { get; set; }
        [BsonElement("pasos")]
        public PasoAsistente pasos {get; set;}
        [BsonElement("paso")]
        public Int32 paso { get; set; }
        [BsonElement("subpaso")]
        public Int32 subpaso { get; set; }
        [BsonElement("tipodocumento")]
        public String tipodocumento { get; set; }
    }

    public class PasoAsistente
    {
        public String nombreexpediente { get; set; }
        public List<DocumentoPaso> documentos { get; set; }
    }
}
