namespace MiddleMan.Tests.Command
{
    using Interfaces.Command;

    public class TestAsyncCommand : ICommand
    {
        public bool HasBeenCalled { get; set; }
    }
}