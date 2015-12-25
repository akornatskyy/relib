using System;
using System.Data.Linq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Abstractions.Services;
using ReusableLibrary.Supplemental.Repository;

namespace ReusableLibrary.QualityAssurance.Repository
{
    public abstract class AbstractRepositoryTest<TRepository, TDataContext> : Disposable
        where TRepository : class
        where TDataContext : DataContext, new()
    {
        protected AbstractRepositoryTest()
            : this(null, null, true)
        {
        }

        protected AbstractRepositoryTest(DbConnectionStringProvider connectionStringProvider)
            : this(connectionStringProvider, null, true)
        {
        }

        protected AbstractRepositoryTest(DbConnectionStringProvider connectionStringProvider, bool autorollback)
            : this(connectionStringProvider, null, autorollback)
        {
        }

        protected AbstractRepositoryTest(DbConnectionStringProvider connectionStringProvider, IMappingSourceProvider mappingSourceProvider)
            : this(connectionStringProvider, mappingSourceProvider, true)
        {
        }

        protected AbstractRepositoryTest(DbConnectionStringProvider connectionStringProvider, IMappingSourceProvider mappingSourceProvider, bool autorollback)
        {
            AutoRollback = autorollback;

            if (connectionStringProvider == null)
            {
                DataContext = new TDataContext();
            }
            else
            {
                if (mappingSourceProvider == null)
                {
                    DataContext = Activator.CreateInstance(typeof(TDataContext), connectionStringProvider) as TDataContext;
                }
                else
                {
                    DataContext = Activator.CreateInstance(typeof(TDataContext), connectionStringProvider, mappingSourceProvider) as TDataContext;
                }
            }

            UnitOfWork = new DefaultUnitOfWork(DataContext);
            Repository = Activator.CreateInstance(typeof(TRepository), DataContext) as TRepository;
        }

        public void SubmitChanges()
        {
            DataContext.SubmitChanges();
        }

        protected bool AutoRollback { get; private set; }

        protected TDataContext DataContext { get; private set; }

        protected TRepository Repository { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!AutoRollback)
                {
                    UnitOfWork.Commit();
                }

                UnitOfWork.Dispose();
                UnitOfWork = null;
                Repository = null;
                DataContext.Dispose();
            }
        }

        private IUnitOfWork UnitOfWork { get; set; }
    }
}
