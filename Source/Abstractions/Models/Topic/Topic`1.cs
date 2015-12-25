using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class Topic<T> : AbstractTopic
    {
        public Topic(string name)
            : base(name)
        {
        }

        public void Subscribe(Action<T> subscriber)
        {
            InnerSubscribe(subscriber, DelegateInvokeStrategy.Publisher);
        }

        public void Subscribe(Action<T> subscriber, IDelegateInvokeStrategy strategy)
        {
            InnerSubscribe(subscriber, strategy);
        }

        public void Unsubscribe(Action<T> subscriber)
        {
            InnerUnsubscribe(subscriber);
        }

        public void Publish(T payload)
        {
            InnerPublish(payload);
        }
    }
}
