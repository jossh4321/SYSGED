using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class EstadisticaDocXMesDTO
    {
        public string mes { get; set; } = "1";
    }
    public class EstadisticaDocXMesXAreaDTO
    {
        public string mes { get; set; } = "1";
        public string area { get; set; } = "MesaPartes";
    }
    public class EstadisticaDocCaducados
    {
        public string mes { get; set; } = "1";
        public string dni { get; set; }
    }

    public class EstadisticaEstDocsUsuario
    {
        public string mes { get; set; } = "1";
        public usuario_unwind usuario { get; set; }
    }
}
