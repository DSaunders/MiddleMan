namespace MiddleMan.Message
{
    public interface IMessageSubscriber
    {
    }

    public interface IMessageSubscriber<in TMessageType> : IMessageSubscriber
        where TMessageType : class, IMessage
    {
        void OnMessageReceived(TMessageType message);
    }
}