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
        public List<DocumentoPasoDTO2> documentos { get; set; } = new List<DocumentoPasoDTO2>();
    }
    public class DocumentoPasoDTO2
    {
        public string uid { get; set; }
        public Int32 indice { get; set; }
        public String tipo { get; set; }
        public List<PasoDocDTO> pasos { get; set; } = new List<PasoDocDTO>();
    }

    public class PasoDocDTO
    {
        public string uid { get; set; }
        public Int32 indice { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public DateTime? fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public DateTime? fechalimite { get; set; }
        public Int32 dias { get; set; }
        public List<SubPaso> subpaso { get; set; } = new List<SubPaso>();
        
    }
}
