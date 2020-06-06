using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Client.Repo
{
    public class DocumentRepository : IDocumentRepository
    {
        public List<OpcionDocumento> ObtenerTiposDocs()
        {
            return new List<OpcionDocumento>() {
                    //new OpcionDocumento() { label = "Documento de Solicitud de Busqueda de Protocolo",value="SolicitudBPN" },
                    new OpcionDocumento() { label = "Documento de Oficio de Busqueda de Protocolo",value="OficioBPN" },
                    new OpcionDocumento() { label = "Documento de Resultado de Busqueda de Protocolo",value="ResultadoBPN" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Expedicion de Firma",value="SolicitudExpedicionFirma" },
                    new OpcionDocumento() { label = "Documento de Oficio de Designacion de Notario",value="OficioDesignacionNotario" },
                    new OpcionDocumento() { label = "Documento de Conclusion de Firma",value="ConclusionFirma" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Denuncia",value="SolicitudDenuncia" },
                    new OpcionDocumento() { label = "Documento de Aperturamiento Disciplinario",value="AperturamientoDisciplinario" },
                    new OpcionDocumento() { label = "Documento de Solicitud de Expediente de Notario",value="SolicitudExpedienteNotario" },
                    new OpcionDocumento() { label = "Documento de Dictamen",value="Dictamen" },
                    new OpcionDocumento() { label = "Documento de Resolucion",value="Resolucion" },
                    new OpcionDocumento() { label = "Documento de Apelacion",value="Apelacion" }};
        }
    }
}
