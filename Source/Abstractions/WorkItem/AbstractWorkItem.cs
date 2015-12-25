using System;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Services;

namespace ReusableLibrary.Abstractions.WorkItem
{
    public abstract class AbstractWorkItem<TStateBag> : SequencialWorkItem<TStateBag>
        where TStateBag : new()
    {
        protected AbstractWorkItem()
            : base(null)
        {
        }

        protected AbstractWorkItem(string name)
            : base(name)
        {
        }

        public string UnitOfWorkName { get; set; }

        public override bool DoWork(Func2<bool> shutdownCallback)
        {
            try
            {
                if (!AcquireUnitOfWork() || shutdownCallback())
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (!HandleException(ex))
                {
                    // Rethrow exception and preserve the full call stack trace
                    throw;
                }

                return false;
            }

            IUnitOfWork unitOfWork = null;
            try
            {
                try
                {
                    unitOfWork = UnitOfWork.Begin(UnitOfWorkName);
                }
                catch (Exception ex)
                {
                    if (!HandleException(ex))
                    {
                        // Rethrow exception and preserve the full call stack trace
                        throw;
                    }
                }

                if (unitOfWork != null)
                {
                    base.DoWork(shutdownCallback);

                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (!HandleException(ex))
                        {
                            // Rethrow exception and preserve the full call stack trace
                            throw;
                        }
                    }
                }
            }
            finally
            {
                unitOfWork.Dispose();
            }

            return !shutdownCallback();
        }

        public abstract bool OnAcquireUnitOfWork();

        protected virtual bool AcquireUnitOfWork()
        {
            bool succeed;
            using (var unitOfWork = UnitOfWork.Begin(UnitOfWorkName))
            {
                succeed = OnAcquireUnitOfWork();
                unitOfWork.Commit();
            }

            return succeed;
        }
    }
}
