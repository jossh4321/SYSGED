using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISGED.Shared.Entities
{
    class ModificarPerfil
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string id { get; set; }
        [BsonElement("nombre_usuario")]
        public string nombre_usuario { get; set; }
        [BsonElement("nombres")]
        public string nombres { get; set; }
        [BsonElement("apellidos")]
        public string apellidos { get; set; }
        [BsonElement("Tipo_documento")]
        public string Tipo_documento { get; set; }
        [BsonElement("Ndocumento")]
        public int Ndocumento { get; set; }
        [BsonElement("Direccion")]
        public string Direccion { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Sexo")]
        public string Sexo {get;set;}
        [BsonElement("foto")]
        public string foto { get; set; }



    }
}
