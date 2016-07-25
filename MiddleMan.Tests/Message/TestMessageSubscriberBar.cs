namespace MiddleMan.Tests.Message
{
    using MiddleMan.Message;

    public class TestMessageSubscriberBar : IMessageSubscriber<TestMessage>
    {
        public void OnMessageReceived(TestMessage message)
        {
            message.Subscribers.Add("Bar");
        }
    }
}