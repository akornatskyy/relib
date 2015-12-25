using System;
using System.Globalization;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Web.Mvc.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Mvc.Tests.Helpers
{
    public sealed class PageHelperTest
    {
        [Theory]
        [InlineData(@"", false)]
        [InlineData(@"<ul><li class=""active""><a href=""test?pageIndex=0"">1</a></li></ul>", true)]        
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_AlwaysVisible(string expected, bool alwaysVisible)
        {
            // Arrange
            var pagedListState = new PagedListState(0, 10, 5, false)
            {
                Settings = new PagingSettings()
                {
                    AlwaysVisible = alwaysVisible,
                    PageCount = 5
                }
            };

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Equal(expected, html);
        }

        [Theory]
        [InlineData(@"<li class=""active""><a href=""test?pageIndex=0"">1</a></li>", 0)]
        [InlineData(@"<li class=""active""><a href=""test?pageIndex=1"">2</a></li>", 1)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_Active_PageIndex(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Contains(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li><a href=""test?pageIndex=0"">1</a></li>", 2)]
        [InlineData(@"<li><a href=""test?pageIndex=1"">2</a></li>", 2)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_Inactive_PageIndex(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Contains(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li title=""{0}""><a href=""test?pageIndex=0"">{1}</a></li>", 5)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasMore_PreviousPages(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Contains(String.Format(CultureInfo.CurrentUICulture, expected, String.Format(CultureInfo.InvariantCulture, Properties.Resources.PreviousNPagesTitle, pagedListState.Settings.PageCount), Properties.Resources.PreviousNPages), 
                html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li title=""{0}""><a href=""test?pageIndex=10"">{1}</a></li>", 5)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasMore_NextPages(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Contains(String.Format(CultureInfo.CurrentUICulture, expected, String.Format(CultureInfo.InvariantCulture, Properties.Resources.NextNPagesTitle, pagedListState.Settings.PageCount), Properties.Resources.NextNPages),
                html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"...</a></li>", 0)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasNot_MorePages(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 50, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Equal(pagedListState.PageCount, pagedListState.Settings.PageCount);
            Assert.DoesNotContain(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"pageIndex=10"">...</a></li>", 11)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasNot_MorePages_Forward(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.DoesNotContain(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"pageIndex=0"">...</a></li>", 2)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasNot_MorePages_Backward(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.DoesNotContain(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li title=""{0}""><a href=""test?pageIndex=1"">{1}", 0)]
        [InlineData(@"<li title=""{0}""><a href=""test?pageIndex=2"">{1}", 1)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_Has_Next(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Contains(String.Format(CultureInfo.CurrentUICulture, expected, Properties.Resources.NextPageTitle, Properties.Resources.NextPage), 
                html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"</a></li>", 12)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasNot_Next(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.DoesNotContain(Properties.Resources.NextPage + expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li title=""{0}""><a href=""test?pageIndex=0"">{1}", 1)]
        [InlineData(@"<li title=""{0}""><a href=""test?pageIndex=5"">{1}", 6)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_Has_Previous(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.Contains(String.Format(CultureInfo.CurrentUICulture, expected, Properties.Resources.PreviousPageTitle, Properties.Resources.PreviousPage), 
                html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"</a></li>", 0)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void Pager_HasNot_Previous(string expected, int currentPageIndex)
        {
            // Arrange
            var pagedListState = new PagedListState(currentPageIndex, 10, 125, false);

            // Act
            var html = PageHelper.Pager(pagedListState, null, GetPageIndexLink).ToHtmlString();

            // Assert
            Assert.DoesNotContain(Properties.Resources.PreviousPage + expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"", false, 11)]
        [InlineData(@"<ul><li class=""active""><a href=""test"">10</a></li></ul>", true, 5)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void PageSize_AlwaysVisible(string expected, bool alwaysVisible, int pageSize)
        {
            // Arrange
            var pagedListState = new PagedListState(0, 10, pageSize, false)
            {
                Settings = new PagingSettings()
                {
                    AlwaysVisible = alwaysVisible,
                    PageCount = 5,
                    DefaultItemsPerPage = 10,
                    PageSizes = new int[] { 10 }
                }
            };

            // Act
            var html = PageHelper.PageSize(pagedListState, null, GetPageSizeLink).ToHtmlString();

            // Assert
            Assert.Equal(expected, html);
        }

        [Theory]
        [InlineData(@"<li class=""active""><a href=""test"">10</a></li>", 10)]
        [InlineData(@"<li class=""active""><a href=""test?pageSize=25"">25</a></li>", 25)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void PageSize_Active(string expected, int currentPageSize)
        {
            // Arrange
            var pagedListState = new PagedListState(0, currentPageSize, 125, false);

            // Act
            var html = PageHelper.PageSize(pagedListState, null, GetPageSizeLink).ToHtmlString();

            // Assert
            Assert.Contains(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li><a href=""test"">10</a></li>", 50)]
        [InlineData(@"<li><a href=""test?pageSize=25"">25</a></li>", 50)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void PageSize_Inactive(string expected, int currentPageSize)
        {
            // Arrange
            var pagedListState = new PagedListState(0, currentPageSize, 125, false);

            // Act
            var html = PageHelper.PageSize(pagedListState, null, GetPageSizeLink).ToHtmlString();

            // Assert
            Assert.Contains(expected, html, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(@"<li><a href=""test"">10</a></li>", 100)]
        [InlineData(@"<li><a href=""test?pageSize=25"">25</a></li>", 100)]
        [InlineData(@"<li><a href=""test?pageSize=50"">50</a></li>", 100)]
        [Trait(Constants.TraitNames.Helpers, "PageHelper")]
        public static void PageSize_OutOfRange(string expected, int currentPageSize)
        {
            // Arrange
            var pagedListState = new PagedListState(0, currentPageSize, 125, false);

            // Act
            var html = PageHelper.PageSize(pagedListState, null, GetPageSizeLink).ToHtmlString();

            // Assert
            Assert.Contains(expected, html, StringComparison.Ordinal);
        }

        private static string GetPageIndexLink(string linkText, int pageIndex, int pageSize)
        {
            return String.Format(CultureInfo.InvariantCulture, @"<a href=""test?pageIndex={1}"">{0}</a>", linkText, pageIndex);
        }

        private static string GetPageSizeLink(string linkText, int pageIndex, int pageSize)
        {
            if (pageSize == 0)
            {
                return String.Format(CultureInfo.InvariantCulture, @"<a href=""test"">{0}</a>", linkText);
            }

            return String.Format(CultureInfo.InvariantCulture, @"<a href=""test?pageSize={1}"">{0}</a>", linkText, pageSize);
        }
    }
}
