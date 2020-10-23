using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace ScraperTask.Controllers
{
    public class BlobClient
    {
        [FunctionName("FetchPayloadFromBlobByLogEntry")]
        public static async Task<string> FetchPayloadFromBlobByLogEntry(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getBlobByDate/")]
            HttpRequest req, ILogger log)
        {
            string request = req.Query["date"];

            //Pageable<TableEntity> entities = tableClient.Query<TableEntity>(filter: "PartitionKey eq 'markers'");

            //// Or using a filter expression
            //Pageable<TableEntity> entities = tableClient.Query<TableEntity>(ent => ent.PartitionKey == "markers");

            //foreach (TableEntity entity in entities)
            //{
            //    Console.WriteLine($"{entity.GetString("Product")}: {entity.GetDouble("Price")}");
            //}


            //blobServiceClient.GetBlobContainerClient("logBlob");
            var link = "C# HTTP trigger function processed a request.";
            log.LogInformation(link);
            return null;
        }
    }
}
