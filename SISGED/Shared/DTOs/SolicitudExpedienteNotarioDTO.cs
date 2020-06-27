using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class SolicitudExpedienteNotarioDTO : Documento
    {
        public ContenidoSolicitudExpedienteNotarioDTO contenidoDTO { get; set; } = new ContenidoSolicitudExpedienteNotarioDTO();
    }


    public class ContenidoSolicitudExpedienteNotarioDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaemision { get; set; }
    }
}
