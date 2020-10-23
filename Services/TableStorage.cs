using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System;
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
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            StatusEntity insertedEntity = result.Result as StatusEntity;
        }
    }
}
