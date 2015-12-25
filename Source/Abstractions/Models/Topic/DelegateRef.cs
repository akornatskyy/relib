using System;

namespace ReusableLibrary.Abstractions.Models
{
    public struct DelegateRef
    {
        private readonly Delegate m_delegate;
        private readonly IDelegateInvokeStrategy m_strategy;

        public DelegateRef(Delegate @delegate, IDelegateInvokeStrategy strategy)
        {
            m_delegate = @delegate;
            m_strategy = strategy;
        }

        public Delegate Delegate 
        { 
            get { return m_delegate; } 
        }

        public IDelegateInvokeStrategy Strategy 
        { 
            get { return m_strategy; } 
        }
    }
}
