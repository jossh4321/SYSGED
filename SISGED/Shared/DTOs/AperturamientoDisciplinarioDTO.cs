using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class AperturamientoDisciplinarioDTO:Documento
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

    public class AperturamientoD_lookup_notario
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public string estado { get; set; }
        public ContenidoAperturamientoDisciplinario contenido { get; set; }
        public List<Notario> notario = new List<Notario>();
    }

    public class AperturamientoD_project
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        public string estado { get; set; }
        public ContenidoAperturamientoD_project contenido { get; set; }
        public List<Notario> notario = new List<Notario>();
    }
    public class ContenidoAperturamientoD_project
    {
        public Notario idnotario { get; set; }//
        public string idfiscal { get; set; }
        public string nombredenunciante { get; set; }//
        public string titulo { get; set; }//
        public string descripcion { get; set; }//
        public DateTime fechainicioaudiencia { get; set; }//
        public DateTime fechafinaudiencia { get; set; }//
        public List<string> participantes { get; set; } = new List<string>();
        public string lugaraudiencia { get; set; }//
        public List<string> hechosimputados { get; set; } = new List<string>();
        public string url { get; set; }//
    }
}
