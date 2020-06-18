using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    class DocumentoDTO
    {
    }

    public class OpcionDocumento
    {
        public string label { get; set; } = "";
        public string value { get; set; } = "";
    }
    public class DocumentoEvaluadoDTO
    {
        public string id { get; set; } = null;
        public Estado estado { get; set; } = new Estado()
        {
            status = "",
            observacion = null,
        };
        public string idexpediente { get; set; } = null;
        public string idusuario { get; set; } = null;
    }
}
