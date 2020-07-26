using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class SolicitudInicialDTO : Documento
    {
        public string nombrecliente { get; set; }
        public string tipodocumento { get; set; }
        public string numerodocumento { get; set; }
        public ContenidoSolicitudInicialDTO contenidoDTO { get; set; } = new ContenidoSolicitudInicialDTO();
    }

    public class ContenidoSolicitudInicialDTO
    {
        public Usuario idcliente { get; set; } = new Usuario();
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public List<string> Urlanexo { get; set; } = new List<string>();
        public DateTime fechacreacion { get; set; }
    }
}
