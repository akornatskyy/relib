using System;
using System.Diagnostics;
using System.Globalization;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Net
{
    public abstract class AbstractClient<T> : Disposable, IClient
        where T : class, IClientConnection
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("AbstractClient"));
        
        protected AbstractClient()
        {
            IdleState = new IdleState();
        }

        public ConnectionOptions Options { get; set; }

        public IPool<T> Pool { get; set; }

        #region IClient Members

        public Func2<Action2<IClientConnection, object>, bool> Context(object state)
        {
            return (Action2<IClientConnection, object> action) => 
            {
                var succeed = false;
                using (var pooled = new Pooled<T>(Pool, Options))
                {
                    var connection = pooled.Item;
                    if (connection == null)
                    {
                        if (g_traceInfo.IsVerboseEnabled)
                        {
                            TraceHelper.TraceVerbose(g_traceInfo, "{0} - There are no free connections available", Options.FullName);
                        }

                        return false;
                    }
                    
                    try
                    {
                        if (connection.TryConnect())
                        {
                            action(connection, state);
                            succeed = true;
                        }
                        else
                        {
                            if (g_traceInfo.IsWarningEnabled)
                            {
                                TraceHelper.TraceWarning(g_traceInfo, "{0} - Can not connect", Options.FullName);
                            }
                        }
                    }
                    catch (SystemException sex)
                    {
                        if (g_traceInfo.IsWarningEnabled)
                        {
                            TraceHelper.TraceWarning(g_traceInfo, sex.Message);
                        }

                        connection.Close();
                    }                    
                }

                return succeed;
            };
        }

        #endregion

        #region IIdleStateProvider Members

        public IdleState IdleState { get; set; }

        #endregion

        #region IEquatable<IClient> Members

        public bool Equals(IClient other)
        {
            return object.ReferenceEquals(this, other);
        }

        #endregion

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Client [{0}]", Options.FullName);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
