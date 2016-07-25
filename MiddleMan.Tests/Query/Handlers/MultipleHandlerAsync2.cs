namespace MiddleMan.Tests.Query.Handlers
{
    using System.Threading.Tasks;
    using MiddleMan.Query;

    public class MultipleHandlerAsync2 : IQueryHandlerAsync<MultipleHandlerAsyncQuery, string>
    {
        public Task<string> HandleQueryAsync(MultipleHandlerAsyncQuery query)
        {
            return Task.FromResult(string.Empty);
        }
    }
}