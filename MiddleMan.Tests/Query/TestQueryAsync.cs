namespace MiddleMan.Tests.Query
{
    using Interfaces.Query;

    public class TestQueryAsync : IQuery<string>
    {
        public string Ping { get; set; }
    }
}