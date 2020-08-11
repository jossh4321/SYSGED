using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class PasosDTO2
    {
        public String id { get; set; }
        public String nombreexpediente { get; set; }
        public List<DocumentoPasoDTO2> documentos { get; set; }
    }
    public class DocumentoPasoDTO2
    {
        public Int32 indice { get; set; }
        public String tipo { get; set; }
        public List<Paso> pasos { get; set; }
    }
}
