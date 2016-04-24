namespace MiddleMan.Tests.Command.Handlers
{
    using MiddleMan.Interfaces.Command;

    public class MultipleCommandHandler2 : ICommandHandler<MultipleHandlerCommand>
    {
        public void HandleCommand(MultipleHandlerCommand command)
        {
        }
    }
}