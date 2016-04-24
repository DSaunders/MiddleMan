﻿namespace MiddleMan.Tests.Query.Handlers
{
    using System.Threading.Tasks;
    using Interfaces.Query;

    public class MultipleHandlerAsync1 : IQueryHandlerAsync<MultipleHandlerAsyncQuery, string>
    {
        public Task<string> HandleQueryAsync(MultipleHandlerAsyncQuery query)
        {
            return Task.FromResult(string.Empty);
        }
    }
}