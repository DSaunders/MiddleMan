namespace MiddleMan.Command
{
    public interface ICommandHandler<in TCommand> : IHandler
            where TCommand : ICommand
    {
        void HandleCommand(TCommand command);
    }
}