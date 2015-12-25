using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class DelegateInvokeContext
    {
        private readonly Delegate m_delegate;
        private readonly object[] m_args;
        private readonly Delegate m_resultDelegate;

        public DelegateInvokeContext(Delegate @delegate, object[] args, Delegate resultDelegate)
        {
            m_delegate = @delegate;
            m_args = args;
            m_resultDelegate = resultDelegate;
        }

        public Delegate Delegate 
        { 
            get { return m_delegate; } 
        }

        public object[] Args 
        { 
            get { return m_args; } 
        }

        public Delegate ResultDelegate 
        { 
            get { return m_resultDelegate; } 
        }
    }
}
