using System;

namespace ReusableLibrary.Abstractions.Repository
{
    public interface IRetrieveRepository<TIdentity, TDomainObject>
    {
        TDomainObject Retrieve(TIdentity identity);
    }
}
