using System.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Host
{
    public class SingletonApplication : BootstrapperApplication
    {
        private Mutex m_mutex;

        public SingletonApplication(string name)
            : base(name)
        {
        }

        #region BootstrapperApplication overrides

        public override bool CanRun()
        {
            m_mutex = ObtainMutex(Name);
            return m_mutex != null;
        }

        #endregion     

        #region Disposable overrides

        protected override void Dispose(bool disposing)
        {
            if (m_mutex != null)
            {
                try
                {
                    m_mutex.ReleaseMutex();
                }
                finally
                {
                    m_mutex = null;
                }
            }
        }

        #endregion

        private Mutex ObtainMutex(string name)
        {
            bool createdNew;
            var mutex = new Mutex(true, @"Global\" + name, out createdNew);
            if (!createdNew)
            {
                if (TraceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(TraceInfo, "Another instance of '{0}' is already running", name);
                }

                return null;
            }

            return mutex;
        }
    }
}
