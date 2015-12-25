using System;

namespace ReusableLibrary.Web.Mvc
{
    public interface IPartialViewNameProvider
    {
        string PartialViewName { get; }
    }
}
