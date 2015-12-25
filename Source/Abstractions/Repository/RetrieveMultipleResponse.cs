using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReusableLibrary.Abstractions.Repository
{
    [Serializable]
    public class RetrieveMultipleResponse<TDomainObject>
    {
        public RetrieveMultipleResponse(IEnumerable<TDomainObject> items, bool hasMore)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            var list = items as IList<TDomainObject>;            

            if (list.Count == 0 && hasMore)
            {
                throw new ArgumentOutOfRangeException("hasMore");
            }

            Items = new ReadOnlyCollection<TDomainObject>(list != null ? list : new List<TDomainObject>(items));
            HasMore = hasMore;
        }

        public IEnumerable<TDomainObject> Items { get; private set; }

        public bool HasMore { get; private set; }
    }
}
