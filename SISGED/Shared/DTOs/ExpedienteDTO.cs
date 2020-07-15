using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ExpedienteDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public List<DocumentoExpediente> documentos { get; set; } = new List<DocumentoExpediente>();
        public List<Documento> documentosobj { get; set; } = new List<Documento>();
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
    }

    public class ExpedienteDocumentoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
    }

    public class ExpedienteDTO2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public List<DocumentoExpediente> documentos { get; set; } = new List<DocumentoExpediente>();
        public List<Documento> documentosobj { get; set; } = new List<Documento>();
        public List<Derivacion> derivaciones { get; set; } = new List<Derivacion>();
        public string estado { get; set; }
    }


    public class ExpedienteDTO_ur1
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
    }
    public class ExpedienteDTO_look_up 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
        public List<Documento> documentoobj { get; set; }
    }
    public class ExpedienteDTO_ur2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
        public Documento documentoobj { get; set; }
    }
    public class ExpedienteWrapper
    {
        public string idexpediente { get; set; }
        public object documento { get; set; }
        public string idusuarioactual { get; set; }
        public string documentoentrada { get; set; }
    }

    public class ExpedienteFinal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public Documento documento { get; set; } = new Documento();
        public List<Documento> documentosobj { get; set; } = new List<Documento>();
    }

    public class Expedientedoc
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; } = new DocumentoExpediente();
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
    }

    public class Expedientedoc_lookup
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; } = new DocumentoExpediente();
        public List<Derivacion> derivaciones { get; set; }
        public List<DocumentoDTO> documentosobj { get; set; } = new List<DocumentoDTO>();
        public string estado { get; set; }
    }

    public class Expedientedoc_lookup_ur
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DocumentoExpediente documentos { get; set; } = new DocumentoExpediente();
        public List<Derivacion> derivaciones { get; set; }
        public DocumentoDTO documentosobj { get; set; } = new DocumentoDTO();
        public string estado { get; set; }
    }

    public class Expedientedoc_group
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public List<DocumentoDTO> documentosobj { get; set; } = new List<DocumentoDTO>();

    }
}
