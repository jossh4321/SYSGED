using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.DTOs;

namespace SISGED.Shared.Entities
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("usuario")]
        public string usuario { get; set; }

        [BsonElement("clave")]
        public string clave { get; set; }

        [BsonElement("datos")]
        public Datos datos { get; set; } = new Datos();

        [BsonElement("estado")]
        public string estado { get; set; }

        [BsonElement("roles")]
        public List<Rol> roles { get; set; } = new List<Rol>();
    }
}
