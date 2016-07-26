namespace MiddleMan.Tests.Fakes.Message.Subscribers.Sync
{
    using Messages;
    using MiddleMan.Message;

    public class TestMessageSubscriberFoo : IMessageSubscriber<TestMessage>
    {
        public void OnMessageReceived(TestMessage message)
        {
            message.Subscribers.Add("Foo");
        }
    }
}