﻿namespace MiddleMan.Tests.Fakes.Command
{
    using System;
    using MiddleMan.Command;

    public class TestCommand : ICommand
    {
        public Func<string> Callback { get; set; }
    }
}
