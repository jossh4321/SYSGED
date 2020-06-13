﻿using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class ContenidoSolicitudBPNDTO
    {
        public Usuario idcliente { get; set; }
        public string direccionoficio { get; set; }
        public Notario idnotario { get; set; } = new Notario();
        public string actojuridico { get; set; }
        public string tipoprotocolo { get; set; }
        public List<string> otorgantes { get; set; } = new List<String>();
        public DateTime fecharealizacion { get; set; }
    }
    public class SolicitudBPNDTO : Documento
    {
        public ContenidoSolicitudBPNDTO contenidoDTO { get; set; } = new ContenidoSolicitudBPNDTO();

    }
}