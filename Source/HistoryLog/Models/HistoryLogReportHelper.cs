using System;
using System.Collections.Generic;

namespace ReusableLibrary.HistoryLog.Models
{
    public static class HistoryLogReportHelper
    {
        public static IEnumerable<HistoryLogReport> AddMissing(HistoryLogSpecification specification, IList<HistoryLogReport> items)
        {
            var n = Convert.ToInt32(Math.Abs(specification.DateRange.To.Date
                .Subtract(specification.DateRange.From.Date).TotalDays) + 1);
            if (items.Count == n)
            {
                return items;
            }

            var date = specification.DateRange.To.Date;
            for (int i = 0; i < n; i++, date = date.AddDays(-1))
            {
                if (i >= items.Count || items[i].Timestamp != date)
                {
                    items.Insert(i, new HistoryLogReport()
                    {
                        Timestamp = date
                    });
                }
            }

            return items;
        }
    }
}
