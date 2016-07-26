namespace MiddleMan.Tests.Fakes.Message.Subscribers.Async
{
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class TestMessageSubscriberAsyncBar : IMessageSubscriberAsync<TestMessage>
    {
        public async Task OnMessageReceived(TestMessage message)
        {
            message.Subscribers.Add("Bar");
        }
    }
}