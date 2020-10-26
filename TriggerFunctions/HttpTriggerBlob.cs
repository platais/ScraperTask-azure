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
    public class HttpTriggerBlob
    {
        private readonly IGetBlobsByDateStr _blobsByStr;
        public HttpTriggerBlob(IGetBlobsByDateStr blobsByStr)
        {
            _blobsByStr = blobsByStr;
        }
        
        [FunctionName("FetchPayloadFromBlobByLogEntry")]
        public async Task<IActionResult> FetchPayloadFromBlobByLogEntry(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getBlobByDate/")]
            HttpRequest req, ILogger log)
        {
            string blobDateStr = req.Query["date"];

            var jsonD = await _blobsByStr.GetBlobByDateStr(blobDateStr);
            var link = "C# HTTP trigger function processed a request.";

            return (ActionResult)new OkObjectResult($"{link} {jsonD}");
        }
    }
}
