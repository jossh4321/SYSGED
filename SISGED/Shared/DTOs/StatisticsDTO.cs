using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class StatisticsDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public int mes { get; set; }
    }
    public class StatisticsDTOR
    {
        [BsonId]
        public string id { get; set; }
        public Int32 cantidad { get; set; }
    }

    public class StatisticsDTO4_project1
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Evaluacion evaluacion { get; set; }
        public Int32 mes { get; set; }
    }

    public class StatisticsDTO4R
    {
        [BsonId]
        public string id { get; set; }
        public Int32 aprobados { get; set; }
        public Int32 rechazados { get; set; }
    }

    public class StatisticsDTO3_project
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Int32 mes { get; set; }
    }

    public class StatisticsDTO3_group
    {
        [BsonId]
        public string id { get; set; }
        public Int32 caducados { get; set; }
    }
    /////////////////////////////////////////
    public class Expediente_unwind1
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
        public DocumentoExpediente documentos { get; set; } = new DocumentoExpediente();
    }
    public class Expediente_lookup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
        public DocumentoExpediente documentos { get; set; } = new DocumentoExpediente();
        public List<Documento> documentoobj { get; set; } = new List<Documento>();
    }
    public class Expediente_unwind2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
        public DocumentoExpediente documentos { get; set; } = new DocumentoExpediente();
        public Documento documentoobj { get; set; } = new Documento();
    }

    public class Expediente_group
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
        public List<DocumentoExpediente> documentos { get; set; } = new List<DocumentoExpediente>();
        public List<Documento> documentoobj { get; set; } = new List<Documento>();
    }
    ///ESTADISTICAS NUEVAS
    public class estadistica1_proyect1
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public int mes { get; set; }
        public string estado { get; set; }
        public string tipo { get; set; }
    }
    public class estadistica1_group
    {
        
        public string id { get; set; }
        public Int32 caducados { get; set; }
        public Int32 procesados { get; set; }
        public Int32 pendientes { get; set; }
    }
}
