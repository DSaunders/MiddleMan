namespace MiddleMan.Tests.Fakes.Message.Subscribers.Async
{
    using System;
    using System.Threading.Tasks;
    using Messages;
    using MiddleMan.Message;

    public class SomeOtherMessageSubscriberThatThrows : IMessageSubscriberAsync<SomeOtherMessage>
    {
        public async Task OnMessageReceived(SomeOtherMessage message)
        {
            throw new Exception("I should not have been called!");
        }
    }
}