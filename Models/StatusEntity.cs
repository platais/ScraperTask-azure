using Microsoft.Azure.Cosmos.Table;
using System;
using System.Net;

namespace ScraperTask.Models
{
    public class StatusEntity : TableEntity
    {
        public StatusEntity() 
        {
        }

        public StatusEntity(string status, string time, string response)
        {
            PartitionKey = status;
            RowKey = time;
            DateTimeNow = DateTime.Now.ToString("dd-MM-yyyy-HH:mm:ss");
            Response = response;
        }

        public string Status { get; set; }
        public string Time { get; set;  }
        public string DateTimeNow { get; set; }
        public string Response { get; set; }

    }
}
