namespace MiddleMan.Tests.Message
{
    using System.Threading.Tasks;
    using MiddleMan.Message;

    public class MessageParentSubscriber : IMessageSubscriber<MessageParent>
    {
        public async Task OnMessageReceived(MessageParent message)
        {
            message.Subscribers.Add("MessageParent");
        }
    }
}