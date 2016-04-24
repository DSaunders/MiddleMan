namespace MiddleMan.Tests.Query.Handlers
{
    using MiddleMan.Interfaces.Query;

    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public string HandleQuery(TestQuery query)
        {
            return query.Ping + "-pong";
        }
    }
}
