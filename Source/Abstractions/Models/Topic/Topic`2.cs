namespace ReusableLibrary.Abstractions.Models
{
    public sealed class Topic<T1, T2> : AbstractTopic
    {
        public Topic(string name)
            : base(name)
        {
        }

        public void Subscribe(Action2<T1, T2> subscriber)
        {
            InnerSubscribe(subscriber, DelegateInvokeStrategy.Publisher);
        }

        public void Subscribe(Action2<T1, T2> subscriber, IDelegateInvokeStrategy strategy)
        {
            InnerSubscribe(subscriber, strategy);
        }

        public void Unsubscribe(Action2<T1, T2> subscriber)
        {
            InnerUnsubscribe(subscriber);
        }

        public void Publish(T1 payload1, T2 payload2)
        {
            InnerPublish(payload1, payload2);
        }
    }
}
