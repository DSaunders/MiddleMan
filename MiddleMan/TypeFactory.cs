namespace MiddleMan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Command;
    using Message;
    using Pipeline;
    using Pipeline.Tasks;
    using Query;

    public class TypeFactory
    {
        private readonly IEnumerable<IHandler> _handlers;
        private readonly IEnumerable<IMessageSubscriber> _messageSubscribers;
        private readonly IEnumerable<IPipelineTask> _pipelineTasks;
        private readonly IEnumerable<IPipeline> _pipelines;

        public TypeFactory(
            IEnumerable<IHandler> handlers,
            IEnumerable<IMessageSubscriber> messageSubscribers,
            IEnumerable<IPipelineTask> pipelineTasks,
            IEnumerable<IPipeline> pipelines)
        {
            _handlers = handlers;
            _messageSubscribers = messageSubscribers;
            _pipelineTasks = pipelineTasks;
            _pipelines = pipelines;
        }

        public IList<IHandler> GetCommandHandlers(ICommand command)
        {
            return GetProcessorForTypeAndDerived(command, _handlers, typeof(ICommandHandler<>));
        }

        public IList<IHandler> GetAsyncCommandHandlers(ICommand command)
        {
            return GetProcessorForTypeAndDerived(command, _handlers, typeof(ICommandHandlerAsync<>));
        }

        public IList<IHandler> GetQueryHandlers<TOut>(IQuery<TOut> query)
        {
            return GetProcessorWithOutputForType<IQuery<TOut>, IHandler, TOut>(query, _handlers, typeof (IQueryHandler<,>));
        }

        public IList<IHandler> GetAsyncQueryHandlers<TOut>(IQuery<TOut> query)
        {
            return GetProcessorWithOutputForType<IQuery<TOut>, IHandler, TOut>(query, _handlers, typeof(IQueryHandlerAsync<,>));
        }

        public List<IMessageSubscriber> GetMessageSubscribers(IMessage message)
        {
            return GetProcessorForTypeAndDerived(message, _messageSubscribers, typeof(IMessageSubscriber<>));
        }

        public List<IMessageSubscriber> GetAsyncMessageSubscribers(IMessage message)
        {
            return GetProcessorForTypeAndDerived(message, _messageSubscribers, typeof(IMessageSubscriberAsync<>));
        }

        public List<IPipeline> GetPipelines(IPipelineMessage message)
        {
            return GetProcessorForType(message, _pipelines, typeof(IPipeline<>));
        }

        public List<IPipeline> GetAsyncPipelines(IPipelineMessage message)
        {
            return GetProcessorForType(message, _pipelines, typeof(IPipelineAsync<>));
        }

        public IPipelineTask GetPipelineHandler(Type pipelineTaskType)
        {
            return _pipelineTasks.FirstOrDefault(p => p.GetType() == pipelineTaskType);
        }


        private static List<TProcessorType> GetProcessorForType<T, TProcessorType>(T input, IEnumerable<TProcessorType> allProcessors, Type genericTypeInterface)
        {
            return allProcessors
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == genericTypeInterface &&
                    i.GetGenericArguments()[0] == input.GetType()))
                .ToList();
        }

        private static List<TProcessorType> GetProcessorWithOutputForType<T, TProcessorType, TOut>(T input, IEnumerable<TProcessorType> allProcessors, Type genericTypeInterface)
        {
            return allProcessors
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == genericTypeInterface &&
                    i.GetGenericArguments()[0] == input.GetType() &&
                    i.GetGenericArguments()[1] == typeof(TOut)))
                .ToList();
        }

        private static List<TProcessorType> GetProcessorForTypeAndDerived<T, TProcessorType>(T input, IEnumerable<TProcessorType> allProcessors, Type genericTypeInterface)
        {
            return allProcessors
                .Where(p => p.GetType().GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == genericTypeInterface &&
                    i.GetGenericArguments()[0].IsInstanceOfType(input)))
                .ToList();
        }
    }
}