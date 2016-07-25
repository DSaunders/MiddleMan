namespace MiddleMan.Tests.Fakes.Query
{
    using MiddleMan.Query;

    public class TestQueryAsync : IQuery<string>
    {
        public string Ping { get; set; }
    }
}