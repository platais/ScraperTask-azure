using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using ScraperTask.Services;
using System.Threading.Tasks;

namespace ScraperTask.Controllers
{
    public class HttpTriggerTable
    {
        [FunctionName("ListAllLogsFromPeriod")]
        public static async Task ListAllLogsFromPeriod(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "entities/")] HttpRequest req, ILogger log)
        {
            string periodReqStr = req.Query["date"];
            
            CloudTable table = await TableStorage.GetTableAsync();

            var query = TableStorage.GetTableQueryByDateRange(periodReqStr);


            foreach (StatusEntity entity in
                await table.ExecuteQuerySegmentedAsync(query, null))
            {
                log.LogInformation(
                    $"\n===============================\n" +
                    $"Status: {entity.PartitionKey}\nDateTime: {entity.Timestamp}\nResponse: {entity.Response}\n" +
                    $"===============================\n");
            }
           
        }

    }
}
