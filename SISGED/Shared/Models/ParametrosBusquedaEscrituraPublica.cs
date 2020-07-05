using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class ParametrosBusquedaEscrituraPublica
    {
        public int pagina { get; set; } = 1;
        public int cantidadregistros { get; set; } = 10;

        public Pagination Paginacion
        {
            get { return new Pagination() { Page = pagina, QuantityPerPage = cantidadregistros }; }
        }

        public string direccionoficionotarial { get; set; }
        public string nombrenotario { get; set; }
        public string actojuridico { get; set; }
        public List<string> nombreotorgantes { get; set; }
    }
    public class ParametrosBusquedaEscrituraPublicaDTO
    {
        public string direccionoficionotarial { get; set; }
        public string nombrenotario { get; set; }
        public string actojuridico { get; set; }
        public List<OtorganteDTO> nombreotorgantes { get; set; }
    }
}
