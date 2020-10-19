using Microsoft.Azure.Cosmos.Table;
using System.Net;

namespace ScraperTask.Models
{
    public class StatusEntity : TableEntity
    {
        public StatusEntity() 
        {
        }

        public StatusEntity(string status, string time)
        {
            PartitionKey = status;
            RowKey = time;
        }

        public string Status { get; set; }
        public string Time { get; set;  }
    }
}
