namespace MiddleMan.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Command;
    using Command.Handlers;
    using Message;
    using MiddleMan;
    using Exceptions;
    using MiddleMan.Message;
    using MiddleMan.Pipeline;
    using Pipeline;
    using Query;
    using Query.Handlers;
    using Should;
    using Xunit;

    public class MessageBrokerTests
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

            var subscibers = new List<IMessageSubscriber>
            {
                new MessageChildSubscriber(),
                new MessageParentSubscriber(),
                new SomeOtherMessageSubscriberThatThrows(),
                new TestMessageSubscriberFoo(),
                new TestMessageSubscriberBar()
            };

            _broker = new Broker(handlers, subscibers, pipelineTasks);
        }

        [Fact]
        public void Finds_And_Executes_Handler_For_Query()
        {
            // Given
            var query = new TestQuery {Ping = "pingMessage"};

            // When
            var result = _broker.ProcessQuery(query);

            // Then
            result.ShouldEqual("pingMessage-pong");
        }

        [Fact]
        public async Task Finds_And_Executes_Asyc_Handler_For_Query()
        {
            // Given
            var query = new TestQueryAsync { Ping = "pingMessage" };

            // When
            var result = await _broker.ProcessQueryAsync(query);

            // Then
            result.ShouldEqual("pingMessage-pong");
        }


        [Fact]
        public void Throws_When_No_Query_Handler()
        {
            // Given
            var query = new NoHandlerQuery();

            // When
            var ex = Record.Exception(() => _broker.ProcessQuery(query));

            // Then
            ex.Message.ShouldEqual("No QueryHandler found for NoHandlerQuery");
            ex.ShouldBeType<NoHandlerException>();
        }

        [Fact]
        public async Task Throws_When_No_Async_Query_Handler()
        {
            // Given
            var query = new NoHandlerQueryAsync();

            // When
            var ex = await Record.ExceptionAsync(() => _broker.ProcessQueryAsync(query));

            // Then
            ex.Message.ShouldEqual("No Async QueryHandler found for NoHandlerQueryAsync");
            ex.ShouldBeType<NoHandlerException>();
        }

        [Fact]
        public void Throws_When_Multiple_Query_Handlers()
        {
            // Given
            var query = new MultipleHandlerQuery();

            // When
            var ex = Record.Exception(() => _broker.ProcessQuery(query));

            // Then
            ex.Message.ShouldEqual("2 QueryHandlers found for MultipleHandlerQuery");
            ex.ShouldBeType<MultipleHandlersException>();
        }

        [Fact]
        public async Task Throws_When_Multiple_Async_Query_Handlers()
        {
            // Given
            var query = new MultipleHandlerAsyncQuery();

            // When
            var ex = await Record.ExceptionAsync(() => _broker.ProcessQueryAsync(query));

            // Then
            ex.Message.ShouldEqual("2 Async QueryHandlers found for MultipleHandlerAsyncQuery");
            ex.ShouldBeType<MultipleHandlersException>();
        }

        [Fact]
        public void Finds_And_Executes_Handler_For_Command()
        {
            // Given
            var result = string.Empty;
            var command = new TestCommand
            {
                Callback = () => result = "Called OK"
            };

            // When
            _broker.ProcessCommand(command);

            // Then
            result.ShouldEqual("Called OK");
        }

        [Fact]
        public async Task Finds_And_Executed_Async_Handler_For_Command()
        {
            // Given
            var command = new TestAsyncCommand
            {
                HasBeenCalled = false
            };

            // When
            await _broker.ProcessCommandAsync(command);

            // Then
            command.HasBeenCalled.ShouldBeTrue();
        }   

        [Fact]
        public void Throws_When_No_Command_Handler()
        {
            // Given
            var command = new NoHandlerCommand();

            // When
            var ex = Record.Exception(() => _broker.ProcessCommand(command));

            // Then
            ex.Message.ShouldEqual("No CommandHandler found for NoHandlerCommand");
            ex.ShouldBeType<NoHandlerException>();
        }

        [Fact]
        public async Task Throws_When_No_Async_Command_Handler()
        {
            // Given
            var command = new NoHandlerCommandAsync();

            // When
            var ex = await Record.ExceptionAsync(() => _broker.ProcessCommandAsync(command));

            // Then
            ex.Message.ShouldEqual("No Async CommandHandler found for NoHandlerCommandAsync");
            ex.ShouldBeType<NoHandlerException>();
        }

        [Fact]
        public void Throws_When_Multiple_Commands_Handlers()
        {
            // Given
            var command = new MultipleHandlerCommand();

            // When
            var ex = Record.Exception(() => _broker.ProcessCommand(command));

            // Then
            ex.Message.ShouldEqual("2 CommandHandlers found for MultipleHandlerCommand");
            ex.ShouldBeType<MultipleHandlersException>();
        }

        [Fact]
        public async Task Throws_When_Multiple_Async_Commands_Handlers()
        {
            // Given
            var command = new MultipleHandlerCommandAsync();

            // When
            var ex = await Record.ExceptionAsync(() => _broker.ProcessCommandAsync(command));

            // Then
            ex.Message.ShouldEqual("2 Async CommandHandlers found for MultipleHandlerCommandAsync");
            ex.ShouldBeType<MultipleHandlersException>();
        }

        [Fact]
        public async Task Dispatches_Messages_To_Subscribers()
        {
            // Arrange
            var subscribersCalled = new List<string>();
            var message = new TestMessage("Hello, World!", subscribersCalled);

            // Act
            await _broker.SendMessageAsync(message);

            // Assert
            subscribersCalled.Count.ShouldEqual(2);
            subscribersCalled.ShouldContain("Foo");
            subscribersCalled.ShouldContain("Bar");
        }

        [Fact]
        public async Task Does_Not_Dispatch_To_Other_Subscribers()
        {
            // Arrange
            var message = new TestMessage("Hello, World!", new List<string>());

            // Act
            await _broker.SendMessageAsync(message);

            // Assert
            // If all subscribers are called, one of them throws an exception so this test will fail
        }

        [Fact]
        public async Task Dispatches_Messages_To_Subscribers_Who_Subscribe_To_Ancestor_MessageType()
        {
            // Arrange
            var subscribersHit = new List<string>();
            var message = new MessageChild("Child Message", subscribersHit);
            
            // Act
            await _broker.SendMessageAsync(message);

            // Assert
            subscribersHit.Count.ShouldEqual(2);
            subscribersHit.ShouldContain("MessageParent");
            subscribersHit.ShouldContain("MessageChild");
        }

        [Fact]
        public async Task Does_Not_Dispatch_Messages_To_Subscribers_Who_Subscribe_To_Derived_MessageTypes()
        {
            // Arrange
            var subscribersHit = new List<string>();
            var message = new MessageParent("Parent Message", subscribersHit);

            // Act
            await _broker.SendMessageAsync(message);

            // Assert
            subscribersHit.Count.ShouldEqual(1);
            subscribersHit.ShouldContain("MessageParent");
        }

        [Fact]
        public async void Runs_Pipeline_In_Order()
        {
            // Given
            var message = new PipelineMessage();
            _broker.ConstructPipeline<PipelineMessage>(p =>
            {
                p.Add<PipelineTaskFoo>();
                p.Add<PipelineTaskBar>();
            });

            // When
            await _broker.RunPipelineAsync(message);

            // Then
            message.TasksRun[0].ShouldEqual("Foo");
            message.TasksRun[1].ShouldEqual("Bar");
            message.TasksRun.Count.ShouldEqual(2);
        }

        [Fact]
        public void Throws_When_A_Pipeline_Is_Already_Registered_For_A_Message()
        {
            // Given
            _broker.ConstructPipeline<PipelineMessage>(p =>
            {
                p.Add<PipelineTaskFoo>();
                p.Add<PipelineTaskBar>();
            });
            
            // When
            var ex = Record.Exception(() => _broker.ConstructPipeline<PipelineMessage>(p => { }));

            // Then
            ex.Message.ShouldEqual("A pipeline already exists to handle this PipelineMessage type");
            ex.ShouldBeType<MultiplePipelinesException>();
        }
    }
}
