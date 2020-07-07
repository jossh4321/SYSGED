namespace SISGED.Shared.Helpers
{
    public class DocumentoEstado
    {
        private DocumentoEstado(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static DocumentoEstado Creado { get { return new DocumentoEstado("creado"); } }
        public static DocumentoEstado Modificado { get { return new DocumentoEstado("modificado"); } }
        public static DocumentoEstado Generado { get { return new DocumentoEstado("generado"); } }
        public static DocumentoEstado Pendiente { get { return new DocumentoEstado("pendiente"); } }
        public static DocumentoEstado Aprobado { get { return new DocumentoEstado("aprobado"); } }
    }

    public class SolicitudEstado
    {
        private SolicitudEstado(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static SolicitudEstado Pendiente { get { return new SolicitudEstado("pendiente"); } }
        public static SolicitudEstado Modificado { get { return new SolicitudEstado("modificado"); } }
        public static SolicitudEstado Cancelado { get { return new SolicitudEstado("cancelado"); } }
        public static SolicitudEstado Revisado { get { return new SolicitudEstado("revisado"); } }
        public static SolicitudEstado Finalizado { get { return new SolicitudEstado("finalizado"); } }
    }
}