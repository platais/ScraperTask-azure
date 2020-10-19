using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ScraperTask.Models;

namespace ScraperTask
{

    public class ApiScraper
    {
        static readonly HttpClient client = new HttpClient();
        //Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");//"
        internal static string connString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1";

        [FunctionName("ApiScraper")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Reading at: {DateTime.Now}");
            var url = "https://api.publicapis.org/random?auth=null";

            try
            {
                var response = await client.GetAsync(url);
                var payload = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode;

                Response data = new Response();
                data = JsonConvert.DeserializeObject<Response>(payload);

                //log.LogInformation(response.ToString());
                log.LogInformation(status.ToString());
                foreach (var entry in data.Entries)
                {
                    log.LogInformation(entry.Link);
                }
                await WriteToBlob(payload, log);
            }
            catch (HttpRequestException e)
            {
                log.LogInformation("exception: " + e);
            }
        }
        private static async Task WriteToBlob(string source, ILogger log)
        {
            string localPath = @"./StorageTmp/";
            var fileName = "scraperFile" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            var blobName = "logBlob" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
            var blobContainerName = "log-container-" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");

            //write a text to a file
            await File.WriteAllTextAsync(localFilePath, source);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connString);
            BlobContainerClient container = await blobServiceClient.CreateBlobContainerAsync(blobContainerName);
            //reference to a blob
            BlobClient blob = container.GetBlobClient(blobName);

            log.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blob.Uri);
            //open file and uload its data
            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blob.UploadAsync(uploadFileStream);
        }

        private static async Task WriteToTable(String source, ILogger log)
        {

        }
    }
}

