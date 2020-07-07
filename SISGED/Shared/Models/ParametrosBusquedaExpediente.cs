using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class ParametrosBusquedaExpediente
    {
        public int pagina { get; set; } = 1;
        public int cantidadregistros { get; set; } = 3;

        public Pagination Paginacion
        {
            get { return new Pagination() { Page = pagina, QuantityPerPage = cantidadregistros }; }
        }

        public string estado { get; set; }
        public string nombrecliente { get; set; }     
        public string tipo { get; set; }        
    }
    public class ParametrosBusquedaExpedienteDTO
    {
        public string estado { get; set; }
        public string tipo { get; set; }
        public string nombrecliente { get; set; }

    }
}
