namespace MiddleMan.Tests
{
    using System.Threading.Tasks;
    using Exceptions;
    using Fakes;
    using Fakes.Query;
    using Should;
    using Xunit;

    public class QueriesTests
    {
        private readonly IBroker _broker;

        public QueriesTests()
        {
            _broker = FakeIoCSetup.GetBroker();
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
    }
}
