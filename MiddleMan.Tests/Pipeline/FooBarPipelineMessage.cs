namespace MiddleMan.Tests.Pipeline
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
