using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    class GetTablesAsync : IGetTablesAsync
    {
        private readonly TableSettings _tableSettings;
        private IConfiguration _configuration;

        public GetTablesAsync(TableSettings tableSettings, IConfiguration configuration)
        {
            _tableSettings = tableSettings;
            _configuration = configuration;
        }
        public async Task<CloudTable> GetTableAsync()
        {
            var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var tableName = "Scraper";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}
