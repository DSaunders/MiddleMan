namespace MiddleMan.Tests.Fakes.Command
{
    using MiddleMan.Command;

    public class TestAsyncCommand : ICommand
    {
        public bool HasBeenCalled { get; set; }
    }
}