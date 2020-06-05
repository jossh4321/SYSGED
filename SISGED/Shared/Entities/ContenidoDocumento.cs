using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Entities
{
    /*[BsonDiscriminator(RootClass =true)]
    [BsonKnownTypes(typeof(SolicitudDenuncia),
        typeof(OficioBPN))]
    public class ContenidoDocumento 
    {
    }

    public class SolicitudDenuncia : Documento
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string nombrecliente { get; set; }
        public DateTime fechaentrega { get; set; }
        public string url { get; set; }
    }
    public class OficioBPN : Documento
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
    }*/

    /*public class SolicitudDenuncia : ContenidoDocumento
    {
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string nombrecliente { get; set; }
        public string fechaentrega { get; set; }
        public string url { get; set; }
    }
    public class OficioBPN : ContenidoDocumento
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
    public class OficioDesignacionNotario : ContenidoDocumento
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecharealizacion { get; set; }
        public string lugaroficionotarial { get; set; }
        public string idusuario { get; set; }
        public string idnotario { get; set; }
    }*/
}
