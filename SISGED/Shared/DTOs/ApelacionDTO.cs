using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ApelacionDTO : Documento
    {
        public Evaluacion evaluacion { get; set; } = new Evaluacion();
        public ContenidoApelacionDTO contenidoDTO { get; set; } = new ContenidoApelacionDTO();
    }


    public class ContenidoApelacionDTO
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaapelacion { get; set; }
        public string data { get; set; }
    }
}
