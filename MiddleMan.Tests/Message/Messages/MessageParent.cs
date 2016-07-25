namespace MiddleMan.Tests.Message.Messages
{
    using System.Collections.Generic;
    using MiddleMan.Message;

    public class MessageParent : IMessage
    {
        public string MessageText { get; }
        public List<string> Subscribers { get; }

        public MessageParent(string message, List<string> subscribers)
        {
            Subscribers = subscribers;
            MessageText = message;
        }
    }
}