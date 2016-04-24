namespace MiddleMan.Tests.Message
{
    using MiddleMan.Interfaces.Message;

    public class MessageParent : IMessage
    {
        public string MessageText { get; set; }

        public MessageParent(string message)
        {
            MessageText = message;
        }
    }
}