using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class EscrituraPublica
    {
        public string id { get; set; }
        public string direccionoficio { get; set; }
        public string idnotario { get; set; }
        public List<ActoJuridico> actosjuridicos { get; set; }
        public DateTime fechaescriturapublica { get; set; }
        public string url { get; set; }
        public string estado { get; set; }
    }
}
