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
    public class HttpTriggerTable
    {
        private readonly IGetTablesAsync _tableStorage;
        private readonly IQueryTablesByDateRange _tableQuery;
        public HttpTriggerTable(IGetTablesAsync tableStorage, IQueryTablesByDateRange tableQuery)
        {
            _tableStorage = tableStorage;
            _tableQuery = tableQuery;
        }
        [FunctionName("ListAllLogsFromPeriod")]
        public async Task<IActionResult> ListAllLogsFromPeriod(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "entities/")] HttpRequest req, ILogger log)
        {
            string periodReqStr = req.Query["date"];
            
            CloudTable table = await _tableStorage.GetTableAsync();

            var query = _tableQuery.GetTableQueryByDateRange(periodReqStr);
            var statusEntity = await table.ExecuteQuerySegmentedAsync(query, null);

            return new OkObjectResult(statusEntity);
        }

    }
}
