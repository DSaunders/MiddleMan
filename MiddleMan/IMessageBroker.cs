namespace MiddleMan
{
    using System;
    using System.Threading.Tasks;
    using Command;
    using Message;
    using Pipeline;
    using Query;

    public interface IMessageBroker
    {
        TOut ProcessQuery<TOut>(IQuery<TOut> query);
        Task<TOut> ProcessQueryAsync<TOut>(IQuery<TOut> query);

        void ProcessCommand(ICommand command);
        Task ProcessCommandAsync(ICommand command);

        void SendMessage<T>(T message) where T : class, IMessage;
        void SubscribeToMessage<T>(Action<T> messageCallback) where T : class, IMessage;
        void SubscribeToAllMessages(Action<IMessage> messageCallback);
        
        void ConstructPipeline<TPipelineMessage>(Action<PipelineBuilder<TPipelineMessage>> action) where TPipelineMessage : class, IPipelineMessage;
        Task RunPipelineAsync<TPipelineMessage>(TPipelineMessage message) where TPipelineMessage : class, IPipelineMessage;
    }
}
