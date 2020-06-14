using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ExpedienteDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafin { get; set; }
        public List<DocumentoExpediente> documentos { get; set; }
        public List<Documento> documentosobj { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
        
    }


   

    public class ExpedienteDTO_ur1
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
    }

    public class ExpedienteDTO_look_up 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
        public List<Documento> documentoobj { get; set; }
    }

    public class ExpedienteDTO_ur2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
        public Documento documentoobj { get; set; }
    }
}
