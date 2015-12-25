using System;
using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public interface IViewDataAccessor
    {
        ViewDataDictionary ViewData { get; }
    }
}
