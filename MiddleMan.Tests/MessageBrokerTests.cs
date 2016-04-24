namespace MiddleMan.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Command;
    using Command.Handlers;
    using Message;
    using MiddleMan;
    using Exceptions;
    using Interfaces;
    using Interfaces.Message;
    using Query;
    using Query.Handlers;
    using Should;
    using Xunit;

    public class MessageBrokerTests
    {
        private readonly IMessageBroker _broker;

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
            _broker = new MessageBroker(handlers);
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
        public void Dispatches_Messages_To_Subscribers()
        {
            // Arrange
            var message = new TestMessage("Hello, World!");

            var subscriber1Message = string.Empty;
            _broker.SubscribeToMessage<TestMessage>(m =>
            {
                subscriber1Message = m.MessageText;
            });

            var subscriber2Message = string.Empty;
            _broker.SubscribeToMessage<TestMessage>(m =>
            {
                subscriber2Message = m.MessageText;
            });

            // Act
            _broker.SendMessage(message);

            // Assert
            subscriber1Message.ShouldEqual("Hello, World!");
            subscriber2Message.ShouldEqual("Hello, World!");
        }

        [Fact]
        public void Does_Not_Dispatch_To_Other_Subscribers()
        {
            // Arrange
            var message = new TestMessage("Hello, World!");
            
            var subscriber1Message = string.Empty;
            _broker.SubscribeToMessage<TestMessage>(m =>
            {
                subscriber1Message = m.MessageText;
            });

            var subscriber2Message = string.Empty;
            _broker.SubscribeToMessage<MessageChild>(m =>
            {
                subscriber2Message = m.MessageText;
            });

            // Act
            _broker.SendMessage(message);

            // Assert
            subscriber1Message.ShouldEqual("Hello, World!");
            subscriber2Message.ShouldBeEmpty();
        }

        [Fact]
        public void Dispatches_Messages_To_Subscribers_Who_Subscribe_To_Ancestor_MessageType()
        {
            // Arrange
            var message = new MessageChild("Child Message");

            var parentMessage = string.Empty;
            _broker.SubscribeToMessage<MessageParent>(m =>
            {
                parentMessage = m.MessageText;
            });

            var childMessage = string.Empty;
            _broker.SubscribeToMessage<MessageChild>(m =>
            {
                childMessage = m.MessageText;
            });

            // Act
            _broker.SendMessage(message);

            // Assert
            parentMessage.ShouldEqual("Child Message");
            childMessage.ShouldEqual("Child Message");
        }

        [Fact]
        public void Does_Not_Dispatch_Messages_To_Subscribers_Who_Subscribe_To_Derived_MessageTypes()
        {
            // Arrange
            var message = new MessageParent("Parent Message");

            var parentMessage = string.Empty;
            _broker.SubscribeToMessage<MessageParent>(m =>
            {
                parentMessage = m.MessageText;
            });

            var childMessage = string.Empty;
            _broker.SubscribeToMessage<MessageChild>(m =>
            {
                childMessage = m.MessageText;
            });

            // Act
            _broker.SendMessage(message);

            // Assert
            parentMessage.ShouldEqual("Parent Message");
            childMessage.ShouldBeEmpty();
        }

        [Fact]
        public void Allows_Subscription_To_All_Messages()
        {
            // Arrange
            var testMessage = new TestMessage("Hello, World!");
            var childMessage = new MessageChild("Hello, World!");

            var messagesRecieved = new List<IMessage>();
            _broker.SubscribeToAllMessages(m =>
            {
                messagesRecieved.Add(m);
            });
            
            // Act
            _broker.SendMessage(testMessage);
            _broker.SendMessage(childMessage);

            // Assert
            messagesRecieved.ShouldContain(testMessage);
            messagesRecieved.ShouldContain(childMessage);
            messagesRecieved.Count.ShouldEqual(2);
        }
    }
}
