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
        private readonly TypeFactory _factory;
        private Action<string> _log;

        public Broker(IEnumerable<IHandler> handlers,
            IEnumerable<IMessageSubscriber> messageSubscribers,
            IEnumerable<IPipelineTask> pipelineTasks,
            IEnumerable<IPipeline> pipelines)
        {
            _factory = new TypeFactory(handlers, messageSubscribers, pipelineTasks, pipelines);
        }

        public TOut ProcessQuery<TOut>(IQuery<TOut> query)
        {
            Log("Query", query);

            var handlers = _factory.GetQueryHandlers(query);

            if (!handlers.Any())
                throw new NoHandlerException($"No QueryHandler found for {query.GetType().Name}");

            if (handlers.Count > 1)
                throw new MultipleHandlersException($"{handlers.Count} QueryHandlers found for {query.GetType().Name}");

            // Can we do this by casting to the interface? Didn't work immediately, investigate.
            var handler = (dynamic)handlers.First();
            return handler.HandleQuery((dynamic)query);
        }

        public Task<TOut> ProcessQueryAsync<TOut>(IQuery<TOut> query)
        {
            Log("QueryAsync", query);

            var handlers = _factory.GetAsyncQueryHandlers(query);

            if (!handlers.Any())
                throw new NoHandlerException($"No Async QueryHandler found for {query.GetType().Name}");

            if (handlers.Count > 1)
                throw new MultipleHandlersException($"{handlers.Count} Async QueryHandlers found for {query.GetType().Name}");

            var handler = (dynamic)handlers.First();
            return handler.HandleQueryAsync((dynamic)query);
        }


        public void ProcessCommand(ICommand command)
        {
            Log("Command", command);

            var handlers = _factory.GetCommandHandlers(command);

            if (!handlers.Any())
                throw new NoHandlerException($"No CommandHandler found for {command.GetType().Name}");

            if (handlers.Count > 1)
                throw new MultipleHandlersException($"{handlers.Count} CommandHandlers found for {command.GetType().Name}");

            dynamic handler = handlers.First();
            handler.HandleCommand((dynamic)command);
        }

        public async Task ProcessCommandAsync(ICommand command)
        {
            Log("CommandAsync", command);

            var handlers = _factory.GetAsyncCommandHandlers(command);

            if (!handlers.Any())
                throw new NoHandlerException($"No Async CommandHandler found for {command.GetType().Name}");
            
            if (handlers.Count > 1)
                throw new MultipleHandlersException($"{handlers.Count} Async CommandHandlers found for {command.GetType().Name}");

            dynamic handler = handlers.First();
            await handler.HandleCommandAsync((dynamic)command);
        }


        public void SendMessage<T>(T message) where T : class, IMessage
        {
            Log("Message", message);

            var subscribers = _factory.GetMessageSubscribers(message);

            foreach (var subscriber in subscribers)
            {
                dynamic dynamicSubscriber = subscriber;
                dynamicSubscriber.OnMessageReceived(message);
            }
        }
        
        public async Task SendMessageAsync<T>(T message) where T : class, IMessage
        {
            Log("MessageAsync", message);

            var subscribers = _factory.GetAsyncMessageSubscribers(message);

            foreach (var subscriber in subscribers)
            {
                dynamic dynamicSubscriber = subscriber;
                await dynamicSubscriber.OnMessageReceived(message).ConfigureAwait(false);
            }
        }


        public void RunPipeline<TPipelineMessage>(TPipelineMessage message) where TPipelineMessage : class, IPipelineMessage
        {
            Log("PieplineMessage", message);

            var pipelines = _factory.GetPipelines(message);

            if (!pipelines.Any())
                return;

            if (pipelines.Count > 1)
                throw new MultiplePipelinesException($"{pipelines.Count} Pipelines found for {message.GetType().Name}");
            
            dynamic thisPipeline = pipelines.First();

            var builder = new PipelineBuilder<TPipelineMessage>();
            thisPipeline.GetPipelineTasks(builder);

            var taskTypes = builder.Get().ToList();
            if (!taskTypes.Any())
                return;
            
            var tasks = taskTypes.Select(t => (IPipelineTask<TPipelineMessage>)_factory.GetPipelineHandler(t)).ToList();

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
            Log("PipelineMessageAsync", message);

            var pipelines = _factory.GetAsyncPipelines(message);

            if (!pipelines.Any())
                return;

            if (pipelines.Count > 1)
                throw new MultiplePipelinesException($"{pipelines.Count} Async Pipelines found for {message.GetType().Name}");

            dynamic thisPipeline = pipelines.First();

            var builder = new PipelineBuilderAsync<TPipelineMessage>();
            thisPipeline.GetPipelineTasks(builder);

            var taskTypes = builder.Get().ToList();
            if (!taskTypes.Any())
                return;

            var tasks = taskTypes.Select(t => (IPipelineTaskAsync<TPipelineMessage>)_factory.GetPipelineHandler(t)).ToList();

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

        public void SetLogCalback(Action<string> callback)
        {
            _log = callback;
        }

        private void Log(string logType, object thing)
        {
            _log?.Invoke("Received " + logType + ": " + thing.GetType().Name);
        }
    }
}