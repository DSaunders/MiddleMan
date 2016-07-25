namespace MiddleMan.Tests
{
    using System.Collections.Generic;
    using Command.Handlers;
    using MiddleMan;
    using Message.Subscriber;
    using MiddleMan.Message;
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Tasks;
    using Pipeline.Async;
    using Pipeline.MultiplePipelines;
    using Pipeline.Sync;
    using Query.Handlers;

    public partial class MessageBrokerTests
    {
        private readonly IBroker _broker;

        public MessageBrokerTests()
        {
            var handlers = new List<IHandler>
            {
                new MultipleHandler1(),
                new MultipleHandler1(),
                new TestQueryHandler(),
                new TestCommandHandler(),
                new MultipleCommandHandler1(),
                new MultipleCommandHandler2(),
                new MultipleHandlerAsync1(),
                new MultipleHandlerAsync2(),
                new TestQueryHandlerAsync(),
                new AsyncCommandHandler(),
                new MultipleCommandHandlerAsync1(),
                new MultipleCommandHandlerAsync2()
            };

            var subscribers = new List<IMessageSubscriber>
            {
                new MessageChildSubscriber(),
                new MessageParentSubscriber(),
                new SomeOtherMessageSubscriberThatThrows(),
                new TestMessageSubscriberFoo(),
                new TestMessageSubscriberBar()
            };

            var pipelineTasks = new List<IPipelineTask>
            {
                new FooBarPipelineTaskFoo(),
                new FooBarPipelineTaskBar(),
                new FooBarPipelineTaskFooAsync(),
                new FooBarPipelineTaskBarAsync(),
            };

            var pipelines = new List<IPipeline>
            {
                new FooBarPipeline(),
                new FooBarPipelineAsync(),
                new MultiplePipelineMessagePipeline1(),
                new MultiplePipelineMessagePipeline2(),
                new MultiplePipelineMessagePipelineAsync1(),
                new MultiplePipelineMessagePipelineAsync2(),
            };

            _broker = new Broker(handlers, subscribers, pipelineTasks, pipelines);
        }
    }
}
