using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class ParametrosBusquedaEscrituraPublica
    {
        public string direccionoficionotarial { get; set; }
        public string nombrenotario { get; set; }
        public string actojuridico { get; set; }
        public List<string> nombreotorgantes { get; set; }
    }
}
