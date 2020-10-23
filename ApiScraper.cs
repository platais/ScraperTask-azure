using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json;
using ScraperTask.Functions;
using ScraperTask.Models;
using CloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;

namespace ScraperTask
{

    public class ApiScraper
    {
        static readonly HttpClient client = new HttpClient();
        public static string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        [FunctionName("ApiScraper")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Reading at: {DateTime.Now}");
            var url = "https://api.publicapis.org/random?auth=null";

            try
            {
                var response = await client.GetAsync(url);
                var responseStr = response.ToString();
                var payload = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode.ToString();
                var timeDateStr = JsonConvert.SerializeObject(response.Headers.Date);

                var SE = new StatusEntity(status, timeDateStr, responseStr);

                await TableStorage.WriteToTable(connString, SE, log);
                await BlobStorage.WriteToBlob(connString, payload, log);
            }
            catch (HttpRequestException e)
            {
                log.LogInformation("exception: " + e);
            }

        }





    }
}

