using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class BandejaDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }
    }

    public class BandejaDTODocumento
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public List<Expediente> bandejadocumento { get; set; }

    }

    public class BandejaDTODocumentoExpediente
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public Expediente bandejadocumento { get; set; }
    }

    public class BandejaDTOR
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejasalida { get; set; }
        public DocumentoExpediente documento { get; set; }
        public string tipoexpediente { get; set; }
    }
}
