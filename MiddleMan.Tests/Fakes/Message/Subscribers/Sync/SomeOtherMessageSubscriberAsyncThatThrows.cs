namespace MiddleMan.Tests.Fakes.Message.Subscribers.Sync
{
    using System;
    using Messages;
    using MiddleMan.Message;

    public class SomeOtherMessageSubscriberAsyncThatThrows : IMessageSubscriber<SomeOtherMessage>
    {
        public void OnMessageReceived(SomeOtherMessage message)
        {
            throw new Exception("I should not have been called!");
        }
    }
}