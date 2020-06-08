using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    [BsonDiscriminator(RootClass = true)]
    public class Notario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("apellido")]
        public string apellido { get; set; }
        [BsonElement("fechanacimiento")]
        public DateTime fechanacimiento { get; set; }
        [BsonElement("dni")]
        public string dni { get; set; }
        [BsonElement("direccion")]
        public string direccion { get; set; }
        [BsonElement("email")]
        public string email { get; set; }
        [BsonElement("colegiatura")]
        public string colegiatura { get; set; }
        [BsonElement("oficionotarial")]
        public OficioNotarial oficionotarial { get; set; }
        [BsonElement("imagen")]
        public string imagen { get; set; }
        [BsonElement("expedientes")]
        public List<string> expedientes { get; set; }
    }
}
