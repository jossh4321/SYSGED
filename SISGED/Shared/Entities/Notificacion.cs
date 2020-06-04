using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Notificacion
    {
        public string id { get; set; }
        public string cuerpo { get; set; }
        public string estado { get; set; }
        public string idemisor { get; set; }
        public string idreceptor { get; set; }
        public DateTime fechaemision { get; set; }
        public string iddocumento { get; set; }
    }
}
