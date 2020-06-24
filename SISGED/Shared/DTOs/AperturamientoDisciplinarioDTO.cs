using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class AperturamientoDisciplinarioDTO : Documento
    {
        public ContenidoAperturamientoDisciplinarioDTO contenidoDTO { get; set; } = new ContenidoAperturamientoDisciplinarioDTO();
    }


    public class ContenidoAperturamientoDisciplinarioDTO
    {
        public Notario idnotario { get; set; }
        public string idfiscal { get; set; }
        public string nombredenunciante { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechainicioaudiencia { get; set; }
        public DateTime fechafinaudiencia { get; set; }
        public List<string> participantes { get; set; }
        public string lugaraudiencia { get; set; }
        public List<string> hechosimputados { get; set; }
        public string estado { get; set; }
        public string data { get; set; }
    }
}
