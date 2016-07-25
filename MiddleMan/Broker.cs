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
    using Pipeline.Builder;
    using Pipeline.Tasks;
    using Query;

    public class Broker : IBroker
    {
        private readonly IEnumerable<IHandler> _handlers;
        private readonly IEnumerable<IMessageSubscriber> _messageSubscribers;
        private readonly IEnumerable<IPipelineTask> _pipelineTasks;
        private readonly IEnumerable<IPipeline> _pipelines;

        public Broker(IEnumerable<IHandler> handlers,
            IEnumerable<IMessageSubscriber> messageSubscribers,
            IEnumerable<IPipelineTask> pipelineTasks,
            IEnumerable<IPipeline> pipelines)
        {
            _handlers = handlers;
            _messageSubscribers = messageSubscribers;
            _pipelineTasks = pipelineTasks;
            _pipelines = pipelines;
        }

        public TOut ProcessQuery<TOut>(IQuery<TOut> query)
        {
            var handlers = GetQueryHandlers(query);

            if (!handlers.Any())
                throw new NoHandlerException($"No QueryHandler found for {query.GetType().Name}");

            if (handlers.Length > 1)
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
            await handler.HandleCommandAsync((dynamic)command);
        }


        public async Task SendMessageAsync<T>(T message) where T : class, IMessage
        {
            var subscribers = GetAsyncMessageSubscribers(message);

            foreach (var subscriber in subscribers)
            {
                dynamic dynamicSubscriber = subscriber;
                await dynamicSubscriber.OnMessageReceived(message).ConfigureAwait(false);
            }
        }


        public void RunPipeline<TPipelineMessage>(TPipelineMessage message) where TPipelineMessage : class, IPipelineMessage
        {
            var pipelines = GetPipelines(message);

            if (!pipelines.Any())
                return;

            if (pipelines.Count > 1)
                throw new MultiplePipelinesException($"{pipelines.Count} Pipelines found for {message.GetType().Name}");
            
            dynamic thisPipeline = pipelines.First();

            var builder = new PipelineBuilder<TPipelineMessage>();
            thisPipeline.GetPipelineTasks(builder);

            var taskTypes = builder.Get();
            var tasks = taskTypes.Select(t => (IPipelineTask<TPipelineMessage>)GetPipelineHandler(t)).ToList();

            for (var i = 0; i < tasks.Count - 1; i++)
            {
                var thisTask = tasks[i];
                var nextTask = tasks[i + 1];

                thisTask.Setup(nextTask.Run);
            }

            // Noop last task
            tasks.Last().Setup(m => { });

            // Run pipeline
            tasks.First().Run(message);
        }

        public async Task RunPipelineAsync<TPipelineMessage>(TPipelineMessage message) where TPipelineMessage : class, IPipelineMessage
        {
            var pipelines = GetAsyncPipelines(message);

            if (!pipelines.Any())
                return;

            if (pipelines.Count > 1)
                throw new MultiplePipelinesException($"{pipelines.Count} Async Pipelines found for {message.GetType().Name}");

            dynamic thisPipeline = pipelines.First();

            var builder = new PipelineBuilderAsync<TPipelineMessage>();
            thisPipeline.GetPipelineTasks(builder);

            var taskTypes = builder.Get();
            var tasks = taskTypes.Select(t => (IPipelineTaskAsync<TPipelineMessage>)GetPipelineHandler(t)).ToList();

            for (var i = 0; i < tasks.Count - 1; i++)
            {
                var thisTask = tasks[i];
                var nextTask = tasks[i + 1];

                thisTask.Setup(nextTask.Run);
            }

            // Noop last task
            tasks.Last().Setup(m => Task.FromResult(0));

            // Run pipeline
            await tasks.First().Run(message).ConfigureAwait(false);
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
                    i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) &&
                    i.GetGenericArguments()[0] == query.GetType() &&
                    i.GetGenericArguments()[1] == typeof(TOut)))
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

        private List<IMessageSubscriber> GetAsyncMessageSubscribers(IMessage message)
        {
            return _messageSubscribers
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IMessageSubscriber<>) &&
                    i.GetGenericArguments()[0].IsInstanceOfType(message)))
                .ToList();
        }

        private List<IPipeline> GetPipelines(IPipelineMessage message)
        {
            return _pipelines
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IPipeline<>) &&
                    i.GetGenericArguments()[0] == message.GetType()))
                    .ToList();
        }

        private List<IPipeline> GetAsyncPipelines(IPipelineMessage message)
        {
            return _pipelines
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IPipelineAsync<>) &&
                    i.GetGenericArguments()[0] == message.GetType()))
                    .ToList();
        }

        private IPipelineTask GetPipelineHandler(Type pipelineTaskType)
        {
            return _pipelineTasks.FirstOrDefault(p => p.GetType() == pipelineTaskType);
        }
    }
}