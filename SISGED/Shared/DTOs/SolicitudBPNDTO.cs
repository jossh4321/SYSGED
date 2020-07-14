using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoSolicitudBPNDTO
    {
        public Usuario idcliente { get; set; } = new Usuario();
        public string direccionoficio { get; set; }
        public Notario idnotario { get; set; } = new Notario();
        public string actojuridico { get; set; }
        public string tipoprotocolo { get; set; }
        public List<string> otorgantes { get; set; } = new List<String>();
        public DateTime fecharealizacion { get; set; }
        public List<Otorgantelista> otorganteslista { get; set; } = new List<Otorgantelista>();
    }
    public class SolicitudBPNDTO : Documento
    {
        public string nombrecliente { get; set; }
        public string tipodocumento { get; set; }
        public string numerodocumento { get; set; }
        public string estado { get; set; } //cambiado por mi
        public ContenidoSolicitudBPNDTO contenidoDTO { get; set; } = new ContenidoSolicitudBPNDTO();

    }
    public class Otorgantelista
    {
        public string nombre { get; set; }
        //public string apellido { get; set; }
        //public int dni { get; set; }
        public Int32 index { get; set; } = 0;
    }
}
