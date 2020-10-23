using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScraperTask.Functions
{
    public class BlobStorage
    {
        private static async Task CreateBlobContainerAsync(BlobServiceClient blobServiceClient)
        {
            var containerName = "log-container";

            try
            {
                BlobContainerClient container = await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            catch (RequestFailedException e)
            {
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            }

        }
        public static async Task WriteToBlob(string connString, string source, ILogger log)
        {
            var containerName = "log-container";
            var blobName = "logBlob" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".json";
            
            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(source ?? ""));

            BlobServiceClient blobServiceClient = new BlobServiceClient(connString);
            await CreateBlobContainerAsync(blobServiceClient);
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blob = container.GetBlobClient(blobName);

            log.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blob.Uri);

            await blob.UploadAsync(jsonStream);
        }

    }
}
