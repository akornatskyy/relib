using System.Collections.Generic;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Abstractions.Repository;

namespace ReusableLibrary.HistoryLog.Repository.Decorated
{
    public abstract class HistoryLogRepository : IHistoryLogRepository
    {
        private readonly IHistoryLogRepository m_innerRepository;

        protected HistoryLogRepository(IHistoryLogRepository innerRepository)
        {
            m_innerRepository = innerRepository;
        }

        #region IHistoryLogRepository Members

        public virtual IEnumerable<HistoryLogEvent> ListEvents(string languageCode)
        {
            return m_innerRepository.ListEvents(languageCode);
        }

        public virtual IDictionary<short, HistoryLogEvent> MapEvents(string languageCode)
        {
            return m_innerRepository.MapEvents(languageCode);
        }

        public virtual void Add(HistoryLogItem item)
        {
            m_innerRepository.Add(item);
        }

        public virtual IEnumerable<HistoryLogReport> CountByDate(HistoryLogSpecification specification)
        {
            return m_innerRepository.CountByDate(specification);
        }

        #endregion

        #region IRetrieveMultipleRepository<HistoryLogSpecification,HistoryLogItem> Members

        public virtual RetrieveMultipleResponse<HistoryLogItem> RetrieveMultiple(RetrieveMultipleRequest<HistoryLogSpecification> request)
        {
            return m_innerRepository.RetrieveMultiple(request);
        }

        #endregion
    }
}
