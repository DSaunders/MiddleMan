namespace MiddleMan.Tests.Fakes.Message.Subscribers.Sync
{
    using Messages;
    using MiddleMan.Message;

    public class MessageChildSubscriber : IMessageSubscriber<MessageChild>
    {
        public void OnMessageReceived(MessageChild message)
        {
            message.Subscribers.Add("MessageChild");
        }
    }
}