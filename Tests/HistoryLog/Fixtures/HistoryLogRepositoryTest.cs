using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReusableLibrary.QualityAssurance.Repository;
using ReusableLibrary.HistoryLog.Repository.LinqToSql;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public sealed class HistoryLogRepositoryTest 
        : AbstractRepositoryTest<HistoryLogRepository, HistoryLogDataContext>
    {
    }
}
