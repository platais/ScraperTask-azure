using System.Threading.Tasks;

namespace ScraperTask.Services
{
    public interface IGetBlobsByDateStr
    {
        Task<string> GetBlobByDateStr(string str);
    }
}
