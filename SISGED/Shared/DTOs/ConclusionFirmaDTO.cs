using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ConclusionFirmaDTO : Documento
    {
        public string estado { get; set; }
        public ContenidoConclusionFirmaDTO contenidoDTO { get; set; } = new ContenidoConclusionFirmaDTO();
    }
    public class ContenidoConclusionFirmaDTO
    {
        public EscrituraPublica idescriturapublica { get; set; }
    }

    public class ConclusionFirma_lookup
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
