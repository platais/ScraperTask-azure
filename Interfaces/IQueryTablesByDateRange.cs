using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using ScraperTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IQueryTablesByDateRange
    {
        TableQuery<StatusEntity> GetTableQueryByDateRange(string period);
    }
}
