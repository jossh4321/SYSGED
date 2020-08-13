using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Pasos
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String id { get; set; }
        [BsonElement("nombreexpediente")]
        public String nombreexpediente { get; set; }
        [BsonElement("documentos")]
        public List<DocumentoPaso> documentos { get; set; }
    }

    public class DocumentoPaso
    {
        public String tipo { get; set; }
        public List<Paso> pasos { get; set; } 
    }
    public class Paso
    {
        
        public Int32 indice { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public DateTime? fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DateTime? fechalimite { get; set; }
        public Int32 dias { get; set; }
        public List<SubPaso> subpaso { get; set; } = new List<SubPaso>();

    }

    public class SubPaso
    {
        public Int32 indice { get; set; }
        public String descripcion { get; set; }
    } 
}
