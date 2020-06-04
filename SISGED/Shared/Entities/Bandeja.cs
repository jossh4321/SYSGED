using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class Bandeja
    {
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public List<BandejaDocumento> bandejasalida { get; set; }
    }
}
