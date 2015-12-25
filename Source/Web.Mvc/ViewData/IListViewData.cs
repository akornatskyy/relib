using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Web.Mvc
{
    public interface IListViewData
    {
        object Specification { get; }

        IPagedListState Items { get; }
    }
}
