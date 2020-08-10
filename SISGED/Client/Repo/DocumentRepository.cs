using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Client.Repo
{
    public class DocumentRepository : IDocumentRepository
    {
        public List<OpcionDocumento> ObtenerTiposDocsConExp()
        {
            return new List<OpcionDocumento>() {
                    new OpcionDocumento() { label = "Documento de Solicitud de Búsqueda de Protocolo",value="SolicitudBPN" },
                    new OpcionDocumento() { label = "Documento de Oficio de Búsqueda de Protocolo",value="OficioBPN" },
                    new OpcionDocumento() { label = "Documento de Resultado de Búsqueda de Protocolo",value="ResultadoBPN" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Expedición de Firma",value="SolicitudExpedicionFirma" },
                    new OpcionDocumento() { label = "Documento de Oficio de Designación de Notario",value="OficioDesignacionNotario" },
                    new OpcionDocumento() { label = "Documento de Conclusión de Firma",value="ConclusionFirma" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Denuncia",value="SolicitudDenuncia" },
                    new OpcionDocumento() { label = "Documento de Aperturamiento Disciplinario",value="AperturamientoDisciplinario" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Expediente de Notario",value="SolicitudExpedienteNotario" },
                    new OpcionDocumento() { label = "Documento de Entrega de Expediente de Notario",value="EntregaExpedienteNotario" },
                    new OpcionDocumento() { label = "Documento de Dictamen",value="Dictamen" },
                    new OpcionDocumento() { label = "Documento de Resolución",value="Resolucion" },
                    new OpcionDocumento() { label = "Documento de Apelación",value="Apelacion" }};
        }


        public List<OpcionDocumento> ObtenerTiposDocsSinExp()
        {
            return new List<OpcionDocumento>()
            {
                new OpcionDocumento() { label = "Documento de Solicitud de Expedición de Firma",value="SolicitudExpedicionFirma" },
                new OpcionDocumento() { label = "Documento de Solicitud de Denuncia",value="SolicitudDenuncia" },
            };
        }
    }
}
