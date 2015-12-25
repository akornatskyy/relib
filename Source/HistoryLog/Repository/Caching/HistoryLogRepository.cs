using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.Caching;

namespace ReusableLibrary.HistoryLog.Repository.Caching
{
    public sealed class HistoryLogRepository : Decorated.HistoryLogRepository
    {
        private readonly ICache m_cache;

        public HistoryLogRepository(IHistoryLogRepository innerRepository, ICache cache)
            : base(innerRepository)
        {
            m_cache = cache;

            EventsLifetime = TimeSpan.FromHours(1);
            HistoryLifetime = TimeSpan.FromMinutes(15);
        }

        public string Keyspace { get; set; }

        public TimeSpan EventsLifetime { get; set; }

        public TimeSpan HistoryLifetime { get; set; }

        public override void Add(HistoryLogItem item)
        {
            base.Add(item);
            HistoryDependency(item.Username).Remove();
        }

        public override IEnumerable<HistoryLogEvent> ListEvents(string languageCode)
        {
            var key = string.Concat(Keyspace, HistoryLogKeyHelper.ListEvents(languageCode));
            return DefaultCache.Instance.Get(key, () => base.ListEvents(languageCode), EventsLifetime);
        }

        public override IDictionary<short, HistoryLogEvent> MapEvents(string languageCode)
        {
            var key = string.Concat(Keyspace, HistoryLogKeyHelper.MapEvents(languageCode));
            return DefaultCache.Instance.Get(key, () => base.MapEvents(languageCode), EventsLifetime);
        }

        public override RetrieveMultipleResponse<HistoryLogItem> RetrieveMultiple(RetrieveMultipleRequest<HistoryLogSpecification> request)
        {
            var key = string.Concat(Keyspace, request.ToKeyString());
            return m_cache.Get(key, () => base.RetrieveMultiple(request), HistoryLifetime,
                HistoryDependency(request.Specification.Username));
        }

        public override IEnumerable<HistoryLogReport> CountByDate(HistoryLogSpecification specification)
        {
            var key = string.Concat(Keyspace, specification.ToKeyString());
            return m_cache.Get(key, () => base.CountByDate(specification), HistoryLifetime,
                HistoryDependency(specification.Username));
        }

        private LinkedCacheDependency HistoryDependency(string username)
        {
            var key = string.Concat(Keyspace, HistoryLogKeyHelper.Dependency(username));
            return new LinkedCacheDependency(m_cache, key, DateTime.Now.Add(HistoryLifetime));
        }
    }
}
