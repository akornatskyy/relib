using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Abstractions.Services;

namespace ReusableLibrary.Supplemental.Repository
{
    public abstract class AbstractUnitOfWork<TDataContext> : Disposable, IUnitOfWork
        where TDataContext : DataContext
    {
        private readonly TDataContext m_context;
        private bool m_disposed;

        protected AbstractUnitOfWork(TDataContext context)
            : this(context, IsolationLevel.ReadCommitted)
        {
        }

        protected AbstractUnitOfWork(TDataContext context, IsolationLevel level)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            m_context = context;
            if (m_context.Connection.State != ConnectionState.Open)
            {
                m_context.Connection.Open();
            }

            if (m_context.Transaction != null && m_context.Transaction.Connection != null)
            {
                throw new InvalidOperationException("Nested database transactions are not supported");
            }

            m_context.Transaction = m_context.Connection.BeginTransaction(level);
        }

        #region IUnitOfWork Members

        public virtual void Commit()
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (m_context.Transaction.Connection == null)
            {
                throw new InvalidOperationException("This transacton has been commited already");
            }

            if (m_context.Transaction.Connection != null)
            {
                try
                {
                    m_context.SubmitChanges();
                    m_context.Transaction.Commit();
                }
                catch (ChangeConflictException ccex)
                {
                    throw new RepositoryGuardException(Properties.Resources.ErrorChangeConflict, ccex);
                }
                catch (DbException dbex)
                {
                    throw new RepositoryFailureException(Properties.Resources.ErrorUnspecified, dbex);
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_disposed = true;
                if (m_context.Transaction != null && m_context.Transaction.Connection != null)
                {
                    m_context.Transaction.Rollback();
                }

                if (m_context.Connection.State == ConnectionState.Open)
                {
                    m_context.Connection.Close();
                }
            }
        }
    }
}
