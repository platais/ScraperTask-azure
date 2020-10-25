using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IGetBlobsAsync
    {
        Task<BlobClient> GetBlobAsync();
    }
}
