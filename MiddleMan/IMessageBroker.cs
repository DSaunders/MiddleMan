namespace MiddleMan
{
    using System;
    using Interfaces.Command;
    using Interfaces.Message;
    using Interfaces.Query;

    public interface IMessageBroker
    {
        TOut ProcessQuery<TOut>(IQuery<TOut> query);
        void ProcessCommand(ICommand command);
        void SendMessage<T>(T message) where T : class, IMessage;
        void SubscribeToMessage<T>(Action<T> messageCallback) where T : class, IMessage;
        void SubscribeToAllMessages(Action<IMessage> messageCallback);
    }
}
