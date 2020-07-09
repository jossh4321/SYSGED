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
        public List<Contrato> contratos { get; set; } = new List<Contrato>();
        public List<Otorgante> otorgantes { get; set; } = new  List<Otorgante>();
        //Mi primer commit
        //Mi primer commit
        //Mi primer commit
        // my tercer commit :"v
        // Mi primer commit
    }
}
