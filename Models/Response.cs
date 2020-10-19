using System;
using System.Collections.Generic;
using System.Text;

namespace ScraperTask.Models
{
    class Response
    {
        public int Count { get; set; }
        public Entries[] Entries {get; set;}
    }

}
