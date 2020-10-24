using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ScraperTask.Services
{
    public class TableStorage
    {
        public static async Task<CloudTable> GetTableAsync()
        {
            var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var tableName = "Scraper";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
        public static async Task WriteToTable(StatusEntity entity, ILogger log)
        {
            var table = await GetTableAsync();
            log.LogInformation("Inserting into table:\n\t {0}\n", table.Uri);
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            //TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            //StatusEntity insertedEntity = result.Result as StatusEntity;
        }

        public static TableQuery<StatusEntity> GetTableQueryByDateRange(string period)
        {
            var rangeSt = period.Split('t');
            //eg. 23-10-2020-23:40:00t24-10-2020-00:28:00
            var rangeStart = rangeSt[0]; 
            var rangeEnd = rangeSt[1]; 
            TableQuery<StatusEntity> tQueryByDateRange = new TableQuery<StatusEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("DateTimeNow", QueryComparisons.GreaterThanOrEqual, rangeStart),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("DateTimeNow", QueryComparisons.LessThanOrEqual, rangeEnd)
                    ));

            return tQueryByDateRange;
        }
    }
}
