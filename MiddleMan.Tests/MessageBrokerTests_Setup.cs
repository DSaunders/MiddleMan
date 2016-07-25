namespace MiddleMan.Tests
{
    using System.Collections.Generic;
    using Command.Handlers;
    using MiddleMan;
    using Message.Subscriber;
    using MiddleMan.Message;
    using MiddleMan.Pipeline;
    using Pipeline;
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

            var pipelineTasks = new List<IPipelineTask>
            {
                new PipelineTaskFoo(),
                new PipelineTaskBar()
            };

            var subscribers = new List<IMessageSubscriber>
            {
                new MessageChildSubscriber(),
                new MessageParentSubscriber(),
                new SomeOtherMessageSubscriberThatThrows(),
                new TestMessageSubscriberFoo(),
                new TestMessageSubscriberBar()
            };

            _broker = new Broker(handlers, subscribers, pipelineTasks);
        }
    }
}
