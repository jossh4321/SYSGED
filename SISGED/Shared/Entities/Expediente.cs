using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Expediente
    {
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public List<DocumentoExpediente> documentos { get; set; }
        public List<Derivacion> derivaciones { get; set; }
        public string estado { get; set; }
    }
}
