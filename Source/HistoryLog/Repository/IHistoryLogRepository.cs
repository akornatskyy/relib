using System.Collections.Generic;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Abstractions.Repository;

namespace ReusableLibrary.HistoryLog.Repository
{
    public interface IHistoryLogRepository :
        IRetrieveMultipleRepository<HistoryLogSpecification, HistoryLogItem>
    {
        IEnumerable<HistoryLogEvent> ListEvents(string languageCode);

        IDictionary<short, HistoryLogEvent> MapEvents(string languageCode);

        void Add(HistoryLogItem item);

        IEnumerable<HistoryLogReport> CountByDate(HistoryLogSpecification specification);
    }
}
