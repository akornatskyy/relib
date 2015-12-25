using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Repository
{
    public interface IRetrieveMultipleRepository<TSpecification, TDomainObject>
    {
        RetrieveMultipleResponse<TDomainObject> RetrieveMultiple(RetrieveMultipleRequest<TSpecification> request);
    }
}
