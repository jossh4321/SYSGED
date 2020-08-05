using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class UsuarioDTO
    {
        public string id { get; set; }
        public string tipo { get; set; }

        public string usuario { get; set; }

        public string clave { get; set; }

        public Datos datos { get; set; } = new Datos();

        public string estado { get; set; }

        public  string rolid { get; set; } 
    }


    public class Usuario_LK
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public Datos datos { get; set; } = new Datos();
        public string estado { get; set; }
        public string rol { get; set; }
        public List<Rol> rolobj { get; set; }
    }


    public class UsuarioRDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public Datos datos { get; set; } = new Datos();
        public string estado { get; set; }
        public string rol { get; set; }
        public Rol rolobj { get; set; } = new Rol();
    }

    public class UsuarioEvaluacionDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public Datos datos { get; set; } = new Datos();
        public string descripcion { get; set; }
        public string status { get; set; }
    }

    /// AUTO COMPLETE USUARIO
    public class usuario_project2
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Datos datos { get; set; } = new Datos();
        public Rol rol { get; set; } = new Rol();
    }
    public class usuario_project1
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Datos datos { get; set; } = new Datos();
        public string rol { get; set; }
        public string nombre { get; set; }
    }
    public class usuario_lookup1
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Datos datos { get; set; } = new Datos();
        public string rol { get; set; }
        public string nombre { get; set; }
        public List<Rol> rolobj { get; set; }
    }
    public class usuario_unwind
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Datos datos { get; set; } = new Datos();
        public string rol { get; set; }
        public string nombre { get; set; }
        public Rol rolobj { get; set; }
    }
}
