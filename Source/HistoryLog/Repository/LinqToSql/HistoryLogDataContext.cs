using System.Data.Linq;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Supplemental.Repository;

namespace ReusableLibrary.HistoryLog.Repository.LinqToSql
{
    public partial class HistoryLogDataContext
    {
        public HistoryLogDataContext(DbConnectionStringProvider connectionStringProvider)
            : this((connectionStringProvider ?? new DbConnectionStringProvider("Default-HistoryLog")).ConnectionString)
        {
            var options = new DataLoadOptions();
            LoadOptions = options;
        }

        public HistoryLogDataContext(DbConnectionStringProvider connectionStringProvider, IMappingSourceProvider mappingSourceProvider)
            : this((connectionStringProvider ?? new DbConnectionStringProvider("Default-HistoryLog")).ConnectionString,
                mappingSourceProvider.MappingSource)
        {
            var options = new DataLoadOptions();
            LoadOptions = options;
        }
    }
}
