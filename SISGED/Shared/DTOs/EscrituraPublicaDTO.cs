using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class EscrituraPublicasDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string direccionoficio { get; set; }
        public string idnotario { get; set; }
        public List<ActoJuridico> actosjuridicos { get; set; }
        public DateTime fechaescriturapublica { get; set; }
        public string url { get; set; }
        public string estado { get; set; }
        public IEnumerable<Notario> notario { get; set; }
    }

    public class EscrituraPublicaDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string direccionoficio { get; set; }
        public string idnotario { get; set; }
        public List<ActoJuridico> actosjuridicos { get; set; }
        public DateTime fechaescriturapublica { get; set; }
        public string url { get; set; }
        public string estado { get; set; }
        public Notario notario { get; set; }
    }

    public class EscrituraPublicaRDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string direccionoficio { get; set; }
        public string idnotario { get; set; }
        public List<ActoJuridico> actosjuridicos { get; set; }
        public DateTime fechaescriturapublica { get; set; }
        public string url { get; set; }
        public string estado { get; set; }
        public string notario { get; set; }
    }
}
