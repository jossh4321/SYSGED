using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Helpers
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly string connectionString;

        public AzureFileStorage(ISysgedDatabaseSettings settings)
        {
            connectionString = settings.StorageConnectionString;
        }
        public async Task deleteFile(string path, string containerName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var clientService = account.CreateCloudBlobClient();
            var contenedor = clientService.GetContainerReference(containerName);

            var blobName = Path.GetFileName(path);
            var blob = contenedor.GetBlobReference(blobName);
            await blob.DeleteIfExistsAsync();

        }

        public async  Task<string> editFile(byte[] contenido, string extension, string nombreContenedor, string rutaArchivoActual)
        {
            if (!string.IsNullOrWhiteSpace(rutaArchivoActual))
            {
                await deleteFile(rutaArchivoActual, nombreContenedor);
            }
            return await saveFile(contenido, extension, nombreContenedor);
        }

        public async Task<string> saveFile(byte[] content, string extension, string containerName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var clientService = account.CreateCloudBlobClient();
            var contenedor = clientService.GetContainerReference(containerName);
            await contenedor.CreateIfNotExistsAsync();
            await contenedor.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var filename = $"{Guid.NewGuid()}.{extension}";
            var blob = contenedor.GetBlockBlobReference(filename);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = "image/jpg";
            await blob.SetPropertiesAsync();
            return blob.Uri.ToString();
        }

        public async Task<string> saveDoc(byte[] content, string extension, string containerName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var clientService = account.CreateCloudBlobClient();
            var contenedor = clientService.GetContainerReference(containerName);
            await contenedor.CreateIfNotExistsAsync();
            await contenedor.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var filename = $"{Guid.NewGuid()}.{extension}";
            var blob = contenedor.GetBlockBlobReference(filename);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = "application/pdf";
            await blob.SetPropertiesAsync();
            return blob.Uri.ToString();
        }
    }
}
