using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    class GetBlobsAsync : IGetBlobsAsync
    {
        private readonly BlobSettings _storageSettings;
        private IConfiguration _configuration;

        public GetBlobsAsync(BlobSettings storageSettings, IConfiguration configuration)
        {
            _storageSettings = storageSettings;
            _configuration = configuration;
        }
        public async Task<BlobClient> GetBlobAsync()
        {   //_configuration.GetConnectionString("AzureWebJobsStorage");
            var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            //_storageSettings.ContainerName;
            var containerName = "log-container";
            var newBlobName = "logBlob" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss") + ".json";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connString);

            BlobContainerClient container;
            try
            {
                container = await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            catch
            {
                container = blobServiceClient.GetBlobContainerClient(containerName);
            }
            
            BlobClient blob = container.GetBlobClient(newBlobName);
            return blob;
        }
    }
}
