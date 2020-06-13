using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoSolicitudDenuncia
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string nombrecliente { get; set; }
        public DateTime fechaentrega { get; set; }
        public string url { get; set; }
    }
    public class SolicitudDenunciaDTO : Documento
    {
        public ContenidoSolicitudDenuncia contenidoDTO { get; set; } = new ContenidoSolicitudDenuncia();
    }
}
