﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(
        typeof(SolicitudDenuncia),
        typeof(OficioBPN),
        typeof(SolicitudBPN),
        typeof(ResultadoBPN),
        typeof(SolicitudExpedicionFirma),
        typeof(OficioDesignacionNotario),
        typeof(ConclusionFirma),
        typeof(AperturamientoDisciplinario),
        typeof(SolicitudExpedienteNotario),
        typeof(Dictamen),
        typeof(Resolucion),
        typeof(Apelacion))]
    public class Documento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }
        [BsonElement("historialcontenido")]
        public List<ContenidoVersion> historialcontenido { get; set; }
        [BsonElement("historialproceso")]
        public List<Proceso> historialproceso { get; set; }
    }


    public class SolicitudDenuncia : Documento
    {
        public ContenidoSolicitudDenuncia contenido { get; set; }
        public class ContenidoSolicitudDenuncia
        {
            public string codigo { get; set; }
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public string nombrecliente { get; set; }
            public DateTime fechaentrega { get; set; }
            public string url { get; set; }
        }
    }
    public class OficioBPN : Documento
    {
        public  ContenidoOficioBPN contenido { get; set; }
        public class ContenidoOficioBPN
        {
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public string idcliente { get; set; }
            public string direccionoficio { get; set; }
            public string idnotario { get; set; }
            public string actojuridico { get; set; }
            public string tipoprotocolo { get; set; }
            public List<string> otorgantes { get; set; }
            public DateTime fecharealizacion { get; set; }
            public string url { get; set; }
        }
        
    }

    public class SolicitudBPN : Documento
    {
        public ContenidoSolicitudBPN contenido { get; set; }
        public class ContenidoSolicitudBPN
        {
            public string idcliente { get; set; }
            public string direccionoficio { get; set; }
            public string idnotario { get; set; }
            public string actojuridico { get; set; }
            public string tipoprotocolo { get; set; }
            public List<string> otorgantes { get; set; }
            public DateTime fecharealizacion { get; set; }
        }

    }

    public class ResultadoBPN : Documento
    {
        public ContenidoResultadoBPN contenido { get; set; }
        public class ContenidoResultadoBPN
        {
            public Int32 costo { get; set; }
            public Int32 cantidadfoja { get; set; }
            public string estado { get; set; }
            public string idescriturapublica { get; set; }
        }

    }

    public class SolicitudExpedicionFirma : Documento
    {
        public ContenidoSolicitudExpedicionFirma contenido { get; set; }
        public class ContenidoSolicitudExpedicionFirma
        {
            public string codigo { get; set; }
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public string cliente { get; set; }
            public DateTime fecharealizacion { get; set; }
            public string url { get; set; }
        }

    }
    
    public class ConclusionFirma : Documento
    {
        public ContenidoConclusionFirma contenido { get; set; }
        public class ContenidoConclusionFirma
        {
            public string idescriturapublica { get; set; }
        }

    }

    
    public class OficioDesignacionNotario : Documento
    {
        public ContenidoOficioDesignacionNotario contenido { get; set; } = new ContenidoOficioDesignacionNotario();
    }
    public class ContenidoOficioDesignacionNotario
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string lugaroficionotarial { get; set; }
        public string idusuario { get; set; }
        public string idnotario { get; set; }
    }

    public class AperturamientoDisciplinario : Documento
    {
        public ContenidoAperturamientoDisciplinario contenido { get; set; }
        public class ContenidoAperturamientoDisciplinario
        {
            public string idnotario { get; set; }
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
            public string url { get; set; }

        }

    }

    public class SolicitudExpedienteNotario : Documento
    {
        public ContenidoSolicitudExpedienteNotario contenido { get; set; }
        public class ContenidoSolicitudExpedienteNotario
        {
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public DateTime fechaemision { get; set; }
        }
    }

    public class Dictamen : Documento
    {
        public ContenidoDictamen contenido { get; set; }
        public class ContenidoDictamen
        {
            public string nombredenunciante { get; set; }
            public string titulo { get; set; }
            public List<string> observaciones { get; set; }
            public string conclusion { get; set; }
            public List<string> recomendaciones { get; set; }
        }
    }


    public class Resolucion : Documento
    {
        public ContenidoResolucion contenido { get; set; }
        public class ContenidoResolucion
        {
            public string descripcion { get; set; }
            public DateTime fechainicioaudiencia { get; set; }
            public DateTime fechafinaudiencia { get; set; }
            public List<string> participantes { get; set; }
            public string sancion { get; set; }
            public string url { get; set; }
        }
    }
    public class Apelacion : Documento
    {
        public ContenidoDictamen contenido { get; set; }
        public class ContenidoDictamen
        {
            public DateTime fechaapelacion { get; set; }
            public string url { get; set; }
        }
    }

    /*public class OficioDesignacionNotario : Documento
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string lugaroficionotarial { get; set; }
        public string idusuario { get; set; }
        public string idnotario { get; set; }
    }*/

}