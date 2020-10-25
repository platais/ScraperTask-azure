using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IGetTablesAsync
    {
        Task<CloudTable> GetTableAsync();
    }
}
