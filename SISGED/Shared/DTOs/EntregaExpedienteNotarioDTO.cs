using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoEntregaExpedienteNotarioDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public Notario idnotario { get; set; } = new Notario();

        public List<string> urlanexo { get; set; } = new List<string>();
    }
    public class EntregaExpedienteNotarioDTO : Documento
    {
        public ContenidoEntregaExpedienteNotarioDTO contenidoDTO { get; set; } = new ContenidoEntregaExpedienteNotarioDTO();
    }
    public class EntregaExpedienteNotario_lookup
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public string estado { get; set; }
        public ContenidoSolicitudExpedienteNotario contenido { get; set; } = new ContenidoSolicitudExpedienteNotario();
        public List<Notario> notario { get; set; } = new List<Notario>();
    }
}
