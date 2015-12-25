using System;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.WorkItem;

namespace ReusableLibrary.Abstractions.Tests.WorkItem
{
    internal sealed class MockAbstractWorkItem : AbstractWorkItem<object>
    {
        public bool AcquiredUnitOfWork { get; set; }

        public bool AcquiredUnitOfWorkThrows { get; set; }

        public bool AllRulesSatisfied { get; set; }

        public bool AllRulesSatisfiedThrows { get; set; }

        public Func2<bool>[] Rules2 
        {
            set { Rules = value; } 
        }

        public override bool OnAcquireUnitOfWork()
        {
            if (AcquiredUnitOfWorkThrows)
            {
                throw new InvalidOperationException();
            }

            return AcquiredUnitOfWork;
        }

        public override void OnAllRulesSatisfied()
        {
            if (AllRulesSatisfiedThrows)
            {
                throw new InvalidOperationException();
            }

            AllRulesSatisfied = true;
        }
    }
}
