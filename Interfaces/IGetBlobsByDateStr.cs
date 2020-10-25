using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IGetBlobsByDateStr
    {
        Task<string> GetBlobByDateStr(string str);
    }
}
