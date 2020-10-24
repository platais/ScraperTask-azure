using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ScraperTask.Services;
using System.Threading.Tasks;

namespace ScraperTask.Controllers
{
    public class BlobTriggerClient
    {
        [FunctionName("FetchPayloadFromBlobByLogEntry")]
        public static async Task FetchPayloadFromBlobByLogEntry(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getBlobByDate/")]
            HttpRequest req, ILogger log)
        {
            string blobDateStr = req.Query["date"];

            var jsonD = await BlobStorage.GetBlobByDateStr(blobDateStr);
            var link = "C# HTTP trigger function processed a request.";
            log.LogInformation($"{link} {jsonD}");
        }
    }
}
