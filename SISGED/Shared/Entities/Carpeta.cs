using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
   public class Carpeta
    {
        public string id { get; set; }
        public DateTime fecha { get; set; }
        public List<DocumentoCarpeta> documentos { get; set; }
    }
}
