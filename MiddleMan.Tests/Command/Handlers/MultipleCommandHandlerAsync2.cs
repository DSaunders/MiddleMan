namespace MiddleMan.Tests.Command.Handlers
{
    using System.Threading.Tasks;
    using Interfaces.Command;

    public class MultipleCommandHandlerAsync2 : ICommandHandlerAsync<MultipleHandlerCommandAsync>
    {
        public Task HandleCommandAsync(MultipleHandlerCommandAsync command)
        {
            return Task.FromResult(true);
        }
    }
}