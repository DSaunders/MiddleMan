namespace MiddleMan.Tests.Message
{
    using System;
    using System.Threading.Tasks;
    using MiddleMan.Message;

    public class SomeOtherMessageSubscriberThatThrows : IMessageSubscriber<SomeOtherMessage>
    {
        public async Task OnMessageReceived(SomeOtherMessage message)
        {
            throw new Exception("I should not have been called!");
        }
    }
}