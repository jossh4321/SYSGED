using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ResultadoBPNDTO : Documento
    {
        public ContenidoResultadoBPNDTO contenidoDTO { get; set; } = new ContenidoResultadoBPNDTO();
    }

    public class ContenidoResultadoBPNDTO
    {
        public Int32 costo { get; set; }
        public Int32 cantidadfoja { get; set; } = 2;
        public string estado { get; set; }
        public EscrituraPublica idescriturapublica { get; set; }
        public List<string> Urlanexo { get; set; } = new List<string>();
    }
    public class ResultadoBPN_lookup
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public string estado { get; set; }
        public ContenidoConclusionFirma contenido { get; set; }

        public List<EscrituraPublica> escriturapublica = new List<EscrituraPublica>();
    }
}
