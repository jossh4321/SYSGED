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
        public List<Item> herramientasutilizables { get; set; } = new List<Item>();
        public List<Item> listaentradas { get; set; } = new List<Item>();
        public List<Item> listasalidas { get; set; } = new List<Item>();
        public string rol { get; set; }
        public Usuario usuario { get; set; } = new Usuario();
    }
}
