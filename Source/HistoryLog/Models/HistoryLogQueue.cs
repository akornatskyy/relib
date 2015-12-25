using System;
using System.Collections.Generic;
using ReusableLibrary.Supplemental.Collections;

namespace ReusableLibrary.HistoryLog.Models
{
    public sealed class HistoryLogQueue
    {
        private readonly object m_sync = new object();
        private readonly List<HistoryLogItem> m_items;

        public HistoryLogQueue()
        {
            m_items = new List<HistoryLogItem>();
        }

        public void Enqueue(HistoryLogItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            lock (m_sync)
            {
                m_items.Add(item);
            }
        }

        public bool IsEmpty()
        {
            lock (m_sync)
            {
                return m_items.Count == 0;
            }
        }

        public void Process(Action<HistoryLogItem> handler)
        {
            HistoryLogItem[] items;
            lock (m_sync)
            {
                if (m_items.Count == 0)
                {
                    return;
                }

                items = m_items.ToArray();
                m_items.Clear();
            }

            items.ForEach(handler);
        }
    }
}
