using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoEntregaExpedienteNotarioDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public Notario idnotario { get; set; } = new Notario();

    }
    public class EntregaExpedienteNotarioDTO : Documento
    {
        public ContenidoEntregaExpedienteNotarioDTO contenidoDTO { get; set; } = new ContenidoEntregaExpedienteNotarioDTO();
    }
}
