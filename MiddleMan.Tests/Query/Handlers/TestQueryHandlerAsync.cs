namespace MiddleMan.Tests.Query.Handlers
{
    using System.Threading.Tasks;
    using MiddleMan.Query;

    public class TestQueryHandlerAsync : IQueryHandlerAsync<TestQueryAsync, string>
    {
        public Task<string> HandleQueryAsync(TestQueryAsync query)
        {
            return Task.FromResult(query.Ping + "-pong");
        }
    }
}