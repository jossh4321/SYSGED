using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class Item
    {
        public string nombre { get; set; }
        public Object valor { get; set; }
        public string descripcion { get; set; }
        public string icono { get; set; }
        //public List<string> roles { get; set; } = new List<string>();
        /*posibles lugares 'tools' o 'workspace'*/
        public string currentPlace { get; set; } = "tools";
        public string originPlace { get; set; }
        public Cliente cliente { get; set; }
        public string itemstatus { get; set; } = "ninguno";
    }
}
