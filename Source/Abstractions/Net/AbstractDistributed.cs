using System;
using System.Collections.Generic;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Net
{
    public abstract class AbstractDistributed : Disposable, IClient
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("AbstractDistributed"));
        
        protected AbstractDistributed()
        {
            IdleState = new IdleState();
        }

        public IPool<IClient> Pool { get; set; }

        public IPool<IClient> Idle { get; set; }

        public DistributedOptions Options { get; set; }

        public void AddRange(IEnumerable<IClient> clients)
        {
            EnumerableHelper.ForEach(clients, client =>
            {
                if (!Pool.Return(client))
                {
                    throw new InvalidOperationException("Unable add a client to the pool");
                }
            });
        }

        #region IClient Members

        public Func2<Action2<IClientConnection, object>, bool> Context(object state)
        {
            var hashes = state as byte[][];
            if (hashes != null)
            {
                return SplitContext(hashes);
            }

            return (Action2<IClientConnection, object> action) =>
            {
                var client = Pool.Take(state);
                if (client == null)
                {
                    return false;
                }

                return Context(client, state, action);
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

        public void AddToIdle(IClient client)
        {
            if (client == null)
            {
                return;
            }

            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "Idle - {0}", client);
            }

            client.IdleState.Reset();
            Idle.Return(client);
        }

        public void RetryIdled(IClient client)
        {
            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "Trying - {0}", client);
            }

            Pool.Return(client);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private Func2<Action2<IClientConnection, object>, bool> SplitContext(byte[][] hashes)
        {
            return (Action2<IClientConnection, object> action) =>
            {
                var clientMap = new Dictionary<IClient, List<byte[]>>(hashes.Length);
                foreach (var hash in hashes)
                {
                    var client = Pool.Take(hash);
                    if (client == null)
                    {
                        // TODO:
                        continue;
                    }

                    List<byte[]> clientHashes;
                    if (!clientMap.TryGetValue(client, out clientHashes))
                    {
                        clientHashes = new List<byte[]>(hashes.Length);
                        clientMap.Add(client, clientHashes);
                    }

                    clientHashes.Add(hash);
                }

                foreach (var pair in clientMap)
                {
                    Context(pair.Key, pair.Value.ToArray(), action);
                }

                return true;
            };
        }

        private bool Context(IClient client, object state, Action2<IClientConnection, object> action)
        {
            var succeed = client.Context(state)(action);
            if (!succeed)
            {
                if (g_traceInfo.IsVerboseEnabled)
                {
                    TraceHelper.TraceVerbose(g_traceInfo, "Removing - {0}", client);
                }

                AddToIdle(Pool.Take(client));
            }

            return succeed;
        }
    }
}
