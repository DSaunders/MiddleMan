namespace MiddleMan.Tests.Command.Handlers
{
    using MiddleMan.Command;

    public class MultipleCommandHandler1 : ICommandHandler<MultipleHandlerCommand>
    {
        public void HandleCommand(MultipleHandlerCommand command)
        {
        }
    }
}