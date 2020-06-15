using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ConclusionFirmaDTO : Documento
    {
        public ContenidoConclusionFirmaDTO contenidoDTO { get; set; } = new ContenidoConclusionFirmaDTO();
    }
    public class ContenidoConclusionFirmaDTO
    {
        public EscrituraPublica idescriturapublica { get; set; }
    }
}
