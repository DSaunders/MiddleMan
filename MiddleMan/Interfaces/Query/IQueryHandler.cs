namespace MiddleMan.Interfaces.Query
{
    public interface IQueryHandler<in TQuery, out TResponse> : IHandler
        where TQuery : IQuery<TResponse>
    {
        TResponse HandleQuery(TQuery command);
    }
}