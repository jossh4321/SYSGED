using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Bandeja
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("usuario")]
        public string usuario { get; set; }
        [BsonElement("bandejaentrada")]
        public List<BandejaDocumento> bandejaentrada { get; set; }
        [BsonElement("bandejasalida")]
        public List<BandejaDocumento> bandejasalida { get; set; }
    }
}
