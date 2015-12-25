using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class ExceptionExtensions
    {
        public static Exception[] Errors(this Exception ex)
        {
            return ExceptionHelper.Errors(ex);
        }

        public static TException Find<TException>(this Exception ex)
            where TException : Exception
        {
            return ExceptionHelper.Find<TException>(ex);
        }
    }
}
