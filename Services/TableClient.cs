using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperTask.Controllers
{
    public class TableClient
    {
        [FunctionName("ListAllLogsFromPeriod")]
        public static async Task ListAllLogsFromPeriod(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "entities/")] HttpRequest req, ILogger log)
        {
            string request = req.Query["date"];
            log.LogInformation(req.ToString());
            
            string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference("Scraper");
            //
            var partKey = "OK";
            var rowKey = "2020-10-23T12:27:06+00:00";
            TableOperation retrieveOperation = TableOperation.Retrieve<StatusEntity>(partKey, rowKey);
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            StatusEntity status = result.Result as StatusEntity;

            //IQueryable<StatusEntity> linqQuery = table.CreateQuery<StatusEntity>()
            //.Where(x => x.Time == request)
            //.Select(x => new StatusEntity() { PartitionKey = x.Status, RowKey = x.Time, DateTimeNow = x.DateTimeNow, Response = x.Response });
            //
            //var id 
            //var from DateTime = DateTime.Parse(req.Query("id"));

            TableQuery<StatusEntity> rangeQuery = new TableQuery<StatusEntity>().Where(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
            "OK"),
            TableOperators.And,
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal,
            request)));

            // Execute the query and loop through the results
            foreach (StatusEntity entity in
                await table.ExecuteQuerySegmentedAsync(rangeQuery, null))
            {
                log.LogInformation(
                    $"{entity.PartitionKey}\t{entity.RowKey}\t{entity.Timestamp}\t{entity.Response}");
            }

            //return req.Query.ToString();
        }
    }
}
