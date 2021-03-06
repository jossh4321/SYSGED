﻿using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.DTOs;
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
        typeof(EntregaExpedienteNotario),
        typeof(Dictamen),
        typeof(Resolucion),
        typeof(Apelacion),
        typeof(SolicitudInicial))]
    public class Documento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("historialcontenido")]
        public List<ContenidoVersion> historialcontenido { get; set; } = new List<ContenidoVersion>();
        [BsonElement("historialproceso")]
        public List<Proceso> historialproceso { get; set; } = new List<Proceso>();
        [BsonElement("urlanexo")]
        public List<string> urlanexo { get; set; } = new List<string>();
        [BsonElement("estado")]
        public string estado { get; set; }
        [BsonElement("fechacreacion")]
        public DateTime fechacreacion { get; set; } = DateTime.Now;
    }
    public class Evaluacion
    {
        public string resultado { get; set; }
        public List<EvaluacionIndividual> evaluaciones { get; set; } = new List<EvaluacionIndividual>();
    }

    public class EvaluacionIndividual
    {
        public string idparticipante { get; set; }
        public string status { get; set; }
        public string descripcion { get; set; }
    }


    public class ContenidoSolicitudDenuncia
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string nombrecliente { get; set; }
        public DateTime fechaentrega { get; set; }
        public string url { get; set; }
        public string firma { get; set; }
        public string urlGenerado{ get; set; }
    }

    

    public class SolicitudDenuncia : Documento
    {

        public ContenidoSolicitudDenuncia contenido { get; set; } = new ContenidoSolicitudDenuncia();
    }

    public class ContenidoOficioBPN
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        //public string observacion { get; set; }
        public string idcliente { get; set; }
        public string direccionoficio { get; set; }
        public string idnotario { get; set; }
        public string actojuridico { get; set; }
        public string tipoprotocolo { get; set; }
        public List<string> otorgantes { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string url { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }

    }
    public class OficioBPN : Documento
    {
        [BsonElement("evaluacion")]
        public Evaluacion evaluacion { get; set; }
        public ContenidoOficioBPN contenido { get; set; } = new ContenidoOficioBPN();
     
    }
    public class ContenidoSolicitudBPN
    {
        public string codigo { get; set; }
        public string idcliente { get; set; }
        public string direccionoficio { get; set; }
        public string idnotario { get; set; }
        public string actojuridico { get; set; }
        public string tipoprotocolo { get; set; }
        public List<string> otorgantes { get; set; } = new List<String>();
        //public List<Otorgantelista> otorganteslista { get; set; } 
        public DateTime fecharealizacion { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }

    }

    public class SolicitudBPN : Documento
    {
        public ContenidoSolicitudBPN contenido { get; set; } = new ContenidoSolicitudBPN();

    }
    public class ContenidoResultadoBPN
    {
        public string codigo { get; set; }
        public Int32 costo { get; set; }
        public Int32 cantidadfoja { get; set; }
        public string estado { get; set; }
        public string idescriturapublica { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }

    }

    public class ResultadoBPN : Documento
    {
        public ContenidoResultadoBPN contenido { get; set; }

    }

    public class ContenidoSolicitudExpedicionFirma
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string cliente { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string url { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }

    }

    public class SolicitudExpedicionFirma : Documento
    {
        public ContenidoSolicitudExpedicionFirma contenido { get; set; }

    }

    public class ContenidoConclusionFirma
    {
        public string codigo { get; set; }
        public string idescriturapublica { get; set; }
        public string idnotario { get; set; }
        public string idcliente { get; set; }
        public Int32 cantidadfoja { get; set; }
        public double precio { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }

    }
    public class ConclusionFirma : Documento
    {
        public ContenidoConclusionFirma contenido { get; set; }
    }

    
    public class OficioDesignacionNotario : Documento
    {
        [BsonElement("evaluacion")]
        public Evaluacion evaluacion { get; set; }
        public ContenidoOficioDesignacionNotario contenido { get; set; } = new ContenidoOficioDesignacionNotario();
    }
    public class ContenidoOficioDesignacionNotario
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string lugaroficionotarial { get; set; }
        public string idusuario { get; set; }
        public string idnotario { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }
    }

    public class ContenidoAperturamientoDisciplinario
    {
        public string codigo { get; set; }
        public string idnotario { get; set; }
        public string idfiscal { get; set; }
        public string nombredenunciante { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechainicioaudiencia { get; set; } = DateTime.Now;
        public DateTime fechafinaudiencia { get; set; } = DateTime.Now;
        public List<string> participantes { get; set; } = new List<string>();
        public string lugaraudiencia { get; set; }
        public List<string> hechosimputados { get; set; } = new List<string>();
        //public string estado { get; set; }
        public string url { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }
    }
    public class AperturamientoDisciplinario : Documento
    {
        public ContenidoAperturamientoDisciplinario contenido { get; set; }
    }

    public class ContenidoSolicitudExpedienteNotario
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string idnotario { get; set; }
        public DateTime fechaemision { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }
    }
    public class SolicitudExpedienteNotario : Documento
    {
        public ContenidoSolicitudExpedienteNotario contenido { get; set; }
    }

    public class ContenidoDictamen
    {
        public string codigo { get; set; }
        public string nombredenunciante { get; set; }
        public string descripcion { get; set; }
        public string titulo { get; set; }
        public List<string> observaciones { get; set; }
        public string conclusion { get; set; }
        public List<string> recomendaciones { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }
    }

    public class Dictamen : Documento
    {
        public ContenidoDictamen contenido { get; set; }
    }

    public class ContenidoResolucion
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechainicioaudiencia { get; set; }
        public DateTime fechafinaudiencia { get; set; }
        public List<string> participantes { get; set; }
        public string sancion { get; set; }
        public string url { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }
    }
    public class Resolucion : Documento
    {
        [BsonElement("evaluacion")]
        public Evaluacion evaluacion { get; set; }
        public ContenidoResolucion contenido { get; set; }
    }
    public class ContenidoApelacion
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaapelacion { get; set; }
        public string url { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }

    }
    public class Apelacion : Documento
    {
        [BsonElement("evaluacion")]
        public Evaluacion evaluacion { get; set; }
        public ContenidoApelacion contenido { get; set; }
    }

    public class ContenidoSolicitudInicial
    {
        public string descripcion { get; set; }
        public string titulo { get; set; }

    }

    public class SolicitudInicial : Documento
    {
        public ContenidoSolicitudInicial contenido { get; set; }
    }

    public class ContenidoEntregaExpedienteNotario
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string titulo { get; set; }
        public string idnotario { get; set; }
        public string firma { get; set; }
        public string urlGenerado { get; set; }
    }

    public class EntregaExpedienteNotario : Documento
    {
        public ContenidoEntregaExpedienteNotario contenido { get; set; }
    }
}
