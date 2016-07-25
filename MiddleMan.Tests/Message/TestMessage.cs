namespace MiddleMan.Tests.Message
{
    using MiddleMan.Message;

    public class TestMessage : IMessage
    {
        public string MessageText { get; }

        public TestMessage(string message)
        {
            MessageText = message;
        }
    }
}
