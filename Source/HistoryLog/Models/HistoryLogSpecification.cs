using System;
using System.Linq.Expressions;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Supplemental.Models;
using ReusableLibrary.Supplemental.System;

namespace ReusableLibrary.HistoryLog.Models
{
    [Serializable]
    public class HistoryLogSpecification : ISpecification<HistoryLogItem>, IKeyProvider
    {
        public HistoryLogSpecification()
        {
            Events = new short[] { };
            var today = DateTime.Today.BeforeMidnight();
            DateRange = new Range<DateTime>(today.AddDays(-7).Date, today);
            EventRange = new Range<short>();
        }

        public string Username { get; set; }

        public short EventId { get; set; }

        public short[] Events { get; set; }

        public Range<short> EventRange { get; set; }

        public Range<DateTime> DateRange { get; set; }

        public string RelatedTo { get; set; }

        #region ISpecification<HistoryLogItem> Members

        public virtual Expression<Func<HistoryLogItem, bool>> IsSatisfied()
        {
            return SatisfyEvents(SatisfyUsername()
                .And(SatisfyDateRange())
                .And(SatisfyRelatedTo()));
        }

        #endregion

        #region IKeyProvider Members

        public virtual string ToKeyString()
        {
            return HistoryLogKeyHelper.Specification(this);
        }

        #endregion

        protected virtual Expression<Func<HistoryLogItem, bool>> SatisfyUsername()
        {
            if (Username == null)
            {
                return item => true;
            }

            return item => item.Username == Username;
        }

        protected virtual Expression<Func<HistoryLogItem, bool>> SatisfyDateRange()
        {
            return item => item.Timestamp >= DateRange.From && item.Timestamp <= DateRange.To;
        }

        protected virtual Expression<Func<HistoryLogItem, bool>> SatisfyRelatedTo()
        {
            if (RelatedTo == null)
            {
                return item => true;
            }

            return item => item.RelatedTo == RelatedTo;
        }

        protected virtual Expression<Func<HistoryLogItem, bool>> SatisfyEvents(Expression<Func<HistoryLogItem, bool>> strategy)
        {
            if (Events.Length == 0)
            {
                if (EventId != 0)
                {
                    strategy = strategy.And(item => item.EventId == EventId);
                }
                else
                {
                    if (EventRange.From != 0)
                    {
                        strategy = strategy.And(item => item.EventId >= EventRange.From);
                    }

                    if (EventRange.To != 0)
                    {
                        strategy = strategy.And(item => item.EventId <= EventRange.To);
                    }
                }
            }
            else
            {
                strategy = strategy.AndOneOf(Events, id => item => item.EventId == id);
            }

            return strategy;
        }
    }
}
