namespace MiddleMan.Tests.Query
{
    using MiddleMan.Interfaces.Query;

    public class TestQuery : IQuery<string>
    {
        public string Ping { get; set; }
    }
}
