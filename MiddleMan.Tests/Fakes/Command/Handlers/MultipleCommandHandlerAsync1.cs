namespace MiddleMan.Tests.Fakes.Command.Handlers
{
    using System.Threading.Tasks;
    using MiddleMan.Command;

    public class MultipleCommandHandlerAsync1 : ICommandHandlerAsync<MultipleHandlerCommandAsync>
    {
        public Task HandleCommandAsync(MultipleHandlerCommandAsync command)
        {
            return Task.FromResult(true);
        }
    }
}

