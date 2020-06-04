using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Derivacion
    {
        public string areaprocedencia { get; set; }
        public string areadestino { get; set; }
        public string usuarioemisor { get; set; }
        public DateTime fechaderivacion { get; set; }
        public string estado { get; set; }
        public string tipo { get; set; }
    }
}
