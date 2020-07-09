using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class NotificacionDTO
    {
        public string id { get; set; }
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public string estado { get; set; }
        public Usuario idemisor { get; set; }
        public Usuario idreceptor { get; set; }
        public DateTime fechaemision { get; set; }
        public Documento iddocumento { get; set; }
    }
}
