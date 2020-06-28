using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class DictamenDTO
    {

        public string estado { get; set; }
        public ContenidoDictamenDTO contenidoDTO { get; set; } = new ContenidoDictamenDTO();
    }

    public class ContenidoDictamenDTO
    {
        public string descripcion { get; set; }
        public string nombredenunciante { get; set; }
        public string titulo { get; set; }
        public List<Observaciones> observaciones { get; set; } = new List<Observaciones>();
        public string conclusion { get; set; }
        public List<Recomendaciones> recomendaciones { get; set; } = new List<Recomendaciones>();
        //public DateTime fechaemision { get; set; } 
    }
    public class Observaciones
    {
        public string descripcion { get; set; }
        public Int32 index { get; set; } = 0;
    }

    public class Recomendaciones
    {
        public string descripcion { get; set; }
        public Int32 index { get; set; } = 0;
    }
}
