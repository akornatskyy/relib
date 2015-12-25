namespace ReusableLibrary.Abstractions.Models
{
    public sealed class Topic : AbstractTopic
    {
        public Topic(string name)
            : base(name)
        {
        }

        public void Subscribe(Action2 subscriber)
        {
            InnerSubscribe(subscriber);
        }

        public void Subscribe(Action2 subscriber, IDelegateInvokeStrategy strategy)
        {
            InnerSubscribe(subscriber, strategy);
        }

        public void Unsubscribe(Action2 subscriber)
        {
            InnerUnsubscribe(subscriber);
        }

        public void Publish()
        {
            InnerPublish();
        }
    }
}
