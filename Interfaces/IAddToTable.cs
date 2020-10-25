using Microsoft.Extensions.Logging;
using ScraperTask.Models;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IAddToTable
    {
        Task WriteToTable(StatusEntity entity, ILogger log);
    }
}
