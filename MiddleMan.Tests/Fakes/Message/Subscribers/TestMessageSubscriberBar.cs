namespace MiddleMan.Tests.Fakes.Message.Subscribers
{
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class TestMessageSubscriberBar : IMessageSubscriber<TestMessage>
    {
        public async Task OnMessageReceived(TestMessage message)
        {
            message.Subscribers.Add("Bar");
        }
    }
}