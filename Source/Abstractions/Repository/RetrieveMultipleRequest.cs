using System;
using System.Globalization;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Repository
{
    [Serializable]
    public class RetrieveMultipleRequest<TSpecification> : IKeyProvider
    {
        public RetrieveMultipleRequest(TSpecification specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            Specification = specification;
            PageIndex = 0;
            PageSize = 10;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public TSpecification Specification { get; private set; }

        #region IKeyProvider Members

        public virtual string ToKeyString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}|{2}",
                PageIndex, PageSize, Key.From(Specification));
        }

        #endregion
    }
}
