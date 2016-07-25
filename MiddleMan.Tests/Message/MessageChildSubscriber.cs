namespace MiddleMan.Tests.Message
{
    using System.Threading.Tasks;
    using MiddleMan.Message;

    public class MessageChildSubscriber : IMessageSubscriber<MessageChild>
    {
        public async Task OnMessageReceived(MessageChild message)
        {
            message.Subscribers.Add("MessageChild");
        }
    }
}