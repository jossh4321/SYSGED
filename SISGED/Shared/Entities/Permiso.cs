using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Permiso
    {
        public string menu { get; set; }
        public string icono { get; set; }
        public string url { get; set; }
        public List<string> listasubmenus { get; set; }
    }
}
