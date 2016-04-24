namespace MiddleMan.Tests.Query.Handlers
{
    using MiddleMan.Interfaces.Query;

    public class MultipleHandler1 : IQueryHandler<MultipleHandlerQuery, string>
    {
        public string HandleQuery(MultipleHandlerQuery query)
        {
            return string.Empty;
        }
    }
}