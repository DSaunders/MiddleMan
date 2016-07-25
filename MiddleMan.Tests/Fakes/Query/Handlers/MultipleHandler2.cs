namespace MiddleMan.Tests.Fakes.Query.Handlers
{
    using MiddleMan.Query;

    public class MultipleHandler2 : IQueryHandler<MultipleHandlerQuery, string>
    {
        public string HandleQuery(MultipleHandlerQuery query)
        {
            return string.Empty;
        }
    }
}