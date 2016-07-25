namespace MiddleMan.Tests.Command.Handlers
{
    using MiddleMan.Command;

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public void HandleCommand(TestCommand command)
        {
            command.Callback();
        }
    }
}