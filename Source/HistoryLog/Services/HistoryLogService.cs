using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Services;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.HistoryLog.Repository;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.Repository;

namespace ReusableLibrary.HistoryLog.Services
{
    public sealed class HistoryLogService : AbstractService, IHistoryLogService
    {
        private readonly IHistoryLogRepository m_historyLogRepository;
        private readonly ILazy<IEnumerable<HistoryLogEvent>> m_lazyEvents;
        private readonly ILazy<IDictionary<short, HistoryLogEvent>> m_lazyEventMap;

        public HistoryLogService(IHistoryLogRepository historyLogRepository)
        {
            m_historyLogRepository = historyLogRepository;

            m_lazyEvents = new LazyObject<IEnumerable<HistoryLogEvent>>(() => 
                m_historyLogRepository.ListEvents(CurrentCulture.TwoLetterISOLanguageName));
            m_lazyEventMap = new LazyObject<IDictionary<short, HistoryLogEvent>>(() => 
                m_historyLogRepository.MapEvents(CurrentCulture.TwoLetterISOLanguageName));
        }

        #region IHistoryLogService Members

        public IPagingSettings HistoryLogPagingSettings { get; set; }

        public IEnumerable<HistoryLogEvent> Events
        {
            get { return m_lazyEvents.Object; }
        }

        public IDictionary<short, HistoryLogEvent> EventMap
        {
            get { return m_lazyEventMap.Object; }
        }

        public IPagedList<HistoryLogItem> SearchHistory(HistoryLogSpecification specification, int? pageIndex, int? pageSize)
        {
            return WithValid(specification, () =>
            {
                var result = m_historyLogRepository.RetrieveMultiple<HistoryLogSpecification, HistoryLogItem>(
                    specification, pageIndex, pageSize, HistoryLogPagingSettings);
                var map = EventMap;
                result.ForEach(x => x.Event = map[x.EventId]);
                return result;
            });
        }

        #endregion
    }
}
