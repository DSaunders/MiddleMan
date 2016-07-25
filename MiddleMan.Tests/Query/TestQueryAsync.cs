namespace MiddleMan.Tests.Query
{
    using MiddleMan.Query;

    public class TestQueryAsync : IQuery<string>
    {
        public string Ping { get; set; }
    }
}