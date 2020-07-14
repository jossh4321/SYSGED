using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class ContenidoVersion
    {
        public Int32 version { get; set; }
        public DateTime fechamodificacion { get; set; } = DateTime.Now;
        public string url { get; set; }
    }
}
