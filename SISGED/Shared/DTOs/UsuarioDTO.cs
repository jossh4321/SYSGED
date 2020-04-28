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

        public string usuario { get; set; }

        public string clave { get; set; }

        public Datos datos { get; set; } = new Datos();

        public string estado { get; set; }

        public  string rolid { get; set; } 
    }
}
