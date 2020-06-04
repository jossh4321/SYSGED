using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class ResultadoSession
    {
        public string id { get; set; }
        public DateTime fecha { get; set; }
        public List<DocumentoResultadoSesion> documentos { get; set; }
        public List<string> participantes { get; set; }
    }
}
