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
        public ContenidoConclusionFirmaDTO contenidoDTO { get; set; } = new ContenidoConclusionFirmaDTO();
    }
    public class ContenidoConclusionFirmaDTO
    {
        public EscrituraPublica idescriturapublica { get; set; } = new EscrituraPublica();
        public Notario idnotario { get; set; } = new Notario();
        public Usuario idcliente { get; set; } = new Usuario();
        public Int32 cantidadfoja { get; set; } = 2;
        public double precio { get; set; } = 0;
        public List<string> Urlanexo { get; set; } = new List<string>();

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

    public class ConclusionFirma_lookup2
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public string estado { get; set; }
        public ContenidoConclusionFirma contenido { get; set; }

        public List<EscrituraPublica> escriturapublica = new List<EscrituraPublica>();

        public List<Notario> notario = new List<Notario>();
    }

    public class ConclusionFirma_lookup3
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public string estado { get; set; }
        public ContenidoConclusionFirma contenido { get; set; }

        public List<EscrituraPublica> escriturapublica = new List<EscrituraPublica>();

        public List<Notario> notario = new List<Notario>();
        public List<Usuario> cliente = new List<Usuario>();
    }

}
