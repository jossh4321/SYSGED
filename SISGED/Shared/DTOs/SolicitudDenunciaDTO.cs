using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoSolicitudDenunciaDTO
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string nombrecliente { get; set; }
        public DateTime fechaentrega { get; set; }
        public string urldata { get; set; }
    }
    public class SolicitudDenunciaDTO : Documento
    {
        public ContenidoSolicitudDenunciaDTO contenidoDTO { get; set; } = new ContenidoSolicitudDenunciaDTO();
    }
}
