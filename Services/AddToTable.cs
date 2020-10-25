using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    class AddToTable : IAddToTable
    {
        private readonly IGetTablesAsync _tableStorage;
        public AddToTable(IGetTablesAsync tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public async Task WriteToTable(StatusEntity entity, ILogger log)
        {
            var table = await _tableStorage.GetTableAsync();
            log.LogInformation("Inserting into table:\n\t {0}\n", table.Uri);
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            //TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            //StatusEntity insertedEntity = result.Result as StatusEntity;
        }

    }
}
