namespace MiddleMan.Tests.Query.Handlers
{
    using MiddleMan.Interfaces.Query;

    public class MultipleHandler2 : IQueryHandler<MultipleHandlerQuery, string>
    {
        public string HandleQuery(MultipleHandlerQuery query)
        {
            return string.Empty;
        }
    }
}