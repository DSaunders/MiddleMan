namespace MiddleMan.Tests.Fakes
{
    using System.Collections.Generic;
    using Command.Handlers;
    using Message.Subscribers;
    using Message.Subscribers.Async;
    using Message.Subscribers.Sync;
    using MiddleMan;
    using MiddleMan.Message;
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Tasks;
    using Pipeline.AsyncPipeline;
    using Pipeline.EmptyPipeline;
    using Pipeline.MultiplePipelines;
    using Pipeline.SyncPipeline;
    using Query.Handlers;
    using SomeOtherMessageSubscriberAsyncThatThrows = Message.Subscribers.Sync.SomeOtherMessageSubscriberAsyncThatThrows;

    public static class FakeIoCSetup
    {
        public static IBroker GetBroker()
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
                new MessageChildSubscriberAsync(),
                new MessageParentSubscriberAsync(),
                new SomeOtherMessageSubscriberAsyncThatThrows(),
                new TestMessageSubscriberAsyncFoo(),
                new TestMessageSubscriberAsyncBar(),
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
                new EmptyPipeline(),
                new EmptyPipelineAsync()
            };

            return new Broker(handlers, subscribers, pipelineTasks, pipelines);
        }
    }
}
