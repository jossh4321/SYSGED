using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class SolicitudInicialDTO : Documento
    {
        public ContenidoSolicitudInicialDTO contenidoDTO { get; set; } = new ContenidoSolicitudInicialDTO();
    }

    public class ContenidoSolicitudInicialDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public List<string> Urlanexo { get; set; } = new List<string>();
    }
}
