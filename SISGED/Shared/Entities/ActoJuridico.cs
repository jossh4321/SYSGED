using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SISGED.Shared.Entities
{
    public class ActoJuridico
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public List<Contrato> contratos { get; set; }
        public List<Otorgante> otorgantes { get; set; }
        // my tercer commit :"v
    }
}
