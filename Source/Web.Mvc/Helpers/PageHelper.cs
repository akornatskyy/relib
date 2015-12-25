using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class PageHelper
    {
        public static MvcHtmlString ListPager(this HtmlHelper helper)
        {
            return ListPager(helper, null, null, null);
        }

        public static MvcHtmlString ListPager(this HtmlHelper helper, object htmlAttributes)
        {
            return ListPager(helper, null, null, htmlAttributes);
        }

        public static MvcHtmlString ListPager(this HtmlHelper helper, string actionName)
        {
            return ListPager(helper, actionName, null, null);
        }

        public static MvcHtmlString ListPager(this HtmlHelper helper, string actionName, object htmlAttributes)
        {
            return ListPager(helper, actionName, null, htmlAttributes);
        }

        public static MvcHtmlString ListPager(this HtmlHelper helper, string actionName, string controllerName)
        {
            return ListPager(helper, actionName, controllerName, null);
        }

        public static MvcHtmlString ListPageSize(this HtmlHelper helper)
        {
            return ListPageSize(helper, null, null, null);
        }

        public static MvcHtmlString ListPageSize(this HtmlHelper helper, object htmlAttributes)
        {
            return ListPageSize(helper, null, null, htmlAttributes);
        }

        public static MvcHtmlString ListPageSize(this HtmlHelper helper, string actionName)
        {
            return ListPageSize(helper, actionName, null, null);
        }

        public static MvcHtmlString ListPageSize(this HtmlHelper helper, string actionName, object htmlAttributes)
        {
            return ListPageSize(helper, actionName, null, htmlAttributes);
        }

        public static MvcHtmlString ListPageSize(this HtmlHelper helper, string actionName, string controllerName)
        {
            return ListPageSize(helper, actionName, controllerName, null);
        }

        #region Pager Implementation Details

        public static MvcHtmlString ListPager(this HtmlHelper helper, string actionName, string controllerName, object htmlAttributes)
        {
            var viewData = (IListViewData)helper.ViewData.Model;
            var routeValues = new RouteValueDictionary(viewData.Specification);
            return Pager(viewData.Items, new RouteValueDictionary(htmlAttributes), (linkText, pageIndex, pageSize) =>
            {
                return ActionLink(helper, linkText, actionName ?? "List", controllerName, routeValues, pageIndex, pageSize);
            });
        }

        public static MvcHtmlString ListPageSize(this HtmlHelper helper, string actionName, string controllerName, object htmlAttributes)
        {
            var viewData = (IListViewData)helper.ViewData.Model;
            IPagedListState state = viewData.Items;
            var routeValues = new RouteValueDictionary(viewData.Specification);
            return PageSize(state, new RouteValueDictionary(htmlAttributes), (linkText, pageIndex, pageSize) =>
            {
                return ActionLink(helper, linkText, actionName ?? "List", controllerName, routeValues, pageIndex, pageSize);
            });
        }

        public static MvcHtmlString Pager(IPagedListState state, IDictionary<string, object> htmlAttributes, Func<string, int, int, string> getPageLink)
        {
            var settings = state.Settings;
            if (!settings.AlwaysVisible)
            {
                if (state.PageCount == 1)
                {
                    return MvcHtmlString.Empty;
                }
            }

            return Pager(state.PageIndex, state.PageSize != settings.DefaultItemsPerPage ? state.PageSize : 0, settings.PageCount, state.PageCount, htmlAttributes, getPageLink);
        }

        public static MvcHtmlString PageSize(IPagedListState state, IDictionary<string, object> htmlAttributes, Func<string, int, int, string> getPageLink)
        {
            var settings = state.Settings;
            var pageSize = settings.PageSizes;
            if (!settings.AlwaysVisible)
            {
                pageSize = settings.AdjustPageSize(state.TotalItemCount);
                if (pageSize.Length == 0)
                {
                    return MvcHtmlString.Empty;
                }
            }

            return PageSize(state.PageSize, pageSize, htmlAttributes, getPageLink);
        }

        private static MvcHtmlString Pager(int currentPageIndex, int pageSize, int maxShowPages, int totalPageCount, IDictionary<string, object> htmlAttributes, Func<string, int, int, string> getPageLink)
        {
            if (maxShowPages == 0)
            {
                return MvcHtmlString.Empty;
            }

            var ul = new TagBuilder("ul");
            ul.MergeAttributes(htmlAttributes);
            
            var sb = new StringBuilder();
            int seed = currentPageIndex - (currentPageIndex % maxShowPages);
            if (currentPageIndex > 0)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, "<li title=\"{0}\">{1}</li>",
                    GlobalResourceHelper.PreviousPageTitle(),
                    getPageLink(GlobalResourceHelper.PreviousPage(), currentPageIndex - 1, pageSize)));
            }

            if (seed > 0)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, "<li title=\"{0}\">{1}</li>",
                    String.Format(CultureInfo.InvariantCulture, GlobalResourceHelper.PreviousNPagesTitle(), maxShowPages),
                    getPageLink(GlobalResourceHelper.PreviousNPages(), seed - maxShowPages, pageSize)));
            }

            for (int i = seed; i < totalPageCount && i < seed + maxShowPages; i++)
            {
                var li = new TagBuilder("li");
                if (i == currentPageIndex)
                {
                    li.AddCssClass("active");
                }

                li.InnerHtml = getPageLink((i + 1).ToString(CultureInfo.InvariantCulture), i, pageSize);
                sb.Append(li.ToString());
            }

            if (seed + maxShowPages < totalPageCount)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, "<li title=\"{0}\">{1}</li>",
                    String.Format(CultureInfo.InvariantCulture, GlobalResourceHelper.NextNPagesTitle(), maxShowPages),
                    getPageLink(GlobalResourceHelper.NextNPages(), seed + maxShowPages, pageSize)));
            }

            if (currentPageIndex + 1 < totalPageCount)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, "<li title=\"{0}\">{1}</li>",
                    GlobalResourceHelper.NextPageTitle(),
                    getPageLink(GlobalResourceHelper.NextPage(), currentPageIndex + 1, pageSize)));
            }

            ul.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }

        private static MvcHtmlString PageSize(int currentPageSize, int[] pageSizes, IDictionary<string, object> htmlAttributes, Func<string, int, int, string> getPageLink)
        {
            var ul = new TagBuilder("ul");
            ul.MergeAttributes(htmlAttributes);
            var sb = new StringBuilder();
            for (int i = 0; i < pageSizes.Length; i++)
            {
                var pageSize = pageSizes[i];
                var li = new TagBuilder("li");
                if (pageSize == currentPageSize)
                {
                    li.AddCssClass("active");
                }

                li.InnerHtml = getPageLink(pageSize.ToString(CultureInfo.InvariantCulture), 0, i > 0 ? pageSize : 0);
                sb.Append(li.ToString());
            }

            ul.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }

        private static string ActionLink(HtmlHelper helper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValuesPrototype, int pageIndex, int pageSize)
        {
            var routeValues = new RouteValueDictionary();
            foreach (var pair in routeValuesPrototype)
            {
                var formattable = pair.Value as IFormattable;
                if (formattable != null)
                {
                    routeValues.Add(pair.Key, formattable.ToString(null, CultureInfo.CurrentCulture));
                }
                else
                {
                    routeValues.Add(pair.Key, pair.Value);
                }
            }
            
            if (pageIndex > 0)
            {
                routeValues.Add("pageIndex", pageIndex);
            }

            if (pageSize > 0)
            {
                routeValues.Add("pageSize", pageSize);
            }

            return helper.ActionLink(linkText, actionName, controllerName, routeValues, null).ToHtmlString();
        }

        #endregion
    }
}
