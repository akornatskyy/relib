using System;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Threading;

namespace ReusableLibrary.Abstractions.WorkItem
{
    public sealed class WorkItemBackgroundTask : AbstractBackgroundTask
    {
        private readonly string m_workItemName;

        public WorkItemBackgroundTask(string workItemName)
            : base(workItemName)
        {
            if (String.IsNullOrEmpty(workItemName))
            {
                throw new ArgumentNullException("workItemName");
            }

            m_workItemName = workItemName;
        }

        public override bool DoWork(Func2<bool> shutdownCallback)
        {
            using (WorkItemContext context = WorkItemContext.Current)
            {
                IWorkItem workItem;

                try
                {
                    workItem = DependencyResolver.Resolve<IWorkItem>(m_workItemName);                    
                }
                catch (NullReferenceException) 
                {
                    /* Container has been reloaded */
                    return false;
                }

                return workItem.DoWork(shutdownCallback) && !shutdownCallback();
            }
        }
    }
}
