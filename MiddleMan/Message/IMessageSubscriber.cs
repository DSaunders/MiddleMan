namespace MiddleMan.Message
{
    using System.Threading.Tasks;

    public interface IMessageSubscriber
    {
    }

    public interface IMessageSubscriber<in TMessageType> : IMessageSubscriber
        where TMessageType : class, IMessage
    {
        Task OnMessageReceived(TMessageType message);
    }
}