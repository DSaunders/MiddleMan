namespace MiddleMan.Query
{
    using System.Threading.Tasks;

    public interface IQueryHandlerAsync<in TQuery, TResponse> : IHandler
        where TQuery : IQuery<TResponse>
    {
        Task<TResponse> HandleQueryAsync(TQuery query);
    }
}