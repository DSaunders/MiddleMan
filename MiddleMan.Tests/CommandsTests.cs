namespace MiddleMan.Tests
{
    using System.Threading.Tasks;
    using Exceptions;
    using Fakes;
    using Fakes.Command;
    using Should;
    using Xunit;

    public class CommandsTests
    {
        private readonly IBroker _broker;

        public CommandsTests()
        {
            _broker = FakeIoCSetup.GetBroker();
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
    }
}
