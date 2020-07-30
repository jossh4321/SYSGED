using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class PasoDTO
    {
        public Int32 indice { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public String fechainicio { get; set; }
        public String fechafin { get; set; }
        public String fechalimite { get; set; }
        public Int32 dias { get; set; }
        public String idexpediente { get; set; }
        public Int32 paso { get; set; }
        public Int32 subpaso { get; set; }
        public String tipodocumento { get; set; }
        public Int32 pasoantiguo { get; set; }
    }
}
