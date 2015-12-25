using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.HistoryLog.Models;

namespace ReusableLibrary.HistoryLog.Services
{
    public interface IHistoryLogService
    {
        IEnumerable<HistoryLogEvent> Events { get; }

        IDictionary<short, HistoryLogEvent> EventMap { get; }

        IPagedList<HistoryLogItem> SearchHistory(HistoryLogSpecification specification, int? pageIndex, int? pageSize);
    }
}
