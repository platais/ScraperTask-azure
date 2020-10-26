using Microsoft.Azure.Cosmos.Table;
using ScraperTask.Models;

namespace ScraperTask.Services
{
    public interface IQueryTablesByDateRange
    {
        TableQuery<StatusEntity> GetTableQueryByDateRange(string period);
    }
}
