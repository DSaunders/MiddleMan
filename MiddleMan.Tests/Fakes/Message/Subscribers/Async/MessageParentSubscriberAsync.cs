namespace MiddleMan.Tests.Fakes.Message.Subscribers.Async
{
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class MessageParentSubscriberAsync : IMessageSubscriberAsync<MessageParent>
    {
        public async Task OnMessageReceived(MessageParent message)
        {
            message.Subscribers.Add("MessageParent");
        }
    }
}