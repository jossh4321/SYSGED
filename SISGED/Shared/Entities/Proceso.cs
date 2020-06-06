using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Proceso
    {
        public Int32 indice { get; set; }
        public string area { get; set; }
        public DateTime fecharecepcion { get; set; }
        public DateTime fechaemision { get; set; }
        public string idusuario { get; set; }
    }
}
