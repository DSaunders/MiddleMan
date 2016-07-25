namespace MiddleMan.Tests.Message
{
    using MiddleMan.Message;

    public class MessageParentSubscriber : IMessageSubscriber<MessageParent>
    {
        public void OnMessageReceived(MessageParent message)
        {
            message.Subscribers.Add("MessageParent");
        }
    }
}