using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Unity;

namespace ReusableLibrary.Unity
{
    public sealed class UnityPerWebRequestLifetimeManager : LifetimeManager
    {
        #region LifetimeManager overrides

        [DebuggerStepThrough]
        public override object GetValue()
        {
            var backingStore = BackingStore;
            return backingStore.ContainsKey(this) ? backingStore[this] : null;
        }

        [DebuggerStepThrough]
        public override void SetValue(object value)
        {
            var backingStore = BackingStore;
            if (backingStore.ContainsKey(this))
            {
                object oldValue = backingStore[this];
                if (!ReferenceEquals(value, oldValue))
                {
                    IDisposable disposable = oldValue as IDisposable;
                    if (disposable != null)
                    {
                        try
                        {
                            disposable.Dispose();
                        }
                        catch
                        {
                            // TODO:
                            // Must not fail
                        }
                    }

                    if (value == null)
                    {
                        backingStore.Remove(this);
                    }
                    else
                    {
                        backingStore[this] = value;
                    }
                }
            }
            else
            {
                if (value != null)
                {
                    backingStore.Add(this, value);
                }
            }
        }

        [DebuggerStepThrough]
        public override void RemoveValue()
        {
            SetValue(null);
        }

        #endregion

        private static IDictionary<UnityPerWebRequestLifetimeManager, object> BackingStore
        {
            get { return UnityPerWebRequestLifetimeModule.BackingStore; }
        }
    }
}
