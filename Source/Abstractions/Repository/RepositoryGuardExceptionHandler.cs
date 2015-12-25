using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Repository
{
    public sealed class RepositoryGuardExceptionHandler : IExceptionHandler
    {
        public int[] Ignore { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var rgex = ex as RepositoryGuardException;
            if (rgex == null || Ignore == null)
            {
                return false;
            }

            return EnumerableHelper.Contains(Ignore, rgex.Number);
        }

        #endregion
    }
}
