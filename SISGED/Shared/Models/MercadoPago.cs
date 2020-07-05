namespace SISGED.Shared.Models
{
    public class MercadoPagoRequest
    {
        public string aplicacion { get; set; }
        public string codigoreferencia { get; set; }
        public string descripcion { get; set; }
        public string monto { get; set; }
    }

    public class MercadoPagoResponse
    {
        public string referencia { get; set; }
        public string estado { get; set; }
        public string estadodetalle { get; set; }
        public string tipooperacion { get; set; }
        public string tipopago { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
    }
}