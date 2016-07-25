namespace MiddleMan.Tests.Message
{
    using MiddleMan.Message;

    public class TestMessageSubscriberFoo : IMessageSubscriber<TestMessage>
    {
        public void OnMessageReceived(TestMessage message)
        {
            message.Subscribers.Add("Foo");
        }
    }
}