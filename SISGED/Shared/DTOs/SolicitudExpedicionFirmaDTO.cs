using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class SolicitudExpedicionFirmaDTO : Documento
    {
        public string nombrecliente { get; set; }
        public string tipodocumento { get; set; } = null;
        public string numerodocumento { get; set; }
        public ContenidoSolicitudExpedicionFirmaDTO contenidoDTO { get; set; } = new ContenidoSolicitudExpedicionFirmaDTO();
    }


    public class ContenidoSolicitudExpedicionFirmaDTO
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string cliente { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string data { get; set; }
    }
}
