using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ScraperTask.Models;

namespace ScraperTask.Functions
{
    public class TableStorage
    {
        public static async Task CreateTable(string connString, ILogger log)
        {

        }
        public static async Task WriteToTable(string connString, StatusEntity entity, ILogger log)
        {
            var tableName = "Scraper";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            //CloudTable table = tableClient.GetTableReference(tableName);
            log.LogInformation("Inserting into table:\n\t {0}\n", table.Uri);
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            StatusEntity insertedEntity = result.Result as StatusEntity;
        }
    }
}
