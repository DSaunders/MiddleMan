namespace MiddleMan.Tests.Fakes.Message.Subscribers.Async
{
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class MessageChildSubscriberAsync : IMessageSubscriberAsync<MessageChild>
    {
        public async Task OnMessageReceived(MessageChild message)
        {
            message.Subscribers.Add("MessageChild");
        }
    }
}