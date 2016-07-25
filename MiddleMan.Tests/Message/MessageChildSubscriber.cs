namespace MiddleMan.Tests.Message
{
    using MiddleMan.Message;

    public class MessageChildSubscriber : IMessageSubscriber<MessageChild>
    {
        public void OnMessageReceived(MessageChild message)
        {
            message.Subscribers.Add("MessageChild");
        }
    }
}