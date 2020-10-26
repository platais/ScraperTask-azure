using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    class AddToBlob : IAddToBlob 
    {
        private readonly IGetBlobsAsync _getBlobsAsync;
        public AddToBlob(IGetBlobsAsync getBlobsAsync)
        {
            _getBlobsAsync = getBlobsAsync;
        }
        public async Task WriteToBlob(string source, ILogger log)
        {
            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(source ?? ""));
            BlobClient blob = await _getBlobsAsync.GetBlobAsync();
            
            log.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blob.Uri);
            await blob.UploadAsync(jsonStream);
        }
    }
}
