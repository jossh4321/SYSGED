using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Notario
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateTime fechanacimiento { get; set; }
        public string dni { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public string colegiatura { get; set; }
        public string imagen { get; set; }
        public OficioNotarial oficionotarial { get; set; }
        public List<string> expedientes { get; set; }
    }
}
