using Microsoft.Azure.Cosmos.Table;
using ScraperTask.Models;

namespace ScraperTask.Services
{
    public class QueryTablesByDateRange : IQueryTablesByDateRange
    {
        public TableQuery<StatusEntity> GetTableQueryByDateRange(string period)
        {
            var rangeSt = period.Split('t');
            //eg. 23-10-2020-23:40:00t24-10-2020-00:28:00
            var rangeStart = rangeSt[0]; 
            var rangeEnd = rangeSt[1]; 
            TableQuery<StatusEntity> tQueryByDateRange = new TableQuery<StatusEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("DateTimeNow", QueryComparisons.GreaterThanOrEqual, rangeStart),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("DateTimeNow", QueryComparisons.LessThanOrEqual, rangeEnd)
                    ));

            return tQueryByDateRange;
        }
    }
}
