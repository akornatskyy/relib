using System;

using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Web.Mvc
{
    [Serializable]
    public class SearchViewData<TSpecification>
        where TSpecification : new()
    {
        public SearchViewData()
        {
            Specification = new TSpecification();
        }

        public SearchViewData(TSpecification specification)
        {
            Specification = specification;
        }

        public TSpecification Specification { get; set; }
    }
}
