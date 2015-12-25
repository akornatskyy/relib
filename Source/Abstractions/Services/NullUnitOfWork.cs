using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Services
{
    public sealed class NullUnitOfWork : Disposable, IUnitOfWork
    {
        #region IUnitOfWork Members

        public void Commit()
        {
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
        }
    }
}
