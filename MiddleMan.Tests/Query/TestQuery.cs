namespace MiddleMan.Tests.Query
{
    using MiddleMan.Query;

    public class TestQuery : IQuery<string>
    {
        public string Ping { get; set; }
    }
}
