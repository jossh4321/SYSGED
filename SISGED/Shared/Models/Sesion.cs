using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class Sesion
    {
        public List<Permiso> permisosHerram { get; set; } = new List<Permiso>();
        public List<Permiso> permisosInterfaz { get; set; } = new List<Permiso>();
        public List<Tool> herramientasutilizables { get; set; } = new List<Tool>();
        public string rol { get; set; }
        public string nombre { get; set; }
    }
}
