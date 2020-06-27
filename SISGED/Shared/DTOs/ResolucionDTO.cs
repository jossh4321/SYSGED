﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ResolucionDTO
    {

        public string estado { get; set; }
        public ContenidoResolucionDTO contenidoDTO { get; set; } = new ContenidoResolucionDTO();
    }

    public class ContenidoResolucionDTO
    {
        public string descripcion { get; set; }
        public string titulo { get; set; }
        public DateTime fechainicioaudiencia { get; set; }
        public DateTime fechafinaudiencia { get; set; }
        public List<Participante> participantes { get; set; } = new List<Participante>();
        public string sancion { get; set; }
        public string data { get; set; }
    }
}