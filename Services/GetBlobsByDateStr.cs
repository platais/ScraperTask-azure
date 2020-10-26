using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public class GetBlobsByDateStr : IGetBlobsByDateStr
    {
        public async Task<string> GetBlobByDateStr(string str)
        {
            var containerName = "log-container";
            var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            var blobName = "logBlob" + str + ".json";
            BlobServiceClient blobServClient = new BlobServiceClient(connString);
            BlobContainerClient container = blobServClient.GetBlobContainerClient(containerName);
            BlobClient blob = container.GetBlobClient(blobName);
            var line = "";
            if (await blob.ExistsAsync())
            {
                var response = await blob.DownloadAsync();
                using (var streamReader = new StreamReader(response.Value.Content))
                {
                    line = await streamReader.ReadToEndAsync();
                }
            }
            return line;
        }
    }
}
