using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Expediente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("cliente")]
        public Cliente cliente { get; set; }
        [BsonElement("fechainicio")]
        public DateTime fechainicio { get; set; }
        [BsonElement("fechafin")]
        public DateTime? fechafin { get; set; }
        [BsonElement("documentos")]
        public List<DocumentoExpediente> documentos { get; set; }
        [BsonElement("derivaciones")]
        public List<Derivacion> derivaciones { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }
    }
}
