using System;
using System.Collections.Generic;
using System.Linq;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.Repository;
using ReusableLibrary.Supplemental.System;

namespace ReusableLibrary.HistoryLog.Repository.LinqToSql
{
    public sealed partial class HistoryLogRepository : IHistoryLogRepository
    {
        private readonly HistoryLogDataContext m_context;

        public HistoryLogRepository(HistoryLogDataContext context)
        {
            m_context = context;
        }

        #region IHistoryLogRepository Members

        public IEnumerable<HistoryLogEvent> ListEvents(string languageCode)
        {
            return FindAllEventsQuery(m_context.Mapping.DatabaseName).Invoke(m_context).ToArray();
        }

        public IDictionary<short, HistoryLogEvent> MapEvents(string languageCode)
        {
            return ListEvents(languageCode).ToDictionary<short, HistoryLogEvent>();
        }

        public void Add(HistoryLogItem item)
        {
            var hosts = item.Hosts;
            var x = new Entities.Item()
            {
                OriginatorId = (item.Username ?? string.Empty).WrapAt(50),
                Timestamp = item.Timestamp,
                EventId = item.EventId,
                Ip = hosts[0],
                RelatedTo = (item.RelatedTo ?? string.Empty).WrapAt(50).NullEmpty(),
                Arguments = (item.Arguments ?? string.Empty).WrapAt(255).NullEmpty()
            };
            m_context.Items.InsertOnSubmit(x);

            for (int i = 1; i < hosts.Length; i++)
            {
                m_context.ItemExtraIps.InsertOnSubmit(new Entities.ItemExtraIp()
                {
                    Item = x,
                    Ip = hosts[i]
                });
            }
        }

        #endregion

        #region IRetrieveMultipleRepository<HistoryLogSpecification,HistoryLogItem> Members

        public RetrieveMultipleResponse<HistoryLogItem> RetrieveMultiple(RetrieveMultipleRequest<HistoryLogSpecification> request)
        {
            return AllItems()
                .Satisfy(request.Specification)
                .SatisfyPaging(request.PageIndex, request.PageSize)
                .ToResponse(request.PageSize);
        }

        #endregion

        public IEnumerable<HistoryLogReport> CountByDate(HistoryLogSpecification specification)
        {
            return AllItems()
                .Satisfy(specification)
                .GroupBy(item => item.Timestamp.Date)
                .Select(x => new HistoryLogReport()
                {
                    Timestamp = x.Key,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        private IQueryable<HistoryLogItem> AllItems()
        {
            return from x in m_context.Items
                   orderby x.Timestamp descending, x.ItemId descending
                   select new HistoryLogItem()
                   {
                       Arguments = x.Arguments,
                       EventId = x.EventId,
                       Hosts = new int[] { x.Ip },
                       RelatedTo = x.RelatedTo,
                       Timestamp = x.Timestamp,
                       Username = x.OriginatorId
                   };
        }
    }
}
