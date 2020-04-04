using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Rol
    {
        public string nombre { get; set; }
        public List<Permiso> listapermisos { get; set; }
        public string descripcion { get; set; }
    }
}
