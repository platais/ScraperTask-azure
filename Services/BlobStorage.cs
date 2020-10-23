﻿using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public class BlobStorage
    {
        public static async Task<BlobClient> GetBlobAsync()
        {
            var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var containerName = "log-container";
            var blobName = "logBlob" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".json";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connString);

            BlobContainerClient container;
            try
            {
                container = await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            catch (RequestFailedException e)
            {
                container = blobServiceClient.GetBlobContainerClient(containerName);
            }

            BlobClient blob = container.GetBlobClient(blobName);
            return blob;
        }
        public static async Task WriteToBlob(string source, ILogger log)
        {
            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(source ?? ""));
            BlobClient blob = await GetBlobAsync(); 
            log.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blob.Uri);
            await blob.UploadAsync(jsonStream);
        }
    }
}
