using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoOficioBPNDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string observacion { get; set; }
        public Usuario idcliente { get; set; } = new Usuario();
        public string direccionoficio { get; set; }
        public Notario idnotario { get; set; } = new Notario();
        public string actojuridico { get; set; }
        public string tipoprotocolo { get; set; }
        public List<string> otorgantes { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string url { get; set; }
    }
    public class OficioBPNDTO : Documento
    {
        public ContenidoOficioBPNDTO contenidoDTO { get; set; } = new ContenidoOficioBPNDTO();
    }
}
