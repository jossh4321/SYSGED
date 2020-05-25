using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class Tool
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string icono { get; set; }
        //public List<string> roles { get; set; } = new List<string>();
        /*posibles lugares 'tools' o 'workspace'*/
        public string currentPlace { get; set; } = "tools";
        public string componentName { get; set; } = "ContenidoPrueba";
    }
}
