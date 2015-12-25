using System;

namespace ReusableLibrary.Abstractions.Models
{
    [Serializable]
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; private set; }
    }
}
