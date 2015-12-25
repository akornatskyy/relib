using System.Data.Linq.Mapping;

namespace ReusableLibrary.Supplemental.Repository
{
    public interface IMappingSourceProvider
    {
        MappingSource MappingSource { get; }
    }
}
