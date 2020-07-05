using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Otorgante
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string dni { get; set; }
    }

    public class OtorganteDTO
    {
        public string nombre { get; set; }
        public int index { get; set; }
    }
}
