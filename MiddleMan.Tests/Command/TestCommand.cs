namespace MiddleMan.Tests.Command
{
    using System;
    using Interfaces.Command;

    public class TestCommand : ICommand
    {
        public Func<string> Callback { get; set; }
    }
}
