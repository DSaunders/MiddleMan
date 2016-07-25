namespace MiddleMan.Command
{
    using System.Threading.Tasks;

    public interface ICommandHandlerAsync<in TCommand> : IHandler
    {
        Task HandleCommandAsync(TCommand command);
    }
}