using System;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Services
{
    public sealed class RunOnceService : IRunOnceService
    {
        public const string ResultKeyPrefix = "rosr";
        public const string ErrorKeyPrefix = "rose";

        private readonly ICache m_cache;
        private readonly TimeSpan m_period;
        private readonly IValidationState m_validationState;

        private string m_key;

        public RunOnceService(ICache cache, TimeSpan period, IValidationState validationState)
        {
            m_cache = cache;
            m_period = period;
            m_validationState = validationState;
        }

        #region IRunOnceService Members

        public bool Begin(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            m_key = key;
            return new RunOnce(m_cache, m_period).TryEnter(key);
        }

        public void Error(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (m_key == null)
            {
                throw new InvalidOperationException(Properties.Resources.RunOnceServiceKeyNotInitialized);
            }

            m_cache.Store<string>(string.Concat(ErrorKeyPrefix, m_key), message, m_period);
        }

        public void Result<T>(T value)
        {
            if (m_key == null)
            {
                throw new InvalidOperationException(Properties.Resources.RunOnceServiceKeyNotInitialized);
            }

            m_cache.Store<T>(string.Concat(ResultKeyPrefix, m_key), value, m_period);
        }

        public RunOnceResult<T> End<T>()
        {
            if (m_key == null)
            {
                throw new InvalidOperationException(Properties.Resources.RunOnceServiceKeyNotInitialized);
            }

            var result = new RunOnceResult<T>();
            var error = m_cache.Get<string>(string.Concat(ErrorKeyPrefix, m_key));
            if (error != null)
            {
                m_validationState.AddError(error);
                result.Error = error;
                result.IsCompleted = true;
            }
            else
            {
                var datakey = new LazyDataKey<T>(string.Concat(ResultKeyPrefix, m_key));
                if (m_cache.Get(datakey))
                {
                    if (datakey.HasValue)
                    {
                        result.Result = datakey.Value;
                        result.IsCompleted = true;
                    }
                }
            }

            return result;
        }

        public RunOnceResult<T> BeginOrEnd<T>(string key)
        {
            return Begin(key) ? null : End<T>();
        }

        #endregion
    }
}
