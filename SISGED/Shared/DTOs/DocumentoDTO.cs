using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
   

    public class OpcionDocumento
    {
        public string label { get; set; } = "";
        public string value { get; set; } = "";
    }
    public class DocumentoEvaluadoDTO
    {
        public string id { get; set; } = null;
        public Evaluacion evaluacion { get; set; } = new Evaluacion()
        {
            resultado = "",
            evaluaciones = new List<EvaluacionIndividual>()
        };
        public string idexpediente { get; set; } = null;
        public string idusuario { get; set; } = null;
    }

    public class DocumentoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public Object contenido { get; set; }
        public Object estado { get; set; }
        public Object evaluacion { get; set; }
        public DateTime fechacreacion { get; set; }
        public List<string> urlanexo { get; set; } = new List<string>();
    }
    public class DocumentoGenerarDTO
    {
        public string iddocumentoAnterior { get; set; } = null;
        public string iddocumento { get; set; } = null;
        public string idexpediente { get; set; } = null;
        public string idusuario { get; set; } = null;
        public string codigo { get; set; }
        public string firma { get; set; }
    }
    public class DocumentoUsuarioDTO
    {
        public DocumentoExpediente documento { get; set; }
    }
    public class DocumentoUsuarioLUDTO
    {
        public DocumentoExpediente documento { get; set; }
        public List<Documento> documentoOriginal { get; set; }
    }
    public class DocumentoUsuarioUDTO
    {
        public DocumentoExpediente documento { get; set; }
        public Documento documentoOriginal { get; set; }
    }
    public class DocumentoADTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public Object contenido { get; set; }
        public string estado { get; set; }
        public Object evaluacion { get; set; }
        public List<string> urlanexo { get; set; } = new List<string>();
    }

    public class DocumentoADTO2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public ContenidoSolicitudInicial contenido { get; set; } = new ContenidoSolicitudInicial();
        public string estado { get; set; }
        public Object evaluacion { get; set; } = new Object();
        public List<string> urlanexo { get; set; } = new List<string>();
    }
}
