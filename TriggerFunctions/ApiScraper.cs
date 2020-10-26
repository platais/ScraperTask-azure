using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScraperTask.Services;
using ScraperTask.Models;

namespace ScraperTask
{

    public class ApiScraper
    {
        static readonly HttpClient client = new HttpClient();
        private readonly IAddToBlob _addToBlob;
        private readonly IAddToTable _addToTable;

        public ApiScraper(IAddToBlob addToBlob, IAddToTable addToTable)
        {
            _addToBlob = addToBlob;
            _addToTable = addToTable;
        }

        [FunctionName("ApiScraper")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Reading at: {DateTime.Now}");
            var url = "https://api.publicapis.org/random?auth=null";

            try
            {
                var response = await client.GetAsync(url);
                var responseStr = response.ToString();
                var payload = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode.ToString();
                var timeDateStr = JsonConvert.SerializeObject(response.Headers.Date);

                var SE = new StatusEntity(status, timeDateStr, responseStr);

                await _addToTable.WriteToTable(SE, log);
                await _addToBlob.WriteToBlob(payload, log);
            }
            catch (HttpRequestException e)
            {
                log.LogInformation("exception: " + e);
            }
        }
    }
}

