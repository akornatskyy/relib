using System;
using System.Linq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.Collections;

namespace ReusableLibrary.HistoryLog.Agents
{
    public sealed class HistoryLogAgent : IHistoryLogAgent
    {
        private readonly IRemoteLocationProvider m_remoteLocationProvider;
        private readonly HistoryLogQueue m_historyLogQueue;

        public HistoryLogAgent(IRemoteLocationProvider remoteLocationProvider,
            HistoryLogQueue historyLogQueue)
        {
            m_remoteLocationProvider = remoteLocationProvider;
            m_historyLogQueue = historyLogQueue;
        }

        #region IHistoryLogAgent Members

        public void Add(string username, short eventid)
        {
            Add(username, eventid, null, null);
        }

        public void Add(string username, short eventid, string related)
        {
            Add(username, eventid, related, null);
        }

        public void Add(string username, short eventid, string[] arguments)
        {
            Add(username, eventid, null, arguments);
        }

        public void Add(string username, short eventid, string related, string[] arguments)
        {
            var item = new HistoryLogItem()
            {
                Username = username,
                Timestamp = DateTime.UtcNow,
                EventId = eventid,
                Hosts = m_remoteLocationProvider.RemoteLocation.Hosts.Translate(host =>
                    IpNumberHelper.ToIpNumber(host)).ToArray(),
                RelatedTo = related,
                Arguments = HistoryLogItem.Join(arguments)
            };

            m_historyLogQueue.Enqueue(item);
        }

        #endregion
    }
}
