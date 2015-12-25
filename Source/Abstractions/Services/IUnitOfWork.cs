using System;

namespace ReusableLibrary.Abstractions.Services
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates 
    /// the writing out of changes and the resolution of concurrency problems.
    /// 
    /// see http://martinfowler.com/eaaCatalog/unitOfWork.html
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
