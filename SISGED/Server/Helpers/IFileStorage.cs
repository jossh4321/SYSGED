using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Helpers
{
    public interface IFileStorage
    {
        Task<string> editFile(byte[] contenido, string extension,
            string nombreContenedor, string rutaArchivo);
        Task deleteFile(string ruta,string nombreContenedor);
        Task<string> saveFile(byte[] contenido, string extension,
            string nombreContenedor);

        Task<string> saveDoc(byte[] contenido, string extension,
            string nombreContenedor);

    }
}
