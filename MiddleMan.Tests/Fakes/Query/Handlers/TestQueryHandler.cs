﻿namespace MiddleMan.Tests.Fakes.Query.Handlers
{
    using MiddleMan.Query;

    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public string HandleQuery(TestQuery query)
        {
            return query.Ping + "-pong";
        }
    }
}
