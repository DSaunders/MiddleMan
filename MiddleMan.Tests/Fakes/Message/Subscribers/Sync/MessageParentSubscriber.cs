namespace MiddleMan.Tests.Fakes.Message.Subscribers.Sync
{
    using Messages;
    using MiddleMan.Message;

    public class MessageParentSubscriber : IMessageSubscriber<MessageParent>
    {
        public void OnMessageReceived(MessageParent message)
        {
            message.Subscribers.Add("MessageParent");
        }
    }
}