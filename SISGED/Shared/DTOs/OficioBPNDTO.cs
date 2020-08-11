using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoOficioBPNDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public Usuario idcliente { get; set; } = new Usuario();
        public string direccionoficio { get; set; }
        public Notario idnotario { get; set; } = new Notario();
        public string actojuridico { get; set; }
        public string tipoprotocolo { get; set; }
        public List<OtorganteDTO> otorgantes { get; set; } = new List<OtorganteDTO>();
        public DateTime? fecharealizacion { get; set; }
        public string data { get; set; }
        public List<string> Urlanexo { get; set; } = new List<string>();

    }
    public class OficioBPNDTO : Documento
    {
        public Evaluacion evaluacion { get; set; } = new Evaluacion();
        public ContenidoOficioBPNDTO contenidoDTO { get; set; } = new ContenidoOficioBPNDTO();
    }

    public class OficioBPNDTO_lookup
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public Evaluacion evaluacion { get; set; } = new Evaluacion();
        public ContenidoOficioBPN contenido { get; set; } = new ContenidoOficioBPN();
        public List<Notario> notario { get; set; } = new List<Notario>();
    }

    public class OficioBPNDTO_ur
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public Evaluacion evaluacion { get; set; } = new Evaluacion();
        public ContenidoOficioBPN contenido { get; set; } = new ContenidoOficioBPN();
        public Notario notario { get; set; } = new Notario();
    }
    public class OficioBPNDTO_lookup2
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public Evaluacion evaluacion { get; set; } = new Evaluacion();
        public ContenidoOficioBPN contenido { get; set; } = new ContenidoOficioBPN();
        public Notario notario { get; set; } = new Notario();
        public List<Usuario> cliente { get; set; } = new List<Usuario>();
    }
}
