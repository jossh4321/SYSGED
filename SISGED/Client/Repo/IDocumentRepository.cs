using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Client.Repo
{
    interface IDocumentRepository
    {
        List<OpcionDocumento> ObtenerTiposDocs();
    }
}
