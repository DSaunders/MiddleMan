namespace MiddleMan.Tests.Fakes.Pipeline
{
    using System.Collections.Generic;
    using MiddleMan.Pipeline;

    public class FooBarPipelineMessage : IPipelineMessage
    {
        public IList<string> TasksRun { get; set; }

        public FooBarPipelineMessage()
        {
            TasksRun = new List<string>();
        }
    }
}
