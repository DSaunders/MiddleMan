namespace MiddleMan.Tests.Message.Messages
{
    using System.Collections.Generic;
    using MiddleMan.Message;

    public class TestMessage : IMessage
    {
        public string MessageText { get; }
        public List<string> Subscribers { get; }

        public TestMessage(string message, List<string> subscribers)
        {
            Subscribers = subscribers;
            MessageText = message;
        }
    }
}
