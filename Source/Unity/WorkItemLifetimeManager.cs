using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using ReusableLibrary.Abstractions.WorkItem;

namespace ReusableLibrary.Unity
{
    public sealed class WorkItemLifetimeManager : LifetimeManager, IDisposable
    {
        private Guid m_key = Guid.NewGuid();

        ~WorkItemLifetimeManager()
        {
            Dispose(false);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override object GetValue()
        {
            return WorkItemContext.Current.Items[m_key];
        }

        public override void RemoveValue()
        {
            WorkItemContext.Current.Items.Remove(m_key);
        }

        public override void SetValue(object newValue)
        {
            WorkItemContext.Current.Items[m_key] = newValue;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveValue();
            }
        }
    }
}
