using System;

namespace ReusableLibrary.HistoryLog.Agents
{
    public interface IHistoryLogAgent
    {
        void Add(string username, short eventid);

        void Add(string username, short eventid, string related);

        void Add(string username, short eventid, string[] arguments);

        void Add(string username, short eventid, string related, string[] arguments);
    }
}
