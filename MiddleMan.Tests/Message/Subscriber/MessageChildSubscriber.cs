namespace MiddleMan.Tests.Message.Subscriber
{
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class MessageChildSubscriber : IMessageSubscriber<MessageChild>
    {
        public async Task OnMessageReceived(MessageChild message)
        {
            message.Subscribers.Add("MessageChild");
        }
    }
}