using System;
using System.Collections;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.WorkItem
{
    public sealed class WorkItemContext : Disposable
    {
        [ThreadStatic]
        private static WorkItemContext g_context;

        private IDictionary m_items;

        private WorkItemContext()
        {
            m_items = new Hashtable();
        }

        public static WorkItemContext Current
        {
            get
            {
                if (g_context == null)
                {
                    g_context = new WorkItemContext();
                }

                return g_context;
            }
        }

        public IDictionary Items
        {
            get { return m_items; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (object o in m_items.Values)
                {
                    IDisposable disposable = o as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }

                m_items.Clear();
                m_items = null;
                g_context = null;
            }
        }
    }
}
