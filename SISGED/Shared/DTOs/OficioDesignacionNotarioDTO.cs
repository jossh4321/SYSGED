using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class OficioDesignacionNotarioDTO:Documento
    {
        public Estado estado { get; set; } = new Estado();
        public ContenidoOficioDesignacionNotarioDTO contenidoDTO { get; set; } = new ContenidoOficioDesignacionNotarioDTO();
    }

    
    public class ContenidoOficioDesignacionNotarioDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string lugaroficionotarial { get; set; }
        public string idusuario { get; set; }
        public Notario idnotario { get; set; }
    }


    public class OficioDesignacionNotario_lookup
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public ContenidoOficioDesignacionNotario contenido { get; set; } = new ContenidoOficioDesignacionNotario();
        public Estado estado { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public List<Notario> notario { get; set; } = new List<Notario>();
        public List<string> urlanexo { get; set; } = new List<string>();
    }

    public class OficioDesignacionNotario_ur
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public ContenidoOficioDesignacionNotario contenido { get; set; } = new ContenidoOficioDesignacionNotario();
        public Estado estado { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public Notario notario { get; set; } = new Notario();
        public List<string> urlanexo { get; set; } = new List<string>();
    }

}
