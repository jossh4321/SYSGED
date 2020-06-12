using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class OficioDesignacionNotarioDTO:Documento
    {
        public ContenidoOficioDesignacionNotarioDTO contenidoDTO { get; set; } = new ContenidoOficioDesignacionNotarioDTO();
    }

    
    public class ContenidoOficioDesignacionNotarioDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string lugaroficionotarial { get; set; }
        public string idusuario { get; set; }
        public Notario idnotario { get; set; }
    }
}
