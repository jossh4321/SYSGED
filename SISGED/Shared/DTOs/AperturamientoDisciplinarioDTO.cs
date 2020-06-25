using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class AperturamientoDisciplinarioDTO
    {
        
        public string estado { get; set; }
        public ContenidoAperturamientoDisciplinarioDTO contenidoDTO { get; set; } = new ContenidoAperturamientoDisciplinarioDTO();
    }

    public class ContenidoAperturamientoDisciplinarioDTO
    {
        public Notario idnotario { get; set; }//
        public string idfiscal { get; set; }
        public string nombredenunciante { get; set; }//
        public string titulo { get; set; }//
        public string descripcion { get; set; }//
        public DateTime fechainicioaudiencia { get; set; }//
        public DateTime fechafinaudiencia { get; set; }//
        public List<Participante> participantes { get; set; } = new List<Participante>();
        public string lugaraudiencia { get; set; }//
        public List<Hecho> hechosimputados { get; set; } = new List<Hecho>();
        public string url { get; set; }//
    }
    public class Participante
    {
        public string nombre { get; set; }
        public Int32 index { get; set; } = 0;
    }

    public class Hecho
    {
        public string descripcion { get; set; }
        public Int32 index { get; set; } = 0;
    }
}
