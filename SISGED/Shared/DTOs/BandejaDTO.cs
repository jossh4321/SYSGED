using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class BandejaDTO
    {
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDTODocumento> bandejaentrada { get; set; }
        public List<BandejaDTODocumento> bandejasalida { get; set; }
    }

    public class BandejaDTODocumento
    {
        public string idexpediente { get; set; }
        public string iddocumento { get; set; }
        public string nombreexpediente { get; set; }
        public string tipoDocumento { get; set; }
        public string nombreCliente { get; set; }

    }
}
