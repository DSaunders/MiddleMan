namespace MiddleMan.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fakes;
    using Fakes.Message.Messages;
    using Should;
    using Xunit;

    public class MessageTests
    {
        private readonly IBroker _broker;

        public MessageTests()
        {
            _broker = FakeIoCSetup.GetBroker();
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
        
    }
}
