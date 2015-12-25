using System;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception[] Errors(Exception ex)
        {
            var list = new List<Exception>();
            list.Add(ex);
            while ((ex = ex.InnerException) != null)
            {
                list.Add(ex);
            }

            return list.ToArray();
        }

        public static TException Find<TException>(Exception ex)
            where TException : Exception
        {
            while (ex != null && !typeof(TException).IsAssignableFrom(ex.GetType()))
            {
                ex = ex.InnerException;
            }

            return (TException)ex;
        }
    }
}
