using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.HistoryLog.Models;

namespace ReusableLibrary.HistoryLog.Repository.LinqToSql
{
    public partial class HistoryLogRepository
    {
        private static readonly Singleton<Func<HistoryLogDataContext, IEnumerable<HistoryLogEvent>>>
            g_findAllEventsQuery = new Singleton<Func<HistoryLogDataContext, IEnumerable<HistoryLogEvent>>>(
                name => CompiledQuery.Compile<HistoryLogDataContext, IEnumerable<HistoryLogEvent>>(
                c => from x in c.Events
                     orderby x.EventId
                     select new HistoryLogEvent(x.EventId, x.Name, x.Format)));

        private static readonly Func<string, Func<HistoryLogDataContext, IEnumerable<HistoryLogEvent>>>
            FindAllEventsQuery = name => g_findAllEventsQuery.Instance(name);
    }
}
