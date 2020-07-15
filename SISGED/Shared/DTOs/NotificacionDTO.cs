using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class NotificacionDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public string estado { get; set; }
        public Usuario idemisor { get; set; } = new Usuario();
        public Usuario idreceptor { get; set; } = new Usuario();
        public DateTime fechaemision { get; set; }
        public Documento iddocumento { get; set; } = new Documento();
    }
    public class Notificacion_lookup1
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public string estado { get; set; }
        public string idemisor { get; set; }
        public string idreceptor { get; set; }
        public DateTime fechaemision { get; set; }
        public string iddocumento { get; set; }
        public List<Usuario> receptor { get; set; } = new List<Usuario>();
    }

    public class Notificacion_lookup2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public string estado { get; set; }
        public string idemisor { get; set; }
        public string idreceptor { get; set; }
        public DateTime fechaemision { get; set; }
        public string iddocumento { get; set; }
        public List<Usuario> receptor { get; set; } = new List<Usuario>();
        public List<Usuario> emisor { get; set; } = new List<Usuario>();
    }

    public class Notificacion_lookup3
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public string estado { get; set; }
        public string idemisor { get; set; }
        public string idreceptor { get; set; }
        public DateTime fechaemision { get; set; }
        public string iddocumento { get; set; }
        public List<Usuario> receptor { get; set; } = new List<Usuario>();
        public List<Usuario> emisor { get; set; } = new List<Usuario>();
        public List<Documento> documento { get; set; } = new List<Documento>();
    }
}
