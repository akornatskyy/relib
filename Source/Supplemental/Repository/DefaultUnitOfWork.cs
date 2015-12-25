using System.Data;
using System.Data.Linq;

namespace ReusableLibrary.Supplemental.Repository
{
    public sealed class DefaultUnitOfWork : AbstractUnitOfWork<DataContext>
    {
        public DefaultUnitOfWork(DataContext context)
            : base(context)
        {
        }

        public DefaultUnitOfWork(DataContext context, IsolationLevel level)
            : base(context, level)
        {
        }
    }
}
