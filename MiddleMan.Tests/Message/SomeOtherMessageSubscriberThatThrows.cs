namespace MiddleMan.Tests.Message
{
    using System;
    using MiddleMan.Message;

    public class SomeOtherMessageSubscriberThatThrows : IMessageSubscriber<SomeOtherMessage>
    {
        public void OnMessageReceived(SomeOtherMessage message)
        {
            throw new Exception("I should not have been called!");
        }
    }
}