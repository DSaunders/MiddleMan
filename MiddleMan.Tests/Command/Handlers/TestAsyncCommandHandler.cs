namespace MiddleMan.Tests.Command.Handlers
{
    using System.Threading.Tasks;
    using MiddleMan.Command;

    public class AsyncCommandHandler : ICommandHandlerAsync<TestAsyncCommand>
    {
        public Task HandleCommandAsync(TestAsyncCommand command)
        {
            return Task.FromResult(command.HasBeenCalled = true);
        }
    }
}