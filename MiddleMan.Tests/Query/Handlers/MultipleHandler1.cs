namespace MiddleMan.Tests.Query.Handlers
{
    using MiddleMan.Query;

    public class MultipleHandler1 : IQueryHandler<MultipleHandlerQuery, string>
    {
        public string HandleQuery(MultipleHandlerQuery query)
        {
            return string.Empty;
        }
    }
}