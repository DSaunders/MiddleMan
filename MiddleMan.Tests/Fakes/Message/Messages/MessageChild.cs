namespace MiddleMan.Tests.Fakes.Message.Messages
{
    using System.Collections.Generic;

    public class MessageChild : MessageParent
    {
        public MessageChild(string message, List<string> subscribers) : base(message, subscribers)
        {
        }
    }
}