using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public  class NotarioDTO
    {
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
    }
}
