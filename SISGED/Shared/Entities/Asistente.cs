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
        public string id { get; set; }
        [BsonElement("nombreexpediente")]
        public string nombreexpediente { get; set; }
        [BsonElement("documentos")]
        public List<DocumentoAsistente> documentos { get; set; }
    }

    public class DocumentoAsistente
    {
        public string tipo { get; set; }
        public List<Paso> pasos { get; set; }
    }

    public class Paso
    {
        public string indice { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}
