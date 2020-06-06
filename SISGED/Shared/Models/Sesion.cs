using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class Sesion
    {
        public List<Permiso> permisosHerram { get; set; }
        public List<Permiso> permisosInterfaz { get; set; }
        public string rol { get; set; }
        public string nombre { get; set; }
    }
}
