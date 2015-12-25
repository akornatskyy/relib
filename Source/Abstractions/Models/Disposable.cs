using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable]
    public abstract class Disposable : IDisposable
    {
        public static void ReleaseFactory(object disposable)
        {
            ReleaseFactory(disposable as IDisposable);
        }

        public static void ReleaseFactory(IDisposable disposable)
        {
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public static Action<IDisposable> CreateReleaseFactory()
        {
            return new Action<IDisposable>(ReleaseFactory);
        }

        ~Disposable()
        {
            Dispose(false);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
