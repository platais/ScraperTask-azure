using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IAddToBlob
    {
        Task WriteToBlob(string source, ILogger log);
    }
}
