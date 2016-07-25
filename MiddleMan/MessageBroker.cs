namespace MiddleMan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Command;
    using Exceptions;
    using Message;
    using Pipeline;
    using Query;

    public class MessageBroker : IMessageBroker
    {
        private readonly IEnumerable<IHandler> _handlers;
        private readonly IEnumerable<IPipelineTask> _pipelineTasks;
        private readonly IList<ISubscription> _messageSubscibers;

        private readonly IDictionary<Type, PipelineBuilder> _pipelines;

        public MessageBroker(IEnumerable<IHandler> handlers, IEnumerable<IPipelineTask> pipelineTasks)
        {
            _handlers = handlers;
            _pipelineTasks = pipelineTasks;
            _messageSubscibers = new List<ISubscription>();
            _pipelines = new Dictionary<Type, PipelineBuilder>();
        }


        public TOut ProcessQuery<TOut>(IQuery<TOut> query)
        {
            var handlers = GetQueryHandlers(query);
           
            if (!handlers.Any())
                throw new NoHandlerException($"No QueryHandler found for {query.GetType().Name}");
            
            if (handlers.Length >1 )
                throw new MultipleHandlersException($"{handlers.Length} QueryHandlers found for {query.GetType().Name}");

            // Can we do this by casting to the interface? Didn't work immediately, investigate.
            var handler = (dynamic)handlers.First();
            return handler.HandleQuery((dynamic)query);
        }

        public Task<TOut> ProcessQueryAsync<TOut>(IQuery<TOut> query)
        {
            var handlers = GetAsyncQueryHandlers(query);

            if (!handlers.Any())
                throw new NoHandlerException($"No Async QueryHandler found for {query.GetType().Name}");

            if (handlers.Length > 1)
                throw new MultipleHandlersException($"{handlers.Length} Async QueryHandlers found for {query.GetType().Name}");

            var handler = (dynamic)handlers.First();
            return handler.HandleQueryAsync((dynamic)query);
        }


        public void ProcessCommand(ICommand command)
        {
            var handlers = GetCommandHandlers(command);

            if (!handlers.Any())
                throw new NoHandlerException($"No CommandHandler found for {command.GetType().Name}");

            if (handlers.Length > 1)
                throw new MultipleHandlersException($"{handlers.Length} CommandHandlers found for {command.GetType().Name}");

            dynamic handler = handlers.First();
            handler.HandleCommand((dynamic)command);
        }

        public async Task ProcessCommandAsync(ICommand command)
        {
            var handlers = GetAsyncCommandHandlersAsync(command);

            if (!handlers.Any())
                throw new NoHandlerException($"No Async CommandHandler found for {command.GetType().Name}");


            if (handlers.Length > 1)
                throw new MultipleHandlersException($"{handlers.Length} Async CommandHandlers found for {command.GetType().Name}");

            dynamic handler = handlers.First();
            await handler.HandleCommandAsync((dynamic) command);
        }


        public void SendMessage<T>(T message) where T : class, IMessage
        {
            foreach (var subsciber in _messageSubscibers)
            {
                if (subsciber.Type.IsInstanceOfType(message))
                    ((dynamic)subsciber).Action((dynamic)message);
            }
        }

        public void SubscribeToMessage<T>(Action<T> messageCallback) where T: class, IMessage
        {
            _messageSubscibers.Add(new Subscription<T>(messageCallback));
        }

        public void SubscribeToAllMessages(Action<IMessage> messageCallback)
        {
            _messageSubscibers.Add(new Subscription<IMessage>(messageCallback));
        }


        public void ConstructPipeline<TPipelineMessage>(Action<PipelineBuilder<TPipelineMessage>> action) where TPipelineMessage : class, IPipelineMessage
        {
            var messageType = typeof (TPipelineMessage);

            if (_pipelines.ContainsKey(messageType))
                throw new MultiplePipelinesException("A pipeline already exists to handle this PipelineMessage type");

            var pipeline = new PipelineBuilder<TPipelineMessage>();
            action(pipeline);
            _pipelines.Add(typeof(TPipelineMessage), pipeline);
        }

        public async Task RunPipelineAsync<TPipelineMessage>(TPipelineMessage message) where TPipelineMessage : class, IPipelineMessage
        {
            var pipeline = _pipelines[message.GetType()];

            var types = pipeline.GetPipelineTaskTypesInOrder();

            foreach (var pipelineTaskType in types)
            {
                var instance = (IPipelineTask<TPipelineMessage>)GetAsyncPipelineHandler(pipelineTaskType);
                await instance.Run(message);
            }
        }


        private IHandler[] GetCommandHandlers(ICommand command)
        {
            return _handlers
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) &&
                    i.GetGenericArguments()[0] == command.GetType()))
                .ToArray();
        }

        private IHandler[] GetAsyncCommandHandlersAsync(ICommand command)
        {
            return _handlers
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommandHandlerAsync<>) &&
                    i.GetGenericArguments()[0] == command.GetType()))
                .ToArray();
        }

        private IHandler[] GetQueryHandlers<TOut>(IQuery<TOut> query)
        {
            return _handlers
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof (IQueryHandler<,>) &&
                    i.GetGenericArguments()[0] == query.GetType() &&
                    i.GetGenericArguments()[1] == typeof (TOut)))
                .ToArray();
        }

        private IHandler[] GetAsyncQueryHandlers<TOut>(IQuery<TOut> query)
        {
            return _handlers
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IQueryHandlerAsync<,>) &&
                    i.GetGenericArguments()[0] == query.GetType() &&
                    i.GetGenericArguments()[1] == typeof(TOut)))
                .ToArray();
        }

        private IPipelineTask GetAsyncPipelineHandler(Type pipelineTaskType)
        {
            return _pipelineTasks
                .FirstOrDefault(p => p.GetType() == pipelineTaskType);
        }
    }
}