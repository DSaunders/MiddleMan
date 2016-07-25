namespace MiddleMan.Tests.Message
{
    using System.Threading.Tasks;
    using MiddleMan.Message;

    public class TestMessageSubscriberBar : IMessageSubscriber<TestMessage>
    {
        public async Task OnMessageReceived(TestMessage message)
        {
            message.Subscribers.Add("Bar");
        }
    }
}